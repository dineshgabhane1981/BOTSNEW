using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class NoActionController : Controller
    {
        // GET: NoAction
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Participants()
        {
            return View();
        }
        public ActionResult Products()
        {
            return View();
        }
    }
}