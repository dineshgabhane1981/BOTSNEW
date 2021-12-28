using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModel
{
    public class FeedbackGetFeedbackViewModel
    {
        public List<Feedback_Content> lstHomeHeading { get; set; }
        public List<Feedback_Content> lstFeedbackHeading { get; set; }
        public List<Feedback_Content> lstFeedbackQuestion { get; set; }
        public List<Feedback_Content> lstOtherInfoHeading { get; set; }
        public List<Feedback_Content> lstOtherInfoQuestion { get; set; }
    }
}