using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class ManagementDashboardRepository
    {
        Exceptions newexception = new Exceptions();

        public List<SelectListItem> GetClusterList()
        {
            List<ClusterMaster> objlist = new List<ClusterMaster>();
            List<SelectListItem> clusterItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objlist = context.ClusterMasters.ToList();
                    foreach (var item in objlist)
                    {
                        clusterItems.Add(new SelectListItem
                        {
                            Text = item.Cluster,
                            Value = Convert.ToString(item.SlNo)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return clusterItems;
        }
        public List<SelectListItem> GetSubClusterList()
        {
            List<SubClusterMaster> objsublist = new List<SubClusterMaster>();
            List<SelectListItem> subclusterItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objsublist = context.SubClusterMasters.ToList();
                    foreach (var item in objsublist)
                    {
                        subclusterItems.Add(new SelectListItem
                        {
                            Text = item.SubCluster,
                            Value = Convert.ToString(item.SlNo)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return subclusterItems;
        }
        public List<SelectListItem> GetCityList()
        {
            List<CityMaster> objCitylist = new List<CityMaster>();
            List<SelectListItem> CityItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objCitylist = context.CityMasters.ToList();
                    foreach (var item in objCitylist)
                    {
                        CityItems.Add(new SelectListItem
                        {
                            Text = item.City,
                            Value = Convert.ToString(item.SlNo)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return CityItems;
        }

        public Dashboardsummary GetSummeryDetails(bool IsBTD, string Cluster, string SubCluster, string City, string FromDate, string Todate)
        {
            Dashboardsummary objDashboardsummary = new Dashboardsummary();
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            using (var context = new ChitaleDBContext())
            {
                if (Cluster == "All" && SubCluster == "All" && City == "All" && FromDate == "" && Todate == "")
                {
                    objtransactionmaster = context.TransactionMasters.ToList();
                }
                else
                {
                    objtransactionmaster = context.TransactionMasters.ToList();
                    if (Cluster != "All")
                    {
                        var lstResult = from s in context.CustomerDetails
                                        join sa in context.TransactionMasters on s.CustomerId equals sa.CustomerId
                                        where s.Cluster == Cluster
                                        select sa;
                        objtransactionmaster = lstResult.ToList();
                    }
                    else if (SubCluster != "All")
                    {
                        var lstResult = from s in context.CustomerDetails
                                        join sa in context.TransactionMasters on s.CustomerId equals sa.CustomerId
                                        where s.SubCluster == SubCluster
                                        select sa;
                        objtransactionmaster = lstResult.ToList();
                    }
                    else if (City != "All")
                    {
                        var lstResult = from s in context.CustomerDetails
                                        join sa in context.TransactionMasters on s.CustomerId equals sa.CustomerId
                                        where s.City == City
                                        select sa;
                        objtransactionmaster = lstResult.ToList();
                    }
                    if (FromDate != "" && Todate != "")
                    {
                        objtransactionmaster = objtransactionmaster.Where(x => x.OrderDatetime >= Convert.ToDateTime(FromDate) && x.OrderDatetime <= Convert.ToDateTime(Todate)).ToList();
                    }
                    else
                    {
                        if (FromDate != "")
                        {
                            objtransactionmaster = objtransactionmaster.Where(x => x.OrderDatetime >= Convert.ToDateTime(FromDate)).ToList();
                        }
                        if (Todate != "")
                        {
                            objtransactionmaster = objtransactionmaster.Where(x => x.OrderDatetime >= Convert.ToDateTime(Todate)).ToList();
                        }
                    }
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


    }
}
