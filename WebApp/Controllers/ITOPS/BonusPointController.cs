using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.ITOPS
{
    public class BonusPointController : Controller
    {
        // GET: BonusPoint
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CancelTransaction()
        {
            return View();
        }
    }
}