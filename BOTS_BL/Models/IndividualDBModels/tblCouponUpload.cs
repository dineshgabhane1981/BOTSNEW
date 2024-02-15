namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCouponUpload")]
    public partial class tblCouponUpload
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(250)]
        public string CouponFileName { get; set; }

        public bool? IsReminder { get; set; }

        public int? Count { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReminderDate { get; set; }

        public DateTime? UploadedDate { get; set; }

        [StringLength(50)]
        public string UploadedBy { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal? CouponValue { get; set; }
        [NotMapped]
        public int RedeemCount { get; set; }
        [NotMapped]
        public string CouponRedeemBiz { get; set; }

    }
}
