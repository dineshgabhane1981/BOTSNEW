namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPointsExpiryMaster")]
    public partial class tblPointsExpiryMaster
    {
        [Key]
        public long SlNo { get; set; }

        public DateTime? Datetime { get; set; }

        public int? CampaignMonth { get; set; }

        public int? CampaignYear { get; set; }

        public long? TotalMemberCount { get; set; }

        public long? UniqueMemberTxnCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsToBeExpired { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsRedeemed { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BusinessGenerated { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsExpired { get; set; }

        [StringLength(50)]
        public string BackEndStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BackEndEndDate { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }
    }
}
