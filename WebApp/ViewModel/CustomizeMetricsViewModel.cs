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
        public string dummyCategory { get; set; }
        public string dummySubCategory { get; set; }
        public string dummyProduct { get; set; }
        public List<SelectListItem> lstCategory { get; set; }
        public List<SelectListItem> lstSubCategory { get; set; }
        public List<SelectListItem> lstProduct { get; set; }
        public List<SelectListItem> lstBrands { get; set; }
        public List<SelectListItem> lstOutlets { get; set; }
        public List<SelectListItem> lstTiers { get; set; }
        public List<SelectListItem> lstDLCSource { get; set; }
        public SelectListItem[] SpendList()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Less than", Value = "1" }, new SelectListItem() { Text = "More than", Value = "2" }, new SelectListItem() { Text = "Between", Value = "3" } };
        }
        public SelectListItem[] RedeemedList()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Redeemed", Value = "1" }, new SelectListItem() { Text = "Not Redeemed", Value = "2" } };
        }
        public SelectListItem[] LessThanMoreThan()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Less than", Value = "1" }, new SelectListItem() { Text = "More than", Value = "2" } };
        }
        public SelectListItem[] OnlyOnceDDL()
        {
            return new SelectListItem[6] { new SelectListItem() { Text = "All", Value = "1" }, new SelectListItem() { Text = "High Spend Long time", Value = "2" }, new SelectListItem() { Text = "Low Spend Long time", Value = "3" }, new SelectListItem() { Text = "High Spend Recent", Value = "4" }, new SelectListItem() { Text = "Low Spend Recent", Value = "5" }, new SelectListItem() { Text = "Custom", Value = "6" } };
        }
        public SelectListItem[] InactiveDDL()
        {
            return new SelectListItem[8] { new SelectListItem() { Text = "All", Value = "1" }, new SelectListItem() { Text = "Within 30 days", Value = "2" }, new SelectListItem() { Text = "31 to 60 days", Value = "3" }, new SelectListItem() { Text = "61 to 90 days", Value = "4" }, new SelectListItem() { Text = "91 to 180 days", Value = "5" }, new SelectListItem() { Text = "181 to 365 days", Value = "6" }, new SelectListItem() { Text = "More than a year", Value = "7" }, new SelectListItem() { Text = "Custom", Value = "8" } };
        }

        public SelectListItem[] Gender()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Male", Value = "1" }, new SelectListItem() { Text = "Female", Value = "2" }};
        }
        public SelectListItem[] CumulativeFrequency()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Per Month", Value = "PerMonth" }, new SelectListItem() { Text = "Cumulative", Value = "Cumulative" } };
        }
    }
}