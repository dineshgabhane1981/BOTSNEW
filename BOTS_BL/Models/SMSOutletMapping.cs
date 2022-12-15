namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSOutletMapping")]
    public partial class SMSOutletMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SlNo { get; set; }

        [StringLength(8)]
        public string OutletId { get; set; }

        [StringLength(4000)]
        public string WATokenId { get; set; }

        [StringLength(15)]
        public string CircleID { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
