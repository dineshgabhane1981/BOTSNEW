using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
    public class PointLedgerModel
    {
        public string CustomerId { get; set; }
        public string TxnType { get; set; }
        public string SubType { get; set; }
        public string RefNo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BasePoints { get; set; }
        public decimal? AddOnPoints { get; set; }
        public decimal? LostOppPoints { get; set; }
        public string AmountStr { get; set; }
        public string BasePointsStr { get; set; }
        public string AddOnPointsStr { get; set; }
        public string LostOppPointsStr { get; set; }
        public string OrderDate { get; set; }
        public string RavanaDate { get; set; }
        public int? DaysDiff { get; set; }
        public decimal? NetEarnPoints { get; set; }
        public string NetEarnPointsStr { get; set; }
        public string AchievedAmt { get; set; }
        public string Variance { get; set; }
        public string AchPercentage { get; set; }





    }
}
