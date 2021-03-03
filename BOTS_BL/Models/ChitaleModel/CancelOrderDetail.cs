namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CancelOrderDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CancelDate { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductAmt { get; set; }
    }
}
