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
    public partial class testingcompanyadmin : System.Web.UI.Page
    {
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
        private List<String> CurrentPermisionID = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                defaultCompany();
                by.Visible = false;
 
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
                List<String> values = GetConnectionStrings();
                string debug = "";
      
                foreach(String f in values)
                {
                    debug += f + System.Environment.NewLine;
                }

                Response.Write(debug.ToString());

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
                findIdString = String.Format($"SELECT id_permision_user from Users where uname='{usersListBox.SelectedItem.Text}'");
            }
            else
            {
                usersListBox.SelectedIndex = 0;
                findIdString = String.Format($"SELECT id_permision_user from Users where uname='{usersListBox.SelectedItem.Text}'");

            }

            // Documentation. This query is for getting all the permision table data from the user
            cmd = new SqlCommand(findIdString, conn);
            idNumber = cmd.ExecuteScalar();
            Int32 Total_Key = System.Convert.ToInt32(idNumber);

            conn.Close();
            conn.Dispose();
            permisionQuery = $"SELECT * FROM permisions_user WHERE id_permisions_user={Total_Key}";
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

                conn.Close();
                conn.Dispose();
                return valuesBool;
            }
        }

        private List<String> GetConnectionStrings()
        {
            // ConnectionStringSetting. Represents a single ConnectionString from the web config.
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            List<String> strings = new List<string>();
            foreach (ConnectionStringSettings conn in connections)
            {

                strings.Add(conn.Name);

            }
            strings.RemoveAt(0);
            return strings;


        }
        public void FillListGraphs()
        {

            try
            {
                graphList.Clear();
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();

               // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"SELECT Caption from Dashboards;", conn);

                // Intepolation or the F string.C# > 5.0       
                 //Execute command and fetch pwd field into lookupPassword string.
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
                Response.Write($"<script type=\"text/javascript\">alert('Napaka... {ex.ToString()}');</script>");

            }
            finally
            {
                cmd.Dispose();
                conn.Close();
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
                Response.Write(ex.ToString());
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
                Response.Write(ex.ToString());
            }
        }


        private void updateForm()
        {

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
                companiesList.Enabled = false;
                email.Enabled = false;

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

            companiesList.SelectedIndex = id - 1;

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
                conn.Close();
                cmd.Dispose();
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
                    SqlCommand check = new SqlCommand($"Select count(*) from Users where uname='{TxtUserName.Text}'", conn);


                    var resultCheck = check.ExecuteScalar();

                    Int32 resultUsername = System.Convert.ToInt32(resultCheck);
                    conn.Close();
                    check.Dispose();
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
                            Response.Write(error.ToString());
                        }
                        string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPassword.Text, "SHA1");

                        conn.Close();
                        createUserPermisions.Dispose();
                        var connRegistration = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                        connRegistration.Open();
                        string finalQueryRegistration = String.Format($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company, ViewState, FullName, email, permision_user) VALUES ('{TxtUserName.Text}', '{HashedPassword}', '{userRole.SelectedValue}', '{next}', '{companiesList.SelectedIndex + 1}','{userType.SelectedValue}','{TxtName.Text}', '{email.Text}', '{next}')");
                        SqlCommand createUser = new SqlCommand(finalQueryRegistration, connRegistration);
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
                            conn.Close();
                            createUser.Dispose();
                            //fillChange();
                            //fillUsersDelete();
                            string filePath = Server.MapPath($"~/App_Data/{spacelessCompany}/{username}");
                            string replacedPath = filePath.Replace(" ", string.Empty);
                           
                        }
                        catch (SqlException ex) when (ex.Number == 2627)
                        {
                            // Implement logging here.
                            Response.Write($"<script type=\"text/javascript\">alert('To uporabniško ime že obstaja, prosimo probajte še enkrat.');</script>");
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
                        var username = TxtUserName.Text.Replace(" ", string.Empty); ;
                        cmd.ExecuteNonQuery();
                        Response.Write("<script type=\"text/javascript\">alert('Uspešno spremenjeni podatki.');</script>");
                        TxtName.Text = "";
                        TxtPassword.Text = "";
                        TxtRePassword.Text = "";
                        TxtUserName.Text = "";
                        email.Text = "";
                        var company = companiesList.SelectedValue.Replace(" ", string.Empty); ;
                        //    fillUsersDelete();
                        string filePath = Server.MapPath($"~/App_Data/{company}/{username}");
                        string replacedPath = filePath.Replace(" ", string.Empty);
                        conn.Close();
                        cmd.Dispose();

                       
                    }
                    catch (Exception ex)
                    {
                        // Implement logging here.
                        Response.Write($"<script type=\"text/javascript\">alert('Napaka... {ex.ToString()}');</script>");
                        // Logging
                        TxtName.Text = "";
                        TxtPassword.Text = "";
                        TxtRePassword.Text = "";
                        TxtUserName.Text = "";
                        email.Text = "";

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

      



       


        protected void usersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            graphsListBox.Enabled = true;
            FillListGraphs();
            showConfig();
            updateForm();
        }

        public void FillListGraphsNames()
        {


            try
            {
                graphList.Clear();
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();

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


            }
            catch (Exception ex)
            {
                Response.Write($"<script type=\"text/javascript\">alert('Napaka... {ex.ToString()}');</script>");

            } finally
            {
                cmd.Dispose();
                conn.Close();
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

      
        private void makeSQLquery()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            for (int i = 0; i < graphsListBox.Items.Count; i++)
            {
                var tempGraphString = values.ElementAt(i);
                findId = String.Format($"SELECT id_permision_user from Users where uname='{usersListBox.SelectedItem.Text}'");
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            for (int i = 0; i < byUserListBox.SelectedValues.Count; i++)
            {
                var tempGraphStringFullOfStuff = byUserListBox.SelectedValues[i].ToString();
                string trimmedless = String.Concat(tempGraphStringFullOfStuff.Where(c => !Char.IsWhiteSpace(c)));
                string trimmed = trimmedless.Replace("-", "");
                find = String.Format($"SELECT id_permision-user from Users where uname='{trimmed}'");
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
            }
        }
        private void getIdPermision()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{deletedID}'", conn);

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

        private List<String> getIdPermisionCurrentUser(string uname, List<String> obj)
        {
            List<String> permisions = new List<string>();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{uname}'", conn);

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
            int idUser = permisionID;
            cmd.Dispose();
            conn.Close();

            foreach(String graph in obj)
            {
                string whiteless = String.Concat(graph.Where(c => !Char.IsWhiteSpace(c)));
                string stripped = whiteless.Replace("-", "");
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                SqlCommand graphResult = new SqlCommand($"select {stripped} from permisions_user where id_permisions_user={idUser}", conn);
                string deb = $"select {stripped} from permisions_user where id_permisions_user={idUser}";
                try
                {
                    var result = graphResult.ExecuteScalar();
                    permisionID = System.Convert.ToInt32(result);

                    if(permisionID==1)
                    {
                        permisions.Add(graph);
                    } else
                    {
                        continue;
                    }

                }
                catch(Exception ex)
                {
                    continue;
                }
            }

            return permisions;

        }

        private void deletePermisionEntry()
        {

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd1 = new SqlCommand($"DELETE FROM permisions_user WHERE id_permisions_user={permisionID}", conn);
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
                
                Response.Write($"<script type=\"text/javascript\">alert('Uspešno brisanje.'  );</script>");

              
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
            int idCompany = getIdCompany(final);
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"DELETE FROM memberships WHERE id_company={idCompany}", conn);
            string dev = $"DELETE FROM companies WHERE company_name='{idCompany}'";
            try
            {
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error.Message}');</script>");
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
             
                    FillListGraphs();
                    showConfig();
                    deletePermisionEntry();
                    FillUsers();
                    // Logging
                
            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error.Message}'  );</script>");
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
                        int bitValueTemp = (int)(reader[values[i]] as int? ?? 0);
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

                conn.Close();
                conn.Dispose();
            }
        }


        protected void byUserListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
