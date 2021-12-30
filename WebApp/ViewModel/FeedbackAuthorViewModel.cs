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
        public string OutletId { get; set; }
        public string LogoUrl { get; set; }
        public Feedback_Headings headings { get; set; }
        public Feedback_Questions questions { get; set; }
        public Feedback_PointsAndMessages PointsAndMessages { get; set; }
        public List<Feedback_Content> lstFeedbackData { get; set; }
        public List<SelectListItem> lstOutletDetail { get; set; }
        public List<SelectListItem> lstKnowAboutUs { get; set; }
        public List<OutletDetailsViewModel> lstOutletData { get; set; }
        public string outletJson { get; set; }
        public SelectListItem[] MandatoryOrNot()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Mandatory", Value = "Mandatory" }, new SelectListItem() { Text = "Non Mandatory", Value = "Non Mandatory" } };
        }
        public SelectListItem[] MandatoryOrNotNew()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Mandatory", Value = "Mandatory" }, new SelectListItem() { Text = "Non Mandatory", Value = "Non Mandatory" } };
        }
        public SelectListItem[] Gender()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Select", Value = "0" }, new SelectListItem() { Text = "Male", Value = "1" }, new SelectListItem() { Text = "Female", Value = "2" } };
        }
    }
}