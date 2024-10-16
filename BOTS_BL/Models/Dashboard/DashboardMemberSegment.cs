﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class DashboardMemberSegment
    {
        public long TotalMember { get; set; }
        public long RepeatMember { get; set; }
        public long NeverRedeem { get; set; }
        public long OnlyOnce { get; set; }
        public long RecentlyEnrolled { get; set; }
        public long NonTransacted { get; set; }

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

    public class DashboardBulkUpload
    {
        public long TotalUpload { get; set; }
        public long UniqueTransacted { get; set; }
        public long TransactedCount { get; set; }
        public long BusinessGenerated { get; set; }
        public long PieChartYellow { get; set; }
        public decimal PieChartGreen { get; set; }
        public decimal PieChartTotalMemberConverted { get; set; }
    }

    public class DashboardRedemption
    {
        public long? RedeemedMembers { get; set; }
        public long? RedemptionTxnCount { get; set; }
        public long? RedeemedPoints { get; set; }
        public long? PointsValueRs { get; set; }
        public long? BusinessGenerated { get; set; }
        public decimal? RedeemToInvoice { get; set; }
        public long? PieChartYellowRedemptionRate { get; set; }
        public decimal? PieChartGreenRedemptionRate { get; set; }
        public long? PieChartYellowUniqueRedeemMember { get; set; }        
        public decimal? PieChartGreenUniqueMember { get; set; }
    }

    public class DashboardBizShared
    {
        public string MonthYear { get; set; }
        public long? FirstMemberTxn { get; set; }
        public long? RepeatMemberTxn { get; set; }
        public long? RedeemTxn { get; set; }
    }
}
