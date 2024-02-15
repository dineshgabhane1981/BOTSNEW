namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSMSWhatsAppScriptMaster")]
    public partial class tblSMSWhatsAppScriptMaster
    {
        [Key]
        public long SlNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(50)]
        public string MessageType { get; set; }

        public string SMSScript { get; set; }

        public string WhatsAppScript { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        public string SMSTemplateId { get; set; }

        [StringLength(50)]
        public string SMSScriptType { get; set; }

        [StringLength(50)]
        public string WhatsAppScriptType { get; set; }

        [StringLength(50)]
        public string SMSWhatsAppSendStatus { get; set; }

        [StringLength(50)]
        public string WhatsAppMessageType { get; set; }

        public bool? IsActive { get; set; }
    }
}
