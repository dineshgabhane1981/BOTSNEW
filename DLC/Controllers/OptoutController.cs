using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DLC.Controllers
{
    public class OptoutController : Controller
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        DLCConfigRepository DCR = new DLCConfigRepository();
        Exceptions newexception = new Exceptions();
        // GET: Optout
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            DLCDashboardContent objData = new DLCDashboardContent();
            objData = DCR.GetDLCDashboardContent(sessionVariables.GroupId, sessionVariables.MobileNo);
            objData.MobileNo = sessionVariables.MobileNo;
            string connStr = objCustRepo.GetCustomerConnString(sessionVariables.GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var earnPoint = context.tblCustPointsMasters.Where(x => x.MobileNo == sessionVariables.MobileNo && x.IsActive == true && x.PointsType == "Base").Sum(y => y.Points) ?? 0;
                var BonousPoint = context.tblCustPointsMasters.Where(x => x.MobileNo == sessionVariables.MobileNo && x.IsActive == true && x.PointsType == "Bonus").Sum(y => y.Points) ?? 0;
                var PointsToRS = context.tblRuleMasters.Select(x => x.PointsAllocation).FirstOrDefault();
                var custumerDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == sessionVariables.MobileNo).FirstOrDefault();
                var pointsinRs = (earnPoint + BonousPoint) * PointsToRS ?? 0;
                pointsinRs = Math.Round(pointsinRs, 2);
                ViewBag.earnPoint = earnPoint;
                ViewBag.bonousPoint = BonousPoint;
                ViewBag.pointsinRs = pointsinRs;
                ViewBag.customerName = custumerDetails.Name;
                ViewBag.optout = custumerDetails.DisableSMSWAPromo;
            }
            return View(objData);
        }
        public ActionResult UpdateOptout(string IsOptout)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            try
            {
                status = DCR.UpdateOptout(sessionVariables.GroupId, sessionVariables.MobileNo, Convert.ToBoolean(IsOptout), false);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateOptout");
            }


            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult Update1Optout(string IsOptout)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            try
            {
                status = DCR.UpdateOptout(sessionVariables.GroupId, sessionVariables.MobileNo, Convert.ToBoolean(IsOptout), true);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Update1Optout");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}