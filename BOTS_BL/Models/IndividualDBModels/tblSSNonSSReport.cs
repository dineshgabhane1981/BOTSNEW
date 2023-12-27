namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSSNonSSReport")]
    public partial class tblSSNonSSReport
    {
        [Key]
        public int SlNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReportDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TotalBiz { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SSBiz { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NonSSBiz { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TotalBizPercentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SSBizPercentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NonSSBizPercentage { get; set; }

        public long TotalTxnCount { get; set; }

        public long SSTxnCount { get; set; }

        public long NonSSTxnCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ATS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SSATS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NonSSATS { get; set; }
    }
}
