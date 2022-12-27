using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class DashboardSummaryViewModel
    {
        public List<MemberBaseAndTransaction> lstMemberBaseAndTransaction { get; set; }
        public List<BusinessGenerated> lstBusinessGenerated { get; set; }
        public TotalStats objTotalStats { get; set; }
        public KeyMetricsTillDate objKeyMetricsTillDate { get; set; }
        public List<KeyInfoForNextMonth> lstKeyInfoForNextMonth { get; set; }
        public List<FestivalDates> lstFestivalDates { get; set; }
        public PointSummary objPointSummary { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLogoURL { get; set; }
        public string ReportMonth { get; set; }
    }
}