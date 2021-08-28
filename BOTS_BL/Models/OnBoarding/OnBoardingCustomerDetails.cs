using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class OnBoardingCustomerDetails
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string OwnerMobileNo { get; set; }
        public string OwnerEmailId { get; set; }
        public string City { get; set; }
        public string AlternateMobileNo { get; set; }
        public string AlternateEmailId { get; set; }
        public string NoOfRetailCategory { get; set; }
        public bool? IsMWP { get; set; }
        public bool? IsWhatsApp { get; set; }
        public string NoOfFreeWhatsAppMsg { get; set; }
        public string NoOfFreeSMS { get; set; }
        public string NoOfPaidWhatsAppMsg { get; set; }
        public string NoOfPaidSMS { get; set; }
        public string IsMobileApp { get; set; }
        public string SourcedBy { get; set; }
        public string AssignedCS { get; set; }
        public string Comments { get; set; }
        public string Referredby { get; set; }
        public string ReferredName { get; set; }
        public decimal? OtherFees { get; set; }
        public string OtherFeesDescription { get; set; }
        public int IsKeyAccount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string GSTDocument { get; set; }
        public string PANDocument { get; set; }
    }
}
