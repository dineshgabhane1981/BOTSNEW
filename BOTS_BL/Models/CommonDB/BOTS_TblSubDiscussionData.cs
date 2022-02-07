namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblSubDiscussionData
    {
        [Key]
        public int SubDiscussionId { get; set; }

        public int DiscussionId { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FollowupDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public DateTime? AddedDate { get; set; }
    }

    public class SubDiscussionData
    {
        public int SubDiscussionId { get; set; }
        public int DiscussionId { get; set; }
       // public string GroupId { get; set; }
        public string FollowupDate { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public string Status { get; set; }
        public string UpdatedDate { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
