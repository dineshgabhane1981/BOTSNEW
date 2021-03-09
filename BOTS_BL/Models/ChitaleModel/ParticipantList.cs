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
        public string participantType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string city { get; set; }
        public string cluster { get; set; }
        public string subcluster { get; set; }
        public int Rank { get; set; }
        public decimal Totalpoints { get; set; }

       public List<ParticipantList> lstparticipantlist { get; set; }

        
    }
}
