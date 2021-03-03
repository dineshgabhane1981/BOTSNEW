namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(100)]
        public string ProductCode { get; set; }

        [StringLength(200)]
        public string ProductName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitRate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductDiscountAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductGSTAmt { get; set; }

        [StringLength(50)]
        public string ServiceProviderId { get; set; }

        [StringLength(50)]
        public string ServiceProviderType { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductVolume { get; set; }
    }
}
