using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class OnBoardingListing
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string OwnerMobileNo { get; set; }
        public string City { get; set; }
        public string PaymentStatus { get; set; }
        public string BillingPartnerName { get; set; }
        public string CustomerStatus { get; set; }
        public bool IsIntroCall { get; set; }
        public string CSAssigned { get; set; }
    }
}
