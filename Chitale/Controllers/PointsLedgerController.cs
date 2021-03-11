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
        PointsLedgerRepository PLR = new PointsLedgerRepository();
        // GET: PointsLedger
        public ActionResult Index()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            bojList = PLR.GetPointLedgerData(UserSession.CustomerId);
            return View(bojList);
        }

        public ActionResult GetInvoiceOrders(string id)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            bojList = PLR.GetInvoiceData(id);
            return Json(bojList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TgtVsAch()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            TgtvsAchMaster objData = new TgtvsAchMaster();
            objData = PLR.GetOverallData(UserSession.CustomerId);

            return View(objData);
        }

        public ActionResult GetCategoryTgtVsAch(string id, string pType)
        {
            List<TgtvsAchMaster> objData = new List<TgtvsAchMaster>();
            objData = PLR.GetCategoryData(id);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubCategoryTgtVsAch(string CategoryCode)
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            List<TgtvsAchMaster> objData = new List<TgtvsAchMaster>();
            objData = PLR.GetSubCategoryData(UserSession.CustomerId, CategoryCode);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductTgtVsAch(string SubCategoryCode)
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            List<TgtvsAchMaster> objData = new List<TgtvsAchMaster>();
            objData = PLR.GetProductData(UserSession.CustomerId, SubCategoryCode);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }       

    }
}