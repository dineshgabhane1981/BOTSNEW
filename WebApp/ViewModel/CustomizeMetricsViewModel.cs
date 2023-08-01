using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class CustomizeMetricsViewModel
    {
        public string Spend { get; set; }
        public string TxnCount { get; set; }
        public string Redeemed { get; set; }
        public string PointsBalance { get; set; }
        public SelectListItem[] SpendList()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Less than", Value = "1" }, new SelectListItem() { Text = "More than", Value = "2" }, new SelectListItem() { Text = "Between", Value = "3" } };
        }
        public SelectListItem[] RedeemedList()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Redeemed", Value = "1" }, new SelectListItem() { Text = "Not Redeemed", Value = "2" } };
        }
    }
}