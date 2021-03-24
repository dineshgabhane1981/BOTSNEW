using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;

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
                var UserSession = (CustomerDetail)Session["ChitaleUser"];                
                bojList = PLR.GetPointLedgerData(UserSession.CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return View(bojList);
        }

        public ActionResult GetInvoiceOrders(string id)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                bojList = PLR.GetInvoiceData(id);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(bojList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TgtVsAch()
        {
            TgtvsAchMaster objData = new TgtvsAchMaster();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];

                objData = PLR.GetOverallData(UserSession.CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return View(objData);
        }

        public ActionResult GetCategoryTgtVsAch(string id, string pType)
        {
            List<TgtvsAchMaster> objData = new List<TgtvsAchMaster>();
            try
            {
                objData = PLR.GetCategoryData(id);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubCategoryTgtVsAch(string CategoryCode)
        {
            List<TgtvsAchMaster> objData = new List<TgtvsAchMaster>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                objData = PLR.GetSubCategoryData(UserSession.CustomerId, CategoryCode);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductTgtVsAch(string SubCategoryCode)
        {
            List<TgtvsAchMaster> objData = new List<TgtvsAchMaster>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                objData = PLR.GetProductData(UserSession.CustomerId, SubCategoryCode);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

    }
}