namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBulkCustList")]
    public partial class tblBulkCustList
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string CustName { get; set; }

        [StringLength(50)]
        public string EnrolledOutlet { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrolledDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPoints { get; set; }

        public bool? DisableStatus { get; set; }

        public bool? ConvertedStatus { get; set; }
    }
}
