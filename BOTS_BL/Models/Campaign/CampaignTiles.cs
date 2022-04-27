using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Models
{
    public class CampaignTiles
    {
        public decimal? Celebration { get; set; }
        public decimal? PointsExpiry { get; set; }
        public decimal? Campaigns { get; set; }
        public decimal? SMSBlasts { get; set; }
        public decimal? Inactive { get; set; }
        public List<SelectListItem> lstMonth { get; set; }
        public List<SelectListItem> lstYear { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
    public class CampaignCelebrations
    {
        public string Celebration { get; set; }
        public long? Counts { get; set; }
        public long? UniqueMemberTransacted { get; set; }
        public long? BonusPointsIssued { get; set; }
        public long? BonusPointsRedeemed { get; set; }
        public decimal? RedemptionRate { get; set; }
        public long? BusinessGenerated { get; set; }
        public decimal? Conversion { get; set; }
    }

    public class CampaignCelebrationsData
    {
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public string CelebrationDate { get; set; }
        public string TxnDate { get; set; }
        public string NoOfTxn { get; set; }
        public long? TotalSpend { get; set; }
        public long? PointsRedeemed { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class CampaignPointsExpiry
    {
        public string Element { get; set; }
        public long? Counts { get; set; }
        public long? PointsToBeExpired { get; set; }
        public long? UniqueMemberTransacted { get; set; }
        public long? PointsRedeemed { get; set; }
        public decimal? RedemptionRate { get; set; }
        public long? BusinessGenerated { get; set; }
        public decimal? Conversion { get; set; }
    }
    public class CampaignInactive
    {
        public string Element { get; set; }
        public long? Counts { get; set; }
        public long? UniqueMemberTransacted { get; set; }
        public long? BusinessGenerated { get; set; }
        public decimal? Conversion { get; set; }
    }
    public class CampaignInactiveData
    {
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string InActiveDateStr { get; set; }
        public string Status { get; set; }
        public DateTime? TxnDate { get; set; }
        public string TxnDateStr { get; set; }
        public long? TotalSpend { get; set; }
        public long? PointsRedeemed { get; set; }
    }

    public class Campaign
    {
        public string CampaignType { get; set; }
        public string CampaignName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long? CampaignMemberCount { get; set; }
        public string Status { get; set; }
        public string CampaignId { get; set; }
        public long? UniqueMemberTransacted { get; set; }
        public long? Transactions { get; set; }
        public long? BizGenerated { get; set; }
        public decimal? Conversion { get; set; }
    }
   
    public class CampaignSecond
    {
        public string CampaignId { get; set; }
        public string Element { get; set; }
        public string CampaignBase { get; set; }
        public string ControlBase { get; set; }
    }
    public class CampaignThird
    {
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public long? TxnCount { get; set; }
        public long? TotalBiz { get; set; }
        public string Status { get; set; }
    }
    public class CampaignSMSBlastFirst
    {
        public string CampaignType { get; set; }
        public string CampaignName { get; set; }
        public string SendDate { get; set; }        
        public long? CampaignMemberCount { get; set; }
        public string Status { get; set; }
        public string CampaignId { get; set; }
        public long? UniqueMemberTransacted { get; set; }
        public long? Transactions { get; set; }
        public long? BizGenerated { get; set; }
        public decimal? Conversion { get; set; }

    }

    public class OutletData
    {
        public string OutletName { get; set; }
        public int EnrollmentCount { get; set; }
    }
    public class CustCount
    {
        public int CustCountALL { get; set; }
        public int CustFiltered { get; set; }
    }
}
