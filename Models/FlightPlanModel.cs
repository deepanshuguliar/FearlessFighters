using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class FlightPlanModel
    {
        public string PlanId { get; set; }
        public string FlightId { get; set; }
        public string FlightName { get; set; }
        public string FIRID { get; set; }
        public string FIRName { get; set; }
        public string ATCID { get; set; }
        public string ATCName { get; set; }
        public string SourceStationId { get; set; }
        public string SourceStationName { get; set; }
        public string DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string Date_ { get; set; }
        public string ExpectedDepatrureTime { get; set; }
        public string ExpectedArrivalTime { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string Speed { get; set; }
        public string Height { get; set; }
        public string No_of_Passanger { get; set; }
        public string No_of_staff { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string routeatc { get; set; }
        public string PilotID { get; set; }
        public string PilotName { get; set; }
        public string CaptainID { get; set; }
        public string CaptainName { get; set; }
        public string CabinCrewId { get; set; }
        public string CabinCrewName { get; set; }
        public string CurrentStatus { get; set; }

    }
}