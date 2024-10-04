﻿using Dash.HelperClasses;
using Dash.Log;
using Dash.Models;
using DevExpress.Web.Bootstrap;
using Newtonsoft.Json;
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
        private string admin_id;
        private string databaseName;
        private bool isEditHappening = false;
        private bool isEditUser;
        private string userRightNow;



        private bool GroupEdit
        {
            get
            {
                if (Session["GroupEdit"] == null)
                {

                    Session["GroupEdit"] = false;

                }
                return (bool) Session["GroupEdit"];
            }
            set
            {
                Session["GroupEdit"] = value;
            }
        }



        private string CurrentGroup
        {
            get
            {
                if (Session["CurrentGroup"] == null)
                {

                    Session["CurrentGroup"] = string.Empty;

                }
                return Session["CurrentGroup"] as string;
            }
            set
            {
                Session["CurrentGroup"] = value;
            }
        }



        private string CurrentCompany
        {
            get
            {
                if (Session["CurrentCompany"] == null || (string)Session["CurrentCompany"] == string.Empty)
                {
                    // Create a new instance if the session is empty
                    Session["CurrentCompany"] = GetFirstCompany();
                }
                return (string)Session["CurrentCompany"];
            }
            set
            {
                Session["CurrentCompany"] = value;
            }
        }

        private string GetFirstCompany()
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

        private bool IsFilterActive
        {
            get
            {
                if (Session["ActiveFilter"] == null)
                {
                    // Create a new instance if the session is empty
                    Session["ActiveFilter"] = false;
                }
                return (bool)Session["ActiveFilter"];
            }
            set
            {
                Session["ActiveFilter"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;


            btnFilterBootstrap = graphsGridView.Toolbars.FindByName("FilterToolbar").Items.FindByName("RemoveFilter").FindControl("ClearFilterButton") as BootstrapButton;
            btnFilterBootstrap.Visible = IsFilterActive;


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


            Authenticate();


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
            groupsGridView.Selection.SetSelectionByKey(CurrentGroup, true);
        }

        private void GraphsGridView_DataBound(object sender, EventArgs e)
        {
            if (graphsGridView.VisibleRowCount > 0)
            {
                graphsGridView.Selection.BeginSelection();
                ShowConfigForUser();
                graphsGridView.Selection.EndSelection();
            }
        }



        private void CompaniesGridView_DataBound(object sender, EventArgs e)
        {
            companiesGridView.Selection.SetSelectionByKey(GetIdCompany(CurrentCompany), true);
        }

        private void CompaniesGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncCompany(); };", true);
        }

        private void UpdateFormCompany(string v)
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
                        admin_id = (reader["admin_id"].ToString());
                        databaseName = (reader["database_name"].ToString());
                    }

                    companyName.Text = company_name;
                    companyNumber.Text = company_number;
                    website.Text = websiteCompany;

                    listAdmin.SelectedValue = admin_id;
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
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }

        private void CompaniesGridView_SelectionChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var plurals = companiesGridView.GetSelectedFieldValues("id_company");
                    if (plurals.Count != 0)
                    {
                        var id = (int) plurals[0];
                        CurrentCompany = GetCompanyName(id);
                        CurrentGroup = GetFirstGroupForCompany(CurrentCompany);
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

        private string GetFirstGroupForCompany(string company)
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

        private void groupsGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
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

        private void groupsGridView_SelectionChanged(object sender, EventArgs e)
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

        private void ShowConfigForUser()
        {
            DashboardPermissions dashboardPermissions = new DashboardPermissions(GetIdGroup(CurrentGroup));
            for (int i = 0; i < graphsGridView.VisibleRowCount; i++)
            {
                int idRow = (int)graphsGridView.GetRowValues(i, "id");
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
                        Response.Redirect("Logon.aspx", true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }








        public string GetCompanyName(int company)
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

        private void UpdateFormName(string name)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {

                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }

        protected void RegistrationButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {

                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }

        private void InsertCompany()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT MAX(id_company) FROM companies", conn);
                    var result = cmd.ExecuteScalar();
                    Int32 next = System.Convert.ToInt32(result) + 1;
                    cmd = new SqlCommand($"INSERT INTO companies(id_company, company_name, company_number, website, database_name) VALUES({next}, '{companyName.Text}', {companyNumber.Text}, '{website.Text}', '{connName.Text}')", conn);
                    cmd.ExecuteNonQuery();
                    Dashboard graph = new Dashboard(next);
                    graph.SetGraphs(next);
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }

        private void UpdateAdminCompany(string admin_value, string cName)
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
        public (string groupName, string groupDescription) GetDataForGroupById(int groupId)
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
        protected void CompanyButton_Click(object sender, EventArgs e)
        {
            var ed = Request.Cookies["Edit"].Value.ToString();

            if (!isEditHappening && ed == "no")
            {
                InsertCompany();
                var names = companyName.Text.Split(' ');
                Random random = new Random();
                string adminname = $"{names[0]}{random.Next(1, 1000)}";
                CreateAdminForTheCompany(adminname, companyName.Text);
                UpdateAdminCompany(adminname, companyName.Text);
                var checkDB = CreateConnectionString(dbDataSource.Text, dbNameInstance.Text, dbPassword.Text, dbUser.Text, connName.Text);

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspeh.')", true);
            }
            else
            {
                SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
                build.InitialCatalog = dbNameInstance.Text;
                build.DataSource = dbDataSource.Text;
                build.UserID = dbUser.Text;
                build.Password = dbPassword.Text;
                //  UpdateConnectionString(dbDataSource.Text, dbNameInstance.Text, dbPassword.Text, dbUser.Text, connName.Text);
                UpdateCompanyData();
            }
        }

        private void AddConnectionString(string stringConnection)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                var builder = new SqlConnectionStringBuilder(stringConnection);
                ConnectionStringSettings conn = new ConnectionStringSettings();
                conn.ConnectionString = builder.ConnectionString;
                conn.Name = connName.Text;
                config.ConnectionStrings.ConnectionStrings.Add(conn);
                config.Save(ConfigurationSaveMode.Modified, true);
            }
            catch (Exception ex)
            {
                Logger.LogError(typeof(Admin), ex.InnerException.Message);
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Isto ime konekcije že obstaja!')", true);
            }
        }

        private string CreateConnectionString(string dbSource, string dbNameInstance, string dbPassword, string dbUser, string connName)
        {
            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
            build.InitialCatalog = dbNameInstance;
            build.DataSource = dbSource;
            build.UserID = dbUser;
            build.Password = dbPassword;
            AddConnectionString(build.ConnectionString);
            return build.ConnectionString;
        }

        private void UpdateCompanyData()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var admin = listAdmin.SelectedValue;
                    var websiteString = website.Text;
                    var companyNum = companyNumber.Text;
                    SqlCommand cmd = new SqlCommand($"UPDATE companies SET admin_id='{admin}', website='{websiteString}', company_number='{companyNum}' WHERE id_company={companiesGridView.FocusedRowIndex}", conn);
                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno spremenjeni podatki, informacije o konekciji spreminjajte v konfiguracijskem fajlu!')", true);
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }

        private void CreateAdminForTheCompany(string name, string cName)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(name, "SHA1");
                    string Company = cName;
                    int IdCompany = GetIdCompany(Company);
                    string FinalQueryRegistration = String.Format($"INSERT INTO users(uname, password, user_role, id_company, view_allowed, full_name, email, id_permision_user) VALUES ('{name}', '{HashedPassword}', 'Admin', '{IdCompany}','Viewer&Designer','{name}', '{name}@{name}.com')");
                    SqlCommand createUser = new SqlCommand(FinalQueryRegistration, conn);
                    var id = GetIdCompany(cName.Trim());
                    createUser.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }

        protected void DeleteUser_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string username = groupsGridView.GetSelectedFieldValues("Uname")[0].ToString();
                    SqlCommand cmd = new SqlCommand($"DELETE FROM users WHERE uname='{username}'", conn);
                    deletedID = groupsGridView.GetSelectedFieldValues("Uname")[0].ToString();
                    var company = GetCompanyQuery(groupsGridView.GetSelectedFieldValues("uname")[0].ToString());
                    var spacelessCompany = company.Replace(" ", string.Empty);
                    idFromString = GetIdCompany(spacelessCompany);
                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Izbrisan uporabnik.')", true);
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }





        private string GetCompanyQuery(string uname)
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

        protected void SaveGraphs_Click(object sender, EventArgs e)
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
                var d = ex;
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
            }
        }

      

        private void RemoveConnectionString(string current)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT database_name FROM companies WHERE company_name='{current}'", conn);
                    result = cmd.ExecuteScalar();
                    Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                    config.ConnectionStrings.ConnectionStrings.Remove($"{result}");
                    config.Save(ConfigurationSaveMode.Modified, true);
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    return;
                }
            }
        }

        private int GetIdCompany(string current)
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


        private int GetIdGroup(string current)
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






        public static bool testConnection()
        {
            return true;
        }



        protected void NewUser_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncUser(); };", true);
        }



        protected void NewUserClick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "showDialogSync()", true);
        }

        private List<string> GetSelectedValues(BootstrapGridView gridView, string columnName)
        {
            var selectedValues = new List<string>();
            foreach (string row in gridView.GetSelectedFieldValues(columnName))
            {
                selectedValues.Add(row);
            }
            return selectedValues;
        }

        public void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {

                var selectedTypes = GetSelectedValues(TypeGroup, "value");

                var selectedCompanies = GetSelectedValues(CompanyGroup, "value");

                var selectedLanguages = GetSelectedValues(LanguageGroup, "value");


                List<int> Ids = new List<int>();

                DataView dvGraphs = (DataView)query.Select(DataSourceSelectArguments.Empty);
                foreach (DataRowView row in dvGraphs)
                {
                    int id = (int)row["id"];
                    var metadata = JsonConvert.DeserializeObject<MetaData>((string)row["meta_data"]);
                    if (FilterDashboardComply(selectedTypes, selectedCompanies, selectedLanguages, metadata))
                    {
                        Ids.Add(id);
                    }
                }


                string FilterIds = string.Empty;
                for (int i = 0; i < Ids.Count; i++)
                {
                    if (i != Ids.Count - 1)
                    {
                        FilterIds += Ids.ElementAt(i) + ",";
                    }
                    else
                    {
                        FilterIds += Ids.ElementAt(i);
                    }
                }
                if (Ids.Count == 0)
                {
                    // No items to display.
                    graphsGridView.FilterExpression = $"[id] = -9999";
                }
                else
                {
                    graphsGridView.FilterExpression = $"[id] IN ({FilterIds})";
                }
                BootstrapButton button = graphsGridView.Toolbars.FindByName("FilterToolbar").Items.FindByName("RemoveFilter").FindControl("ClearFilterButton") as BootstrapButton;
                button.Visible = true;
                IsFilterActive = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(typeof(Admin), ex.InnerException.Message);
                return;
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
            catch
            {
                return false;
            }
        }

        protected void ClearFilterButton_Click(object sender, EventArgs e)
        {
            graphsGridView.FilterExpression = string.Empty;
            BootstrapButton button = graphsGridView.Toolbars.FindByName("FilterToolbar").Items.FindByName("RemoveFilter").FindControl("ClearFilterButton") as BootstrapButton;
            button.Visible = false;
            IsFilterActive = false;
            DashboardPermissions dashboardPermissions = new DashboardPermissions(GetIdGroup(CurrentGroup));
            for (int i = 0; i < graphsGridView.VisibleRowCount; i++)
            {
                int idRow = (int)graphsGridView.GetRowValues(i, "id");
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



        protected void NewGroup_ServerClick(object sender, EventArgs e)
        {
            GroupEdit = false;
            GroupsGrids.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncGroup(); };", true);
        }

        protected void DeleteGroup_Click(object sender, EventArgs e)
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

        protected void SaveGroupButton_Click(object sender, EventArgs e)
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

        protected void moveToNotInGroupButton_Click(object sender, EventArgs e)
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
    

        protected void moveToInGroupButton_Click(object sender, EventArgs e)
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

                if(groupId < 0) {
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
    }
}