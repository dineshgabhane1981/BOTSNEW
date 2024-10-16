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
        public long SINo { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }

        [Required]
        [StringLength(100)]
        public string OwnerName { get; set; }

        [StringLength(50)]
        public string OwnerMobileNo { get; set; }

        [StringLength(100)]
        public string OwnerEmailId { get; set; }

        [StringLength(500)]
        public string GSTDocument { get; set; }

        [StringLength(500)]
        public string PANDocument { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(50)]
        public string AlternateMobileNo { get; set; }

        [StringLength(100)]
        public string SalesModelType { get; set; }

        [StringLength(100)]
        public string AlternateEmailId { get; set; }

        [StringLength(10)]
        public string NoOfRetailCategory { get; set; }

        public bool? IsMWP { get; set; }

        public bool? IsWhatsApp { get; set; }

        public string NoOfFreeWhatsAppMsg { get; set; }

        public string NoOfFreeSMS { get; set; }

        public string NoOfPaidWhatsAppMsg { get; set; }

        public string NoOfPaidSMS { get; set; }

        [StringLength(50)]
        public string IsMobileApp { get; set; }

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

        [StringLength(100)]
        public string AnyOtherName { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public int IsKeyAccount { get; set; }

        [StringLength(50)]
        public string CustomerStatus { get; set; }

        [StringLength(150)]
        public string Constitution { get; set; }

        public long? ExpectedTransaction { get; set; }

        public bool IsExistingLoyalty { get; set; }
        public string RejectReason { get; set; }
        public string PreferredLanguage { get; set; }

        public bool? IntroductionCall { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public DateTime? IntroductionCallDate { get; set; }
        public bool? IsLive { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [NotMapped]
        public HttpPostedFileBase GSTDocumentFile { get; set; }
        [NotMapped]
        public HttpPostedFileBase PANDocumentFile { get; set; }
        [NotMapped]
        public string CategoryData { get; set; }
        [NotMapped]
        public string PaymentScheduleData { get; set; }
        [NotMapped]
        public string OutletData { get; set; }

        [NotMapped]
        public string CityName { get; set; }
        [NotMapped]
        public string SourceByName { get; set; }
        [NotMapped]
        public string AssignedCSName { get; set; }
        [NotMapped]
        public string SourceTypeName { get; set; }
        [NotMapped]
        public string ReferredNameNew { get; set; }
        [NotMapped]
        public string IsFromLead { get; set; }
    }
}
