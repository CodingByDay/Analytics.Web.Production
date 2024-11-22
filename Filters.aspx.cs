using Dash.Log;
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
                SetLocalizationProperties();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected override void InitializeCulture()
        {
            try
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
                    Response.Cookies["Language"].Value = defaultLang;
                    Response.Cookies["Language"].Expires = DateTime.Now.AddYears(1); // Set expiry
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(defaultLang);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(defaultLang);
                }

                // Call the base method to ensure other initializations are performed
                base.InitializeCulture();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void SetLocalizationProperties()
        {
            try
            {
                gridViewTypes.Columns["Možnosti"].Caption = Resources.Resource.Actions;
                gridViewTypes.Columns["Vrednost"].Caption = Resources.Resource.Value;
                gridViewTypes.Columns["Opis"].Caption = Resources.Resource.Description;
                gridViewTypes.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchCompany;
                gridViewTypes.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                gridViewTypes.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;
                gridViewTypes.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchGraph;
                gridViewTypes.SettingsText.CommandCancel = Resources.Resource.Cancel;
                gridViewTypes.SettingsText.CommandUpdate = Resources.Resource.Update;

                gridViewOrganizations.Columns["Možnosti"].Caption = Resources.Resource.Actions;
                gridViewOrganizations.Columns["Vrednost"].Caption = Resources.Resource.Value;
                gridViewOrganizations.Columns["Opis"].Caption = Resources.Resource.Description;
                gridViewOrganizations.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchUser;
                gridViewOrganizations.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                gridViewOrganizations.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;
                gridViewOrganizations.SettingsText.CommandCancel = Resources.Resource.Cancel;
                gridViewOrganizations.SettingsText.CommandUpdate = Resources.Resource.Update;

                gridViewLanguages.Columns["Možnosti"].Caption = Resources.Resource.Actions;
                gridViewLanguages.Columns["Vrednost"].Caption = Resources.Resource.Value;
                gridViewLanguages.Columns["Opis"].Caption = Resources.Resource.Description;
                gridViewLanguages.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                gridViewLanguages.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;
                gridViewLanguages.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchGraph;
                gridViewLanguages.SettingsText.CommandCancel = Resources.Resource.Cancel;
                gridViewLanguages.SettingsText.CommandUpdate = Resources.Resource.Update;
            }
            catch (Exception err)
            {
                SentrySdk.CaptureException(err);
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
                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
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