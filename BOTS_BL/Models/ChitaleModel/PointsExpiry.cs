namespace BOTS_BL.Models.ChitaleModel
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

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(25)]
        public string CustomerType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EarnDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
