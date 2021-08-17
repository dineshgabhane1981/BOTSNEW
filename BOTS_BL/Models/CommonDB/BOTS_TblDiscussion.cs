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

        [StringLength(50)]
        public string ContactNo { get; set; }

        [StringLength(50)]
        public string CallMode { get; set; }

        [StringLength(50)]
        public string CallType { get; set; }

        [StringLength(50)]
        public string SubCallType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FollowupDate { get; set; }

        public string Description { get; set; }

        public string ActionItems { get; set; }

        public DateTime AddedDate { get; set; }

        [Required]
        [StringLength(250)]
        public string AddedBy { get; set; }
    }
}
