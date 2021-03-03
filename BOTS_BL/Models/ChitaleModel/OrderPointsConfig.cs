namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderPointsConfig")]
    public partial class OrderPointsConfig
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string ConditionText { get; set; }

        public int? DateDifference { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AddOnPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PenaltyPoints { get; set; }
    }
}
