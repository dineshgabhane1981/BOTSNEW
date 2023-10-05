using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public string Tier { get; set; }
        public string LastTxnDate { get; set; }
    }
    public class SlabData
    {
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public string Tier { get; set; }
        public DateTime? LastTxnDate { get; set; }
        public Int64? AvlPts { get; set; }
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
        public bool Revolving { get; set; }
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
    public class OTPData
    {
        public string GroupId { get; set; }
        public string OutletId { get; set; }
        public string OTP { get; set; }
    }

    public class CommonOTPDetails
    { 
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string LoginLevel { get; set; }
        public string LoginType { get; set; }
        public string Status { get; set; }
        public string GroupId { get; set; }
    }




    public class DemographicData
    {
        public string GroupId { get; set; }
        public string MobileNo { get; set; }
        public string AlternateNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public string DOA { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string StoreAnniversary { get; set; }        
    }

}