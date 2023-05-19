namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCelebrationMaster")]
    public partial class tblCelebrationMaster
    {
        [Key]
        public long SlNo { get; set; }

        public DateTime? Datetime { get; set; }

        public int? CelebrationMonth { get; set; }

        public int? CelebrationYear { get; set; }

        [StringLength(50)]
        public string CelebrationType { get; set; }

        public long? TotalMemberCount { get; set; }

        public long? UniqueMemberTxnCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ConversionPercentage { get; set; }

        public long? TotalTxnCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BusinessGenerated { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPointsIssued { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPointsRedeemed { get; set; }

        [StringLength(50)]
        public string BackEndStatus { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BackEndEndDate { get; set; }
    }
}
