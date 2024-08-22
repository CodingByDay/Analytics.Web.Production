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
using System.Xml.Linq;

namespace Dash.DataExtract
{
    public static class DataExtractClass
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        public static IEnumerable <DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<DashboardInfo> list = new List<DashboardInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT ID, Caption FROM Dashboards");
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                while (reader.Read())
                {
                    string ID = reader.GetInt32(0).ToString();
                    string Caption = reader.GetString(1);
                    list.Add(new DashboardInfo() { ID = ID, Name = Caption });
                }
                connection.Close();
            }
            return list;
        }
        /// <summary>
        ///  Driver code for updating the extract.
        /// </summary>
        public static void Perform()
        {
            var dashboards = GetAvailableDashboardsInfo();
            foreach(var dash in dashboards)
            {
                if (dash.Name == "DBProdaja1OdDo_t")
                {
                    XDocument xDocument = LoadDashboard(dash.ID);
                    Dashboard d = new Dashboard();
                    d.LoadFromXDocument(xDocument);
                    CreateExtractAndSave(d, dash.ID);
                }
            }
        }

        public static XDocument LoadDashboard(string dashboardID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT  Dashboard FROM Dashboards WHERE ID=@ID");
                GetCommand.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                reader.Read();
                byte[] data = reader.GetValue(0) as byte[];
                MemoryStream stream = new MemoryStream(data);
                connection.Close();
                return XDocument.Load(stream);
            }
        }


        public static void SaveDashboard(string dashboardID, XDocument document)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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


        public static string getcompanyForUser()
        {
            string company = string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string uname = HttpContext.Current.User.Identity.Name;

                
                // Create SqlCommand to select pwd field from users table given supplied userName.
                var command = new SqlCommand($"SELECT uname, company_name FROM Users INNER JOIN Companies ON Users.id_company = Companies.id_company WHERE uname='{HttpContext.Current.User.Identity.Name}';", connection); /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    company = (reader["company_name"].ToString());
                }

                command.Dispose();
                connection.Close();
                return company;
            }
        }
        private static void CreateExtractAndSave(Dashboard dashboard, string ID)
        {

            var company = getcompanyForUser();




            DataSourceCollection dsCollection = new DataSourceCollection();
            dsCollection.AddRange(dashboard.DataSources.Where(d => !(d is DashboardExtractDataSource)));
            foreach (var ds in dsCollection)
            {
                DashboardExtractDataSource extractDataSource = new DashboardExtractDataSource();
                extractDataSource.ExtractSourceOptions.DataSource = ds;

                if (ds is DashboardSqlDataSource)
                    extractDataSource.ExtractSourceOptions.DataMember = ((DashboardSqlDataSource)(ds)).Queries[0].Name;
                var debug = HttpContext.Current.Server.MapPath($"~/App_Data/{company.Trim()}/Extract_{ds.Name}.dat");
                extractDataSource.FileName = HttpContext.Current.Server.MapPath($"~/App_Data/{company}/Extract_{ds.Name}.dat"); 
                extractDataSource.UpdateExtractFile();
                foreach (DataDashboardItem item in dashboard.Items)
                    if (item.DataSource == ds)
                        item.DataSource = extractDataSource;
            }
            dashboard.DataSources.RemoveRange(dsCollection);
            XDocument saved = dashboard.SaveToXDocument();

            SaveDashboard(ID, saved);
        }

    }
}