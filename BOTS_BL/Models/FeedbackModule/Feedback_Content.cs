namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_Content
    {
        public int Id { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string Section { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int? TypeId { get; set; }

        public string Text { get; set; }

        public bool IsDisplay { get; set; }

        [StringLength(50)]
        public string IsMandatory { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }
        
        [NotMapped]
        public string ImagePath { get; set; }
    }
}
