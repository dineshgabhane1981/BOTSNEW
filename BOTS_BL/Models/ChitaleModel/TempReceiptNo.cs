namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TempReceiptNo")]
    public partial class TempReceiptNo
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string ReceiptNo { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
