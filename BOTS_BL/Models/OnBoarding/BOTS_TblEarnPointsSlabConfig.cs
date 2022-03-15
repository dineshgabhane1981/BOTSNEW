namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblEarnPointsSlabConfig
    {
        [Key]
        public long SlabId { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string CategoryId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EarnPointSlabFromPercentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EarnPointSlabToPercentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EarnPointSlabFromRs { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EarnPointSlabToRs { get; set; }

        //[StringLength(50)]
        //public string EarnSlab { get; set; }

        [StringLength(50)]
        public string SlabType { get; set; }

        [StringLength(50)]
        public string EarnSlabDirectOrTelescoping { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SlabDirectOrTelescopingValue { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
