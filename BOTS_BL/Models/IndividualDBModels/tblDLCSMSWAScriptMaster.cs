namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCSMSWAScriptMaster")]
    public partial class tblDLCSMSWAScriptMaster
    {
        [Key]
        public long SlNo { get; set; }

        public long? DLCMessageId { get; set; }

        [StringLength(1000)]
        public string DLCMessageType { get; set; }

        public string DLCSMSScript { get; set; }

        public string DLCWAScript { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(500)]
        public string SMSTemplateId { get; set; }

        [StringLength(50)]
        public string SMSScriptType { get; set; }

        [StringLength(50)]
        public string WhatsAppScriptType { get; set; }

        [StringLength(50)]
        public string SMSWhatsAppSendStatus { get; set; }
    }
}
