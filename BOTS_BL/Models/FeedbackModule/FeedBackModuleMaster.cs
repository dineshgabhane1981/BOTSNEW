namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedBackModuleMaster")]
    public partial class FeedBackModuleMaster
    {
        [Key]
        public int FeedbackId { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(10)]
        public string OutletId { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(5)]
        public string QuestionId { get; set; }

        [StringLength(5)]
        public string QuestionPoints { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOA { get; set; }

        [StringLength(10)]
        public string HowToKnowAbout { get; set; }

        public DateTime? AddedDate { get; set; }
    }
}