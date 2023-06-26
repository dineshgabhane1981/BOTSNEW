using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using DLC.App_Start;
using BOTS_BL.Models;

namespace DLC.Controllers
{
    public class DashboardController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: Dashboard
        public ActionResult Index()
        {
            DashboardViewModel objData = new DashboardViewModel();
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            objData.objDashboardConfig = sessionVariables.objDashboardConfig;
            objData.dLCDashboardContent = DCR.GetDLCDashboardContent(sessionVariables.GroupId, sessionVariables.MobileNo);

            return View(objData);
        }
    }
}