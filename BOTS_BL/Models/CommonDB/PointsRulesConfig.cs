using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class PointsRulesConfig
    {
             
    }
    public class PointsRulesEarnConfig
    {
        public decimal PointsAllocation { get; set; }
        public long PointsExpiryMonths { get; set; }
        public string Type { get; set; }
        public decimal EarnMinTxnAmt { get; set; }
        public decimal MinTxnAmt { get; set; }
        public decimal MaxPointsEarned { get; set; }
        public string RuleType { get; set; }
        public string ProductRuleType { get; set; }
        public decimal PointsPercentage { get; set; }
        public string EarnBlockon { get; set; }
        public string BlockCodes { get; set; }
    }
    public class PointsRulesBurnConfig
    {
        public decimal MinRedemptionPtsFirstTime { get; set; }
        public decimal MinRedemptionPts { get; set; }
        public decimal BurnMinTxnAmt { get; set; }
        public decimal BurnInvoiceAmtPercentage { get; set; }
        public string EarnFullWhileBurnFlag { get; set; }        
        public string BurnBlockon { get; set; }
        public string BlockCodes { get; set; }
    }

    public class SMSConfig
    {
        public string Enrolment { get; set; }
        public string Earn { get; set; }
        public string Burn { get; set; }
        public string CancelEarn { get; set; }
        public string CancelBurn { get; set; }
        public string OTP { get; set; }
        public string BalanceInquiry { get; set; }
        public string BalanceInquiryNew { get; set; }
        //public string BalanceInquiry { get; set; }
    }
    public class WAConfig
    {
        public string Enrolment { get; set; }
        public string Earn { get; set; }
        public string Burn { get; set; }
        public string CancelEarn { get; set; }
        public string CancelBurn { get; set; }
        public string OTP { get; set; }
        public string BalanceInquiry { get; set; }
        public string BalanceInquiryNew { get; set; }
       // public string BalanceInquiry { get; set; }
    }

}
