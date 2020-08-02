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
    public class ATCController : Controller
    {
        // GET: SubAdmin
        public ActionResult Index()
        {
            DataTable dt = Universal.SelectWithDS("select EquipmentName,AirportName,DATE_FORMAT(InstallationDate,'%d-%m-%Y') as InstallationDate,WarrantyPeriod,DATE_FORMAT(WarrantyExpirationDate,'%d-%m-%Y') as ExpirationDate ,Status,RelatedTo,MaintenanceDuration from tbl_equipmentregistration_tbl where AirportId='" + Session["airportid"].ToString() + "'", "tbl_equipmentregistration_tbl");
            ViewBag.eqpttbl = dt;

            //Get VHF RX Daily Data
            DataTable VHFRX_Daily = Universal.SelectWithDS("select DATE_FORMAT(Date_,'%d-%m-%Y') as MaintenanceDate,RXNo,Frq_MHZ,BitTest,Status_,RXNCheck,AC_DC_CO,SQ_Threshold,Remarks,MaintenanceOfficial,Status from tbl_vhf_rx_daily", "tbl_vhf_rx_daily");
            ViewBag.VHFRX_table = VHFRX_Daily;

            //Get VHF TX Daily Data
            DataTable VHFTX_Daily = Universal.SelectWithDS("select DATE_FORMAT(Date,'%d-%m-%Y') as MaintenanceDate,	TXNo,FreqMHz,BitTest,Status,ACPower,TXCheck,TxnCheck,AC_DC_CO,Remarks,MaintenanceOfficial,Status_ from tbl_vhf_tx_daily", "tbl_vhf_tx_daily");
            ViewBag.VHFTX_Table = VHFTX_Daily;
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
      
        public JsonResult EmployeeSetup(StaffModule emp)
        {
            string message = "";
            if (Session["airportid"] != null)
            {
                string aa = Session["airportid"].ToString();
                string bb = Session["airportname"].ToString();
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

      
        public ActionResult preventivechecksfordepart(string planid)
        {
            DataTable dt = Universal.SelectWithDS("select planid,FlightName,SourceStationName,DestinationName,DATE_FORMAT(Date_,'%d-%m-%Y') as Date_ from tbl_flight_plan_master where PlanId='" + planid + "'", "tbl_flight_plan_master");
            ViewBag.plandata = dt;
            return View();
        }

      
        public ActionResult EquipmentRegistration()
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

        [HttpPost]
        public ActionResult EquipmentRegistration(EquipmentRegistrationmodel eqpmodel)
        {
            if (eqpmodel != null)
            {
                string MaintainnaceDuration = "";
                if (eqpmodel.Daily != null)
                    MaintainnaceDuration += eqpmodel.Daily + ",";
                if (eqpmodel.Weakly != null)
                    MaintainnaceDuration += eqpmodel.Weakly + ",";
                if (eqpmodel.Monthly != null)
                    MaintainnaceDuration += eqpmodel.Monthly + ",";
                if (eqpmodel.SixMonthly != null)
                    MaintainnaceDuration += eqpmodel.SixMonthly + ",";
                if (eqpmodel.Yearly != null)
                    MaintainnaceDuration += eqpmodel.Yearly + ",";

                if (MaintainnaceDuration != "")
                    MaintainnaceDuration = MaintainnaceDuration.Remove(MaintainnaceDuration.Length - 1, 1);

                int i = Universal.ExecuteNonQuery("insert into tbl_equipmentregistration_tbl (EquipmentName,AirportId,AirportName,InstallationDate,WarrantyPeriod,WarrantyExpirationDate,Status,RelatedTo,MaintenanceDuration) values('" + eqpmodel.EquipmentName + "','" + Session["airportid"].ToString() + "','" + Session["airportname"].ToString() + "','" + eqpmodel.InstallationDate + "','" + eqpmodel.WarrantyPeriod + "','" + eqpmodel.WarrantyExpirationDate + "','Active','" + eqpmodel.RelatedTo + "','"+ MaintainnaceDuration + "')");
                if(i==1)
                {
                    Response.Write("<script>alert('!! Equipment Saved Successfully !!')</script>");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult EquipmentList()
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

        public ActionResult ShowEquipmentByDuration(string duration)
        {
            DataTable dt = Universal.SelectWithDS("select EquipmentName,WarrantyPeriod,DATE_FORMAT(WarrantyExpirationDate,'%d-%m-%Y') as WarrantyExpirationDate from tbl_equipmentregistration_tbl where MaintenanceDuration like'%" + duration + "%' And AirportId='"+ Session["airportid"].ToString() + "'", "tbl_equipmentregistration_tbl");
            ViewBag.equipmentlist = dt;
            ViewBag.durationtype = duration;
            return View();
        }
        public ActionResult equipmentHistory(string eqpname, string duration)
        {
            if (eqpname == "VHF RX" && duration == "Daily")
            {
                return RedirectToAction("eqpmnt_VHF_RX_Daily_History", "ATC");
            }
            else if (eqpname == "VHF RX" && duration == "Monthly")
            {
                return RedirectToAction("VHF_RX_Monthly_History", "ATC");
            }
            else if (eqpname == "VHF RX" && duration == "Yearly")
            {
                return RedirectToAction("eqpmnt_VHF_RX_Yearly_History", "ATC");
            }
            else if (eqpname == "VHF TX" && duration == "Daily")
            {
                return RedirectToAction("eqpmnt_VHF_TX_Daily_History", "ATC");
            }
            else if (eqpname == "VHF TX" && duration == "Monthly")
            {
                return RedirectToAction("eqpmnt_VHF_TX_Monthly_History", "ATC");
            }
            else if (eqpname == "VHF TX" && duration == "Yearly")
            {
                return RedirectToAction("VHF_TX_Yearly_History", "ATC");
            }
            else
            {
                return View();
            }
        }


        public ActionResult eqpmnt_VHF_TX_Daily_History()
        {
            return View();
        }
        public ActionResult VHF_Daily_History()
        {
            List<VHF_TX_Daily> history = new List<Models.VHF_TX_Daily>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_tx_daily order by Date DESC", "tbl_vhf_tx_daily");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VHF_TX_Daily obj = new VHF_TX_Daily();
                obj.ID = dt.Rows[i]["ID"].ToString();
                obj.Dated = dt.Rows[i]["Date"].ToString().Replace("00:00:00", ""); ;
                obj.TXNo = dt.Rows[i]["TXNo"].ToString();
                obj.FreqMHz = dt.Rows[i]["FreqMHz"].ToString();
                obj.BitTest = dt.Rows[i]["BitTest"].ToString();
                obj.Status = dt.Rows[i]["Status"].ToString();
                obj.ACPower = dt.Rows[i]["ACPower"].ToString();
                obj.TXCheck = dt.Rows[i]["TXCheck"].ToString();
                obj.TxnCheck = dt.Rows[i]["TxnCheck"].ToString();
                obj.Remarks = dt.Rows[i]["Remarks"].ToString();
                obj.MaintenanceOfficial = dt.Rows[i]["MaintenanceOfficial"].ToString();
                obj.Status_ = dt.Rows[i]["Status_"].ToString();
                history.Add(obj);
            }
            var details = history;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eqpmnt_VHF_TX_Monthly_History()
        {
            return View();
        }
        public ActionResult VHF_TX_Monthly_History()
        {
            //																				
            List<VHF_TX_Monthly> history = new List<Models.VHF_TX_Monthly>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_tx_monthly order by Date_ DESC", "tbl_vhf_tx_daily");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VHF_TX_Monthly obj = new VHF_TX_Monthly();
                obj.Date_ = dt.Rows[i]["Date_"].ToString().Replace("00:00:00", "");
                obj.Month_ = dt.Rows[i]["Month_"].ToString();
                obj.TxNo = dt.Rows[i]["TxNo"].ToString();
                obj.FreqMHz = dt.Rows[i]["FreqMHz"].ToString();
                obj.Power_O_P_FWD_W = dt.Rows[i]["Power_O_P_FWD_W"].ToString();
                obj.SET_LINE_I_P_dBm = dt.Rows[i]["SET_LINE_I_P_dBm"].ToString();
                obj.SET_INHIBIT = dt.Rows[i]["SET_INHIBIT"].ToString();
                obj.SET_TIMEOUT_sec = dt.Rows[i]["SET_TIMEOUT_sec"].ToString();
                obj.SET_MODN_DEPTH = dt.Rows[i]["SET_MODN_DEPTH"].ToString();
                obj.Remarks = dt.Rows[i]["Remarks"].ToString();
                obj.SET_VOGAD = dt.Rows[i]["SET_VOGAD"].ToString();
                obj.PTT_I_P = dt.Rows[i]["PTT_I_P"].ToString();
                obj.PHANTOM_PTT_I_P = dt.Rows[i]["PHANTOM_PTT_I_P"].ToString();
                obj.PTT_REF_VOLTAGE_V = dt.Rows[i]["PTT_REF_VOLTAGE_V"].ToString();
                obj.PTT_O_P = dt.Rows[i]["PTT_O_P"].ToString();
                obj.Bit_Test = dt.Rows[i]["Bit_Test"].ToString();
                obj.Remarks = dt.Rows[i]["Remarks"].ToString();
                obj.Maintenance_Official = dt.Rows[i]["Maintenance_Official"].ToString();
                obj.Next_Maintenance_Date= dt.Rows[i]["Next_Maintenance_Date"].ToString();
                history.Add(obj);
            }
            var details = history;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        //Check Daily Maintainnance For Equipment
        public JsonResult checkdailyMaintainVHFTX()
        {
            string message = "No Data";
            DataTable dtx = Universal.SelectWithDS("select * from tbl_equipmentregistration_tbl where MaintenanceDuration like '%Daily%' And EquipmentName='VHF TX'", "tbl_equipmentregistration_tbl");
            if (dtx.Rows.Count > 0)
            {
                DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_tx_daily where Date='" + Universal.GetDate + "'", "tbl_vhf_tx_daily");
                if (dt.Rows.Count == 0)
                {
                    message = "VHF TX";
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult checkdailyMaintainVHFRX()
        {
            string message = "No Data";
            DataTable dtx = Universal.SelectWithDS("select * from tbl_equipmentregistration_tbl where MaintenanceDuration like '%Daily%' And EquipmentName='VHF RX'", "tbl_equipmentregistration_tbl");
            if (dtx.Rows.Count > 0)
            {
                DataTable dt1 = Universal.SelectWithDS("select * from tbl_vhf_rx_daily where Date_='" + Universal.GetDate + "'", "tbl_vhf_tx_daily");
                if (dt1.Rows.Count == 0)
                {
                    message = "VHF RX";
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult checkmonthlyMaintainVHFTX()
        {
            string message = "No Data";
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_tx_monthly where Next_Maintenance_Date='" + Universal.GetDate + "'", "tbl_vhf_tx_daily");
            if (dt.Rows.Count == 1)
            {
                string date = dt.Rows[0][1].ToString().Replace("00:00:00", "");
                   message = "VHF TX Last Checked "+ date;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkmonthlyMaintainVHFRX()
        {
            string message = "No Data";
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_rx_monthly where Nextdate='" + Universal.GetDate + "'", "tbl_vhf_tx_daily");
            if (dt.Rows.Count ==1)
            {
                string date = dt.Rows[0][1].ToString().Replace("00:00:00","");
                message = "VHF RX Last Checked " + date;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        


        public ActionResult eqpmnt_VHF_TX_Yearly_History()
        {
            //																				
            List<VHF_TX_Yearly> history = new List<Models.VHF_TX_Yearly>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_tx_yearly order by Date_ DESC", "tbl_vhf_tx_daily");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VHF_TX_Yearly obj = new VHF_TX_Yearly();
                obj.Date_ = dt.Rows[i]["Date_"].ToString().Replace("00:00:00", ""); ;
                obj.Year = dt.Rows[i]["Year"].ToString();
                obj.TXNo = dt.Rows[i]["TXNo"].ToString();
                obj.FreqMHz = dt.Rows[i]["FreqMHz"].ToString();
                obj.ReferenceFreq_MHz = dt.Rows[i]["ReferenceFreq_MHz"].ToString();
                obj.Power_W = dt.Rows[i]["Power_W"].ToString();
                obj.BitTest = dt.Rows[i]["BitTest"].ToString();
                obj.AC_DC_Change_Over = dt.Rows[i]["AC_DC_Change_Over"].ToString();
                obj.Remarks = dt.Rows[i]["Remarks"].ToString();
                obj.MaintenanceOfficial = dt.Rows[i]["MaintenanceOfficial"].ToString();
                obj.NextMaintenanceDate = dt.Rows[i]["NextMaintenanceDate"].ToString();
                history.Add(obj);
            }
            var details = history;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VHF_TX_Yearly_History()
        {
             return View();
        }

        public ActionResult eqpmnt_VHF_RX_Daily_History()
        {
            return View();
        }
        public ActionResult VHF_RX_Daily_History()
        {
            //																				
            List<VHF_RX_Daily> history = new List<Models.VHF_RX_Daily>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_rx_daily order by Date_ DESC", "tbl_vhf_tx_daily");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VHF_RX_Daily obj = new VHF_RX_Daily();
                obj.Date_ = dt.Rows[i]["Date_"].ToString().Replace("00:00:00", ""); ;
                obj.RXNo = dt.Rows[i]["RXNo"].ToString();
                obj.Frq_MHZ = dt.Rows[i]["Frq_MHZ"].ToString();
                obj.BitTest = dt.Rows[i]["BitTest"].ToString();
                obj.Status_ = dt.Rows[i]["Status_"].ToString();
                obj.RXNCheck = dt.Rows[i]["RXNCheck"].ToString();
                obj.BitTest = dt.Rows[i]["BitTest"].ToString();
                obj.AC_DC_CO = dt.Rows[i]["AC_DC_CO"].ToString();
                obj.SQ_Threshold = dt.Rows[i]["SQ_Threshold"].ToString();
                obj.Remarks = dt.Rows[i]["Remarks"].ToString();
                obj.MaintenanceOfficial = dt.Rows[i]["MaintenanceOfficial"].ToString();
                history.Add(obj);
            }
            var details = history;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eqpmnt_VHF_RX_Monthly_History()
        {
            List<VHF_RX_Monthly> history = new List<Models.VHF_RX_Monthly>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_rx_monthly order by Date_ DESC", "tbl_vhf_tx_daily");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VHF_RX_Monthly obj = new VHF_RX_Monthly();
                obj.Date_ = dt.Rows[i]["Date_"].ToString().Replace("00:00:00", ""); ;
                obj.Month_ = dt.Rows[i]["Month_"].ToString();
                obj.RXNo = dt.Rows[i]["RXNo"].ToString();
                obj.FreqMHz = dt.Rows[i]["FreqMHz"].ToString();
                obj.LineOP = dt.Rows[i]["LineOP"].ToString();
                obj.Threshold = dt.Rows[i]["Threshold"].ToString();
                obj.Modulation = dt.Rows[i]["Modulation"].ToString();
                obj.Defeat = dt.Rows[i]["Defeat"].ToString();
                obj.DefeatIP = dt.Rows[i]["DefeatIP"].ToString();
                obj.Facility = dt.Rows[i]["Facility"].ToString();
                obj.Nextdate = dt.Rows[i]["Nextdate"].ToString();
                history.Add(obj);
            }
            var details = history;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VHF_RX_Monthly_History()
        {
            return View();
        }


        public ActionResult eqpmnt_VHF_RX_Yearly_History()
        {
            return View();
        }
        public ActionResult VHF_RX_Yearly_History()
        {
            List<VHF_RX_Yearly> history = new List<Models.VHF_RX_Yearly>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_vhf_rx_yearly order by Date_ DESC", "tbl_vhf_tx_daily");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VHF_RX_Yearly obj = new VHF_RX_Yearly();
                obj.Date_ = dt.Rows[i]["Date_"].ToString().Replace("00:00:00","");
                obj.Year = dt.Rows[i]["Year"].ToString();
                obj.RXNo = dt.Rows[i]["RXNo"].ToString();
                obj.FreqMHz = dt.Rows[i]["FreqMHz"].ToString();
                obj.BitTest = dt.Rows[i]["BitTest"].ToString();
                obj.Sensitivity = dt.Rows[i]["Sensitivity"].ToString();
                obj.ACDC = dt.Rows[i]["ACDC"].ToString();
                obj.Reffreq = dt.Rows[i]["Reffreq"].ToString();
                obj.Remarks = dt.Rows[i]["Remarks"].ToString();
                obj.MaintenanceOfficial = dt.Rows[i]["MaintenanceOfficial"].ToString();
                obj.Nextdate = dt.Rows[i]["Nextdate"].ToString();
                history.Add(obj);
            }
            var details = history;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult equipmentmaintainnance(string eqpname,string duration)
        {
            if (eqpname == "VHF RX" && duration=="Daily")
            {
                return RedirectToAction("eqpmnt_VHF_RX_Daily", "ATC");
            }
            else if(eqpname == "VHF RX" && duration == "Monthly")
            {
                return RedirectToAction("eqpmnt_VHF_RX_Monthly", "ATC");
            }
            else if(eqpname == "VHF RX" && duration == "Yearly")
            {
                return RedirectToAction("eqpmnt_VHF_RX_Yearly", "ATC");
            }
            else if(eqpname == "VHF TX" && duration == "Daily")
            {
                return RedirectToAction("eqpmnt_VHF_TX_Daily", "ATC");
            }
            else if (eqpname == "VHF TX" && duration == "Monthly")
            {
                return RedirectToAction("eqpmnt_VHF_TX_Monthly", "ATC");
            }
            else if (eqpname == "VHF TX" && duration == "Yearly")
            {
                return RedirectToAction("eqpmnt_VHF_TX_Yearly", "ATC");
            }
            else if (eqpname == "MSSR" && duration == "Daily")
            {
                return RedirectToAction("eqpmnt_MSSR_Daily", "ATC");
            }
            else if (eqpname == "MSSR" && duration == "Monthly")
            {
                return RedirectToAction("eqpmnt_MSSR_Monthly", "ATC");
            }
            else if (eqpname == "MSSR" && duration == "SixMonthly")
            {
                return RedirectToAction("eqpmnt_MSSR_SixMonthly", "ATC");
            }
            else if (eqpname == "MSSR" && duration == "Yearly")
            {
                return RedirectToAction("eqpmnt_MSSR_Yearly", "ATC");
            }
            else
            {
                return View();
            }
        }

        //**********************
        //VHF_TX Daily

        public ActionResult eqpmnt_VHF_TX_Daily()
        {
            return View();
        }
        [HttpPost]
        public JsonResult eqpmnt_VHF_TX_Daily(VHF_TX_Daily modelvhfDaily)
        {
            string message = "";
            if(modelvhfDaily!=null)
            {
                int i = Universal.ExecuteNonQuery("insert into tbl_vhf_tx_daily (Date,TXNo,FreqMHz,BitTest,Status,ACPower,TXCheck,TxnCheck,AC_DC_CO,Remarks,MaintenanceOfficial,Status_) values('" + modelvhfDaily.Dated + "','" + modelvhfDaily.TXNo + "','" + modelvhfDaily.FreqMHz + "','" + modelvhfDaily.BitTest + "','" + modelvhfDaily.Status + "','" + modelvhfDaily.ACPower + "','" + modelvhfDaily.TXCheck + "','" + modelvhfDaily.TxnCheck + "','"+ modelvhfDaily .AC_DC_CO+ "','"+modelvhfDaily.Remarks+"','" + modelvhfDaily.MaintenanceOfficial + "','Active')");
                if(i==1)
                {
                    message = "VHF TX Daily Maintenance Details Saved Successfully!!";
                }
                else
                {
                    message = "Error In Server";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //VHF_TX_Monthly
        public ActionResult eqpmnt_VHF_TX_Monthly()
        {
            return View();
        }

        [HttpPost]
        public JsonResult eqpmnt_VHF_TX_Monthly(VHF_TX_Monthly model)
        {
            string message = "";
            if (model != null)
            {
                int i = Universal.ExecuteNonQuery("insert into tbl_vhf_tx_monthly (Date_,Month_,TxNo,FreqMHz,Power_O_P_FWD_W,SET_LINE_I_P_dBm,SET_INHIBIT,SET_TIMEOUT_sec,SET_MODN_DEPTH,SET_VOGAD,SET_KEY_PRIORITY,Ready_Signal,PTT_I_P,PHANTOM_PTT_I_P,PTT_REF_VOLTAGE_V,PTT_O_P,Bit_Test,Remarks,Maintenance_Official,Next_Maintenance_Date,Status) values('"+model.Date_+"','"+ model.Month_ +"','"+ model.TxNo +"','"+ model.FreqMHz +"','"+ model.Power_O_P_FWD_W +"','"+ model.SET_LINE_I_P_dBm +"','"+ model.SET_INHIBIT +"','"+ model.SET_TIMEOUT_sec +"','"+ model.SET_MODN_DEPTH+"','"+ model.SET_VOGAD+"','"+ model.SET_KEY_PRIORITY +"','"+ model.Ready_Signal +"','"+ model.PTT_I_P +"','"+ model.PHANTOM_PTT_I_P +"','"+ model.PTT_REF_VOLTAGE_V +"','"+ model.PTT_O_P +"','"+ model.Bit_Test +"','"+ model.Remarks+"','"+ model.Maintenance_Official +"','"+ model.Next_Maintenance_Date+"','Active')");
                if (i == 1)
                {
                    message = "VHF TX Monthly Maintenance Details Saved Successfully!!";
                }
                else
                {
                    message = "Error In Server";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //VHF_TX_Yearly
        public ActionResult eqpmnt_VHF_TX_Yearly()
        {
            return View();
        }

        [HttpPost]
        public JsonResult eqpmnt_VHF_TX_Yearly(VHF_TX_Yearly model)
        {
            string message = "";
            if (model != null)
            {
                int i = Universal.ExecuteNonQuery("insert into tbl_vhf_tx_yearly (Date_,Year,TXNo,FreqMHz,ReferenceFreq_MHz,Power_W,BitTest,AC_DC_Change_Over,Remarks,MaintenanceOfficial,NextMaintenanceDate,Status) values('" + model.Date_ + "','" + model.Year + "','" + model.TXNo + "','" + model.FreqMHz + "','" + model.ReferenceFreq_MHz + "','" + model.Power_W + "','" + model.BitTest + "','" + model.AC_DC_Change_Over + "','" + model.Remarks + "','" + model.MaintenanceOfficial + "','" + model.NextMaintenanceDate + "','Active')");
                if (i == 1)
                {
                    message = "VHF TX Yearly Maintenance Details Saved Successfully!!";
                }
                else
                {
                    message = "Error In Server";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //VHF_RX DAILY
        public ActionResult eqpmnt_VHF_RX_Daily()
        {
            return View();
        }
        [HttpPost]
        public JsonResult eqpmnt_VHF_RX_Daily(VHF_RX_Daily model)
        {
            string message = "";
            if (model != null)
            {
                int i = Universal.ExecuteNonQuery("insert into tbl_vhf_rx_daily (Date_,RXNo,Frq_MHZ,BitTest,Status_,RXNCheck,AC_DC_CO,SQ_Threshold,Remarks,MaintenanceOfficial,Status) values ('" + model.Date_+"', '"+ model.RXNo+"','"+ model.Frq_MHZ +"','"+ model.BitTest+"','"+ model.Status_+"','"+ model.RXNCheck +"','"+ model.AC_DC_CO +"','"+ model.SQ_Threshold+"','"+ model.Remarks+"','"+ model.MaintenanceOfficial +"','Active') ");
                if (i == 1)
                {
                    message = "VHF RX Daily Maintenance Details Saved Successfully!!";
                }
                else
                {
                    message = "Error In Server";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //VHF_RX Monthly
        public ActionResult eqpmnt_VHF_RX_Monthly()
        {
            return View();
        }
        [HttpPost]
        public JsonResult eqpmnt_VHF_RX_Monthly(VHF_RX_Monthly model)
        {
            string message = "";
            if (model != null)
            {
                int i = Universal.ExecuteNonQuery("insert into tbl_vhf_rx_monthly (Date_,Month_,RXNo,FreqMHz,LineOP,Threshold,Modulation,Defeat,DefeatIP,Carrier,AGC,Marc,Facility,Phantom,Ready,BitTest,Remarks,MaintenanceOfficial,Nextdate,Status) values ('" + model.Date_ + "','" + model.Month_ + "','" + model.RXNo + "','" + model.FreqMHz + "','" + model.LineOP + "','" + model.Threshold + "','" + model.Modulation + "','" + model.Defeat + "','" + model.DefeatIP + "','" + model.Carrier + "','" + model.AGC + "','" + model.Marc + "','" + model.Facility + "','" + model.Phantom + "','" + model.Ready + "','" + model.BitTest + "','" + model.Remarks + "','" + model.MaintenanceOfficial + "','" + model.Nextdate + "','Active') ");
                if (i == 1)
                {
                    message = "VHF RX Monthly Maintenance Details Saved Successfully!!";
                }
                else
                {
                    message = "Error In Server";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        //VHF_RX Yearly
        public ActionResult eqpmnt_VHF_RX_Yearly()
        {
            return View();
        }

        [HttpPost]
        public JsonResult eqpmnt_VHF_RX_Yearly(VHF_RX_Yearly model)
        {
            string message = "";
            if (model != null)
            {
                int i = Universal.ExecuteNonQuery("insert into tbl_vhf_rx_yearly (Date_,Year,RXNo,FreqMHz,BitTest,Sensitivity,ACDC,Reffreq,Remarks,MaintenanceOfficial,Nextdate,Status) values ('" + model.Date_ + "','" + model.Year + "','" + model.RXNo + "','" + model.FreqMHz + "','" + model.BitTest + "','" + model.Sensitivity + "','" + model.ACDC + "','" + model.Reffreq + "','" + model.Remarks + "','" + model.MaintenanceOfficial + "','" + model.Nextdate + "','Active') ");
                if (i == 1)
                {
                    message = "VHF RX Yearly Maintenance Details Saved Successfully!!";
                }
                else
                {
                    message = "Error In Server";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        //**********************
        public JsonResult EquipmentListAPI()
        {
            List<EquipmentRegistrationmodel> eqp = new List<EquipmentRegistrationmodel>();
            DataTable dt = Universal.SelectWithDS("select EquipmentName,AirportName,DATE_FORMAT(InstallationDate,'%d-%m-%y') as InstallationDate,WarrantyPeriod,DATE_FORMAT(WarrantyExpirationDate,'%d-%m-%Y') as ExpirationDate ,Status,RelatedTo,MaintenanceDuration from tbl_equipmentregistration_tbl where AirportId='" + Session["airportid"].ToString() + "'", "tbl_equipmentregistration_tbl");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                EquipmentRegistrationmodel ewpp = new EquipmentRegistrationmodel();
                ewpp.EquipmentName = dt.Rows[i]["EquipmentName"].ToString();
                ewpp.AirportName = dt.Rows[i]["AirportName"].ToString();
                ewpp.InstallationDate = dt.Rows[i]["InstallationDate"].ToString();
                ewpp.WarrantyPeriod = dt.Rows[i]["WarrantyPeriod"].ToString();
                ewpp.WarrantyExpirationDate = dt.Rows[i]["ExpirationDate"].ToString();
                ewpp.RelatedTo = dt.Rows[i]["RelatedTo"].ToString();
                ewpp.MaintainnanceDuration= dt.Rows[i]["MaintenanceDuration"].ToString();

                eqp.Add(ewpp);
            }
            var details = eqp;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }



        public string savepreventivechecksfordepart(string planid)
        {
            if(planid!=null)
            {
                int rest = Universal.ExecuteNonQuery("update tbl_flight_plan_master set CheckStatus='Done' where planid='" + planid + "'");
                if(rest==1)
                {
                    return "Preventive Check Done! Now Flight is Ready For Departure";
                }
                else
                {
                    return "Internal Server Error";
                }
            }
            else
            {
                return "Internal Server Error";
            }
        }

       
        public JsonResult SaveFlightPlanByFIR(FlightPlanModel flightmodel)
        {
            string message = "";
            if (Session["airportid"] != null)
            {
                string aa = Session["airportid"].ToString();
                string bb = Session["airportname"].ToString();

                string cmdText = "(" + flightmodel.FlightId + ",'" + flightmodel.FlightName + "','" + aa + "','" + bb + "','" + aa + "','" +
                    bb + "','" + flightmodel.DestinationId + "','" + flightmodel.DestinationName + "','" + flightmodel.Date_ + "','" +
                    flightmodel.ExpectedDepatrureTime + "','" + flightmodel.ExpectedArrivalTime + "','" + flightmodel.Distance + "','" + flightmodel.Duration + "','" + flightmodel.Speed + "','" + flightmodel.Height + "','" + flightmodel.No_of_Passanger + "','" + flightmodel.No_of_staff + "','" + flightmodel.Remark + "','Ready','" + flightmodel.PilotID + "','" + flightmodel.PilotName + "','" + flightmodel.CaptainID + "','" + flightmodel.CaptainName + "','" + flightmodel.CabinCrewId + "','" + flightmodel.CabinCrewName + "','Boarding','Pending')";

                int result = Universal.ExecuteNonQuery("insert into tbl_flight_plan_master (FlightId,FlightName,FIRID,FIRName,SourceStationId,SourceStationName,DestinationId,DestinationName,Date_,ExpectedDepatrureTime,ExpectedArrivalTime,Distance,Duration,Speed,Height,No_of_Passanger,No_of_staff,Remark,Status,PilotId,PilotName,CaptainID,CaptainName,CabinCrewId,CabinCrewName,CurrentStatus,CheckStatus) values" + cmdText);
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
                                cmd += "('" + planid + "','" + atcid + "','" + atcname + "','N/A'),";
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





        //------------------------------------------------------------------------------------------------------------

        public ActionResult preventivechecks()
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
        public ActionResult FlightPlanListAPIpending()
        {
            List<DepartureFlightListModel> departurelist = new List<DepartureFlightListModel>();
            DataTable deplist = Universal.SelectWithDS("select * from tbl_flight_plan_master where SourceStationId='" + Session["airportid"].ToString() + "' And Status='Ready' And CheckStatus='Pending'", "tbl_flight_plan_master");
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
            int restl = Universal.ExecuteNonQuery("update tbl_flight_plan_master set Status='Depart',CurrentStatus='Depart' where PlanId='" + planid + "'");

            if (restl == 1)
            {
                Universal.ExecuteNonQuery("update ref_flight_plan_master set Status='Depart' where PlanId='" + planid + "' ");
                Response.Write("<script>alert('Flight Status Changed To Depart ')</script>");
            }
            return RedirectToAction("FlightPlanList", "ATC");
        }
        public ActionResult FlightPlanListAPI()
        {
            List<DepartureFlightListModel> departurelist = new List<DepartureFlightListModel>();
            DataTable deplist = Universal.SelectWithDS("select * from tbl_flight_plan_master where SourceStationId='" + Session["airportid"].ToString() + "' And Status='Ready' And CheckStatus='DONE'", "tbl_flight_plan_master");
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

        [Route("Departure/Flights/Status/Update")]
        public ActionResult DepartureFlightListStatusUpdate()
        {
            DataTable dtdirectPlen = Universal.SelectWithDS("select PlanId,FlightName,Date_,PilotName,DestinationId,DestinationName,CurrentStatus from tbl_flight_plan_master where Status='Depart' And SourceStationId='" + Session["airportid"].ToString() + "' ", "tbl_flight_plan_master");
            ViewBag.DirectPlanForDepart = dtdirectPlen;

            DataTable dtIndirectPlenInRoute = Universal.SelectWithDS("select tbl_flight_plan_master.PlanId,tbl_flight_plan_master.FlightName,tbl_flight_plan_master.Date_,tbl_flight_plan_master.PilotName,ref_flight_plan_master.ATCID,ref_flight_plan_master.ATCName,ref_flight_plan_master.Status from tbl_flight_plan_master INNER JOIN ref_flight_plan_master ON tbl_flight_plan_master.PlanId=ref_flight_plan_master.PlanId where tbl_flight_plan_master.Status='Depart' And tbl_flight_plan_master.SourceStationId='" + Session["airportid"].ToString() + "' ", "tbl_flight_plan_master");
            ViewBag.InDirectPlanForDepart = dtIndirectPlenInRoute;

            return View();
        }

   
        public void DepartureFlightListStatusUpdateDirect(string Planid,string Status)
        {
            try
            {
                int i = Universal.ExecuteNonQuery("update tbl_flight_plan_master set CurrentStatus='" + Status + "' where PlanId='" + Planid + "'");
                if (i == 1)
                {
                    Universal.ExecuteNonQuery("update ref_flight_plan_master set Status='" + Status + "' where PlanId='" + Planid + "' ");
                    Response.Write("<script>alert('Status Changed Successfully')</script>");
                    Response.Write("<script>location.href='/ATC/DepartureFlightListStatusUpdate'</script>");
                }
            }
            catch (Exception ex) { }
        }

        public void DepartureFlightListStatusUpdateINDirect(string Planid, string AtcId, string Status)
        {
            try
            {
                int i = Universal.ExecuteNonQuery("update ref_flight_plan_master set Status='" + Status + "' where PlanId='" + Planid + "' And ATCID='" + AtcId + "'");
                if (i == 1)
                {
                    Response.Write("<script>alert('Status Changed Successfully')</script>");
                    Response.Write("<script>location.href='/ATC/DepartureFlightListStatusUpdate'</script>");
                }
            }
            catch (Exception ex) { }
          
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
                dpmodel.CurrentStatus = deplist.Rows[i]["CurrentStatus"].ToString();
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
                dpmodel.CurrentStatus= deplist.Rows[i]["CurrentStatus"].ToString();
                departurelist.Add(dpmodel);
            }
            var details = departurelist;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetRefArrivalPlan()
        {
            List<ArrivalFlightModel> departurelist = new List<ArrivalFlightModel>();

            DataTable dtrefplanid = Universal.SelectWithDS("select PlanId,Status from ref_flight_plan_master where ATCID='" + Session["airportid"].ToString() + "'", "ref_flight_plan_master");
            for (int i = 0; i < dtrefplanid.Rows.Count; i++)
            {
                string PlanId = dtrefplanid.Rows[i]["PlanId"].ToString();
                string status=dtrefplanid.Rows[i]["Status"].ToString();
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
                    dpmodel.CurrentStatus = status;
                    departurelist.Add(dpmodel);
                }
            }
            var details = departurelist;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArrivalFlightStatusUpdate()
        {
            if (Session["airportid"] != null)
            {
                GetLastArrivalFlightsApiForStatus();
                GetRefArrivalPlanForStatus();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        public void GetLastArrivalFlightsApiForStatus()
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
                dpmodel.CurrentStatus = deplist.Rows[i]["CurrentStatus"].ToString();
                departurelist.Add(dpmodel);
            }
            ViewBag.DataforDirectArrivalStatus = departurelist;
        }

        public void GetRefArrivalPlanForStatus()
        {
            List<ArrivalFlightModel> departurelist = new List<ArrivalFlightModel>();

            DataTable dtrefplanid = Universal.SelectWithDS("select PlanId,Status from ref_flight_plan_master where ATCID='" + Session["airportid"].ToString() + "'", "ref_flight_plan_master");
            for (int i = 0; i < dtrefplanid.Rows.Count; i++)
            {
                string PlanId = dtrefplanid.Rows[i]["PlanId"].ToString();
                string status = dtrefplanid.Rows[i]["Status"].ToString();
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
                    dpmodel.CurrentStatus = status;
                    departurelist.Add(dpmodel);
                }
            }
            ViewBag.indirectDataforStatus = departurelist;
        }

        public void ArrivalFlightListStatusUpdateDirect(string Planid, string status)
        {
            try
            {
                int result = Universal.ExecuteNonQuery("update tbl_flight_plan_master set CurrentStatus='" + status + "',ExpectedArrivalTime='" + Universal.GetTime + "' where PlanId='" + Planid + "'");
                if(result==1)
                {
                    Response.Write("<script>alert('Status Changed Successfully')</script>");
                    Response.Write("<script>location.href='/ATC/ArrivalFlightStatusUpdate'</script>");
                }
            }
            catch (Exception ex) { }
        }
        public void ArrivalFlightListStatusUpdateINDirect(string Planid, string ATCId,string status)
        {
            try
            {
                try
                {
                    int result = Universal.ExecuteNonQuery("update ref_flight_plan_master set Status='" + status + "' where PlanId='" + Planid + "' And ATCID='"+ ATCId + "'");
                    if (result == 1)
                    {
                        Response.Write("<script>alert('Status Changed Successfully')</script>");
                        Response.Write("<script>location.href='/ATC/ArrivalFlightStatusUpdate'</script>");
                    }
                }
                catch (Exception ex) { }
            }
            catch (Exception ex) { }
        }

        ///Change Flight Arrival Status Status
        public ActionResult ChangeFlightArrivalStatus()
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

    }
}
