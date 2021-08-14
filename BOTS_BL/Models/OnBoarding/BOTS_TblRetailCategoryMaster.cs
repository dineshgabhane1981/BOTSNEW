namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblRetailCategoryMaster
    {
        [Key]
        [StringLength(50)]
        public string CategoryId { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}
