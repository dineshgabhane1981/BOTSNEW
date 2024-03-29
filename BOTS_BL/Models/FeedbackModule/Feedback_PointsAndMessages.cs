namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_PointsAndMessages
    {
        public int Id { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        public bool IsAddRepresentative { get; set; }

        public string RepresentativesList { get; set; }

        public bool IsFeedbackPoints { get; set; }

        public int AwardFeedbackPoints { get; set; }
        public bool IsOtherInfoShow { get; set; }
        public bool IsPositiveMessage { get; set; }
        public string MsgToCustomer { get; set; }

        public string MsgNegativeFeedback { get; set; }

        public bool IsMsgMissedFeedback { get; set; }

        public string MsgMissedFeedback { get; set; }
        public string PointsConfig { get; set; }
        public bool IsAudio { get; set; }
        public string AudioMessageText { get; set; }
        public bool IsGoogleReview { get; set; }
        public string GoogleReviewLink { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public bool IsCustomField { get; set; }
        public string CustomFieldText { get; set; }
    }
}
