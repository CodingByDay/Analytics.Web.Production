using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace peptak
{
    public partial class AdminPanelCompany : System.Web.UI.Page
    {
        private List<bool> valuesBool = new List<bool>();
        private List<String> columnNames = new List<string>();
        private List<bool> config = new List<bool>();
        private List<String> fileNames = new List<string>();
        private List<String> graphNames = new List<string>();
        private List<String> values = new List<string>();

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
        private string finalQuery;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                defaultCompany();
                by.Visible = false;
                // Consider this.
                FillUsers();
                FillListGraphs();
                graphsListBox.Enabled = false;
                fillCompaniesRegistration();
                defaultCompany();
                typesOfViews.Add("Viewer");
                typesOfViews.Add("Designer");
                typesOfViews.Add("Viewer&Designer");
                userType.DataSource = typesOfViews;
                userType.DataBind();
            }
            else
            {
                FillUsers();
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
            if (usersListBox.SelectedItem != null)
            {
                findIdString = String.Format($"SELECT id_permisions from Users where uname='{usersListBox.SelectedItem.Text}'");
            }
            else
            {
                usersListBox.SelectedIndex = 0;
                findIdString = String.Format($"SELECT id_permisions from Users where uname='{usersListBox.SelectedItem.Text}'");

            }

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
                        // Logging + error
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
        public void FillUsers()
        {
            try
            {
                var company = defaultCompany();
                var id = getIdCompany(company);
                usersData.Clear();
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();

                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"Select uname from Users where id_company={id}", conn);

                /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    usersData.Add(sdr["uname"].ToString());

                }
                byUserListBox.DataSource = usersData;
                byUserListBox.DataBind();
                usersListBox.DataSource = usersData;
                usersListBox.DataBind();
                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
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
                var company = defaultCompany();
                string uname = HttpContext.Current.User.Identity.Name;
                string name = getCompanyQuery(uname);
                int id = getIdCompany(name);
                companiesList.SelectedIndex = id - 1;
                // Fixes the value problem.
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


    

        private string defaultCompany()
        {
            string uname = HttpContext.Current.User.Identity.Name;

            string name = getCompanyQuery(uname);

            int id = getIdCompany(name);

            companiesList.SelectedIndex = id-1;

            companiesList.Enabled = false;

            return name;

        }

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
                            var id = getIdCompany(companiesList.SelectedValue);
                            createUser.ExecuteNonQuery();
                            Response.Write($"<script type=\"text/javascript\">alert('Uspešno kreiran uporabnik.');</script>");
                            TxtName.Text = "";
                            TxtPassword.Text = "";
                            TxtRePassword.Text = "";
                            TxtUserName.Text = "";
                            email.Text = "";
                            FillUsers();
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


        private bool checkIfNumber(string parametar)
        {
            var regex = new Regex(@"^-?[0-9][0-9,\.]+$");

            var numeric = regex.IsMatch(parametar);

            if (numeric)
                return true;
            else
                return false;


        }

        private void createFileIfDoesNotExist(string company)
        {
            string filePath = Server.MapPath("~/App_Data/" + company);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }



        private void XMLmanipulation(string folderName, string database, string admin)
        {
            var adminFolder = Server.MapPath($"~/App_Data/{folderName}/{admin}").Replace(" ", string.Empty);

            if (!Directory.Exists(adminFolder))
            {
                Directory.CreateDirectory(adminFolder);
            }

            var serverRoom = Server.MapPath($"~/App_Data/Dashboards");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(serverRoom);
            System.IO.FileInfo[] fi = di.GetFiles();

            for (int i = 0; i < fi.Length; i++)
            {
                var item = fi[i].Name;
                var source = Server.MapPath($"~/App_Data/Dashboards/{item}");
                var output = Server.MapPath($"~/App_Data/{folderName}/{admin}/{item}").Replace(" ", string.Empty);

                try
                {
                    File.Copy(source, output, true);
                }
                catch (IOException iox)
                {
                    Response.Write("Exception is: " + iox);

                }

                // Implement logging here.
            }

            // Second update part.
            string destinationFileEdit = Server.MapPath($"~/App_Data/{folderName}/{admin}").Replace(" ", string.Empty);

            fileNames.Clear();
            System.IO.DirectoryInfo edit = new System.IO.DirectoryInfo(destinationFileEdit);
            System.IO.FileInfo[] finfo = di.GetFiles();
            foreach (System.IO.FileInfo file in finfo)
            {

                XDocument doc = XDocument.Load(destinationFileEdit + "/" + file.Name);

                var first = doc.Root.Element("DataSources");

                first.Element("SqlDataSource").Element("Connection").Attribute("Name").Value = database;


            }

        }

         

        protected void usersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            graphsListBox.Enabled = true;
            FillListGraphs();
            showConfig();
            updateForm();
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
        private void makeSQLqueryByUser()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            for (int i = 0; i < byUserListBox.SelectedValues.Count; i++)
            {
                var tempGraphStringFullOfStuff = byUserListBox.SelectedValues[i].ToString();
                string trimmedless = String.Concat(tempGraphStringFullOfStuff.Where(c => !Char.IsWhiteSpace(c)));
                string trimmed = trimmedless.Replace("-", "");
                find = String.Format($"SELECT id_permisions from Users where uname='{trimmed}'");
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

                for (int j = 0; j < graphsListBox.Items.Count; j++)
                {

                    if (graphsListBox.Items.ElementAt(i).Selected == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }

                    tempGraphString = values.ElementAt(j);

                    finalQuerys = String.Format($"UPDATE permisions SET {tempGraphString}={flag} WHERE id_permisions={Total_ID};");
                    var debug = finalQuerys;
                    help.Add(debug.ToString());
                    cmd = new SqlCommand(finalQuerys, conn);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Response.Write($"<script type=\"text/javascript\">alert('Morate izbrati uporabnika. + {e.ToString()}');</script>");

                    }
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
        private void getIdPermision()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_permisions from Users where uname='{deletedID}'", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                permisionID = System.Convert.ToInt32(result);

            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();

        }



        private void deletePermisionEntry()
        {

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd1 = new SqlCommand($"DELETE FROM permisions WHERE id_permisions={permisionID}", conn);
            var final = $"DELETE FROM permisions WHERE id_permisions={permisionID}";
            try
            {
                var result = cmd1.ExecuteScalar();
                Int32 Total_ID = System.Convert.ToInt32(result);

            }


            catch (Exception error)
            {
                // Implement logging here.
            }

            cmd1.Dispose();
            conn.Close();
        }



      

        protected void delete_Click(object sender, EventArgs e)
        {


        }
        protected void deleteCompany_Click(object sender, EventArgs e)
        {
            var id = getIdCompany(current);
            deleteMemberships(id);
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand user = new SqlCommand($"delete from users where id_company={id}", conn);
            try
            {
                user.ExecuteNonQuery();

            }
            catch (Exception error)
            {
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake...'  );</script>");
            }
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"DELETE FROM companies WHERE company_name='{current}'", conn);
            string dev = $"DELETE FROM companies WHERE company_name='{current}'";
            try
            {
                cmd.ExecuteNonQuery();
                //fillUsersDelete();
                //fillCompanyDelete();
                Response.Write($"<script type=\"text/javascript\">alert('Uspešno brisanje.'  );</script>");

                string filePath = Server.MapPath("~/App_Data/" + current);
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath);
                }
            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake...'  );</script>");
            }
            FillListGraphs();
            cmd.Dispose();
            conn.Close();
            FillUsers();
        }

        private int getIdCompany(string current)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_company from companies where company_name='{current}'", conn);
            try
            {
                result = cmd.ExecuteScalar();
            }
            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }
            cmd.Dispose();
            conn.Close();
            int finalID = System.Convert.ToInt32(result);
            return finalID;
        }

        private void deleteMemberships(int number)
        {
            var final = defaultCompany();
            int idCompany =  getIdCompany(final);
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"DELETE FROM memberships WHERE id_company={idCompany}", conn);
            string dev = $"DELETE FROM companies WHERE company_name='{idCompany}'";
            try
            {
                cmd.ExecuteNonQuery();
                // fillUsersDelete();
                // fillCompanyDelete();
            }
            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake...'  );</script>");
            }
            cmd.Dispose();
            conn.Close();
        }

        protected void deleteUser_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"delete from Users where uname='{usersListBox.SelectedItem.Text}'", conn);
            deletedID = usersListBox.SelectedItem.Text;
            getIdPermision();
            try
            {
                var company = getCompanyQuery(usersListBox.SelectedItem.Text);
                var spacelessCompany = company.Replace(" ", string.Empty);
                cmd.ExecuteNonQuery();
                Response.Write($"<script type=\"text/javascript\">alert('Uspešno brisanje.'  );</script>");
                string filePath = Server.MapPath($@"~/App_Data/{spacelessCompany}/{usersListBox.SelectedItem.Text}");
                string finalPath = filePath.Replace(" ", string.Empty);
                if (Directory.Exists(finalPath.ToString()))
                {
                    Directory.Delete(finalPath.ToString());
                    FillListGraphs();
                    showConfig();
                    deletePermisionEntry();
                    FillUsers();
                }
                else
                {
                    FillListGraphs();
                    showConfig();
                    deletePermisionEntry();
                    FillUsers();
                    // Logging
                }
            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
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

            if (graphsListBox.SelectedValues == null | byUserListBox.SelectedValues == null)
            {
                Response.Write($"<script type=\"text/javascript\">alert('Morate izbrati uporabike in graf.');</script>");
            }
            else
            {                
                FillListGraphsNames();
                makeSQLqueryByUser();
                showConfigByUser();
                copyFilesByUser();
            }
        }

        private void showConfigByUser()
        {
            columnNames.Clear();
            config.Clear();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            // DECLARE @ColList Varchar(1000), @SQLStatment VARCHAR(4000)
            // SET @ColList = ''
            // select @ColList = @ColList + Name + ' , ' from syscolumns where id = object_id('permisions') AND Name != 'id_permisions'
            // SELECT @SQLStatment = 'SELECT ' + Substring(@ColList, 1, len(@ColList) - 1) + 'FROM permisions'
            // EXEC(@SQLStatment
            if (byUserListBox.SelectedValues[0] != null)
            {
                findIdString = String.Format($"SELECT id_permisions from Users where uname='{byUserListBox.SelectedValues[0]}'");
            }
            else
            {
                byUserListBox.SelectedIndex = 0;
                findIdString = String.Format($"SELECT id_permisions from Users where uname='{byUserListBox.SelectedValues[0]}'");

            }

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
            }
        }

        private void copyFilesByUser()
        {
            for (int j = 0; j < byUserListBox.SelectedValues.Count; j++)
            {
                var filePath = Server.MapPath("~/App_Data/Dashboards");
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
                System.IO.FileInfo[] fi = di.GetFiles();
                var user = byUserListBox.SelectedValues[j].ToString();


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
        }

        protected void byUserListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
