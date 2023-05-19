namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblOTPDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string OTPType { get; set; }

        [StringLength(50)]
        public string OTP { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(100)]
        public string OutletName { get; set; }
    }
}
