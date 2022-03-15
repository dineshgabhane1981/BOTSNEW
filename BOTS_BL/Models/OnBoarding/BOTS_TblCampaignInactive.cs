namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblCampaignInactive
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string InactiveType { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        public int Days { get; set; }

        public int? LessThanDays { get; set; }

        public string LessThanDaysScript { get; set; }

        public int? GreaterThanDays { get; set; }

        public string GreaterThanDaysScript1 { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
