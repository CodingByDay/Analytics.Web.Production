using Dash.HelperClasses;
using Dash.Log;
using Dash.Models;
using DevExpress.Utils.Behaviors;
using DevExpress.Utils.DragDrop;
using DevExpress.Web;
using DevExpress.Web.Bootstrap;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dash
{
    public partial class TenantAdmin : System.Web.UI.Page
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
        private string admin_id;
        private string databaseName;
        private bool isEditHappening = false;
        private bool isEditUser;
        private string userRightNow;
        private DashboardPermissions dashboardPermissionsGroup = new DashboardPermissions();

        private bool IsEditUser
        {
            get
            {
                // Retrieve the "IsEditUser" session variable from the database
                bool? isEditUser = UserSession.GetSessionVariable<bool?>("IsEditUser");

                // If the value is null, set it to false and save it
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

                // If the value is null, set it to an empty string and save it
                if (string.IsNullOrEmpty(currentUser))
                {
                    currentUser = string.Empty;
                    UserSession.SetSessionVariable("CurrentUser", currentUser);
                }

                return currentUser;
            }
            set
            {
                // Store the new username in the database
                UserSession.SetSessionVariable("CurrentUser", value);
            }
        }


        private string CurrentCompany
        {
            get
            {
                // Retrieve the "CurrentCompany" session variable from the database
                string company = UserSession.GetSessionVariable<string>("CurrentCompany");

                // If the value is null or empty, retrieve the first company and store it in the database
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


                usersGridView.SettingsBehavior.AllowFocusedRow = false;
                usersGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
                usersGridView.SettingsBehavior.AllowSelectByRowClick = true;
                usersGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
                usersGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                usersGridView.EnableCallBacks = false;
                usersGridView.EnableRowsCache = true;

                usersGridView.SelectionChanged += UsersGridView_SelectionChanged;
                usersGridView.DataBound += UsersGridView_DataBound;
                usersGridView.BatchUpdate += UsersGridView_BatchUpdate;
                usersGridView.CustomCallback += UsersGridView_CustomCallback;


                graphsGridView.EnableRowsCache = true;
                graphsGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = false;
                graphsGridView.SettingsBehavior.AllowFocusedRow = true;
                graphsGridView.DataBound += GraphsGridView_DataBound;
                graphsGridView.BatchUpdate += GraphsGridView_BatchUpdate;
                graphsGridView.HtmlRowPrepared += GraphsGridView_HtmlRowPrepared;

                if (!IsPostBack)
                {

                }

                usersGrid.SelectParameters["company_id"].DefaultValue = GetUserCompany();
                GroupsDropdown.SelectParameters["company"].DefaultValue = GetUserCompany();

                InitializeUiChanges();
                Authenticate();
                LimitDashboardsToLocalAdminPermissions();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }



        private void GraphsGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
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

        private void LimitDashboardsToLocalAdminPermissions()
        {
            try
            {
                DashboardPermissions dashboardPermissions = new DashboardPermissions(HttpContext.Current.User.Identity.Name);
                List<int> permittedDashboardIds = dashboardPermissions.Permissions.Select(p => p.id).ToList();
                string filterExpression = $"[id] IN ({string.Join(",", permittedDashboardIds)})";
                // Apply the filter to graphsGridView
                graphsGridView.FilterExpression = filterExpression;
            } catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return;
            }
        }

        private void UsersGridView_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
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
                    query.Update();
                }

                graphsGridView.DataBind();
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

        private void GraphsGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                e.Cancel = true;
                string id = e.Keys["id"] != null ? e.Keys["id"].ToString() : string.Empty;
                query.UpdateParameters["dashboard_id"].DefaultValue = id;
                query.UpdateParameters["company_id"].DefaultValue = GetIdCompany(CurrentCompany).ToString();
                query.UpdateParameters["custom_name"].DefaultValue = e.NewValues["custom_name"] != null ? e.NewValues["custom_name"].ToString() : string.Empty;
                query.Update();
                graphsGridView.DataBind();
                graphsGridView.CancelEdit();
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

        private string GetAllowedDashboardsForAdmin(string name)
        {
            try
            {
                string ids = string.Empty;
                DashboardPermissions dashboardPermissions = new DashboardPermissions(HttpContext.Current.User.Identity.Name);
                for (int i = 0; i < dashboardPermissions.Permissions.Count; i++)
                {
                    var currentPermission = dashboardPermissions.Permissions[i];
                    if (i != dashboardPermissions.Permissions.Count - 1)
                    {
                        ids += currentPermission.id + ",";
                    }
                    else
                    {
                        ids += currentPermission.id;
                    }
                }
                return ids;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

        private string GetUserCompany()
        {
            try
            {
                // Get the username from the current HttpContext
                string username = HttpContext.Current.User.Identity.Name;

                // Connection string from your configuration
                string connectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                // Variable to store the company ID
                string companyId = string.Empty;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        // Create a parameterized SQL query to prevent SQL injection
                        string query = "SELECT id_company FROM users WHERE uname = @uname";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@uname", username);
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                companyId = result.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(TenantAdmin), ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                return companyId;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

   

        /* private void InitializeFilters()
        {
            // Initialize the controls with the empty dataset since no company is selected at the start so its more readable and easier to maintain the codebase.
            // 2.10.2024 Janko Jovičić
            usersGridView.FilterExpression = $"[id_company] = -9999";
            graphsGridView.FilterExpression = 
        }*/

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

        protected void graphsGridView_BeforeHeaderFilterFillItems(object sender, BootstrapGridViewBeforeHeaderFilterFillItemsEventArgs e)
        {
            try
            {
                e.Handled = true;

                List<MetaOption> data = new List<MetaOption>();
                if (e.Column.Caption == "Tip")
                {
                    data = GetFilterValuesForSpecificFilter("type");
                }
                else if (e.Column.Caption == "Jezik")
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

        private void GraphsGridView_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (CurrentUsername != string.Empty)
                {

                    query.SelectParameters["company"].DefaultValue = GetUserCompany().ToString();

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
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }


        private int GetGroupForUser(string uname)
        {
            int groupId = -1;
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
                        if (role == "Admin")
                        {
                            return;
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
                        isEditUser = true;
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
                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
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
                                Notify("Gesla se ne ujemata.", true);
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

                        Notify("Uspešno spremenjeni podatki.", false);
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



        private bool IsUsernameExists(SqlConnection conn, string username)
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
                Notify("Napaka...", true);
            }
            catch (Exception exception)
            {
                SentrySdk.CaptureException(exception);
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
                            Notify("Gesla se ne ujemata.", true);
                            ClearInputs();
                            return;
                        }

                        if (IsUsernameExists(conn, TxtUserName.Text))
                        {
                            Notify("Uporabniško ime že obstaja.", true);
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

                        Notify("Uspešno kreiran uporabnik.", false);
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

        private void UpdateAdminCompany(string admin_value, string cName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();

                        cmd = new SqlCommand($"UPDATE companies SET admin_id='{admin_value}' WHERE company_name='{cName}'", conn);
                        cmd.ExecuteNonQuery();
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
                                usersGridView.DataBind();
                                // Notify success
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uporabnik izbrisan.')", true);
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
                    Logger.LogError(typeof(Admin), errorMessage);
                    // Notify failure
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka pri brisanju uporabnika.')", true);
                }
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
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Morate izbrati uporabnika.')", true);
                }
                else
                {
                    if (HttpContext.Current.User.Identity.Name != CurrentUsername)
                    {
                        SaveUserPermissions();
                    }

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
                if (selectedIds != null && selectedIds.Count > 0)
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
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
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
                usersGridView.Selection.SetSelection(-1, true);
                TxtUserName.Enabled = true;
                email.Enabled = true;
                TxtUserName.Text = "";
                TxtName.Text = "";
                email.Text = "";
                TxtPassword.Text = "";
                TxtRePassword.Text = "";
                isEditUser = false;
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
                isEditUser = false;
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

    
        public bool FilterDashboardComply(List<string> selectedTypes, MetaData dashboardMetaData)
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
                return true;
            }
            catch(Exception ex) 
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

      

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
                            UpdateSortOrder(CurrentUsername, dashboardId, i + 1);
                        }
                        else if (i == focusedRowIndex)
                        {
                            UpdateSortOrder(CurrentUsername, dashboardId, i - 1);
                        }
                    }

                    graphsGridView.FocusedRowIndex = focusedRowIndex - 1;
                    graphsGridView.DataBind();

                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException (ex);
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
                            UpdateSortOrder(CurrentUsername, dashboardId, i - 1);
                        }
                        else if (i == focusedRowIndex)
                        {
                            UpdateSortOrder(CurrentUsername, dashboardId, i + 1);
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

        private void UpdateSortOrder(string uname, int dashboardId, int sortOrder)
        {
            try
            {
                string query = @"
                IF EXISTS (SELECT 1 FROM dashboards_sorted_by_user WHERE dashboard_id = @dashboard_id AND uname = @uname)
                BEGIN
                    UPDATE dashboards_sorted_by_user 
                    SET sort_order = @sort_order 
                    WHERE dashboard_id = @dashboard_id AND uname = @uname;
                END
                ELSE
                BEGIN
                    INSERT INTO dashboards_sorted_by_user (uname, dashboard_id, sort_order) 
                    VALUES (@uname, @dashboard_id, @sort_order);
                END";

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uname", uname);
                        cmd.Parameters.AddWithValue("@dashboard_id", dashboardId);
                        cmd.Parameters.AddWithValue("@sort_order", sortOrder);

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
    }
}