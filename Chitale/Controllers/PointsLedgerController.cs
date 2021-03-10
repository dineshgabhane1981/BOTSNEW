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
            return View();
        }

    }
}