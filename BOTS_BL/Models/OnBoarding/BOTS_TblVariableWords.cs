namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblVariableWords
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string VariableWords { get; set; }
    }
}
