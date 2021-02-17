namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NPCSMS")]
    public partial class NPCSM
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(100)]
        public string DBName { get; set; }

        [StringLength(50)]
        public string DBId { get; set; }

        [StringLength(100)]
        public string DBPassword { get; set; }

        [StringLength(1)]
        public string ReportStatus { get; set; }

        [StringLength(1000)]
        public string EmailId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }
    }
}
