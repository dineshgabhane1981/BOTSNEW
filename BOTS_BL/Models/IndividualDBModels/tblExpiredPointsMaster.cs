namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblExpiredPointsMaster")]
    public partial class tblExpiredPointsMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ExpiredPoints { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiredDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BalancePoints { get; set; }

        [StringLength(50)]
        public string PointsType { get; set; }

        [StringLength(50)]
        public string PointsCategory { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }
    }
}
