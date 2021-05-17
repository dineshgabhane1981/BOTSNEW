using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace BOTS_BL.Models
{
    public class LoyaltyKPIs
    {
        public long? TotalBiz { get; set; }
        public long? LoyaltyBiz { get; set; }
        public long? Redemption { get; set; }
        public long? Referrals { get; set; }
        public long? Campaigns { get; set; }
        public long? SMSBlastWA { get; set; }
        public long? NewMWPRegistration { get; set; }
        public decimal? RedemptionPer { get; set; }
        public decimal? ReferralsPer { get; set; }
        public decimal? CampaignsPer { get; set; }
        public decimal? SMSBlastWAPer { get; set; }
        public decimal? NewMWPRegistrationPer { get; set; }
    }
}
