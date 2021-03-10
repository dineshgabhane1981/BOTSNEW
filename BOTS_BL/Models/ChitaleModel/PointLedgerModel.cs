using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
    public class PointLedgerModel
    {
        public string TxnType { get; set; }
        public string SubType { get; set; }
        public string RefNo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BasePoints { get; set; }
        public decimal? AddOnPoints { get; set; }
        public decimal? LostOppPoints { get; set; }
        public string OrderDate { get; set; }
        public string RavanaDate { get; set; }
        public int? DaysDiff { get; set; }
        public decimal? NetEarnPoints { get; set; }

         

    }
}
