using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class ATCModule
    {
        public int FIRId { get; set; }
        public string FIRName { get; set; }
        public int ATCId { get; set; }
        public string ATCReferenceId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ATCName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ATCType { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string CurrentStatus { get; set; }
        public string UserType { get; set; }
    }
}