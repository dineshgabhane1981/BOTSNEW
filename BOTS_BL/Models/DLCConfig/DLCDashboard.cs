using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BOTS_BL.Models
{
    public class DLCDashboard
    {
        public string RedirectToPage { get; set; }
        [NotMapped]
        public HttpPostedFileBase LogoFile1 { get; set; }
        [NotMapped]
        public HttpPostedFileBase LogoFile2 { get; set; }
        [NotMapped]
        public HttpPostedFileBase LogoFile3 { get; set; }
        public string LoginWithOTP { get; set; }
        public bool AddPersonalDetails { get; set; }
        public bool AddGiftPoints { get; set; }
        public bool AddReferFriend { get; set; }
        public int PersonalDetailsPoints { get; set; }
        public int ReferPoints { get; set; }
        public int GiftPoints { get; set; }

        public string ExtraWidgetText { get; set; }
        public int ExtraWidgetPoints { get; set; }
        public bool ShowLogoToFooter { get; set; }
        public bool CollectPersonalDataRandomly { get; set; }
        public string UseLogo { get; set; }
        public string UseLogoURL { get; set; }

    }
}
