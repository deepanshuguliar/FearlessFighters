using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class EquipmentRegistrationmodel
    {
        public string ID { get; set; }
        public string EquipmentName { get; set; }
        public string AirportId { get; set; }
        public string AirportName { get; set; }
        public string InstallationDate { get; set; }
        public string WarrantyPeriod { get; set; }
        public string WarrantyExpirationDate { get; set; }
        public string Status { get; set; }
        public string RelatedTo { get; set; }
        public string MaintainnanceDuration { get; set; }
        public string Daily { get; set; }
        public string Weakly { get; set; }
        public string Monthly { get; set; }
        public string SixMonthly { get; set; }
        public string Yearly { get; set; }
    }
}