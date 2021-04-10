using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace peptak.Session
{
    public class CustomerSessionClass
    {


        public string name { get; set; }
        public string surname { get; set; }

        public string email { get; set; }

        public DateTime dateOfOrder { get; set; }
    }
}