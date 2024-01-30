using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;

namespace WebApp.Controllers
{
    public class CouponController : Controller
    {
        CommonFunctions Common = new CommonFunctions();
        CouponRepository CR = new CouponRepository();
        // GET: Coupon
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var data = CR.GetAllCouponUpload(userDetails.connectionString);
            var coupon = Common.GenerateCoupon();
            return View(data);
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