using Dash.Log;
using Dash.Models;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
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
                    "INSERT INTO Dashboards (Dashboard, Caption) " +
                    "output INSERTED.ID " +
                    "VALUES (@Dashboard, @Caption)");
                string stripped = String.Concat(dashboardName.ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");
                InsertCommand.Parameters.Add("Caption", SqlDbType.NVarChar).Value = stripped;
                InsertCommand.Parameters.Add("Dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                string ID = InsertCommand.ExecuteScalar().ToString();
                return ID;
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
                             FROM Users
                             INNER JOIN Companies ON Users.id_company = Companies.id_company
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
                SqlCommand GetCommand = new SqlCommand("SELECT  Dashboard FROM Dashboards WHERE ID=@ID");
                GetCommand.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
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
                SqlCommand GetCommand = new SqlCommand($@"
                SELECT 
                    d.id, 
                    d.caption, 
                    d.belongs, 
                    d.meta_data, 
                    COALESCE(ds.sort_order, NULL) AS sort_order 
                FROM 
                    dashboards d
                LEFT JOIN 
                    dashboards_sorted_by_user ds ON d.id = ds.dashboard_id AND ds.uname = @uname 
                WHERE 
                    d.id IN ({inClause}) 
                ORDER BY 
                    CASE 
                        WHEN ds.sort_order IS NOT NULL THEN ds.sort_order  -- Order by sort_order for non-NULL
                        ELSE (SELECT COUNT(*) FROM dashboards_sorted_by_user AS sub_ds 
                              WHERE sub_ds.sort_order IS NOT NULL AND sub_ds.dashboard_id < d.id) + 1
                    END,
                    d.id;");
                GetCommand.Parameters.AddWithValue("@uname", HttpContext.Current.User.Identity.Name);
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                string name = HttpContext.Current.User.Identity.Name;
                int id = GetIdCompany(GetCompanyForUser());
                Models.Dashboard graph = new Models.Dashboard(id);
                var payload = graph.GetNames(id);
                while (reader.Read())
                {
                    string ID = reader.GetInt32(0).ToString();
                    string Caption = reader.GetString(1);
                    try
                    {
                        var custom = payload.FirstOrDefault(x => x.original == Caption).custom;

                        list.Add(new DashboardInfo() { ID = ID, Name = custom });
                    }
                    catch
                    {
                        list.Add(new DashboardInfo() { ID = ID, Name = Caption });
                    }
                }
                var graphs = graph.GetGraphs(id);
                List<Models.Dashboard.Names> data = new List<Models.Dashboard.Names>();
                foreach (var obj in graphs)
                {
                    data.Add(new Models.Dashboard.Names { original = obj.Name, custom = obj.CustomName });
                }
                graph.UpdateGraphs(data, id);
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
                    SqlCommand cmd = new SqlCommand($"SELECT id_company FROM Companies WHERE company_name='{current}'", conn);
                    result = cmd.ExecuteScalar();
                    var id = System.Convert.ToInt32(result);
                    return id;
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
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
                    "UPDATE Dashboards Set Dashboard = @Dashboard " +
                    "WHERE ID = @ID");
                InsertCommand.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                InsertCommand.Parameters.Add("Dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                InsertCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}