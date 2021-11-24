namespace BOTS_BL.Models.FeedBack
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedBackQuestionMaster")]
    public partial class FeedBackQuestionMaster
    {
        [Key]
        [StringLength(10)]
        public string QuestionId { get; set; }

        public string Question { get; set; }

        public DateTime? Date { get; set; }

        
    }
}
