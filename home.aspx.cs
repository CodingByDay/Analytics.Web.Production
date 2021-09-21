using System;
using System.Collections.Generic;
using System.Configuration;
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
            login.Click += Login_Click;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            var version = ConfigurationManager.AppSettings["version"];
            Response.Redirect($"logon.aspx?version={version}", true);
        }
    }
}