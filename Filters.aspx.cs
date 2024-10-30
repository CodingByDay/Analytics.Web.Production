using Dash.Log;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace Dash
{
    public partial class Filters : System.Web.UI.Page
    {
        private string connection;
        private SqlCommand cmd;
        private string role;

        protected void Page_Load(object sender, EventArgs e)
        {
            connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            gridViewTypes.CustomJSProperties += GridViewTypes_CustomJSProperties;
            gridViewOrganizations.CustomJSProperties += GridViewOrganizations_CustomJSProperties;
            gridViewLanguages.CustomJSProperties += GridViewLanguages_CustomJSProperties;

            if (!IsPostBack)
            {
           
            }

            InitializeUiChanges();
            Authenticate();
        }


        private void Authenticate()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var username = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"SELECT user_role FROM users WHERE uname='{username}';", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        role = (reader["user_role"].ToString());
                    }
                    cmd.Dispose();
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
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }


        private void InitializeUiChanges()
        {
            SiteMaster mymaster = Master as SiteMaster;
            mymaster.BackButtonVisible = true;
        }
        private void GridViewLanguages_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties.Add("cpTotalRows", gridViewLanguages.VisibleRowCount);

        }

        private void GridViewOrganizations_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties.Add("cpTotalRows", gridViewOrganizations.VisibleRowCount);
        }

        private void GridViewTypes_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties.Add("cpTotalRows", gridViewTypes.VisibleRowCount);
        }


    }






}