using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class VHF_RX_Yearly
    {
        public string ID { get; set; }
        public string Date_ { get; set; }
        public string Year { get; set; }
        public string RXNo { get; set; }
        public string FreqMHz { get; set; }
        public string BitTest { get; set; }
        public string Sensitivity { get; set; }
        public string ACDC { get; set; }
        public string Reffreq { get; set; }
        public string Remarks { get; set; }
        public string MaintenanceOfficial { get; set; }
        public string Nextdate { get; set; }
        public string Status { get; set; }
    }
}