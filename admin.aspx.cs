﻿using Dash.HelperClasses;
using Dash.Log;
using Dash.Models;
using DevExpress.Web;
using DevExpress.Web.Bootstrap;
using Sentry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;

namespace Dash
{
    public partial class Admin : System.Web.UI.Page
    {
        private string connection;
        private BootstrapButton btnFilterBootstrap;
        private List<string> byUserList = new List<string>();
        private List<bool> valuesBool = new List<bool>();
        private List<String> columnNames = new List<string>();
        private List<bool> config = new List<bool>();
        private List<String> fileNames = new List<string>();
        private List<String> graphNames = new List<string>();
        private List<String> values = new List<string>();
        private List<String> graphList = new List<string>();
        private SqlConnection conn;
        private string findIdString;
        private string permisionQuery;
        private string findId;
        private object tempGraphString;
        private string finalQuerys;
        private string find;
        private SqlCommand cmd;
        private object idNumber;
        private List<String> companiesData = new List<string>();
        private object id;
        private int flag;
        private string companyInfo;
        private List<String> companies = new List<string>();
        private List<String> strings = new List<string>();
        private List<String> admins = new List<string>();
        private int permisionID;
        private string deletedID;
        private string current;
        private object result;
        private List<String> help = new List<string>();
        private List<String> usersDataByUser = new List<string>();
        private Exception e;
        private int idFromString;
        private string role;
        private List<User> usersList = new List<User>();
        private string HashedPasswordEdit;
        private SqlCommand cmdEdit;
        private string returnString;
        private string company_name;
        private string company_number;
        private string websiteCompany;
        private string databaseName;

        private string userRightNow;
        private DashboardPermissions dashboardPermissionsGroup = new DashboardPermissions();

        private bool IsEditCompany
        {
            get
            {
                // Retrieve the "IsEditCompany" session variable from the database
                bool? isEditCompany = UserSession.GetSessionVariable<bool?>("IsEditCompany");

                // If the value is null (not set), default to false and store it in the database
                if (isEditCompany == null)
                {
                    isEditCompany = false;
                    UserSession.SetSessionVariable("IsEditCompany", isEditCompany);
                }

                return isEditCompany.Value;
            }
            set
            {
                // Store the new value in the database
                UserSession.SetSessionVariable("IsEditCompany", value);
            }
        }

        private bool IsEditUser
        {
            get
            {
                // Retrieve the "IsEditUser" session variable from the database
                bool? isEditUser = UserSession.GetSessionVariable<bool?>("IsEditUser");

                // If the value is null (not set), default to false and store it in the database
                if (isEditUser == null)
                {
                    isEditUser = false;
                    UserSession.SetSessionVariable("IsEditUser", isEditUser);
                }

                return isEditUser.Value;
            }
            set
            {
                // Store the new value in the database
                UserSession.SetSessionVariable("IsEditUser", value);
            }
        }

        private string CurrentUsername
        {
            get
            {
                // Retrieve the "CurrentUser" session variable from the database
                string currentUser = UserSession.GetSessionVariable<string>("CurrentUser");

                // If the value is null (not set), default to an empty string and store it in the database
                if (string.IsNullOrEmpty(currentUser))
                {
                    currentUser = string.Empty;
                    UserSession.SetSessionVariable("CurrentUser", currentUser);
                }

                return currentUser;
            }
            set
            {
                // Store the new value in the database
                UserSession.SetSessionVariable("CurrentUser", value);
            }
        }

        private string CurrentCompany
        {
            get
            {
                // Retrieve the "CurrentCompany" session variable from the database
                string company = UserSession.GetSessionVariable<string>("CurrentCompany");

                // If the company is null or empty, retrieve the first company and store it in the database
                if (string.IsNullOrEmpty(company))
                {
                    company = GetFirstCompany();
                    UserSession.SetSessionVariable("CurrentCompany", company);
                }

                return company;
            }
            set
            {
                // Store the new company in the database
                UserSession.SetSessionVariable("CurrentCompany", value);
            }
        }

        private string GetFirstCompany()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand($"SELECT TOP (1) company_name FROM companies;", conn);
                        var company = (string)cmd.ExecuteScalar();
                        return company;
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                companiesGridView.SettingsBehavior.AllowFocusedRow = false;
                companiesGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
                companiesGridView.SettingsBehavior.AllowSelectByRowClick = true;
                companiesGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
                companiesGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                companiesGridView.EnableCallBacks = false;
                companiesGridView.EnableRowsCache = true;

                companiesGridView.SelectionChanged += CompaniesGridView_SelectionChanged;
                companiesGridView.StartRowEditing += CompaniesGridView_StartRowEditing;
                companiesGridView.DataBound += CompaniesGridView_DataBound;

