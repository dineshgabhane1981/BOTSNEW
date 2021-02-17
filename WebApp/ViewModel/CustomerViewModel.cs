using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;


namespace WebApp.ViewModel
{
    public class CustomerViewModel
    {
        public tblGroupDetail objGroupData { get; set; }
        public tblModulesPayment objModulesPayment { get; set; }
        public List<SelectListItem> lstRetailCategory { get; set; }
        public List<SelectListItem> lstCity { get; set; }
        public List<SelectListItem> lstSourcedBy { get; set; }
        public List<SelectListItem> lstRMAssigned { get; set; }
        public List<SelectListItem> lstBillingPartner { get; set; }

        public SelectListItem[] PaymentTerms()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Annual", Value = "1" }, new SelectListItem() { Text = "Installments", Value = "2" } };
        }

        public SelectListItem[] Gateway()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "BOTS", Value = "1" }, new SelectListItem() { Text = "Customers", Value = "2" } };
        }
        public SelectListItem[] OtherDDL()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Separately Charged", Value = "1" }, new SelectListItem() { Text = "Part of PMF", Value = "2" } };
        }
    }
}