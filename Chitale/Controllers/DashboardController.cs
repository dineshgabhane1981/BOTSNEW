using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using BOTS_BL;
using System.Globalization;

namespace Chitale.Controllers
{
    public class DashboardController : Controller
    {
        ChitaleDashboardRepository CDR = new ChitaleDashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: Dashboard
        public ActionResult Index()
        {            
            return View();
        }

        public JsonResult GetDashboardSummeryData(string Flag)
        {
            Dashboardsummary SummeryData = new Dashboardsummary();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                bool IsBTD = false;
                if (Flag == "1")
                    IsBTD = true;
                SummeryData = CDR.GetSummeryDetails(UserSession.CustomerId, IsBTD);
                SummeryData.PurchaseOrderPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.PurchaseOrderPoints));
                SummeryData.SalesOrderPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.SalesOrderPoints));
                SummeryData.RedeemedPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.RedeemedPoints));
                SummeryData.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.AddOnPoints));
                SummeryData.LostPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.LostPoints));
                SummeryData.TotalPointsBalanceStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.TotalPointsBalance));

            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = SummeryData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }



        public JsonResult GetDashboardLostOppData(string profileFlag)
        {
            List<decimal> dataList = new List<decimal>();
            try
            {
                bool IsBTD = false;
                if (profileFlag == "1")
                    IsBTD = true;
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                DashboardLostOpp objDashboardLostOpp = new DashboardLostOpp();
                objDashboardLostOpp = CDR.GetDashboardLostOppData(UserSession.CustomerId, IsBTD);

                dataList.Add(objDashboardLostOpp.LateOrder);
                dataList.Add(objDashboardLostOpp.CancelOrder);
                dataList.Add(objDashboardLostOpp.TgtVsAch);
                dataList.Add(objDashboardLostOpp.SCMOrder);                
                var lstData = string.Join(" ", dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetTargetData(string profileFlag)
        {
            List<object> lstData = new List<object>();
            List<decimal> dataListValue = new List<decimal>();
            List<decimal> dataListVolume = new List<decimal>();
            try
            {
                bool IsBTD = false;
                if (profileFlag == "1")
                    IsBTD = true;
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                DashboardTarget objDashboardTarget = new DashboardTarget();
                objDashboardTarget = CDR.GetTargetData(UserSession.CustomerId, IsBTD);

                dataListValue.Add(objDashboardTarget.TargetValueWise);
                dataListValue.Add(objDashboardTarget.TargetVolumeWise); 
                dataListVolume.Add(objDashboardTarget.AchiveValueWise);
                dataListVolume.Add(objDashboardTarget.AchiveVolumeWise);
                lstData.Add(dataListValue);
                lstData.Add(dataListVolume);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}