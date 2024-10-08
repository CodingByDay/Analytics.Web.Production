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
        public List<DashboardFilter> Filters { get; set; } = new List<DashboardFilter>();


        public DashboardFilters(string uname)
        {
            this.Filters = GetFiltersForUser(uname).Filters;

        }

        public DashboardFilters()
        {
            string json = JsonConvert.SerializeObject(this);
            var debug = true;
        }

        public DashboardFilters GetFiltersForUser(string uname)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT dashboard_permissions FROM users WHERE uname = @uname";
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

        public DashboardFilters ConvertJsonToPermissions(string json)
        {
            string unescapedJson = Regex.Unescape(json);
            var permissions = JsonConvert.DeserializeObject<DashboardFilters>(unescapedJson);
            return permissions;
        }


    }




    public class DashboardFilter
    {
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        [JsonProperty("Values")]
        public List<List<string>> Values { get; set; }
    }
}