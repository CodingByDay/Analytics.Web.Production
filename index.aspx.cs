using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using peptak.DatabaseStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace peptak
{
    public partial class index : System.Web.UI.Page
    {


        public static string ConnectionString = @"Data Source=10.100.100.25\SPLAHOST; Database=graphs;Application Name = Dashboard; Integrated Security = false; User ID = petpakn; Password=net123tnet!";
        private List<String> strings = new List<string>();



        protected void Page_Load(object sender, EventArgs e)
        {
             
                ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());
                var dataBaseDashboardStorage = new DataBaseEditableDashboardStorage(ConnectionString);
                ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);
                ASPxDashboard3.Visible = true;
                ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;
                ASPxDashboard3.ConfigureDataConnection += ASPxDashboard3_ConfigureDataConnection;
        
                ASPxDashboard3.DataRequestOptions.ItemDataRequestMode = ItemDataRequestMode.BatchRequests;
            if (!IsPostBack)
                {
          
                
                }

            
        }

     

        private void ASPxDashboard3_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
        {
          

            ConnectionStringSettings conn = GetConnectionString();          
            CustomStringConnectionParameters parameters =
                  (CustomStringConnectionParameters)e.ConnectionParameters;

            parameters.ConnectionString = conn.ConnectionString;
        }

        private ConnectionStringSettings GetConnectionString()
        {
            var ConnectionName = Session["conn"].ToString();

            ConnectionStringSettings stringFinal = ConfigurationManager.ConnectionStrings[ConnectionName];
            // debug



            return stringFinal;
        }

       





    }
}