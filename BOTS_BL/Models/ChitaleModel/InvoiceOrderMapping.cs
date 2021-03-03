namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvoiceOrderMapping")]
    public partial class InvoiceOrderMapping
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string OrderNo { get; set; }

        [StringLength(100)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string ServiceProviderId { get; set; }

        [StringLength(50)]
        public string ServiceProviderType { get; set; }
    }
}
