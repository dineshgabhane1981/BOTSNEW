namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("tblDLCDashboardConfig")]
    public partial class tblDLCDashboardConfig
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(50)]
        public string LoginWithOTP { get; set; }

        [StringLength(50)]
        public string RedirectToPage { get; set; }

        public bool AddPersonalDetails { get; set; }

        public bool AddGiftPoints { get; set; }

        public bool AddReferFriend { get; set; }

        public int? PersonalDetailsPoints { get; set; }

        public int? ReferPoints { get; set; }

        public int? GiftPoints { get; set; }

        [StringLength(250)]
        public string ExtraWidgetText1 { get; set; }

        public int? ExtraWidgetPoints1 { get; set; }
        [StringLength(250)]
        public string ExtraWidgetText2 { get; set; }

        public int? ExtraWidgetPoints2 { get; set; }
        [StringLength(250)]
        public string ExtraWidgetText3 { get; set; }

        public int? ExtraWidgetPoints3 { get; set; }

        public bool ShowLogoToFooter { get; set; }

        public bool CollectPersonalDataRandomly { get; set; }

        [StringLength(50)]
        public string UseLogo { get; set; }

        [StringLength(250)]
        public string UseLogoURL { get; set; }
        public string PrefferedLanguage { get; set; }
        public string CountryCode { get; set; }
        public string HeaderColor { get; set; }        
        public string FontColor { get; set; }

        [Required]
        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime AddedDate { get; set; }

        [NotMapped]
        public string LogoFile1 { get; set; }
        [NotMapped]
        public string LogoFile2 { get; set; }
        [NotMapped]
        public string LogoFile3 { get; set; }
    }
}
