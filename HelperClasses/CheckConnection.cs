﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace peptak.HelperClasses
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

            OleDbConnection connection = new OleDbConnection(conn);

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