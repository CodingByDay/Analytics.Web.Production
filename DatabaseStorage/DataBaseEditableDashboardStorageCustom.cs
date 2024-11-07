using Dash.Log;
using Dash.Models;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Dash.DatabaseStorage
{
    public class DataBaseEditableDashboardStorageCustom : IEditableDashboardStorage
    {
        private string connectionString;
        public DashboardPermissions permissionsUser;
        public DashboardPermissions permissionsGroup;
        private SqlConnection conn;
        private int permisionID;
        private SqlCommand cmd;
        private string adminName;
        private string company;
        private string connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
        private object result;

        public DataBaseEditableDashboardStorageCustom(string connectionString)
        {
            this.connectionString = connectionString;
            permissionsUser = new DashboardPermissions(HttpContext.Current.User.Identity.Name);
            permissionsGroup = new DashboardPermissions(GetIdGroupForUser(HttpContext.Current.User.Identity.Name));
        }

        private int GetIdGroupForUser(string name)
        {
            int groupId = -1; // Default value if group_id is not found

            string connectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT group_id FROM users WHERE uname = @userName";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@userName", name);
                    conn.Open();

                    // Execute the command and get the group_id
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value) // Check if result is not null
                    {
                        groupId = Convert.ToInt32(result); // Convert result to int
                    }
                }
            }

            return groupId; // Return the found group_id or -1
        }

        public string AddDashboard(XDocument document, string dashboardName)
        {
            DevExpress.DashboardCommon.Dashboard d = new DevExpress.DashboardCommon.Dashboard();
            d.LoadFromXDocument(document);
            d.Title.Text = dashboardName;
            document = d.SaveToXDocument();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "INSERT INTO dashboards (dashboard, caption) " +
                    "output INSERTED.id " +
                    "VALUES (@dashboard, @caption)");
                string stripped = String.Concat(dashboardName.ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");
                InsertCommand.Parameters.Add("caption", SqlDbType.NVarChar).Value = stripped;
                InsertCommand.Parameters.Add("dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;

                object result = InsertCommand.ExecuteScalar();

                if(result == null)
                {
                    return "";
                }
                // Adding the permission for the admins of the company

                // Call the procedure to get admin IDs who need permissions for this dashboard
                SqlCommand GetAdminsCommand = new SqlCommand("sp_get_user_role_info", connection);
                GetAdminsCommand.CommandType = CommandType.StoredProcedure;
                GetAdminsCommand.Parameters.Add("uname", SqlDbType.VarChar).Value = HttpContext.Current.User.Identity.Name; 

                using (SqlDataReader reader = GetAdminsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string adminUname = reader["uname"].ToString();
                        if(adminUname != "Role not recognized")
                        {
                            DashboardPermissions dashboardPermissions = new DashboardPermissions(adminUname);
                            dashboardPermissions.Permissions.Add(new DashboardPermission { id = Int32.Parse(result.ToString()) });
                            dashboardPermissions.SetPermissionsForUser(adminUname);
                        }
                    }
                }


                return result.ToString();
            }




        }

        public string GetCompanyForUser()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    string uname = HttpContext.Current.User.Identity.Name;
                    string company = string.Empty;

                    // Use a parameterized query to prevent SQL injection
                    string query = @"SELECT company_name
                             FROM users
                             INNER JOIN companies ON users.id_company = companies.id_company
                             WHERE uname = @uname";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add the parameter for uname
                        cmd.Parameters.AddWithValue("@uname", uname);

                        // Execute the query and read the result
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // If there's a matching record
                            {
                                company = reader["company_name"].ToString();
                            }
                        }
                    }

                    return company; // Return the company name or empty if not found
                }
                catch (Exception)
                {
                    return string.Empty; // Return empty string on error
                }
            }
        }

        public XDocument LoadDashboard(string dashboardID)
        {
            if (!permissionsUser.DashboardWithIdAllowed(dashboardID)&&permissionsGroup.DashboardWithIdAllowed(dashboardID))
            {
                return null;
            }
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT dashboard FROM dashboards WHERE id=@id");
                GetCommand.Parameters.Add("id", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                reader.Read();
                byte[] data = reader.GetValue(0) as byte[];
                MemoryStream stream = new MemoryStream(data);
                DevExpress.DashboardCommon.Dashboard dashboard = new DevExpress.DashboardCommon.Dashboard();
                dashboard.LoadFromXDocument(XDocument.Load(stream));
                dashboard.DataSources.OfType<DashboardSqlDataSource>().ToList().ForEach(dataSource =>
                {
                    dataSource.DataProcessingMode = DataProcessingMode.Client;
                });
                connection.Close();

                string referer = GetRefererName(HttpContext.Current.User.Identity.Name);

                if (!String.IsNullOrEmpty(referer))
                {
                    var manipulated = ManipulateDocument(dashboard.SaveToXDocument(), referer);
                    return manipulated;
                }
                else
                {
                    return dashboard.SaveToXDocument();
                }
            }
        }

        private string GetRefererName(string name)
        {
#nullable enable
            string? referer = string.Empty;
#nullable disable
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT referrer FROM users WHERE uname=@uname;", connection);
                cmd.Parameters.AddWithValue("@uname", name);

                try
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString()!;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        private XDocument ManipulateDocument(XDocument doc, string referer)
        {
            XmlDocument document = new XmlDocument();
            var sql = doc.Root.Element("DataSources").Element("SqlDataSource").Element("Query").Element("Sql");
            var queryToChange = sql.Value;
            if (queryToChange.Contains("ProdajaKomercialist"))
            {
                if (queryToChange.Contains("WHERE"))
                {
                    if (queryToChange.Last() == ';')
                    {
                        queryToChange = queryToChange.Substring(0, queryToChange.Length - 1);
                    }
                    queryToChange = queryToChange + $" AND ProdajaKomercialist = '{referer}';";
                    sql.Value = queryToChange;
                }
                else
                {
                    if (queryToChange.Last() == ';')
                    {
                        queryToChange = queryToChange.Substring(0, queryToChange.Length - 1);
                    }
                    queryToChange = queryToChange + $" WHERE ProdajaKomercialist = '{referer}';";
                    sql.Value = queryToChange;
                }
            }
            return doc;
        }



        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            var availableUser = permissionsUser.Permissions.Select(p => p.id).ToList();
            var availableGroup = permissionsGroup.Permissions.Select(p => p.id).ToList();

            // Combine both lists and remove duplicates
            var combinedIds = availableUser.Union(availableGroup).ToList();

            // Build the IN clause dynamically
            var inClause = string.Join(",", combinedIds);

            if (combinedIds.Count == 0)
            {
                return new List<DashboardInfo>();
            }

            List<DashboardInfo> list = new List<DashboardInfo>();
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();

                // Create the command for the stored procedure
                using (SqlCommand GetCommand = new SqlCommand("sp_get_dashboards", connection))
                {
                    GetCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    GetCommand.Parameters.AddWithValue("@uname", HttpContext.Current.User.Identity.Name);
                    GetCommand.Parameters.AddWithValue("@company", GetIdCompany(GetCompanyForUser()));

                    // Execute the command and read the results
                    using (SqlDataReader reader = GetCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ID = reader["id"].ToString(); // Use column name
                            string customName = reader["custom_name"] != DBNull.Value ? reader["custom_name"].ToString() : string.Empty; // Use column name
                            list.Add(new DashboardInfo() { ID = ID, Name = customName });
                        }
                    }
                }
            }
            return list;
        }

        private int GetIdCompany(string current)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT id_company FROM companies WHERE company_name='{current}'", conn);
                    result = cmd.ExecuteScalar();
                    var id = System.Convert.ToInt32(result);
                    return id;
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(TenantAdmin), ex.InnerException.Message);
                    return -1;
                }
            }
        }

        public void SaveDashboard(string dashboardID, XDocument document)
        {
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "UPDATE dashboards SET dashboard = @dashboard " +
                    "WHERE id = @id");
                InsertCommand.Parameters.Add("id", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                InsertCommand.Parameters.Add("dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                InsertCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}