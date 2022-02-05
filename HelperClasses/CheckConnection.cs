using System.Data.SqlClient;

namespace Dash.HelperClasses
{
    public class CheckConnection
    {
        /*       Create instance of class:

                 MYSQL mysql = new MYSQL();

                 Use it in Button event or anywhere.


                 source_result = mysql.check_connection("Your connection String");

                 if (source_result == false)

                 {

                 lbl_error.Text = lbl_error.Text + " ::Error In Source Connection";

                 }        */











        public bool check_connection(string conn)

        {

            bool result = false;

            SqlConnection connection = new SqlConnection(conn);

            try

            {

                connection.Open();

                result = true;

                connection.Close();

            }

            catch

            {

                result = false;

            }

            return result;

        }
    }







}