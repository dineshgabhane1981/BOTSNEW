using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models.RetailerWeb;
using BOTS_BL.Models;

namespace RetailerApp.Controllers
{
    public class HomeController : Controller
    {
        RetailerWebRepository RWR = new RetailerWebRepository();
        public ActionResult Index()
        {
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

        [HttpPost]
        public ActionResult GetCustomerDetails(string MobileNo)
        {
            CustomerDetails objData = new CustomerDetails();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData = RWR.GetCustomerDetails(userDetails.OutletOrBrandId, MobileNo);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}