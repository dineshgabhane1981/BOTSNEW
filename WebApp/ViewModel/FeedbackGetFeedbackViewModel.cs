﻿using BOTS_BL.Models;
using BOTS_BL.Models.FeedbackModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class FeedbackGetFeedbackViewModel
    {
        public string GroupId { get; set; }
        public string OutletId { get; set; }
        public string GroupName { get; set; }
        public string LogoUrl { get; set; }
        public bool IsExpiredOrStopped { get; set; }
        public List<SelectListItem> lstoutletlist { get; set; }
        public List<Feedback_Content> lstFeedbackData { get; set; }
        public Feedback_PointsAndMessages PointsAndMessages { get; set; }
        public List<SelectListItem> lstKnowAboutUs { get; set; }
        public List<SelectListItem> lstsalesRepresentive { get; set; }
        public List<Feedback_Report> lstfeedbackreport { get; set; }
        public SelectListItem[] Gender()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "Please Select", Value = "0" }, new SelectListItem() { Text = "Male", Value = "1" }, new SelectListItem() { Text = "Female", Value = "2" } };
        }
    }
}