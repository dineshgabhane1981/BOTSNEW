namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblEReceiptConfig")]
    public partial class tblEReceiptConfig
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(250)]
        public string BrandName { get; set; }

        public string ThankyouMessage { get; set; }

        [StringLength(500)]
        public string WebsiteURL { get; set; }

        public bool IsFeedback { get; set; }

        public bool IsAdvertisingBanner { get; set; }

        public bool IsUpdateProfileAndEarn { get; set; }

        [StringLength(500)]
        public string UpdateProfileText { get; set; }

        [StringLength(500)]
        public string UpdateProfileBanner { get; set; }

        public bool IsSocialMedia { get; set; }

        [StringLength(50)]
        public string FacebookUrl { get; set; }

        [StringLength(50)]
        public string TwitterUrl { get; set; }

        [StringLength(50)]
        public string InstagramUrl { get; set; }

        [StringLength(50)]
        public string WhatsappUrl { get; set; }

        public string TermsAndConditions { get; set; }

        [StringLength(50)]
        public string CustomerServiceEmail { get; set; }

        [StringLength(50)]
        public string CustomerServiceContact { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
