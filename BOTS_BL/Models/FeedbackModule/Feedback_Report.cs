using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.FeedbackModule
{
    public class Feedback_Report
    {
        public string GroupId { get; set; }
        public string OutletName { get; set; }
        public string SalesRName { get; set; }
        public string MobileNo { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }
        public string Datetime { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Source { get; set; }
        public string Txn { get; set; }
        public decimal? TxnAmount { get; set; }
    }

    public class Feedback_MobileNo
    {
        public string MobileNo { get; set; }
        public DateTime? Datetime { get; set; }
        public string OutletName { get; set; }
        public string SalesRName { get; set; }
    }
}
