namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBurnPtsProrataInvAmt")]
    public partial class tblBurnPtsProrataInvAmt
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }

        public DateTime? TxnDatetime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BurnPoints { get; set; }

        [StringLength(50)]
        public string PointsType { get; set; }

        [StringLength(50)]
        public string PointsDesc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProrataInvAmt { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }
    }
}
