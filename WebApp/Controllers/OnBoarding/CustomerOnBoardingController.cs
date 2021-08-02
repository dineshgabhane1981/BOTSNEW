using BOTS_BL;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers.OnBoarding
{
    public class CustomerOnBoardingController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: CustomerOnBoarding
        public ActionResult Index()
        {
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            try
            {
                objData.lstCity = CR.GetCity();
                objData.lstRetailCategory = CR.GetRetailCategory();
                objData.lstBillingPartner = CR.GetBillingPartner();
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "");
            }
            return View(objData);
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