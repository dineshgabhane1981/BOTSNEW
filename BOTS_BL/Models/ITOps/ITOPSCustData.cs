using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ITOps
{
    public class ITOPSCustData
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string EnrolledOutlet { get; set; }
        public DateTime? DOJ { get; set; }
        public string CardNo { get; set; }
        public string CustomerId { get; set; }
        public decimal Points { get; set; }

    }
    public class ITOPSCustTxnData
    {
        public string TxnType { get; set; }
        public string InvoiceNo { get; set; }
        public string MobileNo { get; set; }
        public decimal? InvoiceAmt { get; set; }
        public decimal? PointsEarned { get; set; }
        public string OutletName { get; set; }
        public DateTime TxnDatetime { get; set; }      
        public long SlNo { get; set; }
        public string MobileNoInvId { get; set; }
    }

}
