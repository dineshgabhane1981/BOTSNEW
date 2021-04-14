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
        ChitaleException newexception = new ChitaleException();
        // GET: Dashboard
        public ActionResult Index(string CustomerId, string CustomerType)
        {
            return View();
        }
       
        public JsonResult GetDashboardSummeryData(string Flag)
        {
            DashboardParticipantsummary SummeryData = new DashboardParticipantsummary();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                string IsBTD = "2";
                if (Flag == "1")
                    IsBTD = "1";
                SummeryData = CDR.GetSummeryDetails(UserSession.CustomerId, UserSession.CustomerType, IsBTD);
                SummeryData.PurchaseOrderPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.OrderPointsPurchase));
                SummeryData.SalesOrderPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.OrderPointsSale));
                SummeryData.RedeemedPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.RedeemedPoints));
                SummeryData.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.AddOnPoints));
                SummeryData.LostPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.LOP));
                SummeryData.TotalPointsBalanceStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.BalancePoints));

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
                //dataList.Add(objDashboardLostOpp.SCMOrder);                
                var lstData = string.Join(" ", dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetTargetData(string valueVolumne)
        {
            List<object> lstData = new List<object>();
            List<decimal> dataList = new List<decimal>();

            try
            {
                bool IsValue = false;
                if (valueVolumne == "1")
                    IsValue = true;
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                DashboardTarget objDashboardTarget = new DashboardTarget();
                objDashboardTarget = CDR.GetTargetData(UserSession.CustomerId, IsValue);

                dataList.Add(objDashboardTarget.TargetValueWise);
                dataList.Add(objDashboardTarget.AchiveValueWise);
                dataList.Add(objDashboardTarget.TargetVolumeWise);
                dataList.Add(objDashboardTarget.AchiveVolumeWise);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetRankData()
        {
            DashboardRank rankData = new DashboardRank();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];                
                rankData = CDR.GetRankData(UserSession.CustomerId, UserSession.CustomerType);
                rankData.RankPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(rankData.RankPoints));
                UserSession.CurrentRank = Convert.ToString(rankData.CurrentRank);
                Session["ChitaleUser"] = UserSession;
                ViewBag.schoolName = rankData.CurrentRank;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = rankData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}