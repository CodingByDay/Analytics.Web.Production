using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dash.DatabaseStorage
{
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
            Dashboard d = new Dashboard();

            d.LoadFromXDocument(document);
            d.Title.Text = dashboardName;
            document = d.SaveToXDocument();
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "INSERT INTO dashboards (dashboard, caption) " +
                    "output INSERTED.id " +
                    "VALUES (@dashboard, @caption)");
                string stripped = String.Concat(dashboardName.ToString().Where(c => !Char.IsWhiteSpace(c))).Replace("-", "");
                InsertCommand.Parameters.Add("caption", SqlDbType.NVarChar).Value = stripped;
                InsertCommand.Parameters.Add("dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                string ID = InsertCommand.ExecuteScalar().ToString();
                return ID;
            }
        }

        public XDocument LoadDashboard(string dashboardID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT dashboard FROM dashboards WHERE id=@id");
                GetCommand.Parameters.Add("id", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                reader.Read();
                byte[] data = reader.GetValue(0) as byte[];
                MemoryStream stream = new MemoryStream(data);
                var doc = XDocument.Load(stream);
                return doc;
            }
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<DashboardInfo> list = new List<DashboardInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("sp_get_admin_dashboards", connection);
                GetCommand.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = GetCommand.ExecuteReader();
                while (reader.Read())
                {
                    string ID = reader.GetInt32(0).ToString();
                    string Caption = reader.GetString(1);
                    list.Add(new DashboardInfo() { ID = ID, Name = Caption });
                }
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
                    "UPDATE dashboards SET dashboard = @dashboard " +
                    "WHERE id = @id");
                InsertCommand.Parameters.Add("id", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                InsertCommand.Parameters.Add("dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                InsertCommand.ExecuteNonQuery();
            }
        }
    }
}