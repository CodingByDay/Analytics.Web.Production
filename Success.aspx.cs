using Dash.HelperClasses;
using Dash.Session;
using Stripe;
using Stripe.Checkout;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;

namespace Dash
{
    public partial class Success : System.Web.UI.Page
    {
        public static CustomerSessionClass customer = new CustomerSessionClass();
        public static NewPayedUserCompany user = new NewPayedUserCompany();
        private SqlConnection conn;
        private int company;
        private string id;

        protected void Page_Load(object sender, EventArgs e)
        {

            // Archived page for Stripe API.



            // Stripe configuration key. Safe to use like this...
            StripeConfiguration.ApiKey = "sk_test_51IdsgFI2g0bX7R9aOa1atoifUOkT6csUhopE53KQYUA55UZmKVPdWP3BLcZ9UTqF6zVIn0OIpeAXlL5CiiLHFWv900tcoe7Bzt";
            // Getting the query string.
            id = Request.QueryString["id"];
            // testing the query string.
            /// Response.Write(id.ToString());
            /// Page that will show all the data for the session and also write info to the database.
            var service = new SessionService();
            var session = service.Get(id);

            var email = session.CustomerDetails.Email;
            var customerObject = new CustomerService();
            var customerObj = customerObject.Get(session.CustomerId);
            customer.email = email;
            customer.dateOfOrder = DateTime.Now;

            // Functionality
            DefaultEmailFill();





        }

        /// <summary>
        ///  Fill in the email form field with the stripe related email and disable it.
        /// </summary>
        private void DefaultEmailFill()
        {
            EmailForm.Text = customer.email;
            EmailForm.Enabled = false;
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT count(*) FROM companies", conn);
            var result = cmd.ExecuteScalar();
            Int32 next = System.Convert.ToInt32(result) + 1;
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"Select count(*) from Users", conn);
            var result = cmd.ExecuteScalar();
            Int32 Total_ID = System.Convert.ToInt32(result);

            int next = Total_ID + 1;

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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


                createMembership(company.Replace(" ", string.Empty));
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

        private void createMembership(string v)
        {
            var idCompany = getCompanyID(v.Replace(" ", string.Empty));
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"insert into memberships(id_company,startDate,endDate,id_types_of_memberships, stripe) VALUES({idCompany}, GETDATE(), GETDATE() + 365,2,'{id}');", conn);
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                // Implement loggin here.
            }
        }

        private void UpdateCompany(string username, string company)
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=dashboards;Password=Cporje?%ofgGHH$984d4L;");
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
