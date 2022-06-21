using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dash.ORM
{
    public class Parameter
    {
        public string Name { get; set; }
        public DateTime Value { get; set; }


        public class Root
        {
            public List<Parameter> Params { get; set; }
        }

    }
}