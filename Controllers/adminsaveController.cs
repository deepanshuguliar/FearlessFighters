using ATS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirportAuthorityofIndia.Controllers
{
    public class adminsaveController : Controller
    {
        // GET: adminsave
        public ActionResult Index()
        {
            string message = "";
            string cmdText = "(" + 1 + ",'CMS" + 1 + "','Delhi-NCR','Delhi','Super','Neeraj Sen','7017064248','c3.neeraj@gmail.com','LaxmiNagar, Delhi','Govt','admin','" + Crypto.Hash("admin@123") + "','Active','AirportAdmin')";

            int result = Universal.ExecuteNonQuery("insert into tbl_AirportRegistration (AirportId,AirportReferenceId,State,City,AirportName,ContactPerson,ContactNumber,Email,Address,AirportType,UserLogin,UserPassword,CurrentStatus,UserType) values" + cmdText);
            if (result == 1)
            {
                message = "Admin setup successfully!";
            }
            else
            {
                message = "Server Error! Please contact admin...";
            }
            return View();
        }
       
    }
}