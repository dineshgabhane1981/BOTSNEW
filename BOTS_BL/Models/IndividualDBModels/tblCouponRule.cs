namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCouponRule")]
    public partial class tblCouponRule
    {
        [Key]
        public int EarnRuleId { get; set; }

        [Required]
        [StringLength(250)]
        public string EarnRuleName { get; set; }

        public decimal? EarnInvoiceAmountFrom { get; set; }

        [StringLength(50)]
        public string EarnDay { get; set; }
        [StringLength(150)]
        public string EarnCategory { get; set; }
        [StringLength(150)]
        public string EarnProduct { get; set; }

        [StringLength(150)]
        public string EarnOutlet { get; set; }

        public decimal? RedeemInvoiceAmountFrom { get; set; }

        [StringLength(50)]
        public string RedeemDay { get; set; }
        [StringLength(150)]
        public string RedeemCategory { get; set; }
        [StringLength(150)]
        public string RedeemProduct { get; set; }

        [StringLength(150)]
        public string RedeemOutlet { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public bool? IsOnlyCoupon { get; set; }

        public int? CouponValue { get; set; }

        public decimal? EarnInvoiceAmountTo { get; set; }

        public decimal? RedeemInvoiceAmountTo { get; set; }
        public int? ExpiryDays { get; set; }
        [StringLength(50)]
        public string OfferCode { get; set; }
        [StringLength(50)]
        public string EarnBrand { get; set; }
        [StringLength(50)]
        public string BurnBrand { get; set; }
        [StringLength(50)]
        public string EarnDept { get; set; }
        [StringLength(50)]
        public string BurnDept { get; set; }
        [StringLength(50)]
        public string ValueType { get; set; }
        [StringLength(50)]
        public string CouponCode { get; set; }
        public string UsageTime { get; set; }

    }
}
