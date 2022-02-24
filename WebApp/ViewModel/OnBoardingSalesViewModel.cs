using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

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
        public List<SelectListItem> lstCity { get; set; }
        public List<SelectListItem> lstRetailCategory { get; set; }
        public List<SelectListItem> lstSourcedBy { get; set; }
        public List<SelectListItem> lstRMAssigned { get; set; }
        public List<SelectListItem> lstBillingPartner { get; set; }
        public List<SelectListItem> lstAllGroups { get; set; }
        public List<SelectListItem> lstRefferedCategory { get; set; }
        public List<SelectListItem> lstStates { get; set; }
        public List<SelectListItem> lstNewOutlets { get; set; }

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
            return new SelectListItem[4] { new SelectListItem() { Text = "Customer", Value = "1" }, new SelectListItem() { Text = "Billing Partner", Value = "2" }, new SelectListItem() { Text = "Channel Partner", Value = "3" }, new SelectListItem() { Text = "Cold Call", Value = "4" }};
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
    }
}