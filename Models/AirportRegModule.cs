using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class AirportRegModule
    {
        public int AirportId { get; set; }
        public string AirportReferenceId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string AirportName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AirportType { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string CurrentStatus { get; set; }
        public string UserType { get; set; }    
    }
}