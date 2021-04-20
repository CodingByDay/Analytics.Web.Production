using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace peptak
{
    public partial class AdminPanel : System.Web.UI.Page
    {
        private List<bool> valuesBool = new List<bool>();
        private List<String> columnNames = new List<string>();
        private List<bool> config = new List<bool>();
        private List<String> fileNames = new List<string>();
        private List<String> graphNames = new List<string>();
        private List<String> values = new List<string>();

        private SqlConnection conn;
        private string findIdString;
        private string dev;
        private string permisionQuery;
        private string findId;
        private string finalQuerys;
        private SqlCommand cmd;
        private object idNumber;
        private List<String> companiesData = new List<string>();
        private List<String> usersData = new List<string>();
        private string finalQuery;
        private object id;
        private int flag;
        private string companyInfo;
        private List<String> companies = new List<string>();
        private List<String> typesOfViews = new List<string>();
        private List<String> strings = new List<string>();
        private List<String> admins = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Confirm button, byUser, , delete icon, delete event.
            // Adding create user and create company logic. Adding fforeach for multiple selection listbox.
            if (!IsPostBack)
            {
                companiesListBox.SelectedIndex = 0;
                var beginingID = 1;
                // Consider this.
                FillUsers(beginingID);
                fillCompanies();
                // FillUsers();
                FillListGraphs();
                graphsListBox.Enabled = false;
                fillCompaniesRegistration();
                FillListAdmin();
                // User Types
                typesOfViews.Add("Viewer");
                typesOfViews.Add("Designer");
                typesOfViews.Add("Viewer&Designer");
                userType.DataSource = typesOfViews;
                userType.DataBind();


            }
            else
            {
            }
            
        }


        public void FillListAdmin()
        {
            try
            {

                string UserNameForChecking
                    = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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

                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("select company_name from companies ", conn); /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    strings.Add(reader["company_name"].ToString());

                }

                ConnectionStrings.DataSource = strings;
                ConnectionStrings.DataBind();
                // unit test

                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
            // Implement logging here.    
            }
        }

        private List<bool> showConfig()
        {
            
            valuesBool.Clear();
            columnNames.Clear();
            config.Clear();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            // DECLARE @ColList Varchar(1000), @SQLStatment VARCHAR(4000)
            // SET @ColList = ''
            // select @ColList = @ColList + Name + ' , ' from syscolumns where id = object_id('permisions') AND Name != 'id_permisions'
            // SELECT @SQLStatment = 'SELECT ' + Substring(@ColList, 1, len(@ColList) - 1) + 'FROM permisions'
            // EXEC(@SQLStatment
            findIdString = String.Format($"SELECT id_permisions from Users where uname='{usersListBox.SelectedItem.Text}'");


            // Documentation. This query is for getting all the permision table data from the user
            cmd = new SqlCommand(findIdString, conn);
            idNumber = cmd.ExecuteScalar();
            Int32 Total_Key = System.Convert.ToInt32(idNumber);

            conn.Close();
            conn.Dispose();
            permisionQuery = $"SELECT * FROM permisions WHERE id_permisions={Total_Key}";
            cmd = new SqlCommand(permisionQuery, conn);
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");

            using (SqlConnection connection = new SqlConnection(
              "server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;"))
            {
                SqlCommand command = new SqlCommand(permisionQuery, connection);
                connection.Open();
                SqlDataReader reader =
                command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        bool bitValueTemp = (bool)(reader[values[i]] as bool? ?? false);
                        config.Add(bitValueTemp);
                        if (bitValueTemp == true)
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

                conn.Close();
                conn.Dispose();
                return valuesBool;
            }
        }


        public void FillListGraphs()
        {
            try
            {
                fileNames.Clear();
                string filePath = Server.MapPath("~/App_Data/Dashboards");
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
                System.IO.FileInfo[] fi = di.GetFiles();
                foreach (System.IO.FileInfo file in fi)
                {
                    XDocument doc = XDocument.Load(filePath + "/" + file.Name);
                    var tempXmlName = doc.Root.Element("Title").Attribute("Text").Value;
                    graphNames.Add(tempXmlName);                                //graphNames.Add(file.Name + " " + tempXmlName); oldname

                    string trimmedless = String.Concat(tempXmlName.Where(c => !Char.IsWhiteSpace(c)));
                    string trimmed = trimmedless.Replace("-", "");
                    fileNames.Add(file.Name);
                    // Refils potential new tables.
                    finalQuery = String.Format($"ALTER TABLE permisions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                    values.Add(trimmed);
                    // Execute query.
                    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                    conn.Open();
                    try
                    {
                        cmd = new SqlCommand(finalQuery, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception error)
                    {
                        // Logging
                        continue;
                    }
                }
                graphsListBox.DataSource = graphNames;
                graphsListBox.DataBind();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private int GetCompanyName(string Name)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_company from companies where company_name='{Name}';", conn);
          
                var result = cmd.ExecuteScalar();
                var company = System.Convert.ToInt32(result);



            cmd.Dispose();
            conn.Close();
            return company;

        }


      
        public void FillUsers(int companyID)
        {
            try
            {
                usersData.Clear();
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();

                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"Select uname from Users where id_company={companyID}", conn);

                    /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    usersData.Add(sdr["uname"].ToString());

                }
                usersListBox.DataSource = usersData;
                usersListBox.DataBind();
                cmd.Dispose();
                conn.Close();


            } catch(Exception ex)
            {
                // Logging
            }

            }

        private void fillCompaniesRegistration()
        {
            try
            {
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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


                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {

            }
        }

        private void fillCompanies()
        {

            try
            {
                companiesData.Clear();
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("Select * from companies", conn); /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    companiesData.Add(sdr["company_name"].ToString());

                }
                companiesListBox.DataSource = companiesData;
                companiesListBox.DataBind();


                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {

            }
        }

        private void updateForm()
        {
            ///
            var userRightNow = usersListBox.SelectedItem.Text;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE uname='{userRightNow}'", conn);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                TxtName.Text = sdr["FullName"].ToString();
                TxtUserName.Text = sdr["uname"].ToString();
                TxtUserName.Enabled = false;
                companiesList.Enabled = false;
                email.Enabled = false;
                string pass = sdr["Pwd"].ToString();
                TxtPassword.Text = pass;
                TxtRePassword.Text = TxtPassword.Text;
                string role = sdr["userRole"].ToString();
                string type = sdr["ViewState"].ToString();
                email.Text = sdr["email"].ToString();
                userRole.SelectedIndex = userRole.Items.IndexOf(userRole.Items.FindByValue(role));
                userType.SelectedIndex = userType.Items.IndexOf(userType.Items.FindByValue(type));

            }
            sdr.Close();
            cmd.Dispose();
        }
        //private void fillChange()
        //{
        //    ChooseCompany.Items.Clear();
        //    ChooseUser.Items.Clear();
        //    changeCompanyUser.Clear();
        //    changeUserCompany.Clear();
        //    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
        //    conn.Open();
        //    // Create SqlCommand to select pwd field from users table given supplied userName.
        //    cmd = new SqlCommand("select company_name from companies ", conn); /// Intepolation or the F string. C# > 5.0       
        //    // Execute command and fetch pwd field into lookupPassword string.
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        changeCompanyUser.Add(reader["company_name"].ToString());

        //    }

        //    // unit test

        //    cmd.Dispose();
        //    conn.Close();
        //    ChooseCompany.DataSource = changeCompanyUser;
        //    ChooseCompany.DataBind();
        //    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
        //    conn.Open();
        //    // Create SqlCommand to select pwd field from users table given supplied userName.
        //    cmd = new SqlCommand("select uname from Users ", conn); /// Intepolation or the F string. C# > 5.0       
        //    // Execute command and fetch pwd field into lookupPassword string.
        //    SqlDataReader reader2 = cmd.ExecuteReader();
        //    while (reader2.Read())
        //    {
        //        changeUserCompany.Add(reader2["uname"].ToString());

        //    }

        //    // unit test

        //    cmd.Dispose();
        //    conn.Close();
        //    ChooseUser.DataSource = changeUserCompany;
        //    ChooseUser.DataBind();

        //}



        protected void registrationButton_Click(object sender, EventArgs e)
        {
            if (TxtUserName.Enabled == true)
            {

                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                SqlCommand cmd = new SqlCommand($"Select count(*) from Users", conn);
                var result = cmd.ExecuteScalar();
                Int32 Total_ID = System.Convert.ToInt32(result);

                int next = Total_ID + 1;
                if (TxtPassword.Text != TxtRePassword.Text)
                {
                    Response.Write("<script type=\"text/javascript\">alert('Gesla niso ista. Poskusite še enkrat!');</script>");
                    TxtPassword.Text = "";
                    TxtRePassword.Text = "";
                }
                else
                {
                    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                    conn.Open();
                    SqlCommand check = new SqlCommand($"Select count(*) from Users where uname='{TxtUserName}'", conn);


                    var resultCheck = check.ExecuteScalar();
                    Int32 resultUsername = System.Convert.ToInt32(resultCheck);
                    if (resultUsername > 0)
                    {
                        Response.Write("<script type=\"text/javascript\">alert('Uporabniško ime že obstaja.');</script>");
                    }
                    else
                    {

                        string finalQueryPermsions = String.Format($"insert into permisions(id_permisions) VALUES ({next});");
                        SqlCommand createUserPermisions = new SqlCommand(finalQueryPermsions, conn);

                        try
                        {
                            createUserPermisions.ExecuteNonQuery();
                        }
                        catch (Exception error)
                        {
                            // Logging module.
                        }



                        string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");



                        string finalQueryRegistration = String.Format($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company, ViewState, FullName, email) VALUES ('{TxtUserName.Text}', '{HashedPassword}', '{userRole.SelectedValue}', '{next}', '{companiesList.SelectedIndex + 1}','{userType.SelectedValue}','{TxtName.Text}', '{email.Text}')");
                        SqlCommand createUser = new SqlCommand(finalQueryRegistration, conn);
                        var username = TxtUserName.Text;
                        try
                        {
                            createUser.ExecuteNonQuery();
                            Response.Write($"<script type=\"text/javascript\">alert('Uspešno kreiran uporabnik.');</script>");
                            TxtName.Text = "";
                            TxtPassword.Text = "";
                            TxtRePassword.Text = "";
                            TxtUserName.Text = "";
                            email.Text = "";

                            var company = companiesList.SelectedValue;
                            var spacelessCompany = company.Replace(" ", string.Empty);
                            
                            //fillChange();
                            //fillUsersDelete();
                            string filePath = Server.MapPath($"~/App_Data/{spacelessCompany}/{username}");
                            string replacedPath = filePath.Replace(" ", string.Empty);
                            if (!Directory.Exists(replacedPath.ToString()))
                            {


                                Directory.CreateDirectory(replacedPath.ToString());

                            }
                            else
                            {


                            }
                        }
                        catch (Exception error)
                        {
                            // Implement logging here.
                            Response.Write(error);

                        }
                    }
                }
            }
            else
            {

                string HashedPasswordEdit = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");



                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                var dev = $"UPDATE Users set Pwd='{HashedPasswordEdit}', userRole='{userRole.SelectedValue}', ViewState='{userType.SelectedValue}', FullName='{TxtName.Text}', where uname='{TxtUserName.Text}'";
              //  debug.Add(dev);
                SqlCommand cmd = new SqlCommand($"UPDATE Users set Pwd='{HashedPasswordEdit}', userRole='{userRole.SelectedValue}', ViewState='{userType.SelectedValue}', FullName='{TxtName.Text}' where uname='{TxtUserName.Text}'", conn);

                if (TxtPassword.Text != TxtRePassword.Text)
                {
                    Response.Write("<script type=\"text/javascript\">alert('Gesla niso ista. Poskusite še enkrat!');</script>");
                    TxtPassword.Text = "";
                    TxtRePassword.Text = "";
                }
                else
                {

                    try
                    {
                        var username = TxtUserName.Text;
                        cmd.ExecuteNonQuery();
                        Response.Write("<script type=\"text/javascript\">alert('Uspešno spremenjeni podatki.');</script>");
                        TxtName.Text = "";
                        TxtPassword.Text = "";
                        TxtRePassword.Text = "";
                        TxtUserName.Text = "";
                        email.Text = "";
                        var company = companiesList.SelectedValue;
                        //    fillUsersDelete();
                        string filePath = Server.MapPath($"~/App_Data/{company}/{username}").Replace(" ", string.Empty); ;
                        string replacedPath = filePath.Replace(" ", string.Empty);


                        if (!Directory.Exists(replacedPath.ToString()))
                        {

                            Directory.CreateDirectory(replacedPath);
                           // FillList();

                        }
                    }
                    catch (Exception error)
                    {

                        // Implement logging here.
                    }
                }
            }
        }

        protected void companyButton_Click(object sender, EventArgs e)
        {

        }

        protected void companiesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            var id = GetCompanyName(companiesListBox.SelectedItem.Value.ToString());
            FillUsers(id);
            // Changes the users acording to the selected value in the CompanyList Box.
        }

        protected void usersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            graphsListBox.Enabled = true;
            FillListGraphs();
            showConfig();
            updateForm();

            //  Response.Write($"<script type=\"text/javascript\">alert('{}');</script>");
        }

        public void FillListGraphsNames()
        {
            fileNames.Clear();
            string filePath = Server.MapPath("~/App_Data/Dashboards");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
            System.IO.FileInfo[] fi = di.GetFiles();
            foreach (System.IO.FileInfo file in fi)
            {
                XDocument doc = XDocument.Load(filePath + "/" + file.Name);
                var tempXmlName = doc.Root.Element("Title").Attribute("Text").Value;
                graphNames.Add(file.Name + " " + tempXmlName);
                string trimmed = String.Concat(tempXmlName.Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");

                // Refils potential new tables.
                // finalQuery = String.Format($"ALTER TABLE permisions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                values.Add(trimmed);

            }

        }
        private string getCompanyQuery(string uname)
        {


            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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

        private void copyFiles()
        {
            var filePath = Server.MapPath("~/App_Data/Dashboards");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
            System.IO.FileInfo[] fi = di.GetFiles();
            var user = usersListBox.SelectedItem.Text;


            string uname = getCompanyQuery(user);
            for (int i = 0; i < fi.Length; i++)
            {
                var item = fi[i].Name;
                var source = Server.MapPath($"~/App_Data/Dashboards/{item}");
                var output = Server.MapPath($"~/App_Data/{uname}/{user}/{item}");

                if (graphsListBox.Items.ElementAt(i).Selected == true)
                {
                    if (!File.Exists(output))
                    {
                        File.Copy(source, output);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (File.Exists(output))
                    {
                        File.Delete(output);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private void makeSQLquery()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            for (int i = 0; i < graphsListBox.Items.Count; i++)
            {
                var tempGraphString = values.ElementAt(i);
                findId = String.Format($"SELECT id_permisions from Users where uname='{usersListBox.SelectedItem.Text}'");
                // execute query
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand(findId, conn);
                try
                {
                    id = cmd.ExecuteScalar();

                }
                catch (Exception e)
                {
                    continue;
                }
                Int32 Total_ID = System.Convert.ToInt32(id);
                if (graphsListBox.Items.ElementAt(i).Selected == true)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
                finalQuerys = String.Format($"UPDATE permisions SET {tempGraphString}={flag} WHERE id_permisions={Total_ID};");
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


        protected void saveGraphs_Click(object sender, EventArgs e)
        {
            if (usersListBox.SelectedItem == null) 
            {
                Response.Write("<script type=\"text/javascript\">alert('Morate izbrati uporabnika.');</script>");

            }
            else
            {
                FillListGraphsNames();
                makeSQLquery();
                showConfig();
                copyFiles();
            }
        }










        //protected void createUser_Click(object sender, EventArgs e)
        //{
        //    if (userForm.Visible == false)
        //    {
        //        userForm.Visible = true;
        //    }
        //    else
        //    {
        //        userForm.Visible = false;
        //    }
        //}

        //protected void byUser_Click(object sender, EventArgs e)
        //{
        //    if (byUserForm.Visible == false)
        //    {
        //        byUserForm.Visible = true;
        //    }
        //    else
        //    {
        //        byUserForm.Visible = false;
        //    }
        //}

        //protected void createCompany_Click(object sender, EventArgs e)
        //{
        //    if (companyForm.Visible == false)
        //    {
        //        companyForm.Visible = true;
        //    }
        //    else
        //    {
        //        companyForm.Visible = false;
        //    }
        //}
    }
}