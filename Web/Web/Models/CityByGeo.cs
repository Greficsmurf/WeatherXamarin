using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class CityByGeo
    {
        public res[] results { get; set; }

    }
    public class res {
        public Component components { get; set; }
    }
    public class Component { 
        public String city { get; set; }
    }
}
