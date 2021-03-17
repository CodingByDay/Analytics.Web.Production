using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.DashboardWeb;

namespace peptak
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            ASPxDashboard1.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());
            string uname = HttpContext.Current.User.Identity.Name;
            if (uname == "user1")
            {
                ASPxDashboard1.WorkingMode = WorkingMode.Designer;

            }
            else {
                ASPxDashboard1.WorkingMode = WorkingMode.Viewer;
            }

        }

        protected void cmdSignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("logon.aspx", true);

        }


       
    }
}