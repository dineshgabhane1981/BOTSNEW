namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblExhibitionData")]
    public partial class tblExhibitionData
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(150)]
        public string ExibitionName { get; set; }

        [StringLength(250)]
        public string ProspectName { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(250)]
        public string ShopName { get; set; }

        [StringLength(150)]
        public string BillingSystem { get; set; }

        public string WelcomeScript { get; set; }

        public DateTime? WelcomeScriptSentDate { get; set; }

        public string InactiveScript { get; set; }

        public DateTime? InactiveScriptDate { get; set; }

        public DateTime? InactiveScriptSentDate { get; set; }

        public string FestiveCampaignScript { get; set; }

        public DateTime? FestiveCampaignScriptDate { get; set; }

        public DateTime? FestiveCampaignSentDate { get; set; }

        public string DLCinformationScript { get; set; }

        public DateTime? DLCinformationScriptDate { get; set; }

        public DateTime? DLCinformationSentDate { get; set; }

        public string PointExpiryScript { get; set; }

        public DateTime? PointExpiryScriptDate { get; set; }

        public DateTime? PointExpirySentDate { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
