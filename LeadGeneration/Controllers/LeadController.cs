using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Models.SalesLead;
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
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstsALES_TblLeads = SLR.GetSalesLeads();
            objviewmodel.lstCity = CR.GetCity();
            objviewmodel.lstBillingPartner = CR.GetBillingPartner();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            return View(objviewmodel);

        }
        public ActionResult AddLead(string leadId)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            SALES_tblLeads objData = new SALES_tblLeads();
            if (!string.IsNullOrEmpty(leadId))
            {
                objviewmodel.lstBillingPartner = CR.GetBillingPartner();
                objviewmodel.lstcategory = CR.GetRetailCategory();
                objviewmodel.lstStates = CR.GetStates();
                objviewmodel.lstCity = CR.GetCity();
                objData = SLR.GetsalesLeadByLeadId(Convert.ToInt32(leadId));
                objviewmodel.sALES_TblLeads = objData;
            }
            else
            {
                objviewmodel.lstBillingPartner = CR.GetBillingPartner();
                objviewmodel.lstcategory = CR.GetRetailCategory();
                objviewmodel.lstStates = CR.GetStates();
                objviewmodel.lstCity = CR.GetCity();
                objviewmodel.sALES_TblLeads = objData;
            }
                
            
            return View(objviewmodel);
        }
        
        public bool AddSalesLead(LeadViewModel objData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                
                objData.sALES_TblLeads.AddedDate = DateTime.Now;
                objData.sALES_TblLeads.AddedBy = userDetails.LoginId;
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
    
        public ActionResult GetSearchLeads(string searchData)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstsALES_TblLeads = SLR.GetSearchedLeads();
            return PartialView("_SearchLeadListing", objviewmodel);
        }   
    
    }
}