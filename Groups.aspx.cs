using Dash.HelperClasses;
using Dash.Log;
using Dash.Models;
using DevExpress.Data.Helpers;
using DevExpress.Web.Bootstrap;
using DevExpress.XtraRichEdit.Commands;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
namespace Dash
{
    public partial class Groups : System.Web.UI.Page
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
        private bool isEditHappening = false;
        private bool isEditUser;
        private string userRightNow;



        private bool GroupEdit
        {
            get
            {
                // Retrieve the "GroupEdit" session variable from the database
                bool? groupEdit = UserSession.GetSessionVariable<bool?>("GroupEdit");

                // If the value is null, set it to false and save it
                if (groupEdit == null)
                {
                    groupEdit = false;
                    UserSession.SetSessionVariable("GroupEdit", groupEdit);
                }

                return groupEdit.Value;
            }
            set
            {
                // Store the new value in the database
                UserSession.SetSessionVariable("GroupEdit", value);
            }
        }



        private string CurrentGroup
        {
            get
            {
                // Retrieve the "CurrentGroup" session variable from the database
                string currentGroup = UserSession.GetSessionVariable<string>("CurrentGroup");

                // If the value is null or empty, set it to an empty string and save it
                if (string.IsNullOrEmpty(currentGroup))
                {
                    currentGroup = string.Empty;
                    UserSession.SetSessionVariable("CurrentGroup", currentGroup);
                }

                return currentGroup;
            }
            set
            {
                // Store the new group in the database
                UserSession.SetSessionVariable("CurrentGroup", value);
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
                    string type = GetUserType();
                    if (!string.IsNullOrEmpty(type))
                    {
                        if (type == "SuperAdmin")
                        {
                            company = GetFirstCompany();
                            UserSession.SetSessionVariable("CurrentCompany", company);
                        } else if (type == "Admin")
                        {
                            company = GetCompanyName(GetCompanyIdForUser(HttpContext.Current.User.Identity.Name));
                            UserSession.SetSessionVariable("CurrentCompany", company);
                        }
                    }

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
                else if (e.Column.Caption == "Podjetje")
                {
                    data = GetFilterValuesForSpecificFilter("company");
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

                groupsGridView.SettingsBehavior.AllowFocusedRow = false;
                groupsGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
                groupsGridView.SettingsBehavior.AllowSelectByRowClick = true;
                groupsGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
                groupsGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                groupsGridView.EnableCallBacks = false;
                groupsGridView.EnableRowsCache = true;

                groupsGridView.StartRowEditing += groupsGridView_StartRowEditing;
                groupsGridView.SelectionChanged += groupsGridView_SelectionChanged;
                groupsGridView.DataBound += groupsGridView_DataBound;

                graphsGridView.EnableRowsCache = true;
                graphsGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                graphsGridView.DataBound += GraphsGridView_DataBound;



                if (!IsPostBack)
                {
                }


                InitializeUiChanges();
                Authenticate();
                LimitCompanyGrid();
                HideColumnForCompanies();
                LimitDashboardsPermissions();
                SetLocalizationProperties();

                groupsGridView.SettingsCommandButton.EditButton.IconCssClass = "fas fa-edit";

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

                DeleteGroup.Text = Resources.Resource.Delete;
                saveGraphs.Text = Resources.Resource.Save;

                companiesGridView.Columns["Podjetje"].Caption = Resources.Resource.Company;
                companiesGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchCompany;
                companiesGridView.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                companiesGridView.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;
                companiesGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchCompany;

                groupsGridView.Columns["Naziv"].Caption = Resources.Resource.Username;
                groupsGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchGroups;

                graphsGridView.Columns["Naziv"].Caption = Resources.Resource.Name;
                graphsGridView.Columns["Tip"].Caption = Resources.Resource.InternalName;
                graphsGridView.Columns["Podjetje"].Caption = Resources.Resource.Type;
                graphsGridView.Columns["Jezik"].Caption = Resources.Resource.Company;
                graphsGridView.SettingsText.HeaderFilterCancelButton = Resources.Resource.Cancel;
                graphsGridView.SettingsText.HeaderFilterSelectAll = Resources.Resource.SelectAll;
                graphsGridView.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchGraph;


                usersInGroupGrid.Columns["Uporabnik"].Caption = Resources.Resource.User;
                usersInGroupGrid.Columns["Ime in priimek"].Caption = Resources.Resource.FullName;
                usersInGroupGrid.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchUser;

                usersNotInGroupGrid.Columns["Uporabnik"].Caption = Resources.Resource.User;
                usersNotInGroupGrid.Columns["Ime in priimek"].Caption = Resources.Resource.FullName;
                usersNotInGroupGrid.SettingsText.SearchPanelEditorNullText = Resources.Resource.SearchUser;


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
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(defaultLang);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(defaultLang);
                }

                // Call the base method to ensure other initializations are performed
                base.InitializeCulture();
            } catch(Exception err)
            {
                SentrySdk.CaptureException(err);
            }
        }
        private void LimitDashboardsPermissions()
        {
            try
            {
                DashboardPermissions dashboardPermissionsUser = new DashboardPermissions(HttpContext.Current.User.Identity.Name);
        
                List<int> permittedDashboardIds = dashboardPermissionsUser.Permissions.Select(p => p.id).ToList();
                string filterExpression = $"[id] IN ({string.Join(",", permittedDashboardIds)})";
                // Apply the filter to graphsGridView
                string type = GetUserType();
                if (!string.IsNullOrEmpty(type))
                {
                    if (type == "Admin")
                    {
                        graphsGridView.FilterExpression = filterExpression;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        private void HideColumnForCompanies()
        {
            try
            {
                string type = GetUserType();
                if (!string.IsNullOrEmpty(type))
                {
                    // For all companies set the default values to -1 06.11.2024 Janko Jovičić
                    if (type == "SuperAdmin")
                    {
                        graphsGridView.Columns[4].Visible = true;
                    }
                    else if (type == "Admin")
                    {
                        graphsGridView.Columns[4].Visible = false;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void LimitCompanyGrid()
        {
            try
            {
                string type = GetUserType();
                if (!string.IsNullOrEmpty(type))
                {
                    // For all companies set the default values to -1 06.11.2024 Janko Jovičić
                    if (type == "SuperAdmin")
                    {
                        companiesGrid.SelectParameters["company_id"].DefaultValue = "-1";
                    }
                    else if (type == "Admin")
                    {
                        companiesGrid.SelectParameters["company_id"].DefaultValue = GetCompanyIdForUser(HttpContext.Current.User.Identity.Name).ToString();

                    }
                }
            } catch (Exception)
            {
                return;
            }
        }
        public int GetCompanyIdForUser(string uname)
        {
            try
            {
                int companyId = -1; // Default to -1 or any invalid value if the user is not found

                // Define the SQL query to get the company_id for a specific user
                string query = "SELECT id_company FROM users WHERE uname = @uname";

                // Set up the connection and command
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["graphsConnectionString"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@uname", uname); // Use parameterized query to prevent SQL injection

                    try
                    {
                        connection.Open();
                        object result = cmd.ExecuteScalar(); // Execute the query and get the first column of the first row

                        if (result != null && result != DBNull.Value)
                        {
                            companyId = Convert.ToInt32(result); // Convert the result to an integer
                        }
                    }
                    catch (Exception ex)
                    {
                        SentrySdk.CaptureException(ex);
                    }
                }

                return companyId; // Return the company ID, or -1 if not found
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return -1;
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
        /*private void InitializeFilters()
        {
            // Initialize the controls with the empty dataset since no company is selected at the start so its more readable and easier to maintain the codebase.
            // 2.10.2024 Janko Jovičić
            groupsGridView.FilterExpression = $"[id_company] = -9999";
            graphsGridView.FilterExpression = 
        }*/

        private void groupsGridView_DataBound(object sender, EventArgs e)
        {
            try
            {
                groupsGridView.Selection.SetSelectionByKey(GetIdGroup(CurrentGroup), true);
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
                if (graphsGridView.VisibleRowCount > 0)
                {
                    graphsGridView.Selection.BeginSelection();
                    ShowConfigForUser();
                    graphsGridView.Selection.EndSelection();
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
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncCompany(); };", true);
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
                            groupsGridView.FilterExpression = $"[company_id] = {id}";
                            groupsGridView.DataBind();
                            graphsGridView.DataBind();
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

        private string GetFirstGroupForCompany(string company)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        int id_company = GetIdCompany(company);
                        conn.Open();
                        SqlCommand cmd = new SqlCommand($"SELECT TOP (1) group_name FROM groups WHERE company_id = @id;", conn);
                        cmd.Parameters.AddWithValue("@id", id_company);
                        var group = (string)cmd.ExecuteScalar();
                        return group;
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

        private void groupsGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            try
            {
                if(e == null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncGroup(); };", true);
                    return;
                }

                var key = e.EditingKeyValue;

                groupsGridView.Selection.SelectRowByKey(key);

                GroupEdit = true;
                GroupsGrids.Visible = true;

                // In case of the programmatic call. 3.10.2024 Janko Jovičić
                if (e != null)
                {
                    e.Cancel = true;
                }

                int companyParameter = GetIdCompany(CurrentCompany);
                int groupParameter = GetIdGroup(CurrentGroup);

                // Assign values to SqlDataSource parameters
                UsersInGroupDataSource.SelectParameters["id_company"].DefaultValue = companyParameter.ToString();
                UsersInGroupDataSource.SelectParameters["group_id"].DefaultValue = groupParameter.ToString();

                UsersNotInGroupDataSource.SelectParameters["id_company"].DefaultValue = companyParameter.ToString();
                UsersNotInGroupDataSource.SelectParameters["group_id"].DefaultValue = groupParameter.ToString();


                var result = GetDataForGroupById(groupParameter);

                groupName.Text = result.groupName;
                groupDescription.Text = result.groupDescription;

                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncGroup(); };", true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void groupsGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var NamePlural = groupsGridView.GetSelectedFieldValues("group_name");
                if (NamePlural.Count == 0)
                {
                    return;
                }
                else
                {
                    var selectedName = NamePlural[0].ToString();
                    CurrentGroup = selectedName;
                    graphsGridView.DataBind();
                }
                if (graphsGridView.VisibleRowCount > 0 && !String.IsNullOrEmpty(CurrentGroup))
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

        private void ShowConfigForUser()
        {
            try
            {
                DashboardPermissions dashboardPermissions = new DashboardPermissions(GetIdGroup(CurrentGroup));
                for (int i = 0; i < graphsGridView.VisibleRowCount; i++)
                {
                    int idRow = (int) graphsGridView.GetRowValues(i, "id");
                    if (dashboardPermissions.Permissions.Any(x => x.id == idRow))
                    {
                        graphsGridView.Selection.SetSelection(i, true);
                    }
                    else
                    {
                        graphsGridView.Selection.SetSelection(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private string GetUserType()
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
                        return role;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(Admin), ex.InnerException.Message);
                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
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
                        if (role == "SuperAdmin" || role == "Admin")
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



        public string GetCompanyName(int company)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
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
                return String.Empty;
            }
        }

    
        public (string groupName, string groupDescription) GetDataForGroupById(int groupId)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();

                        // Create the SQL command with a parameterized query
                        using (SqlCommand command = new SqlCommand("SELECT group_name, group_description FROM groups WHERE group_id = @group_id", conn))
                        {
                            // Add the parameter to avoid SQL injection
                            command.Parameters.AddWithValue("@group_id", groupId);

                            // Execute the query and read the result
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Get the group_name and group_description values
                                    string groupName = reader["group_name"].ToString();
                                    string groupDescription = reader["group_description"].ToString();

                                    // Return the values as a tuple
                                    return (groupName, groupDescription);
                                }
                                else
                                {
                                    // If no rows are found, return null values
                                    return (null, null);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as needed
                        Console.WriteLine($"Error: {ex.Message}");
                        return (null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return (null, null);    
            }
        }
      



        protected void SaveGraphs_Click(object sender, EventArgs e)
        {
            try
            {
                if (groupsGridView.GetSelectedFieldValues() == null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Morate izbrati skupino.')", true);
                }
                else
                {
                    SaveGroupPermissions();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void SaveGroupPermissions()
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

                    permissions.SetPermissionsForGroup(GetIdGroup(CurrentGroup));
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException (ex);
                var d = ex;
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
            }
        }

      

        private int GetIdCompany(string current)
        {
            try
            {
                string clean = current.Trim();
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand($"SELECT id_company FROM companies WHERE company_name='{clean}'", conn);
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return -1;
            }
        }


        private int GetIdGroup(string current)
        {
            try
            {
                string clean = current.Trim();
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand($"SELECT group_id FROM groups WHERE group_name='{clean}'", conn);
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return -1;
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
            catch(Exception ex)
            {
                SentrySdk.CaptureException (ex);
                return false;
            }
        }

       


        protected void NewGroup_ServerClick(object sender, EventArgs e)
        {
            try
            {
                groupName.Text = string.Empty;
                groupDescription.Text = string.Empty;
                GroupEdit = false;
                GroupsGrids.Visible = false;
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncGroup(); };", true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void DeleteGroup_Click(object sender, EventArgs e)
        {
            try
            {
                int groupId = GetIdGroup(CurrentGroup);
                if (groupId > 0)
                {
                    // Create the connection string
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        // Open the connection
                        conn.Open();
                        // Prepare the delete command
                        string deleteCommandText = "DELETE FROM groups WHERE group_id = @groupId";
                        using (SqlCommand cmd = new SqlCommand(deleteCommandText, conn))
                        {
                            // Add the parameter
                            cmd.Parameters.AddWithValue("@groupId", groupId);
                            // Execute the command
                            int rowsAffected = cmd.ExecuteNonQuery();
                        }
                    }
                }
                groupsGridView.DataBind();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void SaveGroupButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (GroupEdit)
                {
                    string groupNameValue = groupName.Text;
                    string groupDescriptionValue = groupDescription.Text;
                    int groupIdValue = GetIdGroup(CurrentGroup);
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        string query = "UPDATE Groups SET group_name = @groupName, group_description = @groupDescription WHERE group_id = @groupId";
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            command.Parameters.AddWithValue("@groupName", groupNameValue);
                            command.Parameters.AddWithValue("@groupDescription", groupDescriptionValue);
                            command.Parameters.AddWithValue("@groupId", groupIdValue);
                            conn.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                else // Insert new group 3.10.2024 Janko Jovičić
                {
                    string groupNameValue = groupName.Text;
                    string groupDescriptionValue = groupDescription.Text;
                    int companyId = GetIdCompany(CurrentCompany);
                    using (SqlConnection conn = new SqlConnection(connection))
                    {
                        string insertQuery = "INSERT INTO Groups (group_name, group_description, company_id) VALUES (@groupName, @groupDescription, @companyId)";
                        using (SqlCommand command = new SqlCommand(insertQuery, conn))
                        {
                            command.Parameters.AddWithValue("@groupName", groupNameValue);
                            command.Parameters.AddWithValue("@groupDescription", groupDescriptionValue);
                            command.Parameters.AddWithValue("@companyId", companyId);
                            conn.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                groupsGridView.DataBind();

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void moveToNotInGroupButton_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selectedUsernames = new List<string>();

                foreach (var selectedRow in usersInGroupGrid.GetSelectedFieldValues("uname"))
                {
                    selectedUsernames.Add(selectedRow.ToString());
                }

                if (selectedUsernames.Count > 0)
                {
                    // Prepare the SQL update command
                    string query = "UPDATE users SET group_id = NULL WHERE uname IN ('" + string.Join("','", selectedUsernames) + "')";

                    int groupId = GetIdGroup(CurrentGroup);

                    if (groupId < 0)
                    {
                        return;
                    }

                    // Execute the SQL update
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                    // Optionally, rebind the grids to reflect the changes
                    usersNotInGroupGrid.DataBind();
                    usersInGroupGrid.DataBind();

                    groupsGridView_StartRowEditing(this, null);

                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    

        protected void moveToInGroupButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected usernames from the 'usersNotInGroupGrid'
                List<string> selectedUsernames = new List<string>();

                foreach (var selectedRow in usersNotInGroupGrid.GetSelectedFieldValues("uname"))
                {
                    selectedUsernames.Add(selectedRow.ToString());
                }

                if (selectedUsernames.Count > 0)
                {
                    // Prepare the SQL update command
                    string query = "UPDATE users SET group_id = @group_id WHERE uname IN ('" + string.Join("','", selectedUsernames) + "')";

                    int groupId = GetIdGroup(CurrentGroup);

                    if (groupId < 0)
                    {
                        return;
                    }

                    // Execute the SQL update
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@group_id", groupId);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                    // Optionally, rebind the grids to reflect the changes
                    usersNotInGroupGrid.DataBind();
                    usersInGroupGrid.DataBind();

                    groupsGridView_StartRowEditing(this, null);

                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}