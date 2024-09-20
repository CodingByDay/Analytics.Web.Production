using DevExpress.XtraCharts.Native;
using DevExpress.XtraRichEdit.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace Dash.Models
{
    public class DashboardPermissions
    {
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

        public DashboardPermissions()
        {
            
        }

        private DashboardPermissions GetPermissionsForUser(string uname)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT dashboard_permissions FROM Users WHERE uname = @uname";
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

        public DashboardPermissions ConvertJsonToPermissions(string json)
        {
            string unescapedJson = Regex.Unescape(json);
            // Now, you can deserialize it
            var permissions = JsonConvert.DeserializeObject<DashboardPermissions>(unescapedJson);
            return permissions;
        }
    }




    public class DashboardPermission
    {
        public int id { get; set; }
    }

}