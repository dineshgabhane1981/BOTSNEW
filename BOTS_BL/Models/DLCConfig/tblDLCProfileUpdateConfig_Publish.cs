namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblDLCProfileUpdateConfig_Publish
    {
        [Key]
        public long Slno { get; set; }

        [StringLength(200)]
        public string FieldName { get; set; }        
        public bool DisplayStatus { get; set; }        
        public bool MandStatus { get; set; }

        [NotMapped]
        public string Value { get; set; }
        [NotMapped]
        public DateTime? DOBValue { get; set; }

    }
}
