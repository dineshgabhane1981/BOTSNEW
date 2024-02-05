namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCouponMapping")]
    public partial class tblCouponMapping
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string CouponCode { get; set; }

        public decimal? CouponValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ReminderDate { get; set; }

        [StringLength(150)]
        public string InvoiceNo { get; set; }

        public DateTime? RedeemDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RedeemInvoiceAmount { get; set; }

        [StringLength(50)]
        public string RedeemDay { get; set; }

        [StringLength(50)]
        public string RedeemCategory { get; set; }

        [StringLength(50)]
        public string RedeemProduct { get; set; }

        [StringLength(50)]
        public string RedeemOutlet { get; set; }

        public int? EarnRuleId { get; set; }
    }
}
