namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCProfileUpdatedList")]
    public partial class tblDLCProfileUpdatedList
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPts { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CustomerPoints { get; set; }
    }
}
