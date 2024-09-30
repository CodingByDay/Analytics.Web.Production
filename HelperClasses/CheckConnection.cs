using System;
using System.Data.SqlClient;

namespace Dash.HelperClasses
{
    public static class CheckConnection
    {
        /// <summary>
        /// Tests connection paramaters and returns whether the connection was succesfull-true and !true - false!
        /// </summary>
        /// <param name="InitialCatalog"></param>
        /// <param name="DataSource"></param>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static bool TestConnection(string InitialCatalog, string DataSource, string UserID, string Password)
        {
            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
            build.InitialCatalog = InitialCatalog;
            build.DataSource = DataSource;
            build.UserID = UserID;
            build.Password = Password;

            var _result = CheckDatabaseConnection(build.ConnectionString);

            if (_result == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CheckDatabaseConnection(string conn)
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