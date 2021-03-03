namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        public DateTime? OrderDatetime { get; set; }

        [StringLength(50)]
        public string RefOrderNo { get; set; }

        [StringLength(25)]
        public string ProductCode { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(10)]
        public string Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitRate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductDiscountAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductGSTAmt { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