                usersGridView.SettingsBehavior.AllowFocusedRow = false;
                usersGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
                usersGridView.SettingsBehavior.AllowSelectByRowClick = true;
                usersGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
                usersGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                usersGridView.EnableCallBacks = false;
                usersGridView.EnableRowsCache = true;
                usersGridView.CustomCallback += UsersGridView_CustomCallback;
                usersGridView.BatchUpdate += UsersGridView_BatchUpdate;
                usersGridView.SelectionChanged += UsersGridView_SelectionChanged;
                usersGridView.DataBound += UsersGridView_DataBound;

                graphsGridView.EnableRowsCache = true;
                graphsGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = false;
                graphsGridView.SettingsBehavior.AllowFocusedRow = true;
                graphsGridView.DataBound += GraphsGridView_DataBound;
                graphsGridView.RowValidating += GraphsGridView_RowValidating;
                graphsGridView.BatchUpdate += GraphsGridView_BatchUpdate;

                graphsGridView.HtmlRowPrepared += GraphsGridView_HtmlRowPrepared;

                if (!IsPostBack)
                {
                }

                InitializeUiChanges();
                Authenticate();
                SetLocalizationProperties();

                companiesGridView.SettingsCommandButton.EditButton.IconCssClass = "fas fa-edit";
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void GraphsGridView_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            var debug = true;
        }

