using DevExpress.XtraRichEdit.Model;
using System;
using System.Configuration;

namespace Dash
{
    public partial class Filters : System.Web.UI.Page
    {
        private string connection;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
                gridViewTypes.CustomJSProperties += GridViewTypes_CustomJSProperties;
                gridViewOrganizations.CustomJSProperties += GridViewOrganizations_CustomJSProperties;
                gridViewLanguages.CustomJSProperties += GridViewLanguages_CustomJSProperties;
            }
        }

        private void GridViewLanguages_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties.Add("cpTotalRows", gridViewLanguages.VisibleRowCount);

        }

        private void GridViewOrganizations_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties.Add("cpTotalRows", gridViewOrganizations.VisibleRowCount);
        }

        private void GridViewTypes_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties.Add("cpTotalRows", gridViewTypes.VisibleRowCount);
        }


    }






}