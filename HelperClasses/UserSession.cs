using Newtonsoft.Json;
using Sentry;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace Dash.HelperClasses
{
    public static class UserSession
    {
        public static string Uname
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        public static string GraphsConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            }
        }

        private class SessionVariable
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        // Retrieves a session variable for the current user by name
        public static T GetSessionVariable<T>(string name)
        {
            try
            {
                string query = "SELECT variable_value FROM [session_user] WHERE uname = @uname AND variable_name = @name";

                using (SqlConnection conn = new SqlConnection(GraphsConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@uname", Uname);
                    cmd.Parameters.AddWithValue("@name", name);
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        // Deserialize the JSON string to the desired type
                        return JsonConvert.DeserializeObject<T>(result.ToString());
                    }

                    return default(T);  // Return default value if no result is found
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return default(T);
            }
        }

        // Sets or updates a session variable for the current user by serializing the value to JSON
        public static void SetSessionVariable(string name, object value)
        {
            try
            {
                // Serialize the value to a JSON string
                string serializedValue = value != null ? JsonConvert.SerializeObject(value) : null;

                string query = @"
                IF EXISTS (SELECT 1 FROM [session_user] WHERE uname = @uname AND variable_name = @name)
                    UPDATE [session_user] SET variable_value = @value WHERE uname = @uname AND variable_name = @name;
                ELSE
                    INSERT INTO [session_user] (uname, variable_name, variable_value) VALUES (@uname, @name, @value);";

                try
                {
                    using (SqlConnection conn = new SqlConnection(GraphsConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@uname", Uname);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@value", serializedValue != null ? serializedValue : string.Empty);
                        conn.Open();
                        var result = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        public static void ClearUserCache(string name)
        {
            string query = "DELETE FROM [session_user] WHERE uname = @uname";

            using (SqlConnection connection = new SqlConnection(GraphsConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@uname", name);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}