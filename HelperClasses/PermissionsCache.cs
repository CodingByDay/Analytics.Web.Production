using Dash.Models;
using Sentry;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Dash.HelperClasses
{
    public static class PermissionsCache
    {
        public static string GraphsConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            }
        }

        public static DashboardPermissions GetPermissions(string user)
        {
            // Retrieve the "IsEditCompany" session variable from the database
            DashboardPermissions permissions = UserSession.GetSessionVariable<DashboardPermissions>("IsEditCompany");

            // If the value is null (not set), default to false and store it in the database
            if (permissions == null)
            {
                permissions = new DashboardPermissions(GetGroupForUser(user));
                UserSession.SetSessionVariable("Permissions", permissions);
            }

            return permissions;
        }

        private static int GetGroupForUser(string uname)
        {
            int groupId = -1;
            string query = "SELECT group_id FROM users WHERE uname = @uname";

            using (SqlConnection conn = new SqlConnection(GraphsConnectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@uname", uname);

                try
                {
                    conn.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        groupId = (int)result;
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }

            return groupId;
        }
    }
}