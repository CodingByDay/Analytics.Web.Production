﻿using DevExpress.XtraRichEdit.Model;
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

        protected void Page_Load(object sender, EventArgs e)
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

                case "~/Groups.aspx":
                    AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css");
                    AddCssLink("https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                    AddCssLink("~/Content/Css/All.css");
                    AddCssLink("~/Content/Css/Website.css");
                    AddCssLink("~/Content/Css/Admin.css");
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

        private void CheckUserTypeModifyUI(string uname)
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

                    if(userRole == "User")
                    {
                        adminButton.Visible = false;
                        groupsButton.Visible = false;
                        filtersButton.Visible = false;
                        backButton.Visible = false;
                    } else if (userRole == "SuperAdmin")
                    {
                        adminButton.Visible = true;
                        groupsButton.Visible = true;
                        filtersButton.Visible = true;
                        backButton.Visible = false;
                    } else if (userRole == "Admin")
                    {
                        adminButton.Visible = true;
                        groupsButton.Visible = true;
                        filtersButton.Visible = true;
                        backButton.Visible = false;
                    }
                }
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

        protected void groupsButton_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("Groups.aspx", true);
        }
    }
}