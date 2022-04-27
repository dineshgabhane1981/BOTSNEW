using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.OnBoarding;

namespace WebApp.ViewModel
{
    public class OnBoardingSalesViewModel
    {
        public BOTS_TblGroupMaster bots_TblGroupMaster { get; set; }
        public BOTS_TblRetailMaster bots_TblRetailMaster { get; set; }
        public BOTS_TblDealDetails bots_TblDealDetails { get; set; }
        public BOTS_TblPaymentDetails bots_TblPaymentDetails { get; set; }
        public BOTS_TblInstallmentDetails bots_TblInstallmentDetails { get; set; }
        public BOTS_TblOutletMaster bots_TblOutletMaster { get; set; }
        public List<BOTS_TblRetailMaster> objRetailList { get; set; }
        public List<BOTS_TblInstallmentDetails> objInstallmentList { get; set; }
        public List<BOTS_TblOutletMaster> lstOutlets { get; set; }
        public List<BOTS_TblSMSConfig> lstSMSConfig { get; set; }
        public bool IsBrand { get; set; }
        public List<SelectListItem> lstCity { get; set; }
        public List<SelectListItem> lstRetailCategory { get; set; }
        public List<SelectListItem> lstSourcedBy { get; set; }
        public List<SelectListItem> lstRMAssigned { get; set; }
        public List<SelectListItem> lstBillingPartner { get; set; }
        public List<SelectListItem> lstAllGroups { get; set; }
        public List<SelectListItem> lstRefferedCategory { get; set; }
        public List<SelectListItem> lstStates { get; set; }
        public List<SelectListItem> lstBrands { get; set; }
        public BOTS_TblPointsEarnRuleConfig objpointsearnruleconfig { get; set; }
        public BOTS_TblPointsBurnRuleConfig objpointsburnruleconfig { get; set; }
        public BOTS_TblEarnPointsSlabConfig objearnpointslab { get; set; }
        public List<BOTS_TblEarnPointsSlabConfig> lstearnpointslabconfig { get; set; }
        public List<EarnPointLevel> lstearnpoint { get; set; }
        public List<BOTS_TblCommunicationSet> lstCommunicationSet { get; set; }
        public List<BOTS_TblCommunicationSetAssignment> lstCommunicationSetAssignment { get; set; }
        public BOTS_TblDLCLinkConfig objDLCLinkConfig { get; set; }

        public string LeadId { get; set; }
        public SelectListItem[] BOProducts()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Octa Plus", Value = "1" }, new SelectListItem() { Text = "Octa XS", Value = "2" } };
        }
        public SelectListItem[] YesNo()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Yes", Value = "1" }, new SelectListItem() { Text = "No", Value = "0" } };
        }
        public SelectListItem[] PaymentFrequency()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Single", Value = "1" }, new SelectListItem() { Text = "Installments", Value = "2" } };
        }
        public SelectListItem[] RefferedCategory()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Customer", Value = "1" }, new SelectListItem() { Text = "Billing Partner", Value = "2" }, new SelectListItem() { Text = "Channel Partner", Value = "3" }, new SelectListItem() { Text = "Cold Call", Value = "4" } };
        }
        public SelectListItem[] PaymentCleared()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Full Cleared", Value = "1" }, new SelectListItem() { Text = "Partial Cleared", Value = "2" }, new SelectListItem() { Text = "Full Pending", Value = "3" }, new SelectListItem() { Text = "Partial Pending", Value = "4" } };
        }
        public SelectListItem[] PaymentType()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Online", Value = "Online" }, new SelectListItem() { Text = "Cheque", Value = "Cheque" } };
        }
        public SelectListItem[] PaymentStatus()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Cleared", Value = "Cleared" }, new SelectListItem() { Text = "Pending", Value = "Pending" } };
        }
        public SelectListItem[] ConstitutionType()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Proprietary", Value = "Proprietary" }, new SelectListItem() { Text = "Partnership", Value = "Partnership" }, new SelectListItem() { Text = "Private Limited", Value = "Private Limited" } };
        }
        public SelectListItem[] SMSProvider()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Technocore", Value = "Technocore" }, new SelectListItem() { Text = "Vision HLT", Value = "Vision HLT" }, new SelectListItem() { Text = "Value First", Value = "Value First" } };
        }
        public SelectListItem[] WAProvider()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Technocore", Value = "Technocore" }, new SelectListItem() { Text = "Technocore Verified", Value = "Technocore Verified" }, new SelectListItem() { Text = "Pinnacle Verified", Value = "Pinnacle Verified" } };
        }
        public SelectListItem[] Percentage()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "% with", Value = "percentwith" }, new SelectListItem() { Text = "Fixed Percentage", Value = "fixedpercentage" } };

        }
        public SelectListItem[] VelocityAction()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Block Transaction", Value = "Block Transaction" }, new SelectListItem() { Text = "Report", Value = "Report" }, new SelectListItem() { Text = "Block Redeemption", Value = "Block Redeemption" } };
        }
        public SelectListItem[] BeforeAfter()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Before", Value = "Before" }, new SelectListItem() { Text = "After", Value = "After" } };

        }
        public SelectListItem[] PointsUsed()
        {
            return new SelectListItem[1] { new SelectListItem() { Text = "Points Used", Value = "Points Used" } };
        }
        public SelectListItem[] PointsNotUsed()
        {
            return new SelectListItem[1] { new SelectListItem() { Text = "Points Not Used", Value = "Points Not Used" }};
        }

        public SelectListItem[] RedemptionType()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "1st Time", Value = "firsttime" }, new SelectListItem() { Text = "Subsequent Times", Value = "Subsequent" } };

        }
        public SelectListItem[] TempleteType()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Implicit", Value = "Implicit" }, new SelectListItem() { Text = "Explicit", Value = "Explicit" } };

        }
    }
}