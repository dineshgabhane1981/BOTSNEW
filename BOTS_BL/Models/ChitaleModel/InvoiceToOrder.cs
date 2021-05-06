namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvoiceToOrder")]
    public partial class InvoiceToOrder
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

        [Column(TypeName = "date")]
        public DateTime? InvDate { get; set; }

        [StringLength(50)]
        public string InvNumber { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OrderAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Variance { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
        public string StrDate { get; set; }
        public string InvAmountStr { get; set; }
        public string OrderAmountStr { get; set; }

    }
}
