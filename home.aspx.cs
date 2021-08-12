using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FluentFTP;
namespace peptak
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // testing the ftp class
            login.Click += Login_Click;
            plans.Click += Plans_Click;
            Button1.Click += Button1_Click;
            Button2.Click += Button2_Click;
            Button3.Click += Button3_Click;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("membership.aspx", true);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("membership.aspx", true);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("membership.aspx", true);
        }

        private void Plans_Click(object sender, EventArgs e)
        {
            Response.Redirect("membership.aspx", true);
        }

        private void Login_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx"); // Entry point to the application.
        }

        protected void Login(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx"); // Entry point to the application.
        }

        protected void login_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx"); // Entry point to the application.
        }

        protected void plans_Click(object sender, EventArgs e)
        {
            Response.Redirect("membership.aspx");
        }

       


    }
}