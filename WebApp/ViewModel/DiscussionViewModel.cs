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
        public BOTS_TblDiscussion objDiscussion { get; set; }

        public List<SelectListItem> lstCallTypes { get; set; }
        public List<SelectListItem> lstCallSubTypes { get; set; }

        public SelectListItem[] CallMode()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Phone", Value = "Phone" }, new SelectListItem() { Text = "Zoom", Value = "Zoom" }, new SelectListItem() { Text = "Physical", Value = "Physical" } };
        }
        public SelectListItem[] Status()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Completed", Value = "Completed" }, new SelectListItem() { Text = "WIP", Value = "WIP" } };
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
    }
}