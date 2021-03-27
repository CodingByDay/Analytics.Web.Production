using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace peptak
{
    public partial class Administration : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {
      

            if (!IsPostBack) // Doesn't update the values more than once.
            {
                Button BackButton = (Button)Master.FindControl("back");
                BackButton.Enabled = true;
                BackButton.Visible = true;
                FillList();
                FillListGraphs();
                showConfig();
                fillCompanies();
                FillListAdmin();
                fillUsersDelete();
                fillCompanyDelete();

            } 
            

        

        }

        private void fillCompanies()
        {
            try
            {
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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

        private List<bool> showConfig()
        {
            debug.Clear();
            valuesBool.Clear();
            columnNames.Clear();
            config.Clear();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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
            permisionQuery = $"SELECT * FROM permisions WHERE id_permisions={Total_Key}";
            cmd = new SqlCommand(permisionQuery, conn);
            debug.Add(permisionQuery);
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");

            using (SqlConnection connection = new SqlConnection(
              "server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;"))
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
            var filePath = Server.MapPath("~/App_Data/Dashboards");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
            System.IO.FileInfo[] fi = di.GetFiles();
            var folder = usersPermisions.SelectedValue;
            for (int i = 0; i < fi.Length; i++)
            {
                var item = fi[i].Name;
                var source = Server.MapPath($"~/App_Data/Dashboards/{item}");
                var output = Server.MapPath($"~/App_Data/{folder}/{item}");

                if (graphsFinal.Items.ElementAt(i).Selected == true)
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


        public void FillListAdmin()
        {
            try
            {

                string UserNameForChecking
                    = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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

                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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

            }
        }

        public void FillList()
         {
            try
            {

                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("Select uname from Users", conn); /// Intepolation or the F string. C# > 5.0       
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
            catch (Exception ex)
            {

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
                    graphNames.Add(file.Name + " " + tempXmlName);
                    string trimmedless = String.Concat(tempXmlName.Where(c => !Char.IsWhiteSpace(c)));
                    string trimmed = trimmedless.Replace("-", "");
                    fileNames.Add(file.Name);
                    // Refils potential new tables.
                    finalQuery = String.Format($"ALTER TABLE permisions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                    values.Add(trimmed);
                    // Execute query.
                    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                    conn.Open();
                    try
                    {
                        cmd = new SqlCommand(finalQuery, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception error)
                    {
                        continue;
                    }
                }
                graphsFinal.DataSource = graphNames;
                graphsFinal.DataBind();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
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


        private void makeSQLquery()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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
                catch (Exception e)
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
      
      

        private void saveSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }

        protected void Save_Click1(object sender, EventArgs e)
        {
            FillListGraphsNames();
            makeSQLquery();
            showConfig();
            copyFiles();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
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
                    string finalQueryRegistration = String.Format($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company) VALUES ('{TxtUserName.Text}', '{TxtPassword.Text}', '{userRole.SelectedValue}', '{next}', '{companiesList.SelectedIndex+1}')");
                    SqlCommand createUser = new SqlCommand(finalQueryRegistration, conn);
                    var username = TxtUserName.Text;
                    try
                    {
                        createUser.ExecuteNonQuery();
                        fillUsersDelete();
                        string filePath = Server.MapPath("~/App_Data/"+username); 
                        debug.Add(filePath);
                        if (!Directory.Exists(filePath)) 
                        {
                            FillList();
                            Directory.CreateDirectory(filePath);
                        }
                    }
                    catch (Exception error)
                    {   
                        // Implement logging here.
                    }
                }
            }
        }



        private void fillUsersDelete()
        {
            try
            {
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("Select uname from Users", conn); /// Intepolation or the F string. C# > 5.0       
                // Execute command and fetch pwd field into lookupPassword string.
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    deleteUsers.Add(sdr["uname"].ToString());

                }
                DeleteUser.DataSource = DataUser;
                DeleteUser.DataBind();
                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {

            }



        }

        protected void usersPermisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillListGraphsNames();
            
            showConfig();
           
        }

       private void insertCompany()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"Select count(*) from companies", conn);
            var result = cmd.ExecuteScalar();
            Int32 next = System.Convert.ToInt32(result) + 1;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            cmd = new SqlCommand($"INSERT INTO companies(id_company, company_name, company_number, website, admin_id) VALUES({next}, '{companyName.Text}', {companyNumber.Text}, '{website.Text}', '{listAdmin.SelectedValue}')", conn);

            try
            {
                cmd.ExecuteNonQuery();
              

            } catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }
            
          
            cmd.Dispose();
            conn.Close();
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
        protected void companyButton_Click(object sender, EventArgs e)
        {
           
            if(companyName.Text == "")
            {
                Response.Write($"<script type=\"text/javascript\">alert('Niste vpisali ime podjetja...'  );</script>");

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";
            } else if(website.Text=="")
            {
                Response.Write($"<script type=\"text/javascript\">alert('Niste napisali web page podjetja...'  );</script>");

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";

            } else if(!checkIfNumber(companyNumber.Text))
            {
                Response.Write($"<script type=\"text/javascript\">alert('Številka ni v pravi obliki.'  );</script>");

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";
            }
            else
            {
                insertCompany();
                Response.Write($"<script type=\"text/javascript\">alert('Uspešno poslani podatki.'  );</script>");
                fillCompanies();
                fillCompanyDelete();

                companyNumber.Text = "";
                companyName.Text = "";
                website.Text = "";
           
            }
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"delete from Users where uname='{DeleteUser.SelectedValue}'", conn);
            try
            {

                cmd.ExecuteNonQuery();
                FillList();
                FillListGraphs();
                showConfig();
                fillCompanies();
                FillListAdmin();
                fillUsersDelete();
                fillCompanyDelete();
                Response.Write($"<script type=\"text/javascript\">alert('Uspešno brisanje.'  );</script>");

            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();
        }

        private void fillCompanyDelete()
        {


            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand("select company_name from companies ", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyDestroy.Add(reader["company_name"].ToString());

            }

            deleteCompany.DataSource = strings;
            deleteCompany.DataBind();
            // unit test

            cmd.Dispose();
            conn.Close();



        }

        protected void deleteCompanyButton_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"DELETE FROM companies WHERE company_name='{deleteCompany.SelectedValue}'", conn);
            try
            {

                cmd.ExecuteNonQuery();
                FillList();
                FillListGraphs();
                showConfig();
                fillCompanies();
                FillListAdmin();
                fillUsersDelete();
                fillCompanyDelete();
                Response.Write($"<script type=\"text/javascript\">alert('Uspešno brisanje.'  );</script>");



            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();
        }
    }
}
