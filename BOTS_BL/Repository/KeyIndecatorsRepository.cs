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
        CustomerRepository CR = new CustomerRepository();
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        public OnlyOnce GetOnlyOnceData(string GroupId, string outletId, string connstr,string loginId)
        {
            OnlyOnce objOnlyOnce = new OnlyOnce();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objOnlyOnce = context.Database.SqlQuery<OnlyOnce>("sp_BOTS_OnlyOnce @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<OnlyOnce>();
                    }
                    else
                    {
                        objOnlyOnce = context.Database.SqlQuery<OnlyOnce>("sp_BOTS_OnlyOnce @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<OnlyOnce>();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objOnlyOnce;
        }
        public List<OnlyOnceTxn> GetOnlyOnceTxnData(string GroupId, string outletId, string type, string connstr,string loginId)
        {
            List<OnlyOnceTxn> objOnlyOnceTxn = new List<OnlyOnceTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objOnlyOnceTxn = context.Database.SqlQuery<OnlyOnceTxn>("sp_BOTS_OnlyOnce1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_Type",
                       new SqlParameter("@pi_GroupId", GroupId),
                       new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                       new SqlParameter("@pi_LoginId", loginId),
                       new SqlParameter("@pi_OutletId", outletId),
                       new SqlParameter("@pi_Type", type)).ToList<OnlyOnceTxn>();
                    }
                    else
                    {
                        objOnlyOnceTxn = context.Database.SqlQuery<OnlyOnceTxn>("sp_BOTS_OnlyOnce1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_Type",
                       new SqlParameter("@pi_GroupId", GroupId),
                       new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                       new SqlParameter("@pi_LoginId", ""),
                       new SqlParameter("@pi_OutletId", outletId),
                       new SqlParameter("@pi_Type", type)).ToList<OnlyOnceTxn>();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objOnlyOnceTxn;
        }

        public NonTransactingCls GetNonTransactingData(string GroupId, string outletId, string connstr,string loginId)
        {
            NonTransactingCls objNonTransacting = new NonTransactingCls();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objNonTransacting = context.Database.SqlQuery<NonTransactingCls>("sp_BOTS_NonTransacting @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<NonTransactingCls>();
                    }
                    else
                    {
                        objNonTransacting = context.Database.SqlQuery<NonTransactingCls>("sp_BOTS_NonTransacting @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<NonTransactingCls>();
                    }
                    

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonTransacting;
        }

        public List<NonTransactingTxn> GetNonTransactingTxnData(string GroupId, string outletId, string type, string connstr,string loginId)
        {
            List<NonTransactingTxn> objNonTransactingTxn = new List<NonTransactingTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objNonTransactingTxn = context.Database.SqlQuery<NonTransactingTxn>("sp_BOTS_NonTransacting1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_Type",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_OutletId", outletId),
                        new SqlParameter("@pi_Type", type)).ToList<NonTransactingTxn>();
                    }
                    else
                    {
                        objNonTransactingTxn = context.Database.SqlQuery<NonTransactingTxn>("sp_BOTS_NonTransacting1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_Type",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_OutletId", outletId),
                        new SqlParameter("@pi_Type", type)).ToList<NonTransactingTxn>();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonTransactingTxn;
        }

        public NonRedemptionCls GetNonRedemptionData(string GroupId, string connstr,string loginId)
        {
            NonRedemptionCls objNonRedemption = new NonRedemptionCls();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objNonRedemption = context.Database.SqlQuery<NonRedemptionCls>("sp_BOTS_NonRedeeming @pi_GroupId, @pi_Date, @pi_LoginId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", loginId)).FirstOrDefault<NonRedemptionCls>();
                    }
                    else
                    {
                        objNonRedemption = context.Database.SqlQuery<NonRedemptionCls>("sp_BOTS_NonRedeeming @pi_GroupId, @pi_Date, @pi_LoginId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", "")).FirstOrDefault<NonRedemptionCls>();
                    }
                   

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonRedemption;
        }

        public List<NonRedemptionTxn> GetNonRedemptionTxnData(string GroupId, int type, int daysType, string connstr, string loginId)
        {
            List<NonRedemptionTxn> objNonRedemptionTxn = new List<NonRedemptionTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objNonRedemptionTxn = context.Database.SqlQuery<NonRedemptionTxn>("sp_BOTS_NonRedeeming1 @pi_GroupId, @pi_Date, @pi_LoginId,@pi_Type,@pi_DaysType",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_Type", type),
                        new SqlParameter("@pi_DaysType", daysType)).ToList<NonRedemptionTxn>();
                    }
                    else
                    {
                        objNonRedemptionTxn = context.Database.SqlQuery<NonRedemptionTxn>("sp_BOTS_NonRedeeming1 @pi_GroupId, @pi_Date, @pi_LoginId,@pi_Type,@pi_DaysType",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Type", type),
                        new SqlParameter("@pi_DaysType", daysType)).ToList<NonRedemptionTxn>();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objNonRedemptionTxn;
        }
        public List<MemberPageNewRegisterationData> GetNewRegistrationData(string GroupId, string SourceId, string connstr)
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

        public MembersInformation GetMemberMisinformationData(string GroupId, string connstr,string loginId)
        {
            MembersInformation objMembersInformation = new MembersInformation();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        objMembersInformation = context.Database.SqlQuery<MembersInformation>("sp_BOTS_MemberInformation @pi_GroupId, @pi_Date, @pi_LoginId",
                       new SqlParameter("@pi_GroupId", GroupId),
                       new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                       new SqlParameter("@pi_LoginId", loginId)).FirstOrDefault<MembersInformation>();
                    }
                    else
                    {
                        objMembersInformation = context.Database.SqlQuery<MembersInformation>("sp_BOTS_MemberInformation @pi_GroupId, @pi_Date, @pi_LoginId",
                       new SqlParameter("@pi_GroupId", GroupId),
                       new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                       new SqlParameter("@pi_LoginId", "")).FirstOrDefault<MembersInformation>();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objMembersInformation;
        }

        public List<DLCCreation> GetDLCCreationData(string GroupId)
        {
            List<DLCCreation> lstData = new List<DLCCreation>();
            var connectionString = CR.GetCustomerConnString(GroupId);
            using (var context = new BOTSDBContext(connectionString))
            {
                lstData = context.DLCCreations.ToList();
            }

            return lstData;
        }
        public List<SelectListItem> GetMemberType()
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();

            countriesItem.Add(new SelectListItem
            {
                Text = "New",
                Value = "1"
            });
            countriesItem.Add(new SelectListItem
            {
                Text = "Existing",
                Value = "2"
            });

            return countriesItem;
        }
        public List<SelectListItem> GetPointsExpiryType()
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();

            countriesItem.Add(new SelectListItem
            {
                Text = "Fixed",
                Value = "1"
            });
            countriesItem.Add(new SelectListItem
            {
                Text = "Variable",
                Value = "2"
            });

            return countriesItem;
        }

        public bool UpdateDLCCreation(DLCCreation objDLCCreation, string GroupId)
        {
            bool status = false;
            try
            {
                var connStr = CR.GetCustomerConnString(GroupId);
                using (var context = new BOTSDBContext(connStr))
                {
                    context.DLCCreations.AddOrUpdate(objDLCCreation);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return status;
        }

        public DLCCreation GetDLCForEdit(long SlNo, string GroupId)
        {
            DLCCreation objData = new DLCCreation();
            var connStr = CR.GetCustomerConnString(GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                objData = context.DLCCreations.Where(x => x.SlNo == SlNo).FirstOrDefault();
            }
            return objData;
        }
    
        public int GetDLCRecordCount(string GroupId)
        {
            int count = 0;
            var connStr = CR.GetCustomerConnString(GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                count = context.DLCCreations.Count();                
            }

            return count;
        }
    
    }
}
