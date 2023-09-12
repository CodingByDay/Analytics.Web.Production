using Dash.Log;
using Dash.ORM;
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
        }

        public string AddDashboard(XDocument document, string dashboardName)
        {
            Dashboard d = new Dashboard();
            //
            d.LoadFromXDocument(document);
            d.Title.Text = dashboardName;
            document = d.SaveToXDocument();
            
            // 
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
                connection.Close();
                InsertPermision(stripped);
                var company = getcompanyForUser();
                var admin = GetAdminFromCompanyName(company);
                int idAdmin = GetPermisionUserID(admin);
                InsertPermisionAdminAndUser(idAdmin, stripped);
                return ID;
            }
        }
        private int getIdPermision()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name;
                    SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{UserNameForChecking}'", conn);
                    var result = cmd.ExecuteScalar();
                    permisionID = System.Convert.ToInt32(result);
                    cmd.Dispose();
                    return permisionID;
                }
                catch (Exception)
                {
                    return -1;
                }
            }



        }
        private void InsertPermisionAdminAndUser(int admin, string name)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var idCurrent = getIdPermision();
                    SqlCommand cmd = new SqlCommand($"update permisions_user set {name} = 1 where id_permisions_user = {idCurrent}", conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    SqlCommand cmdSecond = new SqlCommand($"update permisions_user set {name} = 1 where id_permisions_user = {admin}", conn);
                    cmdSecond.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception)
                {

                }
            }


        }
        public int GetPermisionUserID(string user)
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    string uname = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"select id_permision_user from Users where uname='{user}';", conn);
                    // Execute command and fetch pwd field into lookupPassword string.
                    int adminID = (int)cmd.ExecuteScalar();
                    cmd.Dispose();
                    return adminID;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        public string GetAdminFromCompanyName(string company)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string uname = HttpContext.Current.User.Identity.Name;
                    var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"SELECT admin_id FROM companies WHERE company_name='{company}'", conn); /// Intepolation or the F string. C# > 5.0       
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        adminName = (reader["admin_id"].ToString());
                    }

                    cmd.Dispose();
                    return adminName;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

        }

        public string getcompanyForUser()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    string uname = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"SELECT uname, company_name FROM Users INNER JOIN companies ON Users.id_company = companies.id_company WHERE uname='{HttpContext.Current.User.Identity.Name}';", conn); /// Intepolation or the F string. C# > 5.0       
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        company = (reader["company_name"].ToString());
                    }

                    cmd.Dispose();
                    return company;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

        }
        private void InsertPermision(string dashboardName)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"ALTER TABLE permisions_user ADD {dashboardName} int not null default(0);", conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }
                catch (Exception)
                {

                }
            }

        }

        public XDocument LoadDashboard(string dashboardID)
        {

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
                    Dashboard dashboard = new Dashboard();
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
                SqlCommand cmd = new SqlCommand($"select referer from Users where uname='{name}'", connection);

                try
                {
                    string result = (string)cmd.ExecuteScalar();
                    if(!String.IsNullOrEmpty(result))
                    {
                        return result;
                    } else
                    {
                        return string.Empty;
                    }
                } catch
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

        private List<String> getCaptions()
        {
            List<String> list = new List<String>();
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT Caption FROM Dashboards");
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                while (reader.Read())
                {

                    string Caption = reader.GetString(0);
                    list.Add(Caption);
                }
                connection.Close();
            }
            return list;
        }



        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<String> available = getIdPermisionCurrentUser();
            string FinalString = "(";
            // Getting the final string
            for (int i = 0; i < available.Count; i++)
            {
                if (i != available.Count - 1)
                {
                    FinalString += "'" + available[i] + "'" + ",";
                }
                else
                {
                    FinalString += "'" + available[i] + "'" + ")";
                }
            }
            List<DashboardInfo> list = new List<DashboardInfo>();
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand($"SELECT ID, Caption FROM Dashboards WHERE Caption IN {FinalString}");
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                string name = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                int id = getIdCompany(getcompanyForUser());
                Graph graph = new Graph(id);
                var payload = graph.GetNames(id);
                while (reader.Read())
                {
                    string ID = reader.GetInt32(0).ToString();
                    string Caption = reader.GetString(1);
                    try
                    {
                        var custom = payload.FirstOrDefault(x => x.original == Caption).custom;

                        list.Add(new DashboardInfo() { ID = ID, Name = custom });
                    } catch
                    {
                        list.Add(new DashboardInfo() { ID = ID, Name = Caption });
                    }
                }         
                    var graphs = graph.GetGraphs(id);
                    List <Graph.Names> data = new List<Graph.Names>();
                    foreach(var obj in graphs)
                    {
                        data.Add(new Graph.Names { original = obj.Name, custom = obj.CustomName });
                    }
                    graph.UpdateGraphs(data, id);
               
            }
            return list;
        }

        private int getIdCompany(string current)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"select id_company from companies where company_name='{current}'", conn);
                    result = cmd.ExecuteScalar();
                    var finalID = System.Convert.ToInt32(result);
                    return finalID;

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

        private List<String> getIdPermisionCurrentUser()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    List<String> captions = getCaptions();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                    List<String> permisions = new List<string>();
                    SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{UserNameForChecking}'", conn);
                    var result = cmd.ExecuteScalar();
                    permisionID = System.Convert.ToInt32(result);
                    int idUser = permisionID;
                    cmd.Dispose();
                    foreach (String graph in captions)
                    {
                        string whiteless = String.Concat(graph.Where(c => !Char.IsWhiteSpace(c)));
                        string stripped = whiteless.Replace("-", "");
                        SqlCommand graphResult = new SqlCommand($"select {stripped} from permisions_user where id_permisions_user={idUser}", conn);
                        string deb = $"select {stripped} from permisions_user where id_permisions_user={idUser}";
                        var resultID = graphResult.ExecuteScalar();
                        int permision = (int)resultID;
                        graphResult.Dispose();
                        if (permision == 1)
                        {
                            permisions.Add(graph);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    return permisions;
                }
                catch (Exception)
                {
                    List<string> data = new List<string>();
                    return data;
                }
            }


        }
    }
}
