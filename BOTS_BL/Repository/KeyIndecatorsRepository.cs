using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class KeyIndecatorsRepository
    {
        Exceptions newexception = new Exceptions();
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        public OnlyOnce GetOnlyOnceData(string GroupId, string outletId, string connstr)
        {
            OnlyOnce objOnlyOnce = new OnlyOnce();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objOnlyOnce = context.Database.SqlQuery<OnlyOnce>("sp_BOTS_OnlyOnce @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<OnlyOnce>();
                    //objOnlyOnce.lstOnlyOnceTxn = context.Database.SqlQuery<OnlyOnceTxn>("sp_BOTS_OnlyOnce1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).ToList<OnlyOnceTxn>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objOnlyOnce;
        }
        public List<OnlyOnceTxn> GetOnlyOnceTxnData(string GroupId, string outletId, string type, string connstr)
        {
            List<OnlyOnceTxn> objOnlyOnceTxn = new List<OnlyOnceTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    
                    objOnlyOnceTxn = context.Database.SqlQuery<OnlyOnceTxn>("sp_BOTS_OnlyOnce1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_Type", 
                        new SqlParameter("@pi_GroupId", GroupId), 
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), 
                        new SqlParameter("@pi_LoginId", ""), 
                        new SqlParameter("@pi_OutletId", outletId),
                        new SqlParameter("@pi_Type", type)).ToList<OnlyOnceTxn>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objOnlyOnceTxn;
        }

        public NonTransactingCls GetNonTransactingData(string GroupId, string outletId, string connstr)
        {
            NonTransactingCls objNonTransacting = new NonTransactingCls();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objNonTransacting = context.Database.SqlQuery<NonTransactingCls>("sp_BOTS_NonTransacting @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<NonTransactingCls>();
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonTransacting;
        }

        public List<NonTransactingTxn> GetNonTransactingTxnData(string GroupId, string outletId, string type, string connstr)
        {
            List<NonTransactingTxn> objNonTransactingTxn = new List<NonTransactingTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    
                    objNonTransactingTxn = context.Database.SqlQuery<NonTransactingTxn>("sp_BOTS_NonTransacting1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_Type", 
                        new SqlParameter("@pi_GroupId", GroupId), 
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), 
                        new SqlParameter("@pi_LoginId", ""), 
                        new SqlParameter("@pi_OutletId", outletId),
                        new SqlParameter("@pi_Type", type)).ToList<NonTransactingTxn>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonTransactingTxn;
        }

        public NonRedemptionCls GetNonRedemptionData(string GroupId, string connstr)
        {
            NonRedemptionCls objNonRedemption = new NonRedemptionCls();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objNonRedemption = context.Database.SqlQuery<NonRedemptionCls>("sp_BOTS_NonRedeeming @pi_GroupId, @pi_Date, @pi_LoginId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", "")).FirstOrDefault<NonRedemptionCls>();
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonRedemption;
        }

        public List<NonRedemptionTxn> GetNonRedemptionTxnData(string GroupId, int type,int daysType, string connstr)
        {
            List<NonRedemptionTxn> objNonRedemptionTxn = new List<NonRedemptionTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objNonRedemptionTxn = context.Database.SqlQuery<NonRedemptionTxn>("sp_BOTS_NonRedeeming1 @pi_GroupId, @pi_Date, @pi_LoginId,@pi_Type,@pi_DaysType", 
                        new SqlParameter("@pi_GroupId", GroupId), 
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), 
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Type", type),
                        new SqlParameter("@pi_DaysType", daysType)).ToList<NonRedemptionTxn>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonRedemptionTxn;
        }
        public List<MemberPageNewRegisterationData> GetNewRegistrationData(string GroupId,string SourceId, string connstr)
        {
            List<MemberPageNewRegisterationData> objNonRedemptionTxn = new List<MemberPageNewRegisterationData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objNonRedemptionTxn = context.Database.SqlQuery<MemberPageNewRegisterationData>("sp_BOTS_MemberWebPage4 @pi_GroupId,@source ",
                        new SqlParameter("@pi_GroupId", GroupId),                       
                        new SqlParameter("@source", SourceId)
                      ).ToList<MemberPageNewRegisterationData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonRedemptionTxn;
        }
        public MemberWebPage GetMemberWebPageData(string GroupId, string connstr)
        {
            MemberWebPage objMemberWebPage = new MemberWebPage();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objMemberWebPage = context.Database.SqlQuery<MemberWebPage>("sp_BOTS_MemberWebPage @pi_GroupId, @pi_Date, @pi_LoginId", 
                        new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", "")).FirstOrDefault<MemberWebPage>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objMemberWebPage;
        }

        public MemberPage GetMemberPageData(string GroupId, string connstr)
        {
            MemberPage objMemberPage = new MemberPage();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objMemberPage = context.Database.SqlQuery<MemberPage>("sp_BOTS_MemberWebPage1 @pi_GroupId, @pi_Date, @pi_LoginId",
                        new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", "")).FirstOrDefault<MemberPage>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objMemberPage;
        }

        public List<MemberPageRefData> GetMemberPageRefData(string GroupId, string type, string connstr)
        {
            List<MemberPageRefData> objMemberPageRefData = new List<MemberPageRefData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objMemberPageRefData = context.Database.SqlQuery<MemberPageRefData>("sp_BOTS_MemberWebPage2 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Type", 
                        new SqlParameter("@pi_GroupId", GroupId), 
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), 
                        new SqlParameter("@pi_LoginId", ""), 
                        new SqlParameter("@pi_Type", type)).ToList<MemberPageRefData>();                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objMemberPageRefData;
        }

        public MembersInformation GetMemberMisinformationData(string GroupId, string connstr)
        {
            MembersInformation objMembersInformation = new MembersInformation();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objMembersInformation = context.Database.SqlQuery<MembersInformation>("sp_BOTS_MemberInformation @pi_GroupId, @pi_Date, @pi_LoginId",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", "")).FirstOrDefault<MembersInformation>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objMembersInformation;
        }

    }
}
