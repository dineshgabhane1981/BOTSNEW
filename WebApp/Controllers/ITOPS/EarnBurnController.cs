using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.ITOPS
{
    public class EarnBurnController : Controller
    {
        // GET: EarnBurn
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Burn()
        {
            return View();
        }
    }
}