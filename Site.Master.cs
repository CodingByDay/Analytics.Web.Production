using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;

namespace peptak
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
            cmdSignOut.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            admin.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            back.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            string UserNameForCheckingAdmin = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();

            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"Select userRole from Users where uname='{UserNameForCheckingAdmin}';", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            userRole = (string)cmd.ExecuteScalar();
            CheckIsAdminShowAdminButtonOrNot(userRole);
        }

        protected void cmdSignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("home.aspx", true);
        }

        private void CheckIsAdminShowAdminButtonOrNot(string userRole)
        {
            if (userRole != "SuperAdmin" && userRole != "Admin")
            {
                admin.Visible = false;
            }
            else
            {
                admin.Visible = true;
            }
        }


       


        protected void administration_Click(object sender, EventArgs e)
        {
            // Data
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("admin.aspx", true);
            } else if (userRole == "Admin")
            {
                Response.Redirect("tenantadmin.aspx", true);

            } else
            {
                ///
                Response.Redirect("logon.aspx", true); // config for securing data.
            }
        }

        protected void back_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("index", true);
            } else
            {

                Response.Redirect("indextenant", true);
            }

        }
    }
}