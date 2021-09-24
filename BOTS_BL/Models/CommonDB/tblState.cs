namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblState")]
    public partial class tblState
    {
        [Key]
        public int StateId { get; set; }

        [Required]
        [StringLength(150)]
        public string StateName { get; set; }
    }
}
