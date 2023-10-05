namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBirthdaySMSWAScript")]
    public partial class tblBirthdaySMSWAScript
    {
        public long Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public int? Days { get; set; }

        public string SMSScript { get; set; }

        public string WAScript { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(500)]
        public string SMSTemplateId { get; set; }

        [StringLength(50)]
        public string SMSScriptType { get; set; }

        [StringLength(50)]
        public string WhatsAppScriptType { get; set; }

        [StringLength(50)]
        public string SMSWhatsAppSendStatus { get; set; }

        [StringLength(50)]
        public string WhatsAppMessageType { get; set; }
    }
}
