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
using BOTS_BL.Models.IndividualDBModels;

namespace BOTS_BL.Repository
{
    public class ReportsRepository
    {
        DashboardRepository DR = new DashboardRepository();
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        public List<MemberList> GetMemberList(string GroupId, string OutletId, string connstr, string loginId, string FromDate, string ToDate, string FrmPts,
                  string ToPts, string FrmSpend, string ToSpend)
        {
            List<MemberList> lstMember = new List<MemberList>();
            List<MemberListAllData> lstTempMemberData = new List<MemberListAllData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    if (GroupId == "1086")
                    {
                        lstMember = context.Database.SqlQuery<MemberList>("sp_BOTS_MemberList @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId",
                        new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_OutletId", OutletId)).ToList<MemberList>();
                    }
                    //else if (GroupId == "1087")
                    //{
                    if (OutletId == "")
                    {
                        lstTempMemberData = context.Database.SqlQuery<MemberListAllData>("select * from View_CustTxnSummaryWithPts").ToList();
                    }
                    else
                    {
                        lstTempMemberData = context.Database.SqlQuery<MemberListAllData>("select * from View_CustTxnSummaryWithPts where CurrentEnrolledOutlet = @pi_OutletId", new SqlParameter("@pi_OutletId", OutletId)).ToList();
                    }
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        var fDate = Convert.ToDateTime(FromDate);
                        var tDate = Convert.ToDateTime(ToDate);
                        lstTempMemberData = lstTempMemberData.Where(x => x.LasTTxnDate >= fDate && x.LasTTxnDate <= tDate).ToList();
                    }
                    else if (!string.IsNullOrEmpty(FromDate))
                    {
                        var fDate = Convert.ToDateTime(FromDate);
                        lstTempMemberData = lstTempMemberData.Where(x => x.LasTTxnDate >= fDate).ToList();
                    }
                    else if (!string.IsNullOrEmpty(ToDate))
                    {
                        var tDate = Convert.ToDateTime(ToDate);
                        lstTempMemberData = lstTempMemberData.Where(x => x.LasTTxnDate <= tDate).ToList();
                    }

                    if (!string.IsNullOrEmpty(FrmPts) && !string.IsNullOrEmpty(ToPts))
                    {
                        var fPts = Convert.ToInt64(FrmPts);
                        var tPts = Convert.ToInt64(ToPts);
                        lstTempMemberData = lstTempMemberData.Where(x => x.AvlPts >= fPts && x.AvlPts <= tPts).ToList();
                    }
                    else if (!string.IsNullOrEmpty(FrmPts))
                    {
                        var fPts = Convert.ToInt64(FrmPts);
                        lstTempMemberData = lstTempMemberData.Where(x => x.AvlPts >= fPts).ToList();
                    }
                    else if (!string.IsNullOrEmpty(ToPts))
                    {
                        var tPts = Convert.ToInt64(ToPts);
                        lstTempMemberData = lstTempMemberData.Where(x => x.AvlPts <= tPts).ToList();
                    }


                    if (!string.IsNullOrEmpty(FrmSpend) && !string.IsNullOrEmpty(ToSpend))
                    {
                        var fSpend = Convert.ToDecimal(FrmSpend);
                        var tSpend = Convert.ToDecimal(ToSpend);
                        lstTempMemberData = lstTempMemberData.Where(x => x.TotalSpend >= fSpend && x.TotalSpend <= tSpend).ToList();
                    }
                    else if (!string.IsNullOrEmpty(FrmSpend))
                    {
                        var fSpend = Convert.ToDecimal(FrmSpend);
                        lstTempMemberData = lstTempMemberData.Where(x => x.TotalSpend >= fSpend).ToList();
                    }
                    else if (!string.IsNullOrEmpty(ToSpend))
                    {
                        var tSpend = Convert.ToDecimal(ToSpend);
                        lstTempMemberData = lstTempMemberData.Where(x => x.TotalSpend <= tSpend).ToList();
                    }

                    foreach (var item in lstTempMemberData)
                    {
                        MemberList Obj = new MemberList();
                        Obj.EnrooledOutlet = item.OutletName;
                        Obj.EnrolledDate = item.DOJ.Value.ToString("MM/dd/yyyy");
                        Obj.MaskedMobileNo = item.MaskedMobileNo;
                        Obj.MobileNo = item.MobileNo;
                        Obj.MemberName = item.Name;
                        Obj.Type = item.Type;
                        Obj.TxnCount = item.TotalTxnCount;
                        Obj.TotalSpend = Convert.ToInt64(item.TotalSpend);
                        Obj.TotalBurnTxn = item.BurnCount;
                        Obj.TotalBurnPoints = Convert.ToInt64(item.BurnPts);
                        Obj.AvlBalPoints = item.AvlPts;
                        if (item.LasTTxnDate.HasValue)
                        {
                            Obj.LastTxnDate = item.LasTTxnDate.Value.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            Obj.LastTxnDate = item.DOJ.Value.ToString("MM/dd/yyyy");
                        }

                        lstMember.Add(Obj);
                    }
                    //}
                    //else
                    //{
                    //    lstMember = context.Database.SqlQuery<MemberList>("sp_BOTS_MemberList @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId",
                    //    new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", OutletId)).ToList<MemberList>();
                    //}
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberList");
            }
            return lstMember;
        }

