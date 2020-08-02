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
    public class SubAdminController : Controller
    {
        // GET: SubAdmin
        public ActionResult Index()
        {
            return View();
        }

        public List<SelectListItem> GetCountries()
        {
            List<SelectListItem> countrylist = new List<SelectListItem>();

            List<Country> country = new List<Country>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_countries", "tbl_countries");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Country cmd = new Country();
                cmd.countryid = Convert.ToInt32(dt.Rows[i]["id"]);
                cmd.countryname = dt.Rows[i]["name"].ToString();
                country.Add(cmd);
            }
            var details = country;
            foreach (Country info in details)
            {
                SelectListItem crslist = new SelectListItem
                {
                    Text = info.countryname,
                    Value = info.countryid.ToString()
                };
                countrylist.Add(crslist);
            }
            return countrylist;
        }

        public JsonResult GetCountrylist()
        {
            var country_list = GetCountries();
            return Json(country_list, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetStates(int countryid)
        {
            List<SelectListItem> statelist = new List<SelectListItem>();

            List<State> state = new List<State>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_states where country_id=" + countryid, "tbl_states");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                State cmd = new State();
                cmd.stateid = Convert.ToInt32(dt.Rows[i]["id"]);
                cmd.statename = dt.Rows[i]["name"].ToString();
                state.Add(cmd);
            }
            var details = state;
            foreach (State info in details)
            {
                SelectListItem crslist = new SelectListItem
                {
                    Text = info.statename,
                    Value = info.stateid.ToString()
                };
                statelist.Add(crslist);
            }
            return statelist;
        }

        public JsonResult GetStateList(string id)
        {
            var state_list = GetStates(Convert.ToInt32(id));
            return Json(state_list, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetCities(int stateid)
        {
            List<SelectListItem> statelist = new List<SelectListItem>();

            List<City> state = new List<City>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_cities where state_id=" + stateid, "tbl_cities");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                City cmd = new City();
                cmd.cityid = Convert.ToInt32(dt.Rows[i]["id"]);
                cmd.cityname = dt.Rows[i]["name"].ToString();
                state.Add(cmd);
            }
            var details = state;
            foreach (City info in details)
            {
                SelectListItem crslist = new SelectListItem
                {
                    Text = info.cityname,
                    Value = info.cityid.ToString()
                };
                statelist.Add(crslist);
            }
            return statelist;
        }

        public JsonResult GetCitylist(string id)
        {
            var cities = GetCities(Convert.ToInt32(id));
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmail(string mailid)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select Email from tbl_StaffRegistration where Email='" + mailid + "'");
            if (nm != null)
            {
                message = "Email is already exists!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckContactNo(string contactno)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select ContactNo from tbl_StaffRegistration where ContactNo='" + contactno + "'");
            if (nm != null)
            {
                message = "Contact no is already exists!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Staff()
        {
            return View();
        }
        public long GetMaxEmployeeId()
        {
            long maxid = 0;
            string id = Universal.ExecuteScalar("select max(StaffId) from tbl_StaffRegistration").ToString();

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
        [Authorize]
        public JsonResult EmployeeSetup(StaffModule emp)
        {
            string message = "";
            if (TempData["airportid"] != null)
            {
                string aa = TempData["airportid"].ToString();
                string bb = TempData["airportname"].ToString();
                long id = GetMaxEmployeeId();

                string cmdText = "(" + id + ",'FIREMP" + id + "','" + emp.StaffName + "','" + emp.ContactNo + "','" + emp.Email + "','" + emp.Age + "','" +
                    emp.Gender + "','" + emp.Address + "','" + emp.StaffTypeId + "','" + emp.StaffTypeText + "','" +
                    aa + "','" + bb + "','" + DateTime.Now.ToShortDateString() + "')";

                int result = Universal.ExecuteNonQuery("insert into tbl_StaffRegistration (StaffId,StaffReffNo,StaffName,ContactNo,Email,Age,Gender,Address,StaffTypeId,StaffTypeText,CreateById,CreatedByText,CreatedDate) values" + cmdText);
                if (result == 1)
                {
                    message = "Employee with reference number FIREMP" + id + " setup successfully!";
                }
                else
                {
                    message = "Server Error! Please contact admin...";
                }
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllStaff()
        {
            if (Session["airportid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult GetStafflist()
        {
            if (Session["airportid"] != null)
            {
                List<StaffModule> staff = new List<StaffModule>();
                DataTable dt = Universal.SelectWithDS("select * from tbl_StaffRegistration where CreateById='" + Session["airportid"].ToString() + "'", "tbl_StaffRegistration");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StaffModule cmd = new StaffModule();
                    cmd.StaffId = Convert.ToInt32(dt.Rows[i]["StaffId"]);
                    cmd.StaffReffNo = dt.Rows[i]["StaffReffNo"].ToString();
                    cmd.StaffTypeText = dt.Rows[i]["StaffTypeText"].ToString();
                    cmd.StaffName = dt.Rows[i]["StaffName"].ToString();
                    cmd.ContactNo = dt.Rows[i]["ContactNo"].ToString();
                    cmd.Email = dt.Rows[i]["Email"].ToString();
                    cmd.Age = dt.Rows[i]["Age"].ToString();
                    cmd.Address = dt.Rows[i]["Address"].ToString();
                    cmd.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
                    staff.Add(cmd);
                }

                var details = staff;
                return Json(new { data = details }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult EditStaff(int id)
        {
            var empinfo = Universal.SelectWithDS("select * from tbl_StaffRegistration where StaffId=" + id, "tbl_StaffRegistration");
            if (empinfo.Rows.Count > 0)
            {
                StaffModule info = new StaffModule
                {
                    StaffId = Convert.ToInt32(empinfo.Rows[0]["StaffId"]),
                    StaffReffNo = empinfo.Rows[0]["StaffReffNo"].ToString(),
                    StaffName = empinfo.Rows[0]["StaffName"].ToString(),
                    ContactNo = empinfo.Rows[0]["ContactNo"].ToString(),
                    Email = empinfo.Rows[0]["Email"].ToString(),
                    Age = empinfo.Rows[0]["Age"].ToString(),
                    Address = empinfo.Rows[0]["Address"].ToString(),
                    StaffTypeId = empinfo.Rows[0]["StaffTypeId"].ToString(),
                    StaffTypeText = empinfo.Rows[0]["StaffTypeText"].ToString(),
                    Gender = empinfo.Rows[0]["Gender"].ToString(),
                };
                return View(info);
            }
            return View();
        }
        [HttpPost]
        public JsonResult EditStaffDetails(int id, StaffModule staff)
        {
            string message = "";
            string aa = Session["airportid"].ToString();
            string bb = Session["airportname"].ToString();

            if (staff != null)
            {
                int res = Universal.ExecuteNonQuery("Update tbl_StaffRegistration set StaffName='" + staff.StaffName +
    "',ContactNo='" + staff.ContactNo + "',Email='" + staff.Email + "',Age='" +
    staff.Age + "',Gender='" + staff.Gender + "',Address='" +
    staff.Address + "',StaffTypeId='" + staff.StaffTypeId + "',StaffTypeText='" + staff.StaffTypeText + "',CreateById='" + aa + "',CreatedByText='" + bb + "' where StaffId=" + id);
                if (res == 1)
                {
                    message = "Update successfully!";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteStaff(int id)
        {
            int res = Universal.ExecuteNonQuery("delete from tbl_StaffRegistration where StaffId=" + id);
            return RedirectToAction("AllStaff", "SubAdmin");

        }

        public ActionResult Flight()
        {
            return View();
        }
        public JsonResult CheckFlightEmail(string mailid)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select EmailId from tbl_FlightRegistration where EmailId='" + mailid + "'");
            if (nm != null)
            {
                message = "Email is already exists!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckFlightContactNo(string contactno)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select ContactNo from tbl_FlightRegistration where ContactNo='" + contactno + "'");
            if (nm != null)
            {
                message = "Contact no is already exists!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckFlightNo(string flightno)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select FlightNumber from tbl_FlightRegistration where FlightNumber='" + flightno + "'");
            if (nm != null)
            {
                message = "Flight number no is already exists!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public long GetMaxFlightId()
        {
            long maxid = 0;
            string id = Universal.ExecuteScalar("select max(FlightId) from tbl_FlightRegistration").ToString();

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

        public long GetMaxFlightPlanID()
        {
            long maxid = 0;
            string id = Universal.ExecuteScalar("select max(PlanId) from tbl_flight_plan_master").ToString();

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
        public long GetMaxFlightPlanIDNotPlus()
        {
            long maxid = 0;
            string id = Universal.ExecuteScalar("select max(PlanId) from tbl_flight_plan_master").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt64(id);

            }
           // maxid = maxid + 1;
            return maxid;
        }

        [Authorize]
        public JsonResult SaveFlightPlanByFIR(FlightPlanModel flightmodel)
        {
            string message = "";
            if (Session["airportid"] != null)
            {
                string aa = Session["airportid"].ToString();
                string bb = Session["airportname"].ToString();

                string cmdText = "(" + flightmodel.FlightId + ",'" + flightmodel.FlightName + "','" + aa + "','" + bb + "','" + aa + "','" +
                    bb + "','" + flightmodel.DestinationId + "','" + flightmodel.DestinationName + "','" + flightmodel.Date_ + "','" +
                    flightmodel.ExpectedDepatrureTime + "','" + flightmodel.ExpectedArrivalTime + "','" + flightmodel.Distance + "','" + flightmodel.Duration + "','" + flightmodel.Speed + "','" + flightmodel.Height + "','" + flightmodel.No_of_Passanger + "','" + flightmodel.No_of_staff + "','" + flightmodel.Remark + "','Ready')";

                int result = Universal.ExecuteNonQuery("insert into tbl_flight_plan_master (FlightId,FlightName,FIRID,FIRName,SourceStationId,SourceStationName,DestinationId,DestinationName,Date_,ExpectedDepatrureTime,ExpectedArrivalTime,Distance,Duration,Speed,Height,No_of_Passanger,No_of_staff,Remark,Status) values" + cmdText);
                if (result == 1)
                {
                    if (flightmodel.routeatc != null && flightmodel.routeatc.Contains("-"))
                    {
                        string[] routelist = flightmodel.routeatc.Split(',');
                        long planid = GetMaxFlightPlanIDNotPlus();
                        string cmd = "insert into ref_flight_plan_master (PlanId,ATCID,ATCName,Status) values";
                        foreach (string route in routelist)
                        {
                            if (route != "")
                            {
                                string[] routed = route.Split('-');
                                string atcid = routed[0];
                                string atcname = routed[1];
                                cmd += "('" + planid + "','" + atcid + "','" + atcname + "','Active'),";
                            }
                        }
                        cmd = cmd.Remove(cmd.Length - 1, 1);
                        Universal.ExecuteNonQuery(cmd);
                    }
                    message = "Flight plan saved successfully!";
                }
                else
                {
                    message = "Server Error! Please contact admin...";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult FlightSetup(FlightModule flight)
        {
            string message = "";
            if (Session["airportid"] != null)
            {
                string aa = Session["airportid"].ToString();
                string bb = Session["airportname"].ToString();
                long id = GetMaxFlightId();

                string cmdText = "(" + id + ",'Flight" + id + "','" + flight.FlightNumber + "','" + flight.FlightTypeId + "','" + flight.FlightTypeText + "','" + flight.OperatorName + "','" +
                    flight.MaxCapacity + "','" + flight.BusinessClass + "','" + flight.EconomicClass + "','" + flight.HeadName + "','" +
                    flight.ContactNo + "','" + flight.EmailId + "','" + flight.FuelCapacity + "','" + flight.Remarks + "','" + aa + "','" + bb + "','" + DateTime.Now.ToShortDateString() + "')";

                int result = Universal.ExecuteNonQuery("insert into tbl_FlightRegistration (FlightId,FlightReffNo,FlightNumber,FlightTypeId,FlightTypeText,OperatorName,MaxCapacity,BusinessClass,EconomicClass,HeadName,ContactNo,EmailId,FuelCapacity,Remarks,CreatedById,CreatedByText,CreatedDate) values" + cmdText);
                if (result == 1)
                {
                    message = "Flight with reference number Flight" + id + " saved successfully!";
                }
                else
                {
                    message = "Server Error! Please contact admin...";
                }
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Flightlist()
        {
            if (Session["airportid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult ATCList()
        {
            if (Session["airportid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult GetATCListofFIR()
        {
            if (Session["airportid"] != null)
            {
                List<ATCModule> atclist = new List<ATCModule>();
                DataTable dt = Universal.SelectWithDS("select * from tbl_airportregistration where FIRID='" + Session["airportid"].ToString() + "'", "tbl_atcregistration");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ATCModule cmd = new ATCModule();
                    cmd.ATCReferenceId = dt.Rows[i]["AirportReferenceId"].ToString();
                    cmd.State = dt.Rows[i]["State"].ToString();
                    cmd.City = dt.Rows[i]["City"].ToString();
                    cmd.ATCType = dt.Rows[i]["AirportType"].ToString();
                    cmd.ContactNumber = dt.Rows[i]["ContactNumber"].ToString();
                    cmd.ContactPerson = dt.Rows[i]["ContactPerson"].ToString();
                    cmd.Email = dt.Rows[i]["Email"].ToString();
                    cmd.ATCName = dt.Rows[i]["AirportName"].ToString();
                    atclist.Add(cmd);
                }
                var details = atclist;
                return Json(new { data = details }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        public ActionResult FlightPlan()
        {
            if (Session["airportid"] != null)
            {
                //For Flight List
                DataTable dtflights = Universal.SelectWithDS("select FlightId,FlightNumber,FlightTypeText from tbl_flightregistration ORDER BY FlightNumber", "tbl_flightregistration");
                ViewBag.FlightList = dtflights;

                //For ATC And FIR List
                DataTable dtATclist = Universal.SelectWithDS("select AirportId,AirportName,City,State from tbl_airportregistration ORDER BY AirportName", "tbl_atcregistration");
                ViewBag.AtcList = dtATclist;

                //Get Piolotlist
                DataTable dtPilotlist = Universal.SelectWithDS("select StaffId,StaffName from tbl_staffregistration where StaffTypeText='Pilot'", "tbl_staffregistration");
                ViewBag.pilotlist = dtPilotlist;

                //Get Caption
                DataTable dtCaptionlist = Universal.SelectWithDS("select StaffId,StaffName from tbl_staffregistration where StaffTypeText='Captain'", "tbl_staffregistration");
                ViewBag.captionlist = dtCaptionlist;

                DataTable dtcabincrew = Universal.SelectWithDS("select StaffId,StaffName from tbl_staffregistration where StaffTypeText='Cabin Crew'", "tbl_staffregistration");
                ViewBag.cabincrew = dtcabincrew;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult GetFlightlist()
        {
            if (Session["airportid"] != null)
            {
                List<FlightModule> staff = new List<FlightModule>();
                DataTable dt = Universal.SelectWithDS("select * from tbl_FlightRegistration where CreatedById='" + Session["airportid"].ToString() + "'", "tbl_FlightRegistration");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FlightModule cmd = new FlightModule();
                    cmd.FlightId = Convert.ToInt32(dt.Rows[i]["FlightId"]);
                    cmd.FlightNumber = dt.Rows[i]["FlightNumber"].ToString();
                    cmd.FlightTypeText = dt.Rows[i]["FlightTypeText"].ToString();
                    cmd.OperatorName = dt.Rows[i]["OperatorName"].ToString();
                    cmd.HeadName = dt.Rows[i]["HeadName"].ToString();
                    cmd.ContactNo = dt.Rows[i]["ContactNo"].ToString();
                    cmd.EmailId = dt.Rows[i]["EmailId"].ToString();
                    cmd.MaxCapacity = dt.Rows[i]["MaxCapacity"].ToString();
                    cmd.BusinessClass = dt.Rows[i]["BusinessClass"].ToString();
                    cmd.EconomicClass = dt.Rows[i]["EconomicClass"].ToString();
                    cmd.FuelCapacity = dt.Rows[i]["FuelCapacity"].ToString();
                    staff.Add(cmd);
                }

                var details = staff;
                return Json(new { data = details }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public ActionResult EditFlight(int id)
        {
            var empinfo = Universal.SelectWithDS("select * from tbl_FlightRegistration where FlightId=" + id, "tbl_FlightRegistration");
            if (empinfo.Rows.Count > 0)
            {
                FlightModule info = new FlightModule
                {
                    FlightId = Convert.ToInt32(empinfo.Rows[0]["FlightId"]),
                    FlightReffNo = empinfo.Rows[0]["FlightReffNo"].ToString(),
                    FlightNumber = empinfo.Rows[0]["FlightNumber"].ToString(),
                    FlightTypeId = empinfo.Rows[0]["FlightTypeId"].ToString(),
                    FlightTypeText = empinfo.Rows[0]["FlightTypeText"].ToString(),
                    OperatorName = empinfo.Rows[0]["OperatorName"].ToString(),
                    MaxCapacity = empinfo.Rows[0]["MaxCapacity"].ToString(),
                    BusinessClass = empinfo.Rows[0]["BusinessClass"].ToString(),
                    EconomicClass = empinfo.Rows[0]["EconomicClass"].ToString(),
                    HeadName = empinfo.Rows[0]["HeadName"].ToString(),
                    ContactNo = empinfo.Rows[0]["ContactNo"].ToString(),
                    EmailId = empinfo.Rows[0]["EmailId"].ToString(),
                    FuelCapacity = empinfo.Rows[0]["FuelCapacity"].ToString(),
                    Remarks = empinfo.Rows[0]["Remarks"].ToString(),
                };
                return View(info);
            }
            return View();
        }
        [HttpPost]
        public JsonResult EditFlightDetails(int id, FlightModule flight)
        {
            string message = "";
            string aa = Session["airportid"].ToString();
            string bb = Session["airportname"].ToString();

            if (flight != null)
            {
                int res = Universal.ExecuteNonQuery("Update tbl_FlightRegistration set FlightNumber='" + flight.FlightNumber + "', FlightTypeId='" + flight.FlightTypeId +
    "',FlightTypeText='" + flight.FlightTypeText + "',OperatorName='" + flight.OperatorName + "',MaxCapacity='" +
    flight.MaxCapacity + "',BusinessClass='" + flight.BusinessClass + "',EconomicClass='" +
    flight.EconomicClass + "',HeadName='" + flight.HeadName + "',ContactNo='" + flight.ContactNo + "',EmailId='" + flight.EmailId + "',FuelCapacity='" + flight.FuelCapacity + "',Remarks='" + flight.Remarks + "' where FlightId=" + id);
                if (res == 1)
                {
                    message = "Update successfully!";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteFlight(int id)
        {
            int res = Universal.ExecuteNonQuery("delete from tbl_FlightRegistration where FlightId=" + id);
            return RedirectToAction("Flightlist", "SubAdmin");
        }



        //-----------------------------------------------------------------------------------------

        public ActionResult FlightPlanList()
        {
            if (Session["airportid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult ChangeFlightStatusToDepart(string planid)
        {
            int restl = Universal.ExecuteNonQuery("update tbl_flight_plan_master set Status='Depart' where PlanId='" + planid + "'");
            if (restl == 1)
            {
                Response.Write("<script>alert('Flight Status Changed To Depart ')</script>");
            }
            return RedirectToAction("FlightPlanList", "ATC");
        }
        public ActionResult FlightPlanListAPI()
        {
            List<DepartureFlightListModel> departurelist = new List<DepartureFlightListModel>();
            DataTable deplist = Universal.SelectWithDS("select * from tbl_flight_plan_master where SourceStationId='" + Session["airportid"].ToString() + "' And Status='Ready'", "tbl_flight_plan_master");
            for (int i = 0; i < deplist.Rows.Count; i++)
            {
                DepartureFlightListModel dpmodel = new Models.DepartureFlightListModel();
                dpmodel.PlanId = deplist.Rows[i]["PlanId"].ToString();
                dpmodel.FlightId = deplist.Rows[i]["FlightId"].ToString();
                dpmodel.FlightName = deplist.Rows[i]["FlightName"].ToString();
                dpmodel.DestinationName = deplist.Rows[i]["DestinationName"].ToString();
                dpmodel.DepartureDate = deplist.Rows[i]["Date_"].ToString().Replace("00:00:00", "");
                dpmodel.DepatrureTime = deplist.Rows[i]["ExpectedDepatrureTime"].ToString();
                dpmodel.ArrivalTime = deplist.Rows[i]["ExpectedArrivalTime"].ToString();
                dpmodel.Distance = deplist.Rows[i]["Distance"].ToString();
                dpmodel.Duration = deplist.Rows[i]["Duration"].ToString();
                dpmodel.Speed = deplist.Rows[i]["Speed"].ToString();
                dpmodel.Height = deplist.Rows[i]["Height"].ToString();
                dpmodel.No_of_Passanger = deplist.Rows[i]["No_of_Passanger"].ToString();
                dpmodel.No_of_staff = deplist.Rows[i]["No_of_staff"].ToString();
                dpmodel.Remark = deplist.Rows[i]["Remark"].ToString();
                dpmodel.Status = deplist.Rows[i]["Status"].ToString();
                departurelist.Add(dpmodel);
            }
            var details = departurelist;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DepartureFlightList()
        {
            if (Session["airportid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult DepartureFlightListAPI()
        {
            List<DepartureFlightListModel> departurelist = new List<DepartureFlightListModel>();
            DataTable deplist = Universal.SelectWithDS("select * from tbl_flight_plan_master where SourceStationId='" + Session["airportid"].ToString() + "' And Status='Depart'", "tbl_flight_plan_master");
            for (int i = 0; i < deplist.Rows.Count; i++)
            {
                DepartureFlightListModel dpmodel = new Models.DepartureFlightListModel();
                dpmodel.PlanId = deplist.Rows[i]["PlanId"].ToString();
                dpmodel.FlightId = deplist.Rows[i]["FlightId"].ToString();
                dpmodel.FlightName = deplist.Rows[i]["FlightName"].ToString();
                dpmodel.DestinationName = deplist.Rows[i]["DestinationName"].ToString();
                dpmodel.DepartureDate = deplist.Rows[i]["Date_"].ToString().Replace("00:00:00", "");
                dpmodel.DepatrureTime = deplist.Rows[i]["ExpectedDepatrureTime"].ToString();
                dpmodel.ArrivalTime = deplist.Rows[i]["ExpectedArrivalTime"].ToString();
                dpmodel.Distance = deplist.Rows[i]["Distance"].ToString();
                dpmodel.Duration = deplist.Rows[i]["Duration"].ToString();
                dpmodel.Speed = deplist.Rows[i]["Speed"].ToString();
                dpmodel.Height = deplist.Rows[i]["Height"].ToString();
                dpmodel.No_of_Passanger = deplist.Rows[i]["No_of_Passanger"].ToString();
                dpmodel.No_of_staff = deplist.Rows[i]["No_of_staff"].ToString();
                dpmodel.Remark = deplist.Rows[i]["Remark"].ToString();
                dpmodel.Status = deplist.Rows[i]["Status"].ToString();
                departurelist.Add(dpmodel);
            }
            var details = departurelist;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArrivalPlan()
        {
            if (Session["airportid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult GetLastArrivalFlightsApi()
        {
            List<ArrivalFlightModel> departurelist = new List<ArrivalFlightModel>();
            DataTable deplist = Universal.SelectWithDS("select * from tbl_flight_plan_master where DestinationId='" + Session["airportid"].ToString() + "' And Status='Depart'", "tbl_flight_plan_master");
            for (int i = 0; i < deplist.Rows.Count; i++)
            {
                ArrivalFlightModel dpmodel = new Models.ArrivalFlightModel();
                dpmodel.PlanId = deplist.Rows[i]["PlanId"].ToString();
                dpmodel.FlightId = deplist.Rows[i]["FlightId"].ToString();
                dpmodel.FlightName = deplist.Rows[i]["FlightName"].ToString();
                dpmodel.SourceStationId = deplist.Rows[i]["SourceStationId"].ToString();
                dpmodel.SourceStationName = deplist.Rows[i]["SourceStationName"].ToString();
                dpmodel.DepartureDate = deplist.Rows[i]["Date_"].ToString().Replace("00:00:00", "");
                dpmodel.DepatrureTime = deplist.Rows[i]["ExpectedDepatrureTime"].ToString();
                dpmodel.ArrivalTime = deplist.Rows[i]["ExpectedArrivalTime"].ToString();
                dpmodel.Distance = deplist.Rows[i]["Distance"].ToString();
                dpmodel.Duration = deplist.Rows[i]["Duration"].ToString();
                dpmodel.Speed = deplist.Rows[i]["Speed"].ToString();
                dpmodel.Height = deplist.Rows[i]["Height"].ToString();
                dpmodel.No_of_Passanger = deplist.Rows[i]["No_of_Passanger"].ToString();
                dpmodel.No_of_staff = deplist.Rows[i]["No_of_staff"].ToString();
                dpmodel.Remark = deplist.Rows[i]["Remark"].ToString();
                dpmodel.Status = deplist.Rows[i]["Status"].ToString();
                departurelist.Add(dpmodel);
            }
            var details = departurelist;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetRefArrivalPlan()
        {
            List<ArrivalFlightModel> departurelist = new List<ArrivalFlightModel>();

            DataTable dtrefplanid = Universal.SelectWithDS("select DISTINCT PlanId from ref_flight_plan_master where ATCID='" + Session["airportid"].ToString() + "'", "ref_flight_plan_master");
            for (int i = 0; i < dtrefplanid.Rows.Count; i++)
            {
                string PlanId = dtrefplanid.Rows[i]["PlanId"].ToString();
                DataTable deplist = Universal.SelectWithDS("select * from tbl_flight_plan_master where PlanId='" + PlanId + "' And Status='Depart'", "tbl_flight_plan_master");
                for (int j = 0; j < deplist.Rows.Count; j++)
                {
                    ArrivalFlightModel dpmodel = new Models.ArrivalFlightModel();
                    dpmodel.PlanId = deplist.Rows[j]["PlanId"].ToString();
                    dpmodel.FlightId = deplist.Rows[j]["FlightId"].ToString();
                    dpmodel.FlightName = deplist.Rows[j]["FlightName"].ToString();
                    dpmodel.SourceStationId = deplist.Rows[j]["SourceStationId"].ToString();
                    dpmodel.SourceStationName = deplist.Rows[j]["SourceStationName"].ToString();
                    dpmodel.DepartureDate = deplist.Rows[j]["Date_"].ToString().Replace("00:00:00", "");
                    dpmodel.DepatrureTime = deplist.Rows[j]["ExpectedDepatrureTime"].ToString();
                    dpmodel.ArrivalTime = deplist.Rows[j]["ExpectedArrivalTime"].ToString();
                    dpmodel.Distance = deplist.Rows[j]["Distance"].ToString();
                    dpmodel.Duration = deplist.Rows[j]["Duration"].ToString();
                    dpmodel.Speed = deplist.Rows[j]["Speed"].ToString();
                    dpmodel.Height = deplist.Rows[j]["Height"].ToString();
                    dpmodel.No_of_Passanger = deplist.Rows[j]["No_of_Passanger"].ToString();
                    dpmodel.No_of_staff = deplist.Rows[j]["No_of_staff"].ToString();
                    dpmodel.Remark = deplist.Rows[j]["Remark"].ToString();
                    dpmodel.Status = deplist.Rows[j]["Status"].ToString();
                    departurelist.Add(dpmodel);
                }
            }
            var details = departurelist;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }
    }
}