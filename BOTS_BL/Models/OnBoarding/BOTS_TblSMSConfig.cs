namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblSMSConfig
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        public bool IsSMS { get; set; }

        [StringLength(50)]
        public string PEID { get; set; }

        [StringLength(50)]
        public string SMSProvider { get; set; }

        [StringLength(150)]
        public string SMSSenderID { get; set; }

        [StringLength(150)]
        public string SMSUsername { get; set; }

        [StringLength(150)]
        public string SMSPassword { get; set; }

        [StringLength(250)]
        public string SMSlink { get; set; }

        public int? MessageId { get; set; }

        [StringLength(100)]
        public string TemplateId { get; set; }

        [StringLength(250)]
        public string TemplateName { get; set; }

        [StringLength(100)]
        public string TemplateType { get; set; }

        public string SMSScript { get; set; }

        public string SMSScriptDLT { get; set; }        

        [StringLength(50)]
        public string DLTStatus { get; set; }

        public string RejectReason { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
