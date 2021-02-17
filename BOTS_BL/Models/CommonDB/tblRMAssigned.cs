namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRMAssigned")]
    public partial class tblRMAssigned
    {
        [Key]
        public int RMAssignedId { get; set; }

        [Required]
        [StringLength(250)]
        public string RMAssignedName { get; set; }
    }
}
