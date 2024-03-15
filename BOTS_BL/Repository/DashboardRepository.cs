using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.IO;
using BOTS_BL.Models.IndividualDBModels;

namespace BOTS_BL.Repository
{

    public class DashboardRepository
    {
        Exceptions newexception = new Exceptions();

        public List<ExecutiveSummaryAllData> GetExecutiveSummaryAllData(string GroupId, string connstr)
        {
            List<ExecutiveSummaryAllData> lstobj = new List<ExecutiveSummaryAllData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    context.Database.CommandTimeout = 120;
                    lstobj = context.Database.SqlQuery<ExecutiveSummaryAllData>("select * from View_CustTxnSummary").ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetExecutiveSummaryAllData");
            }
            return lstobj;
        }
        public ExecutiveSummary GetDashboardData(string GroupId, string connstr, string LoginId, string frmDate, string toDate)
        {
            ExecutiveSummary dataDashboard = new ExecutiveSummary();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.Database.CommandTimeout = 120;
                    var DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                    dataDashboard = context.Database.SqlQuery<ExecutiveSummary>("sp_LoyaltyPerfromance @pi_GroupId, @pi_DBName,@pi_Date,@pi_LoginId",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_DBName", DBName),
                        new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_LoginId", LoginId)).FirstOrDefault<ExecutiveSummary>();

                    dataDashboard.lstOutletDetails = context.Database.SqlQuery<OutletDetails>("sp_DashboardOutletEnrolCount @pi_GroupId, @pi_DBName,@pi_DateRangeFlag,@pi_FromDate,@pi_ToDate",
                       new SqlParameter("@pi_GroupId", GroupId),
                       new SqlParameter("@pi_DBName", DBName),
                       new SqlParameter("@pi_DateRangeFlag", "0"),
                       new SqlParameter("@pi_FromDate", ""),
                       new SqlParameter("@pi_ToDate", "")).ToList<OutletDetails>();

                    //using (var contextnew = new BOTSDBContext(connstr))
                    //{
                    //    var AllData = GetExecutiveSummaryAllData(GroupId, connstr);
                    //    var outletList = contextnew.tblOutletMasters.ToList();
                    //    List<OutletDetails> lstOutletDetails = new List<OutletDetails>();
                    //    foreach (var item in outletList)
                    //    {
                    //        OutletDetails newItem = new OutletDetails();
                    //        newItem.OutletName = item.OutletName;
                    //        newItem.EnrollmentCount = AllData.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Count();
                    //        lstOutletDetails.Add(newItem);
                    //    }
                    //    dataDashboard.lstOutletDetails = lstOutletDetails;
                    //}
                }
                #region
                //    using (var context = new BOTSDBContext(connstr))
                //    {
                //        context.Database.CommandTimeout = 120;

                //        var AllData = GetExecutiveSummaryAllData(GroupId, connstr);
                //        dataDashboard.TotalBiz = Convert.ToInt64(AllData.Sum(x => x.TotalSpend));
                //        dataDashboard.Redemption = AllData.Sum(x => x.BurnAmtWithPts);
                //        dataDashboard.Referrals = Convert.ToInt64(AllData.Where(x => x.EnrolledBy == "DLCReferral").Sum(y => y.TotalSpend));
                //        dataDashboard.NewMWPRegistration = Convert.ToInt64(AllData.Where(x => x.EnrolledBy == "DLCWalkIn").Sum(y => y.TotalSpend));
                //        dataDashboard.Campaign = 0;
                //        dataDashboard.SMSBlastWA = 0;
                //        dataDashboard.LoyaltyBiz = dataDashboard.Redemption + dataDashboard.Referrals + dataDashboard.NewMWPRegistration + dataDashboard.Campaign + dataDashboard.SMSBlastWA;
                //        var outletList = context.tblOutletMasters.ToList();
                //        List<OutletDetails> lstOutletDetails = new List<OutletDetails>();
                //        foreach (var item in outletList)
                //        {
                //            OutletDetails newItem = new OutletDetails();
                //            newItem.OutletName = item.OutletName;
                //            newItem.EnrollmentCount = AllData.Where(x => x.CurrentEnrolledOutlet == item.OutletId).Count();
                //            lstOutletDetails.Add(newItem);
                //        }
                //        dataDashboard.lstOutletDetails = lstOutletDetails;                        
                //    }

                //using (var context = new CommonDBContext())
                //{
                //    context.Database.CommandTimeout = 120;
                //    var gId = Convert.ToInt32(GroupId);
                //    var livedate = context.tblGroupDetails.Where(x => x.GroupId == gId).Select(y => y.WentLiveDate).FirstOrDefault();
                //    if (livedate.HasValue)
                //    {
                //        var day = livedate.Value.Day;
                //        var month = livedate.Value.Month;
                //        var year = livedate.Value.Year;
                //        var currentYear = DateTime.Today.Year;

                //        DateTime nextRenewal = new DateTime(currentYear, month, day);
                //        DateTime ProgramRenewalDate = new DateTime();
                //        if (nextRenewal < DateTime.Today)
                //        {
                //            ProgramRenewalDate = nextRenewal.AddYears(1);
                //        }
                //        else
                //        {
                //            ProgramRenewalDate = nextRenewal;
                //        }
                //        dataDashboard.RenewalDate = ProgramRenewalDate.ToString("dd-MMM-yyyy");
                //        dataDashboard.RemainingDaysForRenewal = (ProgramRenewalDate - DateTime.Today).Days;
                //    }
                //}

                //using (var context = new CommonDBContext())
                //{
                //    var GrpId = GroupId;
                //    //var RenewDate = context.tblRenewalDatas.Where(x => x.GroupId == GrpId).Select(y => y.PaymentDate).FirstOrDefault();
                //    var RenewDate = (from S in context.tblRenewalDatas where S.GroupId == GrpId && S.PaymentType == "Renewal" orderby S.Id descending select S.NextPaymentDate).FirstOrDefault();
                //    var VerifiedDate = (from S in context.tblRenewalDatas where S.GroupId == GrpId && S.PaymentType == "VerifiedWA" orderby S.Id descending select S.NextPaymentDate).FirstOrDefault();

                //    if (RenewDate == null)
                //    {
                //        dataDashboard.RenewDate = "";
                //    }
                //    else
                //    {
                //        dataDashboard.RenewDate = RenewDate.ToString();
                //    }
                //    if (VerifiedDate == null)
                //    {
                //        dataDashboard.VerifiedWARenewalDate = "";
                //    }
                //    else
                //    {
                //        dataDashboard.VerifiedWARenewalDate = VerifiedDate.ToString();
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardData");
            }
            return dataDashboard;
        }

        public DashboardMemberSegment GetDashboardMemberSegmentData(string GroupId, string OutletId, string connstr, string loginId, string frmDate, string toDate)
        {

            DashboardMemberSegment dashboardMemberSegment = new DashboardMemberSegment();
            try
            {
                string DBName = string.Empty;
                using (var contextnew = new CommonDBContext())
                {
                    contextnew.Database.CommandTimeout = 300;
                    DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                    dashboardMemberSegment = contextnew.Database.SqlQuery<DashboardMemberSegment>("sp_DashboardMemberSegment @pi_GroupId, @pi_Date, @pi_OutletId,@pi_LoginId,@pi_FromDate,@pi_ToDate,@pi_DBName",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_OutletId", OutletId),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_FromDate", frmDate),
                        new SqlParameter("@pi_ToDate", toDate),
                        new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<DashboardMemberSegment>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardMemberSegmentData");
            }
            return dashboardMemberSegment;
        }
        public List<DashboardMemberSegmentTxn> GetDashboardMemberSegmentTxnData(string GroupId, string OutletId, string connstr, string loginId, string frmDate, string toDate)
        {

            List<DashboardMemberSegmentTxn> dashboardMemberSegmentTxn = new List<DashboardMemberSegmentTxn>();
            List<DashboardMemberSegmentTxnDB> dashboardMemberSegmentTxnDB = new List<DashboardMemberSegmentTxnDB>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    context.Database.CommandTimeout = 120;
                    if (GroupId == "1086")
                    {
                        dashboardMemberSegmentTxnDB = context.Database.SqlQuery<DashboardMemberSegmentTxnDB>("sp_BOTS_DashboardMemberSegment1 @pi_GroupId, @pi_Date, @pi_OutletId,@pi_LoginId,@pi_FromDate,@pi_ToDate", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")), new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_FromDate", frmDate), new SqlParameter("@pi_ToDate", toDate)).ToList<DashboardMemberSegmentTxnDB>();
                    }
                    else if (GroupId == "1087")
                    {

                    }
                    else
                    {
                        using (var contextNew = new CommonDBContext())
                        {
                            context.Database.CommandTimeout = 120;
                            var DBName = contextNew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                            dashboardMemberSegmentTxnDB = contextNew.Database.SqlQuery<DashboardMemberSegmentTxnDB>("sp_DashboardKeyPoints @pi_GroupId, @pi_Date, @pi_OutletId,@pi_LoginId,@pi_FromDate,@pi_ToDate,@pi_DBName", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")), new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_FromDate", frmDate), new SqlParameter("@pi_ToDate", toDate), new SqlParameter("@pi_DBName", DBName)).ToList<DashboardMemberSegmentTxnDB>();
                        }
                    }
                    if (GroupId != "1087")
                    {
                        int count = 1;
                        foreach (var item in dashboardMemberSegmentTxnDB)
                        {
                            DashboardMemberSegmentTxn newItem = new DashboardMemberSegmentTxn();
                            if (count == 1)
                            {
                                newItem.Title = "Total Member";                                
                            }
                            if (count == 2)
                            {
                                newItem.Title = "Txn Count";
                            }
                            if (count == 3)
                            {
                                newItem.Title = "Count Per Member";
                            }
                            if (count == 4)
                            {
                                newItem.Title = "Business Vol";
                            }
                            if (count == 5)
                            {
                                newItem.Title = "ATS";
                            }
                            if (count == 6)
                            {
                                newItem.Title = "Spend Per Member";
                            }

                            newItem.TotalBase = item.TotalBase;
                            newItem.RepeatBase = item.RepeatBase;
                            newItem.OnlyOnce = item.OnlyOnce;
                            newItem.NeverRedeem = item.NeverRedeem;
                            newItem.RecentlyEnrolled = item.RecentlyEnrolled;
                            dashboardMemberSegmentTxn.Add(newItem);
                            count++;
                        }
                    }
                    else
                    {
                        var dateToCheck = DateTime.Today.AddDays(-30);
                        var AllData = GetExecutiveSummaryAllData(GroupId, connstr);
                        DashboardMemberSegmentTxn newItem = new DashboardMemberSegmentTxn();
                        newItem.Title = "No of Transactions";
                        newItem.Unit = "Nos";
                        if (string.IsNullOrEmpty(OutletId))
                        {
                            newItem.TotalBase = Convert.ToString(AllData.Sum(x => x.TotalTxnCount));
                            newItem.RepeatBase = Convert.ToString(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck).Sum(x => x.TotalTxnCount));
                            newItem.OnlyOnce = Convert.ToString(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).Sum(x => x.TotalTxnCount));
                            newItem.NeverRedeem = Convert.ToString(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).Sum(x => x.TotalTxnCount));
                            newItem.RecentlyEnrolled = Convert.ToString(AllData.Where(x => x.DOJ > dateToCheck).Sum(x => x.TotalTxnCount));
                        }
                        else
                        {
                            newItem.TotalBase = Convert.ToString(AllData.Where(y => y.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount));
                            newItem.RepeatBase = Convert.ToString(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount));
                            newItem.OnlyOnce = Convert.ToString(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount));
                            newItem.NeverRedeem = Convert.ToString(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount));
                            newItem.RecentlyEnrolled = Convert.ToString(AllData.Where(x => x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount));
                        }
                        dashboardMemberSegmentTxn.Add(newItem);

                        DashboardMemberSegmentTxn newItem1 = new DashboardMemberSegmentTxn();
                        newItem1.Title = "Txns per Member";
                        newItem1.Unit = "Nos";
                        if (string.IsNullOrEmpty(OutletId))
                        {
                            newItem1.TotalBase = Convert.ToString(AllData.Sum(x => x.TotalTxnCount) / AllData.Count());
                            newItem1.RepeatBase = Convert.ToString(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck).Sum(x => x.TotalTxnCount) / AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck).Count());
                            newItem1.OnlyOnce = Convert.ToString(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).Sum(x => x.TotalTxnCount) / AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).Count());
                            newItem1.NeverRedeem = Convert.ToString(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).Sum(x => x.TotalTxnCount) / AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).Count());
                            newItem1.RecentlyEnrolled = Convert.ToString(AllData.Where(x => x.DOJ > dateToCheck).Sum(x => x.TotalTxnCount) / AllData.Where(x => x.DOJ > dateToCheck).Count());
                        }
                        else
                        {
                            newItem1.TotalBase = Convert.ToString(AllData.Where(y => y.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount) / AllData.Where(y => y.CurrentEnrolledOutlet == OutletId).Count());
                            newItem1.RepeatBase = Convert.ToString(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount) / AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count());
                            newItem1.OnlyOnce = Convert.ToString(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount) / AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count());
                            newItem1.NeverRedeem = Convert.ToString(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount) / AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count());
                            newItem1.RecentlyEnrolled = Convert.ToString(AllData.Where(x => x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalTxnCount) / AllData.Where(x => x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count());
                        }
                        dashboardMemberSegmentTxn.Add(newItem1);

                        DashboardMemberSegmentTxn newItem2 = new DashboardMemberSegmentTxn();
                        newItem2.Title = "Avg Days b/w Txns";
                        newItem2.Unit = "Days";
                        if (string.IsNullOrEmpty(OutletId))
                        {
                            var newAllData = AllData.Where(x => x.LastTxnDate != null || x.FirstTxnDate != null).ToList();
                            var daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            var daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.TotalBase = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && x.EarnCount >= 1 && x.BurnCount > 0).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.RepeatBase = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.OnlyOnce = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.NeverRedeem = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && x.DOJ > dateToCheck).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.RecentlyEnrolled = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));
                        }
                        else
                        {
                            var newAllData = AllData.Where(x => x.LastTxnDate != null || x.FirstTxnDate != null && x.CurrentEnrolledOutlet == OutletId).ToList();
                            var daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            var daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.TotalBase = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && x.EarnCount >= 1 && x.BurnCount > 0 && x.CurrentEnrolledOutlet == OutletId).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.RepeatBase = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.OnlyOnce = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.NeverRedeem = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));

                            newAllData = AllData.Where(x => (x.LastTxnDate != null || x.FirstTxnDate != null) && x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).ToList();
                            daysDiff = from items in newAllData select new { days = (items.LastTxnDate.Value - items.FirstTxnDate.Value).TotalDays };
                            daysDiffSum = daysDiff.Sum(x => x.days);
                            newItem2.RecentlyEnrolled = Convert.ToString(Math.Round(daysDiffSum / newAllData.Sum(x => x.TotalTxnCount)));
                        }
                        dashboardMemberSegmentTxn.Add(newItem2);

                        DashboardMemberSegmentTxn newItem3 = new DashboardMemberSegmentTxn();
                        newItem3.Title = "Average Bill Size";
                        newItem3.Unit = "Rs.";

                        if (string.IsNullOrEmpty(OutletId))
                        {
                            newItem3.TotalBase = Convert.ToString(Math.Round(AllData.Sum(x => x.TotalSpend) / AllData.Sum(y => y.TotalTxnCount)));
                            newItem3.RepeatBase = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck).Sum(y => y.TotalTxnCount)));
                            newItem3.OnlyOnce = Convert.ToString(Math.Round(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).Sum(y => y.TotalTxnCount)));
                            newItem3.NeverRedeem = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).Sum(y => y.TotalTxnCount)));
                            newItem3.RecentlyEnrolled = Convert.ToString(Math.Round(AllData.Where(x => x.DOJ > dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => x.DOJ > dateToCheck).Sum(y => y.TotalTxnCount)));
                        }
                        else
                        {
                            newItem3.TotalBase = Convert.ToString(Math.Round(AllData.Where(y => y.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(y => y.CurrentEnrolledOutlet == OutletId).Sum(y => y.TotalTxnCount)));
                            newItem3.RepeatBase = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(y => y.TotalTxnCount)));
                            newItem3.OnlyOnce = Convert.ToString(Math.Round(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(y => y.TotalTxnCount)));
                            newItem3.NeverRedeem = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(y => y.TotalTxnCount)));
                            newItem3.RecentlyEnrolled = Convert.ToString(Math.Round(AllData.Where(x => x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(y => y.TotalTxnCount)));
                        }
                        dashboardMemberSegmentTxn.Add(newItem3);

                        DashboardMemberSegmentTxn newItem4 = new DashboardMemberSegmentTxn();
                        newItem4.Title = "Per Member Spends";
                        newItem4.Unit = "Rs.";
                        if (string.IsNullOrEmpty(OutletId))
                        {
                            newItem4.TotalBase = Convert.ToString(Math.Round(AllData.Sum(x => x.TotalSpend) / AllData.Count()));
                            newItem4.RepeatBase = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck).Count()));
                            newItem4.OnlyOnce = Convert.ToString(Math.Round(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck).Count()));
                            newItem4.NeverRedeem = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck).Count()));
                            newItem4.RecentlyEnrolled = Convert.ToString(Math.Round(AllData.Where(x => x.DOJ > dateToCheck).Sum(x => x.TotalSpend) / AllData.Where(x => x.DOJ > dateToCheck).Count()));
                        }
                        else
                        {
                            newItem4.TotalBase = Convert.ToString(Math.Round(AllData.Where(x => x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Count(y => y.CurrentEnrolledOutlet == OutletId)));
                            newItem4.RepeatBase = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount >= 1 && x.BurnCount > 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count()));
                            newItem4.OnlyOnce = Convert.ToString(Math.Round(AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => ((x.EarnCount == 1 && x.BurnCount == 0) || (x.EarnCount == 0 && x.BurnCount == 1)) && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count()));
                            newItem4.NeverRedeem = Convert.ToString(Math.Round(AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => x.EarnCount > 1 && x.BurnCount == 0 && x.DOJ < dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count()));
                            newItem4.RecentlyEnrolled = Convert.ToString(Math.Round(AllData.Where(x => x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).Sum(x => x.TotalSpend) / AllData.Where(x => x.DOJ > dateToCheck && x.CurrentEnrolledOutlet == OutletId).Count()));

                        }
                        dashboardMemberSegmentTxn.Add(newItem4);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardMemberSegmentTxnData");
            }
            return dashboardMemberSegmentTxn;
        }

        public List<DashboardOutletEnrolment> GetDashboardOutletEnrolmentData(string GroupId, string monthFlag, string connstr, string loginId, string frmDate, string toDate)
        {

            List<DashboardOutletEnrolment> dashboardOutletEnrolment = new List<DashboardOutletEnrolment>();
            try
            {
                using (var contextnew = new CommonDBContext())
                {
                    if (monthFlag == "")
                        monthFlag = "0";
                    
                    contextnew.Database.CommandTimeout = 300;
                    var DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                    dashboardOutletEnrolment = contextnew.Database.SqlQuery<DashboardOutletEnrolment>("sp_DashboardOutletEnrolCount @pi_GroupId, @pi_DBName,@pi_DateRangeFlag,@pi_FromDate,@pi_ToDate",
                       new SqlParameter("@pi_GroupId", GroupId),
                       new SqlParameter("@pi_DBName", DBName),
                       new SqlParameter("@pi_DateRangeFlag", monthFlag),
                       new SqlParameter("@pi_FromDate", ""),
                       new SqlParameter("@pi_ToDate", "")).ToList<DashboardOutletEnrolment>();

                    //dashboardOutletEnrolment = contextnew.Database.SqlQuery<DashboardOutletEnrolment>("sp_DashboardOutletEnrolment @pi_GroupId, @pi_Date, @pi_Flag, @pi_LoginId,@pi_FromDate,@pi_ToDate,@pi_DBName",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                    //    new SqlParameter("@pi_Flag", monthFlag),
                    //    new SqlParameter("@pi_LoginId", loginId),
                    //    new SqlParameter("@pi_FromDate", frmDate),
                    //    new SqlParameter("@pi_ToDate", toDate),
                    //    new SqlParameter("@pi_DBName", DBName)).OrderByDescending(s => s.EnrollmentCount).ToList<DashboardOutletEnrolment>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardOutletEnrolmentData");
            }
            return dashboardOutletEnrolment;
        }

        public DashboardPointsSummary GetDashboardPointsSummaryData(string GroupId, string monthFlag, string connstr, string loginId, string frmDate, string toDate)
        {
            DashboardPointsSummary dashboardPointsSummary = new DashboardPointsSummary();
            try
            {
                if (GroupId != "1341")
                {
                    using (var context = new BOTSDBContext(connstr))
                    {
                        context.Database.CommandTimeout = 300;
                        //if (GroupId == "1087")
                        //{
                        using (var contextnew = new CommonDBContext())
                        {
                            var DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                            //var DBName = "MadhusudanTextiles_New";
                            dashboardPointsSummary = contextnew.Database.SqlQuery<DashboardPointsSummary>("sp_DashboardPointsSummary @pi_GroupId, @pi_Date, @pi_Flag, @pi_LoginId,@pi_FromDate,@pi_ToDate,@pi_DBName", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")), new SqlParameter("@pi_Flag", monthFlag),
                                   new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_FromDate", frmDate), new SqlParameter("@pi_ToDate", toDate), new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<DashboardPointsSummary>();
                        }
                        //}
                        //else
                        //{
                        //    dashboardPointsSummary = context.Database.SqlQuery<DashboardPointsSummary>("sp_BOTS_DashboardPointsSummary @pi_GroupId, @pi_Date, @pi_Flag, @pi_LoginId,@pi_FromDate,@pi_ToDate", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_Flag", monthFlag),
                        //        new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_FromDate", frmDate), new SqlParameter("@pi_ToDate", toDate)).FirstOrDefault<DashboardPointsSummary>();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardPointsSummaryData");
            }
            return dashboardPointsSummary;
        }
        public DashboardMemberWebPage GetDashboardMemberWebPageData(string GroupId, string profileFlag, string connstr, string loginId, string frmDate, string toDate)
        {
            DashboardMemberWebPage dashboardMemberWebPage = new DashboardMemberWebPage();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    context.Database.CommandTimeout = 300;
                    if (GroupId == "1086")
                    {
                        dashboardMemberWebPage = context.Database.SqlQuery<DashboardMemberWebPage>("sp_BOTS_DashboardMemberWebPage @pi_GroupId, @pi_Date, @pi_Flag, @pi_LoginId,@pi_FromDate,@pi_ToDate", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_Flag", profileFlag), new SqlParameter("@pi_LoginId", loginId), new SqlParameter("@pi_FromDate", frmDate), new SqlParameter("@pi_ToDate", toDate)).FirstOrDefault<DashboardMemberWebPage>();
                    }
                    if (GroupId != "1341")
                    {
                        var AllData = GetExecutiveSummaryAllData(GroupId, connstr);
                        dashboardMemberWebPage.MemberBase = AllData.Count();
                        dashboardMemberWebPage.ReferringBase = context.Database.SqlQuery<int>("select distinct count(ReferredByMobileNo) from tblDLCReporting").FirstOrDefault();
                        dashboardMemberWebPage.ReferralGenerated = context.Database.SqlQuery<int>("select distinct count(ReferralMobileNo) from tblDLCReporting").FirstOrDefault();
                        dashboardMemberWebPage.ReferralTransacted = context.tblDLCReportings.Where(x => x.ConvertedStatus.Value == true).Count();
                        dashboardMemberWebPage.ReferralTxnCount = Convert.ToInt64(context.tblDLCReportings.Sum(x => x.ReferralTotalTxnCount));
                        dashboardMemberWebPage.BusinessGenerated = Convert.ToInt64(context.tblDLCReportings.Sum(x => x.ReferralTotalSpend));
                        dashboardMemberWebPage.ProfileUpdatedCount = context.tblDLCProfileUpdatedLists.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardMemberWebPageData");
            }
            return dashboardMemberWebPage;
        }

        public DashboardBulkUpload GetDashboardBulkUpload(string GroupId, string connstr, string loginId, string frmDate, string toDate)
        {
            DashboardBulkUpload objDashboardBulkUpload = new DashboardBulkUpload();
            try
            {
                string DBName = string.Empty;
                using (var contextnew = new CommonDBContext())
                {
                    contextnew.Database.CommandTimeout = 300;
                    DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                    objDashboardBulkUpload = contextnew.Database.SqlQuery<DashboardBulkUpload>("sp_DashboardBulkUpload @pi_GroupId, @pi_Date, @pi_LoginId,@pi_FromDate,@pi_ToDate,@pi_DBName",
                            new SqlParameter("@pi_GroupId", GroupId),
                            new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                            new SqlParameter("@pi_LoginId", loginId),
                            new SqlParameter("@pi_FromDate", frmDate),
                            new SqlParameter("@pi_ToDate", toDate),
                            new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<DashboardBulkUpload>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardBulkUpload");
            }

            return objDashboardBulkUpload;
        }

        public DashboardRedemption GetDashboardRedemption(string GroupId, string Type, string connstr, string loginId, string frmDate, string toDate)
        {
            DashboardRedemption objDashboardRedemption = new DashboardRedemption();
            try
            {
                if (GroupId != "1341")
                {
                    using (var context = new BOTSDBContext(connstr))
                    {
                        context.Database.CommandTimeout = 300;

                        using (var contextNew = new CommonDBContext())
                        {
                            contextNew.Database.CommandTimeout = 120;
                            var DBName = contextNew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                            objDashboardRedemption = contextNew.Database.SqlQuery<DashboardRedemption>("sp_DashboardRedemption @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Type,@pi_FromDate,@pi_ToDate,@pi_DBName", new SqlParameter("@pi_GroupId", GroupId),
                                new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                                new SqlParameter("@pi_LoginId", loginId),
                                new SqlParameter("@pi_Type", Type),
                                new SqlParameter("@pi_FromDate", frmDate),
                                new SqlParameter("@pi_ToDate", toDate),
                                 new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<DashboardRedemption>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardRedemption");
            }
            return objDashboardRedemption;
        }

        public List<DashboardBizShared> GetDashboardBizShared(string GroupId, string OutletId, string connstr, string loginId, string frmDate, string toDate)
        {
            List<DashboardBizShared> lstBizShared = new List<DashboardBizShared>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    context.Database.CommandTimeout = 300;
                    if (GroupId == "1086")
                    {
                        lstBizShared = context.Database.SqlQuery<DashboardBizShared>("sp_BOTS_DashboardBizShared @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId,@pi_FromDate,@pi_ToDate",
                           new SqlParameter("@pi_GroupId", GroupId),
                           new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                           new SqlParameter("@pi_LoginId", loginId),
                           new SqlParameter("@pi_OutletId", OutletId),
                           new SqlParameter("@pi_FromDate", frmDate),
                           new SqlParameter("@pi_ToDate", toDate)).ToList<DashboardBizShared>();
                    }
                }
                using (var contextnew = new CommonDBContext())
                {
                    contextnew.Database.CommandTimeout = 300;
                    var DBName = contextnew.GroupDetail.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                    lstBizShared = contextnew.Database.SqlQuery<DashboardBizShared>("sp_DashboardBizShared @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId,@pi_FromDate,@pi_ToDate,@pi_DBName",
                   new SqlParameter("@pi_GroupId", GroupId),
                   new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                   new SqlParameter("@pi_LoginId", loginId),
                   new SqlParameter("@pi_OutletId", OutletId),
                   new SqlParameter("@pi_FromDate", frmDate),
                   new SqlParameter("@pi_ToDate", toDate),
                   new SqlParameter("@pi_DBName", DBName)).ToList<DashboardBizShared>();
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardBizShared");
            }
            return lstBizShared;
        }

        public bool UpdatePassword(string LoginId, string password)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    CustomerLoginDetail userDetail = new CustomerLoginDetail();
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId).FirstOrDefault();
                    userDetail.Password = password;
                    context.CustomerLoginDetails.AddOrUpdate(userDetail);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdatePassword");
            }

            return status;
        }

        public bool InsertOTP(string emailId, int OTP)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    OTPDetail objOTPDetail = new OTPDetail();
                    objOTPDetail.EmailId = emailId;
                    objOTPDetail.OTP = OTP;
                    objOTPDetail.SentDate = DateTime.Now;

                    context.OTPDetails.Add(objOTPDetail);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "InsertOTP");
            }
            return status;
        }

        public bool VerifyOTP(string emailId, int OTP)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    OTPDetail objOTPDetail = new OTPDetail();
                    objOTPDetail = context.OTPDetails.Where(x => x.EmailId == emailId && x.OTP == OTP).OrderByDescending(y => y.SentDate).FirstOrDefault();
                    if (objOTPDetail != null)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "VerifyOTP");
            }
            return status;
        }

        public bool ResetPassword(string emailId, string password)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    CustomerLoginDetail userDetail = new CustomerLoginDetail();
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == emailId).FirstOrDefault();
                    userDetail.Password = password;
                    context.CustomerLoginDetails.AddOrUpdate(userDetail);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ResetPassword");
            }
            return status;
        }

        public bool SendOTPMessage(string MobileNo, string Sender, string MobileMessage, string Url)
        {
            bool status = false;
            try
            {
                var UserName = System.Configuration.ConfigurationManager.AppSettings["SMSUserID"];
                var Password = System.Configuration.ConfigurationManager.AppSettings["SMSPassword"];

                MobileMessage = HttpUtility.UrlEncode(MobileMessage);
                string type1 = "TEXT";
                StringBuilder sbposdata1 = new StringBuilder();
                sbposdata1.AppendFormat("username={0}", UserName);
                sbposdata1.AppendFormat("&password={0}", Password);
                sbposdata1.AppendFormat("&to={0}", MobileNo);
                sbposdata1.AppendFormat("&from={0}", Sender);
                sbposdata1.AppendFormat("&text={0}", MobileMessage);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(Url);
                UTF8Encoding encoding1 = new UTF8Encoding();
                byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
                httpWReq1.Method = "POST";
                httpWReq1.ContentType = "application/x-www-form-urlencoded";
                httpWReq1.ContentLength = data1.Length;
                using (Stream stream1 = httpWReq1.GetRequestStream())
                {
                    stream1.Write(data1, 0, data1.Length);
                }
                HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
                StreamReader reader1 = new StreamReader(response1.GetResponseStream());
                string responseString1 = reader1.ReadToEnd();
                reader1.Close();
                response1.Close();
                return status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendOTPMessage");
            }
            return status;
        }
    }
}
