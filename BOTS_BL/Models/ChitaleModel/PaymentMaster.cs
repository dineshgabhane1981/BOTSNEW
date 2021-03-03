namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaymentMaster")]
    public partial class PaymentMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PaymentReceiptDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PaymentCreditDate { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; }

        [StringLength(100)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaymentAmt { get; set; }

        [StringLength(25)]
        public string ChequeNo { get; set; }

        [StringLength(50)]
        public string ServiceProviderId { get; set; }

        [StringLength(50)]
        public string ServiceProviderType { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(100)]
        public string ReceiptNo { get; set; }
    }
}
