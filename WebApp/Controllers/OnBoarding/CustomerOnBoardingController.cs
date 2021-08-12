using BOTS_BL;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using BOTS_BL.Models.OnBoarding;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models;
using System.Web.Script.Serialization;

namespace WebApp.Controllers.OnBoarding
{
    public class CustomerOnBoardingController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        CustomerOnBoardingRepository COR = new CustomerOnBoardingRepository();
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
                objData.lstSourcedBy = CR.GetSourcedBy();
                objData.lstRMAssigned = CR.GetRMAssigned();
                List<SelectListItem> refferedname = new List<SelectListItem>();
                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "Please Select";
                refferedname.Add(item);
                objData.lstAllGroups = refferedname;


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