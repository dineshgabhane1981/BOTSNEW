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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlNo { get; set; }

        [Key]
        [StringLength(150)]
        public string InvoiceNo { get; set; }

        [Required]
        public string JSON { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }
    }
}
