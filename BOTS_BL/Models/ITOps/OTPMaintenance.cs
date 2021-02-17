namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OTPMaintenance")]
    public partial class OTPMaintenance
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(15)]
        public string MobileNo { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(10)]
        public string CounterId { get; set; }

        [StringLength(4)]
        public string OTP { get; set; }
    }
}
