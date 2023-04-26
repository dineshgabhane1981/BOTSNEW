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
    public class RedemptionRepository
    {
        Exceptions newexception = new Exceptions();
        public List<GenerateOTPList> GenerateOTP(string Type, string CashIncentive, string Infrastructure, string Deposit, string Promotion)
        {
            List<GenerateOTPList> objData = new List<GenerateOTPList>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objData = context.Database.SqlQuery<GenerateOTPList>("sp_RedemptionOTP @pi_LoginId, @pi_CashIncentive, @pi_InfraStructure, @pi_Deposit, @pi_Promotion, @pi_Datetime,@pi_Type",
                                  new SqlParameter("@pi_LoginId", ""),
                                  new SqlParameter("@pi_CashIncentive", CashIncentive),
                                  new SqlParameter("@pi_InfraStructure", Infrastructure),
                                  new SqlParameter("@pi_Deposit", Deposit),
                                  new SqlParameter("@pi_Promotion", Promotion),
                                  new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                                  new SqlParameter("@pi_Type", Type)).ToList<GenerateOTPList>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GenerateOTP");
            }
            return objData;
        }
        public RedemptionValue GetRedeemptionData(string Type)
        {
            RedemptionValue objData = new RedemptionValue();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objData = context.RedemptionValues.Where(x => x.Status == "00" && x.Type == Type).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRedeemptionData");
            }
            return objData;
        }

        public ChitaleSPResponse ValidateOTP(string Type, string CashIncentive, string Infrastructure, string Deposit, string Promotion,string OTP1, string OTP2)
        {
            ChitaleSPResponse objData = new ChitaleSPResponse();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objData = context.Database.SqlQuery<ChitaleSPResponse>("sp_RedemptionValues @pi_LoginId, @pi_CashIncentive, @pi_InfraStructure, @pi_Deposit, @pi_Promotion, @pi_Datetime,@pi_Type,@pi_OTPValue1,@pi_OTPValue2",
                                  new SqlParameter("@pi_LoginId", ""),
                                  new SqlParameter("@pi_CashIncentive", CashIncentive),
                                  new SqlParameter("@pi_InfraStructure", Infrastructure),
                                  new SqlParameter("@pi_Deposit", Deposit),
                                  new SqlParameter("@pi_Promotion", Promotion),
                                  new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                                  new SqlParameter("@pi_Type", Type),
                                  new SqlParameter("@pi_OTPValue1", OTP1),
                                  new SqlParameter("@pi_OTPValue2", OTP2)).FirstOrDefault<ChitaleSPResponse>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ValidateOTP");
            }
            return objData;
        }
    }
}
