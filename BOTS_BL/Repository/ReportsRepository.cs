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
    public class ReportsRepository
    {
        Exceptions newexception = new Exceptions();
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        public List<MemberList> GetMemberList(string GroupId,string SearchText,string connstr)
        {
            List<MemberList> lstMember = new List<MemberList>();
            using (var context = new BOTSDBContext(connstr))
            {
                lstMember = context.Database.SqlQuery<MemberList>("sp_BOTS_MemberList @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", 
                    new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", SearchText)).ToList<MemberList>();
            }

            return lstMember;
        }

        public List<SelectListItem> GetOutletList(string GroupId, string connstr)
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {

                    var lstOutlet = context.Database.SqlQuery<OutletList>("sp_GetOutletList @pi_GroupId", new SqlParameter("@pi_GroupId", GroupId)).ToList<OutletList>();
                    foreach (var item in lstOutlet)
                    {
                        countriesItem.Add(new SelectListItem
                        {
                            Text = item.OutletName,
                            Value = Convert.ToString(item.OutletId)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return countriesItem;
        }

        public List<SelectListItem> GetBrandList(string GroupId, string connstr)
        {
            List<SelectListItem> BrandItem = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {

                    var lstOutlet = context.Database.SqlQuery<BrandList>("sp_GetBrandList @pi_GroupId", new SqlParameter("@pi_GroupId", GroupId)).ToList<BrandList>();
                    foreach (var item in lstOutlet)
                    {
                        BrandItem.Add(new SelectListItem
                        {
                            Text = item.BrandName,
                            Value = Convert.ToString(item.BrandId)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return BrandItem;

        }

        public List<OutletWise> GetOutletWiseList(string GroupId, string DateRangeFlag, string FromDate, string ToDate, string connstr)
        {
            List<OutletWise> lstOutletWise = new List<OutletWise>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    lstOutletWise = context.Database.SqlQuery<OutletWise>("sp_BOTS_OutletwiseSummary @pi_GroupId, @pi_Date, @pi_LoginId, @pi_DateRangeFlag, @pi_FromDate, @pi_ToDate",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_DateRangeFlag", DateRangeFlag),
                        new SqlParameter("@pi_FromDate", FromDate),
                        new SqlParameter("@pi_ToDate", ToDate)).ToList<OutletWise>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstOutletWise;
        }

        public List<OutletwiseTransaction> GetOutletWiseTransactionList(string GroupId,string DateRangeFlag, string FromDate, string ToDate, string OutletId, bool EnrolmentDataFlag, string connstr)
        {
            List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    lstOutletWiseTransaction = context.Database.SqlQuery<OutletwiseTransaction>("sp_BOTS_DetailedTransaction @pi_GroupId, @pi_Date, @pi_LoginId, @pi_DateRangeFlag, @pi_FromDate, @pi_ToDate, @pi_OutletId, @pi_EnrolmentDataFlag",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_DateRangeFlag", DateRangeFlag),
                        new SqlParameter("@pi_FromDate", FromDate),
                        new SqlParameter("@pi_ToDate", ToDate),
                        new SqlParameter("@pi_OutletId", OutletId),
                        new SqlParameter("@pi_EnrolmentDataFlag", EnrolmentDataFlag)).Take(50000).ToList<OutletwiseTransaction>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstOutletWiseTransaction;
        }

        public PointExpiry GetPointExpiryData(string GroupId, int month,int year, string connstr)
        {
            PointExpiry pointExpiry = new PointExpiry();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    pointExpiry = context.Database.SqlQuery<PointExpiry>("sp_BOTS_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).FirstOrDefault<PointExpiry>();                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pointExpiry;
        }

        public List<PointExpiryTxn> GetPointExpiryTxnData(string GroupId, int month, int year, string connstr)
        {
           List<PointExpiryTxn> pointExpiryTxn = new List<PointExpiryTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    pointExpiryTxn = context.Database.SqlQuery<PointExpiryTxn>("sp_BOTS_PointsExpiry1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<PointExpiryTxn>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pointExpiryTxn;
        }


        public MemberSearch GetMeamberSearchData(string GroupId,string searchData, string connstr)
        {
            MemberSearch memberSearch = new MemberSearch();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    memberSearch = context.Database.SqlQuery<MemberSearch>("sp_BOTS_MemberSearch @pi_GroupId, @pi_Date, @pi_LoginId, @pi_SearchData",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_SearchData", searchData)).FirstOrDefault<MemberSearch>();

                    memberSearch.lstMemberSearchTxn = context.Database.SqlQuery<MemberSearchTxn>("sp_BOTS_MemberSearch1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_SearchData",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_SearchData", searchData)).ToList<MemberSearchTxn>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return memberSearch;
        }

        public Celebrations GetCelebrationsData(string GroupId, string connstr)
        {
            Celebrations celebrationsData = new Celebrations();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    celebrationsData = context.Database.SqlQuery<Celebrations>("sp_BOTS_Celebrate @pi_GroupId, @pi_Date, @pi_LoginId",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", "")).FirstOrDefault<Celebrations>();                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return celebrationsData;
        }

        public List<CelebrationsMoreDetails> GetCelebrationsTxnData(string GroupId, int month,int type, string connstr)
        {
            List<CelebrationsMoreDetails> celebrationTxnsData = new List<CelebrationsMoreDetails>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    celebrationTxnsData = context.Database.SqlQuery<CelebrationsMoreDetails>("sp_BOTS_Celebrate1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month,@pi_Type",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Type", type)).ToList<CelebrationsMoreDetails>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return celebrationTxnsData;
        }

    }
}

