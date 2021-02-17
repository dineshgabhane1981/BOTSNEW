using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class MemberWebPage
    {
        public long TotalMember { get; set; }
        public long ReferringBase { get; set; }
        public long ReferralGenerated { get; set; }
        public long ReferralTransacted { get; set; }
        public long ReferralTxnCount { get; set; }
        public long TotalReferralBusniess { get; set; }
        public long NoofProfileUpdate { get; set; }
        public long ReferralPointsIssued { get; set; }
        public long ReferralPointsRedeem { get; set; }
        public long ReferralPointsExpired { get; set; }
        public long ReferralPointsUnused { get; set; }
        public long GiftPointsCount { get; set; }
        public long ProgramOtpOut { get; set; }
        public long PromoSMSOtpOut { get; set; }        
    }
}
