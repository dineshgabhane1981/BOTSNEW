namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MWPSourceMaster")]
    public partial class MWPSourceMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(200)]
        public string SourceCode { get; set; }

        [StringLength(500)]
        public string SourceDesc { get; set; }

        public string SourceLink { get; set; }
        
    }
}
