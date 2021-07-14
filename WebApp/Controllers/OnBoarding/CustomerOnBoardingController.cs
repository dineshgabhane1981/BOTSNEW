using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.OnBoarding
{
    public class CustomerOnBoardingController : Controller
    {
        // GET: CustomerOnBoarding
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CustomerDetail()
        {
            return View();
        }
        public ActionResult ProductAssignmentDetails()
        {
            return View();
        }
        public ActionResult DealDetails()
        {
            return View();
        }

        public ActionResult PaymentSchedule()
        {
            return View();
        }

        public ActionResult UpdatePaymentDetails()
        {
            return View();
        }
        public ActionResult Notifications()
        {
            return View();
        }

    }
}