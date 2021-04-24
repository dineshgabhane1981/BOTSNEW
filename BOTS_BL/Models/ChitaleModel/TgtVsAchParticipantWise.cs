using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
    public class TgtVsAchParticipantWise
    {
        public string Type { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Cluster { get; set; }
        public string SubCluster { get; set; }
        public string City { get; set; }
        public decimal? VolTgt { get; set; }
        public decimal? VolAch { get; set; }
        public decimal? VolAchPer { get; set; }
        public decimal? ValTgt { get; set; }
        public decimal? ValAch { get; set; }
        public decimal? ValAchPer { get; set; }
        public DateTime? DateVal { get; set; }
        public string MonthYear { get; set; }
    }
    public class GenerateOTPList
    {
        public string MobileNo { get; set; }
        public string MessageText { get; set; }
        public string SenderId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
    }
    public class RedeemptionData
    {
        public string Type { get; set; }
        public decimal? CaseIncentive { get; set; }
        public decimal? InfraStructure { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Promotion { get; set; }

    }

}
