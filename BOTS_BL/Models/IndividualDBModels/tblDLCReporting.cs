namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCReporting")]
    public partial class tblDLCReporting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string ReferredByMobileNo { get; set; }

        [StringLength(100)]
        public string ReferredByName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReferredDate { get; set; }

        [Key]
        [StringLength(50)]
        public string ReferralMobileNo { get; set; }

        [StringLength(50)]
        public string ReferralName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ReferralBonusPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPointsRedeemed { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPointsExpired { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BonusPointsExpiryDate { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        public bool? ConvertedStatus { get; set; }

        public long? ReferralTotalTxnCount { get; set; }

        public long? ReferralTotalSpend { get; set; }
    }
}
