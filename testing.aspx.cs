using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Web;
using peptak.DatabaseStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace peptak
{
    public partial class testing : System.Web.UI.Page
    {


        public static string ConnectionString = @"Data Source=10.100.100.25\SPLAHOST; Database=graphs;Application Name = Dashboard; Integrated Security = false; User ID = petpakn; Password=net123321!";
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());

            var dataBaseDashboardStorage = new DataBaseEditableDashboardStorage(ConnectionString);
            ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);

            ASPxDashboard3.Visible = true;


            ASPxButton1.Click += ASPxButton1_Click;


        }

        private void ASPxButton1_Click(object sender, EventArgs e)
        {
            byte[] result = new byte[1000000];
            string stringedResult = "";
            string filePath = Server.MapPath("~/App_Data/PetPak/user2");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
            System.IO.FileInfo[] fi = di.GetFiles();
            int counter = 2;
            foreach (System.IO.FileInfo file in fi)
            {
                MemoryStream stream = new MemoryStream();

                XDocument doc = XDocument.Load(filePath + "/" + file.Name);
                AddDashboardS(doc, file.Name);
             



            }


        }

        public string AddDashboardS(XDocument document, string dashboardName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "INSERT INTO Dashboards (Dashboard, Caption) " +
                    "output INSERTED.ID " +
                    "VALUES (@Dashboard, @Caption)");
                InsertCommand.Parameters.Add("Caption", SqlDbType.NVarChar).Value = dashboardName;
                InsertCommand.Parameters.Add("Dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                string ID = InsertCommand.ExecuteScalar().ToString();
                connection.Close();
                return ID;
            }
        }

    }
}