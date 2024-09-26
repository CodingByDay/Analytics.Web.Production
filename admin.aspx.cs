﻿using Dash.HelperClasses;
using Dash.Log;
using Dash.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace Dash
{
    public partial class Admin : System.Web.UI.Page
    {


        private string connection;
        // DB.
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
        private List<String> typesOfViews = new List<string>();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            companiesGridView.SettingsBehavior.AllowFocusedRow = true;
            companiesGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
            companiesGridView.SettingsBehavior.AllowSelectByRowClick = true;
            companiesGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
            companiesGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            companiesGridView.EnableCallBacks = false;
            companiesGridView.SelectionChanged += CompaniesGridView_SelectionChanged;
            companiesGridView.StartRowEditing += CompaniesGridView_StartRowEditing;
            companiesGridView.FocusedRowChanged += CompaniesGridView_FocusedRowChanged;     
            usersGridView.SettingsBehavior.AllowFocusedRow = true;
            usersGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
            usersGridView.SettingsBehavior.AllowSelectByRowClick = true;
            usersGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
            usersGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            usersGridView.EnableCallBacks = false;         
            usersGridView.StartRowEditing += UsersGridView_StartRowEditing;


            if (!IsPostBack)
            {
                graphsGridView.Enabled = true;
                usersGridView.SelectionChanged += UsersGridView_SelectionChanged;
                Authenticate();
                FillListGraphsNames();
                companiesList.SelectedIndex = 0;
                companiesGridView.FocusedRowIndex = 0;
                var beginingID = 1;
                companiesList.Enabled = false;
                FillUsers(beginingID);
                FillListGraphs();
                FillCompaniesRegistration();
                FillListAdmin();
                typesOfViews.Add("Viewer");
                typesOfViews.Add("Designer");
                typesOfViews.Add("Viewer&Designer");

                // BindCheckboxGroups();

            }
            else
            {
                graphsGridView.Enabled = true;
                FillListGraphs();
                if (companiesGridView.Selection.Count != 0)
                {
                    var plurals = companiesGridView.GetSelectedFieldValues("company_name");
                    var value = plurals[0].ToString();
                    FillUsers(GetIdCompany(value));
                }
                else
                {
                    companiesGridView.FocusedRowIndex = 0;
                    companiesGridView.Selection.SelectRow(0);
                    var current = companiesGridView.GetSelectedFieldValues("company_name");
                    FillUsers(GetIdCompany(current[0].ToString()));
                }
            }
        }



        private void CompaniesGridView_FocusedRowChanged(object sender, EventArgs e)
        {

            var index = companiesGridView.FocusedRowIndex;
            if (index != -1)
            {
                string[] names = { "company_name" };
                var values = companiesGridView.GetRowValues(index, "company_name");
                Session["current"] = values.ToString();

            }

        }

        private void CompaniesGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

            listAdmin.Visible = true;
            Response.Cookies["Edit"].Value = "yes";
            isEditHappening = true;
            TxtUserName.Enabled = false;
            var name = e.EditingKeyValue;
            updateFormCompany(name.ToString());
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncCompany(); };", true);
            e.Cancel = true;
        }

        private void updateFormCompany(string v)
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
                    var plurals = companiesGridView.GetSelectedFieldValues("company_name");
                    if (plurals.Count != 0)
                    {
                        var connectionStringName = get_connectionStringName(plurals[0].ToString());
                        Session["conn"] = connectionStringName;
                        TxtUserName.Enabled = false;
                        email.Enabled = false;
                        current = plurals[0].ToString();
                        var id = GetIdCompany(plurals[0].ToString());
                        FillUsers(id);
                        var without = plurals[0].ToString();
                        companiesList.SelectedValue = without;
                        companiesList.Enabled = false;
                        usersGridView.Selection.SetSelection(0, true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }

        }

        private void UsersGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            TxtUserName.Enabled = false;
            var name = e.EditingKeyValue;
            // Call js. function here if the test passes.
            updateFormName(name.ToString());
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "window.onload = function() { showDialogSyncUser(); };", true);
            e.Cancel = true;
        }

        private void UsersGridView_SelectionChanged(object sender, EventArgs e)
        {
            TxtUserName.Enabled = false;
            email.Enabled = false;
            graphsGridView.Enabled = true;
            FillListGraphs();
            List<String> values = FillListGraphsNames();
            ShowConfig(values);
            UpdateForm();
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

        public void FillListAdmin()
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    admins.Clear();
                    strings.Clear();

                    string UserNameForChecking
                        = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand("SELECT uname FROM users", conn);        
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        admins.Add(sdr["uname"].ToString());
                    }
                    listAdmin.DataSource = admins;
                    listAdmin.DataBind();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }


        }

        private void ShowConfig(List<String> obj)
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


        public void FillListGraphs()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    graphList.Clear();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"SELECT caption FROM dashboards;", conn);
                           
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        graphList.Add(sdr["caption"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }


        }





        public void FillUsers(int companyID)
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    byUserList.Clear();
                    usersList.Clear();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name; 
                    cmd = new SqlCommand($"SELECT * FROM users WHERE id_company={companyID}", conn);
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        User user = new User(sdr["uname"].ToString(), sdr["password"].ToString(), sdr["user_role"].ToString(), sdr["view_allowed"].ToString(), sdr["email"].ToString());
                        var test = user.uname;
                        usersList.Add(user);
                        byUserList.Add(sdr["uname"].ToString());
                    }
                    usersGridView.DataSource = usersList;
                    usersGridView.DataBind();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);

                }
            }


        }






        private void FillCompaniesRegistration()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand("SELECT * FROM companies", conn); 
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        companies.Add(sdr["company_name"].ToString());
                    }
                    companiesList.DataSource = companies;
                    companiesList.DataBind();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);

                }
            }

        }



        private void UpdateForm()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    isEditUser = true;
                    if (usersGridView.GetSelectedFieldValues("uname").Count > 1)
                    {
                        userRightNow = usersGridView.GetSelectedFieldValues("uname")[0].ToString();
                    }
                    else
                    {
                        try
                        {
                            userRightNow = usersGridView.GetRowValues(0, "uname").ToString();
                        } catch
                        {

                        }
                    }
                    SqlCommand cmd = new SqlCommand($"SELECT * FROM users WHERE uname='{userRightNow}'", conn);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        TxtName.Text = sdr["full_name"].ToString();
                        TxtUserName.Text = sdr["uname"].ToString();
                        TxtUserName.Enabled = false;
                        referrer.Text = sdr["referrer"].ToString();
                        int number = (int)sdr["id_company"];
                        string dare = GetCompanyName(number);
                        companiesList.SelectedValue = dare;
                        companiesList.Enabled = false;
                        email.Enabled = false;
                        string role = sdr["user_role"].ToString();
                        string type = sdr["view_allowed"].ToString();
                        email.Text = sdr["email"].ToString();
                        userTypeList.SelectedIndex = userTypeList.Items.IndexOf(userTypeList.Items.FindByValue(type));
                        userRole.SelectedIndex = userRole.Items.IndexOf(userRole.Items.FindByValue(role));

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
        public string GetCompanyName(int company)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string uname = HttpContext.Current.User.Identity.Name;
                    cmd = new SqlCommand($"SELECT company_name FROM companies WHERE id_company={company}", conn); 
                    var admin = (string) cmd.ExecuteScalar();
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

        private void updateFormName(string name)
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
                        companiesList.SelectedValue = data;
                        companiesList.Enabled = false;
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


        protected void registrationButton_Click(object sender, EventArgs e)
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
                                string CompanyInsert = companiesList.SelectedItem.Value;
                                int IdCompany = GetIdCompany(CompanyInsert);
                                string QueryRegistration = String.Format($"INSERT INTO users(uname, password, user_role, id_company, view_allowed, full_name, email, referrer) VALUES ('{TxtUserName.Text}', '{HashedPassword}', '{userRole.SelectedValue}', '{IdCompany}','{userTypeList.SelectedValue}','{TxtName.Text}', '{email.Text}', '{referrer.Text}')");
                                SqlCommand createUser = new SqlCommand(QueryRegistration, conn);
                                var username = TxtUserName.Text;
                                try
                                {
                                    var id = GetIdCompany(companiesList.SelectedValue);
                                    createUser.ExecuteNonQuery();
                                    createUser.Dispose();
                                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno kreiran uporabnik.')", true);
                                    TxtName.Text = "";
                                    TxtPassword.Text = "";
                                    TxtRePassword.Text = "";
                                    TxtUserName.Text = "";
                                    email.Text = "";
                                    FillListAdmin();
                                    FillUsers(id);
                                    var company = companiesList.SelectedValue;
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
                            cmdEdit = new SqlCommand($"UPDATE Users SET Pwd='{HashedPasswordEdit}', userRole='{userRole.SelectedValue}', ViewState='{userTypeList.SelectedValue}', FullName='{TxtName.Text}', referer='{referrer.Text}' WHERE uname='{TxtUserName.Text}'", conn);
                        }
                        else
                        {
                            cmdEdit = new SqlCommand($"UPDATE Users SET userRole='{userRole.SelectedValue}', ViewState='{userTypeList.SelectedValue}', referer='{referrer.Text}', FullName='{TxtName.Text}' WHERE uname='{TxtUserName.Text}'", conn);
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
                                var company = companiesList.SelectedValue.Replace(" ", string.Empty);
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


        private bool checkIfNumber(string parametar)
        {
            var regex = new Regex(@"^-?[0-9][0-9,\.]+$");

            var numeric = regex.IsMatch(parametar);

            if (numeric)
                return true;
            else
                return false;


        }



        private void insertCompany()
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

        private void updateAdminCompany(string admin_value, string cName)
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
     
        protected void companyButton_Click(object sender, EventArgs e)
        {
            var ed = Request.Cookies["Edit"].Value.ToString();

            if (!isEditHappening && ed == "no")
            {                           
                    insertCompany();
                    FillCompaniesRegistration();
                    var names = companyName.Text.Split(' ');
                    Random random = new Random();
                    string adminname = $"{names[0]}{random.Next(1, 1000)}";
                    createAdminForTheCompany(adminname, companyName.Text);
                    updateAdminCompany(adminname, companyName.Text);
                    var checkDB = createConnectionString(dbDataSource.Text, dbNameInstance.Text, dbPassword.Text, dbUser.Text, connName.Text);
                    var sourceID = companiesGridView.DataSource;
                    companiesGridView.DataSource = null;
                    companiesGridView.DataSource = sourceID;
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
                    updateCompanyData();
            }
        }



        private bool isConnectionOk(string connection)
        {
            var _stringDB = GetResultFromDBTest(connection);
            if (connName.Text == null)
            {
                return true;
            }
            else
            {
                return false;
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
            } catch(Exception ex)
            {
                Logger.LogError(typeof(Admin), ex.InnerException.Message);
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Isto ime konekcije že obstaja!')", true);

            }
        }



        private string createConnectionString(string dbSource, string dbNameInstance, string dbPassword, string dbUser, string connName)
        {
            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
            build.InitialCatalog = dbNameInstance;
            build.DataSource = dbSource;
            build.UserID = dbUser;
            build.Password = dbPassword;
            AddConnectionString(build.ConnectionString);
            return build.ConnectionString;
        }



        private void UpdateConnectionString(string dbSource, string dbNameInstance, string dbPassword, string dbUser, string connName)
        {
            try
            {
                var ConnectionString = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
                SqlConnectionStringBuilder build = new SqlConnectionStringBuilder(ConnectionString);
                build.InitialCatalog = dbNameInstance;
                build.DataSource = dbSource;
                build.UserID = dbUser;
                build.Password = dbPassword;
                ConnectionStringSettings stringSettings = new ConnectionStringSettings();
                stringSettings.ConnectionString = build.ConnectionString;
                var settings = ConfigurationManager.ConnectionStrings[connName];
                var fi = typeof(ConfigurationElement).GetField(
                              "_bReadOnly",
                              BindingFlags.Instance | BindingFlags.NonPublic);
                fi.SetValue(settings, false);
                settings.ConnectionString = build.ConnectionString;
                ConfigurationManager.RefreshSection("connectionStrings");
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Ne morete menjati ime konekcije!')", true);

            }


        }

        private void updateCompanyData()
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

        private void createAdminForTheCompany(string name, string cName)
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
                    FillListAdmin();
                    FillUsers(id);
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }

        }

        protected void deleteUser_Click(object sender, EventArgs e)
        {
           
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string username = usersGridView.GetSelectedFieldValues("Uname")[0].ToString();
                    SqlCommand cmd = new SqlCommand($"DELETE FROM users WHERE uname='{username}'", conn);
                    deletedID = usersGridView.GetSelectedFieldValues("Uname")[0].ToString();
                    var company = getCompanyQuery(usersGridView.GetSelectedFieldValues("uname")[0].ToString());
                    var spacelessCompany = company.Replace(" ", string.Empty);
                    idFromString = GetIdCompany(spacelessCompany);
                    cmd.ExecuteNonQuery();
                    FillListGraphs();
                    List<String> values = FillListGraphsNames();
                    ShowConfig(values);
                    FillUsers(idFromString);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Izbrisan uporabnik.')", true);
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
            }




        }
        private string get_connectionStringName(string name)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                    var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
                    SqlCommand cmd = new SqlCommand($"SELECT database_name FROM companies WHERE company_name='{name}'", conn);
                    string result = cmd.ExecuteScalar().ToString();
                    returnString = result;
                    return returnString;
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    return String.Empty;
                }
            }
        }


        protected void usersGridView_SelectedIndexChanged(object sender, EventArgs e)
        {

            TxtUserName.Enabled = false;
            email.Enabled = false;
            graphsGridView.Enabled = true;
            FillListGraphs();
            List<String> values = FillListGraphsNames();
            ShowConfig(values);
            UpdateForm();
        }

        public List<String> FillListGraphsNames()
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    graphList.Clear();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"SELECT caption FROM dashboards;", conn);
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        graphList.Add(sdr["caption"].ToString());
                        string trimmed = sdr["caption"].ToString();
                        string stripped = String.Concat(trimmed.ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");

                        values.Add(stripped);

                    }
                    return values;
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    return values;
                }
            }


        }


        private string getCompanyQuery(string uname)
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


     


        protected void saveGraphs_Click(object sender, EventArgs e)
        {
            if (usersGridView.GetSelectedFieldValues() == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Morate izbrati uporabnika.')", true);
            }
            else
            {
                List<String> values = FillListGraphsNames();
                ShowConfig(values);
            }
        }


        private string GetCurrentCompany()
        {
            var company = companiesGridView.GetSelectedFieldValues("company_name");
            return company[0].ToString();
        }

        protected void deleteCompany_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    if (companiesGridView.FocusedRowIndex != -1)
                    {
                        var current = Session["current"].ToString();
                        var id = GetIdCompany(current);
                        Dashboard graph = new Dashboard(id);
                        graph.Delete(id);
                        SqlCommand user = new SqlCommand($"DELETE FROM users WHERE id_company={id}", conn);
                        RemoveConnectionString(current);
                        SqlCommand cmd = new SqlCommand($"DELETE FROM companies WHERE company_name='{current}'", conn);
                        string dev = $"DELETE FROM companies WHERE company_name='{current}'";
                        cmd.ExecuteNonQuery();
                        try
                        {
                            user.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(typeof(Admin), ex.InnerException.Message);
                            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                        }
                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno brisanje.')", true);
                        var source = companiesGridView.DataSource;
                        companiesGridView.DataSource = null;
                        companiesGridView.DataSource = source;
                        FillListGraphs();
                        FillListAdmin();
                        cmd.Dispose();
                        string companyName = companiesGridView.GetRowValues(0, "company_name").ToString();
                        int companyID = GetIdCompany(companyName);
                        FillUsers(companyID);                       
                    }
                }
                catch (Exception ex)
                {
                    var d = ex;
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                }
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
                    int finalID = System.Convert.ToInt32(result);
                    return finalID;
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


        protected void byUserListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

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





        private string GetResultFromDBTest(string connectionString)
        {
            var _result = CheckConnection.CheckDatabaseConnection(connectionString);
            string _textResult;

            if (_result == false)

            {

                _textResult = " ::Napaka v konekciji";

            }
            else
            {
                _textResult = " ::Uspešna konekcija";


            }

            return _textResult;

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

        protected void usersGridView_SelectionChanged(object sender, EventArgs e)
        {
            TxtUserName.Enabled = false;
            email.Enabled = false;
            graphsGridView.Enabled = true;
            FillListGraphs();
            List<String> values = FillListGraphsNames();
            ShowConfig(values);
            UpdateForm();
        }

        protected void graphsGridView_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("filter|"))
            {
                string category = e.Parameters.Split('|')[1];

                if (category == "all")
                {
                    // Remove the filter and show all data
                    graphsGridView.FilterExpression = string.Empty;
                }
                else
                {
                    // Apply the filter for the selected category
                    graphsGridView.FilterExpression = $"[Category] = '{category}'";
                }

                graphsGridView.DataBind();
            }
        }

        public void btnFilter_Click(object sender, EventArgs e)
        {
            string Ids = string.Empty;
            DataView dvGraphs = (DataView)query.Select(DataSourceSelectArguments.Empty);
            foreach (DataRowView row in dvGraphs)
            {
                int id = (int)row["id"];
                var metadata = JsonConvert.DeserializeObject<MetaData>((string)row["meta_data"]);
                var debug = true;
            }
        }
    }
}
