using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WebDesigner_CustomDashboardStorage;

namespace peptak
{
    public partial class Custom : System.Web.UI.Page, IEditableDashboardStorage
    {
        static CustomDashboardStorage dashboardStorage = new CustomDashboardStorage();
        private string uname;
        private SqlConnection conn;
        private SqlCommand cmd;
        private string company;
        private string admin;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            var company = getcompanyForUser();
            var folder = HttpContext.Current.User.Identity.Name;
            ASPxDashboard2.DashboardStorageFolder = $"~/App_Data/{company}/{folder}".Replace(" ", string.Empty);
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
            Button BackButton = (Button)Master.FindControl("back");
            BackButton.Enabled = false;
            BackButton.Visible = false;
            ASPxDashboard2.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());     
        }



        //select admin_id from companies where company_name='PetPak';



        private string GetAdminFromCompanyName(string company)
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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

        private string getcompanyForUser()
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT ViewState FROM Users WHERE uname='{uname}' ", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            var state = (string)cmd.ExecuteScalar();

            return state;
        }

     


        protected void cmdSignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("logon.aspx", true);
        }

        public string AddDashboard(XDocument dashboard, string dashboardName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            throw new NotImplementedException();
        }

        public XDocument LoadDashboard(string dashboardID)
        {
            throw new NotImplementedException();
        }

        public void SaveDashboard(string dashboardID, XDocument dashboard)
        {
            var company = getcompanyForUser();
            var admin = GetAdminFromCompanyName(company);
            var userName = (string)HttpContext.Current.Session["CurrentUser"];

            var folder = HttpContext.Current.User.Identity.Name;


            var path = HttpContext.Current.Server.MapPath($"~/App_Data/{company}/{folder}".Replace(" ", string.Empty) + dashboardID + ".xml");

            File.WriteAllText(path, dashboard.ToString());

            var pathAdmin = HttpContext.Current.Server.MapPath($"~/App_Data/{company}/{admin}".Replace(" ", string.Empty) + dashboardID + ".xml");

            File.WriteAllText(path, dashboard.ToString());
        }
    }
}