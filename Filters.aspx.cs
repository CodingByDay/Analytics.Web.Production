using Dash.Log;
using DevExpress.XtraRichEdit.Model;
using Sentry;
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
            try
            {
                connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                gridViewTypes.CustomJSProperties += GridViewTypes_CustomJSProperties;
                gridViewOrganizations.CustomJSProperties += GridViewOrganizations_CustomJSProperties;
                gridViewLanguages.CustomJSProperties += GridViewLanguages_CustomJSProperties;

                gridViewTypes.SettingsCommandButton.EditButton.IconCssClass = "fas fa-edit";
                gridViewTypes.SettingsCommandButton.DeleteButton.IconCssClass = "fa fa-trash";
                gridViewOrganizations.SettingsCommandButton.EditButton.IconCssClass = "fas fa-edit";
                gridViewOrganizations.SettingsCommandButton.DeleteButton.IconCssClass = "fa fa-trash";
                gridViewLanguages.SettingsCommandButton.EditButton.IconCssClass = "fas fa-edit";
                gridViewLanguages.SettingsCommandButton.DeleteButton.IconCssClass = "fa fa-trash";

                if (!IsPostBack)
                {

                }

                InitializeUiChanges();
                Authenticate();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }


        private void Authenticate()
        {
            try
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }


        private void InitializeUiChanges()
        {
            try
            {
                SiteMaster mymaster = Master as SiteMaster;
                mymaster.BackButtonVisible = true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
        private void GridViewLanguages_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                e.Properties.Add("cpTotalRows", gridViewLanguages.VisibleRowCount);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void GridViewOrganizations_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                e.Properties.Add("cpTotalRows", gridViewOrganizations.VisibleRowCount);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }


        private void GridViewTypes_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                e.Properties.Add("cpTotalRows", gridViewTypes.VisibleRowCount);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }


    }






}