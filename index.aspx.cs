using Dash.DatabaseStorage;
using Dash.Models;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using DevExpress.Web;
using DevExpress.Web.Bootstrap;
using DevExpress.Web.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using MetaData = Dash.Models.MetaData;

namespace Dash
{
    public partial class index : System.Web.UI.Page
    {
        public static string ConnectionString;

        private List<String> strings = new List<string>();
        private string state;
        private SqlConnection conn;
        private SqlCommand cmd;
        private string role;
        private static int permisionID;


      

        protected void Page_Load(object sender, EventArgs e)
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
            // ASPxDashboard3.ConfigureDataConnection += ASPxDashboard3_ConfigureDataConnection;
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
                }
            }
        }

        private void ASPxDashboard3_DashboardLoading(object sender, DashboardLoadingWebEventArgs e)
        {
            try
            {
                Response.Cookies["dashboard"].Value = e.DashboardId;

                Session["current"] = e.DashboardId.ToString();

            } catch (Exception)
            {
                return;
            }
        }

        private void Authenticate()
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

        private void ASPxDashboard3_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
        {
            string TARGET_URL = "https://dash.in-sist.si";
            if (Session != null)
            {
                if (System.Web.HttpContext.Current.Session["conn"] != null)
                {
                    if (Session["conn"].ToString() != "")
                    {
                        string test = e.ConnectionName;
                        ConnectionStringSettings conn = GetConnectionString();
                        CustomStringConnectionParameters parameters =
                        (CustomStringConnectionParameters)e.ConnectionParameters;

                        parameters.ConnectionString = conn.ConnectionString;
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
        }

        private ConnectionStringSettings GetConnectionString()
        {
            var ConnectionName = Session["conn"].ToString();
            ConnectionStringSettings stringFinal = ConfigurationManager.ConnectionStrings[ConnectionName];
            return stringFinal;
        }

     

   

    }
}