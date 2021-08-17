namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCategory")]
    public partial class tblCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(250)]
        public string CategoryName { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CategoryDetails
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string UserName { get; set; }
    }
}
