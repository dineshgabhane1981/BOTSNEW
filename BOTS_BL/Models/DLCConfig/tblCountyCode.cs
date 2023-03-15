namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCountyCode")]
    public partial class tblCountyCode
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(5)]
        public string Code { get; set; }
    }
}
