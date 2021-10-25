using DevExpress.Web.Bootstrap;
using peptak.HelperClasses;
using peptak.ORM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace peptak
{
    public partial class admin : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            HtmlAnchor adminButton = this.Master.FindControl("adminButtonAnchor") as HtmlAnchor;
            adminButton.Visible = false;
            companiesGridView.SettingsBehavior.AllowFocusedRow = true;
            
            companiesGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
            companiesGridView.SettingsBehavior.AllowSelectByRowClick = true;
            companiesGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
            companiesGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            companiesGridView.EnableCallBacks = false;
            companiesGridView.SelectionChanged += CompaniesGridView_SelectionChanged;
            companiesGridView.StartRowEditing += CompaniesGridView_StartRowEditing;
            companiesGridView.FocusedRowChanged += CompaniesGridView_FocusedRowChanged;
            // All of this config is neccessary.
            usersGridView.SettingsBehavior.AllowFocusedRow = true;
            usersGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
            usersGridView.SettingsBehavior.AllowSelectByRowClick = true;
            usersGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
            usersGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            usersGridView.EnableCallBacks = false;


            usersGridView.StartRowEditing += UsersGridView_StartRowEditing;

            // Keep this.


            if (!IsPostBack)
            {
                graphsGridView.Enabled = true;
                


                usersGridView.SelectionChanged += UsersGridView_SelectionChanged;
                authenticate();
                HtmlAnchor admin = this.Master.FindControl("backButtonA") as HtmlAnchor;
                admin.Visible = true;
                FillListGraphsNames();
                companiesList.SelectedIndex = 0;
                by.Visible = false;
                companiesGridView.FocusedRowIndex = 0;
               
                var beginingID = 1;
                companiesList.Enabled = false;
                FillUsers(beginingID);
                //fillCompanies();
                // FillUsers();
                FillListGraphs();
                fillCompaniesRegistration();
                FillListAdmin();
                // User Types
                typesOfViews.Add("Viewer");
                typesOfViews.Add("Designer");
                typesOfViews.Add("Viewer&Designer");


              

            }
            else
            {
                graphsGridView.Enabled = true;
                HtmlAnchor admin = this.Master.FindControl("backButtonA") as HtmlAnchor;
                admin.Visible = true;
                FillListGraphs();
                
               

                

                if (companiesGridView.Selection.Count != 0)
                {
                    var plurals = companiesGridView.GetSelectedFieldValues("company_name");
                    var value = plurals[0].ToString();
                    FillUsers(getIdCompany(value));
                }
                else
                {
                    companiesGridView.FocusedRowIndex = 0;
                    companiesGridView.Selection.SelectRow(0);
                    var current = companiesGridView.GetSelectedFieldValues("company_name");
                    FillUsers(getIdCompany(current[0].ToString()));
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
            TxtUserName.Enabled = false;

            var name = e.EditingKeyValue;
            // Call js. function here if the test passes.
            updateFormCompany(name.ToString());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showDialogSyncCompany()", true);


            e.Cancel = true;
        }

        private void updateFormCompany(string v)
        {
            // Select * from companies where id_company={}

            conn = new SqlConnection(connection);
            conn.Open();
            var username = HttpContext.Current.User.Identity.Name;
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"Select * from companies where id_company={v};", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               company_name = (reader["company_name"].ToString());
               company_number = (reader["company_number"].ToString());
               websiteCompany = (reader["website"].ToString());
               admin_id = (reader["admin_id"].ToString());
               databaseName = (reader["databaseName"].ToString());
            }

            companyName.Text = company_name;
            companyNumber.Text = company_number;
            website.Text = websiteCompany;

            listAdmin.SelectedValue = admin_id;
            var connectionDB = ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
            ConnectionString.Text = connectionDB;

            conn.Close();
            cmd.Dispose();


            
        }

        private void CompaniesGridView_SelectionChanged(object sender, EventArgs e)
        {

            var plurals = companiesGridView.GetSelectedFieldValues("company_name");

            if (plurals.Count != 0)
            {
                var connectionStringName = get_connectionStringName(plurals[0].ToString());
                Session["conn"] = connectionStringName;


                TxtUserName.Enabled = false;
                email.Enabled = false;
                current = plurals[0].ToString();

                var id = getIdCompany(plurals[0].ToString());

                FillUsers(id);

                var without = plurals[0].ToString();
                companiesList.SelectedValue = without;
                companiesList.Enabled = false;

                usersGridView.Selection.SetSelection(0, true);
            }
        }

        private void UsersGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

            TxtUserName.Enabled = false;

            var name = e.EditingKeyValue;
            // Call js. function here if the test passes.
            updateFormName(name.ToString());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showDialogSync()", true);


            e.Cancel = true;
        }

        private void UsersGridView_SelectionChanged(object sender, EventArgs e)
        {
            TxtUserName.Enabled = false;
            email.Enabled = false;
            graphsGridView.Enabled = true;
            FillListGraphs();
            List<String> values = FillListGraphsNames();
            showConfig(values);
            updateForm();


        }

        private void UsersGridView_FocusedRowChanged(object sender, EventArgs e)
        {

        }

        private void authenticate()

        {

            conn = new SqlConnection(connection);
            conn.Open();
            var username = HttpContext.Current.User.Identity.Name;
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"select userRole from Users where uname='{username}';", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                role = (reader["userRole"].ToString());
            }

            conn.Close();
            cmd.Dispose();


            if (role == "SuperAdmin")
            {

            }
            else
            {
                Response.Redirect("logon.aspx", true);
            }
        }

        public void FillListAdmin()
        {
            try
            {
                admins.Clear();
                strings.Clear();

                string UserNameForChecking
                    = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection(connection);
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("Select uname from Users", conn); /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    admins.Add(sdr["uname"].ToString());

                }
                listAdmin.DataSource = admins;
                listAdmin.DataBind();
                ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
                foreach (ConnectionStringSettings setting in connections)
                {
                    strings.Add(setting.Name);
                }
                strings.RemoveAt(0);

                ConnectionStrings.DataSource = strings;
                ConnectionStrings.DataBind();



                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
                var log = ex;
            }
        }

        private void showConfig(List<String> obj)
        {


            valuesBool.Clear();
            columnNames.Clear();
            config.Clear();
            conn = new SqlConnection(connection);
            conn.Open();
            // DECLARE @ColList Varchar(1000), @SQLStatment VARCHAR(4000)
            // SET @ColList = ''
            // select @ColList = @ColList + Name + ' , ' from syscolumns where id = object_id('permisions') AND Name != 'id_permisions'
            // SELECT @SQLStatment = 'SELECT ' + Substring(@ColList, 1, len(@ColList) - 1) + 'FROM permisions'
            // EXEC(@SQLStatment
            if (usersGridView.FocusedRowIndex >= 0)
            {
            

                var namePlural = usersGridView.GetSelectedFieldValues("uname");

                if(namePlural.Count == 0)
                {
                    string nameUser = usersList.ElementAt(0).uname;
                    findIdString = String.Format($"SELECT id_permision_user from Users where uname='{nameUser}'");
                } else
                {
                    string name = namePlural[0].ToString();
                    findIdString = String.Format($"SELECT id_permision_user from Users where uname='{name}'");
                }
              
                // Documentation. This query is for getting all the permision table data from the user
                cmd = new SqlCommand(findIdString, conn);
                idNumber = cmd.ExecuteScalar();
                Int32 Total_Key = System.Convert.ToInt32(idNumber);

                conn.Close();
                conn.Dispose();
                permisionQuery = $"SELECT * FROM permisions_user WHERE id_permisions_user={Total_Key}";
                cmd = new SqlCommand(permisionQuery, conn);
                conn = new SqlConnection(connection);

                using (SqlConnection connection = new SqlConnection(
                  this.connection))
                {
                    SqlCommand command = new SqlCommand(permisionQuery, connection);
                    connection.Open();
                    SqlDataReader reader =
                    command.ExecuteReader(CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        for (int i = 0; i < obj.Count; i++)
                        {
                            int bitValueTemp = (int)(reader[values[i]] as int? ?? 0);
                            if (bitValueTemp == 1)
                            {


                                graphsGridView.Selection.SetSelection(i, true);
                                valuesBool.Add(true);
                            }
                            else
                            {
                                graphsGridView.Selection.SetSelection(i, false);
                                valuesBool.Add(false);
                            }
                        }
                    }

                    conn.Close();
                    conn.Dispose();
                }
            }
            else
            {


            }


        }


        public void FillListGraphs()
        {

            try
            {
                graphList.Clear();
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection(connection);

                conn.Open();

                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"SELECT Caption from Dashboards;", conn);

                /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    graphList.Add(sdr["Caption"].ToString());

                }


                cmd.Dispose();
                conn.Dispose();

                //  graphsGridView.DataSource = graphList;
                //  graphsGridView.DataBind();


            }
            catch (Exception ex)
            {
                var log = ex;
            }

        }

   



        public void FillUsers(int companyID)
        {
            try
            {
                byUserList.Clear();
                usersList.Clear();
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection(connection);
                conn.Open();

                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"Select * from Users where id_company={companyID}", conn);

                /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {

                    User user = new User(sdr["uname"].ToString(), sdr["Pwd"].ToString(), sdr["userRole"].ToString(), sdr["ViewState"].ToString(), sdr["email"].ToString());
                    var test = user.uname;
                    usersList.Add(user);
                    byUserList.Add(sdr["uname"].ToString());

                }


                byUserListBox.DataSource = byUserList;
                byUserListBox.DataBind();


                usersGridView.DataSource = usersList;
                usersGridView.DataBind();


                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
                var log = ex;
            }

        }






        private void fillCompaniesRegistration()
        {
            try
            {
                conn = new SqlConnection(connection);
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("Select * from companies", conn); /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    companies.Add(sdr["company_name"].ToString());
                    var debug = sdr["company_name"].ToString();

                }
                companiesList.DataSource = companies;
                companiesList.DataBind();


                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        //private void fillCompanies()
        //{

        //    try
        //    {
        //        companiesData.Clear();
        //        conn = new SqlConnection(connection);
        //        conn.Open();
        //        // Create SqlCommand to select pwd field from users table given supplied userName.
        //        cmd = new SqlCommand("Select * from companies", conn); /// Intepolation or the F string. C# > 5.0       
        //        // Execute command and fetch pwd field into lookupPassword string.
        //        SqlDataReader sdr = cmd.ExecuteReader();
        //        while (sdr.Read())
        //        {
        //            companiesData.Add(sdr["company_name"].ToString());

        //        }
        //        companiesListBox.DataSource = companiesData;
        //        companiesListBox.DataBind();


        //        cmd.Dispose();
        //        conn.Close();


        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.ToString());
        //    }
        //}

        private void updateForm()
        {


            var userRightNow = usersGridView.GetSelectedFieldValues("uname");
            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE uname='{userRightNow}'", conn);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                TxtName.Text = sdr["FullName"].ToString();
                TxtUserName.Text = sdr["uname"].ToString();
                TxtUserName.Enabled = false;
                var number = (int)sdr["id_company"];
                companiesList.SelectedIndex = number - 1;

                companiesList.Enabled = false;
                email.Enabled = false;
                string role = sdr["userRole"].ToString();
                string type = sdr["ViewState"].ToString();
                email.Text = sdr["email"].ToString();
                userTypeRadio.SelectedValue = type;
                userRole.SelectedIndex = userRole.Items.IndexOf(userRole.Items.FindByValue(role));

            }
            sdr.Close();
            cmd.Dispose();
        }


        private void updateFormName(string name)
        {


            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE uname='{name}'", conn);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                TxtName.Text = sdr["FullName"].ToString();
                TxtUserName.Text = sdr["uname"].ToString();
                TxtUserName.Enabled = false;
                var number = (int)sdr["id_company"];
                companiesList.SelectedIndex = number - 1;

                companiesList.Enabled = false;
                email.Enabled = false;
                string role = sdr["userRole"].ToString();
                string type = sdr["ViewState"].ToString();
                email.Text = sdr["email"].ToString();
                userTypeRadio.SelectedValue = type;
                userRole.SelectedIndex = userRole.Items.IndexOf(userRole.Items.FindByValue(role));

            }
            sdr.Close();
            cmd.Dispose();
        }


        protected void registrationButton_Click(object sender, EventArgs e)
        {
            if (TxtUserName.Enabled == true)
            {

                conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT MAX(id_permision_user) FROM Users;", conn);
                var result = cmd.ExecuteScalar();
                Int32 Total_ID = System.Convert.ToInt32(result);
                cmd.Dispose();
                int next = Total_ID + 1;
                if (TxtPassword.Text != TxtRePassword.Text)
                {

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Gesla niso ista.')", true);
                    TxtPassword.Text = "";
                    TxtRePassword.Text = "";
                }
                else
                {
                    conn = new SqlConnection(connection);
                    conn.Open();
                    SqlCommand check = new SqlCommand($"Select count(*) from Users where uname='{TxtUserName.Text}'", conn);


                    var resultCheck = check.ExecuteScalar();

                    Int32 resultUsername = System.Convert.ToInt32(resultCheck);
                    check.Dispose();
                    if (resultUsername > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Uporabniško ime že obstaja.')", true);

                    }
                    else
                    {

                        string finalQueryPermsions = String.Format($"insert into permisions_user(id_permisions_user) VALUES ({next});");
                        SqlCommand createUserPermisions = new SqlCommand(finalQueryPermsions, conn);

                        try
                        {
                            createUserPermisions.ExecuteNonQuery();
                        }
                        catch (Exception error)
                        {
                            var log = error;
                        }
                        createUserPermisions.Dispose();


                        string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");

                        string companyINSERT = companiesList.SelectedItem.Value;
                        int idCOMPANY = getIdCompany(companyINSERT);
                        var nonQuery = new SqlConnection(connection);
                        nonQuery.Open();
                        string finalQueryRegistration = String.Format($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company, ViewState, FullName, email, id_permision_user) VALUES ('{TxtUserName.Text}', '{HashedPassword}', '{userRole.SelectedValue}', '{next}', '{idCOMPANY}','{userTypeRadio.SelectedValue}','{TxtName.Text}', '{email.Text}', {next})");
                        SqlCommand createUser = new SqlCommand(finalQueryRegistration, nonQuery);
                        var username = TxtUserName.Text;
                        try
                        {
                            var id = getIdCompany(companiesList.SelectedValue);
                            createUser.ExecuteNonQuery();
                            createUser.Dispose();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(false, 'Uspešno kreiran uporabnik.')", true);

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
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);

                            TxtName.Text = "";
                            TxtPassword.Text = "";
                            TxtRePassword.Text = "";
                            TxtUserName.Text = "";
                            email.Text = "";

                        }
                    }
                    cmd.Dispose();
                    conn.Close();
                }
            }
            else
            {
                conn.Close();
                conn.Open();
                if (!String.IsNullOrEmpty(TxtPassword.Text))
                {
                    HashedPasswordEdit = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");
                    cmdEdit = new SqlCommand($"UPDATE Users set Pwd='{HashedPasswordEdit}', userRole='{userRole.SelectedValue}', ViewState='{userTypeRadio.SelectedValue}', FullName='{TxtName.Text}' where uname='{TxtUserName.Text}'", conn);

                }
                else
                {
                    cmdEdit = new SqlCommand($"UPDATE Users set userRole='{userRole.SelectedValue}', ViewState='{userTypeRadio.SelectedValue}', FullName='{TxtName.Text}' where uname='{TxtUserName.Text}'", conn);

                }


                conn = new SqlConnection(connection);
             
                //  debug.Add(dev);

                if (TxtPassword.Text != TxtRePassword.Text)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Gesla niso ista.')", true);
                    TxtPassword.Text = "";
                    TxtRePassword.Text = "";
                }
                else
                {

                    try
                    {
                        var username = TxtUserName.Text.Replace(" ", string.Empty); ;
                        cmdEdit.ExecuteNonQuery();
                        conn.Close();
                        cmdEdit.Dispose();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(false, 'Uspešno spremenjeni podatki.')", true);

                        TxtName.Text = "";
                        TxtPassword.Text = "";
                        TxtRePassword.Text = "";
                        TxtUserName.Text = "";
                        email.Text = "";
                        var company = companiesList.SelectedValue.Replace(" ", string.Empty);
                        



                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
                        var debug = ex.Message;
                        TxtName.Text = "";
                        TxtPassword.Text = "";
                        TxtRePassword.Text = "";
                        TxtUserName.Text = "";
                        email.Text = "";

                    }
                    cmdEdit.Dispose();
                    conn.Close();
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
            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"Select count(*) from companies", conn);
            var result = cmd.ExecuteScalar();
            Int32 next = System.Convert.ToInt32(result) + 1;
            conn = new SqlConnection(connection);
            conn.Open();
            cmd = new SqlCommand($"INSERT INTO companies(id_company, company_name, company_number, website, admin_id, databaseName) VALUES({next}, '{companyName.Text}', {companyNumber.Text}, '{website.Text}', '{listAdmin.SelectedValue}', '{ConnectionStrings.SelectedValue}')", conn);
            var debug = $"INSERT INTO companies(id_company, company_name, company_number, website, admin_id, databaseName) VALUES({next}, '{companyName.Text}', {companyNumber.Text}, '{website.Text}', '{listAdmin.SelectedValue}', '{ConnectionStrings.SelectedValue}')";
            var adminForCreation = listAdmin.SelectedValue;

            try
            {
                cmd.ExecuteNonQuery();


            }
            catch (Exception error)
            {
                string debugValue = error.Message;
                // Implement logging here.
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake...')", true);

            }


            cmd.Dispose();
            conn.Close();
        }




        protected void companyButton_Click(object sender, EventArgs e)
        {
            if (companyName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Niste vpisali ime podjetja...')", true);

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";
            }
            else if (website.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Niste vpisali spletno stran...')", true);

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";

            }
            else if (!checkIfNumber(companyNumber.Text))
            {
                Response.Write($"<script type=\"text/javascript\">alert('Številka ni v pravi obliki.'  );</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Številka ni v pravi obliki.')", true);

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";
            }
            else
            {
                insertCompany();
                fillCompaniesRegistration();
                createAdminForTheCompany(companyName.Text);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(false, 'Uspešno poslani podatki.')", true);
                var sourceID = companiesGridView.DataSource;
                companiesGridView.DataSource = null;
                companiesGridView.DataSource = sourceID;
                //fillCompanies();
                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";

            }
        }

        private void createAdminForTheCompany(string name)
        {
            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT MAX(id_permision_user) FROM Users;", conn);
            var result = cmd.ExecuteScalar();
            Int32 Total_ID = System.Convert.ToInt32(result);
            cmd.Dispose();
            int next = Total_ID + 1;




            string finalQueryPermsions = String.Format($"insert into permisions_user(id_permisions_user) VALUES ({next});");
            SqlCommand createUserPermisions = new SqlCommand(finalQueryPermsions, conn);

            try
            {
                createUserPermisions.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                var log = error;
            }
            createUserPermisions.Dispose();

            string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(name, "SHA1");

            string companyINSERT = name;
            int idCOMPANY = getIdCompany(companyINSERT);
            var nonQuery = new SqlConnection(connection);
            nonQuery.Open();
            string finalQueryRegistration = String.Format($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company, ViewState, FullName, email, id_permision_user) VALUES ('{name}', '{HashedPassword}', 'Admin', '{next}', '{idCOMPANY}','Viewer&Designer','{name}', '{name}@{name}.com', {next})");



            SqlCommand createUser = new SqlCommand(finalQueryRegistration, nonQuery);
            var username = TxtUserName.Text;
            try
            {
                var id = getIdCompany(companiesList.SelectedValue);
                createUser.ExecuteNonQuery();
                createUser.Dispose();

                FillListAdmin();
                FillUsers(id);
            } catch(Exception) { 


            }
            
            
            }

        protected void deleteUser_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"delete from Users where uname='{usersGridView.GetSelectedFieldValues("uname")}'", conn);
            var debug = $"delete from Users where uname='{usersGridView.GetSelectedFieldValues("uname")}'";
            deletedID = usersGridView.GetSelectedFieldValues("uname").ToString();
            getIdPermision();
            try
            {
                var company = getCompanyQuery(usersGridView.GetSelectedFieldValues("uname").ToString());
                var spacelessCompany = company.Replace(" ", string.Empty);
                idFromString = getIdCompany(spacelessCompany);

                cmd.ExecuteNonQuery();
                Response.Write($"<script type=\"text/javascript\">alert('Uspešno brisanje.'  );</script>");
                string filePath = Server.MapPath($@"~/App_Data/{spacelessCompany}/{usersGridView.GetSelectedFieldValues("uname")}");
                string finalPath = filePath.Replace(" ", string.Empty);
                FillListGraphs();
                List<String> values = FillListGraphsNames();
                showConfig(values);
                deletePermisionEntry();
            }


            catch (Exception error)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake...')", true);

            }

            FillUsers(idFromString);

            cmd.Dispose();

            conn.Close();
        }
        private string get_connectionStringName(string name)
        {
            //var s = name.Replace(" ", string.Empty);
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);

            conn.Open();
            SqlCommand cmd = new SqlCommand($"select databaseName from companies where company_name='{name}'", conn);

            try
            {
                string result = cmd.ExecuteScalar().ToString();
                returnString = result;

            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();
            return returnString;
        }
    

        protected void usersGridView_SelectedIndexChanged(object sender, EventArgs e)
        {

            TxtUserName.Enabled = false;
            email.Enabled = false;
            graphsGridView.Enabled = true;
            FillListGraphs();
            List<String> values = FillListGraphsNames();
            showConfig(values);
            updateForm();
        }

        public List<String> FillListGraphsNames()
        {


            try
            {
                graphList.Clear();
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection(connection);
                conn.Open();

                // Create SqlCommand to select pwd field from users table given supplied userName.

                cmd = new SqlCommand($"SELECT Caption from Dashboards;", conn);

                // Intepolation or the F string. C# > 5.0    

                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    graphList.Add(sdr["Caption"].ToString());
                    string trimmed = sdr["Caption"].ToString();
                    string stripped = String.Concat(trimmed.ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");
                    // Refils potential new tables.
                    // finalQuery = String.Format($"ALTER TABLE permisions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                    values.Add(stripped);
                }
                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
                var log = ex;
            }

            return values;


        }


        private string getCompanyQuery(string uname)
        {


            conn = new SqlConnection(connection);
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT uname, company_name FROM Users INNER JOIN companies ON Users.id_company = companies.id_company WHERE uname='{uname}';", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                companyInfo = (reader["company_name"].ToString());
            }
            var final = companyInfo.Replace(" ", string.Empty);
            return final;
        }


        private void makeSQLquery(int numberOfRows)
        {
            conn = new SqlConnection(connection);
            conn.Open();
            for (int i = 0; i < numberOfRows; i++)
            {
                var tempGraphString = values.ElementAt(i);

                var name = usersGridView.GetSelectedFieldValues("uname");
                var singular = name[0].ToString();
                findId = String.Format($"SELECT id_permision_user from Users where uname='{singular}'");
                // execute query
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand(findId, conn);
                try
                {
                    id = cmd.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake...')", true);


                    // error handling
                }
                Int32 Total_ID = System.Convert.ToInt32(id);
                if (graphsGridView.Selection.IsRowSelected(i) == true)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
                finalQuerys = String.Format($"UPDATE permisions_user SET {tempGraphString}={flag} WHERE id_permisions_user={Total_ID};");
                cmd = new SqlCommand(finalQuerys, conn);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    continue;
                }

            }
            cmd.Dispose();
            conn.Close();
        }

        private void makeSQLqueryByUser()
        {
            conn = new SqlConnection(connection);
            conn.Open();
            for (int i = 0; i < byUserListBox.SelectedValues.Count; i++)
            {
                var tempGraphStringFullOfStuff = byUserListBox.SelectedValues[i].ToString();
                string trimmedless = String.Concat(tempGraphStringFullOfStuff.Where(c => !Char.IsWhiteSpace(c)));
                string trimmed = trimmedless.Replace("-", "");
                find = String.Format($"SELECT id_permision_user from Users where uname='{trimmed}'");
                // execute query
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand(find, conn);
                try
                {
                    id = cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    continue;
                }
                Int32 Total_ID = System.Convert.ToInt32(id);

                for (int j = 0; j < graphsGridView.VisibleRowCount; j++)
                {

                    if (graphsGridView.Selection.IsRowSelected(i) == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }

                    tempGraphString = values.ElementAt(j);

                    finalQuerys = String.Format($"UPDATE permisions_user SET {tempGraphString}={flag} WHERE id_permisions_user={Total_ID};");
                    var debug = finalQuerys;
                    help.Add(debug.ToString());
                    cmd = new SqlCommand(finalQuerys, conn);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Morate izbrati uporabnika.')", true);


                    }
                }
            }
            cmd.Dispose();
            conn.Close();
        }


        protected void saveGraphs_Click(object sender, EventArgs e)
        {
            if (usersGridView.GetSelectedFieldValues() == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Morate izbrati uporabnika.')", true);

            }
            else
            {
                List<String> values = FillListGraphsNames();
                makeSQLquery(values.Count);
                showConfig(values);
            }
        }
        private void getIdPermision()
        {
            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{deletedID}'", conn);
            try
            {
                var result = cmd.ExecuteScalar();
                permisionID = System.Convert.ToInt32(result);
            }
            catch (Exception error)
            {
                var log = error;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);

            }


            cmd.Dispose();
            conn.Close();

        }



        private void deletePermisionEntry()
        {

            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd1 = new SqlCommand($"DELETE FROM permisions_user WHERE id_permisions_user={permisionID}", conn);
            var final = $"DELETE FROM permisions_user WHERE id_permisions_user={permisionID}";
            try
            {
                var result = cmd1.ExecuteScalar();
                Int32 Total_ID = System.Convert.ToInt32(result);
            }
            catch (Exception error)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                var log = error;
            }
            cmd1.Dispose();
            conn.Close();
        }



        private string getCurrentCompany()
        {
            var company =  companiesGridView.GetSelectedFieldValues("company_name");
            return company[0].ToString();
        }

        protected void deleteCompany_Click(object sender, EventArgs e)
        {
            if (companiesGridView.FocusedRowIndex != -1)
            {
                var current = Session["current"].ToString();
                var id = getIdCompany(current);
                deleteMemberships(id);
                conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand user = new SqlCommand($"delete from users where id_company={id}", conn);

                var deb = $"delete from users where id_company={id}";


                try
                {
                    user.ExecuteNonQuery();

                }
                catch (Exception error)
                {
                    var log = error;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);

                }
                conn = new SqlConnection(connection);

                conn.Open();
                SqlCommand cmd = new SqlCommand($"DELETE FROM companies WHERE company_name='{current}'", conn);
                string dev = $"DELETE FROM companies WHERE company_name='{current}'";
                try
                {
                    cmd.ExecuteNonQuery();


                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(false, 'Uspešno brisanje.')", true);
                    var source = companiesGridView.DataSource;
                    companiesGridView.DataSource = null;
                    companiesGridView.DataSource = source;

                }
                catch (Exception error)
                {


                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                    var log = error;
                }

                FillListGraphs();
                /// fillCompanies();
                FillListAdmin();
                cmd.Dispose();
                conn.Close();



                FillUsers(1);
            }
        }

        private int getIdCompany(string current)
        {
            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_company from companies where company_name='{current}'", conn);
            try
            {
                result = cmd.ExecuteScalar();
            }
            catch (Exception error)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                var log = error;
            }

            cmd.Dispose();
            conn.Close();

            int finalID = System.Convert.ToInt32(result);

            return finalID;


        }

        private void deleteMemberships(int number)
        {

            var final = getCurrentCompany();
            conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"DELETE FROM memberships WHERE id_company={number}", conn);
            string dev = $"DELETE FROM companies WHERE company_name='{number}'";



            try
            {
                cmd.ExecuteNonQuery();
                // fillUsersDelete();
                // fillCompanyDelete();
            }


            catch (Exception error)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                var log = error;
            }



            cmd.Dispose();
            conn.Close();
        }



        protected void byUser_Click(object sender, EventArgs e)
        {
            if (by.Visible == true)
            {
                by.Visible = false;
            }
            else
            {
                by.Visible = true;
            }
        }

        protected void saveByuser_Click(object sender, EventArgs e)
        {

            if (graphsGridView.GetSelectedFieldValues() == null | byUserListBox.SelectedValues == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Morate izbrati uporabnike in graf.')", true);
            }

            else
            {
                FillListGraphsNames();
                makeSQLqueryByUser();
                showConfigByUser();
            }
        }

        private void showConfigByUser()
        {
            columnNames.Clear();
            config.Clear();
            conn = new SqlConnection(connection);
            conn.Open();

            // DECLARE @ColList Varchar(1000), @SQLStatment VARCHAR(4000)
            // SET @ColList = ''
            // select @ColList = @ColList + Name + ' , ' from syscolumns where id = object_id('permisions') AND Name != 'id_permisions'
            // SELECT @SQLStatment = 'SELECT ' + Substring(@ColList, 1, len(@ColList) - 1) + 'FROM permisions'
            // EXEC(@SQLStatment

            if (byUserListBox.SelectedValues[0] != null)
            {
                findIdString = String.Format($"SELECT id_permision_user from Users where uname='{byUserListBox.SelectedValues[0]}'");
            }
            else
            {
                byUserListBox.SelectedIndex = 0;
                findIdString = String.Format($"SELECT id_permision_user from Users where uname='{byUserListBox.SelectedValues[0]}'");

            }
            // Documentation. This query is for getting all the permision table data from the user
            cmd = new SqlCommand(findIdString, conn);
            idNumber = cmd.ExecuteScalar();
            Int32 Total_Key = System.Convert.ToInt32(idNumber);

            conn.Close();
            conn.Dispose();
            permisionQuery = $"SELECT * FROM permisions_user WHERE id_permisions_user={Total_Key}";
            cmd = new SqlCommand(permisionQuery, conn);
            conn = new SqlConnection(connection);

            using (SqlConnection connection = new SqlConnection(
              this.connection))
            {
                SqlCommand command = new SqlCommand(permisionQuery, connection);
                connection.Open();
                SqlDataReader reader =
                command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        int bitValueTemp = (int)(reader[values[i]] as int? ?? 0);
                        if (bitValueTemp == 1)
                        {

                            graphsGridView.Selection.SetSelection(i, true);
                            valuesBool.Add(true);
                        }
                        else
                        {
                            graphsGridView.Selection.SetSelection(i, false);
                            valuesBool.Add(false);
                        }
                    }
                }

                conn.Close();
                conn.Dispose();
            }
        }


        protected void byUserListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void newUser_Click(object sender, EventArgs e)
        {
            usersGridView.Selection.SetSelection(-1, true);
            TxtUserName.Enabled = true;
            email.Enabled = true;
            TxtUserName.Text = "";
            TxtName.Text = "";
            email.Text = "";
            TxtPassword.Text = "";
            TxtRePassword.Text = "";
        }

        protected void AddConnection_Click(object sender, EventArgs e)
        {
            var _stringDB = GetResultFromDBTest(ConnectionString.Text);
          
            if (connName.Text == null)
            {

                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(true, 'Morate napisati ime.')", true);
            }
            else

            {


                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "notify(false, 'Dodana konekcija.')", true);

                AddConnectionString(ConnectionString.Text);
                FillListAdmin();
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl);
                // Unit testing.
            }


        }

        private void AddConnectionString(string stringConnection)
        {

            Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

            var builder = new SqlConnectionStringBuilder(stringConnection);

            ConnectionStringSettings conn = new ConnectionStringSettings();

            conn.ConnectionString = builder.ConnectionString;

            conn.Name = connName.Text;

            config.ConnectionStrings.ConnectionStrings.Add(conn);

            config.Save(ConfigurationSaveMode.Modified, true);

        }

        private string GetResultFromDBTest(string connectionString)
        {
            CheckConnection TestConnection = new CheckConnection();
            var _result = TestConnection.check_connection(connectionString);
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



        protected void new_user_ServerClick2(object sender, EventArgs e)
        {

            TxtUserName.Enabled = true;
            email.Enabled = true;
            TxtUserName.Text = "";
            TxtName.Text = "";
            email.Text = "";
            TxtPassword.Text = "";
            TxtRePassword.Text = "";


            // Call the client.

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showDialogSync()", true);

        }

        protected void usersGridView_SelectionChanged(object sender, EventArgs e)
        {
            TxtUserName.Enabled = false;
            email.Enabled = false;
            graphsGridView.Enabled = true;
            FillListGraphs();
            List<String> values = FillListGraphsNames();
            showConfig(values);
            updateForm();
        }



     
    }
}
