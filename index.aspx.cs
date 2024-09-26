using Dash.DatabaseStorage;
using Dash.Models;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.Cms;
using DevExpress.Web;
using DevExpress.Web.Bootstrap;
using DevExpress.XtraRichEdit.Model;
using Elmah.ContentSyndication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
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
        private MetaData metaData = new MetaData();

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
            HtmlAnchor admin = Master.FindControl("backButtonA") as HtmlAnchor;
            admin.Visible = false;
            var dataBaseDashboardStorage = new DataBaseEditableDashboardStorage(ConnectionString);
            ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);
            ASPxDashboard3.DashboardLoading += ASPxDashboard3_DashboardLoading;
            ASPxDashboard3.Visible = true;
            ASPxDashboard3.WorkingMode = WorkingMode.Viewer;
            ASPxDashboard3.LimitVisibleDataMode = LimitVisibleDataMode.DesignerAndViewer;
            ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;
            ASPxDashboard3.ConfigureDataConnection += ASPxDashboard3_ConfigureDataConnection;
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


        private void LoadMetaDataAndApplySelections(int dashboardId)
        {
            string query = "SELECT meta_data FROM dashboards WHERE id = @dashboardId";
            var connectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@dashboardId", SqlDbType.Int).Value = dashboardId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string jsonMetaData = reader["meta_data"].ToString();
                            metaData = JsonConvert.DeserializeObject<MetaData>(jsonMetaData);
         
                        }
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
                finally
                {

                }
            }

        }

        private void ASPxDashboard3_DashboardLoading(object sender, DashboardLoadingWebEventArgs e)
        {
            Session["current"] = e.DashboardId.ToString();

            var currentDashboardId = Session["current"];
            long parsed;
            if (!String.IsNullOrEmpty((string)currentDashboardId) && Int64.TryParse((string) currentDashboardId, out parsed))
            {
                 LoadMetaDataAndApplySelections((int)parsed);

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
                Response.Redirect("Logon.aspx", true);
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
        private List<string> GetSelectedValues(BootstrapGridView gridView, string columnName)
        {
            var selectedValues = new List<string>();

            foreach (string row in gridView.GetSelectedFieldValues("ID"))
            {
                // Assuming you have a way to get the data based on selected IDs
                // You need to find the corresponding row in the grid view to get its value for the specified column
                var dataRow = gridView.GetRowValues(row, columnName);
                if (dataRow != null)
                {
                    selectedValues.Add(dataRow.ToString());
                }
            }

            return selectedValues;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedTypes = TypeGroup.Items.Cast<ListItem>()
                     .Where(item => item.Selected)
                     .Select(item => item.Value).ToList();

                var selectedCompanies = CompanyGroup.Items.Cast<ListItem>()
                    .Where(item => item.Selected)
                    .Select(item => item.Value).ToList();

                var selectedLanguages = LanguageGroup.Items.Cast<ListItem>()
                    .Where(item => item.Selected)
                    .Select(item => item.Value).ToList();


                // Create MetaData object and assign selected values
                MetaData metaData = new MetaData
                {
                    Types = selectedTypes,
                    Languages = selectedLanguages,
                    Companies = selectedCompanies
                };

                // Serialize MetaData to JSON
                string jsonMetaData = JsonConvert.SerializeObject(metaData);

                var connectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Prepare the SQL command to update the meta_data column
                    if (Session["current"] != null && int.TryParse(Session["current"].ToString(), out int dashboardId))
                    {
                        // Prepare the SQL command to update the meta_data column
                        string updateQuery = "UPDATE dashboards SET meta_data = @metaData WHERE id = @dashboardId";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            // Add parameters to the command
                            command.Parameters.Add("@metaData", SqlDbType.NVarChar).Value = jsonMetaData;
                            command.Parameters.Add("@dashboardId", SqlDbType.Int).Value = dashboardId;

                            // Execute the update command
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showNotificationDevexpress('Uspešno dodani meta podatki'); };", true);

                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showNotificationDevexpress('Napaka pri dodajanju meta podatkov'); };", true);
                            }
                        }

                    }
                }
            }
            catch(Exception ex) 
            {
                return;
            }
        }




    }
}           