namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblEarnRuleConfig
    {
        [Key]
        public int RuleId { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        public int? MinInvoiceAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BasePercentage { get; set; }

        public int? PointsValidityInMonths { get; set; }

        public bool RevolvingExpiry { get; set; }

        public bool? IsBase { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsValueInRS { get; set; }

        public bool? IsSlab { get; set; }

        public bool? IsProductWise { get; set; }

        [StringLength(50)]
        public string ProductWiseType { get; set; }

        public bool? IsBlockForEarn { get; set; }

        [StringLength(50)]
        public string BlockProductWiseType { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
