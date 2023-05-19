namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCelebrationTxnDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceAmt { get; set; }

        public DateTime? TxnDatetime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsEarned { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPointsRedeem { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BasePointsRedeem { get; set; }

        public bool? DisableStatus { get; set; }

        [StringLength(50)]
        public string CelebrationType { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }
    }
}
