using Dash.DatabaseStorage;
using Dash.DataExtract;
using Dash.ORM;
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
using System.Web.UI.HtmlControls;

namespace Dash
{
    public partial class indextenant : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
          
            connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            if (Request.Cookies["dashboard"] != null)
            {
                bool isAllowed = isUserOk(Request.Cookies["dashboard"].Value.ToString());
                if (isAllowed)
                {
                    ASPxDashboard3.InitialDashboardId = Request.Cookies["dashboard"].Value.ToString();
                }
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
        private int getIdPermision()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{HttpContext.Current.User.Identity.Name}'", conn);
                    var result = cmd.ExecuteScalar();
                    permisionID = System.Convert.ToInt32(result);
                    return permisionID;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                    return -1;
                }
            }


        }
        private bool isUserOk(string id)
        {
            string ID = id;
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"select Caption from Dashboards where ID = {id};", conn); /// Intepolation or the F string. C# > 5.0       
                    // Execute command and fetch pwd field into lookupPassword string.
                    string ok = (string) cmd.ExecuteScalar();
                    if(ok!=string.Empty)
                    {
                        int id_user = getIdPermision();
                        var allowed = new SqlCommand($"select {ok} from permisions_user where id_permisions_user = {id_user};", conn);
                        int isOk = (int) allowed.ExecuteScalar();
                        var stop = true;
                        if(isOk==1)
                        {
                            return true;
                        } else
                        {
                            return false;
                        }
                    } else
                    {
                        return false;
                    }                    
                }
                catch (Exception ex)
                {
                    return false;
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

        private string GetProperName(string name)
        {
            try
            {
                var list = Request.Cookies["params"].Value;
                var des = JsonConvert.DeserializeObject<List<Parameter>>(list);
                var no = des;
                return no.ToString();
            }
            catch (Exception ex)
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
            
            Response.Cookies["dashboard"].Value = e.DashboardId;
            Session["current"] = e.DashboardId;
        }

        private bool checkDB(string ID)
        {
            bool flag = false;
            string UserNameForChecking = HttpContext.Current.User.Identity.Name;
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
            var ed = e.ConnectionParameters;
            string type_string = e.ConnectionParameters.GetType().Name;
            // MsSqlConnectionParameters
          
                if (type_string == "MsSqlConnectionParameters") {

                        ConnectionStringSettings conn = GetConnectionString();
                        MsSqlConnectionParameters parameters =
                              (MsSqlConnectionParameters) e.ConnectionParameters;
                        MsSqlConnectionParameters msSqlConnection = new MsSqlConnectionParameters();
                        
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conn.ConnectionString);
                        msSqlConnection.ServerName = builder.DataSource;
                        msSqlConnection.DatabaseName = builder.InitialCatalog;
                        msSqlConnection.UserName = builder.UserID;
                        msSqlConnection.Password = builder.Password;
                        e.ConnectionParameters = msSqlConnection;
                
                                    
               } else
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