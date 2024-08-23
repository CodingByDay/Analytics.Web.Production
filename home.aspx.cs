using System;
using System.Configuration;
namespace Dash
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
            Response.Redirect($"Logon.aspx?version={version}", true);
        }
    }
}