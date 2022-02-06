using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dash.DatabaseStorage
{
    /*
     GO
    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO

    SET ANSI_PADDING ON
    GO

    CREATE TABLE[dbo].[Dashboards]
    (

    [ID][int] IDENTITY(1,1) NOT NULL,

    [Dashboard] [varbinary]
    (max) NULL,

    [Caption] [nvarchar] (255) NULL,
    CONSTRAINT[PK_Dashboards] PRIMARY KEY CLUSTERED
    (
    [ID] ASC
    )WITH(PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON[PRIMARY]
    ) ON[PRIMARY]

    GO

    SET ANSI_PADDING OFF
    GO
     */

    public class DataBaseEditableDashboardStorage : IEditableDashboardStorage
    {

        private string connectionString;
        private SqlConnection conn;
        private int permisionID;

        public DataBaseEditableDashboardStorage(string connectionString)
        {
            this.connectionString = connectionString;
        }




        public string AddDashboard(XDocument document, string dashboardName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "INSERT INTO Dashboards (Dashboard, Caption) " +
                    "output INSERTED.ID " +
                    "VALUES (@Dashboard, @Caption)");
                string stripped = String.Concat(dashboardName.ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");
                InsertCommand.Parameters.Add("Caption", SqlDbType.NVarChar).Value = stripped;
                InsertCommand.Parameters.Add("Dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                string ID = InsertCommand.ExecuteScalar().ToString();
                connection.Close();
                InsertPermision(stripped);
                return ID;
            }
        }



        private void InsertPermision(string dashboardName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
                    SqlCommand cmd = new SqlCommand($"ALTER TABLE permisions_user ADD {dashboardName} int not null default(0);", conn);
                    cmd.ExecuteNonQuery();

                    
                }
                catch (Exception ex)
                {
                   
                }
            }
        
        }
        public XDocument LoadDashboard(string dashboardID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT  Dashboard FROM Dashboards WHERE ID=@ID");
                GetCommand.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                reader.Read();
                byte[] data = reader.GetValue(0) as byte[];
                MemoryStream stream = new MemoryStream(data);
                connection.Close();
                return XDocument.Load(stream);
            }
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<DashboardInfo> list = new List<DashboardInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT ID, Caption FROM Dashboards");
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                while (reader.Read())
                {
                    string ID = reader.GetInt32(0).ToString();
                    string Caption = reader.GetString(1);
                    list.Add(new DashboardInfo() { ID = ID, Name = Caption });
                }
                connection.Close();
            }
            return list;
        }

        public void SaveDashboard(string dashboardID, XDocument document)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "UPDATE Dashboards Set Dashboard = @Dashboard " +
                    "WHERE ID = @ID");
                InsertCommand.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                InsertCommand.Parameters.Add("Dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                InsertCommand.ExecuteNonQuery();

                connection.Close();
            }
        }


    }
}