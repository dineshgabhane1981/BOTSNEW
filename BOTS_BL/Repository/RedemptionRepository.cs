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
        public bool GenerateOTP(string Type, string CashIncentive, string Infrastructure, string Deposit, string Promotion)
        {
            bool status = false;
            using (var context = new ChitaleDBContext())
            {
                var result = context.Database.SqlQuery<GenerateOTPList>("sp_RedemptionOTP @pi_LoginId, @pi_CashIncentive, @pi_InfraStructure, @pi_Deposit, @pi_Promotion, @pi_Datetime,@pi_Type",
                              new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_CashIncentive", CashIncentive),
                              new SqlParameter("@pi_InfraStructure", Infrastructure),
                              new SqlParameter("@pi_Deposit", Deposit),
                              new SqlParameter("@pi_Promotion", Promotion),
                              new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy")),
                              new SqlParameter("@pi_Type", Type)).FirstOrDefault<GenerateOTPList>();
            }
            return status;
        }
        public RedemptionValue GetRedeemptionData(string Type)
        {
            RedemptionValue objData = new RedemptionValue();
            using (var context = new ChitaleDBContext())
            {
                objData = context.RedemptionValues.Where(x => x.Status == "00" && x.Type == Type).FirstOrDefault();
            }
            return objData;
        }
    }
}
