namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TargetMaster")]
    public partial class TargetMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TargetSalesAmt { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TargetFromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TargetToDate { get; set; }

        public DateTime? EntryDatetime { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(500)]
        public string ProductName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TargetProductAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TargetProductVolume { get; set; }

        [StringLength(50)]
        public string MFCCode { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
