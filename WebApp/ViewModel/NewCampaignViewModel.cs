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
        public SelectListItem[] Language()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Hindi", Value = "Hindi" }, new SelectListItem() { Text = "Marathi", Value = "Marathi" }, new SelectListItem() { Text = "Kannada", Value = "Kannada" }, new SelectListItem() { Text = "Telugu", Value = "Telugu" } };
        }
        public SelectListItem[] CampaignSubtype()
        {
            return new SelectListItem[8] { new SelectListItem() { Text = "Only Festival Wish", Value = "Only Festival Wish" },
                new SelectListItem() { Text = "Festival wish with a store offer", Value = "Festival wish with a store offer" },
                new SelectListItem() { Text = "No offer, only communication", Value = "No offer, only communication" },
                new SelectListItem() { Text = "Discount Offer", Value = "Discount Offer" },
            new SelectListItem() { Text = "Free gift offer", Value = "Free gift offer" },
            new SelectListItem() { Text = "Only redemption communication", Value = "Only redemption communication" },
            new SelectListItem() { Text = "Only engagement", Value = "Only engagement" },
            new SelectListItem() { Text = "Reminder about something", Value = "Reminder about something" }};
        }
    }
}