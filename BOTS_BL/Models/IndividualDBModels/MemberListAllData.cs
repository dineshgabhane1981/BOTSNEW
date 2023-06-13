using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class MemberListAllData
    {
        public string CurrentEnrolledOutlet { get; set; }
        public string OutletName { get; set; }
        public DateTime? DOJ { get; set; }

        public string DOJStr { get; set; }

        public string MobileNo { get; set; }
        public string MaskedMobileNo { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long TotalTxnCount { get; set; }
        public decimal TotalSpend  { get; set; }
        public long BurnCount { get; set; }
        public decimal BurnPts { get; set; }
        public long AvlPts { get; set; }
        public DateTime? LasTTxnDate { get; set; }
        public string LasTTxnDateStr { get; set; }
        public long EarnCount { get; set; }
    }

    public class CelebrationMemberData
    {
        public string OutletName { get; set; }
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public long TotalTxnCount { get; set; }
        public decimal TotalSpend { get; set; }
        public long AvlPts { get; set; }
        public DateTime? LastTxnDate { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public DateTime? DOJ { get; set; }
    }
}
