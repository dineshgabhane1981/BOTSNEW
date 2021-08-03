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
        public List<SelectListItem> lstCity { get; set; }
        public List<SelectListItem> lstRetailCategory { get; set; }
        public List<SelectListItem> lstSourcedBy { get; set; }
        public List<SelectListItem> lstRMAssigned { get; set; }
        public List<SelectListItem> lstBillingPartner { get; set; }
        public List<SelectListItem> lstAllGroups { get; set; }
        public SelectListItem[] BOProducts()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Octa Plus", Value = "1" }, new SelectListItem() { Text = "Octa XS", Value = "2" } };
        }
        public SelectListItem[] YesNo()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Yes", Value = "1" }, new SelectListItem() { Text = "No", Value = "0" } };
        }
        public SelectListItem[] RefferedCategory()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Customer", Value = "1" }, new SelectListItem() { Text = "Billing Partner", Value = "2" }, new SelectListItem() { Text = "Channel Partner", Value = "3" }, new SelectListItem() { Text = "Cold Call", Value = "4" }};
        }
    }
}