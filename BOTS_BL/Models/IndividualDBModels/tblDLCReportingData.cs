using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class tblDLCReportingData
    {
        public string ReferredByMobileNo { get; set; }
        public string ReferredByName { get; set; }
        public DateTime? ReferredDate { get; set; }
        public string ReferralMobileNo { get; set; }
        public string ReferralName { get; set; }
        public bool ConvertedStatus { get; set; }
        public long ReferralTotalTxnCount { get; set; }
        public long ReferralTotalSpend { get; set; }


    }
}
