namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReferralBonusSMS")]
    public partial class ReferralBonusSM
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(2)]
        public string SMSStatus { get; set; }

        [StringLength(4000)]
        public string SMS { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Senddate { get; set; }

        [StringLength(50)]
        public string TokenId { get; set; }

        [StringLength(100)]
        public string Url { get; set; }

        [StringLength(10)]
        public string Mobileno { get; set; }
    }
}
