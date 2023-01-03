using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class MemberBaseAndTransaction
    {
        public string MemberType { get; set; }
        public int BaseCount { get; set; }
        public int TxnCount { get; set; }
        public decimal? BizGen { get; set; }
    }
    public class BusinessGenerated
    {
        public string Elements { get; set; }
        public int BaseCount { get; set; }
        public int TxnCount { get; set; }
        public decimal BizGen { get; set; }
    }
    public class TotalStats
    {
        public Int64 TotalBiz { get; set; }
        public Int64 LoyaltyBiz { get; set; }        
        public string LoyaltyPercentage { get; set; }
    }
    public class KeyMetricsTillDate
    {
        public decimal RedemptionRate { get; set; }
        public decimal RedeemToInv { get; set; }
        public Int64 InactiveBase { get; set; }
        public Int64 OnlyOnceBase { get; set; }
        public Int64 NonRedeemBase { get; set; }
        public Int64 NonTransactingBase { get; set; }
    }
    public class KeyInfoForNextMonth
    {
        public string Elements { get; set; }
        public int BaseCount { get; set; }
    }
    public class FestivalDates
    {
        public string Festival { get; set; }
        public DateTime Date { get; set; }
    }
    public class PointSummary
    {
        public decimal Issued { get; set; }
        public decimal Redeemed { get; set; }
        public decimal Expired { get; set; }
        public decimal Available { get; set; }
    }
}
