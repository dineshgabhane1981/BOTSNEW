namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TempBalance")]
    public partial class TempBalance
    {
        [Key]
        public long SlNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NewPoints { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
