using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class PointsExpCustDetailList
    {
        public string OutletName { get; set; }
        public string MaskedMobileNo { get; set; }
        public string Name { get; set; }
        public long TotalTxnCount { get; set; }
        public decimal TotalSpend { get; set; }
        public DateTime? DOJ { get; set; }
        public string MobileNo { get; set; }
        public long AvlPts { get; set; }
        public DateTime? LastTxnDate { get; set; }
        public decimal Points { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
