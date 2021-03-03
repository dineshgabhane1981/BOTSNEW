using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;

namespace Chitale.Controllers
{
    public class HomeController : Controller
    {
        ChitaleDashboardRepository CDR = new ChitaleDashboardRepository();
        public ActionResult Index(string CustomerId, string CustomerType)
        {
            var objCust = CDR.GetCustomerDetail(CustomerId);
            if (objCust != null)
            {
                objCust.Type = CustomerType;
                Session["ChitaleUser"] = objCust;
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}