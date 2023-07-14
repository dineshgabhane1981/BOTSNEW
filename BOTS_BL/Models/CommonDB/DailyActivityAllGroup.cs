namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DailyActivityAllGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [StringLength(200)]
        public string GroupName { get; set; }

        [Key]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }

        [StringLength(200)]
        public string DBName { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(200)]
        public string DBId { get; set; }

        [StringLength(200)]
        public string DBPassword { get; set; }

        public bool? IsActive { get; set; }

        public bool? BirthdayFlag { get; set; }

        public bool? AnniversaryFlag { get; set; }

        public bool? InActiveFlag { get; set; }

        public bool? PointsExpiryFlag { get; set; }

        public bool? BalanceEnquiryFlag { get; set; }

        public bool? BulkUploadTxnFlag { get; set; }
    }
}
