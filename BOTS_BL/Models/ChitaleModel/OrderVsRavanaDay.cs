namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderVsRavanaDay
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(100)]
        public string Cluster { get; set; }

        [StringLength(100)]
        public string SubCluster { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public int? OrderCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AvgDiffInDays { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Diff1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Diff2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Diff3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Diff4 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
    }
}