        public List<SelectListItem> GetOutletList(string GroupId, string connstr)
        {
            string DBStatus = string.Empty;
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            try
            {
                using (var contextnew = new CommonDBContext())
                {
                    DBStatus = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.GroupId).FirstOrDefault();

                }
                using (var context = new BOTSDBContext(connstr))
                {
                    if (!string.IsNullOrEmpty(DBStatus))
                    {
                        var lstOutlet = context.tblOutletMasters.Where(x => !x.OutletName.ToLower().Contains("admin")).ToList();
                        foreach (var item in lstOutlet)
                        {
                            countriesItem.Add(new SelectListItem
                            {
                                Text = item.OutletName,
                                Value = Convert.ToString(item.OutletId)
                            });
                        }
                    }
                    else
                    {
                        var lstOutlet = context.OutletDetails.Where(x => !x.OutletName.ToLower().Contains("admin")).ToList();
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletList");
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
                newexception.AddException(ex, "GetOutletListForSliceAndDice");
            }
            return countriesItem;
        }
        public List<SelectListItem> GetBrandList(string GroupId, string connstr)
        {
            string DBStatus = string.Empty;
            List<SelectListItem> BrandItem = new List<SelectListItem>();
            try
            {
                using (var contextnew = new CommonDBContext())
                {
                    DBStatus = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.GroupId).FirstOrDefault();
                }
                using (var context = new BOTSDBContext(connstr))
                {

                    if (!string.IsNullOrEmpty(DBStatus))
                    {
                        var LstBrand = context.tblBrandMasters.Where(x => x.GroupId == GroupId).ToList();
                        foreach (var item in LstBrand)
                        {
                            BrandItem.Add(new SelectListItem
                            {
                                Text = item.BrandName,
                                Value = Convert.ToString(item.BrandId)
                            });
                        }
                    }
                    else
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBrandList");
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
                    //else if (GroupId == "1087")
                    //{
                    var AllData = DR.GetExecutiveSummaryAllData(GroupId, connstr);
                    var lstobj = context.Database.SqlQuery<View_TxnDetailsMaster>("select * from View_TxnDetailsMaster").ToList();
                    var lstOutlet = context.tblOutletMasters.Where(x => !x.OutletName.ToLower().Contains("admin") && x.IsActive == true).ToList();
                    foreach (var item in lstOutlet)
                    {
                        OutletWise newItem = new OutletWise();
                        newItem.OutletName = item.OutletName;

                        if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                        {
                            var fDate = Convert.ToDateTime(FromDate);
                            var tDate = Convert.ToDateTime(ToDate);

                            newItem.TotalMember = AllData.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Count();
                            newItem.TotalTxn = lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Count();
                            newItem.TotalSpend = Convert.ToInt64(lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.InvoiceAmt));
                            newItem.BizShare = (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.InvoiceAmt) * 100) / lstobj.Where(x => x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(x => x.InvoiceAmt);
                            newItem.ATS = Convert.ToInt64((lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.InvoiceAmt)) / lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Count());
                            var last90DaysDate = DateTime.Today.AddDays(-90);
                            var last30DaysDate = DateTime.Today.AddDays(-30);
                            newItem.NonActive = lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Count();
                            var lstMobileNo = AllData.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TotalTxnCount == 1).Select(y => y.MobileNo).ToList();
                            newItem.OnlyOnce = lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate && lstMobileNo.Contains(x.MobileNo)).Count();
                            if (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsEarned) > 0)
                                newItem.RedemptionRate = (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsBurned) * 100) / lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsEarned);
                            else
                                newItem.RedemptionRate = 0;
                            //newItem.RedemptionRate = (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsBurned) * 100) / lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsEarned);
                            newItem.PointsEarned = Convert.ToInt64(lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsEarned));
                            newItem.PointsBurned = Convert.ToInt64(lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsBurned));
                            newItem.PointsCancelled = Convert.ToInt64(context.tblSalesReturnMasters.Where(x => x.OutletId == item.OutletId && x.TxnDatetime >= fDate && x.TxnDatetime <= tDate).Sum(y => y.PointsRemoved));
                            newItem.PointsExpired = Convert.ToInt64(context.tblExpiredPointsMasters.Where(x => x.OutletId == item.OutletId && x.ExpiredDate >= fDate && x.ExpiredDate <= tDate).Sum(y => y.ExpiredPoints));
                        }
                        else
                        {

                            newItem.TotalMember = AllData.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Count();
                            newItem.TotalTxn = lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Count();
                            newItem.TotalSpend = Convert.ToInt64(lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.InvoiceAmt));
                            if (lstobj.Sum(x => x.InvoiceAmt) > 0)
                                newItem.BizShare = (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.InvoiceAmt) * 100) / lstobj.Sum(x => x.InvoiceAmt);
                            else
                                newItem.BizShare = 0;

                            if (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Count() > 0)
                                newItem.ATS = Convert.ToInt64((lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.InvoiceAmt)) / lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Count());
                            else
                                newItem.ATS = 0;
                            var last90DaysDate = DateTime.Today.AddDays(-90);
                            var last30DaysDate = DateTime.Today.AddDays(-30);
                            newItem.NonActive = lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime < last90DaysDate).Count();
                            var lstMobileNo = AllData.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.LastTxnDate < last30DaysDate && x.TotalTxnCount == 1).Select(y => y.MobileNo).ToList();
                            newItem.OnlyOnce = lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId && x.TxnDatetime < last30DaysDate && lstMobileNo.Contains(x.MobileNo)).Count();
                            if (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.PointsEarned) > 0)
                                newItem.RedemptionRate = (lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.PointsBurned) * 100) / lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.PointsEarned);
                            else
                                newItem.RedemptionRate = 0;
                            newItem.PointsEarned = Convert.ToInt64(lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.PointsEarned));
                            newItem.PointsBurned = Convert.ToInt64(lstobj.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Sum(y => y.PointsBurned));
                            newItem.PointsCancelled = Convert.ToInt64(context.tblSalesReturnMasters.Where(x => x.OutletId == item.OutletId).Sum(y => y.PointsRemoved));
                            newItem.PointsExpired = Convert.ToInt64(context.tblExpiredPointsMasters.Where(x => x.OutletId == item.OutletId).Sum(y => y.ExpiredPoints));
                        }

                        lstOutletWise.Add(newItem);
                    }

                    //}
                    //else
                    //{
                    //    lstOutletWise = context.Database.SqlQuery<OutletWise>("sp_BOTS_OutletwiseSummary @pi_GroupId, @pi_Date, @pi_LoginId, @pi_DateRangeFlag, @pi_FromDate, @pi_ToDate",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    //    new SqlParameter("@pi_LoginId", ""),
                    //    new SqlParameter("@pi_DateRangeFlag", DateRangeFlag),
                    //    new SqlParameter("@pi_FromDate", FromDate),
                    //    new SqlParameter("@pi_ToDate", ToDate)).ToList<OutletWise>();
                    //}
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletWiseList");
            }
            lstOutletWise = lstOutletWise.Where(x => !x.OutletName.ToLower().Contains("admin")).ToList();
            return lstOutletWise;
        }
        public List<OutletwiseTransaction> GetOutletWiseTransactionList(string GroupId, string DateRangeFlag, string FromDate, string ToDate, string OutletId, string EnrolmentDataFlag, string connstr, string loginId)
        {
            List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();

            DateTime Day30 = DateTime.Now;
            Day30 = Day30.AddDays(-30);
            DateTime CurrentDate = DateTime.Now;

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
                    //else if (GroupId == "1087")
                    //{
                    if (DateRangeFlag == "0")
                    {
                        if (string.IsNullOrEmpty(OutletId))
                        {
                            if (EnrolmentDataFlag == "4")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) * FROM View_TxnDetailsMaster").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "2")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) * FROM View_TxnDetailsMaster where TxnType = 'Earn'").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }

                            }
                            else if (EnrolmentDataFlag == "3")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) * FROM View_TxnDetailsMaster where TxnType = 'Burn'").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "5")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) OutletName,MobileNo,MaskedMobileNo,Name,Category,TxnType,InvoiceNo,InvoiceAmt,PointsGiven as PointsEarned,PointsRemoved as PointsBurned,TxnDatetime FROM View_SalesReturnTxnDetailsMaster").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");

                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) Min(T.OutletName) as OutletName ,T.MobileNo,Min(T.MaskedMobileno) as MaskedMobileno,Min(T.Name) as Name,Min(T.Category) as Category,Min(T.DOJ) as DOJ FROM View_TxnDetailsMaster T Group by T.MobileNo").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = "Enrolment";
                                    //Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = 0;
                                    //Obj.InvoiceAmtStr = "0";
                                    Obj.PointsEarned = 0;
                                    Obj.PointsBurned = 0;
                                    Obj.TxnDatetime = item.DOJ.Value.ToString("MM/dd/yyyy");
                                    //Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                        }
                        else
                        {
                            if (EnrolmentDataFlag == "4")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) * FROM View_TxnDetailsMaster where CurrentEnrolledOutlet = " + @OutletId + "", new SqlParameter("@pi_OutletId", OutletId)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "2")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) * FROM View_TxnDetailsMaster where TxnType = 'Earn' and CurrentEnrolledOutlet = " + @OutletId + "", new SqlParameter("@pi_OutletId", OutletId)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }

                            }
                            else if (EnrolmentDataFlag == "3")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) * FROM View_TxnDetailsMaster where TxnType = 'Burn' and CurrentEnrolledOutlet = " + @OutletId + "", new SqlParameter("@pi_OutletId", OutletId)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "5")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) OutletName,MobileNo,MaskedMobileNo,Name,Category,TxnType,InvoiceNo,InvoiceAmt,PointsGiven as PointsEarned,PointsRemoved as PointsBurned,TxnDatetime FROM View_SalesReturnTxnDetailsMaster").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");

                                    lstOutletWiseTransaction.Add(Obj);
                                }

                            }
                            else
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT top(26) Min(T.OutletName) as OutletName ,T.MobileNo,Min(T.MaskedMobileno) as MaskedMobileno,Min(T.Name) as Name,Min(T.Category) as Category,Min(T.DOJ) as DOJ FROM View_TxnDetailsMaster T Group by T.MobileNo").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = "Enrolment";
                                    //Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = 0;
                                    //Obj.InvoiceAmtStr = "0";
                                    Obj.PointsEarned = 0;
                                    Obj.PointsBurned = 0;
                                    Obj.TxnDatetime = item.DOJ.Value.ToString("MM/dd/yyyy");
                                    //Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }

                        }
                    }
                    else if (DateRangeFlag == "1")
                    {
                        if (string.IsNullOrEmpty(OutletId))
                        {
                            if (EnrolmentDataFlag == "4")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }

                            }
                            else if (EnrolmentDataFlag == "2")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Earn' and cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }

                            }
                            else if (EnrolmentDataFlag == "3")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Burn' and cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "5")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT OutletName,MobileNo,MaskedMobileNo,Name,Category,TxnType,InvoiceNo,InvoiceAmt,PointsGiven as PointsEarned,PointsRemoved as PointsBurned,TxnDatetime FROM View_SalesReturnTxnDetailsMaster where cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");

                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT Min(T.OutletName) as OutletName ,T.MobileNo,Min(T.MaskedMobileno) as MaskedMobileno,Min(T.Name) as Name,Min(T.Category) as Category,Min(T.DOJ) as DOJ FROM View_TxnDetailsMaster T where T.DOJ between '" + Day30 + "' and '" + CurrentDate + "' and T.CurrentEnrolledOutlet = '" + @OutletId + "' and Group by T.MobileNo").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = "Enrolment";
                                    //Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = 0;
                                    //Obj.InvoiceAmtStr = "0";
                                    Obj.PointsEarned = 0;
                                    Obj.PointsBurned = 0;
                                    Obj.TxnDatetime = item.DOJ.Value.ToString("MM/dd/yyyy");
                                    //Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                        }
                        else
                        {
                            if (EnrolmentDataFlag == "4")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where CurrentEnrolledOutlet = " + @OutletId + " and  cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@OutletId", @OutletId), new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }

                            }
                            else if (EnrolmentDataFlag == "2")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Earn' and CurrentEnrolledOutlet = " + @OutletId + " and  cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "3")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Burn' and CurrentEnrolledOutlet = " + @OutletId + " and  cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "5")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT OutletName,MobileNo,MaskedMobileNo,Name,Category,TxnType,InvoiceNo,InvoiceAmt,PointsGiven as PointsEarned,PointsRemoved as PointsBurned,TxnDatetime FROM View_SalesReturnTxnDetailsMaster where CurrentEnrolledOutlet = " + @OutletId + " and  cast(TxnDatetime as Date) between '" + Day30 + "' and '" + CurrentDate + "'", new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_Day30", Day30), new SqlParameter("@pi_CurrentDate", CurrentDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");

                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT Min(T.OutletName) as OutletName ,T.MobileNo,Min(T.MaskedMobileno) as MaskedMobileno,Min(T.Name) as Name,Min(T.Category) as Category,Min(T.DOJ) as DOJ FROM View_TxnDetailsMaster T where T.CurrentEnrolledOutlet = '" + @OutletId + "' and  T.DOJ between '" + Day30 + "' and '" + CurrentDate + "' Group by T.MobileNo").ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = "Enrolment";
                                    //Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = 0;
                                    //Obj.InvoiceAmtStr = "0";
                                    Obj.PointsEarned = 0;
                                    Obj.PointsBurned = 0;
                                    Obj.TxnDatetime = item.DOJ.Value.ToString("MM/dd/yyyy");
                                    //Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                        }
                    }
                    else if (DateRangeFlag == "2")
                    {
                        if (string.IsNullOrEmpty(OutletId))
                        {
                            if (EnrolmentDataFlag == "4")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "2")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Earn' and cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "3")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Burn' and cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "5")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT OutletName,MobileNo,MaskedMobileNo,Name,Category,TxnType,InvoiceNo,InvoiceAmt,PointsGiven as PointsEarned,PointsRemoved as PointsBurned,TxnDatetime FROM View_SalesReturnTxnDetailsMaster where cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");

                                    lstOutletWiseTransaction.Add(Obj);
                                }

                            }
                            else
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT Min(T.OutletName) as OutletName ,T.MobileNo,Min(T.MaskedMobileno) as MaskedMobileno,Min(T.Name) as Name,Min(T.Category) as Category,Min(T.DOJ) as DOJ FROM View_TxnDetailsMaster T where T.DOJ between '" + FromDate + "' and '" + ToDate + "' and Group by T.MobileNo", new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = "Enrolment";
                                    //Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = 0;
                                    //Obj.InvoiceAmtStr = "0";
                                    Obj.PointsEarned = 0;
                                    Obj.PointsBurned = 0;
                                    Obj.TxnDatetime = item.DOJ.Value.ToString("MM/dd/yyyy");
                                    //Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                        }
                        else
                        {
                            if (EnrolmentDataFlag == "4")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where CurrentEnrolledOutlet = " + @OutletId + " and cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "2")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Earn' and CurrentEnrolledOutlet = " + @OutletId + " and  cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "3")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT * FROM View_TxnDetailsMaster where TxnType = 'Burn' and CurrentEnrolledOutlet = " + @OutletId + " and  cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");
                                    Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else if (EnrolmentDataFlag == "5")
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT OutletName,MobileNo,MaskedMobileNo,Name,Category,TxnType,InvoiceNo,InvoiceAmt,PointsGiven as PointsEarned,PointsRemoved as PointsBurned,TxnDatetime FROM View_SalesReturnTxnDetailsMaster where cast(TxnDatetime as Date) between '" + FromDate + "' and '" + ToDate + "'", new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = item.TxnType;
                                    Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = item.InvoiceAmt;
                                    Obj.PointsEarned = item.PointsEarned;
                                    Obj.PointsBurned = item.PointsBurned;
                                    Obj.TxnDatetime = item.TxnDatetime.Value.ToString("MM/dd/yyyy");

                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }
                            else
                            {
                                var LstTxnData = context.Database.SqlQuery<TransactionSummaryData>("SELECT Min(T.OutletName) as OutletName ,T.MobileNo,Min(T.MaskedMobileno) as MaskedMobileno,Min(T.Name) as Name,Min(T.Category) as Category,Min(T.DOJ) as DOJ FROM View_TxnDetailsMaster T where T.CurrentEnrolledOutlet = '" + @OutletId + "' and  T.DOJ between '" + FromDate + "' and '" + ToDate + "' Group by T.MobileNo", new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_FromDate", FromDate), new SqlParameter("@pi_ToDate", ToDate)).ToList();
                                foreach (var item in LstTxnData)
                                {
                                    OutletwiseTransaction Obj = new OutletwiseTransaction();
                                    Obj.OutletName = item.OutletName;
                                    Obj.MobileNo = item.MobileNo;
                                    Obj.MaskedMobileNo = item.MaskedMobileNo;
                                    Obj.MemberName = item.Name;
                                    Obj.Type = item.Category;
                                    Obj.TxnType = "Enrolment";
                                    //Obj.InvoiceNo = item.InvoiceNo;
                                    Obj.InvoiceAmt = 0;
                                    //Obj.InvoiceAmtStr = "0";
                                    Obj.PointsEarned = 0;
                                    Obj.PointsBurned = 0;
                                    Obj.TxnDatetime = item.DOJ.Value.ToString("MM/dd/yyyy");
                                    //Obj.TxnUpdateDate = item.TxnReceivedDatetime.Value.ToString("MM/dd/yyyy");
                                    lstOutletWiseTransaction.Add(Obj);
                                }
                            }

                        }

                    }
                    //}
                    //else
                    //{
                    //    lstOutletWiseTransaction = context.Database.SqlQuery<OutletwiseTransaction>("sp_BOTS_DetailedTransaction @pi_GroupId, @pi_Date, @pi_LoginId, @pi_DateRangeFlag, @pi_FromDate, @pi_ToDate, @pi_OutletId, @pi_EnrolmentDataFlag",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                    //    new SqlParameter("@pi_LoginId", ""),
                    //    new SqlParameter("@pi_DateRangeFlag", DateRangeFlag),
                    //    new SqlParameter("@pi_FromDate", FromDate),
                    //    new SqlParameter("@pi_ToDate", ToDate),
                    //    new SqlParameter("@pi_OutletId", OutletId),
                    //    new SqlParameter("@pi_EnrolmentDataFlag", EnrolmentDataFlag)).ToList<OutletwiseTransaction>();
                    //}

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletWiseTransactionList");
            }
            return lstOutletWiseTransaction;
        }

        public PointExpiryTmp GetPointExpiryData(string GroupId, int month, int year, string connstr, string loginId)
        {
            PointExpiryTmp pointExpiry = new PointExpiryTmp();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    //if (GroupId == "1086")
                    //{
                    //    pointExpiry = context.Database.SqlQuery<PointExpiryTmp>("sp_BOTS_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    //    new SqlParameter("@pi_LoginId", loginId),
                    //    new SqlParameter("@pi_Month", month),
                    //    new SqlParameter("@pi_Year", year)).FirstOrDefault<PointExpiryTmp>();
                    //}
                    //if (GroupId == "1087")
                    //{
                    DateTime Today = DateTime.Now;

                    if (month == Today.Month)
                    {
                        DateTime NextMonth = Today.AddMonths(1);
                        DateTime ThirdMonth = Today.AddMonths(2);
                        var lstPtsExp = context.Database.SqlQuery<PointsExpMasterAllData>("select * from tblCustPointsMaster where PointsType = 'Base' and IsActive = '1'").ToList();

                        pointExpiry.MemberCountThisMonth = lstPtsExp.Where(x => x.EndDate.Value.Month == Today.Month && x.EndDate.Value.Year == Today.Year).Count();
                        pointExpiry.MemberPointsThisMonth = Convert.ToInt64(lstPtsExp.Where(x => x.EndDate.Value.Month == Today.Month && x.EndDate.Value.Year == Today.Year).Sum(y => y.Points));
                        pointExpiry.MemberCountNextMonth = lstPtsExp.Where(x => x.EndDate.Value.Month == NextMonth.Month && x.EndDate.Value.Year == NextMonth.Year).Count();
                        pointExpiry.MemberPointsNextMonth = Convert.ToInt64(lstPtsExp.Where(x => x.EndDate.Value.Month == NextMonth.Month && x.EndDate.Value.Year == NextMonth.Year).Sum(y => y.Points));
                        pointExpiry.MemberCount3rdMonth = lstPtsExp.Where(x => x.EndDate.Value.Month == ThirdMonth.Month && x.EndDate.Value.Year == ThirdMonth.Year).Count();
                        pointExpiry.MemberPoints3rdMonth = Convert.ToInt64(lstPtsExp.Where(x => x.EndDate.Value.Month == ThirdMonth.Month && x.EndDate.Value.Year == ThirdMonth.Year).Sum(y => y.Points));
                    }
                    else
                    {
                        DateTime NextMonth = Today.AddMonths(1);
                        DateTime ThirdMonth = Today.AddMonths(2);
                        var lstPtsExp = context.Database.SqlQuery<PointsExpMasterAllData>("select * from tblCustPointsMaster where PointsType = 'Base' and IsActive = '1'").ToList();

                        pointExpiry.MemberCountThisMonth = lstPtsExp.Where(x => x.EndDate.Value.Month == Today.Month && x.EndDate.Value.Year == Today.Year).Count();
                        pointExpiry.MemberPointsThisMonth = Convert.ToInt64(lstPtsExp.Where(x => x.EndDate.Value.Month == Today.Month && x.EndDate.Value.Year == Today.Year).Sum(y => y.Points));
                        pointExpiry.MemberCountNextMonth = lstPtsExp.Where(x => x.EndDate.Value.Month == NextMonth.Month && x.EndDate.Value.Year == NextMonth.Year).Count();
                        pointExpiry.MemberPointsNextMonth = Convert.ToInt64(lstPtsExp.Where(x => x.EndDate.Value.Month == NextMonth.Month && x.EndDate.Value.Year == NextMonth.Year).Sum(y => y.Points));
                        pointExpiry.MemberCount3rdMonth = lstPtsExp.Where(x => x.EndDate.Value.Month == ThirdMonth.Month && x.EndDate.Value.Year == ThirdMonth.Year).Count();
                        pointExpiry.MemberPoints3rdMonth = Convert.ToInt64(lstPtsExp.Where(x => x.EndDate.Value.Month == ThirdMonth.Month && x.EndDate.Value.Year == ThirdMonth.Year).Sum(y => y.Points));
                        pointExpiry.SelectedCount = lstPtsExp.Where(x => x.EndDate.Value.Month == month && x.EndDate.Value.Year == year).Count();
                        pointExpiry.SelectedPoints = Convert.ToInt64(lstPtsExp.Where(x => x.EndDate.Value.Month == month && x.EndDate.Value.Year == year).Sum(y => y.Points));
                    }

                    //}
                    //else
                    //{
                    //    pointExpiry = context.Database.SqlQuery<PointExpiryTmp>("sp_BOTS_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    //    new SqlParameter("@pi_LoginId", ""),
                    //    new SqlParameter("@pi_Month", month),
                    //    new SqlParameter("@pi_Year", year)).FirstOrDefault<PointExpiryTmp>();
                    //}

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointExpiryData");
            }
            return pointExpiry;
        }

        public List<PointExpiryTxn> GetPointExpiryTxnData(string GroupId, int month, int year, string connstr, string loginId)
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
                    //else if (GroupId == "1087")
                    //{
                    DateTime Today = DateTime.Now;
                    int NextMonth = Today.AddMonths(1).Month;
                    int ThirdMonth = Today.AddMonths(2).Month;
                    var AllData = context.Database.SqlQuery<PointsExpCustDetailList>("select C.OutletName,C.MaskedMobileNo,C.Name,isnull(C.TotalTxnCount,0) as TotalTxnCount,isnull(C.TotalSpend,0) as TotalSpend,C.DOJ,P.MobileNo,isnull(C.AvlPts,0) as AvlPts,C.LastTxnDate,isnull(P.Points,0) as Points,P.EndDate from View_CustDetailsWithTxnSummary C inner join tblCustPointsMaster P on C.MobileNo = P.MobileNo where P.PointsType = 'Base'").ToList();
                    var lstPtsExp = context.Database.SqlQuery<PointsExpMasterAllData>("select * from tblCustPointsMaster where PointsType = 'Base'").ToList();
                    var lstMobileno = lstPtsExp.Where(x => x.EndDate.Value.Month == month && x.EndDate.Value.Year == year).Select(y => y.Mobileno).ToList();
                    var lstdata = AllData.Where(x => lstMobileno.Contains(x.MobileNo)).ToList();

                    foreach (var item in lstdata)
                    {
                        PointExpiryTxn lstPoint = new PointExpiryTxn();


                        lstPoint.EnrolledOutlet = item.OutletName;
                        lstPoint.MaskedMobileNo = item.MaskedMobileNo;
                        lstPoint.MemberName = item.Name;
                        lstPoint.TxnCount = Convert.ToInt64(item.TotalTxnCount);
                        lstPoint.TotalSpend = Convert.ToInt64(item.TotalSpend);
                        lstPoint.AvlPoints = Convert.ToInt64(item.AvlPts);
                        if (!string.IsNullOrEmpty(item.LastTxnDate.ToString()))
                        {
                            lstPoint.LastTxnDate = item.LastTxnDate.Value.ToString("MM/dd/yyyy");

                        }
                        else
                        {
                            lstPoint.LastTxnDate = item.DOJ.Value.ToString("MM/dd/yyyy");
                        }

                        lstPoint.PointsExpiry = Convert.ToInt64(item.Points);
                        lstPoint.ExpiryDate = item.EndDate.Value.ToString("MM/dd/yyyy");

                        pointExpiryTxn.Add(lstPoint);
                    }

                    //}
                    //else
                    //{
                    //    pointExpiryTxn = context.Database.SqlQuery<PointExpiryTxn>("sp_BOTS_PointsExpiry1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    //    new SqlParameter("@pi_LoginId", ""),
                    //    new SqlParameter("@pi_Month", month),
                    //    new SqlParameter("@pi_Year", year)).ToList<PointExpiryTxn>();
                    //}

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointExpiryTxnData");
            }
            return pointExpiryTxn;
        }

        public MemberSearch GetMeamberSearchData(string GroupId, string searchData, string connstr, string loginId)
        {
            string DBStatus = string.Empty;
            MemberSearch memberSearch = new MemberSearch();
            try
            {
                using (var contextnew = new CommonDBContext())
                {
                    DBStatus = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(DBStatus))
                {
                    using (var context = new CommonDBContext())
                    {
                        memberSearch = context.Database.SqlQuery<MemberSearch>("sp_MemberSearch @pi_GroupId, @pi_Date, @pi_LoginId, @pi_SearchData,@pi_DBName",
                            new SqlParameter("@pi_GroupId", GroupId),
                            new SqlParameter("@pi_Date", DateTime.Now),
                            new SqlParameter("@pi_LoginId", loginId),
                            new SqlParameter("@pi_SearchData", searchData),
                            new SqlParameter("@pi_DBName", DBStatus)).FirstOrDefault<MemberSearch>();

                        memberSearch.lstMemberSearchTxn = context.Database.SqlQuery<MemberSearchTxn>("sp_BOTS_MemberSearch1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_SearchData,@pi_DBName",
                            new SqlParameter("@pi_GroupId", GroupId),
                            new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                            new SqlParameter("@pi_LoginId", loginId),
                            new SqlParameter("@pi_SearchData", searchData),
                            new SqlParameter("@pi_DBName", DBStatus)).ToList<MemberSearchTxn>();

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
                else
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMeamberSearchData");
            }
            return memberSearch;
        }
        public List<MemberSearchTxn> GetDLCMeamberTransactionHistory(string GroupId, string MobileNo)
        {
            string DBName = String.Empty;
            List<MemberSearchTxn> lstMemberSearchTxn = new List<MemberSearchTxn>();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var context = new CommonDBContext())
                {
                    DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                    lstMemberSearchTxn = context.Database.SqlQuery<MemberSearchTxn>("sp_BOTS_MemberSearch1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_SearchData,@pi_DBName",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_SearchData", MobileNo),
                        new SqlParameter("@pi_DBName", DBName)).ToList<MemberSearchTxn>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCMeamberTransactionHistory");
            }
            lstMemberSearchTxn = lstMemberSearchTxn.OrderByDescending(x => x.TxnDatetime).ToList();
            return lstMemberSearchTxn;
        }
        public Celebrations GetCelebrationsData(string GroupId, string connstr)
        {
            Celebrations celebrationsData = new Celebrations();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    //if (GroupId == "1087")
                    //{
                    DateTime CurrentMonth = DateTime.Now;
                    var NextMonth = (CurrentMonth.AddMonths(1));
                    var ThirdMonth = (CurrentMonth.AddMonths(2));
                    var celebrationsData1 = context.Database.SqlQuery<CelebrationMemberData>("select * from View_CustDetailsWithTxnSummary").ToList();
                    celebrationsData1 = celebrationsData1.Where(x => x.DOB.HasValue).ToList();
                    //celebrationsData1 = celebrationsData1.Where(x => x.AnniversaryDate.HasValue).ToList();
                    celebrationsData.BirthdayCountThisMonth = celebrationsData1.Where(x => x.DOB.Value.Month == CurrentMonth.Month).Count();
                    celebrationsData.BirthdayCountNextMonth = celebrationsData1.Where(x => x.DOB.Value.Month == NextMonth.Month).Count();
                    celebrationsData.BirthdayCount3rdMonth = celebrationsData1.Where(x => x.DOB.Value.Month == ThirdMonth.Month).Count();

                    var CelebrationData1 = context.Database.SqlQuery<CelebrationMemberData>("select * from View_CustDetailsWithTxnSummary").ToList();
                    celebrationsData1 = celebrationsData1.Where(x => x.AnniversaryDate.HasValue).ToList();
                    celebrationsData.AnniversaryCountThisMonth = celebrationsData1.Where(x => x.AnniversaryDate.Value.Month == CurrentMonth.Month).Count();
                    celebrationsData.AnniversaryCountNextMonth = celebrationsData1.Where(x => x.AnniversaryDate.Value.Month == NextMonth.Month).Count();
                    celebrationsData.AnniversaryCount3rdMonth = celebrationsData1.Where(x => x.AnniversaryDate.Value.Month == ThirdMonth.Month).Count();

                    var celebrationsData2 = context.Database.SqlQuery<CelebrationMemberData>("select * from View_CustDetailsWithTxnSummary").ToList();
                    celebrationsData.EnrollmentAnniversaryCountThisMonth = celebrationsData2.Where(x => x.DOJ.Value.Month == CurrentMonth.Month && x.DOJ.Value.Year != CurrentMonth.Year).Count();
                    celebrationsData.EnrollmentAnniversaryCountNextMonth = celebrationsData2.Where(x => x.DOJ.Value.Month == NextMonth.Month && x.DOJ.Value.Year != NextMonth.Year).Count();
                    celebrationsData.EnrollmentAnniversary3rdMonth = celebrationsData2.Where(x => x.DOJ.Value.Month == ThirdMonth.Month && x.DOJ.Value.Year != ThirdMonth.Year).Count();

                    //}
                    //else
                    //{
                    //    celebrationsData = context.Database.SqlQuery<Celebrations>("sp_BOTS_Celebrate @pi_GroupId, @pi_Date, @pi_LoginId",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),

                    //    new SqlParameter("@pi_LoginId", "")).FirstOrDefault<Celebrations>();
                    //}

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCelebrationsData");
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
                    //if (GroupId == "1087")
                    //{
                    DateTime Today = DateTime.Now;
                    int Parmonth;
                    Parmonth = 0;
                    if (month == 1)
                    {
                        Parmonth = Today.Month;
                    }
                    else if (month == 2)
                    {
                        Parmonth = Today.AddMonths(1).Month;
                    }
                    else
                    {
                        Parmonth = Today.AddMonths(2).Month;
                    }

                    var celebrationTxnsData1 = context.Database.SqlQuery<CelebrationMemberData>("select * from View_CustDetailsWithTxnSummary").ToList();
                    if (type == 1)
                    {
                        celebrationTxnsData1 = celebrationTxnsData1.Where(x => x.DOB.HasValue).ToList();
                        celebrationTxnsData1 = celebrationTxnsData1.Where(x => x.DOB.Value.Month == Parmonth).ToList();
                        foreach (var item in celebrationTxnsData1)
                        {
                            CelebrationsMoreDetails Obj = new CelebrationsMoreDetails();
                            Obj.EnrolledOutlet = item.OutletName;
                            Obj.MaskedMobileNo = item.MobileNo;
                            Obj.MemberName = item.Name;
                            Obj.TxnCount = item.TotalTxnCount;
                            Obj.TotalSpend = Convert.ToInt64(item.TotalSpend);
                            Obj.AvlPoints = item.AvlPts;
                            var Temp = Convert.ToString(item.LastTxnDate);


                            if (Temp != "")
                            {
                                Obj.LastTxnDate = item.LastTxnDate.Value.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                Obj.LastTxnDate = "01/01/1900";
                            }

                            Obj.CelebrationDate = item.DOB.Value.ToString("MM/dd/yyyy");
                            celebrationTxnsData.Add(Obj);
                        }
                    }
                    if (type == 2)
                    {
                        celebrationTxnsData1 = celebrationTxnsData1.Where(x => x.AnniversaryDate.HasValue).ToList();
                        celebrationTxnsData1 = celebrationTxnsData1.Where(x => x.AnniversaryDate.Value.Month == Parmonth).ToList();
                        foreach (var item in celebrationTxnsData1)
                        {
                            CelebrationsMoreDetails Obj = new CelebrationsMoreDetails();
                            Obj.EnrolledOutlet = item.OutletName;
                            Obj.MaskedMobileNo = item.MobileNo;
                            Obj.MemberName = item.Name;
                            Obj.TxnCount = item.TotalTxnCount;
                            Obj.TotalSpend = Convert.ToInt64(item.TotalSpend);
                            Obj.AvlPoints = item.AvlPts;
                            Obj.LastTxnDate = Convert.ToString(item.LastTxnDate);
                            Obj.CelebrationDate = item.AnniversaryDate.Value.ToString("MM/dd/yyyy");
                            celebrationTxnsData.Add(Obj);
                        }
                    }
                    if (type == 3)
                    {
                        celebrationTxnsData1 = celebrationTxnsData1.Where(x => x.DOJ.HasValue).ToList();
                        celebrationTxnsData1 = celebrationTxnsData1.Where(x => x.DOJ.Value.Month == Parmonth && x.DOJ.Value.Year != Today.Year).ToList();
                        foreach (var item in celebrationTxnsData1)
                        {
                            CelebrationsMoreDetails Obj = new CelebrationsMoreDetails();
                            Obj.EnrolledOutlet = item.OutletName;
                            Obj.MaskedMobileNo = item.MobileNo;
                            Obj.MemberName = item.Name;
                            Obj.TxnCount = item.TotalTxnCount;
                            Obj.TotalSpend = Convert.ToInt64(item.TotalSpend);
                            Obj.AvlPoints = item.AvlPts;
                            Obj.LastTxnDate = Convert.ToString(item.LastTxnDate);
                            Obj.CelebrationDate = item.DOJ.Value.ToString("MM/dd/yyyy");
                            celebrationTxnsData.Add(Obj);
                        }
                    }
                    //}
                    //else
                    //{
                    //    celebrationTxnsData = context.Database.SqlQuery<CelebrationsMoreDetails>("sp_BOTS_Celebrate1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month,@pi_Type",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //      new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                    //      new SqlParameter("@pi_LoginId", ""),
                    //    new SqlParameter("@pi_Month", month),
                    //    new SqlParameter("@pi_Type", type)).ToList<CelebrationsMoreDetails>();
                    //}

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCelebrationsTxnData");
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
                newexception.AddException(ex, "GetOutletListByBrandId");
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
                newexception.AddException(ex, "GetTotalMemberCount");
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
                newexception.AddException(ex, "GetEnrolledList");
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
                newexception.AddException(ex, "GetNonTransactedList");
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
                newexception.AddException(ex, "GetSpendList");
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
                newexception.AddException(ex, "GetRedeemedList");
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
                newexception.AddException(ex, "GetGenderList");
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
                newexception.AddException(ex, "GetSourseList");
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
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSliceAndDiceFilteredData");
            }
            return objcount;

        }

        //public List<CustomerTypeReport> GenerateCustomerTypeReport(object[] ColumnId, List<CustomerDetail> lstcustdetails, string GroupId, string connstr)
        //{
        //    List<CustomerTypeReport> lstcusttype = new List<CustomerTypeReport>();

        //    List<TransactionMaster> objtrans = new List<TransactionMaster>();
        //    var str = String.Join(",", ColumnId);
        //    var query = from val in str.Split(',')
        //                select int.Parse(val);
        //    try
        //    {
        //        using (var context = new BOTSDBContext(connstr))
        //        {
        //            // var predicate = PredicateBuilder.True<TransactionMaster>();
        //            objtrans = context.Database.SqlQuery<TransactionMaster>("select * from TransactionMaster where InvoiceNo != 'B_Birthday' and InvoiceNo!= 'B_Anniversary'and InvoiceNo!= 'B_ProfileUpdate'and InvoiceNo!= 'B_ReferralBonus' and InvoiceNo!= 'B_GiftingPoints'and InvoiceNo!= 'Bonus'and TransType in(1, 2)").ToList();

        //            if (lstcustdetails != null)
        //            {

        //                foreach (var id in lstcustdetails)
        //                {
        //                    CustomerTypeReport objcustomertypereport = new CustomerTypeReport();
        //                    //foreach (int c in query)
        //                    //{
        //                    //outletname
        //                    if (str.Contains("1"))
        //                    {
        //                        var outletname = context.Database.SqlQuery<string>("select OutletDetails.OutletName from CustomerDetails join OutletDetails on CustomerDetails.EnrollingOutlet = OutletDetails.OutletId where CustomerId=" + id.CustomerId + "").FirstOrDefault();

        //                        objcustomertypereport.OutletName = outletname;

        //                    }
        //                    //mobileno
        //                    if (str.Contains("2"))
        //                    {
        //                        objcustomertypereport.MobileNo = id.MobileNo;

        //                    }
        //                    //customername
        //                    if (str.Contains("3"))
        //                    {
        //                        objcustomertypereport.CustomerName = id.CustomerName;
        //                    }
        //                    //firsttxndt
        //                    if (str.Contains("4"))
        //                    {
        //                        var itemfirstdt = context.Database.SqlQuery<DateTime>("select Min(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();
        //                        objcustomertypereport.FirstTxnDate = itemfirstdt;
        //                    }
        //                    //lasttxndt
        //                    if (str.Contains("5"))
        //                    {
        //                        var Lasttxndate = context.Database.SqlQuery<DateTime>("select Max(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

        //                        objcustomertypereport.LastTxnDate = Lasttxndate;
        //                    }
        //                    //nooftxn
        //                    if (str.Contains("6"))
        //                    {
        //                        var NoofTxn = context.Database.SqlQuery<int>("select count(CustomerId) from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

        //                        objcustomertypereport.NoOfTxn = NoofTxn;
        //                    }
        //                    //enrolleddate
        //                    if (str.Contains("7"))
        //                    {
        //                        objcustomertypereport.EnrolledDate = id.DOJ;

        //                    }
        //                    //availablepoints
        //                    if (str.Contains("8"))
        //                    {
        //                        objcustomertypereport.TotalAvailablePoints = id.Points;

        //                    }
        //                    //totalearn
        //                    if (str.Contains("9"))
        //                    {
        //                        var TotalEarn = context.Database.SqlQuery<decimal>("select sum(pointsearned) from transactionmaster where TransType in(1, 2) and CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

        //                        objcustomertypereport.TotalEarn = TotalEarn;
        //                    }
        //                    //totalburn
        //                    if (str.Contains("10"))
        //                    {
        //                        var TotalBurn = context.Database.SqlQuery<decimal>("select sum(PointsBurned) from transactionmaster where TransType ='2' and CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

        //                        objcustomertypereport.TotalBurn = TotalBurn;

        //                    }
        //                    //totalspend
        //                    if (str.Contains("11"))
        //                    {
        //                        var Totalspend = context.Database.SqlQuery<decimal>("select sum(InvoiceAmt) from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();
        //                        objcustomertypereport.TotalSpend = Totalspend;
        //                    }
        //                    //bonuspoints
        //                    if (str.Contains("12"))
        //                    {
        //                        //var txn = context.Database.SqlQuery<TransactionMaster>("select * from transactionmaster where InvoiceNo in('B_Birthday', 'B_Anniversary','B_ProfileUpdate','B_ReferralBonus', 'B_GiftingPoints', 'Bonus')and TransType in(1, 2)").ToList();

        //                        //var item = (from x in txn
        //                        //            group x by new { x.CustomerId } into g
        //                        //            select new
        //                        //            {

        //                        //                Bonus = g.Sum(x => x.PointsEarned)
        //                        //            }).FirstOrDefault();
        //                        var Bonus = context.Database.SqlQuery<decimal>("select sum(pointsearned) from transactionmaster where InvoiceNo in('B_Birthday', 'B_Anniversary','B_ProfileUpdate','B_ReferralBonus', 'B_GiftingPoints', 'Bonus')and TransType in(1, 2) and CustomerId=" + id.CustomerId + " group by customerid  ").FirstOrDefault();
        //                        objcustomertypereport.BonusPoints = Bonus;
        //                    }


        //                    lstcusttype.Add(objcustomertypereport);
        //                    // }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        newexception.AddException(ex, "GenerateCustomerTypeReport");
        //    }
        //    return lstcusttype;
        //}
        //public List<TransactionTypeReport> GenerateTxnTypeReport(object[] ColumnId, List<CustomerDetail> lstcustdetails, string GroupId, string connstr)
        //{

        //    List<TransactionTypeReport> lstTxntype = new List<TransactionTypeReport>();
        //    List<TransactionMaster> objtrans = new List<TransactionMaster>();
        //    var str = String.Join(",", ColumnId);
        //    var query = from val in str.Split(',')
        //                select int.Parse(val);
        //    try
        //    {
        //        using (var context = new BOTSDBContext(connstr))
        //        {
        //            objtrans = context.Database.SqlQuery<TransactionMaster>("select * from TransactionMaster where InvoiceNo != 'B_Birthday' and InvoiceNo!= 'B_Anniversary'and InvoiceNo!= 'B_ProfileUpdate'and InvoiceNo!= 'B_ReferralBonus' and InvoiceNo!= 'B_GiftingPoints'and InvoiceNo!= 'Bonus'and TransType in(1, 2)").ToList();

        //            if (lstcustdetails != null)
        //            {

        //                foreach (var id in lstcustdetails)
        //                {
        //                    TransactionTypeReport objtranstypereport = new TransactionTypeReport();

        //                    ListEarn objearn = new ListEarn();
        //                    ListBurn objburn = new ListBurn();
        //                    ListType objtype = new ListType();
        //                    ListInvoiceNo objinvoiceno = new ListInvoiceNo();
        //                    ListInvoiceAmt objinvoiceamt = new ListInvoiceAmt();
        //                    ListTxnDate objtxndt = new ListTxnDate();
        //                    List<ListEarn> lstearn = new List<ListEarn>();
        //                    List<ListBurn> lstburn = new List<ListBurn>();
        //                    List<ListType> lsttype = new List<ListType>();
        //                    List<ListInvoiceNo> lstinvoiceno = new List<ListInvoiceNo>();
        //                    List<ListInvoiceAmt> lstinvoiceamt = new List<ListInvoiceAmt>();
        //                    List<ListTxnDate> lsttxndate = new List<ListTxnDate>();


        //                    // pointsearn
        //                    if (str.Contains("13"))
        //                    {
        //                        var pointsEarn = context.Database.SqlQuery<decimal?>("select PointsEarned from TransactionMaster where TransType='1'and CustomerId=" + id.CustomerId + "").ToList();

        //                        if (pointsEarn.Count > 0)
        //                        {
        //                            foreach (var item in pointsEarn)
        //                            {
        //                                // dicearnlist.Add(id.CustomerId, item);
        //                                objearn.CustomerId = id.CustomerId;
        //                                objearn.PointsEarn = item;
        //                                lstearn.Add(objearn);

        //                            }
        //                        }
        //                        //objtranstypereport.PointsEarn = pointsEarn;

        //                    }
        //                    //pointsburn
        //                    if (str.Contains("14"))
        //                    {
        //                        var pointsBurn = context.Database.SqlQuery<decimal?>("select PointsBurned from TransactionMaster where TransType='2'and CustomerId=" + id.CustomerId + "").ToList();
        //                        if (pointsBurn.Count > 0)
        //                        {
        //                            foreach (var item in pointsBurn)
        //                            {
        //                                //dicburnlist.Add(id.CustomerId, item);
        //                                objburn.CustomerId = id.CustomerId;
        //                                objburn.PointsBurn = item;
        //                                lstburn.Add(objburn);
        //                            }
        //                        }
        //                    }
        //                    //type
        //                    if (str.Contains("15"))
        //                    {
        //                        var type = context.Database.SqlQuery<string>("select ( case when TransType='1'then'Earn'Else 'Burn' end) as Type from TransactionMaster where CustomerId=" + id.CustomerId + "").ToList();
        //                        if (type.Count > 0)
        //                        {
        //                            foreach (var item in type)
        //                            {
        //                                //dictypelist.Add(id.CustomerId, item);
        //                                objtype.CustomerId = id.CustomerId;
        //                                objtype.Type = item;
        //                                lsttype.Add(objtype);
        //                            }
        //                        }
        //                    }
        //                    //invoiceno
        //                    if (str.Contains("16"))
        //                    {
        //                        var invoiceno = context.Database.SqlQuery<string>("select InvoiceNo from TransactionMaster where CustomerId=" + id.CustomerId + "").ToList();
        //                        if (invoiceno.Count > 0)
        //                        {
        //                            foreach (var item in invoiceno)
        //                            {
        //                                // dicinvoicenolist.Add(id.CustomerId, item);
        //                                objinvoiceno.CustomerId = id.CustomerId;
        //                                objinvoiceno.InvoiceNo = item;
        //                                lstinvoiceno.Add(objinvoiceno);
        //                            }
        //                        }
        //                    }
        //                    //invoiceamt
        //                    if (str.Contains("17"))
        //                    {

        //                        var invoiceamt = context.Database.SqlQuery<decimal?>("select InvoiceAmt from TransactionMaster where CustomerId =" + id.CustomerId + "").ToList();
        //                        if (invoiceamt.Count > 0)
        //                        {
        //                            foreach (var item in invoiceamt)
        //                            {
        //                                // dicinvoiceamtlist.Add(id.CustomerId, item);
        //                                objinvoiceamt.CustomerId = id.CustomerId;
        //                                objinvoiceamt.InvoiceAmt = item;
        //                                lstinvoiceamt.Add(objinvoiceamt);
        //                            }
        //                        }
        //                    }
        //                    //txndate
        //                    if (str.Contains("18"))
        //                    {
        //                        var txndate = context.Database.SqlQuery<DateTime?>("select Datetime from TransactionMaster where CustomerId =" + id.CustomerId + "").ToList();
        //                        if (txndate.Count > 0)
        //                        {
        //                            foreach (var item in txndate)
        //                            {
        //                                //dictxndatelist.Add(id.CustomerId, item);
        //                                objtxndt.CustomerId = id.CustomerId;
        //                                objtxndt.TxnDate = item;
        //                                lsttxndate.Add(objtxndt);
        //                            }
        //                        }
        //                    }
        //                    //outletnm
        //                    if (str.Contains("1"))
        //                    {
        //                        var outletname = context.Database.SqlQuery<string>("select OutletDetails.OutletName from CustomerDetails join OutletDetails on CustomerDetails.EnrollingOutlet = OutletDetails.OutletId where CustomerId=" + id.CustomerId + "").FirstOrDefault();

        //                        objtranstypereport.OutletName = outletname;

        //                    }
        //                    //mobileno
        //                    if (str.Contains("2"))
        //                    {
        //                        objtranstypereport.MobileNo = id.MobileNo;

        //                    }
        //                    //customername
        //                    if (str.Contains("3"))
        //                    {
        //                        objtranstypereport.CustomerName = id.CustomerName;
        //                    }
        //                    //firsttxndt
        //                    if (str.Contains("4"))
        //                    {

        //                        var itemfirstdt = context.Database.SqlQuery<DateTime>("select Min(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();
        //                        objtranstypereport.FirstTxnDate = itemfirstdt;
        //                    }
        //                    //lasttxndt
        //                    if (str.Contains("5"))
        //                    {

        //                        var Lasttxndate = context.Database.SqlQuery<DateTime>("select Max(datetime) as firsttxndate from transactionmaster where CustomerId=" + id.CustomerId + " group by customerid ").FirstOrDefault();

        //                        objtranstypereport.LastTxnDate = Lasttxndate;
        //                    }
        //                    //enrolleddate
        //                    if (str.Contains("7"))
        //                    {
        //                        objtranstypereport.EnrolledDate = id.DOJ;

        //                    }
        //                    //bonuspoints
        //                    if (str.Contains("12"))
        //                    {

        //                        var Bonus = context.Database.SqlQuery<decimal>("select sum(pointsearned) from transactionmaster where InvoiceNo in('B_Birthday', 'B_Anniversary','B_ProfileUpdate','B_ReferralBonus', 'B_GiftingPoints', 'Bonus')and TransType in(1, 2) and CustomerId=" + id.CustomerId + " group by customerid  ").FirstOrDefault();
        //                        objtranstypereport.BonusPoints = Bonus;
        //                    }

        //                    int[] arr = new int[] { lstearn.Count, lsttxndate.Count, lsttype.Count, lstinvoiceno.Count, lstinvoiceamt.Count, lstburn.Count };
        //                    System.Array.Sort<int>(arr, new Comparison<int>(
        //                      (i1, i2) => i2.CompareTo(i1)));

        //                    int count = arr[0];
        //                    for (int a = 0; a < count; a++)
        //                    {
        //                        TransactionTypeReport objmultipletxn = new TransactionTypeReport();
        //                        var propInfo = objtranstypereport.GetType().GetProperties();
        //                        foreach (var item in propInfo)
        //                        {
        //                            objmultipletxn.GetType().GetProperty(item.Name).SetValue(objmultipletxn, item.GetValue(objtranstypereport, null), null);
        //                        }

        //                        //objmultipletxn.   
        //                        if (lstearn.Count >= a + 1)
        //                        {
        //                            objmultipletxn.PointsEarn = lstearn[a].PointsEarn;
        //                        }
        //                        if (lsttxndate.Count >= a + 1)
        //                        {
        //                            objmultipletxn.TxnDate = lsttxndate[a].TxnDate;
        //                        }
        //                        if (lsttype.Count >= a + 1)
        //                        {
        //                            objmultipletxn.Type = lsttype[a].Type;
        //                        }
        //                        if (lstinvoiceno.Count >= a + 1)
        //                        {
        //                            objmultipletxn.InvoiceNo = lstinvoiceno[a].InvoiceNo;
        //                        }
        //                        if (lstinvoiceamt.Count >= a + 1)
        //                        {
        //                            objmultipletxn.InvoiceAmt = lstinvoiceamt[a].InvoiceAmt;
        //                        }
        //                        if (lstburn.Count >= a + 1)
        //                        {
        //                            objmultipletxn.PointsBurn = lstburn[a].PointsBurn;
        //                        }
        //                        lstTxntype.Add(objmultipletxn);
        //                    }


        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        newexception.AddException(ex, "GenerateTxnTypeReport");
        //    }
        //    return lstTxntype;
        //}
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
                    newexception.AddException(ex, "GetFilterCountOfDrillDown");
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
                newexception.AddException(ex, "GetStartDtOfProgram");
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

        public int[] GetSSFilterCount(object[] objData, string loginId, bool IsCount, string connstr)
        {
            int count = 0;
            int index = 1;
            string Criteria = string.Empty;
            string query = "select count(*) from tblSmartSlicerMaster where ";
            string WhereClause = string.Empty;

            foreach (Dictionary<string, object> item in objData)
            {

                if (index == 1)
                {
                    string FirstChk = Convert.ToString(item["FirstChk"]);
                    string SecondChk = Convert.ToString(item["SecondChk"]);
                    if (FirstChk == "Transacting")
                    {
                        Criteria += "Transacting";
                        WhereClause += " TotalTxnCount > 0 ";

                        if (SecondChk == "All")
                        {
                            Criteria += " : All";
                        }
                        if (SecondChk == "OnlyOnce")
                        {
                            Criteria += " : Only Once";
                            Criteria += " - " + Convert.ToString(item["OnlyOnceSegment"]);
                            if (Convert.ToString(item["OnlyOnceSegment"]) != "All")
                            {
                                try
                                {
                                    using (var context = new BOTSDBContext(connstr))
                                    {
                                        decimal? avgTicketSize = context.tblSmartSlicerRuleMasters.Select(x => x.AvgTicketSize).FirstOrDefault();
                                        if (avgTicketSize.HasValue)
                                        {
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "High Spend Long time")
                                            {
                                                WhereClause += "and VisitStatus= 'LongTime' and AvgTicketSize > " + avgTicketSize;
                                            }
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "Low Spend Long time")
                                            {
                                                WhereClause += "and VisitStatus= 'LongTime' and AvgTicketSize < " + avgTicketSize;
                                            }
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "High Spend Recent")
                                            {
                                                WhereClause += "and VisitStatus= 'RecentTime' and AvgTicketSize > " + avgTicketSize;
                                            }
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "Low Spend Recent")
                                            {
                                                WhereClause += "and VisitStatus= 'RecentTime' and AvgTicketSize < " + avgTicketSize;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    newexception.AddException(ex, "errorofgetting data");
                                }

                            }
                        }
                        if (SecondChk == "Inactive")
                        {
                            Criteria += " : Inactive";
                            Criteria += " - " + Convert.ToString(item["InactiveSegment"]);
                            if (Convert.ToString(item["ExcludeOnlyOnce"]) == "True")
                            {
                                Criteria += " - Exclude Only Once";
                            }
                        }
                        if (SecondChk == "Cumulative")
                        {
                            Criteria += " : Cumulative";
                            if (Convert.ToString(item["SecondChkCumu"]) != "No")
                            {
                                //Criteria += "<br/>" + Convert.ToString(item["SecondChkCumu"]);
                                if (Convert.ToString(item["SecondChkCumu"]) == "TxnCount")
                                {
                                    Criteria += " : Transaction Count";
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["LSGT"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["LSGT"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["TxnCount"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["TxnCount"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Months"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Months"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Frequency"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Frequency"]);
                                    }
                                }
                                if (Convert.ToString(item["SecondChkCumu"]) == "CSpend")
                                {
                                    Criteria += "- Spend";
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["LSGT"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["LSGT"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["TxnCount"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["TxnCount"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Months"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Months"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Frequency"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Frequency"]);
                                    }
                                }
                            }
                        }
                    }
                    if (FirstChk == "NonTransacting")
                    {
                        Criteria += "Non Transacting";
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SecondChk"])))
                        {
                            Criteria += " - " + Convert.ToString(item["SecondChk"]);
                            if (Convert.ToString(item["SecondChk"]) == "DLC")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(item["QRSource"])))
                                {
                                    Criteria += " - Source : " + Convert.ToString(item["QRSource"]);
                                }
                            }
                        }
                    }
                    if (FirstChk == "MemberAcquition")
                    {
                        Criteria += "Member Acquition";
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SecondChk"])))
                        {
                            Criteria += " - " + Convert.ToString(item["SecondChk"]);
                            Criteria += " - " + Convert.ToString(item["MAFinal"]);

                            if (Convert.ToString(item["SecondChk"]) == "DLC")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(item["QRSource"])))
                                {
                                    Criteria += "<br/>Source QR : " + Convert.ToString(item["QRSource"]);
                                }
                            }
                        }
                    }
                }
                if (index == 2)
                {
                    if (Convert.ToString(item["IsAvgTicket"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AvgTicketFrm"])))
                        {
                            Criteria += "<br/>Avgerage ticket size from : " + Convert.ToString(item["AvgTicketFrm"]);
                            WhereClause += " and AvgTicketSize > " + Convert.ToString(item["AvgTicketFrm"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AvgTicketTo"])))
                        {
                            Criteria += "<br/>Avgerage ticket size to : " + Convert.ToString(item["AvgTicketTo"]);
                            WhereClause += " and AvgTicketSize < " + Convert.ToString(item["AvgTicketTo"]);
                        }
                        //Criteria += "<br/>Avgerage ticket size : " + Convert.ToString(item["AvgTicket"]);
                    }
                    if (Convert.ToString(item["IsEnrolledRange"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["Enrollfromdt"])))
                        {
                            Criteria += "<br/>Enrolled date from : " + Convert.ToString(item["Enrollfromdt"]);
                            WhereClause += " and EnrolledDate > " + Convert.ToString(item["Enrollfromdt"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["Enrolltodt"])))
                        {
                            Criteria += "<br/>Enrolled date to : " + Convert.ToString(item["Enrolltodt"]);
                            WhereClause += " and EnrolledDate < " + Convert.ToString(item["Enrolltodt"]);
                        }
                    }
                    if (Convert.ToString(item["IsInactiveSince"]) == "Yes")
                    {
                        Criteria += "<br/>Inactive since(days) : " + Convert.ToString(item["NonTransactedSince"]);
                        WhereClause += " and InActiveDays > " + Convert.ToString(item["NonTransactedSince"]);
                    }
                    if (Convert.ToString(item["IsSpend"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMin"])))
                        {
                            Criteria += "<br/>Spend min : " + Convert.ToString(item["SpendMin"]);
                            WhereClause += " and Spends > " + Convert.ToString(item["SpendMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMax"])))
                        {
                            Criteria += "<br/>Spend max : " + Convert.ToString(item["SpendMax"]);
                            WhereClause += " and Spends < " + Convert.ToString(item["SpendMax"]);
                        }
                    }
                    if (Convert.ToString(item["IsTxnCount"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["CountMin"])))
                        {
                            Criteria += "<br/>Txn Count min : " + Convert.ToString(item["CountMin"]);
                            WhereClause += " and TotalTxnCount > " + Convert.ToString(item["CountMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["CountMax"])))
                        {
                            Criteria += "<br/>Txn Count max : " + Convert.ToString(item["CountMax"]);
                            WhereClause += " and TotalTxnCount < " + Convert.ToString(item["CountMax"]);
                        }
                    }
                    if (Convert.ToString(item["IsPointBalance"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PointsBalMin"])))
                        {
                            Criteria += "<br/>Point Balance min : " + Convert.ToString(item["PointsBalMin"]);
                            WhereClause += " and PointsBalance > " + Convert.ToString(item["PointsBalMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PointsBalMax"])))
                        {
                            Criteria += "<br/>Point Balance max : " + Convert.ToString(item["PointsBalMax"]);
                            WhereClause += " and PointsBalance < " + Convert.ToString(item["PointsBalMax"]);
                        }
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["IsRedeemed"])))
                    {
                        Criteria += "<br/>Redeemed : " + Convert.ToString(item["IsRedeemed"]);
                        int isR = 0;
                        if (Convert.ToString(item["IsRedeemed"]) == "Yes")
                            isR = 1;
                        WhereClause += " and RedeemStatus < " + isR;
                    }
                }
                if (index == 3)
                {
                    if (Convert.ToString(item["IsNeverPurchased"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPCategoryName"])))
                        {
                            Criteria += "<br/>Never Purchase Category : " + Convert.ToString(item["NPCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPSubCategoryName"])))
                        {
                            Criteria += "<br/>Never Purchase Sub Category : " + Convert.ToString(item["NPSubCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPProductName"])))
                        {
                            Criteria += "<br/>Never Purchase Product Name : " + Convert.ToString(item["NPProductName"]);
                        }
                    }

                    if (Convert.ToString(item["IsPurchased"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PCategoryName"])))
                        {
                            Criteria += "<br/>Purchase Category : " + Convert.ToString(item["PCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PSubCategoryName"])))
                        {
                            Criteria += "<br/>Purchase Sub Category : " + Convert.ToString(item["PSubCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PProductName"])))
                        {
                            Criteria += "<br/>Purchase Product Name : " + Convert.ToString(item["PProductName"]);
                        }
                    }
                }
                if (index == 4)
                {
                    if (Convert.ToString(item["IsBrands"]) == "Yes")
                    {
                        Criteria += "<br/>Zone : " + Convert.ToString(item["Zone"]);
                    }
                    if (Convert.ToString(item["IsOutlet"]) == "Yes")
                    {
                        //Need to use item["OutletIds"] for Query building
                        var outlets = (object[])item["Outlets"];
                        Criteria += "<br/>Outlet : ";
                        string outletIds = string.Empty;
                        string outletAll = string.Empty;
                        foreach (var item1 in outlets)
                        {
                            Criteria += ", " + Convert.ToString(Convert.ToString(item1));
                            if (Convert.ToString(item1) == "All")
                            {
                                outletAll = "All";
                            }
                            else
                            {
                                outletIds += Convert.ToString(Convert.ToString(item1)) + ",";
                            }
                        }
                        outletIds = outletIds.Remove(outletIds.Length - 1);
                        if (outletAll != "All")
                            WhereClause += " and EnrolledOutletId in (" + outletIds + ")";

                    }
                    if (Convert.ToString(item["IsTOutlet"]) == "Yes")
                    {
                        //Need to use item["TransactingOutletIds"] for Query building
                        var outlets = (object[])item["TransactingOutlets"];
                        Criteria += "<br/>Transacting Outlet : ";
                        foreach (var item1 in outlets)
                        {
                            Criteria += ", " + Convert.ToString(Convert.ToString(item1));
                        }
                    }
                    if (Convert.ToString(item["TOutletsAnyLast"]) == "Yes")
                    {
                        Criteria += "<br/>Last or Anyone : " + Convert.ToString(item["TOutletsAnyLast"]);
                    }
                }
                if (index == 5)
                {
                    if (Convert.ToString(item["IsTier"]) == "Yes")
                    {
                        Criteria += "<br/>Tier : " + Convert.ToString(item["IsTier"]);
                    }
                    if (Convert.ToString(item["IsGender"]) == "Yes")
                    {
                        Criteria += "<br/>Gender : " + Convert.ToString(item["IsGender"]);
                        if (Convert.ToString(item["Gender"]) == "Male")
                            WhereClause += " and Gender = 'M' ";
                        if (Convert.ToString(item["Gender"]) == "Female")
                            WhereClause += " and Gender = 'F' ";
                    }
                    if (Convert.ToString(item["IsCity"]) == "Yes")
                    {
                        Criteria += "<br/>City : " + Convert.ToString(item["IsCity"]);
                    }
                    if (Convert.ToString(item["IsAge"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AgeFrom"])))
                        {
                            Criteria += "<br/>Age Min : " + Convert.ToString(item["AgeFrom"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AgeTo"])))
                        {
                            Criteria += "<br/>Age Max : " + Convert.ToString(item["AgeTo"]);
                        }
                    }
                }
                index++;
            }

            query = query + WhereClause;
            int totalCount = 0;
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    count = context.Database.SqlQuery<int>(query).FirstOrDefault();
                    totalCount = context.tblSmartSlicerMasters.Count();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "errorofgetting data");
            }

            int[] lstCount = new int[2];
            lstCount[0] = count;
            lstCount[1] = totalCount;
            return lstCount;
        }

        public bool SaveDataset(object[] objData, CustomerLoginDetail userDetails, string DSName)
        {
            bool status = false;
            string Criteria = string.Empty;
            string QueryCriteria = string.Empty;
            int index = 1;
            foreach (Dictionary<string, object> item in objData)
            {
                string WhereClause = string.Empty;
                if (index == 1)
                {
                    string FirstChk = Convert.ToString(item["FirstChk"]);
                    string SecondChk = Convert.ToString(item["SecondChk"]);
                    if (FirstChk == "Transacting")
                    {
                        Criteria += "Transacting";

                        if (SecondChk == "All")
                        {
                            Criteria += " : All";
                        }
                        if (SecondChk == "OnlyOnce")
                        {
                            Criteria += " : Only Once";
                            Criteria += " - " + Convert.ToString(item["OnlyOnceSegment"]);
                        }
                        if (SecondChk == "Inactive")
                        {
                            Criteria += " : Inactive";
                            Criteria += " - " + Convert.ToString(item["InactiveSegment"]);
                            if (Convert.ToString(item["ExcludeOnlyOnce"]) == "True")
                            {
                                Criteria += " - Exclude Only Once";
                            }
                        }
                        if (SecondChk == "Cumulative")
                        {
                            Criteria += " : Cumulative";
                            if (Convert.ToString(item["SecondChkCumu"]) != "No")
                            {
                                //Criteria += "<br/>" + Convert.ToString(item["SecondChkCumu"]);
                                if (Convert.ToString(item["SecondChkCumu"]) == "TxnCount")
                                {
                                    Criteria += " : Transaction Count";
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["LSGT"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["LSGT"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["TxnCount"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["TxnCount"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Months"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Months"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Frequency"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Frequency"]);
                                    }
                                }
                                if (Convert.ToString(item["SecondChkCumu"]) == "CSpend")
                                {
                                    Criteria += "- Spend";
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["LSGT"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["LSGT"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["TxnCount"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["TxnCount"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Months"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Months"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Frequency"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Frequency"]);
                                    }
                                }
                            }
                        }
                    }
                    if (FirstChk == "NonTransacting")
                    {
                        Criteria += "Non Transacting";
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SecondChk"])))
                        {
                            Criteria += " - " + Convert.ToString(item["SecondChk"]);
                            if (Convert.ToString(item["SecondChk"]) == "DLC")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(item["QRSource"])))
                                {
                                    Criteria += " - Source : " + Convert.ToString(item["QRSource"]);
                                }
                            }
                        }
                    }
                    if (FirstChk == "MemberAcquition")
                    {
                        Criteria += "Member Acquition";
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SecondChk"])))
                        {
                            Criteria += " - " + Convert.ToString(item["SecondChk"]);
                            Criteria += " - " + Convert.ToString(item["MAFinal"]);

                            if (Convert.ToString(item["SecondChk"]) == "DLC")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(item["QRSource"])))
                                {
                                    Criteria += "<br/>Source QR : " + Convert.ToString(item["QRSource"]);
                                }
                            }
                        }
                    }
                }
                if (index == 2)
                {
                    if (Convert.ToString(item["IsAvgTicket"]) == "Yes")
                    {
                        Criteria += "<br/>Avgerage ticket size : " + Convert.ToString(item["AvgTicket"]);
                    }
                    if (Convert.ToString(item["IsEnrolledRange"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["Enrollfromdt"])))
                        {
                            Criteria += "<br/>Enrolled date from : " + Convert.ToString(item["Enrollfromdt"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["Enrolltodt"])))
                        {
                            Criteria += "<br/>Enrolled date to : " + Convert.ToString(item["Enrolltodt"]);
                        }
                    }
                    if (Convert.ToString(item["IsInactiveSince"]) == "Yes")
                    {
                        Criteria += "<br/>Inactive since(days) : " + Convert.ToString(item["NonTransactedSince"]);
                    }
                    if (Convert.ToString(item["IsSpend"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMin"])))
                        {
                            Criteria += "<br/>Spend min : " + Convert.ToString(item["SpendMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMax"])))
                        {
                            Criteria += "<br/>Spend max : " + Convert.ToString(item["SpendMax"]);
                        }
                    }
                    if (Convert.ToString(item["IsTxnCount"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMin"])))
                        {
                            Criteria += "<br/>Spend min : " + Convert.ToString(item["SpendMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMax"])))
                        {
                            Criteria += "<br/>Spend max : " + Convert.ToString(item["SpendMax"]);
                        }
                    }
                    if (Convert.ToString(item["IsPointBalance"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PointsBalMin"])))
                        {
                            Criteria += "<br/>Point Balance min : " + Convert.ToString(item["PointsBalMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PointsBalMax"])))
                        {
                            Criteria += "<br/>Point Balance max : " + Convert.ToString(item["PointsBalMax"]);
                        }
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["IsRedeemed"])))
                    {
                        Criteria += "<br/>Redeemed : " + Convert.ToString(item["IsRedeemed"]);
                    }
                }
                if (index == 3)
                {
                    if (Convert.ToString(item["IsNeverPurchased"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPCategoryName"])))
                        {
                            Criteria += "<br/>Never Purchase Category : " + Convert.ToString(item["NPCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPSubCategoryName"])))
                        {
                            Criteria += "<br/>Never Purchase Sub Category : " + Convert.ToString(item["NPSubCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPProductName"])))
                        {
                            Criteria += "<br/>Never Purchase Product Name : " + Convert.ToString(item["NPProductName"]);
                        }
                    }

                    if (Convert.ToString(item["IsPurchased"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PCategoryName"])))
                        {
                            Criteria += "<br/>Purchase Category : " + Convert.ToString(item["PCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PSubCategoryName"])))
                        {
                            Criteria += "<br/>Purchase Sub Category : " + Convert.ToString(item["PSubCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PProductName"])))
                        {
                            Criteria += "<br/>Purchase Product Name : " + Convert.ToString(item["PProductName"]);
                        }
                    }
                }
                if (index == 4)
                {
                    if (Convert.ToString(item["IsBrands"]) == "Yes")
                    {
                        Criteria += "<br/>Zone : " + Convert.ToString(item["Zone"]);
                    }
                    if (Convert.ToString(item["IsOutlet"]) == "Yes")
                    {
                        //Need to use item["OutletIds"] for Query building
                        var outlets = (object[])item["Outlets"];
                        Criteria += "<br/>Outlet : ";
                        foreach (var item1 in outlets)
                        {
                            Criteria += ", " + Convert.ToString(Convert.ToString(item1));
                        }

                    }
                    if (Convert.ToString(item["IsTOutlet"]) == "Yes")
                    {
                        //Need to use item["TransactingOutletIds"] for Query building
                        var outlets = (object[])item["TransactingOutlets"];
                        Criteria += "<br/>Transacting Outlet : ";
                        foreach (var item1 in outlets)
                        {
                            Criteria += ", " + Convert.ToString(Convert.ToString(item1));
                        }
                    }
                    if (Convert.ToString(item["TOutletsAnyLast"]) == "Yes")
                    {
                        Criteria += "<br/>Last or Anyone : " + Convert.ToString(item["TOutletsAnyLast"]);
                    }
                }
                if (index == 5)
                {
                    if (Convert.ToString(item["IsTier"]) == "Yes")
                    {
                        Criteria += "<br/>Tier : " + Convert.ToString(item["IsTier"]);
                    }
                    if (Convert.ToString(item["IsGender"]) == "Yes")
                    {
                        Criteria += "<br/>Gender : " + Convert.ToString(item["IsGender"]);
                    }
                    if (Convert.ToString(item["IsCity"]) == "Yes")
                    {
                        Criteria += "<br/>City : " + Convert.ToString(item["IsCity"]);
                    }
                    if (Convert.ToString(item["IsAge"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AgeFrom"])))
                        {
                            Criteria += "<br/>Age Min : " + Convert.ToString(item["AgeFrom"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AgeTo"])))
                        {
                            Criteria += "<br/>Age Max : " + Convert.ToString(item["AgeTo"]);
                        }
                    }
                }
                index++;
            }
            try
            {
                using (var context = new BOTSDBContext(userDetails.connectionString))
                {
                    tblCRDataset objDataset = new tblCRDataset();
                    objDataset.DSName = DSName;
                    objDataset.DSCriteria = Criteria;
                    objDataset.DSCriteriaForQuery = QueryCriteria;
                    objDataset.AddedBy = userDetails.LoginId;
                    objDataset.AddedDate = DateTime.Now;
                    context.tblCRDatasets.Add(objDataset);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetStartDtOfProgram");
            }
            return status;
        }

        public List<tblCRDataset> GetCRDataset(string connectionString)
        {
            List<tblCRDataset> lstData = new List<tblCRDataset>();
            using (var context = new BOTSDBContext(connectionString))
            {
                lstData = context.tblCRDatasets.ToList();
            }

            return lstData;
        }

        public List<CustomerTypeReport> GetSSFilterReport(object[] objData, string columns, string loginId, string connstr)
        {
            int count = 0;
            int index = 1;
            string Criteria = string.Empty;
            string query = "select " + columns + " from tblSmartSlicerMaster where ";
            string WhereClause = string.Empty;

            foreach (Dictionary<string, object> item in objData)
            {

                if (index == 1)
                {
                    string FirstChk = Convert.ToString(item["FirstChk"]);
                    string SecondChk = Convert.ToString(item["SecondChk"]);
                    if (FirstChk == "Transacting")
                    {
                        Criteria += "Transacting";
                        WhereClause += " TotalTxnCount > 0 ";

                        if (SecondChk == "All")
                        {
                            Criteria += " : All";
                        }
                        if (SecondChk == "OnlyOnce")
                        {
                            Criteria += " : Only Once";
                            Criteria += " - " + Convert.ToString(item["OnlyOnceSegment"]);
                            if (Convert.ToString(item["OnlyOnceSegment"]) != "All")
                            {
                                try
                                {
                                    using (var context = new BOTSDBContext(connstr))
                                    {
                                        decimal? avgTicketSize = context.tblSmartSlicerRuleMasters.Select(x => x.AvgTicketSize).FirstOrDefault();
                                        if (avgTicketSize.HasValue)
                                        {
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "High Spend Long time")
                                            {
                                                WhereClause += "and VisitStatus= 'LongTime' and AvgTicketSize > " + avgTicketSize;
                                            }
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "Low Spend Long time")
                                            {
                                                WhereClause += "and VisitStatus= 'LongTime' and AvgTicketSize < " + avgTicketSize;
                                            }
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "High Spend Recent")
                                            {
                                                WhereClause += "and VisitStatus= 'RecentTime' and AvgTicketSize > " + avgTicketSize;
                                            }
                                            if (Convert.ToString(item["OnlyOnceSegment"]) == "Low Spend Recent")
                                            {
                                                WhereClause += "and VisitStatus= 'RecentTime' and AvgTicketSize < " + avgTicketSize;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    newexception.AddException(ex, "errorofgetting data");
                                }

                            }
                        }
                        if (SecondChk == "Inactive")
                        {
                            Criteria += " : Inactive";
                            Criteria += " - " + Convert.ToString(item["InactiveSegment"]);
                            if (Convert.ToString(item["ExcludeOnlyOnce"]) == "True")
                            {
                                Criteria += " - Exclude Only Once";
                            }
                        }
                        if (SecondChk == "Cumulative")
                        {
                            Criteria += " : Cumulative";
                            if (Convert.ToString(item["SecondChkCumu"]) != "No")
                            {
                                //Criteria += "<br/>" + Convert.ToString(item["SecondChkCumu"]);
                                if (Convert.ToString(item["SecondChkCumu"]) == "TxnCount")
                                {
                                    Criteria += " : Transaction Count";
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["LSGT"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["LSGT"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["TxnCount"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["TxnCount"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Months"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Months"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Frequency"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Frequency"]);
                                    }
                                }
                                if (Convert.ToString(item["SecondChkCumu"]) == "CSpend")
                                {
                                    Criteria += "- Spend";
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["LSGT"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["LSGT"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["TxnCount"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["TxnCount"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Months"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Months"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item["Frequency"])))
                                    {
                                        Criteria += " - " + Convert.ToString(item["Frequency"]);
                                    }
                                }
                            }
                        }
                    }
                    if (FirstChk == "NonTransacting")
                    {
                        Criteria += "Non Transacting";
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SecondChk"])))
                        {
                            Criteria += " - " + Convert.ToString(item["SecondChk"]);
                            if (Convert.ToString(item["SecondChk"]) == "DLC")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(item["QRSource"])))
                                {
                                    Criteria += " - Source : " + Convert.ToString(item["QRSource"]);
                                }
                            }
                        }
                    }
                    if (FirstChk == "MemberAcquition")
                    {
                        Criteria += "Member Acquition";
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SecondChk"])))
                        {
                            Criteria += " - " + Convert.ToString(item["SecondChk"]);
                            Criteria += " - " + Convert.ToString(item["MAFinal"]);

                            if (Convert.ToString(item["SecondChk"]) == "DLC")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(item["QRSource"])))
                                {
                                    Criteria += "<br/>Source QR : " + Convert.ToString(item["QRSource"]);
                                }
                            }
                        }
                    }
                }
                if (index == 2)
                {
                    if (Convert.ToString(item["IsAvgTicket"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AvgTicketFrm"])))
                        {
                            Criteria += "<br/>Avgerage ticket size from : " + Convert.ToString(item["AvgTicketFrm"]);
                            WhereClause += " and AvgTicketSize > " + Convert.ToString(item["AvgTicketFrm"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AvgTicketTo"])))
                        {
                            Criteria += "<br/>Avgerage ticket size to : " + Convert.ToString(item["AvgTicketTo"]);
                            WhereClause += " and AvgTicketSize < " + Convert.ToString(item["AvgTicketTo"]);
                        }
                        //Criteria += "<br/>Avgerage ticket size : " + Convert.ToString(item["AvgTicket"]);
                    }
                    if (Convert.ToString(item["IsEnrolledRange"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["Enrollfromdt"])))
                        {
                            Criteria += "<br/>Enrolled date from : " + Convert.ToString(item["Enrollfromdt"]);
                            WhereClause += " and EnrolledDate > " + Convert.ToString(item["Enrollfromdt"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["Enrolltodt"])))
                        {
                            Criteria += "<br/>Enrolled date to : " + Convert.ToString(item["Enrolltodt"]);
                            WhereClause += " and EnrolledDate < " + Convert.ToString(item["Enrolltodt"]);
                        }
                    }
                    if (Convert.ToString(item["IsInactiveSince"]) == "Yes")
                    {
                        Criteria += "<br/>Inactive since(days) : " + Convert.ToString(item["NonTransactedSince"]);
                        WhereClause += " and InActiveDays > " + Convert.ToString(item["NonTransactedSince"]);
                    }
                    if (Convert.ToString(item["IsSpend"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMin"])))
                        {
                            Criteria += "<br/>Spend min : " + Convert.ToString(item["SpendMin"]);
                            WhereClause += " and Spends > " + Convert.ToString(item["SpendMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SpendMax"])))
                        {
                            Criteria += "<br/>Spend max : " + Convert.ToString(item["SpendMax"]);
                            WhereClause += " and Spends < " + Convert.ToString(item["SpendMax"]);
                        }
                    }
                    if (Convert.ToString(item["IsTxnCount"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["CountMin"])))
                        {
                            Criteria += "<br/>Txn Count min : " + Convert.ToString(item["CountMin"]);
                            WhereClause += " and TotalTxnCount > " + Convert.ToString(item["CountMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["CountMax"])))
                        {
                            Criteria += "<br/>Txn Count max : " + Convert.ToString(item["CountMax"]);
                            WhereClause += " and TotalTxnCount < " + Convert.ToString(item["CountMax"]);
                        }
                    }
                    if (Convert.ToString(item["IsPointBalance"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PointsBalMin"])))
                        {
                            Criteria += "<br/>Point Balance min : " + Convert.ToString(item["PointsBalMin"]);
                            WhereClause += " and PointsBalance > " + Convert.ToString(item["PointsBalMin"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PointsBalMax"])))
                        {
                            Criteria += "<br/>Point Balance max : " + Convert.ToString(item["PointsBalMax"]);
                            WhereClause += " and PointsBalance < " + Convert.ToString(item["PointsBalMax"]);
                        }
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["IsRedeemed"])))
                    {
                        Criteria += "<br/>Redeemed : " + Convert.ToString(item["IsRedeemed"]);

                        if (Convert.ToString(item["IsRedeemed"]) == "Yes")
                            WhereClause += " and RedeemStatus = 1";
                        //WhereClause += " and RedeemStatus < " + isR;
                    }
                }
                if (index == 3)
                {
                    if (Convert.ToString(item["IsNeverPurchased"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPCategoryName"])))
                        {
                            Criteria += "<br/>Never Purchase Category : " + Convert.ToString(item["NPCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPSubCategoryName"])))
                        {
                            Criteria += "<br/>Never Purchase Sub Category : " + Convert.ToString(item["NPSubCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["NPProductName"])))
                        {
                            Criteria += "<br/>Never Purchase Product Name : " + Convert.ToString(item["NPProductName"]);
                        }
                    }

                    if (Convert.ToString(item["IsPurchased"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PCategoryName"])))
                        {
                            Criteria += "<br/>Purchase Category : " + Convert.ToString(item["PCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PSubCategoryName"])))
                        {
                            Criteria += "<br/>Purchase Sub Category : " + Convert.ToString(item["PSubCategoryName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["PProductName"])))
                        {
                            Criteria += "<br/>Purchase Product Name : " + Convert.ToString(item["PProductName"]);
                        }
                    }
                }
                if (index == 4)
                {
                    if (Convert.ToString(item["IsBrands"]) == "Yes")
                    {
                        Criteria += "<br/>Zone : " + Convert.ToString(item["Zone"]);
                    }
                    if (Convert.ToString(item["IsOutlet"]) == "Yes")
                    {
                        //Need to use item["OutletIds"] for Query building
                        var outlets = (object[])item["Outlets"];
                        Criteria += "<br/>Outlet : ";
                        string outletIds = string.Empty;
                        string outletAll = string.Empty;
                        foreach (var item1 in outlets)
                        {
                            Criteria += ", " + Convert.ToString(Convert.ToString(item1));
                            if (Convert.ToString(item1) == "All")
                            {
                                outletAll = "All";
                            }
                            else
                            {
                                outletIds += Convert.ToString(Convert.ToString(item1)) + ",";
                            }
                        }
                        outletIds = outletIds.Remove(outletIds.Length - 1);
                        if (outletAll != "All")
                            WhereClause += " and EnrolledOutletId in (" + outletIds + ")";

                    }
                    if (Convert.ToString(item["IsTOutlet"]) == "Yes")
                    {
                        //Need to use item["TransactingOutletIds"] for Query building
                        var outlets = (object[])item["TransactingOutlets"];
                        Criteria += "<br/>Transacting Outlet : ";
                        foreach (var item1 in outlets)
                        {
                            Criteria += ", " + Convert.ToString(Convert.ToString(item1));
                        }
                    }
                    if (Convert.ToString(item["TOutletsAnyLast"]) == "Yes")
                    {
                        Criteria += "<br/>Last or Anyone : " + Convert.ToString(item["TOutletsAnyLast"]);
                    }
                }
                if (index == 5)
                {
                    if (Convert.ToString(item["IsTier"]) == "Yes")
                    {
                        Criteria += "<br/>Tier : " + Convert.ToString(item["IsTier"]);
                    }
                    if (Convert.ToString(item["IsGender"]) == "Yes")
                    {
                        Criteria += "<br/>Gender : " + Convert.ToString(item["IsGender"]);
                        if (Convert.ToString(item["Gender"]) == "Male")
                            WhereClause += " and Gender = 'M' ";
                        if (Convert.ToString(item["Gender"]) == "Female")
                            WhereClause += " and Gender = 'F' ";
                    }
                    if (Convert.ToString(item["IsCity"]) == "Yes")
                    {
                        Criteria += "<br/>City : " + Convert.ToString(item["IsCity"]);
                    }
                    if (Convert.ToString(item["IsAge"]) == "Yes")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AgeFrom"])))
                        {
                            Criteria += "<br/>Age Min : " + Convert.ToString(item["AgeFrom"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item["AgeTo"])))
                        {
                            Criteria += "<br/>Age Max : " + Convert.ToString(item["AgeTo"]);
                        }
                    }
                }
                index++;
            }

            query = query + WhereClause;
            List<CustomerTypeReport> objcustomertypereport = new List<CustomerTypeReport>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objcustomertypereport = context.Database.SqlQuery<CustomerTypeReport>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "errorofgetting data");
            }


            return objcustomertypereport;
        }
    }
}

