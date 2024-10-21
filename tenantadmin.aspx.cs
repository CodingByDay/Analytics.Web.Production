using Dash.HelperClasses;
using Dash.Log;
using Dash.Models;
using DevExpress.Utils.Behaviors;
using DevExpress.Utils.DragDrop;
using DevExpress.Web.Bootstrap;
using Newtonsoft.Json;
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


        private int CurrentFocusGraph
        {
            get
            {
                if (Session["CurrentFocusGraph"] == null)
                {

                    Session["CurrentFocusGraph"] = -1;

                }
                return (int)Session["CurrentFocusGraph"];
            }
            set
            {
                Session["CurrentFocusGraph"] = value;
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

            usersGrid.SelectParameters["company_id"].DefaultValue = GetUserCompany();
            usersGrid.SelectParameters["uname"].DefaultValue = HttpContext.Current.User.Identity.Name;
            query.SelectParameters["ids"].DefaultValue = GetAllowedDashboardsForAdmin(HttpContext.Current.User.Identity.Name);

            InitializeUiChanges();
            Authenticate();

        }

        private void InitializeUiChanges()
        {
            SiteMaster mymaster = Master as SiteMaster;
            mymaster.BackButtonVisible = true;
        }

        private string GetAllowedDashboardsForAdmin(string name)
        {
            string ids = string.Empty;
            DashboardPermissions dashboardPermissions = new DashboardPermissions(HttpContext.Current.User.Identity.Name);
            for(int i=0; i<dashboardPermissions.Permissions.Count; i++) 
            {
                var currentPermission = dashboardPermissions.Permissions[i];
                if(i!=dashboardPermissions.Permissions.Count-1)
                {
                    ids += currentPermission.id + ",";
                } else
                {
                    ids += currentPermission.id;
                }
            }
            return ids;
        }

        private string GetUserCompany()
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

        private void GraphsGridView_FocusedRowChanged(object sender, EventArgs e)
        {

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
            usersGridView.Selection.SetSelectionByKey(CurrentUsername, true);
        }

        private void GraphsGridView_DataBound(object sender, EventArgs e)
        {

            if (CurrentUsername != string.Empty)
            {

                query.SelectParameters["uname"].DefaultValue = CurrentUsername;

                if (graphsGridView.VisibleRowCount > 0)
                {
                    graphsGridView.Selection.BeginSelection();
                    ShowConfigForUser();
                    graphsGridView.Selection.EndSelection();
                }
            }
        }



    

        private void CompaniesGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            listAdmin.Visible = true;
            Response.Cookies["Edit"].Value = "yes";
            isEditHappening = true;
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
                    return user;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        private void UsersGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            TxtUserName.Enabled = false;
            var name = e.EditingKeyValue;
            // Call js. function here if the test passes.
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
                ShowConfigForUser();
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

        protected void RegistrationButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    if (TxtUserName.Enabled == true)
                    {
                        if (TxtPassword.Text != TxtRePassword.Text)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Gesla niso ista.')", true);
                            TxtPassword.Text = "";
                            TxtRePassword.Text = "";
                        }
                        else
                        {
                            SqlCommand check = new SqlCommand($"SELECT count(*) FROM Users WHERE uname='{TxtUserName.Text}'", conn);
                            var resultCheck = check.ExecuteScalar();
                            Int32 resultUsername = System.Convert.ToInt32(resultCheck);
                            check.Dispose();
                            if (resultUsername > 0)
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Uporabniško ime že obstaja.')", true);
                            }
                            else
                            {
                                string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");
                                string CompanyInsert = CurrentCompany;
                                int IdCompany = GetIdCompany(CompanyInsert);
                                string QueryRegistration = String.Format($"INSERT INTO users(uname, password, user_role, id_company, view_allowed, full_name, email, referrer) VALUES ('{TxtUserName.Text}', '{HashedPassword}', '{userRole.SelectedValue}', '{IdCompany}','{userTypeList.SelectedValue}','{TxtName.Text}', '{email.Text}', '{referrer.Text}')");
                                SqlCommand createUser = new SqlCommand(QueryRegistration, conn);
                                var username = TxtUserName.Text;
                                try
                                {
                                    var id = GetIdCompany(CurrentCompany);
                                    createUser.ExecuteNonQuery();
                                    createUser.Dispose();
                                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno kreiran uporabnik.')", true);
                                    TxtName.Text = "";
                                    TxtPassword.Text = "";
                                    TxtRePassword.Text = "";
                                    TxtUserName.Text = "";
                                    email.Text = "";
                                    var company = CurrentCompany;
                                    var spacelessCompany = company.Replace(" ", string.Empty);
                                }
                                catch (Exception ex)
                                {
                                    var log = ex;
                                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                                    TxtName.Text = "";
                                    TxtPassword.Text = "";
                                    TxtRePassword.Text = "";
                                    TxtUserName.Text = "";
                                    email.Text = "";
                                }
                            }
                            cmd.Dispose();
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(TxtRePassword.Text))
                        {
                            HashedPasswordEdit = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");
                            cmdEdit = new SqlCommand($"UPDATE users SET password='{HashedPasswordEdit}', user_role='{userRole.SelectedValue}', view_allowed='{userTypeList.SelectedValue}', full_name='{TxtName.Text}', referrer='{referrer.Text}' WHERE uname='{TxtUserName.Text}'", conn);
                        }
                        else
                        {
                            cmdEdit = new SqlCommand($"UPDATE users SET user_role='{userRole.SelectedValue}', view_allowed='{userTypeList.SelectedValue}', referrer='{referrer.Text}', full_name='{TxtName.Text}' WHERE uname='{TxtUserName.Text}'", conn);
                        }
                        if (TxtPassword.Text != TxtRePassword.Text)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Gesla niso enaka.')", true);
                            TxtPassword.Text = "";
                            TxtRePassword.Text = "";
                        }
                        else
                        {
                            try
                            {
                                var username = TxtUserName.Text.Replace(" ", string.Empty); ;
                                cmdEdit.ExecuteNonQuery();

                                cmdEdit.Dispose();
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno spremenjeni podatki.')", true);

                                TxtName.Text = "";
                                TxtPassword.Text = "";
                                TxtRePassword.Text = "";
                                TxtUserName.Text = "";
                                email.Text = "";
                                var company = CurrentCompany;
                            }
                            catch (Exception ex)
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                                var debug = ex.Message;
                                TxtName.Text = "";
                                TxtPassword.Text = "";
                                TxtRePassword.Text = "";
                                TxtUserName.Text = "";
                                email.Text = "";
                            }
                            cmdEdit.Dispose();
                        }
                    }
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





        protected void NewUser_Click(object sender, EventArgs e)
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



        protected void NewUserClick(object sender, EventArgs e)
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



                List<int> Ids = new List<int>();

                DataView dvGraphs = (DataView)query.Select(DataSourceSelectArguments.Empty);
                foreach (DataRowView row in dvGraphs)
                {
                    int id = (int)row["id"];
                    var metadata = JsonConvert.DeserializeObject<MetaData>((string)row["meta_data"]);
                    if (FilterDashboardComply(selectedTypes, metadata))
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
            }
            catch (Exception)
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