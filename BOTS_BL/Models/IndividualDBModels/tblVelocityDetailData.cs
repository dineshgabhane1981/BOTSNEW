namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblVelocityDetailData")]
    public partial class tblVelocityDetailData
    {
        [Key]
        public int SlNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReportDate { get; set; }

        [StringLength(50)]
        public string SevenDays { get; set; }

        [StringLength(50)]
        public string ThirtyDays { get; set; }

        [StringLength(50)]
        public string NintyDays { get; set; }

        [StringLength(50)]
        public string OneEightyDays { get; set; }

        [StringLength(50)]
        public string ThreeSixtyFiveDays { get; set; }
    }
}
