namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCelebrationMemberDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CelebrationType { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CelebrationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CelebrationFromDate { get; set; }

        [StringLength(10)]
        public string CelebrationToDate { get; set; }

        public DateTime? Datetime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CelebrationPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CelebrationPercentage { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        public int? CelebrationMonth { get; set; }

        public int? CelebrationYear { get; set; }

        public DateTime? CelebrationDatetime { get; set; }
    }
}
