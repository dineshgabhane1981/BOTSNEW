namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionTypeMaster")]
    public partial class TransactionTypeMaster
    {
        [Key]
        [Column(Order = 0)]
        public long SlNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string TransactionType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string TransactionName { get; set; }
    }
}
