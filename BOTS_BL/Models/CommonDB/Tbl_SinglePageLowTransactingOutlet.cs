namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SinglePageLowTransactingOutlet
    {
        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string OutletName { get; set; }

        [StringLength(50)]
        public string AvgTxn { get; set; }

        [StringLength(50)]
        public string AvgGroupTxnIn30Days { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LowerByPercentage { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int Id { get; set; }
    }
}
