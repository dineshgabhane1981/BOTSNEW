using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class ParticipantRepository
    {
        ChitaleException newexception = new ChitaleException();
        public List<ParticipantList> GetParticipantList(string CustomerId, string CustomerType)
        {
            
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            try
            {                
                using (var context = new ChitaleDBContext())
                {
                    lstparticipantLists= context.Database.SqlQuery<ParticipantList>("Chitale_ParticipantList @pi_LoginId, @pi_Datetime, @pi_CustomerId, @pi_CustomerType", 
                        new SqlParameter("@pi_LoginId", ""), 
                        new SqlParameter("@pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd")), 
                        new SqlParameter("@pi_CustomerId", CustomerId),
                        new SqlParameter("@pi_CustomerType", CustomerType)).ToList<ParticipantList>();


                    if(lstparticipantLists!=null)
                    {
                        lstparticipantLists = lstparticipantLists.OrderBy(x => Convert.ToInt32(x.Rank)).ToList();
                    }
                }


            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return lstparticipantLists;
        }


        public RedemptionModel GetRedemptionData(string CustomerId)
        {
            RedemptionModel objData = new RedemptionModel();
            CustomerDetail objCustomerDetail = new CustomerDetail();
            using (var context = new ChitaleDBContext())
            {
                objCustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                if (objCustomerDetail != null)
                {
                    objData.DepositData = objCustomerDetail.Deposit;
                    objData.CreditData = objCustomerDetail.CashIncentive;
                    objData.InfraData = objCustomerDetail.InfraStructure;
                    objData.PromoData = objCustomerDetail.Promotion;

                    objData.DepositDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.Deposit));
                    objData.CreditDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.CashIncentive));
                    objData.InfraDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.InfraStructure));
                    objData.PromoDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.Promotion));
                }
            }
            return objData;
        }

        public bool GenerateOTP(OTPMaintenance objOTP)
        {
            bool status = false;
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    context.OTPMaintenances.Add(objOTP);
                    context.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }

            return status;
        }

        public string GetOTP(string MobileNo)
        {
            string OTP = string.Empty;
            using (var context = new ChitaleDBContext())
            {
                OTP = context.OTPMaintenances.Where(x => x.MobileNo == MobileNo).OrderByDescending(z => z.Datetime).Select(y => y.OTP).FirstOrDefault();
            }
            return OTP;
        }

        public bool RedeemptionRequest(tblRedemptionRequest objRedeem)
        {
            bool status = false;
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    context.tblRedemptionRequests.Add(objRedeem);
                    context.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }

            return status;
        }


        public List<OrderVsRavanaDay> GetOrderVsRavanaDayData(string Cluster, string SubCluster, string City, string FromDate, string Todate, string CustomerId)
        {
            List<OrderVsRavanaDay> objData = new List<OrderVsRavanaDay>();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                   
                    objData = context.OrderVsRavanaDays.Where(x=> x.CustomerId == CustomerId).ToList();

                    if (Cluster == "All" && SubCluster == "All" && City == "All" && FromDate == "" && Todate == "")
                    {
                        objData = context.OrderVsRavanaDays.Where(x => x.CustomerId == CustomerId).ToList();
                    }
                    else
                    {
                        if (Cluster != "All")
                        {
                            objData = objData.Where(x => x.Cluster == Cluster).ToList();
                        }
                        else if (SubCluster != "All")
                        {
                            objData = objData.Where(x => x.SubCluster == SubCluster).ToList();
                        }
                        else if (City != "All")
                        {
                            objData = objData.Where(x => x.City == City).ToList();
                        }
                        if (FromDate != "" && Todate != "")
                        {
                            objData = objData.Where(x => x.Date >= Convert.ToDateTime(FromDate) && x.Date <= Convert.ToDateTime(Todate)).ToList();
                        }
                        else
                        {
                            if (FromDate != "")
                            {
                                objData = objData.Where(x => x.Date >= Convert.ToDateTime(FromDate)).ToList();
                            }
                            if (Todate != "")
                            {
                                objData = objData.Where(x => x.Date >= Convert.ToDateTime(Todate)).ToList();
                            }
                        }
                    }
                     
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }

            return objData;
        }

    }
}
