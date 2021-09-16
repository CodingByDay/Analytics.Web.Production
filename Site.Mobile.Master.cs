using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        ///  Add a default company entry for the new user and center the buttons. Change the names of all of the so u can change the destination.
        /// </summary>
        private SqlCommand cmd;
        private string userRole;
        private SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            cmdsignOut.Click += CmdsignOut_Click;
            admin.Click += Admin_Click;
            back.Click += Back_Click;
            cmdsignOut.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            admin.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            back.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            string UserNameForCheckingAdmin = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();

            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"Select userRole from Users where uname='{UserNameForCheckingAdmin}';", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            userRole = (string)cmd.ExecuteScalar();
            CheckIsAdminShowAdminButtonOrNot(userRole);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("default", true);
            }
            else
            {

                Response.Redirect("custom", true);
            }
        }

        private void Admin_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("AdminPanel.aspx", true);
            }
            else if (userRole == "Admin")
            {
                Response.Redirect("AdminPanelCompany.aspx", true);

            }
            else
            {
                ///
                Response.Redirect("logon.aspx", true); // config for securing data.
            }
        }

        private void CmdsignOut_Click(object sender, EventArgs e)
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






        protected void cmdsignOut_Click1(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("logon.aspx", true);
        }

        protected void admin_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("AdminPanel.aspx", true);
            }
            else if (userRole == "Admin")
            {
                Response.Redirect("AdminPanelCompany.aspx", true);

            }
            else
            {
                ///
                Response.Redirect("logon.aspx", true); // config for securing data.
            }
        }

        protected void back_Click1(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("default", true);
            }
            else
            {

                Response.Redirect("custom", true);
            }
        }
    }
}
