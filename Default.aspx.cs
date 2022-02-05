using DevExpress.DashboardWeb;
using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
namespace Dash
{
    public partial class _Default : Page
    {
        private SqlConnection conn;
        private SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Button BackButton = (Button)Master.FindControl("back");
            //  BackButton.Enabled = false;
            //  BackButton.Visible = false;
            ASPxDashboard1.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());

            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"select userRole from Users where uname='{uname}'", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            var sdr = cmd.ExecuteScalar();

            var user = sdr.ToString();

            if (user == "SuperAdmin")
            {
                ASPxDashboard1.WorkingMode = WorkingMode.Designer;
            }
            else
            {
                ASPxDashboard1.WorkingMode = WorkingMode.Viewer;
            }



        }


        protected void cmdsignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("logon.aspx", true);

        }



    }
}