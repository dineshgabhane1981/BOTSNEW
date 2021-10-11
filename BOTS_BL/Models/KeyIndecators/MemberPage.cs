using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class MemberPage
    {
        public long? ReferringBase { get; set; }
        public decimal? ReferringBasePercentage { get; set; }
        public long? RefGen { get; set; }
        public long? Con { get; set; }
        public decimal? RefGenConPercentage { get; set; }
        public long? RefBiz { get; set; }
        public long? NewRegistration { get; set; }
        public long? NewRegistrationBiz { get; set; }
        public long? ProfileUpdateCount { get; set; }
        public decimal? ProfileUpdatePercentage { get; set; }
        public long? GiftAPointsCount { get; set; }
        public decimal? GiftAPointsPercentage { get; set; }
        public long? OptOutCount { get; set; }
        public decimal? OptOutPercentage { get; set; }
    }
    public class MemberPageRefData
    {
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public long? ReferralGenerated { get; set; }
        public long? ReferralTransacted { get; set; }
        public long? BusinessGenerated { get; set; }
        public string ReferralMobileNo { get; set; }
        public string ReferralName { get; set; }
        public int? TransactionCount { get; set; }
        public string Source { get; set; }
        public string SourceDesc { get; set; }
        public int? RegCount { get; set; }
        public decimal? Percentage { get; set; }


    }

    public class MemberPageNewRegisterationData
    {
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public string RegDate { get; set; }
        public string OutletName { get; set; }
        public string FirstTxnDate { get; set; }
        public decimal? TotalSpend { get; set; }
        public int? TxnCount { get; set; }
    }

}
