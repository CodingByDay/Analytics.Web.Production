using Dash.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Dash.Administration
{
    public static class HelpfullMethods
    {

        public static bool test()
        {
            return true;
        }


        /// <summary>
        /// Tests connection paramaters and returns whether the connection was succesfull-true and !true - false!
        /// </summary>
        /// <param name="InitialCatalog"></param>
        /// <param name="DataSource"></param>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static bool testSQL(string InitialCatalog, string DataSource, string UserID, string Password)
        {

            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();

            build.InitialCatalog = InitialCatalog;

            build.DataSource = DataSource;

            build.UserID = UserID;

            build.Password = Password;


            CheckConnection TestConnection = new CheckConnection();
            var _result = TestConnection.check_connection(build.ConnectionString);
           

            if (_result == false)

            {

                return false;

            }
            else
            {
                return true;


            }



        }

    }
}