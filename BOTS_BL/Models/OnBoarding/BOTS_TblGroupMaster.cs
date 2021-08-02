namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class BOTS_TblGroupMaster
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SINo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string GroupName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string OwnerName { get; set; }

        [StringLength(10)]
        public string OwnerMobileNo { get; set; }

        [StringLength(100)]
        public string OwnerEmailId { get; set; }

        [StringLength(500)]
        public string GSTDocument { get; set; }
        [NotMapped]
        public HttpPostedFileBase GSTDocumentFile { get; set; }

        [StringLength(500)]
        public string PANDocument { get; set; }
        [NotMapped]
        public HttpPostedFileBase PANDocumentFile { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(10)]
        public string AlternateMobileNo { get; set; }

        [StringLength(100)]
        public string AlternateEmailId { get; set; }

        [StringLength(10)]
        public string NoOfRetailCategory { get; set; }

        [StringLength(50)]
        public string RetailId { get; set; }

        public bool? IsMWP { get; set; }

        public bool? IsWhatsApp { get; set; }

        public string NoOfFreeWhatsAppMsg { get; set; }

        public string NoOfFreeSMS { get; set; }

        public string NoOfPaidWhatsAppMsg { get; set; }

        public string NoOfPaidSMS { get; set; }

        public bool? IsMobileApp { get; set; }

        [Column(TypeName = "text")]
        public string MobileAppDescription { get; set; }

        [StringLength(100)]
        public string SourcedBy { get; set; }

        [StringLength(100)]
        public string AssignedCS { get; set; }

        [Column(TypeName = "text")]
        public string Comments { get; set; }

        [StringLength(200)]
        public string Referredby { get; set; }

        [StringLength(200)]
        public string ReferredName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WhatsAppPerMsgRate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SMSPerMsgRate { get; set; }

        public bool? IsEcommerceIntegration { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OtherFees { get; set; }

        [Column(TypeName = "text")]
        public string OtherFeesDescription { get; set; }
        [Column(TypeName = "text")]
        public string AnyOtherName { get; set; }
    }
}
