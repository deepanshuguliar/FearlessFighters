using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class VHF_TX_Daily
    {
        public string ID { get; set; }
        public string Dated { get; set; }
        public string TXNo { get; set; }
        public string FreqMHz { get; set; }
        public string BitTest { get; set; }
        public string Status { get; set; }
        public string ACPower { get; set; }
        public string TXCheck { get; set; }
        public string TxnCheck { get; set; }
        public string AC_DC_CO { get; set; }
        public string Remarks { get; set; }
        public string MaintenanceOfficial { get; set; }
        public string Status_ { get; set; }
    }
}