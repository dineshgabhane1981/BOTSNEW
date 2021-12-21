namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_PointwsAndMessages
    {
        public int Id { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        public bool IsFeedbackPoints { get; set; }

        public int? AwardFeedbackPoints { get; set; }

        public string MsgToCustomer { get; set; }

        public string MsgNegativeFeedback { get; set; }

        public string MsgMissedFeedback { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
