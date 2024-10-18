using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace Dash
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        //  Add a default company entry for the new user and center the buttons. Change the names of all of the so u can change the destination.
        private SqlCommand cmd;

        private string userRole;
        private SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            signOutAnchor.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            conn = new SqlConnection(ConnectionString);
            string UserNameForCheckingAdmin = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT userRole FROM Users WHERE uname='{UserNameForCheckingAdmin}';", conn);
            // Execute command and fetch pwd field into lookupPassword string.
        }

        protected void cmdsignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("Home.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void administration_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Admin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (userRole == "Admin")
            {
                Response.Redirect("TenantAdmin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("Logon.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void back_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Index", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("IndexTenant", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void desktop_button_Click(object sender, EventArgs e)
        {
            var test = userRole;

            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Index", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("IndexTenant", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void mobile_button_Click(object sender, EventArgs e)
        {
            Response.Redirect("Emulator", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void adminButtonAnchor_ServerClick(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Admin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (userRole == "Admin")
            {
                Response.Redirect("TenantAdmin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("Logon.aspx", false); // config for securing data.
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void backButtonA_ServerClick(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Index", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("IndexTenant", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void signOutAnchor_ServerClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("Home.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}