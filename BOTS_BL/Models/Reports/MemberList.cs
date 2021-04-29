using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class MemberList
    {
        public string MobileNo { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }
        public long AvBalPoints { get; set; }
        public long TxnCount { get; set; }
        public long TotalSpend { get; set; }
        public long TotalBurnTxn { get; set; }
        public long TotalBurnPoints { get; set; }
        public string LastTxnDate { get; set; }
        public string EnrooledOutlet { get; set; }
        public string EnrolledDate { get; set; }

    }
    public class OutletList
    {
        public string OutletId { get; set; }
        public string OutletName { get; set; }
    }

    public class BrandList
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
