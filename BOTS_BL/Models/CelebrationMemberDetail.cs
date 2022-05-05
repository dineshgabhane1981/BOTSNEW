namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CelebrationMemberDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [StringLength(200)]
        public string CampaignType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPoints { get; set; }
    }
}
