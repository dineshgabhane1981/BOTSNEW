namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustInfo")]
    public partial class tblCustInfo
    {
        [Key]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
