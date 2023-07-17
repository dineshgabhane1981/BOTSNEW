using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.Reports
{
    public class ProductAnalytics
    {
        public string OutletId { get; set; }
        public string Mobileno { get; set; }
        public string Invoiceno { get; set; }
        public decimal ProductAmt { get; set; }
        public decimal ProductRate { get; set; }
        public string ProductQty { get; set; }
        public string ProductCode { get; set; }
        public string CategoryCode { get; set; }
        public string SubCategoryCode { get; set; }
        public DateTime TxnDatetime { get; set; }
        public string Name { get; set; }
        public string TotalVisits { get; set; }
        public string TotalSpends { get; set; }
        public string LastTxnDate { get; set; }
        public string LastTxnSelection { get; set; }

    }
}
