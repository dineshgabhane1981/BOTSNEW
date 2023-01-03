namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblFestival
    {
        [Key]
        public int SlNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Festival { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    }
}
