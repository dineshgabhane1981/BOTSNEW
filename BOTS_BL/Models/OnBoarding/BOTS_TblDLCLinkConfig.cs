namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblDLCLinkConfig
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        public int? ProfileUpdatePoints { get; set; }

        public int? ReferralPoints { get; set; }

        public int? ReferredPoints { get; set; }

        public int? MaxNoOfReferrals { get; set; }

        public int? ValidityOfReferralPoints { get; set; }

        public int? ReferralReminder { get; set; }

        [StringLength(500)]
        public string ToTheReferralSMSScript { get; set; }

        [StringLength(500)]
        public string ToTheReferralSMSScriptDLT { get; set; }

        [StringLength(100)]
        public string TemplateId1 { get; set; }

        [StringLength(250)]
        public string TemplateName1 { get; set; }

        [StringLength(100)]
        public string TemplateType1 { get; set; }

        [StringLength(500)]
        public string ToTheReferralWAScript { get; set; }

        [StringLength(500)]
        public string ReminderForPointsUsageSMSScript { get; set; }

        [StringLength(500)]
        public string ReminderForPointsUsageSMSScriptDLT { get; set; }

        [StringLength(100)]
        public string TemplateId2 { get; set; }

        [StringLength(250)]
        public string TemplateName2 { get; set; }

        [StringLength(100)]
        public string TemplateType2 { get; set; }

        [StringLength(500)]
        public string ReminderForPointsUsageWAScript { get; set; }

        [StringLength(500)]
        public string ReferredSuccessOnReferralTxnSMSScript { get; set; }

        [StringLength(500)]
        public string ReferredSuccessOnReferralTxnSMSScriptDLT { get; set; }

        [StringLength(100)]
        public string TemplateId3 { get; set; }

        [StringLength(250)]
        public string TemplateName3 { get; set; }

        [StringLength(100)]
        public string TemplateType3 { get; set; }

        [StringLength(500)]
        public string ReferredSuccessOnReferralTxnWAScript { get; set; }

        [StringLength(500)]
        public string DLCOTPScriptSMS { get; set; }

        [StringLength(500)]
        public string DLCOTPScriptSMSDLT { get; set; }

        [StringLength(500)]
        public string DLCOTPScriptWA { get; set; }

        [StringLength(100)]
        public string TemplateId4 { get; set; }

        [StringLength(250)]
        public string TemplateName4 { get; set; }

        [StringLength(100)]
        public string TemplateType4 { get; set; }

        [StringLength(500)]
        public string GiftPointsOTPScriptSMS { get; set; }

        [StringLength(500)]
        public string GiftPointsOTPScriptSMSDLT { get; set; }

        [StringLength(500)]
        public string GiftPointsOTPScriptWA { get; set; }

        [StringLength(100)]
        public string TemplateId5 { get; set; }

        [StringLength(250)]
        public string TemplateName5 { get; set; }

        [StringLength(100)]
        public string TemplateType5 { get; set; }

        [StringLength(500)]
        public string GiftPointsDebitOTPScriptSMS { get; set; }

        [StringLength(500)]
        public string GiftPointsDebitOTPScriptSMSDLT { get; set; }

        [StringLength(500)]
        public string GiftPointsDebitOTPScriptWA { get; set; }

        [StringLength(100)]
        public string TemplateId6 { get; set; }

        [StringLength(250)]
        public string TemplateName6 { get; set; }

        [StringLength(100)]
        public string TemplateType6 { get; set; }

        [StringLength(500)]
        public string GiftPointsCreditOTPScriptSMS { get; set; }

        [StringLength(500)]
        public string GiftPointsCreditOTPScriptSMSDLT { get; set; }

        [StringLength(500)]
        public string GiftPointsCreditOTPScriptWA { get; set; }

        [StringLength(100)]
        public string TemplateId7 { get; set; }

        [StringLength(250)]
        public string TemplateName7 { get; set; }

        [StringLength(100)]
        public string TemplateType7 { get; set; }

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

        [StringLength(50)]
        public string DLTStatus7 { get; set; }

        public string RejectReason1 { get; set; }

        public string RejectReason2 { get; set; }

        public string RejectReason3 { get; set; }

        public string RejectReason4 { get; set; }

        public string RejectReason5 { get; set; }

        public string RejectReason6 { get; set; }

        public string RejectReason7 { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
