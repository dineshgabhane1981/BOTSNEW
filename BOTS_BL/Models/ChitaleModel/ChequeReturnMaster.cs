namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChequeReturnMaster")]
    public partial class ChequeReturnMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string ReceiverCustomerType { get; set; }

        [StringLength(50)]
        public string ReceiverCustomerId { get; set; }

        [StringLength(50)]
        public string ChequeNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ChequeAmt { get; set; }

        [StringLength(100)]
        public string ReceiptNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReceiptDate { get; set; }
    }
}
