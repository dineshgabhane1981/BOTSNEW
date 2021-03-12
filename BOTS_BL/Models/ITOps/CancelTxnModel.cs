using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class CancelTxnModel
    {
        public string TransactionName { get; set; }
        public string InvoiceNo { get; set; }
        public string MobileNo { get; set; }
        public decimal? InvoiceAmt { get; set; }
        public string Points { get; set; }
        public string OutletName { get; set; }
        public string Datetime { get; set; }
        public string DatetimeOriginal { get; set; }
    }
}
