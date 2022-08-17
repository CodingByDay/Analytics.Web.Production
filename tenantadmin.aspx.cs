using Dash.Log;
using Dash.ORM;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Dash
{
    public partial class tenantadmin : System.Web.UI.Page
    {
        private string connection;

        private List<string> permisionsReturn = new List<string>();
        private List<User> userObjectList = new List<User>();
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
        private List<String> usersData = new List<string>();
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
        private List<String> CurrentPermisionID = new List<string>();
        private string role;
        private string admin;
        private SqlConnection connectionSQL;

        protected void Page_Load(object sender, EventArgs e)
        {
            connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
            usersGridView.SettingsBehavior.AllowFocusedRow = true;
            usersGridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
            usersGridView.SettingsBehavior.AllowSelectByRowClick = true;
            usersGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
            usersGridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            usersGridView.EnableCallBacks = false;
            usersGridView.StartRowEditing += UsersGridView_StartRowEditing;
            namesGridView.RowUpdating += NamesGridView_RowUpdating;
            namesGridView.RowUpdated += NamesGridView_RowUpdated;

            if (!IsPostBack)
            {
                authenticate();
                HtmlAnchor admin = Master.FindControl("backButtonA") as HtmlAnchor;
                admin.Visible = true;
                defaultCompany();
                FillUsers();
                FillListGraphs();
                graphsListBox.Enabled = false;
                fillCompaniesRegistration();
                defaultCompany();
                typesOfViews.Add("Viewer");
                typesOfViews.Add("Designer");
                typesOfViews.Add("Viewer&Designer");
                if (userObjectList.Count != 0)
                {
                    usersGridView.Selection.SetSelection(0, true);
                }
            }
            else
            {
                HtmlAnchor admin = Master.FindControl("backButtonA") as HtmlAnchor;
                admin.Visible = true;
                FillUsers();
            }
        }

        private void NamesGridView_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            string uname = HttpContext.Current.User.Identity.Name;
        }

        private void NamesGridView_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                string uname = HttpContext.Current.User.Identity.Name;
                string name = getCompanyQuery(uname);
                int id = getIdCompany(name);
                Graph graph = new Graph(id);
                var payload = graph.GetNames(id);
                var s = e.OldValues;
                var names = graph.getNamesCurrent(id);

                names.FirstOrDefault(x => x.original == e.OldValues[0].ToString()).custom = e.NewValues[1].ToString();

                graph.UpdateGraphs(names, id);
                var data_payload = graph.GetGraphs(id);
                namesGridView.AutoGenerateColumns = false;
                namesGridView.DataSource = null;
                namesGridView.DataSource = data_payload;
                namesGridView.EndUpdate();
                namesGridView.DataBind();
                e.Cancel = true;
                namesGridView.CancelEdit();
                updateControl();
            }
            catch
            {
            }
        }

        private void updateControl()
        {
            var source = namesGridView.DataSource;
            namesGridView.DataSource = null;
            namesGridView.DataSource = source;
            namesGridView.DataBind();
            DevExpress.Web.ASPxWebControl.RedirectOnCallback(Page.Request.Url.ToString());
        }

        private void UsersGridView_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            var name = e.EditingKeyValue;
            updateFormName(name.ToString());
            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "showDialogSync()", true);
            e.Cancel = true;
        }

        public string GetCompanyName(int company)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand($"SELECT company_name FROM companies WHERE id_company={company}", conn); /// Intepolation or the F string. C# > 5.0
                    try
                    {
                        admin = (string)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
            return admin;
        }

        private void updateFormName(string name)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE uname='{name}'", conn);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        TxtName.Text = sdr["FullName"].ToString();
                        TxtUserName.Text = sdr["uname"].ToString();
                        TxtUserName.Enabled = false;
                        var number = (int)sdr["id_company"];
                        var data = GetCompanyName(number);
                        companiesList.SelectedValue = data;

                        companiesList.Enabled = false;
                        email.Enabled = false;
                        string role = sdr["userRole"].ToString();
                        string type = sdr["ViewState"].ToString();
                        email.Text = sdr["email"].ToString();

                        userRole.SelectedIndex = userRole.Items.IndexOf(userRole.Items.FindByValue(role));
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
        }

        protected void newUser_Click(object sender, EventArgs e)
        {
            //usersGridView.SelectedIndex = -1;
            TxtUserName.Enabled = true;
            email.Enabled = true;
            TxtUserName.Text = "";
            TxtName.Text = "";
            email.Text = "";
            TxtPassword.Text = "";
            TxtRePassword.Text = "";
        }

        private void authenticate()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var username = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"select userRole from Users where uname='{username}';", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        role = (reader["userRole"].ToString());
                    }
                    if (role == "Admin")
                    {
                    }
                    else
                    {
                        Response.Redirect("logon.aspx", true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
        }

        private List<bool> showConfig()
        {
            string singular;
            valuesBool.Clear();
            columnNames.Clear();
            config.Clear();
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    if (usersGridView.FocusedRowIndex >= 0)
                    {
                        var plural = usersGridView.GetSelectedFieldValues("uname");
                        if (plural.Count==0)
                        {
                            usersGridView.Selection.SelectRow(0);
                            var plural_new = usersGridView.GetSelectedFieldValues("uname");
                            singular = plural_new[0].ToString();

                        } else
                        {
                            var plural_new = usersGridView.GetSelectedFieldValues("uname");
                            singular = plural_new[0].ToString();
                        }

                        findIdString = String.Format($"SELECT id_permision_user from Users where uname='{singular}'");
                        cmd = new SqlCommand(findIdString, conn);
                        try
                        {
                            idNumber = cmd.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                        }
                    }
                    else
                    {
                        var plural = usersGridView.GetSelectedFieldValues("uname");
                        singular = plural[0].ToString();
                        usersGridView.Selection.SetSelection(0, true);
                        findIdString = String.Format($"SELECT id_permision_user from Users where uname='{singular}'");
                        cmd = new SqlCommand(findIdString, conn);
                        try
                        {
                            idNumber = cmd.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
            Int32 Total_Key = System.Convert.ToInt32(idNumber);
            permisionQuery = $"SELECT * FROM permisions_user WHERE id_permisions_user={Total_Key}";
            using (SqlConnection connection = new SqlConnection(
              this.connection))
            {
                SqlCommand command = new SqlCommand(permisionQuery, connection);
                connection.Open();
                SqlDataReader reader =
                command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    for (int i = 0; i < CurrentPermisionID.Count; i++)
                    {
                        int bitValueTemp = (int)(reader[CurrentPermisionID[i]] as int? ?? 0);
                        var debug = CurrentPermisionID[i].ToString();
                        if (bitValueTemp == 1)
                        {
                            graphsListBox.Items.ElementAt(i).Selected = true;
                            
                            valuesBool.Add(true);
                        }
                        else
                        {
                            graphsListBox.Items.ElementAt(i).Selected = false;
                            valuesBool.Add(false);
                        }
                    }
                }
            }

            return valuesBool;
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
                    cmd = new SqlCommand($"SELECT Caption from Dashboards;", conn);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        graphList.Add(sdr["Caption"].ToString());
                        string trimmed = String.Concat(sdr["Caption"].ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");
                        // Refils potential new tables.
                        // finalQuery = String.Format($"ALTER TABLE permisions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                        values.Add(trimmed);
                    }
                    CurrentPermisionID = getIdPermisionCurrentUser(UserNameForChecking, graphList);
                    graphsListBox.DataSource = CurrentPermisionID;
                    graphsListBox.DataBind();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                }
            } 
        }

        public void FillUsers()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    //Perform DB operation here i.e. any CRUD operation
                    userObjectList.Clear();
                    string uname = HttpContext.Current.User.Identity.Name;
                    string name = getCompanyQuery(uname);
                    var company = defaultCompany();
                    var id = getIdCompany(name);
                    usersData.Clear();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"Select * from Users where id_company={id}", conn);

                    /// Intepolation or the F string. C# > 5.0
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        User user = new User(sdr["uname"].ToString(), sdr["Pwd"].ToString(), sdr["userRole"].ToString(), sdr["ViewState"].ToString(), sdr["email"].ToString());
                        var test = user.uname;
                        userObjectList.Add(user);
                    }

                    usersGridView.DataSource = null;
                    usersGridView.DataSource = userObjectList;
                    usersGridView.DataBind();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                    Response.Write(ex.ToString());
                }
            }
        }

        private void fillCompaniesRegistration()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand("Select * from companies", conn); /// Intepolation or the F string. C# > 5.0
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
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
        }

        private void updateForm()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var plural = usersGridView.GetSelectedFieldValues("uname");

                    var singular = plural[0].ToString();

                    SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE uname='{singular}'", conn);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        TxtName.Text = sdr["FullName"].ToString();
                        TxtUserName.Text = sdr["uname"].ToString();
                        TxtUserName.Enabled = false;
                        var company = defaultCompany();
                        string uname = HttpContext.Current.User.Identity.Name;
                        string name = getCompanyQuery(uname);
                        int id = getIdCompany(name);
                        var data = GetCompanyName(id);
                        companiesList.SelectedValue = data;
                        companiesList.Enabled = false;
                        email.Enabled = false;
                        string role = sdr["userRole"].ToString();
                        string type = sdr["ViewState"].ToString();
                        email.Text = sdr["email"].ToString();
                        userRole.SelectedIndex = userRole.Items.IndexOf(userRole.Items.FindByValue(role));
                        userTypeList.SelectedIndex = userTypeList.Items.IndexOf(userTypeList.Items.FindByValue(type));
                    }
                    sdr.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
        }

        private string defaultCompany()
        {
            string uname = HttpContext.Current.User.Identity.Name;
            string name = getCompanyQuery(uname);
            int id = getIdCompany(name);

            Graph graph = new Graph(id);

            var payload = graph.AllGraphs;

            namesGridView.AutoGenerateColumns = false;
            namesGridView.DataSource = payload;
            namesGridView.DataBind();

            var data = GetCompanyName(id);
            companiesList.SelectedValue = data;
            companiesList.Enabled = false;
            return name;
        }

        protected void registrationButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    if (TxtUserName.Enabled == true)
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand($"SELECT MAX(id_permisions_user) FROM permisions_user;", conn);
                        var result = cmd.ExecuteScalar();
                        Int32 Total_ID = System.Convert.ToInt32(result);

                        int next = Total_ID + 1;
                        if (TxtPassword.Text != TxtRePassword.Text)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Gesla niso ista. Poskusite še enkrat!')", true);

                            TxtPassword.Text = "";
                            TxtRePassword.Text = "";
                        }
                        else
                        {
                            SqlCommand check = new SqlCommand($"Select count(*) from Users where uname='{TxtUserName.Text}'", conn);
                            var resultCheck = check.ExecuteScalar();
                            Int32 resultUsername = System.Convert.ToInt32(resultCheck);
                            if (resultUsername > 0)
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Uporabniško ime že obstaja.')", true);
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
                                    Response.Write(error.ToString());
                                }
                                string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");

                                createUserPermisions.Dispose();
                                var companyIndex = getIdCompany(companiesList.SelectedValue);
                                string finalQueryRegistration = String.Format($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company, ViewState, FullName, email, id_permision_user) VALUES ('{TxtUserName.Text}', '{HashedPassword}', '{userRole.SelectedValue}', '{next}', '{companyIndex}','{userTypeList.SelectedValue}','{TxtName.Text}', '{email.Text}', '{next}')");
                                SqlCommand createUser = new SqlCommand(finalQueryRegistration, conn);
                                var username = TxtUserName.Text;
                                try
                                {
                                    var id = getIdCompany(companiesList.SelectedValue);
                                    createUser.ExecuteNonQuery();

                                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno kreiran uporabnik.')", true);
                                    Logger.LogInfo(typeof(tenantadmin), "Uspešno kreiran uporabnik.");

                                    TxtName.Text = "";
                                    TxtPassword.Text = "";
                                    TxtRePassword.Text = "";
                                    TxtUserName.Text = "";
                                    email.Text = "";
                                    FillUsers();
                                    var company = companiesList.SelectedValue;
                                    var spacelessCompany = company.Replace(" ", string.Empty);
                                    createUser.Dispose();
                                }
                                catch (SqlException ex) when (ex.Number == 2627)
                                {
                                    // Implement logging here.
                                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'To uporabniško ime že obstaja, prosimo probajte še enkrat.')", true);

                                    TxtName.Text = "";
                                    TxtPassword.Text = "";
                                    TxtRePassword.Text = "";
                                    TxtUserName.Text = "";
                                    email.Text = "";
                                }
                            }
                        }
                    }
                    else
                    {
                        conn.Open();
                        string HashedPasswordEdit = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");

                        var dev = $"UPDATE Users set Pwd='{HashedPasswordEdit}', userRole='{userRole.SelectedValue}', ViewState='{userTypeList.SelectedValue}', FullName='{TxtName.Text}', where uname='{TxtUserName.Text}'";
                        //  debug.Add(dev);
                        SqlCommand cmd;
                        if (!String.IsNullOrEmpty(TxtRePassword.Text))
                        {
                            HashedPasswordEdit = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");
                            cmd = new SqlCommand($"UPDATE Users set Pwd='{HashedPasswordEdit}', userRole='{userRole.SelectedValue}', ViewState='{userTypeList.SelectedValue}', FullName='{TxtName.Text}' where uname='{TxtUserName.Text}'", conn);
                        }
                        else
                        {
                            cmd = new SqlCommand($"UPDATE Users set userRole='{userRole.SelectedValue}', ViewState='{userTypeList.SelectedValue}', FullName='{TxtName.Text}' where uname='{TxtUserName.Text}'", conn);
                        }
                        if (TxtPassword.Text != TxtRePassword.Text)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Gesla niso ista. Poskusite še enkrat!')", true);
                            TxtPassword.Text = "";
                            TxtRePassword.Text = "";
                        }
                        else
                        {
                            try
                            {
                                var username = TxtUserName.Text.Replace(" ", string.Empty); ;
                                cmd.ExecuteNonQuery();
                                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno spremenjeni podatki.')", true);
                                TxtName.Text = "";
                                TxtPassword.Text = "";
                                TxtRePassword.Text = "";
                                TxtUserName.Text = "";
                                email.Text = "";
                                var company = companiesList.SelectedValue.Replace(" ", string.Empty); ;
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);

                                Response.Write($"<script type=\"text/javascript\">alert('Napaka... {ex.Message}');</script>");

                                TxtName.Text = "";
                                TxtPassword.Text = "";
                                TxtRePassword.Text = "";
                                TxtUserName.Text = "";
                                email.Text = "";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
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

        public void FillListGraphsNames()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    graphList.Clear();
                    string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */

                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"SELECT Caption from Dashboards;", conn);

                    /// Intepolation or the F string. C# > 5.0
                    // Execute command and fetch pwd field into lookupPassword string.
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        graphList.Add(sdr["Caption"].ToString());
                        string trimmed = String.Concat(sdr["Caption"].ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");

                        // Refils potential new tables.
                        // finalQuery = String.Format($"ALTER TABLE permisions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                        values.Add(trimmed);
                    }

                    List<String> CurrentPermisionID = getIdPermisionCurrentUser(UserNameForChecking, graphList);

                    graphsListBox.DataSource = CurrentPermisionID;
                    graphsListBox.DataBind();

                    //Perform DB operation here i.e. any CRUD operation
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Napaka...')", true);
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
                    cmd = new SqlCommand($"SELECT uname, company_name FROM Users INNER JOIN companies ON Users.id_company = companies.id_company WHERE uname='{uname}';", conn);
                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            companyInfo = (reader["company_name"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
            return companyInfo;
        }

        private void makeSQLquery()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    for (int i = 0; i < graphsListBox.Items.Count; i++)
                    {
                        var item_check = graphsListBox.Items.ElementAt(i);

                        var tempGraphString = values.ElementAt(values.IndexOf(item_check.Text));
                        var plural = usersGridView.GetSelectedFieldValues("uname");
                        var singular = plural[0].ToString();
                        findId = String.Format($"SELECT id_permision_user from Users where uname='{singular}'");
                        // execute query
                        // Create SqlCommand to select pwd field from users table given supplied userName.
                        cmd = new SqlCommand(findId, conn);
                        try
                        {
                            id = cmd.ExecuteScalar();
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        Int32 Total_ID = System.Convert.ToInt32(id);



                        var item = graphsListBox.Items.ElementAt(i);

                        if (graphsListBox.Items.ElementAt(i).Selected == true)
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
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception)
                {
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
                FillListGraphsNames();
                makeSQLquery();
                showConfig();
            }
        }

        private void getIdPermision()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{deletedID}'", conn);

                    try
                    {
                        var result = cmd.ExecuteScalar();
                        permisionID = System.Convert.ToInt32(result);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);

                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
        }

        private List<String> getIdPermisionCurrentUser(string uname, List<String> obj)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    permisionsReturn.Clear();
                    conn.Open();
                    permisionsReturn = new List<string>();

                    SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{uname}'", conn);

                    var result = cmd.ExecuteScalar();
                    permisionID = System.Convert.ToInt32(result);

                    int idUser = permisionID;

                    foreach (String graph in obj)
                    {
                        string whiteless = String.Concat(graph.Where(c => !Char.IsWhiteSpace(c)));
                        string stripped = whiteless.Replace("-", "");
                        SqlCommand graphResult = new SqlCommand($"select {stripped} from permisions_user where id_permisions_user={idUser}", conn);
                        string deb = $"select {stripped} from permisions_user where id_permisions_user={idUser}";
                        try
                        {
                            var resultID = graphResult.ExecuteScalar();
                            permisionID = System.Convert.ToInt32(resultID);

                            if (permisionID == 1)
                            {
                                permisionsReturn.Add(graph);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
            return permisionsReturn;
        }

        private void deletePermisionEntry()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand($"DELETE FROM permisions_user WHERE id_permisions_user={permisionID}", conn);
                    var final = $"DELETE FROM permisions WHERE id_permisions={permisionID}";
                    var result = cmd1.ExecuteScalar();
                    Int32 Total_ID = System.Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                }
            }
        }

        protected void deleteCompany_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var id = getIdCompany(current);
                    deleteMemberships(id);
                    SqlCommand user = new SqlCommand($"delete from users where id_company={id}", conn);
                    user.ExecuteNonQuery();
                    SqlCommand cmd = new SqlCommand($"DELETE FROM companies WHERE company_name='{current}'", conn);
                    string dev = $"DELETE FROM companies WHERE company_name='{current}'";
                    try
                    {
                        cmd.ExecuteNonQuery();
                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno brisanje.')", true);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                    }

                    FillListGraphs();
                    FillUsers();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                }
            }
        }

        private int getIdCompany(string current)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"select id_company from companies where company_name='{current}'", conn);
                    result = cmd.ExecuteScalar();
                    var finalID = System.Convert.ToInt32(result);
                    return finalID;
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
                    return -1;
                }
            }
        }

        private void deleteMemberships(int number)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var final = defaultCompany();
                    int idCompany = getIdCompany(final);
                    SqlCommand cmd = new SqlCommand($"DELETE FROM memberships WHERE id_company={idCompany}", conn);
                    string dev = $"DELETE FROM companies WHERE company_name='{idCompany}'";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);
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

                    var plural = usersGridView.GetSelectedFieldValues("uname");

                    var singular = plural[0].ToString();
                    SqlCommand cmd = new SqlCommand($"delete from Users where uname='{singular}'", conn);

                    try
                    {
                        var company = getCompanyQuery(singular);
                        var spacelessCompany = company.Replace(" ", string.Empty);
                        cmd.ExecuteNonQuery();
                        usersGridView.Selection.SetSelection(0, true);
                        FillListGraphs();
                        showConfig();
                        deletePermisionEntry();
                        FillUsers();

                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(false, 'Uspešno brisanje.')", true);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);

                        Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(tenantadmin), ex.InnerException.Message);

                    Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "notify(true, 'Prišlo je do napake.')", true);
                }
            }
        }

        protected void byUserListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void usersGridView_SelectionChanged(object sender, EventArgs e)
        {
            graphsListBox.Enabled = true;
            FillListGraphs();
            showConfig();
            updateForm();
        }

        protected void new_user_ServerClick(object sender, EventArgs e)
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

        protected void usersGridView_SelectionChanged1(object sender, EventArgs e)
        {
            graphsListBox.Enabled = true;
            FillListGraphs();
            showConfig();
            updateForm();
        }

        protected void hidden_Click(object sender, EventArgs e)
        {
            var mat = 3;
        }
    }
}