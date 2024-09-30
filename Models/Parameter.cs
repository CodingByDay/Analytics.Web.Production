using System;
using System.Collections.Generic;

namespace Dash.Models
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