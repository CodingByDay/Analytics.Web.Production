using System;
using System.Data.SqlClient;

namespace Dash.HelperClasses
{
    public class CheckConnection
    {
        public bool CheckDatabaseConnection(string conn)
        {
            SqlConnection connection = new SqlConnection(conn);
            bool result;
            try
            {
                connection.Open();
                result = true;
                connection.Close();
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        // 26.08.2024 Janko Jovičić Code cleaning.
    }
}