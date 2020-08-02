using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirportAuthorityofIndia.Models
{
    public class UserLogin
    {
        [Display(Name = "Login Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login ID required")]
        public string loginname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string loginpassword { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public int? UserRoleId { get; set; }
        public string RoleName { get; set; }

    }
}