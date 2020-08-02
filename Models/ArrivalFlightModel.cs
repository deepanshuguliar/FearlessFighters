using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class ArrivalFlightModel
    {
        public string PlanId { get; set; }
        public string FlightId { get; set; }
        public string FlightName { get; set; }
        public string SourceStationId { get; set; }
        public string SourceStationName { get; set; }
        public string DestinationName { get; set; }
        public string DepartureDate { get; set; }
        public string DepatrureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string Speed { get; set; }
        public string Height { get; set; }
        public string No_of_Passanger { get; set; }
        public string No_of_staff { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string CurrentStatus { get; set; }
    }
}