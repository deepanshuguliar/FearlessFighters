using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class VHF_TX_Yearly
    {
        public string ID { get; set; }
        public string Date_ { get; set; }
        public string Year { get; set; }
        public string TXNo { get; set; }
        public string FreqMHz { get; set; }
        public string ReferenceFreq_MHz { get; set; }
        public string Power_W { get; set; }
        public string BitTest { get; set; }
        public string AC_DC_Change_Over { get; set; }
        public string Remarks { get; set; }
        public string MaintenanceOfficial { get; set; }
        public string NextMaintenanceDate { get; set; }
        public string Status { get; set; }
    }
}