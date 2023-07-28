namespace BOTS_BL.Models.FeedbackModule
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_ContentMaster
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Section { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int? TypeId { get; set; }

        public string Text { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool IsDisplay { get; set; }

        public int? IsMandatory { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }
    }
}
