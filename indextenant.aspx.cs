using Dash.DatabaseStorage;
using Dash.Log;
using Dash.Models;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

namespace Dash
{
    public partial class IndexTenant : System.Web.UI.Page
    {
        private string connection;
        public static string ConnectionString;
        private SqlConnection conn;
        private int companyID;
        private int stringID;
        private string stringConnection;
        private int value;
        private bool flag = false;
        private string state;
        private string returnString;
        private SqlCommand cmd;
        private int permisionID;
        private HttpRequest httpRequest;
        private string companyInfo;
        private object result;
        private DataBaseEditableDashboardStorageCustom dataBaseDashboardStorage;


        protected void Page_Load(object sender, EventArgs e)
        {
            dataBaseDashboardStorage = new DataBaseEditableDashboardStorageCustom(ConnectionString);

            connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            if (Request.Cookies["dashboard"] != null)
            {
                // New OOP structure 23.09.2024
                if (dataBaseDashboardStorage.permissionsUser.DashboardWithIdAllowed(Request.Cookies["dashboard"].Value.ToString()))
                {
                    ASPxDashboard3.InitialDashboardId = Request.Cookies["dashboard"].Value.ToString();
                }               
            }

            HtmlAnchor admin = Master.FindControl("backButtonA") as HtmlAnchor;
            ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            ASPxDashboard3.LimitVisibleDataMode = LimitVisibleDataMode.DesignerAndViewer;
            ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());
            ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);
            ASPxDashboard3.ConfigureDataConnection += ASPxDashboard1_ConfigureDataConnection;
            ASPxDashboard3.AllowCreateNewDashboard = true;
            ASPxDashboard3.DashboardLoading += ASPxDashboard1_DashboardLoading;
            ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;
            ASPxDashboard3.DataRequestOptions.ItemDataRequestMode = ItemDataRequestMode.BatchRequests;
            ASPxDashboard3.WorkingMode = WorkingMode.Viewer;
            ASPxDashboard3.CustomExport += ASPxDashboard3_CustomExport;

          
            SetUpPage();
        }

        private void SetUpPage()
        {
            if (Request.Cookies.Get("state") is null)
            {
                Response.Cookies["state"].Value = "light";
            }
            else
            {
                state = Request.Cookies.Get("state").Value;
                switch (state)
                {
                    case "light":
                        ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeLight;
                        break;

                    case "dark":
                        ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeDarkMoon;
                        break;
                }
            }
        }

        [WebMethod]
        public static void DeleteItem(string id)
        {
            string ID = id;
            var connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"DELETE FROM Dashboards WHERE ID={ID}", conn);
                    var result = cmd.ExecuteNonQuery();
                }
                catch
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Custom export event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ASPxDashboard3_CustomExport(object sender, CustomExportWebEventArgs e)
        {
            var eDocument = e;

            foreach (var printControl in e.GetPrintableControls())
            {
                if (printControl.Value is XRChart)
                {
                    XRControl ctr = printControl.Value;
                    if (printControl.Value is XRChart)
                    {
                        try
                        {
                            var chartItemName = printControl.Key;
                            var chartDashboardItem = e.GetDashboardItem(chartItemName) as ChartDashboardItem;
                            var legend = ((XRChart)ctr).Legend;
                        }
                        catch { }
                    }
                    else if (printControl.Key.StartsWith("grid"))
                    {
                        try
                        {
                            var ItemName = printControl.Key;
                            var chartDashboardItem = e.GetDashboardItem(ItemName) as GridDashboardItem;
                            foreach (var item in chartDashboardItem.Columns)
                            {
                                var deb = item;
                            }
                        }
                        catch { }
                    }
                }
            }
        }

  

        private void ASPxDashboard3_CustomParameters(object sender, CustomParametersWebEventArgs e)
        {
            string group = GetCurrentUserID();
            e.Parameters.Add(new DevExpress.DataAccess.Parameter("ID", typeof(string), group));
        }

        private void ASPxDashboard1_DashboardLoading(object sender, DevExpress.DashboardWeb.DashboardLoadingWebEventArgs e)
        {
            Response.Cookies["dashboard"].Value = e.DashboardId;
            return;
        }

       
        private void ASPxDashboard1_ConfigureDataConnection(object sender, DevExpress.DashboardWeb.ConfigureDataConnectionWebEventArgs e)
        {
            string type_string = e.ConnectionParameters.GetType().Name;

            if (type_string == "MsSqlConnectionParameters")
            {
                ConnectionStringSettings conn = GetConnectionString();
                MsSqlConnectionParameters parameters =
                      (MsSqlConnectionParameters)e.ConnectionParameters;
                MsSqlConnectionParameters msSqlConnection = new MsSqlConnectionParameters();

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conn.ConnectionString);
                msSqlConnection.ServerName = builder.DataSource;
                msSqlConnection.DatabaseName = builder.InitialCatalog;
                msSqlConnection.UserName = builder.UserID;
                msSqlConnection.Password = builder.Password;
                e.ConnectionParameters = msSqlConnection;
            }
            else
            {
                ConnectionStringSettings conn = GetConnectionString();
                CustomStringConnectionParameters parameters =
                        (CustomStringConnectionParameters)e.ConnectionParameters;
                MsSqlConnectionParameters msSqlConnection = new MsSqlConnectionParameters();
                parameters.ConnectionString = conn.ConnectionString;
            }
        }

        private string GetCurrentUserID()
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT id_company FROM Users WHERE uname='{UserNameForChecking}'", conn);
            try
            {
                var result = cmd.ExecuteScalar();
                companyID = System.Convert.ToInt32(result);
            }
            catch (Exception error)
            {
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            var a = GetConnectionStringName(companyID);
            return a;
        }

        private ConnectionStringSettings GetConnectionString()
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT id_company FROM Users WHERE uname='{UserNameForChecking}'", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                companyID = System.Convert.ToInt32(result);
            }
            catch (Exception error)
            {
                // Implement logging here
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            var a = GetConnectionStringName(companyID);
            ConnectionStringSettings stringFinal = ConfigurationManager.ConnectionStrings[a];
            return stringFinal;
        }

        private string GetConnectionStringName(int companyID)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);

            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT database_name FROM companies WHERE id_company={companyID}", conn);

            try
            {
                string result = cmd.ExecuteScalar().ToString();
                returnString = result;
            }
            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return returnString;
        }
    }
}