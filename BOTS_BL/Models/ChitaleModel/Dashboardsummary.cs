using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
    public class Dashboardsummary
    {
        public decimal PurchaseOrderPoints { get; set; }
        public decimal SalesOrderPoints { get; set; }
        public decimal RedeemedPoints { get; set; }
        public decimal AddOnPoints { get; set; }
        public decimal LostPoints { get; set; }
        public decimal TotalPointsBalance { get; set; }

        public string PurchaseOrderPointsStr { get; set; }
        public string SalesOrderPointsStr { get; set; }
        public string RedeemedPointsStr { get; set; }
        public string AddOnPointsStr { get; set; }
        public string LostPointsStr { get; set; }
        public string TotalPointsBalanceStr { get; set; }

    }

    public class DashboardLostOpp
    {
        public decimal LateOrder { get; set; }
        public decimal CancelOrder { get; set; }
        public decimal TgtVsAch { get; set; }
        public decimal SCMOrder { get; set; }
    }
    public class DashboardTarget
    {
        public decimal TargetValueWise { get; set; }
        public decimal AchiveValueWise { get; set; }
        public decimal TargetVolumeWise { get; set; }
        public decimal AchiveVolumeWise { get; set; }

    }
}
