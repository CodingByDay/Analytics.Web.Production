using System;

namespace Dash
{
    public partial class plans : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void login_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx");
        }


    }
}