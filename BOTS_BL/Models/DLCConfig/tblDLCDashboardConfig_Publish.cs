namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblDLCDashboardConfig_Publish
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

        public bool? IsExtraWidgetText1 { get; set; }

        [StringLength(250)]
        public string ExtraWidgetText1 { get; set; }

        public int? ExtraWidgetPoints1 { get; set; }

        [StringLength(250)]
        public string ExtraWidgetText2 { get; set; }

        public bool? IsExtraWidgetText2 { get; set; }

        public int? ExtraWidgetPoints2 { get; set; }

        public bool? IsExtraWidgetText3 { get; set; }

        [StringLength(250)]
        public string ExtraWidgetText3 { get; set; }

        public int? ExtraWidgetPoints3 { get; set; }

        public bool ShowLogoToFooter { get; set; }

        public bool CollectPersonalDataRandomly { get; set; }

        [StringLength(50)]
        public string UseLogo { get; set; }

        [StringLength(250)]
        public string UseLogoURL { get; set; }

        [StringLength(50)]
        public string PrefferedLanguage { get; set; }

        [StringLength(50)]
        public string CountryCode { get; set; }

        [StringLength(50)]
        public string HeaderColor { get; set; }

        [StringLength(50)]
        public string FontColor { get; set; }

        [Required]
        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime AddedDate { get; set; }

        [StringLength(250)]
        public string UseCardURL { get; set; }

        [StringLength(50)]
        public string UseCard { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string WhatsappUrl { get; set; }
        public string YoutubeURL { get; set; }
        public string HeaderTheme { get; set; }
    }
}
