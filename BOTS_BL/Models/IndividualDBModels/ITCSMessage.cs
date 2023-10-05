using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class ITCSMessage
    {
        public string GroupCode { get; set; }
        public string CSName { get; set; }
        public string WAAPILink { get; set; }
        public string BOTokenid { get; set; }
        public string OldEarnMinTxnAmt { get; set; }
        public string OldPointsExpiryMonths { get; set; }
        public string OldPointsAllocation { get; set; }
        public string OldPointsPercentage { get; set; }
        public string OldRevolvingStatus { get; set; }
        public string EarnMinTxnAmt { get; set; }
        public string PointsExpiryMonths { get; set; }
        public string PointsPercentage { get; set; }
        public string PointsAllocation { get; set; }
        public string Revolving { get; set; }
        public string OldBurnMinTxnAmt { get; set; }
        public string OldMinRedemptionPts { get; set; }
        public string OldMinRedemptionPtsFirstTime { get; set; }
        public string OldBurnInvoiceAmtPercentage { get; set; }
        public string OldBurnDBPointsPercentage { get; set; }
        public string BurnMinTxnAmt { get; set; }
        public string MinRedemptionPts { get; set; }
        public string MinRedemptionPtsFirstTime { get; set; }
        public string BurnInvoiceAmtPercentage { get; set; }
        public string BurnDBPointsPercentage { get; set; }
        public string Message { get; set; }
        public string FromName { get; set; }
    }
}
