namespace Dash.ORM
{
    public class User
    {

        public string uname { get; set; }

        public string Pwd { get; set; }


        public string userRole { get; set; }


        public string ViewState { get; set; }


        public string email { get; set; }

        public User(string uname, string Pwd, string userRole, string ViewState, string email)
        {
            this.uname = uname;
            this.Pwd = Pwd;
            this.userRole = userRole;
            this.ViewState = ViewState;
            this.email = email;
        }



        public User()
        {

        }
    }
}