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
        Exceptions newexception = new Exceptions();
        public List<ParticipantList> GetParticipantList(string CustomerId, string CustomerType)
        {
            
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            try
            {
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = new SqlConnection("Data Source=DESKTOP-JOLRHRS\\SQLEXPRESS;Initial Catalog=ChitaleLive;Integrated Security=True");
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

                    foreach (DataRow dr in dt.Rows)
                    {
                        ParticipantList objparticipantList = new ParticipantList();
                        CustomerDetail objcustomerDetail = new CustomerDetail();
                        using (var context = new ChitaleDBContext())
                        {

                            string customerid = Convert.ToString(dr["Id"]);
                            objcustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == customerid).FirstOrDefault();
                            objparticipantList.Id = Convert.ToString(dr["Id"]);
                            objparticipantList.Name = objcustomerDetail.CustomerName;
                            objparticipantList.participantType = objcustomerDetail.CustomerType;
                            objparticipantList.Rank = Convert.ToInt32(dr["CurrentRank"]);
                            objparticipantList.subcluster = objcustomerDetail.SubCluster;
                            objparticipantList.Totalpoints = (decimal)objcustomerDetail.Points;
                            objparticipantList.city = objcustomerDetail.City;
                            objparticipantList.cluster = objcustomerDetail.Cluster;

                        }

                        lstparticipantLists.Add(objparticipantList);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
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
    }
}
