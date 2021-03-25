namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TgtvsAchMaster")]
    public partial class TgtvsAchMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(500)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string ProductType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VolumeTgt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VolumeAch { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VolumeAchPercentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ValueTgt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ValueAch { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ValueAchPercentage { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string CategoryCode { get; set; }

        [StringLength(50)]
        public string SubCategoryCode { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VolumePoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ValuePoints { get; set; }
    }
}
