namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductReturnMaster")]
    public partial class ProductReturnMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalProductReturnAmt { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ProductReturnDate { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }
    }
}
