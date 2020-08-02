using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class VHF_RX_Monthly
    {
        public string Date_ { get; set; }
        public string Month_ { get; set; }
        public string RXNo { get; set; }
        public string FreqMHz { get; set; }
        public string LineOP { get; set; }
        public string Threshold { get; set; }
        public string Modulation { get; set; }
        public string Defeat { get; set; }
        public string DefeatIP { get; set; }
        public string Carrier { get; set; }
        public string AGC { get; set; }
        public string Marc { get; set; }
        public string Facility { get; set; }
        public string Phantom { get; set; }
        public string Ready { get; set; }
        public string BitTest { get; set; }
        public string Remarks { get; set; }
        public string MaintenanceOfficial { get; set; }
        public string Nextdate { get; set; }
        public string Status { get; set; }
    }
}