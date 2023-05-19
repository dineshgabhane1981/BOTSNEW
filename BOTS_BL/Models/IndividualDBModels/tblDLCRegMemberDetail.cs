namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblDLCRegMemberDetail
    {
        [Key]
        public long SlNo { get; set; }

        public long? SourceId { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        public long? TxnCount { get; set; }

        public long? TotalSpend { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? FirstTxnDatetime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsIssued { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsRedeemed { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ExpiredPoints { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PointsExpiryDate { get; set; }
    }
}
