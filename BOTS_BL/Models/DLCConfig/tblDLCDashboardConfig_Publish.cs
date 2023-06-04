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

        public bool ShowLogoToFooter { get; set; }

        public bool CollectPersonalDataRandomly { get; set; }

        [StringLength(50)]
        public string UseLogo { get; set; }

        [StringLength(250)]
        public string UseLogoURL { get; set; }

        [StringLength(50)]
        public string PrefferedLanguage { get; set; }

        [StringLength(50)]
        public string HeaderColor { get; set; }
        [StringLength(50)]
        public string FontColor { get; set; }

        [Required]
        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
