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

        [StringLength(100)]
        public string TemplateId3 { get; set; }

        [StringLength(250)]
        public string TemplateName3 { get; set; }

        [StringLength(100)]
        public string TemplateType3 { get; set; }

        [StringLength(500)]
        public string ReferredSuccessOnReferralTxnWAScript { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
