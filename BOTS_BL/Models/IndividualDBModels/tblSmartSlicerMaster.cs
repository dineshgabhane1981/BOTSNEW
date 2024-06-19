namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSmartSlicerMaster")]
    public partial class tblSmartSlicerMaster
    {
        [Key]
        [StringLength(50)]
        public string MobileNo { get; set; }

        public long? EarnCount { get; set; }

        public long? BurnCount { get; set; }

        public long? TotalTxnCount { get; set; }

        public long? AvgTicketSize { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FirstTxnDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastTxnDate { get; set; }

        [StringLength(50)]
        public string VisitStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrolledDate { get; set; }

        public long? InActiveDays { get; set; }

        public long? Spends { get; set; }

        public long? PointsBalance { get; set; }

        public bool? RedeemStatus { get; set; }

        [StringLength(50)]
        public string EnrolledOutlet { get; set; }

        [StringLength(50)]
        public string FirstTxnOutlet { get; set; }

        [StringLength(50)]
        public string LastTxnOutlet { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public long? Age { get; set; }
        public string EnrolledBy { get; set; }
    }
}
