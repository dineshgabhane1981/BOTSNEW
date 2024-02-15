namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblTempTxnJSON")]
    public partial class tblTempTxnJSON
    {
        [Key]
        public int SlNo { get; set; }

        [Required]
        [StringLength(150)]
        public string InvoiceNo { get; set; }

        [Required]
        public string JSON { get; set; }
    }
}
