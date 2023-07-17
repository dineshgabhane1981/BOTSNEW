namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblProductMaster")]
    public partial class tblProductMaster
    {
        [Key]
        public long SLno { get; set; }

        public long? ProductCode { get; set; }

        public string ProductName { get; set; }

        public long? CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public long? SubCategoryCode { get; set; }

        public string SubCategoryName { get; set; }
    }
}
