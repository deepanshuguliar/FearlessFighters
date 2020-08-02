using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class VHF_TX_Monthly
    {
        public string ID { get; set; }
        public string Date_ { get; set; }
        public string Month_ { get; set; }
        public string TxNo { get; set; }
        public string FreqMHz { get; set; }
        public string Power_O_P_FWD_W { get; set; }
        public string SET_LINE_I_P_dBm { get; set; }
        public string SET_INHIBIT { get; set; }
        public string SET_TIMEOUT_sec { get; set; }
        public string SET_MODN_DEPTH { get; set; }
        public string SET_VOGAD { get; set; }
        public string SET_KEY_PRIORITY { get; set; }
        public string Ready_Signal { get; set; }
        public string PTT_I_P { get; set; }
        public string PHANTOM_PTT_I_P { get; set; }
        public string PTT_REF_VOLTAGE_V { get; set; }
        public string PTT_O_P { get; set; }
        public string Bit_Test { get; set; }
        public string Remarks { get; set; }
        public string Maintenance_Official { get; set; }
        public string Next_Maintenance_Date { get; set; }
        public string Status { get; set; }
    }
}