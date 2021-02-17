namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblModulesPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GroupId { get; set; }

        public bool IsProgramManagement { get; set; }

        public int? NoOfOutlets { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AnnualFees { get; set; }

        public int? PaymentTerms { get; set; }

        public int? PaymentScheduleId { get; set; }

        public bool IsSMSPack { get; set; }

        public int? Gateway { get; set; }

        public int? FreeSMS { get; set; }

        public int? PrepaidSMSCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PrepaidSMSAmount { get; set; }

        public bool IsWhatsApp { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IntegrationAmount { get; set; }

        public int? WAPrepaidPackCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WAPrepaidPackAmount { get; set; }

        public bool IsMemberWebpage { get; set; }

        public int? MemberWebpageCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MemberWebpageAnnualFee { get; set; }

        public bool IsNPC { get; set; }

        public int? NPCCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NPCAnnualCharge { get; set; }

        public bool IsMobileApp { get; set; }

        public int? MobileAppCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MobileAppAnnualCharge { get; set; }

        public bool IsSProfileUpdate { get; set; }

        public int? SProfileUpdateCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SProfileUpdateAnnualCharge { get; set; }

        public bool IsOtherOne { get; set; }

        [StringLength(50)]
        public string OtherOneDesc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OtherOneFees { get; set; }

        public bool IsOtherTwo { get; set; }

        [StringLength(50)]
        public string OtherTwoDesc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OtherTwoFees { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
