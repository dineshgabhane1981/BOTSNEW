namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblDLCGiftPtsDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string GiftPtsSenderMobileNo { get; set; }

        [StringLength(100)]
        public string GiftPtsSenderName { get; set; }

        [StringLength(50)]
        public string GiftPtsReceiverMobileNo { get; set; }

        [StringLength(100)]
        public string GiftPtsReceiverName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GiftPts { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GiftPtsSenderBalPts { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GiftPtsReceiverBalPts { get; set; }

        public DateTime? TxnDatetime { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }
    }
}
