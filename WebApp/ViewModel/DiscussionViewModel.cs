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
        public List<BOTS_TblDiscussion> lstDiscussions { get; set; }
        public BOTS_TblDiscussion objDiscussion { get; set; }

        public List<SelectListItem> lstCallTypes { get; set; }
        public List<SelectListItem> lstCallSubTypes { get; set; }

        public SelectListItem[] CallMode()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Phone", Value = "Phone" }, new SelectListItem() { Text = "Zoom", Value = "Zoom" } };
        }
    }
}