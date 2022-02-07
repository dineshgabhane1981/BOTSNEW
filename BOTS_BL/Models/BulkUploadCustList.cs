namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BulkUploadCustList")]
    public partial class BulkUploadCustList
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(25)]
        public string RW_CustId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(8)]
        public string OutletId { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [StringLength(1)]
        public string Status { get; set; }
    }
}
