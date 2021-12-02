namespace BOTS_BL.Models.SalesLead
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SALES_tblLeadTracking
    {
        [Key]
        public int TrackingId { get; set; }

        public int LeadId { get; set; }

        [StringLength(50)]
        public string ContactType { get; set; }

        [StringLength(250)]
        public string SpokeWith { get; set; }

        [StringLength(50)]
        public string LeadStatus { get; set; }

        [StringLength(50)]
        public string MeetingType { get; set; }

        public DateTime? FollowupDate { get; set; }

        [StringLength(50)]
        public string BillingPartner { get; set; }

        public int? NoOfOutlet { get; set; }

        public bool? EcomIntegration { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string AlternateNo { get; set; }

        [StringLength(150)]
        public string EmailId { get; set; }

        [StringLength(250)]
        public string AuthorizedPerson { get; set; }

        [StringLength(50)]
        public string APMobileNo { get; set; }

        [StringLength(50)]
        public string PriceQuoted { get; set; }

        [StringLength(50)]
        public string LeadSource { get; set; }

        [StringLength(250)]
        public string LeadSourceName { get; set; }

        public string Comments { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AssignedLead { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }
    }

    public class LeadTracking
    {
        public DateTime? AddedDate { get; set; }
        public string AddedDateStr { get; set; }
        public string AddedByName { get; set; }
        public string ContactType { get; set; }
        public string LeadStatus { get; set; }
        public string MeetingType { get; set; }
        public string Comments { get; set; }
    }
}
