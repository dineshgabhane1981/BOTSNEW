using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.ChitaleModel;


namespace BOTS_BL.Repository
{
    public class ChitaleDashboardRepository
    {
        public CustomerDetail GetCustomerDetail(string CustomerId)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            using (var context = new ChitaleDBContext())
            {
                objCustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            }
            return objCustomerDetail;
        }

        public Dashboardsummary GetSummeryDetails(string CustomerId, bool IsBTD)
        {
            Dashboardsummary objDashboardsummary = new Dashboardsummary();
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            using (var context = new ChitaleDBContext())
            {
                if (IsBTD)
                {
                    objtransactionmaster = context.TransactionMasters.Where(x => x.CustomerId == CustomerId).ToList();
                }
                else
                {
                    objtransactionmaster = context.TransactionMasters.Where(x => x.CustomerId == CustomerId && x.OrderDatetime.Value.Month == DateTime.Today.Month
                    && x.OrderDatetime.Value.Year == DateTime.Today.Year).ToList();
                }
                if (objtransactionmaster != null)
                {
                    objDashboardsummary.PurchaseOrderPoints = (decimal)objtransactionmaster.Where(x => x.TxnType == "Purchase").Sum(x => x.NormalPoints);
                    objDashboardsummary.SalesOrderPoints = (decimal)objtransactionmaster.Where(x => x.TxnType == "Sale").Sum(x => x.NormalPoints);
                    objDashboardsummary.AddOnPoints = (decimal)objtransactionmaster.Sum(x => x.AddOnPoints);
                    objDashboardsummary.RedeemedPoints = 0;
                    objDashboardsummary.LostPoints = (decimal)objtransactionmaster.Sum(x => x.PenaltyPoints);
                    var NormalPoints = (decimal)objtransactionmaster.Sum(x => x.NormalPoints);
                    objDashboardsummary.TotalPointsBalance = (NormalPoints + objDashboardsummary.AddOnPoints) - objDashboardsummary.LostPoints;
                }
            }
            return objDashboardsummary;
        }

        public DashboardLostOpp GetDashboardLostOppData(string CustomerId, bool IsBTD)
        {
            DashboardLostOpp objDashboardLostOpp = new DashboardLostOpp();
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            using (var context = new ChitaleDBContext())
            {
                if (IsBTD)
                {
                    objtransactionmaster = context.TransactionMasters.Where(x => x.CustomerId == CustomerId).ToList();
                }
                else
                {
                    objtransactionmaster = context.TransactionMasters.Where(x => x.CustomerId == CustomerId && x.OrderDatetime.Value.Month == DateTime.Today.Month
                    && x.OrderDatetime.Value.Year == DateTime.Today.Year).ToList();
                }
                if (objtransactionmaster != null)
                {
                    objDashboardLostOpp.LateOrder= (decimal)objtransactionmaster.Where(x => x.TxnType == "Purchase").Sum(x => x.PenaltyPoints);
                    objDashboardLostOpp.CancelOrder = (decimal)objtransactionmaster.Where(x => x.TxnType == "Cancel Order").Sum(x => x.PenaltyPoints);
                    objDashboardLostOpp.TgtVsAch = (decimal)objtransactionmaster.Where(x => x.TxnType == "tgt vs ach").Sum(x => x.PenaltyPoints);
                    objDashboardLostOpp.SCMOrder = (decimal)objtransactionmaster.Where(x => x.TxnType == "SCM").Sum(x => x.PenaltyPoints);
                }
            }
            return objDashboardLostOpp;
        }

        public DashboardTarget GetTargetData(string CustomerId, bool IsCurrentMonth)
        {
            DashboardTarget objDashboardTarget = new DashboardTarget();
            List<TargetMaster> objTargetMaster = new List<TargetMaster>();
            using (var context = new ChitaleDBContext())
            {
                if (IsCurrentMonth)
                {
                    objTargetMaster = context.TargetMasters.Where(x => x.CustomerId == CustomerId && x.TargetFromDate.Value.Month == DateTime.Today.Month
                     && x.TargetFromDate.Value.Year == DateTime.Today.Year).ToList();
                }
                else
                {
                    objTargetMaster = context.TargetMasters.Where(x => x.CustomerId == CustomerId && x.TargetFromDate.Value.Month == DateTime.Today.Month-1
                     && x.TargetFromDate.Value.Year == DateTime.Today.Year).ToList();

                }
                if (objTargetMaster != null)
                {
                    objDashboardTarget.TargetValueWise = (decimal)objTargetMaster.Sum(x => x.TargetProductAmt);
                    objDashboardTarget.TargetVolumeWise = (decimal)objTargetMaster.Sum(x => x.TargetProductVolume);
                    if (IsCurrentMonth)
                    {
                        objDashboardTarget.AchiveValueWise = 0;
                        objDashboardTarget.AchiveVolumeWise = 0;
                    }
                    else
                    {
                        objDashboardTarget.AchiveValueWise = 0;
                        objDashboardTarget.AchiveVolumeWise = 0;
                    }
                       
                    
                }
            }
            return objDashboardTarget;
        }
    }
}
