namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReferAndEarn")]
    public partial class ReferAndEarn
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(1000)]
        public string Name { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(1000)]
        public string BussinessName { get; set; }

        [StringLength(1000)]
        public string BussinessType { get; set; }

        [StringLength(1000)]
        public string RetailCategory { get; set; }

        [StringLength(1000)]
        public string City { get; set; }

        [StringLength(1000)]
        public string ReferByLoginId { get; set; }

        [StringLength(5)]
        public string GroupId { get; set; }
    }
}
