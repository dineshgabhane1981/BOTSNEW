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

        public decimal? EarnInvoiceAmount { get; set; }

        [StringLength(50)]
        public string EarnDay { get; set; }

        public int? EarnCategory { get; set; }

        public int? EarnProduct { get; set; }

        [StringLength(50)]
        public string EarnOutlet { get; set; }

        public decimal? RedeemInvoiceAmount { get; set; }

        [StringLength(50)]
        public string RedeemDay { get; set; }

        public int? RedeemCategory { get; set; }

        public int? RedeemProduct { get; set; }

        [StringLength(50)]
        public string RedeemOutlet { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public bool? IsOnlyCoupon { get; set; }
    }
}
