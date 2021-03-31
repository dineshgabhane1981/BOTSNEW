using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;

namespace Chitale.Controllers
{
    public class PointsLedgerController : Controller
    {
        ChitaleException newexception = new ChitaleException();
        PointsLedgerRepository PLR = new PointsLedgerRepository();
        // GET: PointsLedger
        public ActionResult Index()
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                //var UserSession = (CustomerDetail)Session["ChitaleUser"];
                //bojList = PLR.GetPointLedgerData(UserSession.CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return View();
        }

        public ActionResult GetPointLedgerData(string isBTD, string FrmDate,string ToDate)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                bojList = PLR.GetPointLedgerData(UserSession.CustomerId, isBTD, FrmDate, ToDate);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return PartialView("_Listing",bojList);
        }

        public ActionResult GetInvoiceOrders(string id,string CustomerId)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                bojList = PLR.GetInvoiceData(id, CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(bojList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TgtVsAch()
        {
            TgtVsAchViewModel objData = new TgtVsAchViewModel();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];

                objData.objOverAll = PLR.GetOverallData(UserSession.CustomerId);
                objData.objCategory = PLR.GetCategoryData(UserSession.CustomerId);
                objData.objSubCategory = PLR.GetSubCategoryData(UserSession.CustomerId);
                objData.objProducts = PLR.GetProductData(UserSession.CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return View(objData);
        }
    }
}