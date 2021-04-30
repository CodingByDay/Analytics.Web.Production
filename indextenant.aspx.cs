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
    public partial class indextenant : System.Web.UI.Page
    {
        public static string ConnectionString = @"Data Source=10.100.100.25\SPLAHOST; Database=graphs;Application Name = Dashboard; Integrated Security = false; User ID = petpakn; Password=net123321!";
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());

            var dataBaseDashboardStorage = new DataBaseEditableDashboardStorageCustom(ConnectionString);
            ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);

            ASPxDashboard3.Visible = true;

        }
    }
}