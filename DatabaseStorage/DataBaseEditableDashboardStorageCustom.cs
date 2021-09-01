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
        private SqlCommand cmd;
        private string adminName;
        private string company;

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
                InsertPermision(stripped);
                var company = getcompanyForUser();
                var admin = GetAdminFromCompanyName(company);
                int idAdmin = GetPermisionUserID(admin);
                InsertPermisionAdminAndUser(idAdmin, stripped);
                return ID;
            }
        }
        private int getIdPermision()
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_permision_user from Users where uname='{UserNameForChecking}'", conn);
            try
            {
                var result = cmd.ExecuteScalar();
                permisionID = System.Convert.ToInt32(result);
            }
            catch (Exception error)
            {
                var log = error;
            }

            cmd.Dispose();
            conn.Close();

            return permisionID;


        }
        private void InsertPermisionAdminAndUser(int admin, string name)
        {
            var idCurrent = getIdPermision();
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"update permisions_user set {name} = 1 where id_permisions_user = {idCurrent}", conn);
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception error)
            {
                var log = error;
            }
            cmd.Dispose();
            conn.Close();
            // Admin insert...
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            SqlCommand cmdSecond = new SqlCommand($"update permisions_user set {name} = 1 where id_permisions_user = {admin}", conn);
            try
            {
                cmdSecond.ExecuteNonQuery();

            }
            catch (Exception error)
            {
                var log = error;
            }
            cmd.Dispose();
            conn.Close();

        }
        public int GetPermisionUserID(string user)
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"select id_permision_user from Users where uname='{user}';", conn);   
            // Execute command and fetch pwd field into lookupPassword string.
            int adminID = (int) cmd.ExecuteScalar();

         

            cmd.Dispose();
            conn.Close();
            return adminID;
        }
        public string GetAdminFromCompanyName(string company)
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT admin_id FROM companies WHERE company_name='{company}'", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                adminName = (reader["admin_id"].ToString());
            }

            cmd.Dispose();
            conn.Close();
            return adminName;
        }

        public string getcompanyForUser()
        {
            string uname = HttpContext.Current.User.Identity.Name;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"SELECT uname, company_name FROM Users INNER JOIN companies ON Users.id_company = companies.id_company WHERE uname='{HttpContext.Current.User.Identity.Name}';", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                company = (reader["company_name"].ToString());
            }

            cmd.Dispose();
            conn.Close();
            return company;
        }
        private void InsertPermision(string dashboardName)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"ALTER TABLE permisions_user ADD {dashboardName} int not null default(0);", conn);



            try
            {
                cmd.ExecuteNonQuery();

            }


            catch (Exception error)
            {
                var log = error;
            }



            cmd.Dispose();
            conn.Close();
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
            // Getting the final string
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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
