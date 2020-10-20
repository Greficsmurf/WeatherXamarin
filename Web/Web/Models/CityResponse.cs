using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class CityName {
        public string name { get; set; }
    }
    public class CityResponse
    {
        public string name { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
    }
    public class Main
    {
        public string temp { get; set; }
        public string pressure { get; set; }
        public string humidity { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }

    }
    public class Wind {
        public string speed { get; set; }
        public string deg { get; set; }
    }
}
