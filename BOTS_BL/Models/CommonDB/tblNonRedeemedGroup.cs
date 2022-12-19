namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblNonRedeemedGroup
    {
        [Key]
        public long SlNo { get; set; }

        public string GroupName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TxnCount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastTxnDate { get; set; }
    }
}
