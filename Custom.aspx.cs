using DevExpress.DashboardWeb;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WebDesigner_CustomDashboardStorage;

namespace peptak
{
    public partial class Custom : System.Web.UI.Page
    {
        static CustomDashboardStorage dashboardStorage = new CustomDashboardStorage();
        private string uname;
        private SqlConnection conn;
        private SqlCommand cmd;
        private string company;
        private string admin;
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxDashboard2.DashboardSaving += ASPxDashboard2_DashboardSaving;
            var company = getcompanyForUser();
            var folder = HttpContext.Current.User.Identity.Name;
            ASPxDashboard2.DashboardStorageFolder = $"~/App_Data/{company}/{folder}".Replace(" ", string.Empty);
            //ASPxDashboard2.DashboardStorageFolder = @"ftp://insistinsist:w3bp4ss!@89.212.55.202/web/dashboards/PetPak/user2";
            

            //FtpClient client = new FtpClient();
            //client.Host = "89.212.55.202";
            //client.Credentials = new NetworkCredential("insistinsist", "w3bp4ss!");
            //client.Connect();

            //ASPxDashboard2.DashboardStorageFolder = client.GetWorkingDirectory();

          

            // ASPxDashboard2.DashboardStorageFolder = "ftp://insistinsist:w3bp4ss!@89.212.55.202/web/dashboards/PetPak/user2";
            // ASPxDashboard2.SetDashboardStorage(dashboardStorage);
            var state = getViewState();

            switch (state)
            {

                case "Viewer":
                    ASPxDashboard2.WorkingMode = WorkingMode.ViewerOnly; //
                    break;
                case "Designer":
                    ASPxDashboard2.WorkingMode = WorkingMode.Designer;
                    break;
                case "Viewer&Designer":
                    ASPxDashboard2.WorkingMode = WorkingMode.Designer;
                    break;

            }
            HtmlAnchor BackButton = (HtmlAnchor)Master.FindControl("backButtonA");
            BackButton.Visible = false;
            ASPxDashboard2.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());     
        }

        private void ASPxDashboard2_DashboardSaving(object sender, DashboardSavingWebEventArgs e)
        {
            var dashboard = e.DashboardXml;


            var company = getcompanyForUser();
            var admin = GetAdminFromCompanyName(company);
            var userName = (string)HttpContext.Current.Session["CurrentUser"];
            var dashboardID = e.DashboardId;
            var folder = HttpContext.Current.User.Identity.Name;


            var pathAdmin = HttpContext.Current.Server.MapPath($"~/App_Data/{company.Replace(" ", string.Empty)}/{admin.Replace(" ", string.Empty)}/" + dashboardID + ".xml").Replace(" ", string.Empty);

            File.WriteAllText(pathAdmin, dashboard.ToString());

        }



        //select admin_id from companies where company_name='PetPak';



        public string GetAdminFromCompanyName(string company)
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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

        private string getViewState()
        {
         
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT ViewState FROM Users WHERE uname='{uname}' ", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            var state = (string)cmd.ExecuteScalar();

            return state;
        }

     


        protected void cmdsignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("logon.aspx", true);
        }

     
    }
}