namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblCampaignBirthdayAnniversaryConfig
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

        public int? IntroDays2 { get; set; }

        public string IntroScript2 { get; set; }

        public int? ReminderDays1 { get; set; }

        [StringLength(50)]
        public string ReminderWhen1 { get; set; }

        public string ReminderScript1 { get; set; }

        public int? ReminderDays2 { get; set; }

        [StringLength(50)]
        public string ReminderWhen2 { get; set; }

        public string ReminderScript2 { get; set; }

        [StringLength(50)]
        public string OnDayType { get; set; }

        public string OnDayScript { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}