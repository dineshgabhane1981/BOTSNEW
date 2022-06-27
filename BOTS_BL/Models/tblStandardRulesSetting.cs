namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblStandardRulesSetting")]
    public partial class tblStandardRulesSetting
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }

        public string OverlappingCategories { get; set; }

        public string SubElements { get; set; }

        public string OptimumEarnPercentRange { get; set; }

        public string TypicalExclusions { get; set; }

        public string OptimumPtsValidityInMonths { get; set; }

        public string RedemptionGuidelines { get; set; }

        public string ReferralPoints { get; set; }

        public string RefereePoints { get; set; }

        public string BirthdayAnniversaryPts { get; set; }

        public string InactiveCampaignPeriodInDays { get; set; }

        public string PointsExpiryAlertInDaysBefore { get; set; }
    }
}
