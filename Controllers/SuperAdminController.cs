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
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult State()
        {
            return View();
        }

        public int GetMaxStateId()
        {
            int maxid = 0;
            string id = Universal.ExecuteScalar("select max(StateID) from tbl_statemaster").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt32(id);

            }
            maxid = maxid + 1;
            return maxid;
        }

        public JsonResult SaveState(string statename)
        {
            string message = "";
            int id = GetMaxStateId();
            int res= Universal.ExecuteNonQuery("insert into tbl_states (id,name,country_id) values(" + id + ",'" + statename + "'"+101+")");
            if (res == 1)
            {
                message = "State with " + id + " is successfully saved!";
            }
            else
            {
                message = "Server Error! Please contact admin...";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckStatename(string statename)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select name from tbl_states where name='" + statename + "'");
            if (nm != null)
            {
                message = "State name is already exists!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public List<SelectListItem> GetStatelist()
        {
            List<SelectListItem> subjectlist = new List<SelectListItem>();

            List<StateModule> states = new List<StateModule>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_states", "tbl_states");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StateModule cmd = new StateModule();
                cmd.stateId = Convert.ToInt32(dt.Rows[i]["id"]);
                cmd.stateName = dt.Rows[i]["name"].ToString();
                states.Add(cmd);
            }
            //var details = subjects;
            foreach (StateModule info in states)
            {
                SelectListItem crslist = new SelectListItem
                {
                    Text = info.stateName,
                    Value = info.stateId.ToString()
                };
                subjectlist.Add(crslist);
            }
            return subjectlist;
        }

        public JsonResult States()
        {
            var statelist = GetStatelist();
            return Json(statelist, JsonRequestBehavior.AllowGet);
        }
        public List<SelectListItem> GetCitylists(int id)
        {
            List<SelectListItem> citylist = new List<SelectListItem>();

            List<CityModule> cities = new List<CityModule>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_cities where state_id=" + id, "tbl_cities");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CityModule cmd = new CityModule();
                cmd.cityId = Convert.ToInt32(dt.Rows[i]["id"]);
                cmd.cityName = dt.Rows[i]["name"].ToString();
                cities.Add(cmd);
            }
            //var details = subjects;
            foreach (CityModule info in cities)
            {
                SelectListItem crslist = new SelectListItem
                {
                    Text = info.cityName,
                    Value = info.cityId.ToString()
                };
                citylist.Add(crslist);
            }
            return citylist;
        }

        public JsonResult Cities(string sid)
        {
            var citylist = GetCitylists(Convert.ToInt32(sid));
            return Json(citylist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult City()
        {
            return View();
        }
        public int GetMaxCityId()
        {
            int maxid = 0;
            string id = Universal.ExecuteScalar("select max(id) from tbl_cities").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt32(id);

            }
            maxid = maxid + 1;
            return maxid;
        }
        public JsonResult SaveCity(string cityname,string stateid)
        {
            string message = "";
            int id = GetMaxCityId();
            int res = Universal.ExecuteNonQuery("insert into tbl_cities (id,name,state_id) values(" + id + ",'" + cityname + "',"+Convert.ToInt32(stateid)+")");
            if (res == 1)
            {
                message = "City with " + id + " is successfully saved!";
            }
            else
            {
                message = "Server Error! Please contact admin...";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckCityname(string cityname)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select name from tbl_cities where name='" + cityname + "'");
            if (nm != null)
            {
                message = "City name is already exists!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public string GetStateNamebyId(int stateid)
        {
            string message = "";
            object state = Universal.ExecuteScalar("select name from tbl_states where id=" + stateid);
            if (state != null)
            {
                message = state.ToString();
            }
            return message;
        }
        public ActionResult GetCityList()
        {
            List<CityModule> cities = new List<CityModule>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_cities", "tbl_cities");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CityModule cmd = new CityModule();
                cmd.cityId = Convert.ToInt32(dt.Rows[i]["id"]);
                cmd.cityName = dt.Rows[i]["name"].ToString();
                cmd.stateName = GetStateNamebyId(Convert.ToInt32(dt.Rows[i]["state_id"]));
                cities.Add(cmd);
            }

            var details = cities;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AirCraft()
        {
            return View();
        }
        public JsonResult CheckCraftModelname(string craftmodel)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select AircraftModelName from tbl_aircraftcategorymaster where AircraftModelName='" + craftmodel + "'");
            if (nm != null)
            {
                message = "Aircraft model name is already exists!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckCraftCategoryname(string craftcategory)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select CategoryName from tbl_aircraftcategorymaster where CategoryName='" + craftcategory + "'");
            if (nm != null)
            {
                message = "Aircraft model category name is already exists!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public int GetMaxCategoryId()
        {
            int maxid = 0;
            string id = Universal.ExecuteScalar("select max(AirCraftCategoryId) from tbl_aircraftcategorymaster").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt32(id);

            }
            maxid = maxid + 1;
            return maxid;
        }

        public JsonResult SaveCraftCategory(AircraftCategoryModel craftmodel)
        {
            string message = "";
            if (craftmodel != null)
            {
                int res = Universal.ExecuteNonQuery("Insert into tbl_aircraftcategorymaster (AirCraftCategoryId,AircraftModelName,CategoryName) values(" +
                    GetMaxCategoryId() + ",'" + craftmodel.AircraftModelName + "','" + craftmodel.CategoryName + "')");
                if (res == 1)
                {
                    message = "Category save successfully!";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllCategory()
        {
            return View();
        }
        public ActionResult GetCategoryList()
        {
            List<AircraftCategoryModel> categories = new List<AircraftCategoryModel>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_aircraftcategorymaster", "tbl_aircraftcategorymaster");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AircraftCategoryModel cmd = new AircraftCategoryModel();
                cmd.AirCraftCategoryId = Convert.ToInt32(dt.Rows[i]["AirCraftCategoryId"]);
                cmd.AircraftModelName = dt.Rows[i]["AircraftModelName"].ToString();
                cmd.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                categories.Add(cmd);
            }

            var details = categories;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCategory(int id)
        {
            var info = Universal.SelectWithDS("select * from tbl_aircraftcategorymaster where AirCraftCategoryId=" + id, "tbl_aircraftcategorymaster");
            if (info.Rows.Count > 0)
            {
                AircraftCategoryModel carftcateg = new AircraftCategoryModel
                {
                    AirCraftCategoryId = Convert.ToInt32(info.Rows[0]["AirCraftCategoryId"]),
                    AircraftModelName = info.Rows[0]["AircraftModelName"].ToString(),
                    CategoryName = info.Rows[0]["CategoryName"].ToString(),
                };
                return View(carftcateg);
            }
            return View();
        }
        public JsonResult ModifyCategory(int id,string mname,string catname)
        {
            string message = "";
            var info = Universal.ExecuteNonQuery("update tbl_aircraftcategorymaster set AircraftModelName='" + mname + "',CategoryName='" + catname + "' where AirCraftCategoryId=" + id);
            if (info == 1)
            {
                message = "Category modified successfully!";
            }
            return Json(message,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            int res = Universal.ExecuteNonQuery("delete from tbl_aircraftcategorymaster where AirCraftCategoryId=" + id);
            return RedirectToAction("AllCategory", "SuperAdmin");

        }
        public ActionResult PartCategory()
        {
            return View();
        }
        public JsonResult CheckPartCategname(string pcatgname)
        {
            string message = "";
            object nm = Universal.ExecuteScalar
                ("select PastCategoryName from tbl_aircraftpartcatgorymaster where PastCategoryName='" + pcatgname + "'");
            if (nm != null)
            {
                message = "Part category name is already exists!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public int GetMaxPartCategoryId()
        {
            int maxid = 0;
            string id = Universal.ExecuteScalar("select max(PastCategoryId) from tbl_aircraftpartcatgorymaster").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt32(id);

            }
            maxid = maxid + 1;
            return maxid;
        }

        public JsonResult SavePartCategory(string partcatgmodel)
        {
            string message = "";
            if (partcatgmodel != null)
            {
                int res = Universal.ExecuteNonQuery("Insert into tbl_aircraftpartcatgorymaster (PastCategoryId,PastCategoryName) values(" +
                    GetMaxPartCategoryId() + ",'" + partcatgmodel + "')");
                if (res == 1)
                {
                    message = "Part category save successfully!";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartList()
        {
            return View();
        }
        public ActionResult AllPartCategory()
        {
            List<AircraftPartCatgoryModel> partcategories = new List<AircraftPartCatgoryModel>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_aircraftpartcatgorymaster", "tbl_aircraftpartcatgorymaster");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AircraftPartCatgoryModel cmd = new AircraftPartCatgoryModel();
                cmd.PartCategoryId = Convert.ToInt32(dt.Rows[i]["PastCategoryId"]);
                cmd.PasrCategoryName = dt.Rows[i]["PastCategoryName"].ToString();
                partcategories.Add(cmd);
            }

            var details = partcategories;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult EditPartCategory(int id)
        {
            var info = Universal.SelectWithDS("select * from tbl_aircraftpartcatgorymaster where PastCategoryId=" + id, "tbl_aircraftpartcatgorymaster");
            if (info.Rows.Count > 0)
            {
                AircraftPartCatgoryModel partcarftcateg = new AircraftPartCatgoryModel
                {
                    PartCategoryId = Convert.ToInt32(info.Rows[0]["PastCategoryId"]),
                    PasrCategoryName = info.Rows[0]["PastCategoryName"].ToString(),
                };
                return View(partcarftcateg);
            }
            return View();
        }

        public JsonResult ModifyPartCategory(int id,string partcatname)
        {
            string message = "";
            var info = Universal.ExecuteNonQuery("update tbl_aircraftpartcatgorymaster set PastCategoryName='" + partcatname + "' where PastCategoryId=" + id);
            if (info == 1)
            {
                message = "Part category modified successfully!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeletePartCategory(int id)
        {
            int res = Universal.ExecuteNonQuery("delete from tbl_aircraftpartcatgorymaster where PastCategoryId=" + id);
            return RedirectToAction("PartList", "SuperAdmin");

        }
        public ActionResult Slag()
        {
            return View();
        }
        public int GetMaxSlagId()
        {
            int maxid = 0;
            string id = Universal.ExecuteScalar("select max(SalgId) from tbl_aircraftslagdefectmaster").ToString();

            if (id == "")
            {
                maxid = 0;
            }
            else
            {
                maxid = Convert.ToInt32(id);

            }
            maxid = maxid + 1;
            return maxid;
        }
        public JsonResult SaveSlag(string defactname,string craftcategory)
        {
            string message = "";
            if (defactname != null && craftcategory!=null)
            {
                int res = Universal.ExecuteNonQuery("Insert into tbl_aircraftslagdefectmaster (SalgId,DefectName,AircraftCategoryName) values(" +
                    GetMaxSlagId() + ",'" + defactname + "','"+ craftcategory + "')");
                if (res == 1)
                {
                    message = "Slag/defect category save successfully!";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SlagList()
        {
            return View();
        }
        public ActionResult AllSlagCategory()
        {
            List<AircraftSlagDefectModule> slages = new List<AircraftSlagDefectModule>();
            DataTable dt = Universal.SelectWithDS("select * from tbl_aircraftslagdefectmaster", "tbl_aircraftslagdefectmaster");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AircraftSlagDefectModule cmd = new AircraftSlagDefectModule();
                cmd.SlagId = Convert.ToInt32(dt.Rows[i]["SalgId"]);
                cmd.DefactName = dt.Rows[i]["DefectName"].ToString();
                cmd.AircraftCategoryName = dt.Rows[i]["AircraftCategoryName"].ToString();

                slages.Add(cmd);
            }

            var details = slages;
            return Json(new { data = details }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult EditSlag(int id)
        {
            var info = Universal.SelectWithDS("select * from tbl_aircraftslagdefectmaster where SalgId=" + id, "tbl_aircraftslagdefectmaster");
            if (info.Rows.Count > 0)
            {
                AircraftSlagDefectModule slag = new AircraftSlagDefectModule
                {
                    SlagId = Convert.ToInt32(info.Rows[0]["SalgId"]),
                    DefactName = info.Rows[0]["DefectName"].ToString(),
                    AircraftCategoryName = info.Rows[0]["AircraftCategoryName"].ToString(),
                };
                return View(slag);
            }
            return View();
        }
        public JsonResult ModifySlag(int id, string defectname,string aircraftcateg)
        {
            string message = "";
            var info = Universal.ExecuteNonQuery("update tbl_aircraftslagdefectmaster set DefectName='" + defectname + "', AircraftCategoryName='"+ aircraftcateg + "' where SalgId=" + id);
            if (info == 1)
            {
                message = "Slag/defact modified successfully!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeleteSlag(int id)
        {
            int res = Universal.ExecuteNonQuery("delete from tbl_aircraftslagdefectmaster where SalgId=" + id);
            return RedirectToAction("SlagList", "SuperAdmin");

        }

    }
}