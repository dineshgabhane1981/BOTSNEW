namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BurnRule")]
    public partial class BurnRule
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string RuleId { get; set; }

        [StringLength(50)]
        public string RuleName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RedemptionInMultipleOf { get; set; }

        [StringLength(1)]
        public string PartialRedemption { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinRedemptionPoints { get; set; }

        [StringLength(1)]
        public string EarnWhileBurnFlag { get; set; }

        [StringLength(1)]
        public string EarnFullWhileBurnFlag { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinThresholdPointsFirstTime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinThresholdPointsEveryTime { get; set; }

        public long? RedemptionActivationDays { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinTxnAmt { get; set; }
    }
}
