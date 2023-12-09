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
        public List<tblCRDataset> lstCRDataset { get; set; }
        public SelectListItem[] SpendList()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Less than", Value = "Less than" }, new SelectListItem() { Text = "More than", Value = "More than" }, new SelectListItem() { Text = "Between", Value = "Between" } };
        }
        public SelectListItem[] RedeemedList()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Redeemed", Value = "Redeemed" }, new SelectListItem() { Text = "Not Redeemed", Value = "Not Redeemed" } };
        }
        public SelectListItem[] LessThanMoreThan()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Less than", Value = "Less than" }, new SelectListItem() { Text = "More than", Value = "More than" } };
        }
        public SelectListItem[] OnlyOnceDDL()
        {
            return new SelectListItem[5] { new SelectListItem() { Text = "High Spend Long time", Value = "High Spend Long time" }, new SelectListItem() { Text = "Low Spend Long time", Value = "Low Spend Long time" }, new SelectListItem() { Text = "High Spend Recent", Value = "High Spend Recent" }, new SelectListItem() { Text = "Low Spend Recent", Value = "Low Spend Recent" }, new SelectListItem() { Text = "Custom", Value = "Custom" } };
        }
        public SelectListItem[] InactiveDDL()
        {
            return new SelectListItem[8] { new SelectListItem() { Text = "All", Value = "All" }, new SelectListItem() { Text = "Within 30 days", Value = "Within 30 days" }, new SelectListItem() { Text = "31 to 60 days", Value = "31 to 60 days" }, new SelectListItem() { Text = "61 to 90 days", Value = "61 to 90 days" }, new SelectListItem() { Text = "91 to 180 days", Value = "91 to 180 days" }, new SelectListItem() { Text = "181 to 365 days", Value = "181 to 365 days" }, new SelectListItem() { Text = "More than a year", Value = "More than a year" }, new SelectListItem() { Text = "Custom", Value = "Custom" } };
        }

        public SelectListItem[] Gender()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Male", Value = "Male" }, new SelectListItem() { Text = "Female", Value = "Female" } };
        }
        public SelectListItem[] CumulativeFrequency()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Per Month", Value = "Per Month" }, new SelectListItem() { Text = "Cumulative", Value = "Cumulative" } };
        }
    }
}