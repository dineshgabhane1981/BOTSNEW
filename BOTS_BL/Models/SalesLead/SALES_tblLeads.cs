namespace BOTS_BL.Models.SalesLead
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SALES_tblLeads
    {
        [Key]
        public int LeadId { get; set; }

        [Required]
        [StringLength(250)]
        public string BusinessName { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Product { get; set; }

        [StringLength(50)]
        public string BillingPartner { get; set; }

        public bool? EcomIntegration { get; set; }

        public int? NoOfOutlet { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public int? Pincode { get; set; }

        [StringLength(50)]
        public string ContactType { get; set; }

        [StringLength(250)]
        public string SpokeWith { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string AlternateNo { get; set; }

        [StringLength(150)]
        public string EmailId { get; set; }

        [StringLength(250)]
        public string AuthorizedPerson { get; set; }

        [StringLength(50)]
        public string APMobileNo { get; set; }

        [StringLength(50)]
        public string LeadStatus { get; set; }

        [StringLength(50)]
        public string PriceQuoted { get; set; }

        [StringLength(50)]
        public string MeetingType { get; set; }

        public DateTime? FollowupDate { get; set; }

        [StringLength(50)]
        public string LeadSource { get; set; }

        [StringLength(250)]
        public string LeadSourceName { get; set; }

        public string Comments { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string AssignedLead { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }
    }
    public class SalesLead
    {
        public int LeadId { get; set; }

        public string BusinessName { get; set; }

        public string Category { get; set; }

        public string Product { get; set; }

        public string BillingPartner { get; set; }

        public bool? EcomIntegration { get; set; }

        public int? NoOfOutlet { get; set; }

        public string Address { get; set; }

        public string State { get; set; }

        public string City { get; set; }
        public int? Pincode { get; set; }
        public string ContactType { get; set; }

        public string SpokeWith { get; set; }

        public string MobileNo { get; set; }

        public string AlternateNo { get; set; }

        public string EmailId { get; set; }

        public string AuthorizedPerson { get; set; }

        public string APMobileNo { get; set; }

        public string LeadStatus { get; set; }

        public string PriceQuoted { get; set; }

        public string MeetingType { get; set; }

        public DateTime? FollowupDate { get; set; }
        public string LeadSource { get; set; }
        public string LeadSourceName { get; set; }
        public string Comments { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AssignedLead { get; set; }
        public string CityName { get; set; }
        public string GroupId { get; set; }
        public string CustomerType { get; set; }
        public string SalesManagerName { get; set; }
        public long? noOfOutlet { get; set; }
    }

    public class SalesCount
    {
        public int? NoOfMeeting { get; set; }
        public int? NoOfSalesDone { get; set; }
        public long? NoofEnrolledOutlet { get; set; }
        public long? ratio { get; set; }       
        public int? octaplus { get; set; }
        public int? octaxs { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? SalesAvg { get; set; }
        public int? NoOfBillingpartner { get; set; }
        public List<salesCountDetails> lstSalesCountDetail { get; set; }


    }
    public class salesCountDetails
    {
        public int? LeadId { get; set; }
        public string SalesManager { get; set; }
        public string BusinessName { get; set; }
        public string Product { get; set; }
        public string BillingPartner { get; set; }
        public decimal? Amount { get; set; }
        public long? OutletName { get; set; }
    }
}
