namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPointsExpiryMemberDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CampaignFromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CampaignToDate { get; set; }

        public DateTime? Datetime { get; set; }

        public int? CampaignMonth { get; set; }

        public int? CampaignYear { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CampaignPoints { get; set; }
    }
}
