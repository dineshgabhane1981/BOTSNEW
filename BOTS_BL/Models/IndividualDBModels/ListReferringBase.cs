using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class ListReferringBase
    {
        public string ReferredByMobileNo { get; set; }
        public string ReferredByName { get; set; }
        public int ReferralsGenerated { get; set; }
        public long ReferralTotalTxnCount { get; set; }
        public long ReferralTotalSpend { get; set; }

    }
}
