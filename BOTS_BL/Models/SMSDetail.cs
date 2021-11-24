namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SMSDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(10)]
        public string SenderId { get; set; }

        [StringLength(25)]
        public string TxnUserName { get; set; }

        [StringLength(25)]
        public string TxnPassword { get; set; }

        [StringLength(50)]
        public string TxnUrl { get; set; }

        [StringLength(25)]
        public string OTPUserName { get; set; }

        [StringLength(25)]
        public string OTPPassword { get; set; }

        [StringLength(50)]
        public string OTPUrl { get; set; }

        [StringLength(500)]
        public string WhatsAppTokenId { get; set; }

        [StringLength(25)]
        public string VirtualSmsUserName { get; set; }

        [StringLength(25)]
        public string VirtualSmsPassword { get; set; }

        [StringLength(25)]
        public string VirtualWhatsAppUserName { get; set; }

        [StringLength(25)]
        public string VirtualWhatsAppPassword { get; set; }

        [StringLength(8)]
        public string OutletId { get; set; }
    }
}
