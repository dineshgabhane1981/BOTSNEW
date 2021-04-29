using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class DashboardMemberSegment
    {
        public long NoofMember_Total { get; set; }
        public long NoofMember_Repeat { get; set; }
        public long NoofMember_NeverRedeem { get; set; }
        public long NoofMember_OnlyOnce { get; set; }
        public long NoofMember_RecentlyEnrolled { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class DashboardOutletEnrolment
    {
        public string OutletName { get; set; }
        public long EnrollmentCount { get; set; }
    }

    public class DashboardPointsSummary
    {
        public long PointsIssued { get; set; }
        public long PointsRedeemed { get; set; }
        public long PointsExpired { get; set; }
        public long PointsCancelled { get; set; }
        public long PointsBalance { get; set; }
    }

    public class DashboardMemberWebPage
    {
        public long MemberBase { get; set; }
        public long ReferringBase { get; set; }
        public long ReferralGenerated { get; set; }
        public long ReferralTransacted { get; set; }
        public long ReferralTxnCount { get; set; }
        public long BusinessGenerated { get; set; }
        public long ProfileUpdatedCount { get; set; }
        public string MWPStatus { get; set; }
        public long MWPStatusCode { get; set; }
    }

    public class DashboardMemberSegmentTxnDB
    {
        public string TotalBase { get; set; }
        public string RepeatBase { get; set; }
        public string OnlyOnce { get; set; }
        public string NeverRedeem { get; set; }
        public string RecentlyEnrolled { get; set; }        
    }
    public class DashboardMemberSegmentTxn
    {
        public string Title { get; set; }
        public string Unit { get; set; }
        public string TotalBase { get; set; }
        public string RepeatBase { get; set; }
        public string OnlyOnce { get; set; }
        public string NeverRedeem { get; set; }
        public string RecentlyEnrolled { get; set; }
    }
}
