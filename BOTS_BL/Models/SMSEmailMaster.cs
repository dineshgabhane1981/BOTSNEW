namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSEmailMaster")]
    public partial class SMSEmailMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(3)]
        public string MessageId { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(4000)]
        public string SMS { get; set; }

        [StringLength(100)]
        public string MessageIdType { get; set; }
    }
}
