using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class DLCDashboardContent
    {
        public decimal? EarnPoints { get; set; }
        public decimal? EarnPercentage { get; set; }
        public string OutletName { get; set; }
        public string OutletAddress { get; set; }
        public string OutletLongitude { get; set; }
        public string OutletLatitude { get; set; }
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public decimal? PointsToRS { get; set; }
        public bool IsOptout { get; set; }
    }
}
