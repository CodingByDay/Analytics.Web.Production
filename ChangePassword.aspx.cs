using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Security;

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
                    Response.Write($"<script type=\"text/javascript\">alert('Cajt za resetiranje je potekal ali link ni v vredu.'  );</script>");

                }
            }
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }

        protected void change_Click(object sender, EventArgs e)
        {
            if (checkEquality()) {
                if (ChangeUserPassword())
                {
                    Response.Write($"<script type=\"text/javascript\">alert('Geslo uspešno spremenjena.'  );</script>");

                } else
                {
                    Response.Write($"<script type=\"text/javascript\">alert('Link za spremembo gesla je potekal ali ni vejaven.'  );</script>");

                }
            } else
            {
                Response.Write($"<script type=\"text/javascript\">alert('Gesla niso ista.'  );</script>");

            }
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
        private bool ChangeUserPassword()
        {
                    List<SqlParameter> paramList = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = "@GUID",
                    Value = Request.QueryString["uid"]
                },
                new SqlParameter()
                {
                    ParameterName = "@Password",
                    Value = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd.Text, "SHA1")
                }
            };

            return ExecuteSP("spChangePassword", paramList);
        }




        private bool checkEquality()
        {
            if(pwd.Text==REpwd.Text)
            {
                return true;

            } else
            {
                return false;
            }
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