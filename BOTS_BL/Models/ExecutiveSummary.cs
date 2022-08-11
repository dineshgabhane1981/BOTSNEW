using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class ExecutiveSummary
    {
        //public long NoofMember_Total { get; set; }
        // public long NoofMember_Recurring { get; set; }
        // public long NoofMember_NeverRedeem { get; set; }
        // public long NoofMember_OnlyOnce { get; set; }
        // public long AvgEnrolPerDay_Total { get; set; }
        // public long NonTransactedMember { get; set; }
        // public long NoofTxn_Total { get; set; }
        // public long NoofTxn_Recurring { get; set; }
        // public long NoofTxn_NeverRedeem { get; set; }
        // public long NoofTxn_OlnyOnce { get; set; }
        // public long TxnPerMember_Total { get; set; }
        // public long TxnPerMember_Recurring { get; set; }
        // public long TxnPerMember_NeverRedeem { get; set; }
        // public long AvgDaysBtTxn_Total { get; set; }
        // public long AvgDayBtTxn_Recurring { get; set; }
        // public long AvgDayBtTxn_NeverRedeem { get; set; }
        // public long TotalSpend_Total { get; set; }
        // public long TotalSpend_Recurring { get; set; }
        // public long TotalSpend_NeverRedeem { get; set; }
        // public long TotalSpend_OnlyOnce { get; set; }
        // public long AvgBillSize_Total { get; set; }
        // public long AvgBillSize_Recurring { get; set; }
        // public long AvgBillSize_OnlyOnce { get; set; }
        // public long AvgBillSize_NeverRedeem { get; set; }
        // public long PerMemberSpend_Total { get; set; }
        // public long PerMemberSpend_Recurring { get; set; }
        // public long PerMemberSpend_NeverRedeem { get; set; }
        // public long PerMemberSpend_OnlyOnce { get; set; }
        // public decimal ActivationRate_Total { get; set; }
        // public decimal ActivationRate_Recurring { get; set; }
        // public decimal ActivationRate_NeverRedeem { get; set; }
        // public decimal ActivationRate_OnlyOnce { get; set; }  
        // public long NonActiveBase_Total { get; set; }
        // public long NonActiveBase_Recurring { get; set; }
        // public long NonActiveBase_NeverRedeem { get; set; }
        // public long NonActiveBase_OnlyOnce { get; set; }
        // public long PointsIssued_Total { get; set; }
        // public long PointsIssued_Recurring { get; set; }
        // public long PointsIssued_NeverRedeem { get; set; }
        // public long PointsIssued_OnlyOnce { get; set; }
        // public long PointsRedeem_Total { get; set; }
        // public long PointsRedeem_Recurring { get; set; }
        // public decimal RedemptionRate_Total { get; set; }
        // public decimal RedemptionRate_Recurring { get; set; }
        // public decimal AvgDaysTo1Redeem_Total { get; set; }
        // public decimal AvgDaysTo1Redeem_Recurring { get; set; }
        // public decimal RedeemToInvoice_Total { get; set; }
        // public decimal RedeemToInvoice_Recurring { get; set; }
        // public long PointsExpiry_Total { get; set; }        
        // public long PointsExpiry_Recurring { get; set; }
        // public long PointsExpiry_NeverRedeem { get; set; }
        // public long PointsExpiry_OnlyOnce { get; set; }
        // public long PointsBalance_Total { get; set; }
        // public long PointsBalance_Recurring { get; set; }
        // public long PointsBalance_NeverRedeem { get; set; }
        // public long PointsBalance_OnlyOnce { get; set; }
        // public decimal AvgBalPerMember_Total { get; set; }
        // public decimal AvgBalPerMember_Recurring { get; set; }
        // public decimal AvgBalPerMember_NeverRedeem { get; set; }
        // public decimal AvgBalPerMember_OnlyOnce { get; set; }        
        // public long ExpiryMemberCount_Total { get; set; }
        // public long ExpiryMemberCount_Recurring { get; set; }
        // public long ExpiryMemberCount_NeverRedeem { get; set; }
        // public long ExpiryMemberCount_OnlyOnce { get; set; }
        // public long ExpiryMemberPoints_Total { get; set; }
        // public long ExpiryMemberPoints_Recurring { get; set; }
        // public long ExpiryMemberPoints_NeverRedeem { get; set; }
        // public long ExpiryMemberPoints_OnlyOnce { get; set; }
        // public long NoofProfileUpdate { get; set; }
        // public long NoofReferee { get; set; }
        // public long NoofReferral { get; set; }
        // public long UniqueReferralTxn { get; set; }
        // public long TotalReferralTxn { get; set; }
        // public long TotalReferralBusniess { get; set; }
        // public long ReferralBonusPoints { get; set; }
        // public long ReferralBonusRedeem { get; set; }
        // public long ActiveUnusedReferralBonus { get; set; }
        // public long NoofUploadedBase { get; set; }
        // public long UniqueTxnForUploadedBase { get; set; }
        // public long UploadBaseTotalTxn { get; set; }
        // public long UploadBaseTotalAmt { get; set; }
        // public decimal UploadBaseATS { get; set; }        
        // public long TotalBiz { get; set; }
        // public long LoyaltyBiz { get; set; }
        // public decimal LoyaltyPercentageBiz { get; set; }
        // public long RecentlyEnrolled { get; set; }


        // public long ActiveBase_Recurring { get; set; }
        // public long ActiveBase_NeverRedeem { get; set; }
        // public long ActiveBase_OnlyOnce { get; set; }
        // public long PointsIssued { get; set; }
        // public long PointsRedeemed { get; set; }
        // public long PointsExpired { get; set; }
        // public long PointsCancelled { get; set; }
        // public long PointsBalance { get; set; }
        // public long? NoofTxn_RecentlyEnrolled { get; set; }
        // public long? TxnPerMember_RecentlyEnrolled { get; set; }
        // public long? AvgDayBtTxn_RecentlyEnrolled { get; set; }
        // public long? AvgBillSize_RecentlyEnrolled { get; set; }
        // public long? PerMemberSpend_RecentlyEnrolled { get; set; }
        // public long ActiveBase_RecentlyEnrolled { get; set; }
        // public long NonActiveBase_RecentlyEnrolled { get; set; }
        // public long RedeemToInvoice_RecentlyEnrolled { get; set; }
        public long ActiveBase_Total { get; set; }
        public long TotalBiz { get; set; }
        public long Redemption { get; set; }
        public long Referrals { get; set; }
        public long Campaign { get; set; }
        public long SMSBlastWA { get; set; }
        public long NewMWPRegistration { get; set; }
        public long LoyaltyBiz { get; set; }

        public string RenewalDate { get; set; }
        public int RemainingDaysForRenewal { get; set; }

        public List<OutletDetails> lstOutletDetails { get; set; }

    }

    public class OutletDetails
    {
        public string OutletName { get; set; }
        public int EnrollmentCount { get; set; }
    }
}
