using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FluentFTP;
using peptak.ftp;
namespace peptak
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // testing the ftp class






        }

        protected void login_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx"); // Entry point to the application.
        }

        protected void plans_Click(object sender, EventArgs e)
        {
            Response.Redirect("plans.aspx");
        }

        protected void FTP_Click(object sender, EventArgs e)
        {

            FtpClient client = new FtpClient();
            client.Host = "89.212.55.202";
            client.Credentials = new NetworkCredential("insistinsist", "w3bp4ss!");
            client.Connect();

            // upload a file
            

            // download the file again
            client.DownloadFile(@"C:\Users\janko\Desktop\ftp.txt", "/web/dashboards/ftp.txt");
        }


    }
}