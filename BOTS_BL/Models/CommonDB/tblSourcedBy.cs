namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSourcedBy")]
    public partial class tblSourcedBy
    {
        [Key]
        public int SourcedbyId { get; set; }

        [Required]
        [StringLength(250)]
        public string SourcedbyName { get; set; }
    }
}
