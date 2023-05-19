namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblProfileUpdateMaster")]
    public partial class tblProfileUpdateMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPoints { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CustBalancePts { get; set; }
    }
}
