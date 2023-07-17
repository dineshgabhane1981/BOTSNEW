using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class PointExpiryDummyModel
    {
        public string MobileNo { get; set; }
        public string CustName { get; set; }
        public string EndDate  { get; set; }
        public DateTime? EDate { get; set; }
        public decimal? Points { get; set; }        
    }
    public class PointExpiryCampaignDetails
    {
        public string EndDate { get; set; }
        public int NoOfUsers { get; set; }
        public string CampaignStatus { get; set; }
    }
}
