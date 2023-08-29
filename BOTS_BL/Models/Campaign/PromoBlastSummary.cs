using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class PromoBlastSummary
    {
        public Int64 CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string CampaignStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public Int64 CampaignMemberCount { get; set; }
        public Int64 TotalTxnCount { get; set; }
        public Int64 BusinessGenerated { get; set; }
    }

    public class PromoBlastDetails
    {       
        public string MobileNo { get; set; }
        public string Name { get; set; }        
        public string EnrolledOutlet { get; set; }       
        public Int64 TotalTxnCount { get; set; }
        public decimal BusinessGenerated { get; set; }
    }
    public class GroupCode
    {
        public string GroupCodeId { get; set; }
    }
}
