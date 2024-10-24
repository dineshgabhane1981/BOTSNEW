﻿using System;
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
        Exceptions newexception = new Exceptions();
        public CustomerDetail GetCustomerDetail(string CustomerId, string CustomerType)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objCustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == CustomerId && x.CustomerType == CustomerType).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetail");
            }
            return objCustomerDetail;
        }        

        public DashboardParticipantsummary GetSummeryDetails(string CustomerId, string CustomerType,  string IsBTD)
        {
            DashboardParticipantsummary objDashboardsummary = new DashboardParticipantsummary();
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            try
            {
                using (var context = new ChitaleDBContext())
                {

                    objDashboardsummary = context.Database.SqlQuery<DashboardParticipantsummary>("sp_DashboardSeg_Participant @pi_CustomerId,@pi_CustomerType,@pi_Date,@pi_BTDType",
                                new SqlParameter("@pi_CustomerId", CustomerId),
                                new SqlParameter("@pi_CustomerType", CustomerType),
                                new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                                  new SqlParameter("@pi_BTDType", IsBTD)
                                ).FirstOrDefault<DashboardParticipantsummary>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSummeryDetails");
            }
            return objDashboardsummary;
        }

        public DashboardLostOpp GetDashboardLostOppData(string CustomerId, bool IsBTD)
        {
            DashboardLostOpp objDashboardLostOpp = new DashboardLostOpp();
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardLostOppData");
            }
            return objDashboardLostOpp;
        }

        public DashboardTarget GetTargetData(string CustomerId, bool IsValue)
        {
            DashboardTarget objDashboardTarget = new DashboardTarget();
            List<TargetMaster> objTargetMaster = new List<TargetMaster>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objTargetMaster = context.TargetMasters.Where(x => x.CustomerId == CustomerId && x.TargetFromDate.Value.Month == DateTime.Today.Month
                     && x.TargetFromDate.Value.Year == DateTime.Today.Year).ToList();

                    if (objTargetMaster != null)
                    {
                        if (IsValue)
                        {
                            objDashboardTarget.TargetValueWise = (decimal)objTargetMaster.Sum(x => x.TargetProductAmt);
                            objDashboardTarget.AchiveValueWise = 0;
                            objDashboardTarget.TargetVolumeWise = 0;
                            objDashboardTarget.AchiveVolumeWise = 0;
                        }
                        else
                        {
                            objDashboardTarget.TargetVolumeWise = (decimal)objTargetMaster.Sum(x => x.TargetProductVolume);
                            objDashboardTarget.AchiveVolumeWise = 0;
                            objDashboardTarget.TargetValueWise = 0;
                            objDashboardTarget.AchiveValueWise = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTargetData");
            }
            return objDashboardTarget;
        }

        public DashboardRank GetRankData(string CustomerId, string CustomerType)
        {
            DashboardRank objRank = new DashboardRank();
            DataSet retVal = new DataSet();
            //SqlConnection sqlConn = new SqlConnection("Data Source=DESKTOP-JOLRHRS\\SQLEXPRESS;Initial Catalog=ChitaleLive;Integrated Security=True");
            SqlConnection sqlConn = new SqlConnection("Data Source=65.0.4.176;Initial Catalog=B2BDemo;user id = Dinesh; password=Sneeti@0303");
            SqlCommand cmdReport = new SqlCommand("sp_Dashboard", sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
            try
            {
                using (cmdReport)
                {
                    SqlParameter param1 = new SqlParameter("pi_CustomerId", CustomerId);
                    SqlParameter param2 = new SqlParameter("pi_CustomerType", CustomerType);
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
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["RankNoPenaltyPoints"])))
                    {
                        objRank.RankPoints = Convert.ToDecimal(dt.Rows[0]["RankNoPenaltyPoints"]);
                    }
                    else
                    {
                        objRank.RankPoints = 0;
                    }
                    objRank.LostRank = Convert.ToInt32(dt.Rows[0]["RankNoPenalty"]);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRankData");
            }

            return objRank;
        }

        public void AddException(tblErrorLog objerrorlog)
        {
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    context.tblErrorLogs.Add(objerrorlog);
                    context.SaveChanges();
                }
                catch(Exception ex)
                {

                }

            }


        }
    }
}
