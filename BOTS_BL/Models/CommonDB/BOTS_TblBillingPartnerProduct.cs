namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblBillingPartnerProduct
    {
        public int BillingPartnerId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BillingPartnerProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string BillingPartnerProductName { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool? IsActive { get; set; }        
    }
      

    public class BillingPartnerProductDetails
    {
        public int BillingPartnerId { get; set; }       
        public int BillingPartnerProductId { get; set; }                
        public string BillingPartnerProductName { get; set; }       
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsActive { get; set; }        
        public string CreatedDateStr { get; set; }       
        public string UserName { get; set; }
    }
}
