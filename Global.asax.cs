using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ATS;
namespace AirportAuthorityofIndia
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Universal.InitConnection();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
