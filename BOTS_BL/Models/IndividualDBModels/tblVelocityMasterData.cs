namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblVelocityMasterData")]
    public partial class tblVelocityMasterData
    {
        [Key]
        public int SlNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReportDate { get; set; }

        public int? SevenDaysCount { get; set; }

        public int? ThirtyDaysCount { get; set; }

        public int? NintyDaysCount { get; set; }

        public int? OneEightyDaysCount { get; set; }

        public int? ThreeSixtyFiveDaysCount { get; set; }
    }
}
