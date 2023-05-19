namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCRuleMaster")]
    public partial class tblDLCRuleMaster
    {
        [Key]
        public long DLCId { get; set; }

        [StringLength(100)]
        public string DLCName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DLCValue { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }
    }
}
