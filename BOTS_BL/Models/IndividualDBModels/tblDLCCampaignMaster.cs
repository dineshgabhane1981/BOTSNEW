namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCCampaignMaster")]
    public partial class tblDLCCampaignMaster
    {
        [Key]
        public long DLCId { get; set; }

        [StringLength(100)]
        public string DLCName { get; set; }

        [StringLength(1000)]
        public string DLCLink { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public long? UniqueRegCount { get; set; }

        public long? TotalTxnCount { get; set; }

        public long? BusinessGenerated { get; set; }

        public bool? BackEndStatus { get; set; }
        public int? PointsGiven { get; set; }
        public int? PointValidityDayes { get; set; }
    }
}
