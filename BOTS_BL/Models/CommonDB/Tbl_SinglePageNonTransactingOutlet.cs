namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SinglePageNonTransactingOutlet
    {
        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string OutletName { get; set; }

        [StringLength(50)]
        public string AvgGroupTxn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastTxnDate { get; set; }

        [StringLength(50)]
        public string DaysSinceLastTxn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int Id { get; set; }
        [NotMapped]
        public int DaySinceLastTxn { get; set; }
    }
}
