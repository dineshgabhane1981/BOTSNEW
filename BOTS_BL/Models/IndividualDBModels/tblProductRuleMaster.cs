namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblProductRuleMaster")]
    public partial class tblProductRuleMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Percentage { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string CategoryType { get; set; }
    }
}
