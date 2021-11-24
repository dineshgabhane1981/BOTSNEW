namespace BOTS_BL.Models.FeedBack
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedBackMaster")]
    public partial class FeedBackMaster
    {
        [Key]
        public int FeedBackId { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(5)]
        public string QuestionPoints { get; set; }

        [StringLength(5)]
        public string QuestionId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOA { get; set; }

        [StringLength(10)]
        public string Location { get; set; }

        [StringLength(10)]
        public string HowToKonwAbout { get; set; }

        public DateTime? DOJ { get; set; }

        [StringLength(10)]
        public string OutletId { get; set; }
    }
}
