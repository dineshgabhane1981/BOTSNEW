namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSalesReturnMaster")]
    public partial class tblSalesReturnMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        [StringLength(50)]
        public string TxnType { get; set; }

        public DateTime? TxnDatetime { get; set; }

        public DateTime? TxnReceivedDatetime { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceAmt { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsGiven { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsRemoved { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CampaignPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OriginalInvAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CustBalancePts { get; set; }
    }
}
