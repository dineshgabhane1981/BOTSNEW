using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class FeedbackDashboardViewModel
    {
        public Feedback_PointsAndMessages feedbackConfig {get;set;}
        public bool SRChart { get; set; }
        public List<SelectListItem> lstOutlet { get; set; }
    }
}