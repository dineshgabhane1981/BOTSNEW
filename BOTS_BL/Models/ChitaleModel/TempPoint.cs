namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TempPoint
    {
        [Key]
        public long SlNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OldLOPPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NewLOPPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NetPoints { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        public long? DBSlNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OldNormalPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OldNetPoints { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OrderDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ravana { get; set; }

        [StringLength(50)]
        public string TxnType { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        public long? DateDiffs { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Amount { get; set; }
    }
}
