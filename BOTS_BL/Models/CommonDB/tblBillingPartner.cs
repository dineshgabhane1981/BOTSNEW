namespace BOTS_BL.Models.CommonDB
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

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }

    public class BillingPartnerDetails
    {
        public int BillingPartnerId { get; set; }
        public string BillingPartnerName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string UserName { get; set; }
    }
}
