namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblOTPSMSWhatsAppCredential
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [Key]
        [StringLength(50)]
        public string OutletId { get; set; }

        [StringLength(50)]
        public string SMSVendor { get; set; }

        public string SMSUrl { get; set; }

        public string SMSLoginId { get; set; }

        public string SMSPassword { get; set; }

        public string SMSAPIKey { get; set; }

        public string WhatsAppVendor { get; set; }

        public string WhatsAppUrl { get; set; }

        public string WhatsAppTokenId { get; set; }

        public bool? IsActiveWhatsApp { get; set; }

        public bool? IsActiveSMS { get; set; }

        public string VerifiedWhatsAppUrl { get; set; }

        public string VerifiedWhatsAppLoginId { get; set; }

        public string VerifiedWhatsAppPassword { get; set; }

        public string VerifiedWhatsAppAPIKey { get; set; }

        [StringLength(50)]
        public string SMSSenderId { get; set; }

        [StringLength(50)]
        public string WhatsAppMessageType { get; set; }

        [StringLength(50)]
        public string VerifiedWhatsAppVendor { get; set; }
    }
}
