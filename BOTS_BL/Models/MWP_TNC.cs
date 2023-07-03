namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MWP_TNC
    {
        [Column("MWP_TNC")]
        [StringLength(5000)]
        public string MWP_TNC1 { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [Key]
        public int SlNo { get; set; }
    }
}
