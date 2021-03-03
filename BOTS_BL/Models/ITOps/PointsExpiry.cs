namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PointsExpiry")]
    public partial class PointsExpiry
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(10)]
        public string CounterId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EarnDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BurnDate { get; set; }

        [StringLength(25)]
        public string InvoiceNo { get; set; }

        public long? TransRefNo { get; set; }

        [StringLength(25)]
        public string OriginalInvoiceNo { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(12)]
        public string CustomerId { get; set; }
    }
}
