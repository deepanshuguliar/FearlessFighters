using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class StaffModule
    {
        public long StaffId { get; set; }
        public string StaffReffNo { get; set; }
        public string StaffName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string StaffTypeId { get; set; }
        public string StaffTypeText { get; set; }
        public int CreateById { get; set; }
        public string CreatedByText { get; set; }
        public string CreatedDate { get; set; }
    }
}