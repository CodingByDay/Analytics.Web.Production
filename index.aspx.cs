using Dash.DatabaseStorage;
using Dash.Models;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using DevExpress.Web;
using DevExpress.Web.Bootstrap;
using DevExpress.Web.Internal;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using MetaData = Dash.Models.MetaData;

namespace Dash
{
    public partial class Index : System.Web.UI.Page
    {
        public static string ConnectionString;

        private List<String> strings = new List<string>();
        private string state;
        private SqlConnection conn;
        private SqlCommand cmd;
        private string role;
        private static int permisionID;


        protected override void InitializeCulture()
        {
            // Check if the language cookie exists
            HttpCookie langCookie = HttpContext.Current.Request.Cookies["Language"];

            if (langCookie != null && !string.IsNullOrEmpty(langCookie.Value))
            {
                // Get the language code from the cookie
                string lang = langCookie.Value;

                // Set the culture and UI culture
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            }
            else
            {
                // Optional: Set a default language if no cookie is found
                string defaultLang = "sl"; // Default to English
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(defaultLang);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(defaultLang);
            }

            // Call the base method to ensure other initializations are performed
            base.InitializeCulture();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Session["current"] as string))
                {
                    ASPxDashboard3.InitialDashboardId = Session["current"].ToString();
                }

                Authenticate();
                ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());
                ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
                // Hide the back button.

                var dataBaseDashboardStorage = new DataBaseEditableDashboardStorage(ConnectionString);
                ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);
                ASPxDashboard3.DashboardLoading += ASPxDashboard3_DashboardLoading;
                ASPxDashboard3.Visible = true;
                ASPxDashboard3.WorkingMode = WorkingMode.Viewer;
                ASPxDashboard3.LimitVisibleDataMode = LimitVisibleDataMode.DesignerAndViewer;
                ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;
                ASPxDashboard3.DataRequestOptions.ItemDataRequestMode = ItemDataRequestMode.BatchRequests;
                if (!IsPostBack)
                {
                    ASPxDashboard3.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());
                    ASPxDashboard3.WorkingMode = WorkingMode.Viewer;
                    HtmlInputCheckBox toggle = (HtmlInputCheckBox)Master.FindControl("togglebox");
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
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void ASPxDashboard3_DashboardLoading(object sender, DashboardLoadingWebEventArgs e)
        {
            try
            {
                Response.Cookies["dashboard"].Value = e.DashboardId;

                Session["current"] = e.DashboardId.ToString();

            } catch (Exception ex)
            {
                SentrySdk.CaptureException (ex);
                return;
            }
        }

        private void Authenticate()
        {
            try
            {
                var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var username = HttpContext.Current.User.Identity.Name;
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"SELECT user_role FROM users WHERE uname='{username}';", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    role = (reader["user_role"].ToString());
                }
                if (role == "SuperAdmin")
                {
                }
                else
                {
                    Response.Redirect("Logon.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

       

        private ConnectionStringSettings GetConnectionString(string name)
        {
            try
            {
                ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings[name];
                return connection;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return new ConnectionStringSettings();
            }
        }

     

   

    }
}