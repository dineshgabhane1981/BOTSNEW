namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StoreDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(8)]
        public string OutletId { get; set; }

        [StringLength(10)]
        public string CounterId { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(15)]
        public string CounterType { get; set; }

        [StringLength(15)]
        public string SecurityKey { get; set; }
    }
}
