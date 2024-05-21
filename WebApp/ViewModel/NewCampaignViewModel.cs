using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class NewCampaignViewModel
    {
        public List<SelectListItem> lstOutlet { get; set; }
        public string SMSVendor { get; set; }
        public string SMSBalance { get; set; }
        public List<SelectListItem> lstDataset { get; set; }
        public string dummyValue { get; set; }

        public SelectListItem[] BaseType()
        {
            return new SelectListItem[5] { new SelectListItem() { Text = "All", Value = "1" }, new SelectListItem() { Text = "Member Base", Value = "2" }, new SelectListItem() { Text = "Bulk Upload", Value = "3" }, new SelectListItem() { Text = "Points Based", Value = "4" }, new SelectListItem() { Text = "Slicer Dataset", Value = "5" } };
        }
        public SelectListItem[] LessAndGreater()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Less Than", Value = "1" }, new SelectListItem() { Text = "Greater Than", Value = "2" }, new SelectListItem() { Text = "Equal To", Value = "3" }, new SelectListItem() { Text = "Points Range", Value = "4" } };
        }
    }
}