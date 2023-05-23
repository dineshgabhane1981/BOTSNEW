using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class TransactionSummaryData
    {
        public string OutletName { get; set; }
        public string MobileNo { get; set; }
        public string MaskedMobileNo { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime? DOJ { get; set; }
        public string InvoiceNo { get; set; }
        public decimal InvoiceAmt { get; set; }
        public string TxnType { get; set; }
        public decimal PointsEarned { get; set; }
        public decimal PointsBurned { get; set; }
        public DateTime? TxnDatetime { get; set; }
        public DateTime? TxnReceivedDatetime { get; set; }
    }
}
