namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback_Headings
    {
        public int Id { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(500)]
        public string HomeHeading1 { get; set; }

        [StringLength(500)]
        public string HomeHeading2 { get; set; }

        [StringLength(500)]
        public string HomeHeading3 { get; set; }

        [StringLength(500)]
        public string HomeRepresentative { get; set; }

        [StringLength(500)]
        public string QuestionsHeading1 { get; set; }

        [StringLength(500)]
        public string QuestionsHeading2 { get; set; }

        [StringLength(500)]
        public string OtherInfoHeading1 { get; set; }

        [StringLength(500)]
        public string OtherInfoHeading2 { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
