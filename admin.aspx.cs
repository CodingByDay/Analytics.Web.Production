using Dash.HelperClasses;
using Dash.Log;
using Dash.Models;
using DevExpress.Utils.Behaviors;
using DevExpress.Utils.DragDrop;
using DevExpress.Web.Bootstrap;
using Newtonsoft.Json;
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
using System.Web.UI.WebControls;

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


        private int CurrentFocusGraph
        {
            get
            {
                if (Session["CurrentFocusGraph"] == null)
                {

                    Session["CurrentFocusGraph"] = -1;

                }
                return (int) Session["CurrentFocusGraph"];
            }
            set
            {
                Session["CurrentFocusGraph"] = value;
            }
        }

        private bool IsEditCompany
        {
            get
            {
                if (Session["IsEditCompany"] == null)
                {

                    Session["IsEditCompany"] = false;

                }
                return (bool) Session["IsEditCompany"];
            }
            set
            {
                Session["IsEditCompany"] = value;
            }
        }

        private bool IsEditUser
        {
            get
            {
                if (Session["IsEditUser"] == null)
                {

                    Session["IsEditUser"] = false;

                }
                return (bool)Session["IsEditUser"];
            }
            set
            {
                Session["IsEditUser"] = value;
            }
        }


        private string CurrentUsername
        {
            get
            {
                if (Session["CurrentUser"] == null)
                {

                   Session["CurrentUser"] = string.Empty;
                    
                }
                return Session["CurrentUser"] as string;
            }
            set
            {
                Session["CurrentUser"] = value;
            }
        }



        private string CurrentCompany
        {
            get
            {
                if (Session["CurrentCompany"] == null || (string) Session["CurrentCompany"] == string.Empty)
                {
                    // Create a new instance if the session is empty
                    Session["CurrentCompany"] = GetFirstCompany();
                }
                return (string) Session["CurrentCompany"];
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

            usersGridView.SettingsBehavior.AllowFocusedRow = false;
            usersGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
            usersGridView.SettingsBehavior.AllowSelectByRowClick = true;
            usersGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
            usersGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            usersGridView.EnableCallBacks = false;
            usersGridView.EnableRowsCache = true;

            usersGridView.StartRowEditing += UsersGridView_StartRowEditing;
            usersGridView.SelectionChanged += UsersGridView_SelectionChanged;
            usersGridView.DataBound += UsersGridView_DataBound;

            graphsGridView.EnableRowsCache = true;
            graphsGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
            graphsGridView.DataBound += GraphsGridView_DataBound;
            graphsGridView.SettingsBehavior.AllowFocusedRow = true;
            graphsGridView.SettingsBehavior.AllowSelectByRowClick = false;
            graphsGridView.EnableCallBacks = false;
            graphsGridView.FocusedRowChanged += GraphsGridView_FocusedRowChanged;

            if (!IsPostBack)
            {

            }

            InitializeUiChanges();
            Authenticate();  
        }

        private void GraphsGridView_FocusedRowChanged(object sender, EventArgs e)
        {
   
        }
        private void InitializeUiChanges()
        {
            SiteMaster mymaster = Master as SiteMaster;
            mymaster.BackButtonVisible = true;
        }
   

        private void UsersGridView_DataBound(object sender, EventArgs e)
        {
            usersGridView.Selection.SetSelectionByKey(CurrentUsername, true);
        }

        private void GraphsGridView_DataBound(object sender, EventArgs e)
        {

            if (CurrentUsername != string.Empty)
            {

                query.SelectParameters["uname"].DefaultValue = CurrentUsername;
                query.SelectParameters["company"].DefaultValue = GetIdCompany(CurrentCompany).ToString(); 

                if (graphsGridView.VisibleRowCount > 0)
                {
                    graphsGridView.Selection.BeginSelection();
                    //ShowConfigForUser();
                    graphsGridView.Selection.EndSelection();
                }
            } 
        }

       

        private void CompaniesGridView_DataBound(object sender, EventArgs e)
        {
            companiesGridView.Selection.SetSelectionByKey(GetIdCompany(CurrentCompany), true);   
        }

        private void CompaniesGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            IsEditCompany = true;
            TxtUserName.Enabled = false;
            var name = e.EditingKeyValue;
            UpdateFormCompany(name.ToString());
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncCompany(); };", true);
            e.Cancel = true;
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


                        var firstUser = GetFirstUserForCompany(CurrentCompany);

                        CurrentUsername = firstUser;
                        query.SelectParameters["uname"].DefaultValue = CurrentUsername;
                        usersGridView.FocusedRowIndex = 0;
                        
                    
                        // Apply the filter to the userGridView based on the selected id_company 30.09.2024 Janko Jovičić
                        usersGridView.FilterExpression = $"[id_company] = {id}";
                        usersGridView.DataBind();  // Refresh the userGridView with the applied filter
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

        private string GetFirstUserForCompany(string company)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    int id_company = GetIdCompany(company);

                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT TOP (1) uname FROM Users WHERE id_company = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", id_company);
                    var user = (string)cmd.ExecuteScalar();
                    return user ?? string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

        }

        private void UsersGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            IsEditUser = true;
            TxtUserName.Enabled = false;
            var name = e.EditingKeyValue;
            UpdateFormName(name.ToString());
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncUser(); };", true);
            e.Cancel = true;
        }

        private void UsersGridView_SelectionChanged(object sender, EventArgs e)
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
               // ShowConfigForUser();
            }

        }

        private void ShowConfigForUser()
        {
            DashboardPermissions dashboardPermissions = new DashboardPermissions(CurrentUsername);
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

        protected void RegistrationButton_Click(object sender, EventArgs e)
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



        private void InsertUser()
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

        private void UpdateUser()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string query;

                    if (!string.IsNullOrEmpty(TxtPassword.Text))
                    {
                        if(TxtPassword.Text == TxtRePassword.Text) {
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
                        } else {
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

        private bool IsUsernameExists(SqlConnection conn, string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE uname = @uname";
            using (SqlCommand cmdCheck = new SqlCommand(query, conn))
            {
                cmdCheck.Parameters.AddWithValue("@uname", username);
                return (int)cmdCheck.ExecuteScalar() > 0;
            }
        }

        private string HashPassword(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
        }

        private void ClearInputs()
        {
            TxtName.Text = "";
            TxtPassword.Text = "";
            TxtRePassword.Text = "";
            TxtUserName.Text = "";
            email.Text = "";
        }

        private void Notify(string message, bool isError)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", $"notify({isError.ToString().ToLower()}, '{message}')", true);
        }

        private void LogErrorAndNotify(Exception ex)
        {
            Logger.LogError(typeof(Admin), ex.InnerException?.Message ?? ex.Message);
            Notify("Napaka...", true);
        }

        private void InsertCompany()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    // Fetch the current max id, handle if no companies exist
                    using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(id_company), 0) FROM companies", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        int next = Convert.ToInt32(result) + 1;

                        // Use parameterized queries to avoid SQL injection
                        using (SqlCommand insertCmd = new SqlCommand("INSERT INTO companies(id_company, company_name, company_number, website, database_name) VALUES(@Id, @Name, @Number, @Website, @DbName)", conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Id", next);
                            insertCmd.Parameters.AddWithValue("@Name", companyName.Text);
                            insertCmd.Parameters.AddWithValue("@Number", companyNumber.Text);
                            insertCmd.Parameters.AddWithValue("@Website", website.Text);
                            insertCmd.Parameters.AddWithValue("@DbName", connName.Text);

                            insertCmd.ExecuteNonQuery();
                        }

                        // Set up the dashboard
                        Dashboard graph = new Dashboard(next);
                        graph.SetGraphs(next);
                    }
                }
                catch (Exception ex)
                {
                    // Log the main exception message
                    Logger.LogError(typeof(Admin), ex.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }
        }

       

        protected void CompanyButton_Click(object sender, EventArgs e)
        {
            if (!IsEditCompany)
            {
                InsertCompany();
                CreateConnectionString(dbDataSource.Text, dbNameInstance.Text, dbPassword.Text, dbUser.Text, connName.Text);
                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";
                companiesGridView.DataBind();
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspeh.')", true);
            }
            else
            {
                UpdateCompanyData();
            }
        }

        private void AddConnectionString(string stringConnection)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                var builder = new SqlConnectionStringBuilder(stringConnection);

                string newConnectionName = connName.Text;

                // Check if the connection string already exists
                if (config.ConnectionStrings.ConnectionStrings[newConnectionName] != null)
                {
                    return; // Exit the method if it already exists
                }

                // Create new connection string settings and add it
                ConnectionStringSettings conn = new ConnectionStringSettings
                {
                    ConnectionString = builder.ConnectionString,
                    Name = newConnectionName
                };

                config.ConnectionStrings.ConnectionStrings.Add(conn);
                config.Save(ConfigurationSaveMode.Modified, true);

                // Notify the user that the connection string has been added successfully
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno dodano!')", true);
            }
            catch (Exception ex)
            {
                // Log the full exception message
                Logger.LogError(typeof(Admin), $"Error adding connection string: {ex.Message}, StackTrace: {ex.StackTrace}");
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake!')", true);
            }
        }

        private void CreateConnectionString(string dbSource, string dbNameInstance, string dbPassword, string dbUser, string connName)
        {
            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
            build.InitialCatalog = dbNameInstance;
            build.DataSource = dbSource;
            build.UserID = dbUser;
            build.Password = dbPassword;
            AddConnectionString(build.ConnectionString);
        }

        private void UpdateCompanyData()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var websiteString = website.Text;
                    var companyNum = companyNumber.Text;
                    SqlCommand cmd = new SqlCommand($"UPDATE companies SET website='{websiteString}', company_number='{companyNum}' WHERE id_company={companiesGridView.FocusedRowIndex}", conn);
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

       


        protected void DeleteUser_Click(object sender, EventArgs e)
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


        [WebMethod]
        public static void CreatingCompanySessionEdit()
        {
            HttpContext.Current.Session["IsEditCompany"] = false; 
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
            if (usersGridView.GetSelectedFieldValues() == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Morate izbrati uporabnika.')", true);
            }
            else
            {
                SaveUserPermissions();
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
                var d = ex;
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
            }
        }

        protected void DeleteCompany_Click(object sender, EventArgs e)
        {
            var current = CurrentCompany;

            if (string.IsNullOrEmpty(current))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka pri brisanju podjetja.')", true);
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();

                    using (SqlCommand companyCmd = new SqlCommand("DELETE FROM companies WHERE company_name = @CompanyName", conn))
                    {
                        companyCmd.Parameters.AddWithValue("@CompanyName", current);
                        companyCmd.ExecuteNonQuery();
                    }

                    RemoveConnectionString(current);

                    // Refresh the grid
                    companiesGridView.DataBind();
                    usersGridView.DataBind();
                    graphsGridView.DataBind();

                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno brisanje.')", true);
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(typeof(Admin), $"SQL Error: {ex.Message}");
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
            }
            catch (Exception ex)
            {
                Logger.LogError(typeof(Admin), $"Error: {ex.Message}");
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

        private int GetIdCompany(string current)
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
            } else
            {
                return -1;
            }
        }

 

  

        protected void NewUser_Click(object sender, EventArgs e)
        {
            IsEditUser = false;
            usersGridView.Selection.SetSelection(-1, true);
            TxtUserName.Enabled = true;
            email.Enabled = true;
            TxtUserName.Text = "";
            TxtName.Text = "";
            email.Text = "";
            TxtPassword.Text = "";
            TxtRePassword.Text = "";
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncUser(); };", true);
        }

  

        protected void NewUserClick(object sender, EventArgs e)
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

                DataView dvGraphs = (DataView) query.Select(DataSourceSelectArguments.Empty);
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
            DashboardPermissions dashboardPermissions = new DashboardPermissions(CurrentUsername);
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
            } catch(Exception) 
            {
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
            catch (Exception)
            {
                return;
            }
        }

        private void UpdateSortOrder(string uname, int dashboardId, int sortOrder)
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
    }
}