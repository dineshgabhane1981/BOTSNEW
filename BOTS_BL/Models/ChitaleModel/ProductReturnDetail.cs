namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductReturnDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ProductReturnDate { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(10)]
        public string Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductAmt { get; set; }

        [StringLength(500)]
        public string ProductReturnReason { get; set; }

        [StringLength(50)]
        public string MFCCode { get; set; }

        [StringLength(50)]
        public string ProductReturnRefNo { get; set; }

        [StringLength(50)]
        public string ReceiverCustomerId { get; set; }

        [StringLength(50)]
        public string ReceiverCustomerType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductVolume { get; set; }
    }
}
