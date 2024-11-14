using Dash.DatabaseStorage;
using Dash.Log;
using Dash.Models;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using static Dash.Models.DashboardFilters;

namespace Dash
{
    public partial class IndexTenant : System.Web.UI.Page
    {
        private string connection;
        private SqlConnection conn;
        private int companyId;
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
            try
            {
                connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                dataBaseDashboardStorage = new DataBaseEditableDashboardStorageCustom(connection);

                if (Request.Cookies["dashboard"] != null)
                {
                    // New OOP structure 23.09.2024
                    if (   // User or group permissions.
                           dataBaseDashboardStorage.permissionsUser.DashboardWithIdAllowed(Request.Cookies["dashboard"].Value.ToString())
                        || dataBaseDashboardStorage.permissionsGroup.DashboardWithIdAllowed(Request.Cookies["dashboard"].Value.ToString()
                        ))
                    {
                        ASPxDashboard3.InitialDashboardId = Request.Cookies["dashboard"].Value.ToString();
                    }
                }

                string visibleDataMode = GetWorkingModeForUser(HttpContext.Current.User.Identity.Name);

                if (visibleDataMode == "Viewer & Designer")
                {
                    ASPxDashboard3.WorkingMode = WorkingMode.Viewer;
                }
                else
                {
                    ASPxDashboard3.WorkingMode = WorkingMode.ViewerOnly;
                }

                ASPxDashboard3.LimitVisibleDataMode = LimitVisibleDataMode.DesignerAndViewer;
                ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());
                ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);
                ASPxDashboard3.ConfigureDataConnection += ASPxDashboard1_ConfigureDataConnection;
                ASPxDashboard3.AllowCreateNewDashboard = true;
                ASPxDashboard3.DashboardLoading += ASPxDashboard1_DashboardLoading;
                ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;
                ASPxDashboard3.DataRequestOptions.ItemDataRequestMode = ItemDataRequestMode.BatchRequests;
                ASPxDashboard3.CustomExport += ASPxDashboard3_CustomExport;
                ASPxDashboard3.SetInitialDashboardState += ASPxDashboard3_SetInitialDashboardState;
                SetUpPage();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private string GetWorkingModeForUser(string name)
        {
            // Placeholder to store the result
            string workingMode = string.Empty;
            try
            {
                // Define the SQL query with a parameter
                string query = "SELECT view_allowed FROM users WHERE uname = @name";

                // Create a new SQL connection
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    // Open the connection
                    conn.Open();

                    // Create the SQL command with the query and connection
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        // Add the parameter to prevent SQL injection
                        command.Parameters.AddWithValue("@name", name);

                        // Execute the query and retrieve the result
                        var result = command.ExecuteScalar();

                        // If the result is not null, convert it to a string
                        if (result != null)
                        {
                            workingMode = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Capture the exception with Sentry for logging and debugging
                SentrySdk.CaptureException(ex);
            }
            // Return the working mode, or an empty string if not found
            return workingMode;
        }


        private void ASPxDashboard3_SetInitialDashboardState(object sender, SetInitialDashboardStateEventArgs e)
        {
            try
            {
                var dashboardId = e.DashboardId;
                UserDashboardState userDashboardStates = new UserDashboardState(HttpContext.Current.User.Identity.Name);
                var userStates = userDashboardStates.GetInitialStateForTheUser(dashboardId);
                LogDashboardView(HttpContext.Current.User.Identity.Name, e.DashboardId);
                if (userStates.State != null)
                {
                    e.InitialState = userStates.State;
                }
            } catch(Exception ex)
            {
                SentrySdk.CaptureException (ex);
                Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
            }
        }

  

        private void SetUpPage()
        {
            try
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        [WebMethod]
        public static void DeleteItem(string id)
        {
            try
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
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        /// <summary>
        /// Custom export event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ASPxDashboard3_CustomExport(object sender, CustomExportWebEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        [WebMethod]
        public static void ProcessStateChanged(string state, string dashboard)
        {
            try
            {
                DashboardState stateObject = new DashboardState();

                if (state != string.Empty)
                {
                    stateObject.LoadFromJson(state);
                }
                UserDashboardState userDashboardStates = new UserDashboardState(HttpContext.Current.User.Identity.Name);
                userDashboardStates.UpdateStates(dashboard, stateObject);
                userDashboardStates.SetStatesForUser();                            
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException (ex);
                Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
            }

        }


        private void ASPxDashboard3_CustomParameters(object sender, CustomParametersWebEventArgs e)
        {
            try
            {
                string group = GetCurrentUserID();
                e.Parameters.Add(new DevExpress.DataAccess.Parameter("ID", typeof(string), group));
            } catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
            }
        }

        private void ASPxDashboard1_DashboardLoading(object sender, DevExpress.DashboardWeb.DashboardLoadingWebEventArgs e)
        {
            try
            {
                Response.Cookies["dashboard"].Value = e.DashboardId;
                return;
            } catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
            }
        }



        public void LogDashboardView(string userId, string dashboardId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connection))
                {
                    connection.Open();

                    string query = @"
                    INSERT INTO dashboard_view_logs (user_id, dashboard_id, view_time)
                    VALUES (@userId, @dashboardId, GETDATE());";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@dashboardId", dashboardId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }


        private void ASPxDashboard1_ConfigureDataConnection(object sender, DevExpress.DashboardWeb.ConfigureDataConnectionWebEventArgs e)
        {
            try
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
            } catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
            }
        }

        private string GetCurrentUserID()
        {
            try
            {
                string userNameForChecking = HttpContext.Current.User.Identity.Name; // Get the current username
                string connectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                // Use 'using' statement to ensure resources are disposed properly
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Use parameterized query to avoid SQL injection
                    using (var cmd = new SqlCommand("SELECT id_company FROM Users WHERE uname = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", userNameForChecking);
                        try
                        {
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                int companyId = Convert.ToInt32(result);
                                return GetConnectionStringName(companyId);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
                            return string.Empty; // Return null or an appropriate value if an error occurs
                        }
                    }
                }
                return string.Empty;
            } catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
                return string.Empty;
            }
        }

        private ConnectionStringSettings GetConnectionString()
        {
            try
            {
                string uname = HttpContext.Current.User.Identity.Name;
                try
                {
                    using (var conn = new SqlConnection(connection))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("SELECT id_company FROM Users WHERE uname = @uname", conn))
                        {
                            cmd.Parameters.AddWithValue("@uname", uname);
                            var result = cmd.ExecuteScalar();
                            companyId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
                }
                string connectionName = GetConnectionStringName(companyId);
                return ConfigurationManager.ConnectionStrings[connectionName];
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return new ConnectionStringSettings();
            }
        }


        private string GetConnectionStringName(int companyID)
        {
            try
            {
                string result = string.Empty;
                try
                {
                    using (var conn = new SqlConnection(connection))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("SELECT database_name FROM companies WHERE id_company = @companyId", conn))
                        {
                            cmd.Parameters.AddWithValue("@companyId", companyID);

                            var queryResult = cmd.ExecuteScalar();
                            if (queryResult != null)
                            {
                                result = queryResult.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(IndexTenant), ex.InnerException.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }
    }
}