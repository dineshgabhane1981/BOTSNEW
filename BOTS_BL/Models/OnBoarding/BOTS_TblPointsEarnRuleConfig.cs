namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblPointsEarnRuleConfig
    {
        [Key]
        public long SrNo { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(4)]
        public string CategoryId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OnePointValueInRs { get; set; }

        [StringLength(50)]
        public string EarnPointLevel { get; set; }

        [StringLength(50)]
        public string EarnPointLevelType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FixedEarnPointPecentageWith { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FixedEarnPointPercentage { get; set; }

        [StringLength(50)]
        public string IncrementedValue { get; set; }

        [StringLength(50)]
        public string BlockOnEarnType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BlockOnInvoiceAmtMin { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BlockOnInvoiceAmtMax { get; set; }

        [StringLength(50)]
        public string EarnOnMaking { get; set; }

        [StringLength(50)]
        public string EarnOnFullAmt { get; set; }

        [StringLength(50)]
        public string EarnFullAmtWithGst { get; set; }

        [StringLength(50)]
        public string EarnFullAmtWithoutGst { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IncrementedpercentageWith { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IncrementedFixedPercentage { get; set; }

        [StringLength(50)]
        public string PercentageType { get; set; }
    }

    public class EarnPointLevel
    {
        public string EarnPointLevelId { get; set; }
        public string EarnPointLevelName { get; set; }
    }
}
