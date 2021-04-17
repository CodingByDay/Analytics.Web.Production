using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
        private string permisionQuery;
        private SqlCommand cmd;
        private object idNumber;
        private List<String> companiesData = new List<string>();
        private List<String> usersData = new List<string>();
        private string finalQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            // default select value for the user must be set before render...
            // width must be more, margins, byUser, onSelectedIndexChanged.

            // Also js function to disable more than one aperent divs......
            if (!IsPostBack)
            {
                companiesListBox.SelectedIndex = 0; 
                hideItems();
                fillCompanies();
                //FillUsers();
                FillListGraphs();
                //showConfig();
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

        private string GetCompanyName(string Name)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_company from companies where company_name='{Name}';", conn);
          
                var result = cmd.ExecuteScalar();
                var company = result.ToString();

            

            cmd.Dispose();
            conn.Close();
            return company;

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
            // EXEC(@SQLStatment)
            findIdString = String.Format($"SELECT id_permisions from Users where uname='{usersListBox.SelectedItem.Value}'");
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
        public void FillUsers(string companyID)
        {
            try
            {
                usersData.Clear();
                usersData.Add("Izberi");
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();

                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand($"Select uname from Users where company_id={companyID}", conn);
                Response.Write($"Select uname from Users where company_id={companyID}");
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


        private void hideItems()
        {
            
        }

        protected void registrationButton_Click(object sender, EventArgs e)
        {

        }

        protected void companyButton_Click(object sender, EventArgs e)
        {

        }

        protected void companiesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Write("<script type=\"text/javascript\">alert('Morate izbrati uporabnika.');</script>");

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