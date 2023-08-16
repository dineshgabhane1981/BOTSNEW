namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblTierMaster")]
    public partial class tblTierMaster
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(50)]
        public string TierName { get; set; }
    }
}
