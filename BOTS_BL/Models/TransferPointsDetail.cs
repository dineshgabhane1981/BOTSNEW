namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TransferPointsDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(15)]
        public string OldMobileNo { get; set; }

        [StringLength(15)]
        public string NewMobileNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(1)]
        public string Type { get; set; }
    }
}
