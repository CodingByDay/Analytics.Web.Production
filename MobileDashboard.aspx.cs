using Dash.DatabaseStorage;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Web;
using System;
using System.Configuration;

namespace Dash
{
    public partial class MobileDashboard : System.Web.UI.Page
    {

        public static string ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;



            ASPxWebDashboard1.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());
            ASPxWebDashboard1.WorkingMode = WorkingMode.Viewer;
            ASPxWebDashboard1.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());

            var dataBaseDashboardStorage = new DataBaseEditableDashboardStorage(ConnectionString);

            ASPxWebDashboard1.SetDashboardStorage(dataBaseDashboardStorage);

            ASPxWebDashboard1.Visible = true;
        }



        protected void OnDataLoading(object sender, DevExpress.DashboardWeb.DataLoadingWebEventArgs e)
        {
            //   DashboardMainDemo.DataLoader.LoadData(e);
        }

        protected void OnDashboardLoading(object sender, DashboardLoadingWebEventArgs e)
        {

        }
    }
}




