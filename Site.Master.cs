using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;

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
            cmd = new SqlCommand($"Select userRole from Users where uname='{UserNameForCheckingAdmin}';", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            userRole = (string)cmd.ExecuteScalar();
            CheckIsAdminShowAdminButtonOrNot(userRole);

        }

        protected void cmdsignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("home.aspx", true);
        
           
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

                Response.Redirect("logon.aspx", true); // config for securing data.
            }
        }

        protected void back_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Session["change"] = "yes";
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
    }
}