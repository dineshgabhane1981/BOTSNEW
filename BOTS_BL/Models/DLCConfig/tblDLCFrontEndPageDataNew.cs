namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCFrontEndPageDataNew")]
    public partial class tblDLCFrontEndPageDataNew
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public string MWP_Name { get; set; }
    }
}