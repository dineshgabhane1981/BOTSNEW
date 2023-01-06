using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class CustomerPaymentsViewModel
    {
        public List<SelectListItem> objGroupData { get; set; }
        public tblRenewalData objRenewalData { get; set; }
        public SelectListItem[] PaymentType()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Renewal", Value = "Renewal" }, new SelectListItem() { Text = "Verified WA", Value = "VerifiedWA" } };
        }
        public SelectListItem[] Frequency()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Annual", Value = "Annual" }, new SelectListItem() { Text = "Half Yearly", Value = "Half Yearly" } };
        }
    }
}