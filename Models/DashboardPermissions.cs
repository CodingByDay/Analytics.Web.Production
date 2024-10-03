using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Dash.Models
{
    public class DashboardPermissions
    {
        [JsonIgnore]
        public string Connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        public List<DashboardPermission> Permissions { get; set; } = new List<DashboardPermission>();

        public string ConvertPermissionsToJson(DashboardPermissions permissions)
        {
            return JsonConvert.SerializeObject(permissions);
        }

        public DashboardPermissions(string uname)
        {
            this.Permissions = GetPermissionsForUser(uname).Permissions;
        }


        public DashboardPermissions(int group)
        {
            if(group == -1)
            {
                this.Permissions = new List<DashboardPermission>();
            }
            this.Permissions = GetPermissionsForGroup(group).Permissions;
        }

        public DashboardPermissions()
        {
        }

        public DashboardPermissions GetPermissionsForGroup(int group)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT group_permissions FROM groups WHERE group_id = @group_id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add the parameter properly
                        cmd.Parameters.Add("@group_id", SqlDbType.VarChar, 25).Value = group;
                        // Execute the query and get the result
                        string result = cmd.ExecuteScalar() as string;
                        if (string.IsNullOrEmpty(result))
                        {
                            // If the result is null or empty, return an empty DashboardPermissions object
                            return new DashboardPermissions();
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
                    return new DashboardPermissions();
                }
            }
        }







        public DashboardPermissions GetPermissionsForUser(string uname)
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
                            return new DashboardPermissions();
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
                    return new DashboardPermissions();
                }
            }
        }


        public bool SetPermissionsForGroup(int group)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    // Convert permissions to JSON
                    string jsonValue = this.ConvertPermissionsToJson(this);
                    conn.Open();

                    // Use parameterized query to avoid SQL injection
                    string query = "UPDATE groups SET group_permissions = @permissions WHERE group_id = @group_id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Set parameter for JSON permissions (NVarChar(MAX))
                        cmd.Parameters.Add("@permissions", SqlDbType.NVarChar).Value = jsonValue;

                        // Set parameter for username
                        cmd.Parameters.Add("@group_id", SqlDbType.VarChar, 25).Value = group;

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
                catch (Exception ex)
                {
                    // Log the error (optional)
                    // Handle the exception properly or return false in case of failure
                    return false;
                }
            }
        }

        public bool SetPermissionsForUser(string uname)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    // Convert permissions to JSON
                    string jsonValue = this.ConvertPermissionsToJson(this);
                    conn.Open();

                    // Use parameterized query to avoid SQL injection
                    string query = "UPDATE users SET dashboard_permissions = @permissions WHERE uname = @uname";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Set parameter for JSON permissions (NVarChar(MAX))
                        cmd.Parameters.Add("@permissions", SqlDbType.NVarChar).Value = jsonValue;

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
                catch (Exception ex)
                {
                    // Log the error (optional)
                    // Handle the exception properly or return false in case of failure
                    return false;
                }
            }
        }

        public DashboardPermissions ConvertJsonToPermissions(string json)
        {
            string unescapedJson = Regex.Unescape(json);
            var permissions = JsonConvert.DeserializeObject<DashboardPermissions>(unescapedJson);
            return permissions;
        }

        public bool DashboardWithIdAllowed(string dashboardID)
        {
            foreach (var permission in this.Permissions)
            {
                if (permission.id.ToString() == dashboardID)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class DashboardPermission
    {
        public int id { get; set; }
    }
}