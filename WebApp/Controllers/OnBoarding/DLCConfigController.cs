using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.OnBoarding
{
    public class DLCConfigController : Controller
    {
        // GET: DLCConfig
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DashboardConfig()
        {
            return View();
        }
        public ActionResult ProfileConfig()
        {
            return View();
        }
    }
}