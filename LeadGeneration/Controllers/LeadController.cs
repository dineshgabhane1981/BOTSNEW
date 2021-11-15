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
        public ActionResult LeadTransfer()
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

        public ActionResult AddSalesLead(LeadViewModel objData)
        {
            int LeadId = 0;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                objData.sALES_TblLeads.AddedDate = DateTime.Now;
                objData.sALES_TblLeads.AddedBy = userDetails.LoginId;
                objData.sALES_TblLeads.AssignedLead = userDetails.LoginId;
                var meetingType = objData.sALES_TblLeads.MeetingType;
                LeadId = SLR.AddSalesLead(objData.sALES_TblLeads);
                if (meetingType == "salesdone")
                {
                    //string url = "https://blueocktopus.in/bots?LoginID=" + userDetails.LoginId + "&LeadId=" + objData.sALES_TblLeads.LeadId + "";
                    string url = "http://localhost:57265?LoginID=" + userDetails.LoginId + "";
                    ViewData["LeadId"] = LeadId;
                    ViewData["URL"] = url;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["error"] = "Error Occured";
                // return View("AddLead");
            }
            ViewData["Status"] = LeadId;
            objData.lstBillingPartner = CR.GetBillingPartner();
            objData.lstcategory = CR.GetRetailCategory();
            objData.lstStates = CR.GetStates();
            objData.lstCity = CR.GetCity();

            return View("AddLead", objData);

        }
        
        public ActionResult GetSearchLeads(string searchData)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                objviewmodel.lstsALES_TblLeads = SLR.GetSearchedLeads(Convert.ToString(item["MobileNo"]), Convert.ToString(item["BusinessName"]), 
                    Convert.ToString(item["DtFrom"]), Convert.ToString(item["DtTo"]), Convert.ToString(item["LeadStatus"]), 
                    Convert.ToString(item["ContactType"]), Convert.ToString(item["MeetingType"]), Convert.ToString(item["City"]),
                    Convert.ToString(item["BillingPartner"]), Convert.ToString(item["SalesManager"]));
            }
            
            return PartialView("_SearchLeadListing", objviewmodel);
        }

        public ActionResult GetSearchLeadsforLeadTransfer(string searchData)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(searchData);
                foreach (Dictionary<string, object> item in objData)
                {
                    objviewmodel.lstsALES_TblLeads = SLR.GetSearchedLeads(Convert.ToString(item["MobileNo"]), Convert.ToString(item["BusinessName"]),
                        Convert.ToString(item["DtFrom"]), Convert.ToString(item["DtTo"]), Convert.ToString(item["LeadStatus"]),
                        Convert.ToString(item["ContactType"]), Convert.ToString(item["MeetingType"]), Convert.ToString(item["City"]),
                        Convert.ToString(item["BillingPartner"]), Convert.ToString(item["SalesManager"]));
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "Get Lead");
            }

            return PartialView("_LeadTransferList", objviewmodel);
        }

        public JsonResult GetLeadTrackingList(string Id)
        {
            List<LeadTracking> lstLeadTracking = new List<LeadTracking>();
            lstLeadTracking = SLR.GetLeadTracking(Id);

            return new JsonResult() { Data = lstLeadTracking, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult TransferLead(int[] LeadId, string SaleManagerId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            LeadViewModel objviewmodel = new LeadViewModel();
            bool result = SLR.LeadTransfer(LeadId, SaleManagerId, userDetails.LoginId);
            return PartialView("_LeadTransferList", objviewmodel);
        }

        public ActionResult SalesCount()
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();           
            return View(objviewmodel);
        }

        public JsonResult GetSalesCount(DateTime Fromdate,DateTime ToDate)
        {
            List<SalesCount> lstsalescount = SLR.GetSalesCounts(Fromdate, ToDate);
            return new JsonResult() { Data = lstsalescount, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

    }
}