using DevExpress.DashboardWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace peptak.DatabaseStorage
{

    public class DataBaseEditableDashboardStorageCustom : IEditableDashboardStorage
    {
        private string connectionString;
        private SqlConnection conn;
        private int permisionID;

        public DataBaseEditableDashboardStorageCustom(string connectionString)
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
                return ID;
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


        private List<String> getCaptions()
        {
            List<String> list = new List<String>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT Caption FROM Dashboards");
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                while (reader.Read())
                {

                    string Caption = reader.GetString(0);
                    list.Add(Caption);
                }
                connection.Close();
            }
            return list;
        }



        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<String> available = getIdPermisionCurrentUser();
            string FinalString = "(";
            //Getting the final string
            for (int i = 0; i < available.Count; i++)
            {
                if (i != available.Count - 1)
                {
                    FinalString += "'" + available[i] + "'" + ",";
                }
                else
                {
                    FinalString += "'" + available[i] + "'" + ")";

                }
            }



            List<DashboardInfo> list = new List<DashboardInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand($"SELECT ID, Caption FROM Dashboards WHERE Caption IN {FinalString}");
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

        private List<String> getIdPermisionCurrentUser()
        {
            List<String> captions = getCaptions();
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */

            List<String> permisions = new List<string>();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{UserNameForChecking}'", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                permisionID = System.Convert.ToInt32(result);

            }




            catch (Exception error)
            {
                // Implement logging here.
            }
            int idUser = permisionID;
            cmd.Dispose();
            conn.Close();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            foreach (String graph in captions)
            {
                string whiteless = String.Concat(graph.Where(c => !Char.IsWhiteSpace(c)));
                string stripped = whiteless.Replace("-", "");

                SqlCommand graphResult = new SqlCommand($"select {stripped} from permisions_user where id_permisions_user={idUser}", conn);
                string deb = $"select {stripped} from permisions_user where id_permisions_user={idUser}";

                var result = graphResult.ExecuteScalar();
                int permision = (int)result;
                graphResult.Dispose();
                if (permision == 1)
                {
                    permisions.Add(graph);
                }
                else
                {
                    continue;
                }


                

            }
            
            conn.Close();

            return permisions;

        }
    }
}
