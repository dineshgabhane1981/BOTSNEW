namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblVelocityMonthwise")]
    public partial class tblVelocityMonthwise
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        [StringLength(50)]
        public string VelocityCount { get; set; }
    }
}
