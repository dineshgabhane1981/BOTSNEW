namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblCampaignOtherConfig
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string CampaignType { get; set; }

        [StringLength(50)]
        public string SMSType { get; set; }

        public int? BonusPoints { get; set; }

        [StringLength(50)]
        public string Frequency { get; set; }

        public int? IntroDays1 { get; set; }

        public string IntroScript1 { get; set; }

        public string IntroScript1DLT { get; set; }

        [StringLength(100)]
        public string TemplateId1 { get; set; }

        [StringLength(250)]
        public string TemplateName1 { get; set; }

        [StringLength(100)]
        public string TemplateType1 { get; set; }

        public int? IntroDays2 { get; set; }

        public string IntroScript2 { get; set; }

        public string IntroScript2DLT { get; set; }

        [StringLength(100)]
        public string TemplateId2 { get; set; }

        [StringLength(250)]
        public string TemplateName2 { get; set; }

        [StringLength(100)]
        public string TemplateType2 { get; set; }

        public int? ReminderDays1 { get; set; }

        [StringLength(50)]
        public string ReminderWhen1 { get; set; }

        public string ReminderScript1 { get; set; }

        public string ReminderScript1DLT { get; set; }

        [StringLength(100)]
        public string TemplateId3 { get; set; }

        [StringLength(250)]
        public string TemplateName3 { get; set; }

        [StringLength(100)]
        public string TemplateType3 { get; set; }

        public int? ReminderDays2 { get; set; }

        [StringLength(50)]
        public string ReminderWhen2 { get; set; }

        public string ReminderScript2 { get; set; }

        public string ReminderScript2DLT { get; set; }

        [StringLength(100)]
        public string TemplateId4 { get; set; }

        [StringLength(250)]
        public string TemplateName4 { get; set; }

        [StringLength(100)]
        public string TemplateType4 { get; set; }

        [StringLength(50)]
        public string OnDayTypePT { get; set; }

        public string OnDayScriptPT { get; set; }

        public string OnDayScriptPTDLT { get; set; }

        [StringLength(100)]
        public string TemplateId5 { get; set; }

        [StringLength(250)]
        public string TemplateName5 { get; set; }

        [StringLength(100)]
        public string TemplateType5 { get; set; }

        [StringLength(50)]
        public string OnDayTypeNPT { get; set; }

        public string OnDayScriptNPT { get; set; }

        public string OnDayScriptNPTDLT { get; set; }

        [StringLength(100)]
        public string TemplateId6 { get; set; }

        [StringLength(250)]
        public string TemplateName6 { get; set; }

        [StringLength(100)]
        public string TemplateType6 { get; set; }

        [StringLength(50)]
        public string DLTStatus1 { get; set; }

        [StringLength(50)]
        public string DLTStatus2 { get; set; }

        [StringLength(50)]
        public string DLTStatus3 { get; set; }

        [StringLength(50)]
        public string DLTStatus4 { get; set; }

        [StringLength(50)]
        public string DLTStatus5 { get; set; }

        [StringLength(50)]
        public string DLTStatus6 { get; set; }

        public string RejectReason1 { get; set; }

        public string RejectReason2 { get; set; }

        public string RejectReason3 { get; set; }

        public string RejectReason4 { get; set; }

        public string RejectReason5 { get; set; }

        public string RejectReason6 { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
