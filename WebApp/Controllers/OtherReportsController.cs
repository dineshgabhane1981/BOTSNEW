using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class OtherReportsController : Controller
    {
        // GET: OtherReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Productwise()
        {
            return View();
        }
        public ActionResult Manufacturer()
        {
            return View();
        }
    }
}