using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class FlightModule
    {
        public long FlightId { get; set; }
        public string FlightReffNo { get; set; }
        public string FlightNumber { get; set; }
        public string FlightTypeId { get; set; }
        public string FlightTypeText { get; set; }
        public string OperatorName { get; set; }
        public string MaxCapacity { get; set; }
        public string BusinessClass { get; set; }
        public string EconomicClass { get; set; }
        public string HeadName { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string FuelCapacity { get; set; }
        public string Remarks { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByText { get; set; }
        public string CreatedDate { get; set; }
    }
}