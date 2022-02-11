using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dash
{
    public partial class SidebarNavigation : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int collapseAtWindowInnerWidth = 1200;
            NavigationPanel.SettingsAdaptivity.CollapseAtWindowInnerWidth = collapseAtWindowInnerWidth;
            NavigationPanel.JSProperties["cpCollapseAtWindowInnerWidth"] = collapseAtWindowInnerWidth;

            PageHeader.InnerText = GetPageName();
            Content.InnerText = string.Format("Content for {0}", GetPageName());
        }

        string GetPageName()
        {
            string xField = Request.QueryString["x"];
            string name = string.IsNullOrEmpty(xField)
                ? "Home" : Request.QueryString["x"];
            return name;
        }

        protected void NavigationPanel_Load(object sender, EventArgs e)
        {

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "tryLoad()", true);

        }
    }
}