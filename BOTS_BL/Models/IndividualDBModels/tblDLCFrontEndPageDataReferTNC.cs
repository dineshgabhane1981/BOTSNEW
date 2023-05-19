namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDLCFrontEndPageDataReferTNC")]
    public partial class tblDLCFrontEndPageDataReferTNC
    {
        [StringLength(5000)]
        public string MWP_ReferTNC { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [Key]
        public int SlNo { get; set; }
    }
}
