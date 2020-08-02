using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class Country
    {
        public int countryid { get; set; }
        public string countrynickname { get; set; }
        public string countryname { get; set; }
    }

    public class State
    {
        public int stateid { get; set; }
        public string statename { get; set; }
        public int countryid { get; set; }
    }

    public class City
    {
        public int cityid { get; set; }
        public string cityname { get; set; }
        public int stateid { get; set; }
    }
}