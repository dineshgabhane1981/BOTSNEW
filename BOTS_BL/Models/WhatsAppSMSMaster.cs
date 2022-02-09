namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WhatsAppSMSMaster")]
    public partial class WhatsAppSMSMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(3)]
        public string MessageId { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        public string SMS { get; set; }

        [StringLength(500)]
        public string TokenId { get; set; }

        [StringLength(8)]
        public string OutletId { get; set; }
    }
}
