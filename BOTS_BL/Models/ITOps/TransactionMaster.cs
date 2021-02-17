namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionMaster")]
    public partial class TransactionMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string CounterId { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(1)]
        public string TransType { get; set; }

        [StringLength(1)]
        public string TransSource { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceAmt { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(12)]
        public string CustomerId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsEarned { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsBurned { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CampaignPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TxnAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CustomerPoints { get; set; }

        [StringLength(1)]
        public string Synchronization { get; set; }

        public DateTime? SyncDatetime { get; set; }
        
    }
}
