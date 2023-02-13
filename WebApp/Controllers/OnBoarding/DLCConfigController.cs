using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;

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
            DLCDashboard objDLCDashboard = new DLCDashboard();
            DLCDashboardViewModel objData = new DLCDashboardViewModel();
            objData.objDLCDashboard = objDLCDashboard;
            return View(objData);
        }
        public ActionResult ProfileConfig()
        {
            return View();
        }
    }
}