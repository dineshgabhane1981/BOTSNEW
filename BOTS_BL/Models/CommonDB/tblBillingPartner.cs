namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBillingPartner")]
    public partial class tblBillingPartner
    {
        [Key]
        public int BillingPartnerId { get; set; }

        [Required]
        [StringLength(250)]
        public string BillingPartnerName { get; set; }
    }
}
