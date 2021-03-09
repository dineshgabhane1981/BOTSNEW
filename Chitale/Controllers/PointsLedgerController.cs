using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class PointsLedgerController : Controller
    {
        // GET: PointsLedger
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TgtVsAch()
        {
            return View();
        }

    }
}