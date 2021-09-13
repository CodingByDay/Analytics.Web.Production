using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Web;
using peptak.DatabaseStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class MobileDashboard : System.Web.UI.Page
    {

        public static string ConnectionString = @"Data Source=10.100.100.25\SPLAHOST; Database=graphs;Application Name = Dashboard; Integrated Security = false; User ID = dashboards; Password=Cporje?%ofgGHH$984d4L";

        protected void Page_Load(object sender, EventArgs e)
        {




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




