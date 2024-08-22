using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace Dash
{
    public partial class adminsitrationcompany : System.Web.UI.Page
    {
        private string findId;
        private string finalQuerys;
        private SqlConnection conn;
        private string findIdString;
        private SqlCommand cmd;
        private object idNumber;
        private List<String> DataUser = new List<string>();
        private List<String> graphNames = new List<string>();
        private string[] graphQuery = new string[100];
        private String finalQuery = "";
        public string ArraySelected;
        private List<String> values = new List<string>();
        private string[] answer = new string[100];
        private object id;
        private String permisionQuery;
        private List<String> BinaryPermisionList = new List<String>();
        private List<String> columnNames = new List<string>();
        private List<bool> config = new List<bool>();
        private List<String> debug = new List<string>();
        private List<bool> valuesBool = new List<bool>();
        private int flag;
        private List<String> fileNames = new List<string>();
        private List<String> companies = new List<string>();
        private List<String> admins = new List<string>();
        private List<String> strings = new List<string>();
        private List<String> deleteUsers = new List<string>();
        private List<String> CompanyDestroy = new List<string>();
        private List<String> changeCompanyUser = new List<string>();
        private List<String> changeUserCompany = new List<string>();
        private List<String> typesOfViews = new List<string>();
        private int permisionID;
        private string deletedID;
        private string sourceFile;
        private string destinationFile;
        private string companyInfo;
        private int result;
        private string admin;
        private int company;
        private string name;

        protected void Page_Load(object sender, EventArgs e)
        {

            /////////////////////////////////////////////////////////////
            // Initial "Postback"
            if (!IsPostBack) // Doesn't update the values more than once.
            {
                defaultCompany();
                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
                welcomeFunction();
                deleteUsers.Clear();
                HtmlAnchor BackButton = (HtmlAnchor)Master.FindControl("backButtonA");
                BackButton.Visible = true;
                FillList();
                FillListGraphs();
                showConfig();
                fillCompanies();
                fillUsersDelete();
                typesOfViews.Add("Viewer");
                typesOfViews.Add("Designer");
                typesOfViews.Add("Viewer&Designer");
                userType.DataSource = typesOfViews;
                userType.DataBind();
            }


            else
            {
                // Pass for now
            }

        }
        private void defaultCompany()
        {
            string uname = HttpContext.Current.User.Identity.Name;

            string name = getCompanyQuery(uname);


            companiesList.SelectedValue = name;

            companiesList.Enabled = false;


        }
        private void fillCompanies()
        {
            try
            {
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("SELECT * FROM Companies", conn); 
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
            catch (Exception)
            {

            }
        }
        public string GetAdminFromCompanyId(int company)
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT admin_id FROM Companies WHERE id_company={company}", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                admin = (reader["admin_id"].ToString());
            }

            cmd.Dispose();
            conn.Close();
            return admin;
        }



        public string GetCompanyName(int company)
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT company_name FROM Companies WHERE id_company={company}", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            admin = (string)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();
            return admin;
        }
        private List<bool> showConfig()
        {
            debug.Clear();
            valuesBool.Clear();
            columnNames.Clear();
            config.Clear();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            // DECLARE @ColList Varchar(1000), @SQLStatment VARCHAR(4000)
            // SET @ColList = ''
            // select @ColList = @ColList + Name + ' , ' from syscolumns where id = object_id('permisions') AND Name != 'id_permisions'
            // SELECT @SQLStatment = 'SELECT ' + Substring(@ColList, 1, len(@ColList) - 1) + 'FROM permisions'
            // EXEC(@SQLStatment)
            findIdString = String.Format($"SELECT id_permisions from Users where uname='{usersPermisions.SelectedValue}'");
            // Documentation. This query is for getting all the permision table data from the user
            cmd = new SqlCommand(findIdString, conn);
            idNumber = cmd.ExecuteScalar();
            Int32 Total_Key = System.Convert.ToInt32(idNumber);
            conn.Close();
            conn.Dispose();
            permisionQuery = $"SELECT * FROM Permissions WHERE id_permisions={Total_Key}";
            cmd = new SqlCommand(permisionQuery, conn);
            debug.Add(permisionQuery);
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");

            using (SqlConnection connection = new SqlConnection(
              "server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;"))
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
                            graphsFinal.Items.ElementAt(i).Selected = true;
                            valuesBool.Add(true);
                        }
                        else
                        {
                            graphsFinal.Items.ElementAt(i).Selected = false;
                            valuesBool.Add(false);
                        }
                    }
                }

                conn.Close();
                conn.Dispose();
                return valuesBool;
            }
        }







        private void copyFiles()
        {

            var userAdmin = HttpContext.Current.User.Identity.Name;
            string uname = getCompanyQuery(userAdmin);
            string name = uname.Replace(" ", string.Empty);
            var user = usersPermisions.SelectedValue;

            var filePath = Server.MapPath($"~/App_Data/{name}/{userAdmin}");
            string spaceless = filePath.Replace(" ", string.Empty);
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(spaceless.ToString());
            System.IO.FileInfo[] fi = di.GetFiles();
            for (int i = 0; i < fi.Length; i++)
            {
                var item = fi[i].Name;
                var source = Server.MapPath($"~/App_Data/{name}/{userAdmin}/{item}").Replace(" ", string.Empty);
                var output = Server.MapPath($"~/App_Data/{name}/{user}/{item}").Replace(" ", string.Empty);

                if (graphsFinal.Items.ElementAt(i).Selected == true)
                {
                    if (!File.Exists(output))
                    {
                        File.Copy(source, output, true);
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

        private int getCompanyId()
        {


            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"Select id_company from Users where uname='{UserNameForChecking}'", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            var objectInt = cmd.ExecuteScalar();
            Int32 next = System.Convert.ToInt32(objectInt);


            cmd.Dispose();
            conn.Close();




            return next;
        }

        public void FillList()
        {
            try
            {
                DataUser.Clear();
                var company = getCompanyId();
                DataUser.Add("Izberi");
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"Select uname from Users where id_company={company}", conn); /// Intepolation or the F string. C# > 5.0      

                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    DataUser.Add(sdr["uname"].ToString());

                }
                usersPermisions.DataSource = DataUser;
                usersPermisions.DataBind();
                cmd.Dispose();
                conn.Close();


            }
            catch (Exception)
            {

            }
        }

        public void FillListGraphs()
        {
            try
            {
                int company = getCompanyId();
                string name = GetCompanyName(company);
                string admin = GetAdminFromCompanyId(company);
                var nameless = name.Replace(" ", string.Empty);
                fileNames.Clear();
                var output = Server.MapPath($@"~/App_Data/{nameless}/{admin}");

                string filePath = Server.MapPath($@"~/App_Data/{nameless}/{admin}");
                string path = filePath.Replace(" ", string.Empty);
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] fi = di.GetFiles();
                foreach (System.IO.FileInfo file in fi)
                {
                    XDocument doc = XDocument.Load(path + "/" + file.Name);
                    var tempXmlName = doc.Root.Element("Title").Attribute("Text").Value;
                    graphNames.Add(file.Name + " " + tempXmlName);
                    string trimmedless = String.Concat(tempXmlName.Where(c => !Char.IsWhiteSpace(c)));
                    string trimmed = trimmedless.Replace("-", "");
                    fileNames.Add(file.Name);
                    // Refils potential new tables.
                    finalQuery = String.Format($"ALTER TABLE Permissions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                    values.Add(trimmed);
                    // Execute query.
                    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
                    conn.Open();
                    try
                    {
                        cmd = new SqlCommand(finalQuery, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                graphsFinal.DataSource = graphNames;
                graphsFinal.DataBind();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception)
            {

            }
        }


        public void FillListGraphsNames()
        {
            int company = getCompanyId();
            string name = GetCompanyName(company);
            string admin = GetAdminFromCompanyId(company);

            var nameless = name.Replace(" ", string.Empty);
            fileNames.Clear();
            var output = Server.MapPath($@"~/App_Data/{nameless}/{admin}");

            string filePath = Server.MapPath($@"~/App_Data/{nameless}/{admin}");
            string path = filePath.Replace(" ", string.Empty);
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo[] fi = di.GetFiles();
            foreach (System.IO.FileInfo file in fi)
            {
                XDocument doc = XDocument.Load(path + "/" + file.Name);
                var tempXmlName = doc.Root.Element("Title").Attribute("Text").Value;
                graphNames.Add(file.Name + " " + tempXmlName);
                string trimmed = String.Concat(tempXmlName.Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");
                values.Add(trimmed);

            }

        }


        private void makeSQLquery()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            for (int i = 0; i < graphsFinal.Items.Count; i++)
            {
                var tempGraphString = values.ElementAt(i);
                findId = String.Format($"SELECT id_permisions from Users where uname='{usersPermisions.SelectedValue}'");
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
                if (graphsFinal.Items.ElementAt(i).Selected == true)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
                finalQuerys = String.Format($"UPDATE Permissions SET {tempGraphString}={flag} WHERE id_permisions={Total_ID};");
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
            cmd.Dispose();
            conn.Close();
        }



        private void saveSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }

        protected void Save_Click1(object sender, EventArgs e)
        {
            if (usersPermisions.SelectedValue == "Izberi")
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





        private string getCompanyQuery(string uname)
        {


            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT uname, company_name FROM Users INNER JOIN Companies ON Users.id_company = Companies.id_company WHERE uname='{uname}';", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                companyInfo = (reader["company_name"].ToString());
            }

            cmd.Dispose();
            conn.Close();
            return companyInfo;

        }


        private void welcomeFunction()
        {
            var username = HttpContext.Current.User.Identity.Name;
            var company = getCompanyQuery(username);


            welcome.Text = $"Dobrodošli administrativni del aplikacije podjetja: {company}, trenutno ste prijavljeni v sistem kot uporabnik: {username}!";

        }

        private void fillUsersDelete()
        {
            DeleteUser.Items.Clear();
            deleteUsers.Clear();
            try
            {
                var company = getCompanyId();

                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"Select uname from Users where id_company={company}", conn); /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    deleteUsers.Add(sdr["uname"].ToString());

                }
                DeleteUser.DataSource = deleteUsers;
                DeleteUser.DataBind();
                cmd.Dispose();
                conn.Close();


            }
            catch (Exception)
            {

            }



        }

        protected void usersPermisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillListGraphsNames();
            showConfig();
            updateForm();
        }

        /// <summary>
        /// Allows superadmin to update the user value. SelectedValue.
        /// </summary>
        /// 


        private void updateForm()
        {
            ///
            var userRightNow = usersPermisions.SelectedValue;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
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


        private void deletePermisionEntry()
        {

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            SqlCommand cmd1 = new SqlCommand($"DELETE FROM Permissions WHERE id_permisions={permisionID}", conn);
            var final = $"DELETE FROM Permissions WHERE id_permisions={permisionID}";
            try
            {
                var result = cmd1.ExecuteScalar();
                Int32 Total_ID = System.Convert.ToInt32(result);

            }


            catch (Exception)
            {
                // Implement logging here.
            }

            cmd1.Dispose();
            conn.Close();
        }

        protected void delete_Click(object sender, EventArgs e)
        {

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"delete from Users where uname='{DeleteUser.SelectedValue}'", conn);
            deletedID = DeleteUser.SelectedValue;
            getIdPermision();
            try
            {

                var company = getCompanyQuery(DeleteUser.SelectedValue);
                var spacelessCompany = company.Replace(" ", string.Empty);

                cmd.ExecuteNonQuery();
                // Response.Write($"<script type=\"text/javascript\">alert('Uspešno brisanje.'  );</script>");
                string filePath = Server.MapPath($@"~/App_Data/{spacelessCompany}/{DeleteUser.SelectedValue}");
                string finalPath = filePath.Replace(" ", string.Empty);
                if (Directory.Exists(finalPath.ToString()))
                {
                    Directory.Delete(finalPath.ToString());
                    //DeleteUser.Items.Clear();
                    FillList();
                    FillListGraphs();
                    showConfig();

                    //fillCompanies();
                    //FillListAdmin();
                    //fillUsersDelete();
                    //fillCompanyDelete();
                    //fillChange();
                    fillUsersDelete();
                    deletePermisionEntry();

                }
                else
                {
                    FillList();
                    FillListGraphs();
                    showConfig();

                    //fillCompanies();
                    //FillListAdmin();
                    //fillUsersDelete();
                    //fillCompanyDelete();
                    //fillChange();
                    fillUsersDelete();

                    deletePermisionEntry();
                    //Logging
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

        private void getIdPermision()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
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

    





        protected void completelyNewUser_Click(object sender, EventArgs e)
        {
            TxtName.Text = "";
            TxtUserName.Text = "";
            TxtPassword.Text = "";
            TxtRePassword.Text = "";


            Response.Write($"<script type=\"text/javascript\">alert('Izpolnite potrebne podatke.'  );</script>");
            TxtUserName.Enabled = true;


        }


        protected void registrationButton_Click1(object sender, EventArgs e)
        {

            if (TxtUserName.Enabled == true)
            {

                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
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
                    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
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

                        string finalQueryPermsions = String.Format($"insert into Permissions(id_permisions) VALUES ({next});");
                        SqlCommand createUserPermisions = new SqlCommand(finalQueryPermsions, conn);

                        try
                        {
                            createUserPermisions.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {

                        }
                        string finalQueryRegistration = String.Format($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company, ViewState, FullName) VALUES ('{TxtUserName.Text}', '{TxtPassword.Text}', '{userRole.SelectedValue}', '{next}', '{companiesList.SelectedIndex + 1}','{userType.SelectedValue}','{TxtName.Text}')");
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
                            FillList();
                            fillUsersDelete();
                            //fillUsersDelete();
                            string filePath = Server.MapPath($"~/App_Data/{spacelessCompany}/{username}");

                            string replacedPath = filePath.Replace(" ", string.Empty);
                            Response.Write($"<script type=\"text/javascript\">alert({replacedPath.ToString()});</script>");
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



                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123tnet!;");
                conn.Open();
                var dev = $"UPDATE Users set Pwd='{TxtPassword.Text}', userRole='{userRole.SelectedValue}', ViewState='{userType.SelectedValue}', FullName='{TxtName.Text}', where uname='{TxtUserName.Text}'";
                debug.Add(dev);
                SqlCommand cmd = new SqlCommand($"UPDATE Users set Pwd='{TxtPassword.Text}', userRole='{userRole.SelectedValue}', ViewState='{userType.SelectedValue}', FullName='{TxtName.Text}' where uname='{TxtUserName.Text}'", conn);

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
                            FillList();

                        }
                    }
                    catch (Exception)
                    {

                        // Implement logging here.
                    }
                }
            }
        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Clicked";
            MainView.ActiveViewIndex = 2;
        }
    }
}
