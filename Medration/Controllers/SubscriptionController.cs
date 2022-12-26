using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medration.Controllers
{
    public class SubscriptionController : Controller
    {
        // GET: Subscription
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadDetails(string num)
        {
            var NumberOfPerson = Convert.ToInt32(num);
            ViewBag.NumberOfPerson = NumberOfPerson;
            return PartialView("_LoadDetailsForSubscription");
        }
    }
}