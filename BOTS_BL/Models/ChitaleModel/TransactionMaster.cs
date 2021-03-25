namespace BOTS_BL.Models.ChitaleModel
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

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(25)]
        public string CustomerType { get; set; }

        public DateTime? OrderDatetime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RavanaDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceAmt { get; set; }

        [StringLength(50)]
        public string TxnType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NormalPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AddOnPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PenaltyPoints { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CustomerPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalPoints { get; set; }

        [StringLength(50)]
        public string PONumber { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [StringLength(100)]
        public string ReceiptNo { get; set; }

        [StringLength(50)]
        public string TxnElement { get; set; }

        [StringLength(100)]
        public string AppID { get; set; }

        [StringLength(100)]
        public string RefOrderNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AchievedAmt { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        [StringLength(100)]
        public string SubType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AchPercentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Variance { get; set; }
    }
}
