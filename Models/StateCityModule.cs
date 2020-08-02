using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class StateModule
    {
        public int stateId { get; set; }
        public string stateName { get; set; }
    }

    public class CityModule
    {
        public string cityName { get; set; }
        public int cityId { get; set; }
        public int stateId { get; set; }
        public string stateName { get; set; }

    }
}