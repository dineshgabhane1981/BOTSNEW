using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace BOTS_BL.Repository
{
    public class LoyaltyKPIsRepository
    {
        Exceptions newexception = new Exceptions();
        DashboardRepository DR = new DashboardRepository();
        public LoyaltyKPIs GetobjLoyaltyKPIsData(string GroupId, string connstr,string loginId)
        {
            LoyaltyKPIs objLoyaltyKPIs = new LoyaltyKPIs();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objLoyaltyKPIs = context.Database.SqlQuery<LoyaltyKPIs>("sp_BOTS_LoyaltyPerfromance @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year, @pi_OutletId,@pi_FromDate,@pi_ToDate",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_Month", DateTime.Now.Month),
                        new SqlParameter("@pi_Year", DateTime.Now.Year),
                        new SqlParameter("@pi_OutletId", ""),
                        new SqlParameter("@pi_FromDate", ""), 
                        new SqlParameter("@pi_ToDate", "")).FirstOrDefault<LoyaltyKPIs>();
                    }
                    //else if(GroupId == "1087")
                    //{
                       
                    //    var AllData = DR.GetExecutiveSummaryAllData(GroupId, connstr);

                    //    var TotalBiz = Convert.ToInt64(AllData.Select(x => x.TotalSpend).Sum());
                    //    var Redemption = Convert.ToInt64((AllData.Where(y=> y.BurnAmtWithPts>0).Select(x => x.BurnAmtWithPts).Sum()));
                    //    var Referrals = Convert.ToInt64((AllData.Where(y=> y.EnrolledBy == "DLCReferral").Select(x => x.TotalSpend).Sum()));
                    //    var Campaign = Convert.ToInt64((context.tblCampaignMasters.Select(x=> x.BusinessGenerated).Sum()));
                    //    var SMSBlastWA = Convert.ToInt64((context.tblPromoBlastMasters.Select(x=> x.BusinessGenerated).Sum()));
                    //    var NewMWPRegistration = Convert.ToInt64((AllData.Where(x => x.EnrolledBy == "DLCWalkIn" || x.EnrolledBy == "DLCScoialMedia").Select(y => y.TotalSpend).Sum()));
                    //    var LoyaltyBiz = TotalBiz + Redemption + Referrals + Campaign + SMSBlastWA + NewMWPRegistration;

                    //    if(TotalBiz > 0 && LoyaltyBiz > 0)
                    //    {
                    //        var LoyaltyToBiz = LoyaltyBiz / TotalBiz;
                    //    }

                    //    objLoyaltyKPIs.LoyaltyBiz = LoyaltyBiz;
                    //    objLoyaltyKPIs.TotalBiz = TotalBiz;
                    //    objLoyaltyKPIs.Redemption = Redemption;
                    //    objLoyaltyKPIs.Referrals = Referrals;
                    //    objLoyaltyKPIs.Campaigns = Campaign;
                    //    objLoyaltyKPIs.SMSBlastWA = SMSBlastWA;
                    //    objLoyaltyKPIs.NewMWPRegistration = NewMWPRegistration;

                    //}
                    else
                    {
                        objLoyaltyKPIs = context.Database.SqlQuery<LoyaltyKPIs>("sp_BOTS_LoyaltyPerfromance @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year, @pi_OutletId,@pi_FromDate,@pi_ToDate",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", DateTime.Now.Month),
                        new SqlParameter("@pi_Year", DateTime.Now.Year),
                        new SqlParameter("@pi_OutletId", ""),
                        new SqlParameter("@pi_FromDate", ""),
                        new SqlParameter("@pi_ToDate", "")).FirstOrDefault<LoyaltyKPIs>();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetobjLoyaltyKPIsData");
            }

            return objLoyaltyKPIs;

        }
    }
}
