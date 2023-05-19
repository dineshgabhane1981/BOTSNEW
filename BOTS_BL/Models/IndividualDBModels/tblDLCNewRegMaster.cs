namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCNewRegMaster")]
    public partial class tblDLCNewRegMaster
    {
        [Key]
        public long SourceId { get; set; }

        [StringLength(500)]
        public string SourceDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SourceDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public long? UniqueTxnCount { get; set; }

        public long? TotalTxnCount { get; set; }

        public long? TotalSpend { get; set; }

        public bool? BackEndStatus { get; set; }
    }
}
