using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Xml.Linq;
using DevExpress.DashboardWeb;
using peptak;
using System.Data.SqlClient;

namespace WebDesigner_CustomDashboardStorage
{
    public class CustomDashboardStorage : IEditableDashboardStorage
    {
        DashboardStorageDataSet dashboards = new DashboardStorageDataSet();
        private SqlConnection conn;
        private SqlCommand cmd;
        private string company;
        private string admin;

        // Adds a dashboard with the specified ID and name to a DataSet. 
        // Note that the 'DashboardID' column is an auto-increment column that is used to store unique dashboard IDs.
        public string AddDashboard(XDocument dashboard, string dashboardName)
        {
            DataRow newRow = dashboards.Tables[0].NewRow();
            newRow["DashboardName"] = dashboardName;
            newRow["DashboardXml"] = dashboard;
            dashboards.Tables[0].Rows.Add(newRow);
            return newRow["DashboardID"].ToString();
        }

        // Gets information about dashboards available in a DataSet.
        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<DashboardInfo> dashboardInfos = new List<DashboardInfo>();
            foreach (DataRow row in dashboards.Tables[0].Rows)
            {
                DashboardInfo dashboardInfo = new DashboardInfo();
                dashboardInfo.ID = row["DashboardID"].ToString();
                dashboardInfo.Name = row["DashboardName"].ToString();
                dashboardInfos.Add(dashboardInfo);
            }
            return dashboardInfos;
        }

        // Loads a dashboard corresponding to the specified ID.
        public XDocument LoadDashboard(string dashboardID)
        {
            DataRow currentRow = dashboards.Tables[0].Rows.Find(dashboardID);
            XDocument dashboardXml = XDocument.Parse(currentRow["DashboardXml"].ToString());
            return dashboardXml;
        }

        // Saves the dashboard with the specified ID to a DataSet.
        public void SaveDashboard(string dashboardID, XDocument dashboard)
        {
            DataRow currentRow = dashboards.Tables[0].Rows.Find(dashboardID);
            currentRow["DashboardXml"] = dashboard;
            var company = getcompanyForUser();
            var admin = GetAdminFromCompanyName(company);
            var userName = (string)HttpContext.Current.Session["CurrentUser"];

            var folder = HttpContext.Current.User.Identity.Name;


            var path = HttpContext.Current.Server.MapPath($"~/App_Data/{company}/{folder}".Replace(" ", string.Empty) + dashboardID + ".xml");

            File.WriteAllText(path, dashboard.ToString());

            var pathAdmin = HttpContext.Current.Server.MapPath($"~/App_Data/{company}/{admin}".Replace(" ", string.Empty) + dashboardID + ".xml");

            File.WriteAllText(path, dashboard.ToString());
        }




        public string GetAdminFromCompanyName(string company)
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT admin_id FROM companies WHERE company_name='{company}'", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                admin = (reader["admin_id"].ToString());
            }

            cmd.Dispose();
            conn.Close();
            return admin;
        }

        public string getcompanyForUser()
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT uname, company_name FROM Users INNER JOIN companies ON Users.id_company = companies.id_company WHERE uname='{HttpContext.Current.User.Identity.Name}';", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                company = (reader["company_name"].ToString());
            }

            cmd.Dispose();
            conn.Close();
            return company;
        }

    }
}