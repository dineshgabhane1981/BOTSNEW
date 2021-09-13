using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;

namespace Chitale.Controllers
{
    public class HomeController : Controller
    {
        ChitaleDashboardRepository CDR = new ChitaleDashboardRepository();
        public ActionResult Index(string CustomerId, string CustomerType, string MobileNo)
        {
            var objCust = CDR.GetCustomerDetail(CustomerId, CustomerType);

            if (objCust != null)
            {
                objCust.Type = CustomerType;
                if (CustomerType == "Distributors" || CustomerType == "SuperStockiest" || CustomerType == "Retailers")
                {
                    objCust.CustomerCategory = "Participant";
                    Session["ChitaleUser"] = objCust;
                    Session["CategoryParticipant"] = "Participant";
                    return RedirectToAction("Index", "Dashboard", new { CustomerId = CustomerId, CustomerType = CustomerType });
                }

                if (CustomerType == "Management")
                {
                    objCust.CustomerCategory = "Management";
                    Session["ChitaleManagement"] = objCust;
                    Session["CategoryManagement"] = "Management";
                    return RedirectToAction("Index", "ManagementDashboard", new { CustomerId = CustomerId, CustomerType = CustomerType });
                }

                if (CustomerType == "SalesExecutive" || CustomerType == "SalesManager" || CustomerType == "SalesOfficer" || CustomerType == "SalesRepresentative" || CustomerType == "NationalHead"
                    || CustomerType == "ZonalHead" || CustomerType == "StateHead")
                {
                    objCust.CustomerCategory = "Employee";
                    Session["ChitaleEmployee"] = objCust;
                    Session["CategoryEmployee"] = "Employee";
                    return RedirectToAction("Index", "Employee", new { CustomerId = CustomerId, CustomerType = CustomerType });
                }
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