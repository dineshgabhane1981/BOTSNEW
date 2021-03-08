using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
    public class RedemptionModel
    {
        public decimal? DepositData { get; set; }
        public decimal? CreditData { get; set; }
        public decimal? InfraData { get; set; }
        public decimal? PromoData { get; set; }
        public string DepositDataStr { get; set; }
        public string CreditDataStr { get; set; }
        public string InfraDataStr { get; set; }
        public string PromoDataStr { get; set; }
    }
}
