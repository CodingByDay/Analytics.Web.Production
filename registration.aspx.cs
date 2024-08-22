﻿using Dash.HelperClasses;
using Dash.Session;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;

namespace Dash
{
    public partial class registration : System.Web.UI.Page
    {
        public static CustomerSessionClass customer = new CustomerSessionClass();
        public static NewPayedUserCompany user = new NewPayedUserCompany();
        private SqlConnection conn;
        private int company;
        private string id;

        protected void Page_Load(object sender, EventArgs e)
        {







        }




        protected void Register_Click(object sender, EventArgs e)
        {
            // if any field is empty.
            if (UsernameForm.Text == "" | PasswordForm.Text == "" |
            RePasswordForm.Text == "" | CompanyName.Text == "" |
            NameForm.Text == "" | EmailForm.Text == "" | PhoneForm.Text == ""
            | WebsiteForm.Text == "")
            {
                // Show javascript popup
                Response.Write("<script type=\"text/javascript\">alert('Nekateri podatki Vam manjkajo.');</script>");
            }
            else
            {
                var Username = UsernameForm.Text;
                var Password = PasswordForm.Text;
                var RePassword = RePasswordForm.Text;
                var Name = NameForm.Text;
                var Email = EmailForm.Text;
                var Phone = PhoneForm.Text;
                var Company = CompanyName.Text;
                var Website = WebsiteForm.Text;
                insertCompany(Company, Username, Website, Phone);

                if (Password == RePassword)
                {
                    CreateUser(Username, Password, Name, Email, Phone, Company);
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">alert('Gesla niso enaka.');</script>");
                    PasswordForm.Text = "";
                    RePasswordForm.Text = "";

                }
            }
        }

        private void insertCompany(string company, string username, string website, string phone)
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT count(*) FROM Companies", conn);
            var result = cmd.ExecuteScalar();
            Int32 next = System.Convert.ToInt32(result) + 1;
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            cmd = new SqlCommand($"INSERT INTO Companies(id_company, company_name, company_number, website, admin_id, databaseName) VALUES({next}, '{company.Replace(" ", string.Empty)}', {phone.Replace(" ", string.Empty)}, '{website.Replace(" ", string.Empty)}', null, null)", conn);

            try
            {
                cmd.ExecuteNonQuery();


            }
            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();
        }


        private int getCompanyID(string name)
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT id_company FROM Companies WHERE company_name='{name}'", conn);
            var result = cmd.ExecuteScalar();
            if (result != null)
            {
                company = (int)result;


            }
            else
            {
                company = -1;
            }


            return company;
        }
        private void CreateUser(string username, string password, string name, string email, string phone, string company)
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);



            conn.Open();
            SqlCommand cmd = new SqlCommand($"Select count(*) from Users", conn);
            var result = cmd.ExecuteScalar();
            Int32 Total_ID = System.Convert.ToInt32(result);

            int next = Total_ID + 1;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand check = new SqlCommand($"Select count(*) from Users where uname='{username}'", conn);


            var resultCheck = check.ExecuteScalar();
            Int32 resultUsername = System.Convert.ToInt32(resultCheck);
            if (resultUsername > 0)
            {
                Response.Write("<script type=\"text/javascript\">alert('Uporabniško ime že obstaja.');</script>");
            }
            else
            {

                string finalQueryPermsions = String.Format($"INSERT INTO Permissions(id_permisions) VALUES ({next});");
                SqlCommand createUserPermisions = new SqlCommand(finalQueryPermsions, conn);

                try
                {
                    createUserPermisions.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // Logging module.
                }


                string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");



                string finalQueryRegistration = String.Format($"INSERT INTO Users(uname, Pwd, userRole, id_permisions, id_company, ViewState, FullName, email) VALUES ('{username.Replace(" ", string.Empty)}', '{HashedPassword}', 'Admin', '{next}', '{getCompanyID(company.Replace(" ", string.Empty))}','Viewer&Designer','{name}', '{email.Replace(" ", string.Empty)}')");
                SqlCommand createUser = new SqlCommand(finalQueryRegistration, conn);
                try
                {
                    createUser.ExecuteNonQuery();
                    Response.Write($"<script type=\"text/javascript\">alert('Uspešno kreiran uporabnik.');</script>");

                    UpdateCompany(username.Replace(" ", string.Empty), company.Replace(" ", string.Empty));
                    var spacelessCompany = company.Replace(" ", string.Empty);

                    //fillUsersDelete();
                    string filePath = Server.MapPath($"~/App_Data/{spacelessCompany}/{username}");
                    string replacedPath = filePath.Replace(" ", string.Empty);
                    if (!Directory.Exists(replacedPath.ToString()))
                    {


                        Directory.CreateDirectory(replacedPath.ToString());

                    }
                    else
                    {


                    }
                }
                catch (Exception error)
                {
                    // Implement logging here.
                    Response.Write(error);

                }
            }
        }


        private void UpdateCompany(string username, string company)
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand($"UPDATE Companies SET admin_id='{username}' WHERE company_name='{company}';", conn);
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                // Implement loggin here.
            }
        }
    }
}
