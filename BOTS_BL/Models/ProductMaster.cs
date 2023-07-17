namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductMaster")]
    public partial class ProductMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public long? CategoryCode { get; set; }

        [StringLength(200)]
        public string CategoryName { get; set; }

        public long? SubCategoryCode { get; set; }

        [StringLength(200)]
        public string SubCategoryName { get; set; }
    }
}
