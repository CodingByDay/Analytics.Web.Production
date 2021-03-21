using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
        private SelectedIndexCollection indexList;
        private List<String> BinaryPermisionList = new List<String>();
        private List<String> columnNames = new List<string>();
        private List<bool> config = new List<bool>();
        private List<String> debug = new List<string>();
        private List<bool> valuesBool = new List<bool>();
        private int flag;
        private List<String> companies = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) // Doesn't update the values more than once.
            {

                FillList();
                FillListGraphs();
                showConfig();
                fillCompanies();

            }





            //foreach (bool configValue in config)
            //{
            //    Response.Write(configValue);
            //}




        }

        private void fillCompanies()
        {
            try
            {


                // Button btn = ((Button)Master.FindControl("admin"));
                // btn.Text = "Nazaj";
                // btn.Click += Btn_Click;
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

            permisionQuery = $"SELECT * FROM permisions WHERE id_permisions={Total_Key}";


            cmd = new SqlCommand(permisionQuery, conn);


            SqlDataReader permisions = cmd.ExecuteReader();

            while (permisions.Read())
            {
                // test = (int)permisions["id_permision"];
                //string nameofTable = values.ElementAt(i);
                // //...
                for (int i = 1; i < permisions.FieldCount; i++)
                {
                    columnNames.Add(permisions.GetName(i));
                    ////  BinaryPermisionList.Add(sdr["uname"].ToString());
                    //BinaryPermisionList.Add(bitValue.ToString());
                    //i++;//
                    debug.Add(permisions.GetName(i));
                }
                for (int i = 0; i < columnNames.Count; i++)
                {
                    string name = columnNames[i];
                    bool bitValueTemp = (bool)(permisions[name] as bool? ?? false);
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
            return valuesBool;
        }

        public void FillList()
        {
            try
            {


                // Button btn = ((Button)Master.FindControl("admin"));
                // btn.Text = "Nazaj";
                // btn.Click += Btn_Click;
                string UserNameForCheckingAdmin = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
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


                string filePath = Server.MapPath("~/App_Data/Dashboards");

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
                System.IO.FileInfo[] fi = di.GetFiles();


                foreach (System.IO.FileInfo file in fi)
                {
                    XDocument doc = XDocument.Load(filePath + "/" + file.Name);

                    var tempXmlName = doc.Root.Element("Title").Attribute("Text").Value;

                    graphNames.Add(file.Name + " " + tempXmlName);
                    string trimmed = String.Concat(tempXmlName.Where(c => !Char.IsWhiteSpace(c)));

                    // Refils potential new tables.
                    finalQuery = String.Format($"ALTER TABLE permisions ADD {trimmed} BIT DEFAULT 0 NOT NULL;");
                    values.Add(trimmed);

                    // execute query
                    conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                    conn.Open();
                    // Create SqlCommand to select pwd field from users table given supplied userName.
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



            string filePath = Server.MapPath("~/App_Data/Dashboards");

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
            System.IO.FileInfo[] fi = di.GetFiles();


            foreach (System.IO.FileInfo file in fi)
            {
                XDocument doc = XDocument.Load(filePath + "/" + file.Name);

                var tempXmlName = doc.Root.Element("Title").Attribute("Text").Value;

                graphNames.Add(file.Name + " " + tempXmlName);
                string trimmed = String.Concat(tempXmlName.Where(c => !Char.IsWhiteSpace(c)));

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
                debug.Add(finalQuerys);
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

        protected void usersPermisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            showConfig();
        }

        protected void Save_Click1(object sender, EventArgs e)
        {


            indexList = graphsFinal.SelectedIndices;
            FillListGraphsNames();
            makeSQLquery();
            showConfig();
            //foreach (string deb in debug)
            //{
            //    Response.Write(deb);
            //}
            //showConfig();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"Select count(*) from Users", conn);


            var result = cmd.ExecuteScalar();


            Int32 Total_ID = System.Convert.ToInt32(result);

            int next = Total_ID + 1;
            //pass
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
                SqlCommand check = new SqlCommand($"Select count(*) from Users where Username='{TxtUserName}'", conn);


                var resultCheck = cmd.ExecuteScalar();
                if (result != null)
                {
                    Response.Write("<script type=\"text/javascript\">alert('Uporabniško ime že obstaja.');</script>");
                }
                else
                {
                    SqlCommand createUser = new SqlCommand($"Insert into Users(uname, Pwd, userRole, id_permisions, id_company) VALUES ('{TxtUserName}', '{TxtPassword}', '{userRole.SelectedValue}' {next}, '{companiesList.SelectedValue}'))");

                    try
                    {
                        cmd = new SqlCommand(finalQuery, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception error)
                    {

                        
                    }
                    //
                }










            }
        }
    }
}
