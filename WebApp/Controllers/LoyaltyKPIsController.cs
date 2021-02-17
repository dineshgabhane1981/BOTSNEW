using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class LoyaltyKPIsController : Controller
    {
        // GET: LoyaltyKPIs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BizObj()
        {
            return View();
        }

        public ActionResult LoyaltyPerformance()
        {
            return View();
        }

        public ActionResult LoyaltySegments()
        {
            return View();
        }
    }
}