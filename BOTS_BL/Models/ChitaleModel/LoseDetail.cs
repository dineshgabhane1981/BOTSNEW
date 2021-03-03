namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LoseDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [StringLength(50)]
        public string ServiceProviderId { get; set; }

        [StringLength(50)]
        public string ServiceProviderType { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmountLose { get; set; }

        [StringLength(200)]
        public string ProductName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }
    }
}
