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
        private string finalQuerys;
        private SqlConnection conn;
        private SqlCommand cmd;
        private List<String> DataUser = new List<string>();
        private List<String> graphNames = new List<string>();
        private string[] graphQuery;
        private String finalQuery = "";
        private string selectedUser;
        private int[] selectedGraphs;

        public string ArraySelected;
        private int user;

        protected void Page_Load(object sender, EventArgs e)
        {

           
            
            
            Save.Click += Save_Click;
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




            users.DataSource = DataUser;
            users.DataBind();

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
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


                // execute query
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                try
                {
                    cmd = new SqlCommand(finalQuery, conn);
                    cmd.ExecuteNonQuery();
                } catch(Exception error)
                {
                    continue;
                }

            }


            users.SelectedIndexChanged += Users_SelectedIndexChanged;
            graphs.DataSource = graphNames;
            graphs.DataBind();

            ArraySelected = users.SelectedValue;

            graphs.SelectedIndexChanged += Graphs_SelectedIndexChanged;


        }

        private void Graphs_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedGraphs = graphs.GetSelectedIndices();
        }

        private void Users_SelectedIndexChanged(object sender, EventArgs e)
        {
            user = users.SelectedIndex;
        }

        private void makeSQLquery(string name, int[] selectedGraphs)
        {

            for (int i = 0; i < selectedGraphs.Length-1; i++)
            {
                var tempGraph = selectedGraphs[i];
                graphQuery[i] = graphNames.ElementAt(tempGraph);
            }
            for (int i = 0; i < graphQuery.Length-1; i++)
            {
                finalQuerys = String.Format($"UPDATE permisions SET {graphQuery}=false WHERE uname={name}");

                // execute query
                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand(finalQuerys, conn);
                cmd.ExecuteNonQuery();

            }
        }



     

        private void Save_Click(object sender, EventArgs e)
        {
          
            
            
                makeSQLquery(DataUser.ElementAt(user), selectedGraphs);
         

            // Sql time

            //pass
        }

        private void saveSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }
    }
}
