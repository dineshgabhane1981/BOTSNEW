using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;

namespace WebApp.Controllers
{
    public class CouponController : Controller
    {
        CommonFunctions Common = new CommonFunctions();
        // GET: Coupon
        public ActionResult Index()
        {
            var coupon = Common.GenerateCoupon();
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
    }
}