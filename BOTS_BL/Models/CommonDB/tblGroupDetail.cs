namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblGroupDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GroupId { get; set; }

        [Required]
        [StringLength(250)]
        public string RetailName { get; set; }

        [Required]
        [StringLength(250)]
        public string GroupName { get; set; }
        public string LanguagePreference { get; set; }       

        public int ProductType { get; set; }

        [Required]
        [StringLength(250)]
        public string OwnerName { get; set; }

        [Required]
        [StringLength(50)]
        public string OwnerMobileNo { get; set; }

        [StringLength(100)]
        public string OwnerEmail { get; set; }

        [StringLength(50)]
        public string GSTNO { get; set; }

        public int? RetailCategory { get; set; }

        public int? OutletCount { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public int? City { get; set; }

        public int? SourcedBy { get; set; }

        public int? RMAssigned { get; set; }

        public int? BillingPartner { get; set; }

        public int? BillingProduct { get; set; }

        [StringLength(500)]
        public string Logo { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? TrainingDoneDate { get; set; }

        public DateTime? MovedToLiveDate { get; set; }

        public DateTime? WentLiveDate { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsFeedback { get; set; }

        public bool IsMasked { get; set; }

        public bool? IsLive { get; set; }

        [StringLength(1)]
        public string CustomerType { get; set; }

        [NotMapped]
        public string OtherRetailCategory { get; set; }

        [NotMapped]
        public string OtherCity { get; set; }

        [NotMapped]
        public string OtherSourcedBy { get; set; }

        [NotMapped]
        public string OtherRMAssigned { get; set; }

        [NotMapped]
        public string OtherBillingPartner { get; set; }

        [NotMapped]
        public string LogoBase64 { get; set; }

        [NotMapped]
        public string CityName { get; set; }

        [NotMapped]
        public string RMTeamName { get; set; }

        [NotMapped]
        public double AverageTicket { get; set; }

    }
}
