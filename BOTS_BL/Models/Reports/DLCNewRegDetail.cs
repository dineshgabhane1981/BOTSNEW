using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.Reports
{
    public class DLCNewRegDetail
    {
        public DateTime SourceCreation { get; set; }
        public string SourceDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public string MemberType { get; set; }
        public Int64 BonusPointsIssued { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime? FirstTxnDateAfterReg { get; set; }
        public Int64 TxnAfterDays { get; set; }
        public Int64 TotalTxnCount { get; set; }
        public Int64 BusinessGenerated { get; set; }

        public Int64 PointsRedeemed { get; set; }

        public Int64 PointsExpired { get; set; }


    }
}
