namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TelecallerTracking")]
    public partial class TelecallerTracking
    {
        [Key]
        public long SrNo { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string CustomerName { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        public DateTime? DOB { get; set; }

        public DateTime? DOA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        public bool? IsSMSSend { get; set; }

        [Column(TypeName = "ntext")]
        public string Comments { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(8)]
        public string OutletId { get; set; }       
    }

    public class TelecallerCustomerData
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string DOA { get; set; }
        public decimal? TotalBalPoints { get; set; }
        public string outletid { get; set; }
        public string OutletName { get; set; }
        public string EnrollmentDate { get; set; }
        public string LastTxnDate { get; set; }
    }

}
