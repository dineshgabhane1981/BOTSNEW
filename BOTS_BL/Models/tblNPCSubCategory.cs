namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblNPCSubCategory")]
    public partial class tblNPCSubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        [StringLength(50)]
        public string SubCategoryname { get; set; }
    }
}
