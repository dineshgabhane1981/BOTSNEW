using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;

namespace WebApp.Controllers
{
    public class HeatMapsController : Controller
    {
        HeatMapsRepository HMR = new HeatMapsRepository();
        ReportsRepository RR = new ReportsRepository();
        // GET: HeatMaps
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DaywiseHourwise()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletList = lstOutlet;
            return View();
        }

        public ActionResult SMSConversion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDaywiseHourwiseResult(string outletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            List<DaywiseHourwise> objNonTransactingLST = new List<DaywiseHourwise>();
            objNonTransactingLST = HMR.GetDaywiseHourwiseData(userDetails.GroupId, outletId, userDetails.connectionString);
            return PartialView("_DaywiseHourwise", objNonTransactingLST);
            //return new JsonResult() { Data = objNonTransactingLST, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}