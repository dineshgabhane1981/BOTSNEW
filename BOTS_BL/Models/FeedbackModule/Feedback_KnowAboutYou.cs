namespace BOTS_BL.Models.FeedbackModule
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_KnowAboutYou
    {
        [Key]
        [StringLength(50)]
        public string KnowAboutYouId { get; set; }

        [StringLength(100)]
        public string KnowAboutYou { get; set; }
    }
}
