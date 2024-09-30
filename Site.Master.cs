using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Dash
{
    public partial class SiteMaster : MasterPage
    {
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
            cmd = new SqlCommand($"SELECT user_role FROM users WHERE uname='{UserNameForCheckingAdmin}';", conn);
            // Execute command and fetch pwd field into lookupPassword string.
            userRole = (string)cmd.ExecuteScalar();
            CheckIsAdminShowAdminButtonOrNot(userRole);
            CheckWhetherToShowTheSwitcherAtAll();
            ConditionalyAddStylesBasedOnTheUrl();
        }

        private void ConditionalyAddStylesBasedOnTheUrl()
        {
            string currentPage = Page.AppRelativeVirtualPath;

            switch (currentPage)
            {
                case "~/Admin.aspx":
                    AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                    AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                    AddCssLink("~/Content/Css/All.css");
                    AddCssLink("~/Content/Css/Website.css");
                    AddCssLink("~/Content/Css/Admin.css");
                    break;

                case "~/TenantAdmin.aspx":
                    AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                    AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                    AddCssLink("~/Content/Css/All.css");
                    AddCssLink("~/Content/Css/Website.css");
                    AddCssLink("~/Content/Css/Admin.css");
                    break;

                case "~/Emulator.aspx":
                    AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                    AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                    AddCssLink("~/Content/Css/All.css");
                    AddCssLink("~/Content/Css/Website.css");
                    AddCssLink("~/Content/Css/Admin.css");
                    break;

                case "~/Filters.aspx":
                    AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                    AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                    AddCssLink("~/Content/Css/All.css");
                    AddCssLink("~/Content/Css/Website.css");
                    AddCssLink("~/Content/Css/Admin.css");
                    break;

                case "~/Index.aspx":
                    AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                    AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                    AddCssLink("~/Content/Css/All.css");
                    AddCssLink("~/Content/Css/Graphs.css");
                    AddCssLink("~/Content/Css/Website.css");
                    break;

                case "~/IndexTenant.aspx":
                    AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                    AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                    AddCssLink("~/Content/Css/All.css");
                    AddCssLink("~/Content/Css/Graphs.css");
                    AddCssLink("~/Content/Css/Website.css");
                    break;
            }
        }

        private void AddCssLink(string cssPath)
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = cssPath;
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(cssLink);
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
                Response.Redirect("Logon.aspx", true);
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            if (userRole == "SuperAdmin")
            {
                Response.Redirect("Index.aspx", true);
            }
            else
            {
                Response.Redirect("IndexTenant.aspx", true);
            }
        }

        protected void Filters_Click(object sender, EventArgs e)
        {
            Response.Redirect("Filters.aspx", true);
        }
    }
}