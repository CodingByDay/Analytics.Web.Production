using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace Dash
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        ///  Add a default company entry for the new user and center the buttons. Change the names of all of the so u can change the destination.
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
            cmd = new SqlCommand($"SELECT userRole FROM Users WHERE uname='{UserNameForCheckingAdmin}';", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.

        }

        protected void cmdsignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("home.aspx", true);
        }







        protected void administration_Click(object sender, EventArgs e)
        {
            // Data
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("admin.aspx", true);
            }
            else if (userRole == "Admin")
            {
                Response.Redirect("tenantadmin.aspx", true);

            }
            else
            {

                Response.Redirect("Logon.aspx", true); // config for securing data.
            }
        }

        protected void back_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("index", true);
            }
            else
            {

                Response.Redirect("indextenant", true);
            }

        }

        protected void desktop_button_Click(object sender, EventArgs e)
        {
            var test = userRole;


            if (userRole == "SuperAdmin")
            {
                Response.Redirect("index", true);

            }
            else
            {

                Response.Redirect("indextenant", true);

            }
        }

        protected void mobile_button_Click(object sender, EventArgs e)
        {
            Response.Redirect("Emulator", true);
        }

        protected void adminButtonAnchor_ServerClick(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("admin.aspx", true);
            }
            else if (userRole == "Admin")
            {
                Response.Redirect("tenantadmin.aspx", true);

            }
            else
            {

                Response.Redirect("Logon.aspx", true); // config for securing data.
            }
        }

        protected void backButtonA_ServerClick(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("index", true);
            }
            else
            {

                Response.Redirect("indextenant", true);
            }
        }

        protected void signOutAnchor_ServerClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("home.aspx", true);
        }
    }
}
