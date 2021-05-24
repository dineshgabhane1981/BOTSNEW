using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class OtherReportProductwise
    {
    }
    public class SellingProductValue
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? UniqueMember { get; set; }
        public int? UniqueTxn { get; set; }
        public long? TotalAmt { get; set; }
    }
}
