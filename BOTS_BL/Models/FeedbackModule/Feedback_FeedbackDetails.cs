namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_FeedbackConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GroupId { get; set; }

        [StringLength(50)]
        public string Fees { get; set; }

        [StringLength(50)]
        public string PaymentMode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public DateTime? StoppedDate { get; set; }

        public string StoppedReason { get; set; }

        public DateTime? RenewDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }
        public string Status { get; set; }
    }

    public class FeedbackActiveGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Amount { get; set; }
        public string ExpiringInDays { get; set; }
    }

    public class FeedbackDeActivatedGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Amount { get; set; }
        public DateTime? StoppedDate { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
