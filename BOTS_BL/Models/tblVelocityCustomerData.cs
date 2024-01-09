namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblVelocityCustomerData")]
    public partial class tblVelocityCustomerData
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string VelocityPeriod { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string MemberName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrolledDate { get; set; }

        public long? NoofTxn { get; set; }

        public long? TotalAmtSpend { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastTxnDate { get; set; }
    }
}
