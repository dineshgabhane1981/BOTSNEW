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
        public long PointsExpiryVariableDate { get; set; }
        public string Type { get; set; }
        public decimal MinTxnAmt { get; set; }
        public decimal MaxPointsEarned { get; set; }
        public string PointsProductORBase { get; set; }
        public decimal PointsPrecentage { get; set; }
        public string EarnBlockon { get; set; }
        public string BlockCodes { get; set; }
    }
    public class PointsRulesBurnConfig
    {
        public decimal MinThresholdPointsFirstTime { get; set; }
        public decimal MinThresholdPointsEveryTime { get; set; }
        public decimal MinTxnAmt { get; set; }
        public decimal PointsofInvoiceAmt { get; set; }
        public string EarnFullWhileBurnFlag { get; set; }        
        public string BurnBlockon { get; set; }
        public string BlockCodes { get; set; }
    }

    public class SMSConfig
    {
        public string Enrollment { get; set; }
        public string EnrollmentAndEarn  { get; set; }
        public string Earn { get; set; }
        public string OTP { get; set; }
        public string Burn { get; set; }
        public string CancelEarn  { get; set; }
        public string CancelBurn { get; set; }
        public string AnyCancel  { get; set; }
        public string BalanceInquiry { get; set; }
    }
    public class WAConfig
    {
        public string Enrollment { get; set; }
        public string EnrollmentAndEarn { get; set; }
        public string Earn { get; set; }
        public string OTP { get; set; }
        public string Burn { get; set; }
        public string CancelEarn { get; set; }
        public string CancelBurn { get; set; }
        public string AnyCancel { get; set; }
        public string BalanceInquiry { get; set; }
    }

}
