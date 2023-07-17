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

    public class ProductWisePerformance
    {
        public string Mobileno { get; set; }
        public string Productcode { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductSubCategoryCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public Int32 UniqueMember { get; set; }
        public Int32 TotalTxn { get; set; }
        public Int64 TotalQuantity { get; set; }
        public Decimal TotalAmount { get; set; }
        public DateTime LastPurchasedate { get; set; }
        public string LstDateStr { get; set; }

    }
}
