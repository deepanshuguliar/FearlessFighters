using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirportAuthorityofIndia.Controllers
{
    public class StateMasterController : Controller
    {
        // GET: StateMaster
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(String statename)
        {
            try
            {
                if (statename != "")
                {
                    ATS.Universal.ExecuteNonQuery("insert into tbl_statemaster (StateName) values('" + statename + "')");
                    Response.Write("<script>alert('Data Saved')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Enter StateName')</script>");
                }
            }
            catch (Exception ex) { }
            return View();
        }

    }
}