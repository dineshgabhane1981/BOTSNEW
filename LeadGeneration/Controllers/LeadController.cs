using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using LeadGeneration.ViewModel;

namespace LeadGeneration.Controllers
{
    public class LeadController : Controller
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        SalesLeadRepository SLR = new SalesLeadRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddLead()
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            SALES_tblLeads objData = new SALES_tblLeads();
            objviewmodel.lstBillingPartner = CR.GetBillingPartner();
            objviewmodel.lstcategory = CR.GetRetailCategory();
            objviewmodel.lstStates = CR.GetStates();
            objviewmodel.lstCity = CR.GetCity();
            objviewmodel.sALES_TblLeads = objData;
            return View(objviewmodel);
        }
        [HttpPost]
        public bool AddSalesLead(LeadViewModel objData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                status = SLR.AddSalesLead(objData.sALES_TblLeads);
               
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["error"] = "Error Occured";
               // return View("AddLead");
            }

            return status;

        }
    }
}