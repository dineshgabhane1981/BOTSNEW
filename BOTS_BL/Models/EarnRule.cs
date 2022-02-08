namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EarnRule")]
    public partial class EarnRule
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(3)]
        public string RuleId { get; set; }

        [StringLength(50)]
        public string RuleName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinTxnAmt { get; set; }

        [StringLength(1)]
        public string RewardFlag { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsPrecentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MaxPointsEarned { get; set; }

        [StringLength(1)]
        public string PointsExpiryFlag { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PointsExpiryFixedDate { get; set; }

        public long? PointsExpiryVariableDate { get; set; }

        [StringLength(1)]
        public string RoundOffFlag { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsAllocation { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsPerTransaction { get; set; }
    }
}
