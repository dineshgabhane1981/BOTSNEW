using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using System.Web.Script.Serialization;

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
            DLCProfileUpdate objDLCProfUpdt = new DLCProfileUpdate();
            DLCProfileUpdateViewModel objProfData = new DLCProfileUpdateViewModel();
            objProfData.objDLCProfUpdt = objDLCProfUpdt;
            return View(objProfData);
        }
        public ActionResult SaveDashboard(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objDashboardData = (object[])json_serializer.DeserializeObject(jsonData);

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}