namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSourceType")]
    public partial class tblSourceType
    {
        [Key]
        public int SourceTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string SourceTypeName { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