        private void SetLocalizationProperties()
        {
            try
            {
                deleteCompany.Text = Resources.Resource.Delete;
                deleteUser.Text = Resources.Resource.Delete;
                saveGraphs.Text = Resources.Resource.Save;

                companiesGridView.Columns["Podjetje"].Caption = Resources.Resource.Company;
                companiesGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchCompany;
                companiesGridView.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                companiesGridView.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;
                companiesGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchGraph;

                usersGridView.Columns["Uporabniško ime"].Caption = Resources.Resource.Username;
                usersGridView.Columns["Skupina"].Caption = Resources.Resource.Group;
                usersGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchUser;
                usersGridView.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                usersGridView.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;

                graphsGridView.Columns["Naziv"].Caption = Resources.Resource.Name;
                graphsGridView.Columns["Interni naziv"].Caption = Resources.Resource.InternalName;
                graphsGridView.Columns["Tip"].Caption = Resources.Resource.Type;
                graphsGridView.Columns["Podjetje"].Caption = Resources.Resource.Company;
                graphsGridView.Columns["Jezik"].Caption = Resources.Resource.Language;
                graphsGridView.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                graphsGridView.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;
                graphsGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchGraph;
            }
            catch (Exception err)
            {
                SentrySdk.CaptureException(err);
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

        private void UsersGridView_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                e.Handled = true;

                foreach (var row in e.UpdateValues)
                {
                    usersGrid.UpdateParameters["group"].DefaultValue = row.NewValues["group_name"] != null ? row.NewValues["group_name"].ToString() : string.Empty;
                    usersGrid.UpdateParameters["uname"].DefaultValue = CurrentUsername;
                    usersGrid.Update();
                }

                usersGridView.DataBind();

                if (graphsGridView.VisibleRowCount > 0 && !String.IsNullOrEmpty(CurrentUsername))
                {
                    ShowConfigForUser();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void UsersGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                var key = e.Parameters;
                usersGridView.Selection.SelectRowByKey(key);
                IsEditUser = true;
                TxtUserName.Enabled = false;
                UpdateFormName(key.ToString());
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncUser(); };", true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void GraphsGridView_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                e.Handled = true;

                foreach (var row in e.UpdateValues)
                {
                    string rowId = row.Keys["id"].ToString();

                    query.UpdateParameters["dashboard_id"].DefaultValue = rowId;
                    query.UpdateParameters["company_id"].DefaultValue = GetIdCompany(CurrentCompany).ToString();
                    query.UpdateParameters["custom_name"].DefaultValue = row.NewValues["custom_name"] != null ? row.NewValues["custom_name"].ToString() : string.Empty;

                    // Ensure DBNull.Value is passed for nulls
                    query.UpdateParameters["meta_type"].DefaultValue = row.NewValues["meta_type"] != null
                        ? GetIdForMetaDescription("type", row.NewValues["meta_type"].ToString())
                        : DBNull.Value.ToString();

                    query.UpdateParameters["meta_company"].DefaultValue = row.NewValues["meta_company"] != null
                        ? GetIdForMetaDescription("company", row.NewValues["meta_company"].ToString())
                        : DBNull.Value.ToString();

                    query.UpdateParameters["meta_language"].DefaultValue = row.NewValues["meta_language"] != null
                        ? GetIdForMetaDescription("language", row.NewValues["meta_language"].ToString())
                        : DBNull.Value.ToString();

                    query.UpdateParameters["sort"].DefaultValue = row.NewValues["sort"] != null
                        ? row.NewValues["sort"].ToString()
                        : DBNull.Value.ToString();

                    query.Update();
                }

                graphsGridView.DataBind();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private string GetIdForMetaDescription(string type, string description)
        {
            try
            {
                // Corrected SQL query with both conditions combined using AND
                string query = "SELECT id FROM meta_options WHERE option_type = @type AND description = @description";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);

                    // Adding both parameters to the SQL command
                    command.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                    command.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;

                    // Execute the query and retrieve the id
                    object result = command.ExecuteScalar();

                    // If result is not null, return it as string, otherwise return null
                    return result != null ? result.ToString() : null;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
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

        private void UsersGridView_DataBound(object sender, EventArgs e)
        {
            try
            {
                usersGridView.Selection.SetSelectionByKey(CurrentUsername, true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void GraphsGridView_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (CurrentUsername != string.Empty)
                {
                    query.SelectParameters["company"].DefaultValue = GetIdCompany(CurrentCompany).ToString();

                    if (graphsGridView.VisibleRowCount > 0)
                    {
                        graphsGridView.Selection.BeginSelection();
                        ShowConfigForUser();
                        graphsGridView.Selection.EndSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void CompaniesGridView_DataBound(object sender, EventArgs e)
        {
            try
            {
                companiesGridView.Selection.SetSelectionByKey(GetIdCompany(CurrentCompany), true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void CompaniesGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            try
            {
                var key = e.EditingKeyValue;
                companiesGridView.Selection.SelectRowByKey(key);

                IsEditCompany = true;
                TxtUserName.Enabled = false;
                var name = e.EditingKeyValue;
                UpdateFormCompany(name.ToString());

                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncCompany(); };", true);
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void UpdateFormCompany(string v)
        {
            try
            {
                // Select * from companies where id_company={}
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();

                        var username = HttpContext.Current.User.Identity.Name;
                        // Create SqlCommand to select pwd field from users table given supplied userName.
                        cmd = new SqlCommand($"SELECT * FROM companies WHERE id_company={v};", conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            company_name = (reader["company_name"].ToString());
                            company_number = (reader["company_number"].ToString());
                            websiteCompany = (reader["website"].ToString());
                            databaseName = (reader["database_name"].ToString());
                        }

                        companyName.Text = company_name;
                        companyNumber.Text = company_number;
                        website.Text = websiteCompany;

                        var connectionDB = ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;

                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionDB);
                        dbDataSource.Text = builder.DataSource;
                        dbNameInstance.Text = builder.InitialCatalog;
                        dbUser.Text = builder.UserID;
                        dbPassword.Text = builder.Password;
                        connName.Text = databaseName.ToString();
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

        private void CompaniesGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        var plurals = companiesGridView.GetSelectedFieldValues("id_company");
                        if (plurals.Count != 0)
                        {
                            var id = (int)plurals[0];

                            CurrentCompany = GetCompanyName(id);
                            GroupsDropdown.SelectParameters["company"].DefaultValue = id.ToString();

                            // Apply the filter to the userGridView based on the selected id_company 30.09.2024 Janko Jovičić
                            usersGridView.FilterExpression = $"[id_company] = {id}";
                            usersGridView.DataBind();  // Refresh the userGridView with the applied filter
                            graphsGridView.DataBind();
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

        private void UsersGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var NamePlural = usersGridView.GetSelectedFieldValues("uname");
                if (NamePlural.Count == 0)
                {
                    return;
                }
                else
                {
                    var selectedName = NamePlural[0].ToString();
                    CurrentUsername = selectedName;
                    graphsGridView.DataBind();
                }

                TxtUserName.Enabled = false;
                email.Enabled = false;
                graphsGridView.Enabled = true;

                // UpdateForm();

                if (graphsGridView.VisibleRowCount > 0 && !String.IsNullOrEmpty(CurrentUsername))
                {
                    // Show the configuration for the user.
                    ShowConfigForUser();
                }

                usersGridView.StartEdit(usersGridView.FindVisibleIndexByKeyValue(CurrentUsername));
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private int GetGroupForUser(string uname)
        {
            int groupId = -1;
            try
            {
                string query = "SELECT group_id FROM users WHERE uname = @uname";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@uname", uname);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            groupId = (int)result;
                        }
                    }
                    catch (Exception ex)
                    {
                        SentrySdk.CaptureException(ex);
                    }
                }

                return groupId;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return -1;
            }
        }

        private void ShowConfigForUser()
        {
            try
            {
                // Group dashboards should be checked and disabled first. 14.11.2024
                dashboardPermissionsGroup = new DashboardPermissions(GetGroupForUser(CurrentUsername));
                for (int i = 0; i < graphsGridView.VisibleRowCount; i++)
                {
                    int idRow = (int)graphsGridView.GetRowValues(i, "id");
                    if (dashboardPermissionsGroup.Permissions.Any(x => x.id == idRow))
                    {
                        graphsGridView.Selection.SetSelection(i, true);
                    }
                    else
                    {
                        graphsGridView.Selection.SetSelection(i, false);
                    }
                }

                DashboardPermissions dashboardPermissions = new DashboardPermissions(CurrentUsername);
                for (int i = 0; i < graphsGridView.VisibleRowCount; i++)
                {
                    int idRow = (int)graphsGridView.GetRowValues(i, "id");
                    if (dashboardPermissions.Permissions.Any(x => x.id == idRow))
                    {
                        graphsGridView.Selection.SetSelection(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void GraphsGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.KeyValue == null)
                {
                    return;
                }

                if (dashboardPermissionsGroup.Permissions.Any(x => x.id == (int)e.KeyValue))
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                }
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
                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        public string GetCompanyName(int company)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        string uname = HttpContext.Current.User.Identity.Name;
                        cmd = new SqlCommand($"SELECT company_name FROM companies WHERE id_company={company}", conn);
                        var admin = (string)cmd.ExecuteScalar();
                        cmd.Dispose();
                        return admin;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(Admin), ex.InnerException.Message);
                        return String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

        private void UpdateFormName(string name)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand($"SELECT * FROM users WHERE uname='{name}'", conn);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            TxtName.Text = sdr["full_name"].ToString();
                            TxtUserName.Text = sdr["uname"].ToString();
                            TxtUserName.Enabled = false;
                            referrer.Text = sdr["referrer"].ToString();
                            var number = (int)sdr["id_company"];
                            var data = GetCompanyName(number);
                            email.Enabled = false;
                            string role = sdr["user_role"].ToString();
                            string type = sdr["view_allowed"].ToString();
                            email.Text = sdr["email"].ToString();
                            userRole.SelectedIndex = userRole.Items.IndexOf(userRole.Items.FindByValue(role));
                            userTypeList.SelectedIndex = userTypeList.Items.IndexOf(userTypeList.Items.FindByValue(type));
                        }
                        sdr.Close();
                        cmd.Dispose();
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

        protected void RegistrationButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsEditUser)
                {
                    UpdateUser();
                }
                else
                {
                    InsertUser();
                }

                // Call DataBind at the end
                usersGridView.DataBind();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void InsertUser()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        if (TxtPassword.Text != TxtRePassword.Text)
                        {
                            Notify("Passwords are not the same.", true);
                            ClearInputs();
                            return;
                        }

                        if (DoesUsernameExists(conn, TxtUserName.Text))
                        {
                            Notify("Username already exists.", true);
                            return;
                        }

                        string hashedPassword = HashPassword(TxtPassword.Text);
                        int idCompany = GetIdCompany(CurrentCompany);
                        string query = @"INSERT INTO users(uname, password, user_role, id_company, view_allowed, full_name, email, referrer)
                             VALUES (@uname, @password, @user_role, @id_company, @view_allowed, @full_name, @Email, @referrer)";

                        using (SqlCommand createUser = new SqlCommand(query, conn))
                        {
                            createUser.Parameters.AddWithValue("@uname", TxtUserName.Text);
                            createUser.Parameters.AddWithValue("@password", hashedPassword);
                            createUser.Parameters.AddWithValue("@user_role", userRole.SelectedValue);
                            createUser.Parameters.AddWithValue("@id_company", idCompany);
                            createUser.Parameters.AddWithValue("@view_allowed", userTypeList.SelectedValue);
                            createUser.Parameters.AddWithValue("@full_name", TxtName.Text);
                            createUser.Parameters.AddWithValue("@Email", email.Text);
                            createUser.Parameters.AddWithValue("@referrer", referrer.Text);

                            createUser.ExecuteNonQuery();
                        }

                        ClearInputs();
                    }
                    catch (Exception ex)
                    {
                        LogErrorAndNotify(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void UpdateUser()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        string query;

                        if (!string.IsNullOrEmpty(TxtPassword.Text))
                        {
                            if (TxtPassword.Text == TxtRePassword.Text)
                            {
                                string hashedPassword = HashPassword(TxtPassword.Text);
                                query = @"UPDATE users SET password = @password, user_role = @user_role, view_allowed = @view_allowed,
                          full_name = @full_name, referrer = @referrer WHERE uname = @uname";

                                using (SqlCommand cmdEdit = new SqlCommand(query, conn))
                                {
                                    cmdEdit.Parameters.AddWithValue("@password", hashedPassword);
                                    cmdEdit.Parameters.AddWithValue("@user_role", userRole.SelectedValue);
                                    cmdEdit.Parameters.AddWithValue("@view_allowed", userTypeList.SelectedValue);
                                    cmdEdit.Parameters.AddWithValue("@full_name", TxtName.Text);
                                    cmdEdit.Parameters.AddWithValue("@referrer", referrer.Text);
                                    cmdEdit.Parameters.AddWithValue("@uname", TxtUserName.Text);

                                    cmdEdit.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                Notify("Passwords are not the same.", true);
                                return;
                            }
                        }
                        else
                        {
                            query = @"UPDATE users SET user_role = @user_role, view_allowed = @view_allowed,
                          referrer = @referrer, full_name = @full_name WHERE uname = @uname";

                            using (SqlCommand cmdEdit = new SqlCommand(query, conn))
                            {
                                cmdEdit.Parameters.AddWithValue("@user_role", userRole.SelectedValue);
                                cmdEdit.Parameters.AddWithValue("@view_allowed", userTypeList.SelectedValue);
                                cmdEdit.Parameters.AddWithValue("@referrer", referrer.Text);
                                cmdEdit.Parameters.AddWithValue("@full_name", TxtName.Text);
                                cmdEdit.Parameters.AddWithValue("@uname", TxtUserName.Text);

                                cmdEdit.ExecuteNonQuery();
                            }
                        }

                        ClearInputs();
                    }
                    catch (Exception ex)
                    {
                        LogErrorAndNotify(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private bool DoesUsernameExists(SqlConnection conn, string username)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE uname = @uname";
                using (SqlCommand cmdCheck = new SqlCommand(query, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@uname", username);
                    return (int)cmdCheck.ExecuteScalar() > 0;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            try
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

        private void ClearInputs()
        {
            try
            {
                TxtName.Text = "";
                TxtPassword.Text = "";
                TxtRePassword.Text = "";
                TxtUserName.Text = "";
                email.Text = "";
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void Notify(string message, bool isError)
        {
            try
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", $"notify({isError.ToString().ToLower()}, '{message}')", true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void LogErrorAndNotify(Exception ex)
        {
            try
            {
                Logger.LogError(typeof(Admin), ex.InnerException?.Message ?? ex.Message);
                Notify("Error", true);
            }
            catch (Exception exception)
            {
                SentrySdk.CaptureException(exception);
            }
        }

        private int InsertCompany()
        {
            int newCompanyId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();

                    using (SqlCommand insertCmd = new SqlCommand(
                        "INSERT INTO companies (id_company, company_name, company_number, website, database_name) " +
                        "VALUES (@Id, @Name, @Number, @Website, @DbName);", conn))
                    {
                        // Manually assign a value for id_company if needed
                        // You can calculate the next ID or insert a value directly
                        int nextId = GetNextCompanyId(conn); // You need to implement GetNextCompanyId to get the next ID

                        insertCmd.Parameters.AddWithValue("@Id", nextId);
                        insertCmd.Parameters.AddWithValue("@Name", companyName.Text);
                        insertCmd.Parameters.AddWithValue("@Number", companyNumber.Text);
                        insertCmd.Parameters.AddWithValue("@Website", website.Text);
                        insertCmd.Parameters.AddWithValue("@DbName", connName.Text);

                        int affected = insertCmd.ExecuteNonQuery();
                        // Execute the query and get the new ID
                        if (affected == 1)
                        {
                            newCompanyId = nextId;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
            }

            return newCompanyId;
        }

        private int GetNextCompanyId(SqlConnection conn)
        {
            try
            {
                // Assuming the ID column is either NULL or has a value
                using (SqlCommand cmd = new SqlCommand("SELECT MAX(id_company) FROM companies", conn))
                {
                    var result = cmd.ExecuteScalar();

                    // Check if the result is DBNull and return the next ID (e.g., 1 if no rows exist)
                    if (result == DBNull.Value)
                    {
                        return 1; // If there are no companies, return 1 as the next ID
                    }

                    // Otherwise, return the next ID
                    return Convert.ToInt32(result) + 1;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return 1;
            }
        }

        protected void CompanyButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsEditCompany)
                {
                    var keyCompany = InsertCompany();

                    companiesGridView.DataBind();

                    companiesGridView.Selection.SelectRowByKey(keyCompany);

                    CreateOrModifyConnectionString(dbDataSource.Text, dbNameInstance.Text, dbPassword.Text, dbUser.Text, connName.Text);


                    companyNumber.Text = "";
                    companyName.Text = "";
                    website.Text = "";

                }
                else
                {
                    UpdateCompanyDataForm(companyName.Text, companyNumber.Text, website.Text);
                    UpdateCompanyData(connName.Text, GetIdCompany(CurrentCompany));
                    CreateOrModifyConnectionString(dbDataSource.Text, dbNameInstance.Text, dbPassword.Text, dbUser.Text, connName.Text);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }



        public void UpdateCompanyDataForm(string name, string number, string website)
        {
            
            const string query = @"
            UPDATE companies 
            SET company_name = @name, 
                company_number = @number, 
                website = @website 
            WHERE id_company = @id";

            try
            {
                using (var conn = new SqlConnection(connection))
                {
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@number", number);
                        command.Parameters.AddWithValue("@website", website);
                        command.Parameters.AddWithValue("@id", GetIdCompany(CurrentCompany));

                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }





        private void AddOrUpdateConnectionString(string stringConnection)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                var builder = new SqlConnectionStringBuilder(stringConnection);

                string connectionName = connName.Text;

                // Check if the connection string already exists
                ConnectionStringSettings existingConn = config.ConnectionStrings.ConnectionStrings[connectionName];

                if (existingConn != null)
                {
                    // Modify the existing connection string
                    existingConn.ConnectionString = builder.ConnectionString;
                }
                else
                {
                    // Create a new connection string if it doesn't exist
                    ConnectionStringSettings conn = new ConnectionStringSettings
                    {
                        ConnectionString = builder.ConnectionString,
                        Name = connectionName
                    };
                    config.ConnectionStrings.ConnectionStrings.Add(conn);
                }

                // Save the configuration changes
                config.Save(ConfigurationSaveMode.Modified, true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                // Log the full exception message
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
            }
        }

        private void CreateOrModifyConnectionString(string dbSource, string dbNameInstance, string dbPassword, string dbUser, string connName)
        {
            try
            {
                if (connName != string.Empty)
                {
                    SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
                    build.InitialCatalog = dbNameInstance;
                    build.DataSource = dbSource;
                    build.UserID = dbUser;
                    build.Password = dbPassword;
                    AddOrUpdateConnectionString(build.ConnectionString);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void UpdateCompanyData(string dbName, int idCompany)
        {
            try
            {
                // Define the SQL query with parameters
                string query = "UPDATE companies SET database_name = @db_name WHERE id_company = @id_company";

                // Create a new SQL connection
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    // Open the connection
                    conn.Open();

                    // Create the SQL command with the query and connection
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        // Add the parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@db_name", dbName);
                        command.Parameters.AddWithValue("@id_company", idCompany);

                        // Execute the update command
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Capture the exception with Sentry for logging and debugging
                SentrySdk.CaptureException(ex);
            }
        }

        protected void DeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                string username = null;
                try
                {
                    var selectedValues = usersGridView.GetSelectedFieldValues("uname");
                    // Ensure a user is selected
                    if (selectedValues != null && selectedValues.Count > 0)
                    {
                        username = selectedValues[0]?.ToString();
                        if (!string.IsNullOrEmpty(username))
                        {
                            using (SqlConnection conn = new SqlConnection(connection))
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE uname = @uname", conn))
                            {
                                cmd.Parameters.AddWithValue("@uname", username);
                                conn.Open();
                                cmd.ExecuteNonQuery();

                                CurrentUsername = GetFirstUserForCompany(CurrentCompany);

                                usersGridView.DataBind();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.InnerException?.Message ?? ex.Message;
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private string GetFirstUserForCompany(string currentCompany)
        {
            // Get the company ID based on the current company name
            int companyId = GetIdCompany(currentCompany);
            // Define the query to get the first user for the company
            string query = "SELECT TOP 1 uname FROM users WHERE id_company = @company_id";
            // Initialize the result variable
            string firstUser = string.Empty;
            // Use the global connection string for the operation
            using (SqlConnection conn = new SqlConnection(connection))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Add the parameter for the query
                cmd.Parameters.AddWithValue("@company_id", companyId);
                // Open the connection
                conn.Open();
                // Execute the query and read the results
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Retrieve the value of the uname column
                        firstUser = reader["uname"] as string;
                    }
                }
            }
            return firstUser; 
        }


        [WebMethod]
        public static void CreatingCompanySessionEdit()
        {
            try
            {
                UserSession.SetSessionVariable("IsEditCompany", false);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private string GetCompanyQuery(string uname)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        // Create SqlCommand to select pwd field from users table given supplied userName.
                        cmd = new SqlCommand($"SELECT uname, company_name FROM users INNER JOIN companies ON users.id_company = companies.id_company WHERE uname='{uname}';", conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            companyInfo = (reader["company_name"].ToString());
                        }
                        var final = companyInfo.Replace(" ", string.Empty);
                        return final;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(Admin), ex.InnerException.Message);
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

        protected void SaveGraphs_Click(object sender, EventArgs e)
        {
            try
            {
                if (usersGridView.GetSelectedFieldValues() == null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'You need to choose the user.')", true);
                }
                else
                {
                    SaveUserPermissions();
                    graphsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void SaveUserPermissions()
        {
            try
            {
                DashboardPermissions permissions = new DashboardPermissions();
                var selectedIds = graphsGridView.GetSelectedFieldValues("id");
                if (selectedIds != null)
                {
                    for (int i = 0; i < selectedIds.Count; i++)
                    {
                        string currentSelectedId = selectedIds[i].ToString();
                        long parsed;
                        if (Int64.TryParse(currentSelectedId, out parsed))
                        {
                            permissions.Permissions.Add(new DashboardPermission { id = (int)parsed });
                        }
                    }

                    permissions.SetPermissionsForUser(CurrentUsername);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                var d = ex;
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
            }
        }

        protected void DeleteCompany_Click(object sender, EventArgs e)
        {
            try
            {
                var current = CurrentCompany;
                int companyId;

                if (string.IsNullOrEmpty(current))
                {
                    return;
                }
                else
                {
                    companyId = GetIdCompany(current);
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        conn.Open();

                        using (SqlCommand companyCmd = new SqlCommand("DELETE FROM companies WHERE id_company = @id_company;", conn))
                        {
                            companyCmd.Parameters.AddWithValue("@id_company", companyId);
                            companyCmd.ExecuteNonQuery();
                        }

                        RemoveConnectionString(current);

                        CurrentCompany = GetFirstCompany();

                        // Refresh the grid
                        companiesGridView.DataBind();
                        usersGridView.DataBind();
                        graphsGridView.DataBind();

                    }
                }
                catch (SqlException ex)
                {
                    SentrySdk.CaptureException(ex);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Error')", true);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void RemoveConnectionString(string current)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();

                        // Use parameterized query to prevent SQL injection
                        using (SqlCommand cmd = new SqlCommand("SELECT database_name FROM companies WHERE company_name = @CompanyName", conn))
                        {
                            cmd.Parameters.AddWithValue("@CompanyName", current);
                            var result = cmd.ExecuteScalar();

                            // Ensure result is valid
                            if (result != null)
                            {
                                string databaseName = result.ToString();

                                // Modify the web.config and remove the connection string
                                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                                ConnectionStringSettings connectionString = config.ConnectionStrings.ConnectionStrings[databaseName];

                                if (connectionString != null)
                                {
                                    config.ConnectionStrings.ConnectionStrings.Remove(databaseName);
                                    config.Save(ConfigurationSaveMode.Modified, true);

                                    // Refresh the section to reflect changes immediately
                                    ConfigurationManager.RefreshSection("connectionStrings");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the full exception message and stack trace
                        Logger.LogError(typeof(Admin), $"Error removing connection string: {ex.Message}, StackTrace: {ex.StackTrace}");
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private int GetIdCompany(string current)
        {
            try
            {
                if (current != null)
                {
                    string spaceless = current.Trim();
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand($"SELECT id_company FROM companies WHERE company_name='{spaceless}'", conn);
                            result = cmd.ExecuteScalar();
                            int id = System.Convert.ToInt32(result);
                            return id;
                        }
                        catch (Exception)
                        {
                            return -1;
                        }
                    }
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return -1;
            }
        }

        protected void NewUser_Click(object sender, EventArgs e)
        {
            try
            {
                IsEditUser = false;
                TxtUserName.Enabled = true;
                email.Enabled = true;
                TxtUserName.Text = "";
                TxtName.Text = "";
                email.Text = "";
                TxtPassword.Text = "";
                TxtRePassword.Text = "";
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncUser(); };", true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void NewUserClick(object sender, EventArgs e)
        {
            try
            {
                TxtUserName.Enabled = true;
                email.Enabled = true;
                TxtUserName.Text = "";
                TxtName.Text = "";
                email.Text = "";
                TxtPassword.Text = "";
                TxtRePassword.Text = "";
                // Call the client.
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "showDialogSync()", true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private List<string> GetSelectedValues(BootstrapGridView gridView, string columnName)
        {
            try
            {
                var selectedValues = new List<string>();
                foreach (string row in gridView.GetSelectedFieldValues(columnName))
                {
                    selectedValues.Add(row);
                }
                return selectedValues;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return new List<string>();
            }
        }

        public bool FilterDashboardComply(List<string> selectedTypes, List<string> selectedCompanies, List<string> selectedLanguages, MetaData dashboardMetaData)
        {
            try
            {
                foreach (string item in selectedTypes)
                {
                    if (!dashboardMetaData.Types.Contains(item))
                    {
                        return false;
                    }
                }
                foreach (string item in selectedCompanies)
                {
                    if (!dashboardMetaData.Companies.Contains(item))
                    {
                        return false;
                    }
                }
                foreach (string item in selectedLanguages)
                {
                    if (!dashboardMetaData.Languages.Contains(item))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

       /* 
        public void MoveUpButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the index of the focused row
                int focusedRowIndex = graphsGridView.FocusedRowIndex;
                int beforeFocusedRowIndex = focusedRowIndex - 1;
                // Get the DataRow for the focused row
                DataRow focusedRowData = graphsGridView.GetDataRow(focusedRowIndex);

                // Retrieve the id from the DataRow
                if (focusedRowData != null)
                {
                    for (int i = 0; i < graphsGridView.VisibleRowCount; i++)
                    {
                        DataRow dr = graphsGridView.GetDataRow(i);
                        int dashboardId = (int)dr["id"];
                        if (i == beforeFocusedRowIndex)
                        {
                            UpdateSortOrder(dashboardId, i + 1);
                        }
                        else if (i == focusedRowIndex)
                        {
                            UpdateSortOrder(dashboardId, i - 1);
                        }
                    }

                    graphsGridView.FocusedRowIndex = focusedRowIndex - 1;
                    graphsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return;
            }
        }

        protected void MoveDownButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the index of the focused row
                int focusedRowIndex = graphsGridView.FocusedRowIndex;
                int afterFocusedRowIndex = focusedRowIndex + 1;
                // Get the DataRow for the focused row
                DataRow focusedRowData = graphsGridView.GetDataRow(focusedRowIndex);
                // Retrieve the id from the DataRow
                if (focusedRowData != null)
                {
                    for (int i = 0; i < graphsGridView.VisibleRowCount; i++)
                    {
                        DataRow dr = graphsGridView.GetDataRow(i);
                        int dashboardId = (int)dr["id"];
                        if (i == afterFocusedRowIndex)
                        {
                            UpdateSortOrder(dashboardId, i - 1);
                        }
                        else if (i == focusedRowIndex)
                        {
                            UpdateSortOrder(dashboardId, i + 1);
                        }
                    }

                    graphsGridView.FocusedRowIndex = focusedRowIndex + 1;
                    graphsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return;
            }
        }
        */

        private void UpdateSortOrder(int dashboardId, int sortOrder)
        {
            try
            {
                string query = @"UPDATE dashboards SET sort = @sort WHERE id = @id;";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", dashboardId);
                        cmd.Parameters.AddWithValue("@sort", sortOrder);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void graphsGridView_BeforeHeaderFilterFillItems(object sender, BootstrapGridViewBeforeHeaderFilterFillItemsEventArgs e)
        {
            try
            {
                e.Handled = true;

                List<MetaOption> data = new List<MetaOption>();
                if (e.Column.Caption == "Tip" || e.Column.Caption == "Type")
                {
                    data = GetFilterValuesForSpecificFilter("type");
                }
                else if (e.Column.Caption == "Podjetje" || e.Column.Caption == "Company" || e.Column.Caption == "Firma")
                {
                    data = GetFilterValuesForSpecificFilter("company");
                }
                else if (e.Column.Caption == "Jezik" || e.Column.Caption == "Language")
                {
                    data = GetFilterValuesForSpecificFilter("language");
                }

                e.Values.Clear();

                for (int i = 0; i < data.Count; i++)
                {
                    e.AddValue(displayText: data[i].description, value: data[i].value);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private List<MetaOption> GetFilterValuesForSpecificFilter(string filter)
        {
            try
            {
                List<MetaOption> filterValues = new List<MetaOption>();

                // Define your connection string (replace with actual connection string)

                // Define the query
                string query = "SELECT description, value FROM meta_options WHERE option_type = @filter";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        // Add parameter to prevent SQL injection
                        command.Parameters.AddWithValue("@filter", filter);

                        // Open the connection
                        conn.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Read each result and add to the list
                            while (reader.Read())
                            {
                                filterValues.Add(new MetaOption
                                {
                                    description = reader["description"].ToString(),
                                    value = reader["description"].ToString()
                                });
                            }
                        }
                    }
                }

                return filterValues;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return new List<MetaOption>();
            }
        }
    }
}