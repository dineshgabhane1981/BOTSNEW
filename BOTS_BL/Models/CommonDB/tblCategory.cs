namespace BOTS_BL.Models
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
    }
}
