using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;

namespace WebApp.ViewModel
{
    public class FeedbackAuthorViewModel
    {
        public string GroupId { get; set; }
        public Feedback_Headings headings { get; set; }
        public Feedback_Questions questions { get; set; }
        public Feedback_PointsAndMessages PointsAndMessages { get; set; }
        public List<Feedback_Content> lstFeedbackData { get; set; }
        public List<SelectListItem> lstOutletDetail { get; set; }
        public List<OutletDetailsViewModel> lstOutletData { get; set; }
        public string outletJson { get; set; }
        public SelectListItem[] MandatoryOrNot()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Mandatory", Value = "1" }, new SelectListItem() { Text = "Non Mandatory", Value = "0" } };
        }
    }
}