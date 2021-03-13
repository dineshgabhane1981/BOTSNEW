using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
  public  class ParticipantList
    {
        public string ParticipantType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Cluster { get; set; }
        public string SubCluster { get; set; }
        public string Rank { get; set; }
        public decimal TotalPoints { get; set; }

       public List<ParticipantList> lstparticipantlist { get; set; }

        
    }
}
