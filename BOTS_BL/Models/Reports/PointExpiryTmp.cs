using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class PointExpiryTmp
    {
        public long MemberCountThisMonth { get; set; }
        public long MemberPointsThisMonth { get; set; }
        public long MemberCountNextMonth { get; set; }
        public long MemberPointsNextMonth { get; set; }
        public long SelectedCount { get; set; }
        public long SelectedPoints { get; set; }   
        
        public List<PointExpiryTxn> lstPointExpiryTxn { get; set; }
    }

    public class PointExpiryTxn
    {
        public string EnrolledOutlet { get; set; }
        public string MobileNo { get; set; }
        public string MaskedMobileNo { get; set; }
        public string MemberName { get; set; }
        public long TxnCount { get; set; }
        public long TotalSpend { get; set; }
        public long AvlPoints { get; set; }
        public string LastTxnDate { get; set; }
        public long PointsExpiry { get; set; }
        public string ExpiryDate { get; set; }
    }
}
