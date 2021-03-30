using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebDesigner_CustomDashboardStorage;

namespace peptak
{
    public partial class Custom : System.Web.UI.Page
    {
        static CustomDashboardStorage dashboardStorage = new CustomDashboardStorage();
        private SqlConnection conn;
        private SqlCommand cmd;
        private string company;

        protected void Page_Load(object sender, EventArgs e)
        {
            Button BackButton = (Button)Master.FindControl("back");
            BackButton.Enabled = false;
            BackButton.Visible = false;
            ASPxDashboard2.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());
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
            var folder = HttpContext.Current.User.Identity.Name;
            ASPxDashboard2.DashboardStorageFolder = $"~/App_Data/{company}/{folder}";

        }


        protected void cmdSignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("logon.aspx", true);

        }

    }
}