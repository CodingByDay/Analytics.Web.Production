using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPasswordResetLinkValid())
                {
                    Response.Write($"<script type=\"text/javascript\">alert('Cajt za resetiranje je potekal.'  );</script>");

                }
            }
        }

        protected void backButton_Click(object sender, EventArgs e)
        {

        }

        protected void change_Click(object sender, EventArgs e)
        {

        }



        private bool IsPasswordResetLinkValid()
         {
            List<SqlParameter> paramList = new List<SqlParameter>()
         {
         new SqlParameter()
          {
            ParameterName = "@GUID",
            Value = Request.QueryString["uid"]
          }
        };

            return ExecuteSP("spIsPasswordResetLinkValid", paramList);
        }


        private bool ExecuteSP(string SPName, List<SqlParameter> SPParameters)
        {

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");

            using(conn)
            {

                SqlCommand cmd = new SqlCommand(SPName, conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach(SqlParameter parameter in SPParameters)
                {
                    cmd.Parameters.Add(parameter);
                }


                conn.Open();
                return Convert.ToBoolean(cmd.ExecuteScalar());







            }









        }
    }
}