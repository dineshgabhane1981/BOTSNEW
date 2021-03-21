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
    }
}
