using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
   public class ParticipantListForManagement
    {
        public string ParticipantType { get; set; }
        public string Id { get; set; }
        public string ParticipantName { get; set; }
        public string CurrentRank { get; set; }
        public string LastMonthRank { get; set; }
        public string PurchasePoints { get; set; }
        public string SalePoints { get; set; }
        public string AddOnPoints { get; set; }
        public string LostOppPoints { get; set; }
        public string RedeemedPoints { get; set; }
        public string BalancedPoints { get; set; }
        public string BalancePoints { get; set; }

    }
}
