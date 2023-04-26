using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class ManagementDashboardRepository
    {
        ChitaleException newexception = new ChitaleException();

        public List<SelectListItem> GetClusterList()
        {
            List<ClusterMaster> objlist = new List<ClusterMaster>();
            List<SelectListItem> clusterItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objlist = context.ClusterMasters.OrderBy(s => s.Cluster).ToList();
                   
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
        
        public List<SelectListItem> GetSubClusterList(int cluster)
        {
            List<SubClusterMaster> objsublist = new List<SubClusterMaster>();
            List<SelectListItem> subclusterItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objsublist = context.SubClusterMasters.Where(s => s.ClusterId == cluster).ToList();
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
        
        public List<SelectListItem> GetCityList(int Subcluster)
        {
            List<CityMaster> objCitylist = new List<CityMaster>();
            List<SelectListItem> CityItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objCitylist = context.CityMasters.Where(s => s.SubClusterId == Subcluster).ToList();
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
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    if (Cluster == "All" && SubCluster == "All" && City == "All" && FromDate == "" && Todate == "")
                    {
                        objtransactionmaster = context.TransactionMasters.ToList();
                    }
                    else
                    {
                        objtransactionmaster = context.TransactionMasters.ToList();
                        if (City != "All")
                        {
                            var lstResult = from s in context.CustomerDetails
                                            join sa in context.TransactionMasters on s.CustomerId equals sa.CustomerId
                                            where s.City == City
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
                        else if (Cluster != "All")
                        {
                            var lstResult = from s in context.CustomerDetails
                                            join sa in context.TransactionMasters on s.CustomerId equals sa.CustomerId
                                            where s.Cluster == Cluster
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
                        var TxnFromDate = context.TransactionMasters.OrderBy(y => y.OrderDatetime).Select(z => z.OrderDatetime).FirstOrDefault();
                        if (TxnFromDate != null)
                        {
                            objDashboardsummary.FromDate = TxnFromDate.Value.ToString("dd-MM-yyyy");
                            objDashboardsummary.ToDate = DateTime.Now.ToString("dd-MM-yyyy");
                        }
                        objDashboardsummary.PurchaseOrderPoints = (decimal)objtransactionmaster.Where(x => x.TxnType == "Purchase").Sum(x => x.NormalPoints);
                        objDashboardsummary.SalesOrderPoints = (decimal)objtransactionmaster.Where(x => x.TxnType == "Sale").Sum(x => x.NormalPoints);
                        objDashboardsummary.AddOnPoints = (decimal)objtransactionmaster.Sum(x => x.AddOnPoints);
                        objDashboardsummary.RedeemedPoints = 0;
                        objDashboardsummary.LostPoints = (decimal)objtransactionmaster.Sum(x => x.PenaltyPoints);
                        var NormalPoints = (decimal)objtransactionmaster.Sum(x => x.NormalPoints);
                        objDashboardsummary.TotalPointsBalance = (NormalPoints + objDashboardsummary.AddOnPoints) - objDashboardsummary.LostPoints;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return objDashboardsummary;
        }

        public List<CustomerDetail> GetTop5Participant(string type)
        {
            List<CustomerDetail> objTop5Participant = new List<CustomerDetail>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objTop5Participant = context.CustomerDetails.Where(a => a.CustomerType == type).OrderByDescending(x => x.Points).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return objTop5Participant;
        }

        public List<CustomerDetail> Bottom5Participants(string type)
        {
            List<CustomerDetail> objTop5Participant = new List<CustomerDetail>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objTop5Participant = context.CustomerDetails.Where(a => a.CustomerType == type).OrderBy(x => x.Points).Take(5).ToList();
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return objTop5Participant;
        }

        public List<Top5LostParticipants> GetTop5LostParticipant(string type)
        {
            List<Top5LostParticipants> objTop5Participant = new List<Top5LostParticipants>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objTop5Participant = context.Database.SqlQuery<Top5LostParticipants>("sp_GetTop5LostParticipants @pi_CustomerType", new SqlParameter("@pi_CustomerType", type)).ToList<Top5LostParticipants>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return objTop5Participant;
        }
        public List<ParticipantListForManagement> GetParticipantListForMgt(int Cluster, int SubCluster, int City)
        {

            List<ParticipantListForManagement> lstparticipantListsformgt = new List<ParticipantListForManagement>();
            try
            {
                if (Cluster == 0 && SubCluster == 0 && City == 0)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstparticipantListsformgt = context.Database.SqlQuery<ParticipantListForManagement>("sp_KYBLoad_MFC @pi_LoginId, @pi_Datetime, @pi_DataType, @pi_City,@pi_Cluster ,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                              new SqlParameter("@pi_DataType", ""),
                                new SqlParameter("@pi_City", ""),
                            new SqlParameter("@pi_Cluster", ""),
                            new SqlParameter("@pi_SubCluster", "")

                            ).ToList<ParticipantListForManagement>();
                    }

                }
                else if (City > 0)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstparticipantListsformgt = context.Database.SqlQuery<ParticipantListForManagement>("sp_KYBLoad_MFC @pi_LoginId, @pi_Datetime, @pi_DataType, @pi_City,@pi_Cluster ,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                              new SqlParameter("@pi_DataType", ""),
                                new SqlParameter("@pi_City", City),
                            new SqlParameter("@pi_Cluster", ""),
                            new SqlParameter("@pi_SubCluster", "")

                            ).ToList<ParticipantListForManagement>();
                    }

                }
                else if (SubCluster > 0)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstparticipantListsformgt = context.Database.SqlQuery<ParticipantListForManagement>("sp_KYBLoad_MFC @pi_LoginId, @pi_Datetime, @pi_DataType, @pi_City,@pi_Cluster ,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                              new SqlParameter("@pi_DataType", ""),
                                new SqlParameter("@pi_City", ""),
                            new SqlParameter("@pi_Cluster", ""),
                            new SqlParameter("@pi_SubCluster", SubCluster)

                            ).ToList<ParticipantListForManagement>();
                    }

                }
                else if (Cluster > 0)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstparticipantListsformgt = context.Database.SqlQuery<ParticipantListForManagement>("sp_KYBLoad_MFC @pi_LoginId, @pi_Datetime, @pi_DataType, @pi_City,@pi_Cluster ,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                              new SqlParameter("@pi_DataType", ""),
                                new SqlParameter("@pi_City", ""),
                            new SqlParameter("@pi_Cluster", Cluster),
                            new SqlParameter("@pi_SubCluster", "")

                            ).ToList<ParticipantListForManagement>();
                    }
                }               
                //else
                //{
                //    using (var context = new ChitaleDBContext())
                //    {
                //        lstparticipantListsformgt = context.Database.SqlQuery<ParticipantListForManagement>("sp_KYBLoad_MFC @pi_LoginId, @pi_Datetime, @pi_DataType, @pi_City,@pi_Cluster ,@pi_SubCluster",
                //            new SqlParameter("@pi_LoginId", ""),
                //            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                //              new SqlParameter("@pi_DataType", ""),
                //                new SqlParameter("@pi_City", ""),
                //            new SqlParameter("@pi_Cluster", ""),
                //            new SqlParameter("@pi_SubCluster", "")

                //            ).ToList<ParticipantListForManagement>();
                //    }
                //}

                if (lstparticipantListsformgt != null)
                {
                    lstparticipantListsformgt = lstparticipantListsformgt.OrderBy(x => Convert.ToInt32(x.CurrentRank)).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return lstparticipantListsformgt;
        }

        public List<ParticipantListForManagement> GetSubParticipantListForMgt(string Id, string ParticipantType)
        {

            List<ParticipantListForManagement> lstparticipantListsformgt = new List<ParticipantListForManagement>();
            try
            {

                using (var context = new ChitaleDBContext())
                {
                    lstparticipantListsformgt = context.Database.SqlQuery<ParticipantListForManagement>("sp_KYBLoadParticipant_MFC @pi_LoginId, @pi_Datetime, @pi_DataType, @pi_City,@pi_Cluster ,@pi_SubCluster,@pi_Id,@pi_ParticipantType",
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                          new SqlParameter("@pi_DataType", ""),
                            new SqlParameter("@pi_City", ""),
                        new SqlParameter("@pi_Cluster", ""),
                        new SqlParameter("@pi_SubCluster", ""),
                         new SqlParameter("@pi_Id", Id),
                          new SqlParameter("@pi_ParticipantType", ParticipantType)

                        ).ToList<ParticipantListForManagement>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return lstparticipantListsformgt;
        }

        public List<LeaderBoardForMgt> GetLeaderBoardForMgts(string radiobtn, int Cluster, int SubCluster, int City)
        {
            List<LeaderBoardForMgt> lstLeaderBrd = new List<LeaderBoardForMgt>();
            try
            {
                if (Cluster == 0 && SubCluster == 0 && City == 0 && radiobtn != null)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstLeaderBrd = context.Database.SqlQuery<LeaderBoardForMgt>("sp_LeaderBoard_MFC @pi_LoginId,@pi_ParticipantType,@pi_Datetime,@pi_City,@pi_Cluster,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_ParticipantType", radiobtn),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                                new SqlParameter("@pi_City", ""),
                            new SqlParameter("@pi_Cluster", ""),
                            new SqlParameter("@pi_SubCluster", "")
                            ).ToList<LeaderBoardForMgt>();
                    }
                }
                else if (City > 0)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstLeaderBrd = context.Database.SqlQuery<LeaderBoardForMgt>("sp_LeaderBoard_MFC @pi_LoginId,@pi_ParticipantType, @pi_Datetime, @pi_City,@pi_Cluster ,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_ParticipantType", radiobtn),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                                new SqlParameter("@pi_City", City),
                            new SqlParameter("@pi_Cluster", ""),
                            new SqlParameter("@pi_SubCluster", "")
                            ).ToList<LeaderBoardForMgt>();
                    }

                }
                else if (SubCluster > 0)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstLeaderBrd = context.Database.SqlQuery<LeaderBoardForMgt>("sp_LeaderBoard_MFC @pi_LoginId,@pi_ParticipantType, @pi_Datetime, @pi_City,@pi_Cluster ,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_ParticipantType", radiobtn),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                                new SqlParameter("@pi_City", ""),
                            new SqlParameter("@pi_Cluster", ""),
                            new SqlParameter("@pi_SubCluster", SubCluster)
                            ).ToList<LeaderBoardForMgt>();
                    }
                }
                else if (Cluster > 0)
                {
                    using (var context = new ChitaleDBContext())
                    {
                        lstLeaderBrd = context.Database.SqlQuery<LeaderBoardForMgt>("sp_LeaderBoard_MFC @pi_LoginId,@pi_ParticipantType, @pi_Datetime, @pi_City,@pi_Cluster,@pi_SubCluster",
                            new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_ParticipantType", radiobtn),
                            new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                                new SqlParameter("@pi_City", ""),
                            new SqlParameter("@pi_Cluster", Cluster),
                            new SqlParameter("@pi_SubCluster", "")
                            ).ToList<LeaderBoardForMgt>();
                    }
                }
                

                if (lstLeaderBrd != null)
                {
                    lstLeaderBrd = lstLeaderBrd.OrderBy(x => Convert.ToInt32(x.CurrentOverallRank)).ToList();
                }

            }

            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return lstLeaderBrd;

        }

        public ManagementDashboardLostOpp GetManagementDashboardLostOpp(string type)
        {
            ManagementDashboardLostOpp objLostOpp = new ManagementDashboardLostOpp();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    try
                    {
                        objLostOpp = context.Database.SqlQuery<ManagementDashboardLostOpp>("sp_GetAvgOrderToRavanaDate @pi_CustomerType", new SqlParameter("@pi_CustomerType", type)).FirstOrDefault<ManagementDashboardLostOpp>();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return objLostOpp;
        }

        public List<ManagementTGTVsACHPerformance> GetManagementTGTVsACHPerformance(string type)
        {
            List<ManagementTGTVsACHPerformance> lstData = new List<ManagementTGTVsACHPerformance>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    var lstResult = from s in context.CustomerDetails
                                    join sa in context.TgtvsAchMasters on s.CustomerId equals sa.CustomerId
                                    where sa.CustomerType == type && sa.ProductType == "Over All"
                                    select new ManagementTGTVsACHPerformance
                                    {
                                        CustomerName = s.CustomerName,
                                        VolumeAchPercentage = sa.VolumeAchPercentage
                                    };

                    lstData = lstResult.OrderByDescending(x => x.VolumeAchPercentage).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return lstData;
        }

        public List<ManagementOrderToRavanaPerformance> GetManagementOrderToRavanaPerformance(string type)
        {
            List<ManagementOrderToRavanaPerformance> objdata = new List<ManagementOrderToRavanaPerformance>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    try
                    {
                        objdata = context.Database.SqlQuery<ManagementOrderToRavanaPerformance>("sp_GetOrderToRavanaPerformance @pi_CustomerType", new SqlParameter("@pi_CustomerType", type)).ToList<ManagementOrderToRavanaPerformance>();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return objdata;
        }

        public List<OrderVsRavanaDay> GetOrderVsRavanaDayData(string Cluster, string SubCluster, string City, string type)
        {
            List<OrderVsRavanaDay> objData = new List<OrderVsRavanaDay>();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    objData = context.OrderVsRavanaDays.ToList();
                    if (Cluster == "All" && SubCluster == "All" && City == "All" && type == "0")
                    {
                        objData = context.OrderVsRavanaDays.ToList();
                    }
                    else
                    {
                        if (City != "All")
                        {
                            objData = objData.Where(x => x.City == City).ToList();
                        }                        
                        else if(SubCluster != "All")
                        {
                            objData = objData.Where(x => x.SubCluster == SubCluster).ToList();
                        }
                        else if (Cluster != "All")
                        {
                            objData = objData.Where(x => x.Cluster == Cluster).ToList();
                        }                        
                    }
                    if (type != "0")
                    {
                        objData = objData.Where(x => x.CustomerType == type).ToList();
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }

            return objData;
        }

        public List<InvoiceToOrder> GetInvoiceToOrderData(string Cluster, string SubCluster, string City, string type, string FromDate, string Todate)
        {
            List<InvoiceToOrder> objData = new List<InvoiceToOrder>();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    objData = context.InvoiceToOrders.ToList();
                    if (Cluster == "All" && SubCluster == "All" && City == "All" && FromDate == "" && Todate == "" && type == "0")
                    {
                        objData = context.InvoiceToOrders.ToList();
                    }
                    else
                    {
                        
                        if (City != "All")
                        {
                            objData = objData.Where(x => x.City == City).ToList();
                        }
                        else if (SubCluster != "All")
                        {
                            objData = objData.Where(x => x.SubCluster == SubCluster).ToList();
                        }
                        else if (Cluster != "All")
                        {                            
                            objData = objData.Where(x => x.Cluster == Cluster).ToList();
                        }
                        if (FromDate != "" && Todate != "")
                        {
                            objData = objData.Where(x => x.InvDate >= Convert.ToDateTime(FromDate) && x.InvDate <= Convert.ToDateTime(Todate)).ToList();
                        }
                        else
                        {
                            if (FromDate != "")
                            {
                                objData = objData.Where(x => x.InvDate >= Convert.ToDateTime(FromDate)).ToList();
                            }
                            if (Todate != "")
                            {
                                objData = objData.Where(x => x.InvDate >= Convert.ToDateTime(Todate)).ToList();
                            }
                        }
                    }
                    if (type != "0")
                    {
                        objData = objData.Where(x => x.CustomerType == type).ToList();
                    }

                    foreach(var item in objData)
                    {
                        item.InvAmountStr= String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.InvAmount));
                        item.OrderAmountStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.OrderAmount));
                    }


                    
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }


            foreach(var item in objData)
            {
                item.StrDate = item.InvDate.Value.ToString("dd-MM-yyyy");
            }

            return objData;
        }

    }
}
