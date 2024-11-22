using Sentry;
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
        private string connection;

        public bool BackButtonVisible
        {
            get { return backButton.Visible; }

            set
            {
                backButton.Visible = value;
                backButtonOuter.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("Expires", "0");

                signOutButton.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                adminButton.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                backButton.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                string uname = HttpContext.Current.User.Identity.Name;
                CheckUserTypeModifyUI(uname);

                CheckWhetherToShowTheSwitcherAtAll();

                ConditionalyAddStylesBasedOnTheUrl();

                HideHamburgerIfNecessary();

                UpdateVersionLiteral();

                if (Request.Browser.IsMobileDevice && Request.Browser.ScreenPixelsWidth <= 767)
                {
                    HideActionButtonsForMobile();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void UpdateVersionLiteral()
        {
            try
            {
                // Get the version value from the appSettings section
                string version = ConfigurationManager.AppSettings["Version"];

                if (!string.IsNullOrEmpty(version))
                {
                    // Assuming you have a Literal control named VersionLiteral
                    versionLiteral.Text = "v. " + version;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void HideActionButtonsForMobile()
        {
            try
            {
                adminButton.Visible = false;
                adminButtonOuter.Visible = false;
                backButton.Visible = false;
                backButtonOuter.Visible = false;
                filtersButton.Visible = false;
                filterButtonOuter.Visible = false;
                groupsButton.Visible = false;
                groupsButtonOuter.Visible = false;
                pic.Visible = false;
                languages.Visible = false;
                versionLiteral.Visible = false;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void HideHamburgerIfNecessary()
        {
            try
            {
                string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;

                // Check if the URL contains "Index" or "IndexTenant"
                if (!currentUrl.Contains("Index") && !currentUrl.Contains("IndexTenant"))
                {
                    pic.Visible = false;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void ConditionalyAddStylesBasedOnTheUrl()
        {
            try
            {
                string currentPage = Page.AppRelativeVirtualPath;

                switch (currentPage)
                {
                    case "~/Admin.aspx":
                        AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                        AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                        AddCssLink("~/Content/Css/All.css");
                        AddCssLink("~/Content/Css/Graphs.css");
                        AddCssLink("~/Content/Css/Website.css");
                        AddCssLink("~/Content/Css/Admin.css");
                        break;

                    case "~/TenantAdmin.aspx":
                        AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                        AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                        AddCssLink("~/Content/Css/All.css");
                        AddCssLink("~/Content/Css/Website.css");
                        AddCssLink("~/Content/Css/Graphs.css");
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

                    case "~/Groups.aspx":
                        AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                        AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                        AddCssLink("~/Content/Css/All.css");
                        AddCssLink("~/Content/Css/Website.css");
                        AddCssLink("~/Content/Css/Admin.css");
                        break;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void AddCssLink(string cssPath)
        {
            try
            {
                HtmlLink cssLink = new HtmlLink();
                cssLink.Href = cssPath;
                cssLink.Attributes.Add("rel", "stylesheet");
                cssLink.Attributes.Add("type", "text/css");
                Page.Header.Controls.Add(cssLink);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void CheckWhetherToShowTheSwitcherAtAll()
        {
            try
            {
                // Get the current URL
                string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;

                // Check if the URL contains "Index" or "IndexTenant"
                if (!currentUrl.Contains("Index") && !currentUrl.Contains("IndexTenant"))
                {
                    switcherOuter.Visible = false;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void SignOut_Click(object sender, EventArgs e)
        {
            try
            {
                HelperClasses.UserSession.SignOutUser(HttpContext.Current.User.Identity.Name);
                FormsAuthentication.SignOut();
                Session["current"] = string.Empty;
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void CheckUserTypeModifyUI(string uname)
        {
            try
            {
                // Use 'using' statements to ensure proper disposal of resources
                using (var conn = new SqlConnection(connection))
                {
                    conn.Open();
                    // Use a parameterized query to avoid SQL injection
                    using (var cmd = new SqlCommand("SELECT user_role FROM users WHERE uname = @UserName", conn))
                    {
                        // Add the parameter for the user name
                        cmd.Parameters.AddWithValue("@UserName", uname);

                        // Execute command and fetch user_role field into the userRole variable
                        userRole = cmd.ExecuteScalar()?.ToString();

                        if (userRole == "User")
                        {
                            adminButton.Visible = false;
                            adminButtonOuter.Visible = false;
                            groupsButton.Visible = false;
                            groupsButtonOuter.Visible = false;
                            filtersButton.Visible = false;
                            filterButtonOuter.Visible = false;
                        }
                        else if (userRole == "SuperAdmin")
                        {
                            adminButton.Visible = true;
                            adminButtonOuter.Visible = true;
                            groupsButton.Visible = true;
                            groupsButtonOuter.Visible = true;
                            filtersButton.Visible = true;
                            filterButtonOuter.Visible = true;
                        }
                        else if (userRole == "Admin")
                        {
                            adminButton.Visible = true;
                            adminButtonOuter.Visible = true;
                            groupsButton.Visible = true;
                            groupsButtonOuter.Visible = true;
                            filtersButton.Visible = false;
                            filterButtonOuter.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void Administration_Click(object sender, EventArgs e)
        {
            try
            {
                // Data
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            try
            {
                if (userRole == "SuperAdmin")
                {
                    Response.Redirect("Index.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Response.Redirect("IndexTenant.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void Filters_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Filters.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void groupsButton_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Groups.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}