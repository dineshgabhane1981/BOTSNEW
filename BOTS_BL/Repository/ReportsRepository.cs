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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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

        public List<OutletwiseTransaction> GetOutletWiseTransactionList(string GroupId,string DateRangeFlag, string FromDate, string ToDate, string OutletId, string EnrolmentDataFlag, string connstr)
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
                        new SqlParameter("@pi_EnrolmentDataFlag", EnrolmentDataFlag)).ToList<OutletwiseTransaction>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstOutletWiseTransaction;
        }

        public PointExpiryTmp GetPointExpiryData(string GroupId, int month,int year, string connstr)
        {
            PointExpiryTmp pointExpiry = new PointExpiryTmp();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    pointExpiry = context.Database.SqlQuery<PointExpiryTmp>("sp_BOTS_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).FirstOrDefault<PointExpiryTmp>();                    
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
                newexception.AddException(ex, GroupId);
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
                          new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),                      
                          new SqlParameter("@pi_LoginId",""),
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

        public List<SelectListItem> GetOutletListByBrandId(string BrandId, string connstr)
        {
            List<SelectListItem> outletlist = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {

                    var lstOutlet = context.Database.SqlQuery<OutletList>("sp_GetOutletListByBrandId @pi_BrandId", new SqlParameter("@pi_BrandId", BrandId)).ToList<OutletList>();
                    foreach (var item in lstOutlet)
                    {
                        outletlist.Add(new SelectListItem
                        {
                            Text = item.OutletName,
                            Value = Convert.ToString(item.OutletId)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, BrandId);
            }
            return outletlist;

        }

        public int GetTotalMemberCount(string GroupId, string connstr)
        {
            int total = 0;
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {

                    total = context.CustomerDetails.Where(x => x.CustomerThrough != "1").Count();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return total;
        }
        public List<SelectListItem> GetEnrolledList(string GroupId, string connstr)
        {
            List<SelectListItem> mySkills = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    mySkills.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                    mySkills.Add(new SelectListItem { Text = "Recently Enrolled", Value = "1" });
                    mySkills.Add(new SelectListItem { Text = "31-90 ", Value = "2" });
                    mySkills.Add(new SelectListItem { Text = "91-180", Value = "3" });
                    mySkills.Add(new SelectListItem { Text = "181-365", Value = "4" });
                    mySkills.Add(new SelectListItem { Text = "1-2 Years", Value = "5" });
                    mySkills.Add(new SelectListItem { Text = "2-3 Years", Value = "6" });
                    mySkills.Add(new SelectListItem { Text = "3+ Years", Value = "7" });
                    mySkills.Add(new SelectListItem { Text = "Custom", Value = "8" });

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return mySkills;
        }
        public List<SelectListItem> GetNonTransactedList(string GroupId, string connstr)
        {
            List<SelectListItem> mySkills = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    mySkills.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                    mySkills.Add(new SelectListItem { Text = "Within 30", Value = "1" });
                    mySkills.Add(new SelectListItem { Text = "31-60 ", Value = "2" });
                    mySkills.Add(new SelectListItem { Text = "61-90", Value = "3" });
                    mySkills.Add(new SelectListItem { Text = "91-180", Value = "4" });
                    mySkills.Add(new SelectListItem { Text = "181-365", Value = "5" });
                    mySkills.Add(new SelectListItem { Text = "365+", Value = "6" });
                    mySkills.Add(new SelectListItem { Text = "Custom", Value = "7" });

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return mySkills;
        }
        public List<SelectListItem> GetSpendList(string GroupId, string connstr)
        {
            List<SelectListItem> mySkills = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    mySkills.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                    mySkills.Add(new SelectListItem { Text = "Less than", Value = "1" });
                    mySkills.Add(new SelectListItem { Text = "More than", Value = "2" });
                    mySkills.Add(new SelectListItem { Text = "Between", Value = "3" });

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return mySkills;
        }
        public List<SelectListItem> GetRedeemedList(string GroupId, string connstr)
        {
            List<SelectListItem> mySkills = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    mySkills.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                    mySkills.Add(new SelectListItem { Text = "Redeemed", Value = "1" });
                    mySkills.Add(new SelectListItem { Text = "Not Redeemed", Value = "2" });

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return mySkills;
        }
        public List<SelectListItem> GetGenderList(string GroupId, string connstr)
        {
            List<SelectListItem> mySkills = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    mySkills.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                    mySkills.Add(new SelectListItem { Text = "Male", Value = "M" });
                    mySkills.Add(new SelectListItem { Text = "Female", Value = "F" });
                    mySkills.Add(new SelectListItem { Text = "Not Present", Value = "NP" });

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return mySkills;
        }
        public List<SelectListItem> GetSourseList(string GroupId, string connstr)
        {
            List<SelectListItem> mySkills = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    mySkills.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                    mySkills.Add(new SelectListItem { Text = "Walk-in", Value = "2" });
                    mySkills.Add(new SelectListItem { Text = "Referred", Value = "3" });
                    mySkills.Add(new SelectListItem { Text = "Bulk Upload", Value = "1" });
                    mySkills.Add(new SelectListItem { Text = "MWP", Value = "6" });

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return mySkills;
        }

        public int GetSliceAndDiceFilteredData(string Gender, string Age_min, string Age_max, string source, string Enroll_min, string Enroll_max, string Nontransacted_max, string Nontransacted_min, int Spend_min, int Spend_max, int txncount_min, int txncount_max, int pointBaln_min, int pointBaln_max, string Redeem, int TicketSize_min, int TicketSize_max, string Brand, object[] outletId, string GroupId, string connstr)
        {

            List<TransactionMaster> objtrans = new List<TransactionMaster>();
            List<CustomerDetail> objcust = new List<CustomerDetail>();

            var transcount = 0;
            using (var context = new BOTSDBContext(connstr))
            {
                objcust = context.CustomerDetails.Where(x => x.CustomerThrough != "1").ToList();


                try
                {
                    if (Enroll_max != "0")
                    {
                        DateTime FromDate;
                        DateTime ToDate = DateTime.Now.Date;
                        FromDate = ToDate.Date;
                        switch (Enroll_max)
                        {
                            case "Recently Enrolled":
                                ToDate = ToDate.AddDays(-30);
                                break;
                            case "31-90":
                                ToDate = ToDate.AddDays(-31);
                                FromDate = FromDate.AddDays(-60);
                                break;
                            case "91-180":
                                ToDate = ToDate.AddDays(-91);
                                FromDate = FromDate.AddDays(-180);
                                break;
                            case "181-365":
                                ToDate = ToDate.AddDays(-181);
                                FromDate = FromDate.AddDays(-365);
                                break;
                            case "1-2 Years":
                                ToDate = ToDate.AddDays(-365);
                                FromDate = DateTime.Now.AddYears(-2).Date;
                                break;
                            case "2-3 Years":
                                ToDate = DateTime.Now.AddYears(-2);
                                FromDate = DateTime.Now.AddYears(-3);
                                break;
                            case "3+ Years":
                                ToDate = DateTime.Now.AddYears(-3);
                                FromDate = DateTime.Now.AddYears(-25);
                                break;
                            case "Custom":
                                ToDate = Convert.ToDateTime(Enroll_min);
                                FromDate = Convert.ToDateTime(Enroll_max);
                                break;

                        }

                        objcust = objcust.Where(x => x.DOJ >= FromDate && x.DOJ <= ToDate).ToList();
                        
                    }
                    if (Gender != "0")
                    {
                        objcust = objcust.Where(x => x.Gender == Gender).ToList();                       

                    }
                    if (Age_min != "0" && Age_max != "0")
                    {
                        int age_min = Convert.ToInt32(Age_min);
                        int age_max = Convert.ToInt32(Age_max);
                        int currentyear = DateTime.Now.Year;
                        DateTime Birthyearmin = DateTime.Now.AddYears(-age_min).Date;
                        DateTime Birthyearmax = DateTime.Now.AddYears(-age_max).Date;
                        objcust = objcust.Where(x => x.DOB <= Birthyearmin && x.DOB >= Birthyearmax).ToList();                        

                    }

                    else if (Age_min != "0")
                    {
                        int age_min = Convert.ToInt32(Age_min);
                        int currentyear = DateTime.Now.Year;
                        DateTime Birthyear = DateTime.Now.AddYears(-age_min).Date;
                        objcust = objcust.Where(x => x.DOB <= Birthyear).ToList();                       

                    }
                    else if (Age_max != "0")
                    {
                        int age_max = Convert.ToInt32(Age_max);
                        int currentyear = DateTime.Now.Year;
                        DateTime Birthyear = DateTime.Now.AddYears(-age_max).Date;
                        objcust = objcust.Where(x => x.DOB >= Birthyear).ToList();
                       
                    }
                    if (source != "0")
                    {
                        objcust = objcust.Where(x => x.CustomerThrough == source).ToList();                       
                    }
                    if (pointBaln_min > 0 && pointBaln_max > 0)
                    {
                        objcust = objcust.Where(x => x.Points >= pointBaln_min && x.Points <= pointBaln_max).ToList();

                    }
                    else if (pointBaln_min > 0)
                    {
                        objcust = objcust.Where(x => x.Points < pointBaln_max).ToList();

                    }
                    else if (pointBaln_max > 0)
                    {
                        objcust = objcust.Where(x => x.Points > pointBaln_max).ToList();

                    }
                    if (Redeem == "1")
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        objcust = (from c in objcust
                                   join t in objtrans on c.CustomerId equals t.CustomerId
                                   where t.TransType.Contains("2")
                                   select c).Distinct().ToList();
                    }
                    else if (Redeem == "2")
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        objcust = (from c in objcust
                                   join t in objtrans on c.CustomerId equals t.CustomerId
                                   where t.TransType.Contains("1")
                                   select c).Distinct().ToList();
                    }
                    if (txncount_min > 0 && txncount_max > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into grouped
                                     where grouped.Count() > txncount_min && grouped.Count() < txncount_max
                                     select grouped.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                    }

                    else if (txncount_min > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into grouped
                                     where grouped.Count() < txncount_min
                                     select grouped.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                    }
                    else if (txncount_max > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into grouped
                                     where grouped.Count() > txncount_max
                                     select grouped.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }
                    if (Spend_min > 0 && Spend_max > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) > Spend_min && g.Sum(x => x.InvoiceAmt) < Spend_max
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }

                    else if (Spend_min > 0)
                    {

                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) < Spend_min
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                    }
                    else if (Spend_max > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) > Spend_max
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }
                    if (TicketSize_min > 0 && TicketSize_max > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) / g.Count() > TicketSize_min && g.Sum(x => x.InvoiceAmt) / g.Count() < TicketSize_max
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }

                    else if (TicketSize_min > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) / g.Count() < TicketSize_min
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                    }
                    else if (TicketSize_max > 0)
                    {
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) / g.Count() > TicketSize_max
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }
                    if (Nontransacted_min != "0")
                    {
                        DateTime ToDate;
                        DateTime FromDate = DateTime.Now.Date;
                        ToDate = FromDate.Date;
                        switch (Nontransacted_min)
                        {
                            case "Within 30":
                                FromDate = FromDate.AddDays(-30);
                                break;
                            case "31-60":
                                FromDate = FromDate.AddDays(-60);
                                ToDate = ToDate.AddDays(-31);
                                break;
                            case "61-90":
                                FromDate = FromDate.AddDays(-90);
                                ToDate = ToDate.AddDays(-61);
                                break;
                            case "91-180":
                                FromDate = FromDate.AddDays(-180);
                                ToDate = ToDate.AddDays(-91);
                                break;
                            case "180-365":
                                FromDate = FromDate.AddDays(-365);
                                ToDate = ToDate.AddDays(-181);
                                break;
                            case "365+":
                                FromDate = DateTime.Now.AddYears(-25).Date;
                                ToDate = DateTime.Now.AddDays(-365);
                                break;
                            case "Custom":
                                FromDate = Convert.ToDateTime(Nontransacted_min);
                                ToDate = Convert.ToDateTime(Nontransacted_max);
                                break;

                        }
                        string strNm = "B";
                        objtrans = context.TransactionMasters.Where(x => !x.InvoiceNo.StartsWith(strNm)).ToList();
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     orderby g.Key
                                     where g.Max(x => x.Datetime) >= FromDate && g.Max(x => x.Datetime) <= ToDate
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }
                    if (Brand != "0")
                    {
                        var item = context.Database.SqlQuery<string>("sp_BOTS_GetBrandIdBySubStrings @brandId",
                           new SqlParameter("@brandId", Brand)).ToList();
                        objcust = objcust.Where(x => item.Contains(x.CustomerId)).ToList();

                    }
                    if (outletId.Count() > 0)
                    {
                        var str = String.Join(",", outletId);
                        var item = context.Database.SqlQuery<string>("sp_BOTS_GetOutletIdBySubStrings @outletId",
                            new SqlParameter("@outletId", str)).ToList();
                        objcust = objcust.Where(x => item.Contains(x.CustomerId)).ToList();

                    }
                    transcount = objcust.Count();

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }

            }
            return transcount;

        }

    }
}

