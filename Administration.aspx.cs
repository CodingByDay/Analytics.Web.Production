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
        private SqlCommand cmd;
        private List<String> DataUser = new List<string>();
        private List<String> graphNames = new List<string>();
        private string[] graphQuery = new string[100];
        private String finalQuery = "";
        private int[] index;
        public string ArraySelected;
        private int user;
        private List<String> values = new List<string>();
        private SelectedIndexCollection indexList;
        private string[] answer = new string[100];
        private object id;

        protected void Page_Load(object sender, EventArgs e)
        {

           


          
            FillList();
            FillListGraphs();






            Save.Click += Save_Click;


        }

    

        public void FillList()
        {
            try
            {


                //Button btn = ((Button)Master.FindControl("admin"));
                //btn.Text = "Nazaj";
                //btn.Click += Btn_Click;
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

                    // REffils potential new tables.
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
      

        private void makeSQLquery(string name, SelectedIndexCollection selectedGraphs)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            for (int i = 0; i < selectedGraphs.Count; i++)
            {
                var tempGraphString = values.ElementAt(i);
                findId = String.Format($"SELECT id_permisions from Users where uname='{name}'");
              

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

                finalQuerys = String.Format($"UPDATE permisions SET {tempGraphString}='True' WHERE id_permisions={Total_ID};");
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





        private void Save_Click(object sender, EventArgs e)
        {
            indexList = graphsFinal.SelectedIndices;
          
            makeSQLquery(usersPermisions.SelectedValue, indexList);
           
           
        }
        private void saveSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }
    }
}
