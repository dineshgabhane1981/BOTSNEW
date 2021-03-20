using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
   public class LeaderBoardForMgt
    {
        public string ParticipantType { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string NormalPoints { get; set; }
        public string CurrentOverallRank { get; set; }
        public string CurrentClusterRank { get; set; }
        public string CurrentStarRating { get; set; }
        public string LastMonthOverallRank { get; set; }
        public string LastMonthClusterRank { get; set; }
        public string LastStarRating { get; set; }
        public long RankMovement { get; set; }
    }
}
