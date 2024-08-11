namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblTierChange")]
    public partial class tblTierChange
    {
        [Key]
        public long Slno { get; set; }

        [StringLength(10)]
        public string Mobileno { get; set; }

        [StringLength(20)]
        public string OldTier { get; set; }

        [StringLength(20)]
        public string NewTier { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
    }
}
