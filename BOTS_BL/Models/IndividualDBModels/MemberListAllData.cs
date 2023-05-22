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
        public long BurnPts { get; set; }
        public long AvlPts { get; set; }
        public DateTime? LasTTxnDate { get; set; }
        public string LasTTxnDateStr { get; set; }
    }
}
