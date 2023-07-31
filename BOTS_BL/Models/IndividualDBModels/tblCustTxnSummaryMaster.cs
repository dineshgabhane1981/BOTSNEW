namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustTxnSummaryMaster")]
    public partial class tblCustTxnSummaryMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [Key]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FirstTxnDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastTxnDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalSpend { get; set; }

        public long? TotalTxnCount { get; set; }

        public long? EarnCount { get; set; }

        public long? BurnCount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FirstBurnDate { get; set; }

        public long? SalesReturnCount { get; set; }

        public long? SalesReturnAmt { get; set; }

        public long? BurnAmtWithPts { get; set; }

        public long? BurnAmtWithoutPts { get; set; }

        public decimal? BurnPts { get; set; }

        public decimal? EarnPts { get; set; }
        public decimal? SalesReturnPtsGiven { get; set; }
        public decimal ? SalesReturnPtsRemoved { get; set; }
    }
}
