using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class VHF_RX_Daily
    {
        public string ID { get; set; }
        public string Date_ { get; set; }
        public string RXNo { get; set; }
        public string Frq_MHZ { get; set; }
        public string BitTest { get; set; }
        public string Status_ { get; set; }
        public string RXNCheck { get; set; }
        public string AC_DC_CO { get; set; }
        public string SQ_Threshold { get; set; }
        public string Remarks { get; set; }
        public string MaintenanceOfficial { get; set; }
        public string Status { get; set; }
    }
}