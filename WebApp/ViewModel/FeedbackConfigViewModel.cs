using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;

namespace WebApp.ViewModel
{
    public class FeedbackConfigViewModel
    {
        public List<tblGroupDetail> lstNeverOptFor { get; set; }
        public List<FeedbackActiveGroup> lstActiveGroup { get; set; }
        public List<FeedbackDeActivatedGroup> lstDeActivatedGroup { get; set; }

        public SelectListItem[] PaymentMode()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Online", Value = "Online" }, new SelectListItem() { Text = "Cheque", Value = "Cheque" } };
        }
    }
    
}