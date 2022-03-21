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
        
        [StringLength(50)]
        public string SMSorWA { get; set; }

        public int Days { get; set; }

        public int? LessThanDays { get; set; }

        public string LessThanDaysScript { get; set; }

        public int? GreaterThanDays { get; set; }

        public string GreaterThanDaysScript { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
