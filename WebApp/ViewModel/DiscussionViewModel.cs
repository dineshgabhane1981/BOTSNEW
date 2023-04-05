using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;

namespace WebApp.ViewModel
{
    public class DiscussionViewModel
    {
        public List<DiscussionDetails> lstDiscussions { get; set; }
        public List<DiscussionDetails> lstFollowUpsDiscussions { get; set; }
        public BOTS_TblDiscussion objDiscussion { get; set; }
        
        public List<SelectListItem> lstCallTypes { get; set; }
        public List<SelectListItem> lstCallSubTypes { get; set; }

        public string dept { get; set; }

        public string prior { get; set; }

        public string Member { get; set; }

        //public HttpPostedFileBase File { get; set; }

        public string File { get; set; }

        public string FileName { get; set; }


        public SelectListItem[] CallMode()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Phone", Value = "Phone" }, new SelectListItem() { Text = "Zoom", Value = "Zoom" }, new SelectListItem() { Text = "Physical", Value = "Physical" } };
        }
        public SelectListItem[] Status()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Completed", Value = "Completed" }, new SelectListItem() { Text = "WIP", Value = "WIP" } };
        }
        public SelectListItem[] Departments()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Customer Success", Value = "Customer Success" }, new SelectListItem() { Text = "Finance", Value = "Finance" }, new SelectListItem() { Text = "Operations", Value = "Operations" }, new SelectListItem() { Text = "Technology", Value = "Technology" }};
        }
       
        public SelectListItem[] CustomerType()
        {
            return new SelectListItem[5] 
            { new SelectListItem() { Text = "Main Owner ", Value = "MainOwner" },
                new SelectListItem() { Text = "Office Manager", Value = "OfficeManager" },
                new SelectListItem() { Text = "Store Manager ", Value = "StoreManager " },
                new SelectListItem() { Text = "Cashier", Value = "Cashier" },
                 new SelectListItem() { Text = "Another Owner", Value = "AnotherOwner" }
            };
        }
        public SelectListItem[] Priority()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Critical", Value = "Critical" }, new SelectListItem() { Text = "High", Value = "High" }, new SelectListItem() { Text = "Medium", Value = "Medium" }, new SelectListItem() { Text = "Low", Value = "Low" } };
        }
        public SelectListItem[] RequestType()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Valid", Value = "Valid" }, new SelectListItem() { Text = "Invalid", Value = "Invalid" } };
        }
    }
}