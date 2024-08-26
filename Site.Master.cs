using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Dash
{
    public partial class SiteMaster : MasterPage
    {
        /// <summary>
        ///  Add a default company entry for the new user and center the buttons. Change the names of all of the so u can change the destination.
        /// </summary>
        private SqlCommand cmd;
        private string userRole;
        private SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("Expires", "0");

            signOutAnchor.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            adminButtonAnchor.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            backButtonA.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            string UserNameForCheckingAdmin = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT userRole FROM Users WHERE uname='{UserNameForCheckingAdmin}';", conn);        
            // Execute command and fetch pwd field into lookupPassword string.
            userRole = (string)cmd.ExecuteScalar();
            CheckIsAdminShowAdminButtonOrNot(userRole);
            CheckWhetherToShowTheSwitcherAtAll();
        }

        private void CheckWhetherToShowTheSwitcherAtAll()
        {
            // Get the current URL
            string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;

            // Check if the URL contains "Index" or "IndexTenant"
            if (!currentUrl.Contains("Index") && !currentUrl.Contains("IndexTenant"))
            {
                toggle.Visible = false;
            }
        }

        protected void SignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session["current"] = string.Empty;
            Response.Redirect("Home.aspx", true);


        }

        private void CheckIsAdminShowAdminButtonOrNot(string userRole)
        {
            if (userRole != "SuperAdmin" && userRole != "Admin")
            {
                adminButtonAnchor.Visible = false;
            }
            else
            {
                adminButtonAnchor.Visible = true;
            }
        }





        protected void Administration_Click(object sender, EventArgs e)
        {
            // Data
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Admin.aspx", true);
            }
            else if (userRole == "Admin")
            {
                Response.Redirect("TenantAdmin.aspx", true);

            }
            else
            {

                Response.Redirect("Logon.aspx", true); // config for securing data.
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Index", true);
            }
            else
            {

                Response.Redirect("IndexTenant", true);
            }

        }


        protected void Filters_Click(object sender, EventArgs e)
        {
            Response.Redirect("Filters", true);
        }


    }
}