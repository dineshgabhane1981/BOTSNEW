using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.RetailerWeb
{
    public class CustomerDetails
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CardNo { get; set; }
        public string PointBalance { get; set; }
        public string TotalSpend { get; set; }
        public string LastTxnDate { get; set; }
        public string ResponseCode { get; set; }
    }
}
