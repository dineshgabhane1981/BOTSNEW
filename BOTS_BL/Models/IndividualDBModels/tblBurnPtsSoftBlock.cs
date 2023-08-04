namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBurnPtsSoftBlock")]
    public partial class tblBurnPtsSoftBlock
    {
        [Key]
        public long RefNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string CounterId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BurnPoints { get; set; }

        public DateTime? TxnDatetime { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(100)]
        public string BillRefNo { get; set; }

        public DateTime? UnBlockingDatetime { get; set; }
    }
}
