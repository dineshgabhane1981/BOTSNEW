namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MobileAppCommSecurityKey")]
    public partial class MobileAppCommSecurityKey
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string SecurityKey { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(500)]
        public string WebServiceLink { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
