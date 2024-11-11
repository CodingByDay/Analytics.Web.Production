using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Dash.Models
{
    public class DashboardFilters
    {
        [JsonIgnore]
        public string Connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
        [JsonIgnore]
        public string uname { get; set; }

        public List<DashboardFilter> Filters { get; set; } = new List<DashboardFilter>();


        public DashboardFilters(string username)
        {
            this.uname = username;
            this.Filters = GetFiltersForUser(uname).Filters;
        }

        public DashboardFilters()
        {
            
        }

        public DashboardFilters GetFiltersForUser(string uname)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT dashboard_filters FROM users WHERE uname = @uname";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add the parameter properly
                        cmd.Parameters.Add("@uname", SqlDbType.VarChar, 25).Value = uname;
                        // Execute the query and get the result
                        string result = cmd.ExecuteScalar() as string;
                        if (string.IsNullOrEmpty(result))
                        {
                            // If the result is null or empty, return an empty DashboardPermissions object
                            return new DashboardFilters();
                        }
                        // Deserialize the JSON string back into the object
                        var permissions = ConvertJsonToPermissions(result);
                        // Return the permissions object
                        return permissions;
                    }
                }
                catch (Exception)
                {
                    // In case of error, return a new (empty) DashboardPermissions object 20.09.2024 Janko Jovičić
                    return new DashboardFilters();
                }
            }
        }
        public string ConvertFiltersToJson(DashboardFilters permissions)
        {
            return JsonConvert.SerializeObject(permissions);
        }

        public bool SetFiltersForUser()
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    // Convert permissions to JSON
                    string jsonValue = this.ConvertFiltersToJson(this);
                    conn.Open();

                    // Use parameterized query to avoid SQL injection
                    string query = "UPDATE users SET dashboard_filters = @filters WHERE uname = @uname";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Set parameter for JSON permissions (NVarChar(MAX))
                        cmd.Parameters.Add("@filters", SqlDbType.NVarChar).Value = jsonValue;

                        // Set parameter for username
                        cmd.Parameters.Add("@uname", SqlDbType.VarChar, 25).Value = uname;

                        // Execute the query (ExecuteNonQuery is more appropriate since you are doing an UPDATE)
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were updated
                        if (rowsAffected > 0)
                        {
                            return true;  // Permissions updated successfully
                        }
                        return false; // No rows affected, return false
                    }
                }
                catch (Exception)
                {
                    // Log the error (optional)
                    // Handle the exception properly or return false in case of failure
                    return false;
                }
            }
        }
        public DashboardFilters ConvertJsonToPermissions(string json)
        {
            string unescapedJson = Regex.Unescape(json);
            var permissions = JsonConvert.DeserializeObject<DashboardFilters>(unescapedJson);
            return permissions;
        }

        public void FilterChanged(DashboardFilter filterChange)
        {
            try
            {

                if (this.Filters.Any(f => f.ItemName == filterChange.ItemName))
                {
                    if (this.Filters.Where(f => 
                    
                    f.ItemName == filterChange.ItemName
                    
                    &&
                    
                    f.Dashboard == filterChange.Dashboard
                    
                    ).ToList().Count > 1)
                    {
                        return;
                    }
                    var found = this.Filters.Where(f => f.ItemName == filterChange.ItemName).FirstOrDefault();
                    this.Filters.ElementAt(this.Filters.IndexOf(found)).Values = filterChange.Values;
                }
                else
                {
                    this.Filters.Add(filterChange);
                }

                SetFiltersForUser();
            }
            catch (Exception)
            {
                return;
            }

        }

        public class FilterSelection
        {
            public int Index { get; set; }
            public List<string> Values { get; set; }
        }

        public class DashboardFilter
        {
            [JsonProperty("Dashboard")]
            public int Dashboard { get; set; }

            [JsonProperty("ItemName")]
            public string ItemName { get; set; }

            [JsonProperty("Values")]
            public List<FilterSelection> Values { get; set; }
        }
    }
}