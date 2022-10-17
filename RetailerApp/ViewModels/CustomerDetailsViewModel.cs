using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetailerApp.ViewModels
{
    public class CustomerDetailsViewModel
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CardNo { get; set; }
        public string PointBalance { get; set; }
        public string TotalSpend { get; set; }
        public string LastTxnDate { get; set; }
    }
}