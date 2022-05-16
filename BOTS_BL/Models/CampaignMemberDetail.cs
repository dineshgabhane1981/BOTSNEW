namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CampaignMemberDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string CampaignId { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string CustomerBaseType { get; set; }

        [StringLength(50)]
        public string MemberQualifiedStatus { get; set; }

        public string Script { get; set; }
    }
}
