namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_Questions
    {
        public int Id { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(500)]
        public string FeedbackQuestion1 { get; set; }

        [StringLength(500)]
        public string FeedbackQuestion2 { get; set; }

        [StringLength(500)]
        public string FeedbackQuestion3 { get; set; }

        [StringLength(500)]
        public string FeedbackQuestion4 { get; set; }

        [StringLength(500)]
        public string OtherInfoQuestion1 { get; set; }

        [StringLength(500)]
        public string OtherInfoQuestion2 { get; set; }

        [StringLength(500)]
        public string OtherInfoQuestion3 { get; set; }

        [StringLength(500)]
        public string OtherInfoQuestion4 { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool? IsOtherInfoEnabled { get; set; }
    }
}
