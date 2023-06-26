using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models;
using BOTS_BL.Models.SalesLead;

namespace LeadGeneration.ViewModel
{
    public class LeadViewModel
    {
        public SALES_tblLeads sALES_TblLeads { get; set; }
        public List<SalesLead> lstsALES_TblLeads { get; set; }
        public List<SelectListItem> lstcategory { get; set; }
        public List<SelectListItem> lstBillingPartner { get; set; }
        public List<SelectListItem> lstLeadSource { get; set; }
        public List<SelectListItem> lstStates { get; set; }
        public List<SelectListItem> lstCity { get; set; }
        public List<SelectListItem> lstSalesManager { get; set; }
        public List<SalesMatrix> lstsalesMatrices { get; set; }
        public List<SelectListItem> lstLeadSourceNames { get; set; }
        public SelectListItem[] BOProducts()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Octa Plus", Value = "1" }, new SelectListItem() { Text = "Octa XS", Value = "2" } };
        }
        public SelectListItem[] ContactType()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Call", Value = "Call" }, new SelectListItem() { Text = "Online Meeting", Value = "OnlineMeeting" }, new SelectListItem() { Text = "Personal Meeting", Value = "PersonalMeeting" } };
        }
        public SelectListItem[] LeadStatus()
        {
            return new SelectListItem[6] { new SelectListItem() { Text = "Interested", Value = "Interested" }, new SelectListItem() { Text = "Not Interested", Value = "NotInterested" }, new SelectListItem() { Text = "No Response", Value = "NoResponse" }, new SelectListItem() { Text = "Follow Up", Value = "Followup" }, new SelectListItem() { Text = "Long Follow Up", Value = "LongFollowUp" }, new SelectListItem() { Text = "Sales Done", Value = "salesdone" }};
        }
        public SelectListItem[] MeetingType()
        {
            return new SelectListItem[6] { new SelectListItem() { Text = "1st Call", Value = "1stcall" }, new SelectListItem() { Text = "1st Meeting", Value = "1stMeeting" }, new SelectListItem() { Text = "Follow Up", Value = "followup" }, new SelectListItem() { Text = "Closure", Value = "Closure" }, new SelectListItem() { Text = "Sales Done", Value = "salesdone" }, new SelectListItem() { Text = "Other", Value = "other" } };
        }
        public SelectListItem[] LeadSource()
        {
            return new SelectListItem[6] { new SelectListItem() { Text = "Direct", Value = "Direct" }, new SelectListItem() { Text = "Telecaller", Value = "Telecaller" }, new SelectListItem() { Text = "Customer Reference", Value = "CustomerReference" }, new SelectListItem() { Text = "Billing Partner", Value = "billingpartner" }, new SelectListItem() { Text = "Channel Partner", Value = "channelpartner" }, new SelectListItem() { Text = "Employee Referral", Value = "employeereferral" } };
        }
        public SelectListItem[] CustomerType()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Manger/Cashier", Value = "Manger/Cashier" }, new SelectListItem() { Text = "Owner/Partner", Value = "Owner/Partner" }, new SelectListItem() { Text = "Decision Maker", Value = "DecisionMaker" }, new SelectListItem() { Text = "Others", Value = "Others" } };
        }
        public SelectListItem[] LeadType()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Hot", Value = "Hot" }, new SelectListItem() { Text = "Warm", Value = "Warm" }, new SelectListItem() { Text = "Cold", Value = "Cold" } };
        }
        public SelectListItem[] LeadSourceForReferral()
        {
            return new SelectListItem[2] {new SelectListItem() { Text = "Customer Reference", Value = "CustomerReference" }, new SelectListItem() { Text = "Billing Partner", Value = "billingpartner" } };
        }
        public List<SelectListItem> lstMonthlist { get; set; }
        public List<SelectListItem> lstYearlist { get; set; }

    }
}