using Dash.DatabaseStorage;
using Dash.ORM;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Dash
{
    public partial class indextenant : System.Web.UI.Page
    {
        public static string ConnectionString;
        private SqlConnection conn;
        private int companyID;
        private int stringID;
        private string stringConnection;
        private int value;
        private bool flag = false;
        private string state;
        private string returnString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session["current"] as string))
            {
                ASPxDashboard3.InitialDashboardId = Session["current"].ToString();
            }
            HtmlAnchor admin = Master.FindControl("backButtonA") as HtmlAnchor;
            admin.Visible = false;
            ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            ASPxDashboard3.LimitVisibleDataMode = LimitVisibleDataMode.DesignerAndViewer;
            ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());
            var dataBaseDashboardStorage = new DataBaseEditableDashboardStorageCustom(ConnectionString);
            ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);
            ASPxDashboard3.ConfigureDataConnection += ASPxDashboard1_ConfigureDataConnection;
            ASPxDashboard3.AllowCreateNewDashboard = true;
            ASPxDashboard3.DashboardLoading += ASPxDashboard1_DashboardLoading;
            ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;
            ASPxDashboard3.DataRequestOptions.ItemDataRequestMode = ItemDataRequestMode.BatchRequests;
            ASPxDashboard3.CustomParameters += ASPxDashboard3_CustomParameters;
            string TARGET_URL = "https://dash.in-sist.si";
            if (Session != null)
            {
                if (System.Web.HttpContext.Current.Session["UserAllowed"] != null)
                {
                    if (Session["UserAllowed"].ToString() == "true")
                    {
                        ASPxDashboard3.WorkingMode = WorkingMode.Viewer;
                    }
                    else
                    {
                        ASPxDashboard3.WorkingMode = WorkingMode.ViewerOnly;

                    }
                }
                else
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
                }
            }
            else
            {
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            }
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
            ASPxDashboard3.CustomExport += ASPxDashboard3_CustomExport;
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
                XRControl ctr = printControl.Value;
                if (printControl.Value is XRChart)
                {
                    try
                    {
                        var chartItemName = printControl.Key;
                        var chartDashboardItem = e.GetDashboardItem(chartItemName) as ChartDashboardItem;
                       
                        foreach (var series in chartDashboardItem.Panes)
                        {
                            if (series.Series.Count > 0)
                            {
                                foreach (var dashSeries in series.Series)
                                {
                                    var controlSeries = e.GetChartContext(chartItemName).GetControlSeries(dashSeries);
                                    foreach (var ser in controlSeries)
                                    {
                                        ser.Visible = false;
                                    }
                                }
                            }
                        }
                    } catch { }
                } else if(printControl.Key.StartsWith("grid"))
                {
                    try
                    {
                        var ItemName = printControl.Key;
                        var chartDashboardItem = e.GetDashboardItem(ItemName) as GridDashboardItem;
                        foreach (var item in chartDashboardItem.Columns)
                        {
                            var deb = item;
                            var stop = true;
                        
                            
                        }

                    } catch { }
                }
            }
        }

        private string GetProperName(string name)
        {
            try
            {
                var list = Request.Cookies["params"].Value;
                var des = JsonConvert.DeserializeObject<List<Parameter>>(list);
                var no = des;
                return no.ToString();


            }
            catch(Exception ex)
            {
                var err = ex.InnerException;
                return default(string);
            }
        }

        private void ASPxDashboard3_CustomParameters(object sender, CustomParametersWebEventArgs e)
        {
            string group = getCurrentUserID();
            e.Parameters.Add(new DevExpress.DataAccess.Parameter("ID", typeof(string), group));
        }
       
        private void ASPxDashboard1_DashboardLoading(object sender, DevExpress.DashboardWeb.DashboardLoadingWebEventArgs e)
        {
            Session["current"] = e.DashboardId;
        }

        private bool checkDB(string ID)
        {
            bool flag = false; 

            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select isViewerOnly from Dashboards where ID={ID}", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                value = System.Convert.ToInt32(result);
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


            if (value == 1)
            {
                flag = true;
            }

            else
            {
                flag = false;
            }

            return flag;
        }

        private void ASPxDashboard1_ConfigureDataConnection(object sender, DevExpress.DashboardWeb.ConfigureDataConnectionWebEventArgs e)
        {
            if (e.ConnectionParameters is DevExpress.DataAccess.ConnectionParameters.CustomStringConnectionParameters)
            {
                ConnectionStringSettings conn = GetConnectionString();
                CustomStringConnectionParameters parameters =
                      (CustomStringConnectionParameters)e.ConnectionParameters;
                MsSqlConnectionParameters msSqlConnection = new MsSqlConnectionParameters();
                parameters.ConnectionString = conn.ConnectionString;

            }
        }



        private string getCurrentUserID()
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_company from Users where uname='{UserNameForChecking}'", conn);
            try
            {
                var result = cmd.ExecuteScalar();
                companyID = System.Convert.ToInt32(result);
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
            var a = get_connectionStringName(companyID);
            return a;
        }

        private ConnectionStringSettings GetConnectionString()
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_company from Users where uname='{UserNameForChecking}'", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                companyID = System.Convert.ToInt32(result);

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

            var a = get_connectionStringName(companyID);



            ConnectionStringSettings stringFinal = ConfigurationManager.ConnectionStrings[a];

            return stringFinal;

        }

        private string get_connectionStringName(int companyID)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);

            conn.Open();
            SqlCommand cmd = new SqlCommand($"select databaseName from companies where id_company={companyID}", conn);

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int get_id_string(int id)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);

            conn.Open();
            SqlCommand cmd = new SqlCommand($"select ID from companies where id_company={id}", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                stringID = System.Convert.ToInt32(result);

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
            return stringID;
        }



        public string get_conn_name(int id)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select string from company_string where ID={id}", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                stringConnection = result.ToString().Replace(" ", string.Empty);

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
            return stringConnection;
        }
    }
}