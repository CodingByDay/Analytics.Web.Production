using DevExpress.DashboardCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Dash.Models
{
    public class UserDashboardState
    {
        [JsonIgnore]
        public string Connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        [JsonIgnore]
        public string uname { get; set; }

        public UserDashboardState()
        {
        }

        public UserDashboardState(string username)
        {
            this.uname = username;
            this.States = GetStatesForUser(uname).States;
        }

        public UserDashboardState GetStatesForUser(string uname)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT dashboard_states FROM users WHERE uname = @uname";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add the parameter properly
                        cmd.Parameters.Add("@uname", SqlDbType.VarChar, 25).Value = uname;
                        // Execute the query and get the result
                        string result = cmd.ExecuteScalar() as string;
                        if (string.IsNullOrEmpty(result))
                        {
                            // If the result is null or empty, return an empty DashboardPermissions object
                            return new UserDashboardState();
                        }
                        // Deserialize the JSON string back into the object
                        var states = ConvertJsonToStates(result);
                        // Return the states object
                        return states;
                    }
                }
                catch (Exception)
                {
                    // In case of error, return a new (empty) DashboardPermissions object 20.09.2024 Janko Jovičić
                    return new UserDashboardState();
                }
            }
        }

        public UserDashboardState ConvertJsonToStates(string json)
        {
            string unescapedJson = Regex.Unescape(json);
            var states = JsonConvert.DeserializeObject<UserDashboardState>(unescapedJson);
            return states;
        }

        public DashboardStateSingle GetInitialStateForTheUser(string dashboardId)
        {
            UserDashboardState userDashboardStates = new UserDashboardState(HttpContext.Current.User.Identity.Name);
            foreach (var userDashboardState in userDashboardStates.States)
            {
                if (userDashboardState.DashboardId.ToString() == dashboardId)
                {
                    return userDashboardState;
                }
            }
            return new DashboardStateSingle();
        }

        public string ConvertStatesToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public bool SetStatesForUser()
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    // Convert permissions to JSON
                    string jsonValue = ConvertStatesToJson();
                    conn.Open();

                    // Use parameterized query to avoid SQL injection
                    string query = "UPDATE users SET dashboard_states = @states WHERE uname = @uname";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Set parameter for JSON permissions (NVarChar(MAX))
                        cmd.Parameters.Add("@states", SqlDbType.NVarChar).Value = jsonValue;

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

        public void UpdateStates(string dashboardId, DashboardState stateObject)
        {
            // Find if the dashboard state already exists in the list
            var existingState = States.FirstOrDefault(s => s.DashboardId == dashboardId);

            if (existingState != null)
            {
                // If it exists, update the existing state
                existingState.State = stateObject;
            }
            else
            {
                // If it doesn't exist, add a new entry to the list
                States.Add(new DashboardStateSingle
                {
                    DashboardId = dashboardId,
                    State = stateObject
                });
            }

            // After updating or inserting, save the updated states to the database
            SetStatesForUser();
        }

        public List<DashboardStateSingle> States { get; set; } = new List<DashboardStateSingle>();
    }

    public class DashboardStateSingle
    {
        public string DashboardId { get; set; } = string.Empty;
        public DashboardState State { get; set; } = new DashboardState();
    }
}