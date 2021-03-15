using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class ManagementDashboardController : Controller
    {
        ManagementDashboardRepository MDR = new ManagementDashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: ManagementDashboard
        public ActionResult Index()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            objModel.SubClusterList = MDR.GetSubClusterList();
            objModel.CityList = MDR.GetCityList();            
            return View(objModel);
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
                //SummeryData = CDR.GetSummeryDetails(UserSession.CustomerId, IsBTD);
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
        public ActionResult ParticipantList()
        {
            return View();
        }
        public ActionResult LeaderBoard()
        {
            return View();
        }
        public ActionResult OrdertoRavanaDays()
        {
            return View();
        }
        public ActionResult InvoicetoOrder()
        {
            return View();
        }
       

    }
}