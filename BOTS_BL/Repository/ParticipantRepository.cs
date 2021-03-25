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
    }
}
