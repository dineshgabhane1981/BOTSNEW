using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOTS_BL.Models
{
    public class MemberData
    {
        public string MemberName { get; set; }
        public string MobileNo { get; set; }
        public string OldMobileNo { get; set; }
        public string CardNo { get; set; }
        public decimal? PointsBalance { get; set; }        
        public string EnrolledOutletName { get; set; }
        public string EnrolledOn { get; set; }      
        public string CustomerId { get; set; }
        public bool DisableSMSWAPromo { get; set; }
        public bool DisableSMSWATxn { get; set; }
    }

    public class GroupData
    {
        //public string GroupId { get; set; }
        public string RMAssignedName { get; set; }
    }
    public class Earndata
    {   
        public decimal? EarnMinTxnAmt { get; set; }
        public int? PointsExpiryMonths { get; set; }
        public decimal? PointsAllocation { get; set; }
        public decimal? PointsPercentage { get; set; }
    }
    public class BurnData
    {
        public string GroupId { get; set; }
        public decimal? BurnMinTxnAmt { get; set; }
        public decimal? MinRedemptionPts { get; set; }
        public decimal? MinRedemptionPtsFirstTime { get; set; }
        public decimal? BurnInvoiceAmtPercentage { get; set; }
        public decimal? BurnDBPointsPercentage { get; set; }
        
    }

}