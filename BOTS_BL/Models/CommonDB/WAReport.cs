namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WAReport")]
    public partial class WAReport
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
        public string SMSStatus { get; set; }

        [StringLength(1000)]
        public string EmailId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(100)]
        public string GroupCode { get; set; }

        [StringLength(1)]
        public string Status { get; set; }
    }
}
