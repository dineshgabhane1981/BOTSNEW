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
using BOTS_BL.Models.Reports;
using System.Globalization;
using System.Net.Mail;
using LinqKit;
using System.Data.Entity.Core.Objects;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using System.Web;
using System.Collections;

namespace BOTS_BL.Repository
{
    public class ReportsRepository
    {
        Exceptions newexception = new Exceptions();
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        public List<MemberList> GetMemberList(string GroupId, string SearchText, string connstr,string loginId)
        {
            List<MemberList> lstMember = new List<MemberList>();
            using (var context = new BOTSDBContext(connstr))
            {
                if (GroupId == "1086")
                {
                    lstMember = context.Database.SqlQuery<MemberList>("sp_BOTS_MemberList @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId",
                    new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_OutletId", SearchText)).ToList<MemberList>();
                }
                else
                {
                    lstMember = context.Database.SqlQuery<MemberList>("sp_BOTS_MemberList @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId",
                    new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", SearchText)).ToList<MemberList>();
                }
                
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
                        //if (!item.OutletName.ToLower().Contains("admin"))
                        //{
                            countriesItem.Add(new SelectListItem
                            {
                                Text = item.OutletName,
                                Value = Convert.ToString(item.OutletId)
                            });
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return countriesItem;
        }
        public List<SelectListItem> GetOutletListForSliceAndDice(string GroupId, string connstr)
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    countriesItem.Add(new SelectListItem
                    {
                        Text = "All",
                        Value = "1"
                    });
                    var lstOutlet = context.Database.SqlQuery<OutletList>("sp_GetOutletList @pi_GroupId", new SqlParameter("@pi_GroupId", GroupId)).ToList<OutletList>();
                    foreach (var item in lstOutlet)
                    {
                        countriesItem.Add(new SelectListItem { Text = item.OutletName, Value = Convert.ToString(item.OutletId) });
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

        public List<OutletWise> GetOutletWiseList(string GroupId, string DateRangeFlag, string FromDate, string ToDate, string connstr, string loginId)
        {
            List<OutletWise> lstOutletWise = new List<OutletWise>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    context.Database.CommandTimeout = 120;
                    if (GroupId == "1086")
                    {
                        lstOutletWise = context.Database.SqlQuery<OutletWise>("sp_BOTS_OutletwiseSummary @pi_GroupId, @pi_Date, @pi_LoginId, @pi_DateRangeFlag, @pi_FromDate, @pi_ToDate",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_DateRangeFlag", DateRangeFlag),
                        new SqlParameter("@pi_FromDate", FromDate),
                        new SqlParameter("@pi_ToDate", ToDate)).ToList<OutletWise>();
                    }
                    else
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return lstOutletWise;
        }

        public List<OutletwiseTransaction> GetOutletWiseTransactionList(string GroupId, string DateRangeFlag, string FromDate, string ToDate, string OutletId, string EnrolmentDataFlag, string connstr, string loginId)
        {
            List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        lstOutletWiseTransaction = context.Database.SqlQuery<OutletwiseTransaction>("sp_BOTS_DetailedTransaction @pi_GroupId, @pi_Date, @pi_LoginId, @pi_DateRangeFlag, @pi_FromDate, @pi_ToDate, @pi_OutletId, @pi_EnrolmentDataFlag",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_DateRangeFlag", DateRangeFlag),
                        new SqlParameter("@pi_FromDate", FromDate),
                        new SqlParameter("@pi_ToDate", ToDate),
                        new SqlParameter("@pi_OutletId", OutletId),
                        new SqlParameter("@pi_EnrolmentDataFlag", EnrolmentDataFlag)).ToList<OutletwiseTransaction>();
                    }
                    else
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstOutletWiseTransaction;
        }

        public PointExpiryTmp GetPointExpiryData(string GroupId, int month, int year, string connstr,string loginId)
        {
            PointExpiryTmp pointExpiry = new PointExpiryTmp();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        pointExpiry = context.Database.SqlQuery<PointExpiryTmp>("sp_BOTS_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).FirstOrDefault<PointExpiryTmp>();
                    }
                    else
                    {
                        pointExpiry = context.Database.SqlQuery<PointExpiryTmp>("sp_BOTS_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).FirstOrDefault<PointExpiryTmp>();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pointExpiry;
        }

        public List<PointExpiryTxn> GetPointExpiryTxnData(string GroupId, int month, int year, string connstr,string loginId)
        {
            List<PointExpiryTxn> pointExpiryTxn = new List<PointExpiryTxn>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        pointExpiryTxn = context.Database.SqlQuery<PointExpiryTxn>("sp_BOTS_PointsExpiry1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<PointExpiryTxn>();
                    }
                    else
                    {
                        pointExpiryTxn = context.Database.SqlQuery<PointExpiryTxn>("sp_BOTS_PointsExpiry1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<PointExpiryTxn>();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return pointExpiryTxn;
        }

        public MemberSearch GetMeamberSearchData(string GroupId, string searchData, string connstr, string loginId)
        {
            MemberSearch memberSearch = new MemberSearch();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    memberSearch = context.Database.SqlQuery<MemberSearch>("sp_BOTS_MemberSearch @pi_GroupId, @pi_Date, @pi_LoginId, @pi_SearchData",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_SearchData", searchData)).FirstOrDefault<MemberSearch>();

                    memberSearch.lstMemberSearchTxn = context.Database.SqlQuery<MemberSearchTxn>("sp_BOTS_MemberSearch1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_SearchData",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_SearchData", searchData)).ToList<MemberSearchTxn>();

                    //memberSearch.lstMemberSearchTxn = memberSearch.lstMemberSearchTxn.OrderByDescending(x => x.TxnDatetime).ToList();
                    foreach (var item in memberSearch.lstMemberSearchTxn)
                    {
                        if (!string.IsNullOrEmpty(item.TxnDatetime))
                        {
                            var subDate = Convert.ToString(item.TxnDatetime);
                            var convertedDate = DateTime.ParseExact(subDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                            .ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            item.TxnDatetime = convertedDate;
                        }
                        if (!string.IsNullOrEmpty(item.TxnUpdateDate))
                        {
                            var subDate = Convert.ToString(item.TxnUpdateDate).Substring(0, 10);
                            var convertedDate = DateTime.ParseExact(subDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            item.TxnUpdateDate = convertedDate;
                        }
                    }
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
        public List<CelebrationsMoreDetails> GetCelebrationsTxnData(string GroupId, int month, int type, string connstr)
        {
            List<CelebrationsMoreDetails> celebrationTxnsData = new List<CelebrationsMoreDetails>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    celebrationTxnsData = context.Database.SqlQuery<CelebrationsMoreDetails>("sp_BOTS_Celebrate1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month,@pi_Type",
                        new SqlParameter("@pi_GroupId", GroupId),
                          new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
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
                    var names = new string[] { "2", "4", "5", "7" };
                    total = (from c in context.CustomerDetails where (names.Contains(c.CustomerThrough) && c.Status == "00") select c).Count();
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

        public CustomerIdListAndCount GetSliceAndDiceFilteredData(DateTime Fromdtforall, DateTime Todtforall, string Enroll_min, string Enroll_max, string Nontransacted_min, int Spend_min, int Spend_max, int txncount_min, int txncount_max, int pointBaln_min, int pointBaln_max, string Redeem, string Brand, object[] outletId, string GroupId, string connstr)
        {
            CustomerIdListAndCount objcount = new CustomerIdListAndCount();
            List<TransactionMaster> objtrans = new List<TransactionMaster>();
            List<CustomerDetail> objcust = new List<CustomerDetail>();
            // List<customerIdDetails> lstcustomerId = new List<customerIdDetails>();
            // var transcount = 0;
            using (var context = new BOTSDBContext(connstr))
            {
                /*
                var names1 = new string[] { "2", "4", "5", "7" };
                var customer = (from c in context.CustomerDetails where (names1.Contains(c.CustomerThrough) && c.Status == "00") select c);

                var predicate = PredicateBuilder.True<CustomerDetail>();
                var predicate2 = PredicateBuilder.True<TransactionMaster>();

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

                    predicate = predicate.And(x => x.DOJ >= FromDate && x.DOJ <= ToDate);

                }
                if (Gender != "0")
                {
                    predicate = predicate.And(i => i.Gender == Gender);
                }

                if (Age_min != "0" && Age_max != "0")
                {
                    int age_min;
                    int age_max;
                    Int32.TryParse(Age_min, out age_min);
                    Int32.TryParse(Age_max, out age_max);
                    DateTime Birthyearmin = DateTime.Now.AddYears(-age_min).Date;
                    DateTime Birthyearmax = DateTime.Now.AddYears(-age_max).Date;
                    predicate = predicate.And(i => i.DOB <= Birthyearmin && i.DOB >= Birthyearmax);
                }
                if (Age_min != "0")
                {
                    int age_min = Convert.ToInt32(Age_min);
                    int currentyear = DateTime.Now.Year;
                    DateTime Birthyear = DateTime.Now.AddYears(-age_min).Date;
                    predicate = predicate.And(x => x.DOB <= Birthyear);
                }
                if (Age_max != "0")
                {
                    int age_max = Convert.ToInt32(Age_max);
                    int currentyear = DateTime.Now.Year;
                    DateTime Birthyear = DateTime.Now.AddYears(-age_max).Date;
                    predicate = predicate.And(x => x.DOB >= Birthyear);
                }
                if (source != "0")
                {
                    predicate = predicate.And(x => x.CustomerThrough == source);
                }
                if (pointBaln_min > 0 && pointBaln_max > 0)
                {
                    predicate = predicate.And(x => x.Points >= pointBaln_min && x.Points <= pointBaln_max);

                }
                else if (pointBaln_min > 0)
                {
                    predicate = predicate.And(x => x.Points < pointBaln_max);

                }
                else if (pointBaln_max > 0)
                {
                    predicate = predicate.And(x => x.Points > pointBaln_max);

                }
                var filterCustomerlist = customer.Where(predicate).Select(i => i).ToList();

                var type1 = new string[] { "1", "2" };
                var transaction = (from x in context.TransactionMasters where (type1.Contains(x.TransType) && x.InvoiceNo != "B_Birthday" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_ProfileUpdate" && x.InvoiceNo != "B_ReferralBonus" && x.InvoiceNo != "B_RefereePoints" && x.InvoiceNo != "B_GiftingPoints" && x.InvoiceNo != "Bonus") select x);
                if (Redeem == "1")
                {
                    predicate2 = predicate2.And(p => p.TransType == "2");
                }
                if (Redeem == "2")
                {
                    predicate2 = predicate2.And(p => p.TransType == "1");
                }

                var filterallData = transaction.Where(predicate2).Select(i => i).ToList();

                // var filterallData = transaction.Where(predicate2).Select(i => i).ToList();

                var final = (from c in filterCustomerlist
                             join t in filterallData on c.CustomerId equals t.CustomerId
                             select new
                             {
                                 c.CustomerId,
                                 c.CustomerName,
                                 c.DOJ,
                                 c.EnrollingOutlet,
                                 c.MobileNo,
                                 c.Gender,
                                 t.TransType,
                                 t.TxnAmt,
                                 t.Datetime,
                                 t.CustomerPoints

                             }).ToList();
                return View(patients.ToList()); */

                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                /*
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
                 if (TicketSize_min > 0 && TicketSize_max > 0)
                 {
                     // var type = new string[] { "1", "2" };
                     // objtrans = (from x in context.TransactionMasters where (type.Contains(x.TransType) && x.InvoiceNo != "B_Birthday" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_ProfileUpdate" && x.InvoiceNo != "B_ReferralBonus" && x.InvoiceNo != "B_RefereePoints" && x.InvoiceNo != "B_GiftingPoints" && x.InvoiceNo != "Bonus") select x).ToList();

                     var items = (from t in objtrans
                                  group t by t.CustomerId into g
                                  where g.Sum(x => x.InvoiceAmt) / g.Count() > TicketSize_min && g.Sum(x => x.InvoiceAmt) / g.Count() < TicketSize_max
                                  select g.Key).ToList();
                     objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                 }
                 else if (TicketSize_min > 0)
                 {
                     //  var type = new string[] { "1", "2" };
                     //objtrans = (from x in context.TransactionMasters where (type.Contains(x.TransType) && x.InvoiceNo != "B_Birthday" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_ProfileUpdate" && x.InvoiceNo != "B_ReferralBonus" && x.InvoiceNo != "B_RefereePoints" && x.InvoiceNo != "B_GiftingPoints" && x.InvoiceNo != "Bonus") select x).ToList();

                     var items = (from t in objtrans
                                  group t by t.CustomerId into g
                                  where g.Sum(x => x.InvoiceAmt) / g.Count() < TicketSize_min
                                  select g.Key).ToList();
                     objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                 }
                 else if (TicketSize_max > 0)
                 {
                     // var type = new string[] { "1", "2" };
                     //objtrans = (from x in context.TransactionMasters where (type.Contains(x.TransType) && x.InvoiceNo != "B_Birthday" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_Anniversary" && x.InvoiceNo != "B_ProfileUpdate" && x.InvoiceNo != "B_ReferralBonus" && x.InvoiceNo != "B_RefereePoints" && x.InvoiceNo != "B_GiftingPoints" && x.InvoiceNo != "Bonus") select x).ToList();

                     var items = (from t in objtrans
                                  group t by t.CustomerId into g
                                  where g.Sum(x => x.InvoiceAmt) / g.Count() > TicketSize_max
                                  select g.Key).ToList();
                     objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                 } */
                var names = new string[] { "2", "4", "5", "7" };
                objcust = (from c in context.CustomerDetails where (names.Contains(c.CustomerThrough) && c.Status == "00") select c).ToList();
                objtrans = context.Database.SqlQuery<TransactionMaster>("select * from TransactionMaster where InvoiceNo != 'B_Birthday' and InvoiceNo!= 'B_Anniversary'and InvoiceNo!= 'B_ProfileUpdate'and InvoiceNo!= 'B_ReferralBonus' and InvoiceNo!= 'B_GiftingPoints'and InvoiceNo!= 'Bonus'and TransType in(1, 2)").ToList();
                                
                try
                {
                    if (Enroll_max != "0")
                    {
                        if (!string.IsNullOrEmpty(Enroll_max))
                        {
                            DateTime FromDate;
                            DateTime ToDate = DateTime.Now.Date;
                            FromDate = ToDate.Date;

                            if (Enroll_min != "0" && Enroll_max != "0")
                            {
                                ToDate = Convert.ToDateTime(Enroll_min);
                                FromDate = Convert.ToDateTime(Enroll_max);
                            }
                            else
                            {
                                int days = Convert.ToInt32(Enroll_max);
                                FromDate = FromDate.AddDays(-days);
                            }
                            //switch (Enroll_max)
                            //{
                            //    case "Recently Enrolled":
                            //        FromDate = FromDate.AddDays(-30);
                            //        break;
                            //    case "31-90":
                            //        ToDate = ToDate.AddDays(-31);
                            //        FromDate = FromDate.AddDays(-60);
                            //        break;
                            //    case "91-180":
                            //        ToDate = ToDate.AddDays(-91);
                            //        FromDate = FromDate.AddDays(-180);
                            //        break;
                            //    case "181-365":
                            //        ToDate = ToDate.AddDays(-181);
                            //        FromDate = FromDate.AddDays(-365);
                            //        break;
                            //    case "1-2 Years":
                            //        ToDate = ToDate.AddDays(-365);
                            //        FromDate = DateTime.Now.AddYears(-2).Date;
                            //        break;
                            //    case "2-3 Years":
                            //        ToDate = DateTime.Now.AddYears(-2);
                            //        FromDate = DateTime.Now.AddYears(-3);
                            //        break;
                            //    case "3+ Years":
                            //        ToDate = DateTime.Now.AddYears(-3);
                            //        FromDate = DateTime.Now.AddYears(-25);
                            //        break;
                            //    case "Custom":
                            //        ToDate = Convert.ToDateTime(Enroll_min);
                            //        FromDate = Convert.ToDateTime(Enroll_max);
                            //        break;

                            //}

                            objcust = objcust.Where(x => x.DOJ >= FromDate && x.DOJ <= ToDate).ToList();
                        }
                    }


                    if (pointBaln_min > 0 && pointBaln_max > 0)
                    {
                        //lstcustomerId = (from c in objcust where c.Points >= pointBaln_min && c.Points <= pointBaln_max
                        //                        select new customerIdDetails
                        //                        { 
                        //                            CustomerId = c.CustomerId,
                        //                        }).ToList();
                        objcust = objcust.Where(x => x.Points >= pointBaln_min && x.Points <= pointBaln_max).ToList();

                    }
                    else if (pointBaln_min > 0)
                    {
                        objcust = objcust.Where(x => x.Points < pointBaln_min).ToList();

                    }
                    else if (pointBaln_max > 0)
                    {
                        objcust = objcust.Where(x => x.Points > pointBaln_max).ToList();

                    }

                    if (Redeem == "1")
                    {
                        objcust = (from c in objcust
                                   join t in objtrans on c.CustomerId equals t.CustomerId
                                   where t.TransType.Contains("2") && t.Datetime >= Fromdtforall && t.Datetime <= Todtforall
                                   select c).Distinct().ToList();
                    }
                    else if (Redeem == "2")
                    {
                        objcust = (from c in objcust
                                   join t in objtrans on c.CustomerId equals t.CustomerId
                                   where t.TransType.Contains("1") && t.Datetime >= Fromdtforall && t.Datetime <= Todtforall
                                   select c).Distinct().ToList();
                    }

                    if (txncount_min > 0 && txncount_max > 0)
                    {

                        var items = (from t in objtrans
                                     group t by t.CustomerId into grouped
                                     where grouped.Count() > txncount_min && grouped.Count() < txncount_max
                                     select grouped.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                    }
                    else if (txncount_min > 0)
                    {
                        var items = (from t in objtrans
                                     group t by t.CustomerId into grouped
                                     where grouped.Count() < txncount_min
                                     select grouped.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                    }
                    else if (txncount_max > 0)
                    {
                        var items = (from t in objtrans
                                     group t by t.CustomerId into grouped
                                     where grouped.Count() > txncount_max
                                     select grouped.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }

                    if (Spend_min > 0 && Spend_max > 0)
                    {

                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) > Spend_min && g.Sum(x => x.InvoiceAmt) < Spend_max
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                        //var item = (from t in objtrans join c in objcust on t.CustomerId equals c.CustomerId
                        //            where t.Datetime >= Fromdtforall && t.Datetime <= Todtforall
                        //            group t by new
                        //            {
                        //                t.CustomerId
                        //            } into g
                        //            select new { 
                        //            customerid = g.Sum(t =>t.InvoiceAmt) > Spend_min && g.Sum(t => t.InvoiceAmt) < Spend_max
                        //            }).ToList();
                    }
                    else if (Spend_min > 0)
                    {
                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) < Spend_min
                                     select g.Key).ToList();
                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                    }
                    else if (Spend_max > 0)
                    {

                        var items = (from t in objtrans
                                     group t by t.CustomerId into g
                                     where g.Sum(x => x.InvoiceAmt) > Spend_max
                                     select g.Key).ToList();

                        objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();

                    }


                    if (Nontransacted_min != "0")
                    {
                        if (!string.IsNullOrEmpty(Nontransacted_min))
                        {
                            DateTime ToDate;
                            DateTime FromDate = DateTime.Now.Date;
                            ToDate = FromDate.Date;
                            int days = Convert.ToInt32(Nontransacted_min);
                            FromDate = FromDate.AddDays(-days);

                            //switch (Nontransacted_min)
                            //{
                            //    case "Within 30":
                            //        FromDate = FromDate.AddDays(-30);
                            //        break;
                            //    case "31-60":
                            //        FromDate = FromDate.AddDays(-60);
                            //        ToDate = ToDate.AddDays(-31);
                            //        break;
                            //    case "61-90":
                            //        FromDate = FromDate.AddDays(-90);
                            //        ToDate = ToDate.AddDays(-61);
                            //        break;
                            //    case "91-180":
                            //        FromDate = FromDate.AddDays(-180);
                            //        ToDate = ToDate.AddDays(-91);
                            //        break;
                            //    case "180-365":
                            //        FromDate = FromDate.AddDays(-365);
                            //        ToDate = ToDate.AddDays(-181);
                            //        break;
                            //    case "365+":
                            //        FromDate = DateTime.Now.AddYears(-25).Date;
                            //        ToDate = DateTime.Now.AddDays(-365);
                            //        break;
                            //    case "Custom":
                            //        FromDate = Convert.ToDateTime(Nontransacted_min);
                            //        ToDate = Convert.ToDateTime(Nontransacted_max);
                            //        break;

                            //}

                            var items = (from t in objtrans
                                         group t by t.CustomerId into g
                                         orderby g.Key
                                         where g.Max(x => x.Datetime) >= FromDate && g.Max(x => x.Datetime) <= ToDate
                                         select g.Key).ToList();
                            objcust = objcust.Where(x => items.Contains(x.CustomerId)).ToList();
                        }
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
                        if (str == "1")
                        {
                            var lstoutletId = GetOutletList(GroupId, connstr);
                            str = String.Join(",", lstoutletId.Select(x => x.Value).ToArray());
                        }

                        var item = context.Database.SqlQuery<string>("sp_BOTS_GetOutletIdBySubStrings @outletId",
                                new SqlParameter("@outletId", str)).ToList();
                        objcust = objcust.Where(x => item.Contains(x.CustomerId)).ToList();

                    }
                    objtrans = (from t in objtrans
                                join c in objcust on t.CustomerId equals c.CustomerId
                                select new TransactionMaster
                                {
                                }).ToList();

                    objcount.Filteredcount = objcust.Count();
                    objcount.txncount = objtrans.Count();
                    objcount.lstcustomerDetails = objcust;




                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }

            }
            return objcount;

        }
        
        public List<CustomerTypeReport> GenerateCustomerTypeReport(object[] ColumnId, List<CustomerDetail> lstcustdetails, string GroupId, string connstr)
        {
            List<CustomerTypeReport> lstcusttype = new List<CustomerTypeReport>();

            List<TransactionMaster> objtrans = new List<TransactionMaster>();
            var str = String.Join(",", ColumnId);
            var query = from val in str.Split(',')
                        select int.Parse(val);
            using (var context = new BOTSDBContext(connstr))
            {
               // var predicate = PredicateBuilder.True<TransactionMaster>();
                objtrans = context.Database.SqlQuery<TransactionMaster>("select * from TransactionMaster where InvoiceNo != 'B_Birthday' and InvoiceNo!= 'B_Anniversary'and InvoiceNo!= 'B_ProfileUpdate'and InvoiceNo!= 'B_ReferralBonus' and InvoiceNo!= 'B_GiftingPoints'and InvoiceNo!= 'Bonus'and TransType in(1, 2)").ToList();

                if (lstcustdetails != null)
                {

                    foreach (var id in lstcustdetails)
                    {
                        CustomerTypeReport objcustomertypereport = new CustomerTypeReport();
                        //foreach (int c in query)
                        //{
                        //outletname
                        if (str.Contains("1"))
                        {
                            var outletname = context.Database.SqlQuery<string>("select OutletDetails.OutletName from CustomerDetails join OutletDetails on CustomerDetails.EnrollingOutlet = OutletDetails.OutletId where CustomerId=" + id.CustomerId + "").FirstOrDefault();

                            objcustomertypereport.OutletName = outletname;

                        }
                        //mobileno
                        if (str.Contains("2"))
                        {
                            objcustomertypereport.MobileNo = id.MobileNo;

                        }
                        //customername
                        if (str.Contains("3"))
                        {
                            objcustomertypereport.CustomerName = id.CustomerName;
                        }
                        //firsttxndt
                        if (str.Contains("4"))
                        {
                            var itemfirstdt = context.Database.SqlQuery<DateTime>("select Min(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();
                            objcustomertypereport.FirstTxnDate = itemfirstdt;
                        }
                        //lasttxndt
                        if (str.Contains("5"))
                        {
                            var Lasttxndate = context.Database.SqlQuery<DateTime>("select Max(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

                            objcustomertypereport.LastTxnDate = Lasttxndate;
                        }
                        //nooftxn
                        if (str.Contains("6"))
                        {
                            var NoofTxn = context.Database.SqlQuery<int>("select count(CustomerId) from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

                            objcustomertypereport.NoOfTxn = NoofTxn;
                        }
                        //enrolleddate
                        if (str.Contains("7"))
                        {
                            objcustomertypereport.EnrolledDate = id.DOJ;

                        }
                        //availablepoints
                        if (str.Contains("8"))
                        {
                            objcustomertypereport.TotalAvailablePoints = id.Points;

                        }
                        //totalearn
                        if (str.Contains("9"))
                        {
                            var TotalEarn = context.Database.SqlQuery<decimal>("select sum(pointsearned) from transactionmaster where TransType in(1, 2) and CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

                            objcustomertypereport.TotalEarn = TotalEarn;
                        }
                        //totalburn
                        if (str.Contains("10"))
                        {
                            var TotalBurn = context.Database.SqlQuery<decimal>("select sum(PointsBurned) from transactionmaster where TransType ='2' and CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

                            objcustomertypereport.TotalBurn = TotalBurn;

                        }
                        //totalspend
                        if (str.Contains("11"))
                        {
                            var Totalspend = context.Database.SqlQuery<decimal>("select sum(InvoiceAmt) from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();
                            objcustomertypereport.TotalSpend = Totalspend;
                        }
                        //bonuspoints
                        if (str.Contains("12"))
                        {
                            //var txn = context.Database.SqlQuery<TransactionMaster>("select * from transactionmaster where InvoiceNo in('B_Birthday', 'B_Anniversary','B_ProfileUpdate','B_ReferralBonus', 'B_GiftingPoints', 'Bonus')and TransType in(1, 2)").ToList();

                            //var item = (from x in txn
                            //            group x by new { x.CustomerId } into g
                            //            select new
                            //            {

                            //                Bonus = g.Sum(x => x.PointsEarned)
                            //            }).FirstOrDefault();
                            var Bonus = context.Database.SqlQuery<decimal>("select sum(pointsearned) from transactionmaster where InvoiceNo in('B_Birthday', 'B_Anniversary','B_ProfileUpdate','B_ReferralBonus', 'B_GiftingPoints', 'Bonus')and TransType in(1, 2) and CustomerId=" + id.CustomerId + " group by customerid  ").FirstOrDefault();
                            objcustomertypereport.BonusPoints = Bonus;
                        }
                        
                       
                        lstcusttype.Add(objcustomertypereport);
                        // }
                    }

                }
            }
            return lstcusttype;
        }
        public List<TransactionTypeReport> GenerateTxnTypeReport(object[] ColumnId, List<CustomerDetail> lstcustdetails, string GroupId, string connstr)
        {

            List<TransactionTypeReport> lstTxntype = new List<TransactionTypeReport>();
            List<TransactionMaster> objtrans = new List<TransactionMaster>();
            var str = String.Join(",", ColumnId);
            var query = from val in str.Split(',')
                        select int.Parse(val);
            using (var context = new BOTSDBContext(connstr))
            {                
                objtrans = context.Database.SqlQuery<TransactionMaster>("select * from TransactionMaster where InvoiceNo != 'B_Birthday' and InvoiceNo!= 'B_Anniversary'and InvoiceNo!= 'B_ProfileUpdate'and InvoiceNo!= 'B_ReferralBonus' and InvoiceNo!= 'B_GiftingPoints'and InvoiceNo!= 'Bonus'and TransType in(1, 2)").ToList();

                if (lstcustdetails != null)
                {

                    foreach (var id in lstcustdetails)
                    {
                        TransactionTypeReport objtranstypereport = new TransactionTypeReport();                      
                        
                        ListEarn objearn = new ListEarn();
                        ListBurn objburn = new ListBurn();
                        ListType objtype = new ListType();
                        ListInvoiceNo objinvoiceno = new ListInvoiceNo();
                        ListInvoiceAmt objinvoiceamt = new ListInvoiceAmt();
                        ListTxnDate objtxndt = new ListTxnDate();
                        List<ListEarn> lstearn = new List<ListEarn>();
                        List<ListBurn> lstburn = new List<ListBurn>();
                        List<ListType> lsttype = new List<ListType>();
                        List<ListInvoiceNo> lstinvoiceno = new List<ListInvoiceNo>();
                        List<ListInvoiceAmt> lstinvoiceamt = new List<ListInvoiceAmt>();
                        List<ListTxnDate> lsttxndate = new List<ListTxnDate>();
                        

                        // pointsearn
                        if (str.Contains("13"))
                        {
                            var pointsEarn = context.Database.SqlQuery<decimal?>("select PointsEarned from TransactionMaster where TransType='1'and CustomerId=" + id.CustomerId + "").ToList();
                            
                            if (pointsEarn.Count > 0)
                            {
                                foreach (var item in pointsEarn)
                                {
                                    // dicearnlist.Add(id.CustomerId, item);
                                    objearn.CustomerId = id.CustomerId;
                                    objearn.PointsEarn = item;
                                    lstearn.Add(objearn);
                                   
                                }
                            }
                            //objtranstypereport.PointsEarn = pointsEarn;

                        }
                        //pointsburn
                        if (str.Contains("14"))
                        {
                            var pointsBurn = context.Database.SqlQuery<decimal?>("select PointsBurned from TransactionMaster where TransType='2'and CustomerId=" + id.CustomerId + "").ToList();
                            if (pointsBurn.Count > 0)
                            {
                                foreach (var item in pointsBurn)
                                {
                                    //dicburnlist.Add(id.CustomerId, item);
                                    objburn.CustomerId = id.CustomerId;
                                    objburn.PointsBurn = item;
                                    lstburn.Add(objburn);
                                }
                            }
                        }
                        //type
                        if (str.Contains("15"))
                        {
                            var type = context.Database.SqlQuery<string>("select ( case when TransType='1'then'Earn'Else 'Burn' end) as Type from TransactionMaster where CustomerId=" + id.CustomerId + "").ToList();
                            if (type.Count > 0)
                            {
                                foreach (var item in type)
                                {
                                    //dictypelist.Add(id.CustomerId, item);
                                    objtype.CustomerId = id.CustomerId;
                                    objtype.Type = item;
                                    lsttype.Add(objtype);
                                }
                            }
                        }
                        //invoiceno
                        if (str.Contains("16"))
                        {
                            var invoiceno = context.Database.SqlQuery<string>("select InvoiceNo from TransactionMaster where CustomerId=" + id.CustomerId + "").ToList();
                            if (invoiceno.Count > 0)
                            {
                                foreach (var item in invoiceno)
                                {
                                    // dicinvoicenolist.Add(id.CustomerId, item);
                                    objinvoiceno.CustomerId = id.CustomerId;
                                    objinvoiceno.InvoiceNo = item;
                                    lstinvoiceno.Add(objinvoiceno);
                                }
                            }
                        }
                        //invoiceamt
                        if (str.Contains("17"))
                        {
                            
                            var invoiceamt = context.Database.SqlQuery<decimal?>("select InvoiceAmt from TransactionMaster where CustomerId =" + id.CustomerId + "").ToList();
                            if (invoiceamt.Count > 0)
                            {
                                foreach (var item in invoiceamt)
                                {
                                    // dicinvoiceamtlist.Add(id.CustomerId, item);
                                    objinvoiceamt.CustomerId = id.CustomerId;
                                    objinvoiceamt.InvoiceAmt = item;
                                    lstinvoiceamt.Add(objinvoiceamt);
                                }
                            }
                        }
                        //txndate
                        if (str.Contains("18"))
                        {
                            var txndate = context.Database.SqlQuery<DateTime?>("select Datetime from TransactionMaster where CustomerId =" + id.CustomerId + "").ToList();
                            if (txndate.Count > 0)
                            {
                                foreach (var item in txndate)
                                {
                                    //dictxndatelist.Add(id.CustomerId, item);
                                    objtxndt.CustomerId = id.CustomerId;
                                    objtxndt.TxnDate = item;
                                    lsttxndate.Add(objtxndt);
                                }
                            }
                        }
                        //outletnm
                        if (str.Contains("1"))
                        {
                            var outletname = context.Database.SqlQuery<string>("select OutletDetails.OutletName from CustomerDetails join OutletDetails on CustomerDetails.EnrollingOutlet = OutletDetails.OutletId where CustomerId=" + id.CustomerId + "").FirstOrDefault();

                            objtranstypereport.OutletName = outletname;

                        }
                        //mobileno
                        if (str.Contains("2"))
                        {
                            objtranstypereport.MobileNo = id.MobileNo;

                        }
                        //customername
                        if (str.Contains("3"))
                        {
                            objtranstypereport.CustomerName = id.CustomerName;
                        }
                        //firsttxndt
                        if (str.Contains("4"))
                        {

                            var itemfirstdt = context.Database.SqlQuery<DateTime>("select Min(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();
                            objtranstypereport.FirstTxnDate = itemfirstdt;
                        }
                        //lasttxndt
                        if (str.Contains("5"))
                        {

                            var Lasttxndate = context.Database.SqlQuery<DateTime>("select Max(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

                            objtranstypereport.LastTxnDate = Lasttxndate;
                        }                      
                        //enrolleddate
                        if (str.Contains("7"))
                        {
                            objtranstypereport.EnrolledDate = id.DOJ;

                        }                        
                        //bonuspoints
                        if (str.Contains("12"))
                        {
                            
                            var Bonus = context.Database.SqlQuery<decimal>("select sum(pointsearned) from transactionmaster where InvoiceNo in('B_Birthday', 'B_Anniversary','B_ProfileUpdate','B_ReferralBonus', 'B_GiftingPoints', 'Bonus')and TransType in(1, 2) and CustomerId=" + id.CustomerId + " group by customerid  ").FirstOrDefault();
                            objtranstypereport.BonusPoints = Bonus;
                        }
                        
                        int[] arr = new int[] { lstearn.Count, lsttxndate.Count, lsttype.Count, lstinvoiceno.Count, lstinvoiceamt.Count, lstburn.Count };
                        System.Array.Sort<int>(arr, new Comparison<int>(
                          (i1, i2) => i2.CompareTo(i1)));

                        int count = arr[0];
                        for (int a = 0; a < count; a++)
                        {
                            TransactionTypeReport objmultipletxn = new TransactionTypeReport();
                            var propInfo = objtranstypereport.GetType().GetProperties();
                            foreach (var item in propInfo)
                            {
                                objmultipletxn.GetType().GetProperty(item.Name).SetValue(objmultipletxn, item.GetValue(objtranstypereport, null), null);
                            }

                            //objmultipletxn.   
                            if (lstearn.Count >= a + 1)
                            {
                                objmultipletxn.PointsEarn = lstearn[a].PointsEarn;
                            }
                            if (lsttxndate.Count >= a + 1)
                            {
                                objmultipletxn.TxnDate = lsttxndate[a].TxnDate;
                            }
                            if (lsttype.Count >= a + 1)
                            {
                                objmultipletxn.Type = lsttype[a].Type;
                            }
                            if (lstinvoiceno.Count >= a + 1)
                            {
                                objmultipletxn.InvoiceNo = lstinvoiceno[a].InvoiceNo;
                            }
                            if (lstinvoiceamt.Count >= a + 1)
                            {
                                objmultipletxn.InvoiceAmt = lstinvoiceamt[a].InvoiceAmt;
                            }
                            if (lstburn.Count >= a + 1)
                            {
                                objmultipletxn.PointsBurn = lstburn[a].PointsBurn;
                            }
                            lstTxntype.Add(objmultipletxn);
                        }
                            
                        
                    }

                }
            }
            return lstTxntype;
        }
        public List<ReportFilterCount> GetFilterCountOfDrillDown(int Elementtype, int Elementfilter, long Element1, long Element2, string BillSizeFilter, long BillSize_min, long BillSize_max, int periodsfilter, string periodFrm, string periodTo, string outletIds, string GroupId, string connstr)
        {

            List<ReportFilterCount> filterCount = new List<ReportFilterCount>();

            using (var context = new BOTSDBContext(connstr))
            {

                try
                {
                    var FromdatefirstDayOfMonth = new DateTime(Convert.ToDateTime(periodFrm).Year, Convert.ToDateTime(periodFrm).Month, 1).ToShortDateString();
                    var ToDatelastDayOfMonth = new DateTime(Convert.ToDateTime(periodTo).Year, Convert.ToDateTime(periodTo).Month, 1).AddMonths(1).AddDays(-1).ToShortDateString();

                    filterCount = context.Database.SqlQuery<ReportFilterCount>("sp_BOTS_TxnCountSpendDrillDown @pi_GroupId,@pi_Date,@pi_LoginId,@pi_OutletId,@pi_ElementType,@pi_ElementFilter,@pi_Element1,@pi_Element2,@pi_AvgBillSizeFilter,@pi_AvgBill1,@pi_AvgBill2,@pi_PeriodFilter,@pi_PeriodFromDate,@pi_PeriodToDate",
                           new SqlParameter("@pi_GroupId", GroupId),
                           new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                           new SqlParameter("@pi_LoginId", ""),
                           new SqlParameter("@pi_OutletId", outletIds),
                           new SqlParameter("@pi_ElementType", Elementtype),
                           new SqlParameter("@pi_ElementFilter", Elementfilter),
                           new SqlParameter("@pi_Element1", Element1),
                           new SqlParameter("@pi_Element2", Element2),
                           new SqlParameter("@pi_AvgBillSizeFilter", BillSizeFilter),
                           new SqlParameter("@pi_AvgBill1", BillSize_min),
                           new SqlParameter("@pi_AvgBill2", BillSize_max),
                           new SqlParameter("@pi_PeriodFilter", periodsfilter),
                           new SqlParameter("@pi_PeriodFromDate", FromdatefirstDayOfMonth),
                           new SqlParameter("@pi_PeriodToDate", ToDatelastDayOfMonth)
                           ).ToList<ReportFilterCount>();
                    if (filterCount[0].FilterCount == null)
                    {
                        filterCount[0].FilterCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
            }
            return filterCount;
        }
        public DateTime GetStartDtOfProgram(string GroupId, string connstr)
        {
            DateTime dt = new DateTime();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    dt = (DateTime)context.TransactionMasters.Min(x => x.Datetime);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return dt;
        }
        public void email_send(string emailid, string subject, byte[] ms, string BCC)
        {


            StringBuilder str = new StringBuilder();
            str.Append("<table>");
            str.Append("<tr>");

            str.AppendLine("<td>Dear Customer,</td>");
            str.AppendLine("</br>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");

            str.AppendLine("<td>Please find the detailed report attached.</td>");
            str.AppendLine("</br>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.AppendLine("<td>If you have any questions on this report, please do not reply to this email, as this email report is being sent from is an unmonitored email alias. Instead, write to info@blueocktopus.in or call us for information / clarification.</td>");
            str.AppendLine("</br>");
            str.AppendLine("</br>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.AppendLine("<td>Regards,</td>");
            str.AppendLine("</br>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.AppendLine("<td>Blue Ocktopus team</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.AppendLine("<td>info@blueocktopus.in</td>");
            str.AppendLine("</br>");
            str.AppendLine("</br>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td>&nbsp;</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.AppendLine("<td>Disclaimer: The information/contents of this e-mail message and any attachments are confidential and are intended solely for the addressee. Any review, re-transmission, dissemination or other use of, or taking of any action in reliance upon, this information by persons or entities other than the intended recipient is prohibited. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Any unauthorized use, copying or dissemination of this transmission is prohibited. Neither the confidentiality nor the integrity of this message can be vouched for following transmission on the internet.</td>");
            str.Append("</tr>");
            str.Append("</table>");

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.zoho.com");
            mail.From = new MailAddress("report@blueocktopus.in");
            mail.IsBodyHtml = true;
            mail.To.Add(emailid);
            mail.Bcc.Add(BCC);
            mail.Subject = "BOTS_" + subject;
            mail.Body = str.ToString();
            System.IO.MemoryStream stream1 = new System.IO.MemoryStream(ms, true);

            stream1.Write(ms, 0, ms.Length);
            stream1.Position = 0;
            mail.Attachments.Add(new Attachment(stream1, "BOTS_" + subject + ".xlsx"));

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("report@blueocktopus.in", "Report@123");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
    }
}

