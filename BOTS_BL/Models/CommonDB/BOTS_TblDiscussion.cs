namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblDiscussion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string SpokenTo { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(50)]
        public string CallMode { get; set; }

        public int CallType { get; set; }

        [StringLength(50)]
        public string SubCallType { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FollowupDate { get; set; }

        public string Description { get; set; }

        public string ActionItems { get; set; }

        public DateTime AddedDate { get; set; }

        [Required]
        [StringLength(250)]
        public string AddedBy { get; set; }

        [NotMapped]
        public string GroupName { get; set; }
    }


    public class DiscussionDetails
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public string SpokenTo { get; set; }
        public string ContactNo { get; set; }
        public string CallType { get; set; }
        public string Status { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string CallMode { get; set; }
        public string Description { get; set; }
        public string ActionItems { get; set; }
        public string AddedBy { get; set; }       
        
    }

}
