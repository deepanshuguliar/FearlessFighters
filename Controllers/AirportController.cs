using AirportAuthorityofIndia.Models;
using ATS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirportAuthorityofIndia.Controllers
{
    [Authorize]
    public class AirportController : Controller
    {
        // GET: Airport
        public ActionResult Index()
        {
            return View();
        }
        public long GetMaxAirportId()
        {
            long maxid = 0;
            string id = Universal.ExecuteScalar("select max(AirportId) from tbl_airportregistration").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt64(id);

            }
            maxid = maxid + 1;
            return maxid;
        }
        public JsonResult CheckAirportname(string airportname)
     {
            string message = "";
            object nm= Universal.ExecuteScalar
                ("select AirportName from tbl_airportregistration where AirportName='"+airportname+"'");
            if (nm != null)
            {
                message = "Airport name is already exists!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CheckAirportLoginname(string loginid)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select UserLogin from tbl_airportregistration where UserLogin='" + loginid + "'");
            if (nm != null)
            {
                message = "Airport login name is already exists!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AirportSetup(AirportRegModule airsetup)
        {
            long id = GetMaxAirportId();
            string message = "";
            //string cmdText = "(" + id + ",'CMS" + id + "','"+airsetup.State+"','"+airsetup.City+"','" + airsetup.AirportName + "','" + airsetup.ContactPerson + "','" +
            //    airsetup.ContactNumber + "','" + airsetup.Email + "','" + airsetup.Address + "','" + airsetup.AirportType + "','" +
            //    airsetup.UserLogin + "','" + Crypto.Hash(airsetup.UserPassword) + "','" + airsetup.CurrentStatus + "','AirportAdmin')";
            string cmdText = "(" + id + ",'FIR" + id + "','" + airsetup.State + "','" + airsetup.City + "','" + airsetup.AirportName + "','" + airsetup.ContactPerson + "','" +
                airsetup.ContactNumber + "','" + airsetup.Email + "','" + airsetup.Address + "','None','" +
                airsetup.UserLogin + "','" + Crypto.Hash(airsetup.UserPassword) + "','" + airsetup.CurrentStatus + "','FIRAdmin')";

            int result = Universal.ExecuteNonQuery("insert into tbl_AirportRegistration (AirportId,AirportReferenceId,State,City,AirportName,ContactPerson,ContactNumber,Email,Address,AirportType,UserLogin,UserPassword,CurrentStatus,UserType) values" + cmdText);
            if (result == 1)
            {
                message = "Airport with reference number CMS" + id + " setup successfully!";
            }
            else
            {
                message = "Server Error! Please contact admin...";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Airport()
        {
            return View();
        }

        public ActionResult adminregister()
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
            return View(message) ;
        }



        public ActionResult GetAirportlist()
        {
            List<AirportRegModule> airport = new List<AirportRegModule>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_airportregistration where AirportId!=1 And FIRID=''", "tbl_airportregistration");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AirportRegModule cmd = new AirportRegModule();
                cmd.AirportId =Convert.ToInt32(dt.Rows[i]["AirportId"]);
                cmd.AirportReferenceId = dt.Rows[i]["AirportReferenceId"].ToString();
                cmd.State = dt.Rows[i]["State"].ToString();
                cmd.City = dt.Rows[i]["City"].ToString();
                cmd.AirportName = dt.Rows[i]["AirportName"].ToString();
                cmd.ContactPerson = dt.Rows[i]["ContactPerson"].ToString();
                cmd.ContactNumber = dt.Rows[i]["ContactNumber"].ToString();
                cmd.Email =dt.Rows[i]["Email"].ToString();
                cmd.Address = dt.Rows[i]["Address"].ToString();
                //cmd.AirportType = dt.Rows[i]["AirportType"].ToString();
                cmd.UserLogin = dt.Rows[i]["UserLogin"].ToString();
                cmd.CurrentStatus = dt.Rows[i]["CurrentStatus"].ToString();

                airport.Add(cmd);
            }

            var details = airport;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var airport = Universal.SelectWithDS("select * from tbl_airportregistration where AirportId=" + id, "tbl_airportregistration");
            if(airport.Rows.Count>0)
            {
                //for (int i = 0; i < airport.Rows.Count; i++)
                //{
                    AirportRegModule info = new AirportRegModule
                    {
                        AirportId = Convert.ToInt32(airport.Rows[0]["AirportId"]),
                        AirportReferenceId = airport.Rows[0]["AirportReferenceId"].ToString(),
                        State = airport.Rows[0]["State"].ToString(),
                        City = airport.Rows[0]["City"].ToString(),
                        AirportName = airport.Rows[0]["AirportName"].ToString(),
                        ContactPerson = airport.Rows[0]["ContactPerson"].ToString(),
                        ContactNumber = airport.Rows[0]["ContactNumber"].ToString(),
                        Email = airport.Rows[0]["Email"].ToString(),
                        Address = airport.Rows[0]["Address"].ToString(),
                       // AirportType = airport.Rows[0]["AirportType"].ToString(),
                      //UserLogin = airport.Rows[0]["UserLogin"].ToString(),
                        CurrentStatus = airport.Rows[0]["CurrentStatus"].ToString(),
                };
            //}
                return View(info);
            }
            return View();
        }
       
        
        public JsonResult EditAirport(int id, AirportRegModule airport)
        {
            string message = "";
          if(airport!=null)
            {
                //int res = Universal.ExecuteNonQuery("Update tbl_airportregistration set State='" + airport.State +
                //    "',City='" + airport.City + "',AirportName='" + airport.AirportName + "',ContactPerson='" +
                //    airport.ContactPerson + "',ContactNumber='" + airport.ContactNumber + "',Email='" +
                //    airport.Email + "',Address='" + airport.Address + "',AirportType='" + airport.AirportType +
                //    "',CurrentStatus='" + airport.CurrentStatus + "' where AirportId=" + id); 
                int res = Universal.ExecuteNonQuery("Update tbl_airportregistration set State='" + airport.State +
    "',City='" + airport.City + "',AirportName='" + airport.AirportName + "',ContactPerson='" +
    airport.ContactPerson + "',ContactNumber='" + airport.ContactNumber + "',Email='" +
    airport.Email + "',Address='" + airport.Address + "',AirportType='None',CurrentStatus='" + airport.CurrentStatus + "' where AirportId=" + id);
                if (res==1)
                {
                    message = "Update successfully!";
                }
            }
            return Json(message,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult delete(int id)
        {
            int res = Universal.ExecuteNonQuery("delete from tbl_airportregistration where AirportId=" + id);
            return RedirectToAction("Airport", "Airport");
        }

        public List<SelectListItem> GetFirlist()
        {
            List<SelectListItem> firlist = new List<SelectListItem>();

            List<AirportRegModule> fir_list = new List<AirportRegModule>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_AirportRegistration where UserType!='SuperAdmin' And FIRID=''", "tbl_AirportRegistration");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AirportRegModule cmd = new AirportRegModule();
                cmd.AirportId = Convert.ToInt32(dt.Rows[i]["AirportId"]);
                cmd.AirportName = dt.Rows[i]["AirportName"].ToString();
                fir_list.Add(cmd);
            }
            foreach (AirportRegModule info in fir_list)
            {
                SelectListItem list = new SelectListItem
                {
                    Text = info.AirportName,
                    Value = info.AirportId.ToString()
                };
                firlist.Add(list);
            }
            return firlist;
        }

        public long GetMaxATCId()
        {
            long maxid = 0;
            string id = Universal.ExecuteScalar("select max(AtcId) from tbl_atcregistration").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt64(id);

            }
            maxid = maxid + 1;
            return maxid;
        }

        public JsonResult FIRs()
        {
            var firslist = GetFirlist();
            return Json(firslist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ATC()
        {
            return View();
        }
        public JsonResult CheckAtcname(string atcname)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select AirportName from tbl_airportregistration where AirportName='" + atcname + "'");
            if (nm != null)
            {
                message = "ATC name is already exists!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckAtcLoginname(string loginid)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select UserLogin from tbl_airportregistration where UserLogin='" + loginid + "'");
            if (nm != null)
            {
                message = "ATC login name is already exists!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AtcSetup(ATCModule atcsetup)
        {
            long id = GetMaxAirportId();
            string message = "";
            string cmdText = "(" + id + ",'ATC" + id + "','" + atcsetup.State + "','" + atcsetup.City + "','" + atcsetup.ATCName + "','" + atcsetup.ContactPerson + "','" +
                atcsetup.ContactNumber + "','" + atcsetup.Email + "','" + atcsetup.Address + "','None','" +
                atcsetup.UserLogin + "','" + Crypto.Hash(atcsetup.UserPassword) + "','" + atcsetup.CurrentStatus + "','ATCAdmin','" + atcsetup.FIRId + "','" + atcsetup.FIRName + "')";

            int result = Universal.ExecuteNonQuery("insert into tbl_AirportRegistration (AirportId,AirportReferenceId,State,City,AirportName,ContactPerson,ContactNumber,Email,Address,AirportType,UserLogin,UserPassword,CurrentStatus,UserType,FIRID,FIRName) values" + cmdText);
            if (result == 1)
            {
                message = "ATC with reference number CMSATC" + id + " setup successfully!";
            }
            else
            {
                message = "Server Error! Please contact admin...";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllATC()
        {
            return View();
        }
        public ActionResult GetATClist()
        {
            List<ATCModule> cities = new List<ATCModule>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_airportregistration where FIRID!=''", "tbl_atcregistration");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ATCModule cmd = new ATCModule();
                cmd.FIRName = dt.Rows[i]["FIRName"].ToString();
                cmd.ATCId = Convert.ToInt32(dt.Rows[i]["AirportId"]);
                cmd.ATCReferenceId = dt.Rows[i]["AirportReferenceId"].ToString();
                cmd.State = dt.Rows[i]["State"].ToString();
                cmd.City = dt.Rows[i]["City"].ToString();
                cmd.ATCName = dt.Rows[i]["AirportName"].ToString();
                cmd.ContactPerson = dt.Rows[i]["ContactPerson"].ToString();
                cmd.ContactNumber = dt.Rows[i]["ContactNumber"].ToString();
                cmd.Email = dt.Rows[i]["Email"].ToString();
                cmd.Address = dt.Rows[i]["Address"].ToString();
                //cmd.AirportType = dt.Rows[i]["AirportType"].ToString();
                cmd.UserLogin = dt.Rows[i]["UserLogin"].ToString();
                cmd.CurrentStatus = dt.Rows[i]["CurrentStatus"].ToString();
                cities.Add(cmd);
            }

            var details = cities;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditATC(int id)
        {
            var airport = Universal.SelectWithDS("select * from tbl_atcregistration where AtcId=" + id, "tbl_atcregistration");
            if (airport.Rows.Count > 0)
            {
                ATCModule info = new ATCModule
                {
                    ATCId = Convert.ToInt32(airport.Rows[0]["AtcId"]),
                    ATCReferenceId = airport.Rows[0]["AtcReferenceId"].ToString(),
                    State = airport.Rows[0]["State"].ToString(),
                    City = airport.Rows[0]["City"].ToString(),
                    ATCName = airport.Rows[0]["AtcName"].ToString(),
                    ContactPerson = airport.Rows[0]["ContactPerson"].ToString(),
                    ContactNumber = airport.Rows[0]["ContactNumber"].ToString(),
                    Email = airport.Rows[0]["Email"].ToString(),
                    Address = airport.Rows[0]["Address"].ToString(),
                    CurrentStatus = airport.Rows[0]["CurrentStatus"].ToString(),
                };
                return View(info);
            }
            return View();
        }

        public JsonResult EditATC(int id, ATCModule atc)
        {
            string message = "";
            if (atc != null)
            {
                int res = Universal.ExecuteNonQuery("Update tbl_atcregistration set Firid="+atc.FIRId+ ",FirName='"+atc.FIRName+"',State='" + atc.State +
    "',City='" + atc.City + "',AtcName='" + atc.ATCName + "',ContactPerson='" +
    atc.ContactPerson + "',ContactNumber='" + atc.ContactNumber + "',Email='" +
    atc.Email + "',Address='" + atc.Address + "',AtcType='None',CurrentStatus='" + atc.CurrentStatus + "' where AtcId=" + id);
                if (res == 1)
                {
                    message = "Update successfully!";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

    }
}