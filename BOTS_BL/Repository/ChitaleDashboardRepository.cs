using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
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
                    objDashboardLostOpp.LateOrder = (decimal)objtransactionmaster.Where(x => x.TxnType == "Purchase").Sum(x => x.PenaltyPoints);
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
                    objTargetMaster = context.TargetMasters.Where(x => x.CustomerId == CustomerId && x.TargetFromDate.Value.Month == DateTime.Today.Month - 1
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

        public DashboardRank GetRankData(string CustomerId)
        {
            DashboardRank objRank = new DashboardRank();
            DataSet retVal = new DataSet();            
            SqlConnection sqlConn = new SqlConnection("Data Source=LAPTOP-SIDRDIDV;Initial Catalog=ChitaleLive;Integrated Security=True");
            SqlCommand cmdReport = new SqlCommand("sp_Dashboard", sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
            using (cmdReport)
            {
                SqlParameter param1 = new SqlParameter("pi_CustomerId", CustomerId);
                SqlParameter param2 = new SqlParameter("pi_CustomerType", "Distributors");
                SqlParameter param3 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy"));
                SqlParameter param4 = new SqlParameter("pi_Year", DateTime.Now.Year);
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.Parameters.Add(param1);
                cmdReport.Parameters.Add(param2);
                cmdReport.Parameters.Add(param3);
                cmdReport.Parameters.Add(param4);
                daReport.Fill(retVal);
                DataTable dt = retVal.Tables[1];
                objRank.CurrentRank = Convert.ToInt32(dt.Rows[0]["Rank"]);
                objRank.LastMonthRank = Convert.ToInt32(dt.Rows[0]["LastMonthRank"]);
                objRank.RankPoints = Convert.ToDecimal(dt.Rows[0]["RankNoPenaltyPoints"]);
                objRank.LostRank = Convert.ToInt32(dt.Rows[0]["RankNoPenalty"]);
            }

            return objRank;
        }

        public ParticipantList GetParticipantList(string CustomerId,string CustomerType)
        {
            ParticipantList objparticipantList = new ParticipantList();
            try
            {
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = new SqlConnection("Data Source=LAPTOP-SIDRDIDV;Initial Catalog=ChitaleLive;Integrated Security=True");
                SqlCommand cmdReport = new SqlCommand("sp_KYBLoadParticipant_MFC", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    SqlParameter param1 = new SqlParameter("@pi_LoginId", "");
                    SqlParameter param2 = new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy"));
                    SqlParameter param3 = new SqlParameter("@pi_DataType", "");
                    SqlParameter param4 = new SqlParameter("@pi_City", "");
                    SqlParameter param5 = new SqlParameter("@pi_Cluster", "");
                    SqlParameter param6 = new SqlParameter("@pi_SubCluster", "");
                    SqlParameter param7 = new SqlParameter("@pi_Id ", CustomerId);
                    SqlParameter param8 = new SqlParameter("@pi_ParticipantType", CustomerType);
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);
                    cmdReport.Parameters.Add(param2);
                    cmdReport.Parameters.Add(param3);
                    cmdReport.Parameters.Add(param4);
                    cmdReport.Parameters.Add(param5);
                    cmdReport.Parameters.Add(param6);
                    cmdReport.Parameters.Add(param7);
                    cmdReport.Parameters.Add(param8);
                    daReport.Fill(retVal);
                    DataTable dt = retVal.Tables[1];
                    DataTable dt1 = new DataTable();
                    DataRow dr1 = dt1.NewRow();
                    dt1.Columns.Add("Id");
                    dt1.Columns.Add("Name");
                    dt1.Columns.Add("participantType");
                    dt1.Columns.Add("Rank");
                    dt1.Columns.Add("subcluster");
                    dt1.Columns.Add("Totalpoints");
                    dt1.Columns.Add("city");
                    dt1.Columns.Add("cluster");
                                      
                     foreach (DataRow dr in dt.Rows)                   
                    {
                        CustomerDetail objcustomerDetail = new CustomerDetail();
                            using (var context = new ChitaleDBContext())
                            {
                                
                                string customerid = Convert.ToString(dr["Id"]);
                                objcustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == customerid).FirstOrDefault();                            
                                dr1["Id"] = Convert.ToString(dr["Id"]);
                                dr1["Name"] = objcustomerDetail.CustomerName;
                                dr1["participantType"] = objcustomerDetail.CustomerType;
                                dr1["Rank"] = Convert.ToInt32(dr["CurrentRank"]);
                                dr1["subcluster"] = objcustomerDetail.SubCluster;
                                dr1["Totalpoints"] = (decimal)objcustomerDetail.Points;
                                dr1["city"] = objcustomerDetail.City;
                                dr1["cluster"] = objcustomerDetail.Cluster;

                                //string customerid = Convert.ToString(dt.Rows[0]["Id"]);
                                //objcustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == customerid).FirstOrDefault();                            
                                //objparticipantList.Id = Convert.ToString(dt.Rows[0]["Id"]);
                                //objparticipantList.Name = objcustomerDetail.CustomerName;
                                //objparticipantList.participantType = objcustomerDetail.CustomerType;
                                //objparticipantList.Rank = Convert.ToInt32(dt.Rows[0]["CurrentRank"]);
                                //objparticipantList.subcluster = objcustomerDetail.SubCluster;
                                //objparticipantList.Totalpoints = (decimal)objcustomerDetail.Points;
                                //objparticipantList.city = objcustomerDetail.City;
                                //objparticipantList.cluster = objcustomerDetail.Cluster;
                                
                            }
                            dt1.Rows.Add(dr1.ItemArray);
                      

                    }

                   // objparticipantList = dt1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objparticipantList;
        }
    }
}
