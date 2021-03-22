using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebDesigner_CustomDashboardStorage;

namespace peptak
{
    public partial class Custom : System.Web.UI.Page
    {
        static CustomDashboardStorage dashboardStorage = new CustomDashboardStorage();
        protected void Page_Load(object sender, EventArgs e)
        {

           // DevExpress.DashboardWeb.ASPxDashboard2.SetDashboardStorage(dashboardStorage);
        }
    }
}