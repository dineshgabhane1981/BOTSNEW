namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCProfileUpdateConfig")]
    public partial class tblDLCProfileUpdateConfig
    {
        [Key]
        public long Slno { get; set; }

        [Required]
        [StringLength(200)]
        public string FieldName { get; set; }

        public bool IsDisplay { get; set; }

        public bool IsMandatory { get; set; }
    }
}
