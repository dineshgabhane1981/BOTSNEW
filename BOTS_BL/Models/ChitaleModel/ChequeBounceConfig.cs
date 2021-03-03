namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChequeBounceConfig")]
    public partial class ChequeBounceConfig
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string ConditionText { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ChequeBounce { get; set; }

        public int? ChequeBounceCount { get; set; }
    }
}
