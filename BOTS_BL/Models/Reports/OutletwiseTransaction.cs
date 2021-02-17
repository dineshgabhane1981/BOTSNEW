using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class OutletwiseTransaction
    {
        public string OutletName { get; set; }
        public string MobileNo { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? InvoiceAmt { get; set; }
        public string TxnType { get; set; }
        public decimal? PointsEarned { get; set; }
        public decimal? PointsBurned { get; set; }
        public string TxnDatetime { get; set; }
        public string TxnUpdateDate { get; set; }

    }
}
