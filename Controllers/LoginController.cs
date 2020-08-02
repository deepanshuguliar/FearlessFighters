using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportAuthorityofIndia.Models;
using ATS;
using System.Web.Security;
using System.Net;
using System.IO;

namespace AirportAuthorityofIndia.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            var v = Universal.SelectWithDS(
                "select * from tbl_AirportRegistration where UserLogin='"+login.loginname.Trim().ToLower()+ "'", "tbl_AirportRegistration");
            //work.Repository<Reseller>().Get(x => x.loginname.Trim().ToLower() == login.loginname.Trim().ToLower());
            //v != null
            if (v.Rows.Count > 0)
            {
                if (string.Compare(Crypto.Hash(login.loginpassword), v.Rows[0]["UserPassword"].ToString()) == 0)
                {
                    int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                    var ticket = new FormsAuthenticationTicket(login.loginname, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    //List<MenuModels> _menus = dbentities.SubMenuTable.Where(x => x.roleId == v.roleid).Select(x => new MenuModels
                    //{
                    //    MainMenuId = x.MainMenuMaster.Id,
                    //    MainMenuName = x.MainMenuMaster.MainMenu,
                    //    SubMenuId = x.Id,
                    //    SubMenuName = x.SubMenu,
                    //    ControllerName = x.Controller,
                    //    ActionName = x.Action,
                    //    RoleId = x.roleId,
                    //    RoleName = x.RoleMaster.role
                    //}).ToList();
                    TempData["airportid"] = v.Rows[0]["AirportId"];
                    TempData["airportname"] = v.Rows[0]["AirportName"];
                    TempData["referenceid"] = v.Rows[0]["AirportReferenceId"];
                    TempData["roletype"] = v.Rows[0]["UserType"];
                    Session["ContactPerson"] = v.Rows[0]["ContactPerson"];
                    Session["ContactNumber"] = v.Rows[0]["ContactNumber"];

                    Session["airportid"] = v.Rows[0]["AirportId"];
                    Session["roletype"] = v.Rows[0]["UserType"];
                    Session["referenceid"] = v.Rows[0]["AirportReferenceId"];
                    Session["airportname"] = v.Rows[0]["AirportName"];
                    //Session["MenuMaster"] = _menus;
                    //if (Url.IsLocalUrl(ReturnUrl))
                    //{
                    //    return Redirect(ReturnUrl);
                    //}
                    //else
                    //{ 
                    if (Session["roletype"].ToString() == "SuperAdmin")
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            //
                            Session["url"] = "SuperAdmin/Index";
                            return RedirectToAction("otpcheck", "login");
                            // return RedirectToAction("Index", "SuperAdmin");
                        }
                    }
                    else if (Session["roletype"].ToString() == "FIRAdmin")
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Session["url"] = "SubAdmin/Index";
                            return RedirectToAction("otpcheck", "login");
                            // return RedirectToAction("Index", "SubAdmin");
                        }
                    }
                    else if (Session["roletype"].ToString() == "ATCAdmin")
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Session["url"] = "ATC/Index";
                            return RedirectToAction("otpcheck", "login");
                            // return RedirectToAction("Index", "ATC");
                        }
                    }
                    //}
                }
                else
                {
                    message = "Invalid login password!";
                }
            }
            else
            {
                message = "Invalid login name!";
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            var v = Universal.SelectWithDS(
                "select * from tbl_AirportRegistration where UserLogin='" + login.loginname.Trim().ToLower() + "'", "tbl_AirportRegistration");
            //work.Repository<Reseller>().Get(x => x.loginname.Trim().ToLower() == login.loginname.Trim().ToLower());
            //v != null
            if (v.Rows.Count > 0)
            {
                if (string.Compare(Crypto.Hash(login.loginpassword), v.Rows[0]["UserPassword"].ToString()) == 0)
                {
                    int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                    var ticket = new FormsAuthenticationTicket(login.loginname, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    //List<MenuModels> _menus = dbentities.SubMenuTable.Where(x => x.roleId == v.roleid).Select(x => new MenuModels
                    //{
                    //    MainMenuId = x.MainMenuMaster.Id,
                    //    MainMenuName = x.MainMenuMaster.MainMenu,
                    //    SubMenuId = x.Id,
                    //    SubMenuName = x.SubMenu,
                    //    ControllerName = x.Controller,
                    //    ActionName = x.Action,
                    //    RoleId = x.roleId,
                    //    RoleName = x.RoleMaster.role
                    //}).ToList();
                    TempData["airportid"] = v.Rows[0]["AirportId"];
                    TempData["airportname"] = v.Rows[0]["AirportName"];
                    TempData["referenceid"] = v.Rows[0]["AirportReferenceId"];
                    TempData["roletype"] = v.Rows[0]["UserType"];

                    Session["airportid"] = v.Rows[0]["AirportId"];
                    Session["roletype"] = v.Rows[0]["UserType"];
                    Session["referenceid"] = v.Rows[0]["AirportReferenceId"];
                    Session["airportname"] = v.Rows[0]["AirportName"];
                    //Session["MenuMaster"] = _menus;
                    //if (Url.IsLocalUrl(ReturnUrl))
                    //{
                    //    return Redirect(ReturnUrl);
                    //}
                    //else
                    //{ 
                    if (Session["roletype"].ToString() == "SuperAdmin")
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "SuperAdmin");
                        }
                    }
                    else if (Session["roletype"].ToString() == "FIRAdmin")
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "SubAdmin");
                        }
                    }
                    else if (Session["roletype"].ToString() == "ATCAdmin")
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "ATC");
                        }
                    }
                    //}
                }
                else
                {
                    message = "Invalid login password!";
                }
            }
            else
            {
                message = "Invalid login name!";
            }
            ViewBag.Message = message;
            return View();

        }

        [Authorize]
        public ActionResult otpcheck()
        {
            string otp = new Random().Next(1000, 9000).ToString();
            Session["otp"] = otp;
            string name = Session["ContactPerson"].ToString();
            string MobileNo = Session["ContactNumber"].ToString();
            string strUrl = "http://sms.3dguardian.com/http-api.php?username=BBPSPP&password=bbpspp01&senderid=BBPSPP&route=1&number=" + MobileNo + "&message=Hello," + name + " Someone trying to login. The OTP is :" + otp;
            WebRequest request = HttpWebRequest.Create(strUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = (Stream)response.GetResponseStream();
            StreamReader readStream = new StreamReader(s);
            string dataString = readStream.ReadToEnd();
            response.Close();
            s.Close();
            readStream.Close();
            Response.Write(Session["otp"].ToString());
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult otpcheck(string otp, string status)
        {
            if (otp == Session["otp"].ToString())
            {
                return Redirect("/" + Session["url"].ToString());
            }
            else
            {
                Response.Write("<script>alert('Invalid OTP')</script>");
            }
            return View();
        }
    }
}