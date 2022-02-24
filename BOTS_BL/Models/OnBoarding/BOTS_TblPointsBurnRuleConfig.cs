namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblPointsBurnRuleConfig
    {
        [Key]
        public long SrNo { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string CategoryId { get; set; }

        [StringLength(50)]
        public string BurnType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FirstTime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SubsequentTime { get; set; }

        [StringLength(50)]
        public string EarnWhileBurn { get; set; }

        [StringLength(50)]
        public string PointValidity { get; set; }
    }
}
