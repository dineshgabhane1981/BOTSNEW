namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRuleMaster")]
    public partial class tblRuleMaster
    {
        [Key]
        public long RuleId { get; set; }

        [StringLength(50)]
        public string RuleName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsAllocation { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EarnMinTxnAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BurnMinTxnAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinRedemptionPts { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinRedemptionPtsFirstTime { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsPercentage { get; set; }

        public int? PointsExpiryMonths { get; set; }

        public bool? DisableEarn { get; set; }

        public bool? DisableBurn { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BurnInvoiceAmtPercentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BurnDBPointsPercentage { get; set; }

        public bool? Revolving { get; set; }

        [StringLength(50)]
        public string RuleType { get; set; }

        public bool? DisablePointsForDiscount { get; set; }

        public bool? RemoveDisProForPts { get; set; }

        [StringLength(50)]
        public string ProductRuleType { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [NotMapped]
        public decimal? OldEarnMinTxnAmt { get; set; }

        [NotMapped]
        public int? OldPointsExpiryMonths { get; set; }

        [NotMapped]
        public decimal? OldPointsPercentage { get; set; }

        [NotMapped]
        public decimal? OldPointsAllocation { get; set; }

        [NotMapped]
        public bool OldRevolvingStatus { get; set; }

        [NotMapped]
        public decimal? OldBurnMinTxnAmt { get; set; }

        [NotMapped]
        public decimal? OldMinRedemptionPts { get; set; }

        [NotMapped]
        public decimal? OldMinRedemptionPtsFirstTime { get; set; }

        [NotMapped]
        public decimal? OldBurnInvoiceAmtPercentage { get; set; }

        [NotMapped]
        public decimal? OldBurnDBPointsPercentage { get; set; }

    }
}
