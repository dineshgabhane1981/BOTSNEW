namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDocumentType")]
    public partial class tblDocumentType
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(50)]
        public string Dept { get; set; }

        [StringLength(100)]
        public string DocumentType { get; set; }
    }
}
