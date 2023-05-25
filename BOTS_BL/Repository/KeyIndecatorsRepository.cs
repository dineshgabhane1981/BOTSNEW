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
using BOTS_BL.Models.IndividualDBModels;

namespace BOTS_BL.Repository
{
    public class KeyIndecatorsRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        DashboardRepository DR = new DashboardRepository();
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        public OnlyOnce GetOnlyOnceData(string GroupId, string outletId, string connstr, string loginId)
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
                    else if (GroupId == "1087")
                    {
                        var AllData = DR.GetExecutiveSummaryAllData(GroupId, connstr);
                        if (outletId == "")
                        {
                            objOnlyOnce.TotalMember = AllData.Count();
                            objOnlyOnce.OnlyOnceMember = AllData.Where(x => x.TotalTxnCount == 1).Count();
                            objOnlyOnce.OnlyOncePercentage = Math.Round((Convert.ToDecimal(objOnlyOnce.OnlyOnceMember) * 100) / Convert.ToDecimal(objOnlyOnce.TotalMember), 2);

                            var DBTotalSpend = AllData.Sum(x => x.TotalSpend);
                            var AvgAmt = DBTotalSpend / objOnlyOnce.OnlyOnceMember;
                            var date150 = DateTime.Today.AddDays(-150);
                            objOnlyOnce.RecentVisitHigh = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend > AvgAmt && x.LastTxnDate >= date150).Count();
                            objOnlyOnce.RecentVisitLow = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend <= AvgAmt && x.LastTxnDate >= date150).Count();
                            objOnlyOnce.NotSeenHigh = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend > AvgAmt && x.LastTxnDate < date150).Count();
                            objOnlyOnce.NotSeenLow = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend <= AvgAmt && x.LastTxnDate < date150).Count();
                        }
                        else
                        {
                            objOnlyOnce.TotalMember = AllData.Where(x => x.CurrentEnrolledOutlet == outletId).Count();
                            objOnlyOnce.OnlyOnceMember = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId).Count();
                            objOnlyOnce.OnlyOncePercentage = Math.Round((Convert.ToDecimal(objOnlyOnce.OnlyOnceMember) * 100) / Convert.ToDecimal(objOnlyOnce.TotalMember), 2);

                            var DBTotalSpend = AllData.Where(x => x.CurrentEnrolledOutlet == outletId).Sum(x => x.TotalSpend);
                            var AvgAmt = DBTotalSpend / objOnlyOnce.OnlyOnceMember;
                            var date150 = DateTime.Today.AddDays(-150);
                            objOnlyOnce.RecentVisitHigh = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend > AvgAmt && x.LastTxnDate >= date150).Count();
                            objOnlyOnce.RecentVisitLow = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend <= AvgAmt && x.LastTxnDate >= date150).Count();
                            objOnlyOnce.NotSeenHigh = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend > AvgAmt && x.LastTxnDate < date150).Count();
                            objOnlyOnce.NotSeenLow = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend <= AvgAmt && x.LastTxnDate < date150).Count();
                        }
                    }
                    else
                    {
                        objOnlyOnce = context.Database.SqlQuery<OnlyOnce>("sp_BOTS_OnlyOnce @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<OnlyOnce>();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOnlyOnceData");
            }

            return objOnlyOnce;
        }
        public List<OnlyOnceTxn> GetOnlyOnceTxnData(string GroupId, string outletId, string type, string connstr, string loginId)
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
                    else if (GroupId == "1087")
                    {
                        List<MemberListAllData> AllData = new List<MemberListAllData>();

                        AllData = context.Database.SqlQuery<MemberListAllData>("select * from View_CustTxnSummaryWithPts").ToList();

                        var onlyOnceData = AllData.Where(x => x.TotalTxnCount == 1).ToList();
                        var DBTotalSpend = AllData.Sum(x => x.TotalSpend);
                        var AvgAmt = DBTotalSpend / onlyOnceData.Count();
                        var date150 = DateTime.Today.AddDays(-150);
                        List<MemberListAllData> highRecentData = new List<MemberListAllData>();
                        if (type == "1")
                        {
                            if (outletId == "")
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend > AvgAmt && x.LasTTxnDate >= date150).ToList();
                            }
                            else
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend > AvgAmt && x.LasTTxnDate >= date150).ToList();
                            }
                        }
                        if (type == "2")
                        {
                            if (outletId == "")
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend <= AvgAmt && x.LasTTxnDate >= date150).ToList();
                            }
                            else
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend <= AvgAmt && x.LasTTxnDate >= date150).ToList();
                            }
                        }
                        if (type == "3")
                        {
                            if (outletId == "")
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend > AvgAmt && x.LasTTxnDate < date150).ToList();
                            }
                            else
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend > AvgAmt && x.LasTTxnDate < date150).ToList();
                            }
                        }
                        if (type == "4")
                        {
                            if (outletId == "")
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.TotalSpend <= AvgAmt && x.LasTTxnDate < date150).ToList();
                            }
                            else
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId && x.TotalSpend <= AvgAmt && x.LasTTxnDate < date150).ToList();
                            }
                        }
                        if (type == "5")
                        {
                            if (outletId == "")
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1).ToList();
                            }
                            else
                            {
                                highRecentData = AllData.Where(x => x.TotalTxnCount == 1 && x.CurrentEnrolledOutlet == outletId).ToList();
                            }
                        }
                        foreach (var item in highRecentData)
                        {
                            OnlyOnceTxn newItem = new OnlyOnceTxn();
                            newItem.EnrolledOutlet = item.OutletName;
                            newItem.MobileNo = item.MobileNo;
                            newItem.MaskedMobileNo = item.MaskedMobileNo;
                            newItem.MemberName = item.Name;
                            newItem.Type = item.Type;
                            newItem.TotalSpend = Convert.ToInt64(item.TotalSpend);
                            newItem.TotalVisit = item.TotalTxnCount;
                            newItem.AvlBalPoints = item.AvlPts;
                            newItem.LastTxnDate = Convert.ToString(item.LasTTxnDate);

                            objOnlyOnceTxn.Add(newItem);
                        }
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
                newexception.AddException(ex, "GetOnlyOnceTxnData");
            }

            return objOnlyOnceTxn;
        }

        public NonTransactingCls GetNonTransactingData(string GroupId, string outletId, string connstr, string loginId)
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
                    else if (GroupId == "1087")
                    {
                        List<MemberListAllData> AllData = new List<MemberListAllData>();

                        AllData = context.Database.SqlQuery<MemberListAllData>("select * from View_CustTxnSummaryWithPts").ToList();
                        if (outletId == "")
                        {
                            objNonTransacting._Within30Days = AllData.Where(x => x.LasTTxnDate >= DateTime.Today.AddDays(-30)).Count();
                            objNonTransacting._31to60Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-30) && x.LasTTxnDate >= DateTime.Today.AddDays(-60)).Count();
                            objNonTransacting._61to90Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-61) && x.LasTTxnDate >= DateTime.Today.AddDays(-90)).Count();
                            objNonTransacting._91to180Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-91) && x.LasTTxnDate >= DateTime.Today.AddDays(-180)).Count();
                            objNonTransacting._181to365Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-181) && x.LasTTxnDate >= DateTime.Today.AddDays(-365)).Count();
                            objNonTransacting._MoreThanYear = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-365)).Count();
                        }
                        else
                        {
                            objNonTransacting._Within30Days = AllData.Where(x => x.LasTTxnDate >= DateTime.Today.AddDays(-30) && x.CurrentEnrolledOutlet == outletId).Count();
                            objNonTransacting._31to60Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-30) && x.LasTTxnDate >= DateTime.Today.AddDays(-60) && x.CurrentEnrolledOutlet == outletId).Count();
                            objNonTransacting._61to90Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-61) && x.LasTTxnDate >= DateTime.Today.AddDays(-90) && x.CurrentEnrolledOutlet == outletId).Count();
                            objNonTransacting._91to180Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-91) && x.LasTTxnDate >= DateTime.Today.AddDays(-180) && x.CurrentEnrolledOutlet == outletId).Count();
                            objNonTransacting._181to365Days = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-181) && x.LasTTxnDate >= DateTime.Today.AddDays(-365) && x.CurrentEnrolledOutlet == outletId).Count();
                            objNonTransacting._MoreThanYear = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-365) && x.CurrentEnrolledOutlet == outletId).Count();
                        }
                    }
                    else
                    {
                        objNonTransacting = context.Database.SqlQuery<NonTransactingCls>("sp_BOTS_NonTransacting @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).FirstOrDefault<NonTransactingCls>();
                    }


                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNonTransactingData");
            }

            return objNonTransacting;
        }

        public List<NonTransactingTxn> GetNonTransactingTxnData(string GroupId, string outletId, string type, string connstr, string loginId)
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
                    else if (GroupId == "1087")
                    {
                        List<MemberListAllData> AllData = new List<MemberListAllData>();

                        AllData = context.Database.SqlQuery<MemberListAllData>("select * from View_CustTxnSummaryWithPts").ToList();
                        if (outletId != "")
                        {
                            if (type == "1")
                                AllData = AllData.Where(x => x.LasTTxnDate >= DateTime.Today.AddDays(-30) && x.CurrentEnrolledOutlet == outletId).ToList();
                            if (type == "2")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-30) && x.LasTTxnDate >= DateTime.Today.AddDays(-60) && x.CurrentEnrolledOutlet == outletId).ToList();
                            if (type == "3")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-61) && x.LasTTxnDate >= DateTime.Today.AddDays(-90) && x.CurrentEnrolledOutlet == outletId).ToList();
                            if (type == "4")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-91) && x.LasTTxnDate >= DateTime.Today.AddDays(-180) && x.CurrentEnrolledOutlet == outletId).ToList();
                            if (type == "5")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-181) && x.LasTTxnDate >= DateTime.Today.AddDays(-365) && x.CurrentEnrolledOutlet == outletId).ToList();
                            if (type == "6")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-365) && x.CurrentEnrolledOutlet == outletId).ToList();
                        }
                        else
                        {
                            if (type == "1")
                                AllData = AllData.Where(x => x.LasTTxnDate >= DateTime.Today.AddDays(-30)).ToList();
                            if (type == "2")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-30) && x.LasTTxnDate >= DateTime.Today.AddDays(-60)).ToList();
                            if (type == "3")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-61) && x.LasTTxnDate >= DateTime.Today.AddDays(-90)).ToList();
                            if (type == "4")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-91) && x.LasTTxnDate >= DateTime.Today.AddDays(-180)).ToList();
                            if (type == "5")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-181) && x.LasTTxnDate >= DateTime.Today.AddDays(-365)).ToList();
                            if (type == "6")
                                AllData = AllData.Where(x => x.LasTTxnDate < DateTime.Today.AddDays(-365)).ToList();
                        }

                        foreach (var item in AllData)
                        {
                            NonTransactingTxn objItem = new NonTransactingTxn();
                            objItem.EnrolledOutlet = item.OutletName;
                            objItem.MobileNo = item.MobileNo;
                            objItem.MaskedMobileNo = item.MaskedMobileNo;
                            objItem.MemberName = item.Name;
                            objItem.Type = item.Type;
                            objItem.TotalSpend = Convert.ToInt64(item.TotalSpend);
                            objItem.TotalVisit = item.TotalTxnCount;
                            objItem.AvlBalPoints = item.AvlPts;
                            objItem.LastTxnDate = item.LasTTxnDate.HasValue ? item.LasTTxnDate.Value.ToString("MM/dd/yyyy") : "";
                            objNonTransactingTxn.Add(objItem);
                        }
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
                newexception.AddException(ex, "GetNonTransactingTxnData");
            }

            return objNonTransactingTxn;
        }

        public NonRedemptionCls GetNonRedemptionData(string GroupId, string connstr, string loginId)
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
                    if (GroupId == "1087")
                    {
                        List<MemberListAllData> AllData = new List<MemberListAllData>();

                        AllData = context.Database.SqlQuery<MemberListAllData>("select * from View_CustTxnSummaryWithPts").ToList();
                        objNonRedemption.TotalMember = AllData.Where(x => x.EarnCount > 0).Count();
                        objNonRedemption.UniqueRedeemedMember = AllData.Where(x => x.BurnCount > 0).Count();
                        objNonRedemption.NeverRedeemed = AllData.Where(x => x.BurnCount == 0 && x.EarnCount > 0).Count();
                        objNonRedemption.NeverRedeemedPercentage = (objNonRedemption.NeverRedeemed * 100) / objNonRedemption.TotalMember;

                        var TotalPoints = AllData.Where(x => x.BurnCount > 0).Sum(y => y.AvlPts);
                        var avg = TotalPoints / objNonRedemption.NeverRedeemed;
                        var Low = avg / 3;
                        var Medium = Low + Low;
                        var High = avg;
                        var last90 = DateTime.Today.AddDays(-90);
                        var last180 = DateTime.Today.AddDays(-180);

                        objNonRedemption.LessThan90DaysLow = AllData.Where(x => x.AvlPts < Low && x.DOJ >= last90 && x.BurnCount == 0 && x.EarnCount > 0).Count();
                        objNonRedemption.LessThan90DaysMedium = AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ >= last90 && x.BurnCount == 0 && x.EarnCount > 0).Count();
                        objNonRedemption.LessThan90DaysHigh = AllData.Where(x => x.AvlPts >= Medium && x.DOJ >= last90 && x.BurnCount == 0 && x.EarnCount > 0).Count();

                        objNonRedemption.Bt90to180Low = AllData.Where(x => x.AvlPts < Low && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).Count();
                        objNonRedemption.Bt90to180Medium = AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).Count();
                        objNonRedemption.Bt90to180High = AllData.Where(x => x.AvlPts >= Medium && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).Count();

                        objNonRedemption.MoreThan180DaysLow = AllData.Where(x => x.AvlPts < Low && x.DOJ < last180 && x.BurnCount == 0 && x.EarnCount > 0).Count();
                        objNonRedemption.MoreThan180DaysMedium = AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ < last180 && x.BurnCount == 0 && x.EarnCount > 0).Count();
                        objNonRedemption.MoreThan180DaysHigh = AllData.Where(x => x.AvlPts >= Medium && x.DOJ < last180 && x.BurnCount == 0 && x.EarnCount > 0).Count();
                    }
                    else
                    {
                        objNonRedemption = context.Database.SqlQuery<NonRedemptionCls>("sp_BOTS_NonRedeeming @pi_GroupId, @pi_Date, @pi_LoginId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", "")).FirstOrDefault<NonRedemptionCls>();
                    }


                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNonRedemptionData");
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
                    if (GroupId == "1087")
                    {
                        List<MemberListAllData> AllData = new List<MemberListAllData>();
                        List<MemberListAllData> FilteredData = new List<MemberListAllData>();

                        AllData = context.Database.SqlQuery<MemberListAllData>("select * from View_CustTxnSummaryWithPts").ToList();
                        var TotalPoints = AllData.Where(x => x.BurnCount > 0).Sum(y => y.AvlPts);
                        var NeverRedeemed = AllData.Where(x => x.BurnCount == 0 && x.EarnCount > 0).Count();
                        var avg = TotalPoints / NeverRedeemed;
                        var Low = avg / 3;
                        var Medium = Low + Low;
                        var High = avg;
                        var last90 = DateTime.Today.AddDays(-90);
                        var last180 = DateTime.Today.AddDays(-180);

                        if (type == 1)
                        {
                            if (daysType == 1)
                            {
                                FilteredData = AllData.Where(x => x.AvlPts >= Medium && x.DOJ >= last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                            if (daysType == 2)
                            {
                                FilteredData= AllData.Where(x => x.AvlPts >= Medium && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                            if (daysType == 3)
                            {
                                FilteredData = AllData.Where(x => x.AvlPts >= Medium && x.DOJ < last180 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                        }
                        if (type == 2)
                        {
                            if (daysType == 1)
                            {
                                FilteredData= AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ >= last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                            if (daysType == 2)
                            {
                                FilteredData= AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                            if (daysType == 3)
                            {
                                FilteredData= AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ < last180 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                        }
                        if (type == 3)
                        {
                            if (daysType == 1)
                            {
                                FilteredData = AllData.Where(x => x.AvlPts < Low && x.DOJ >= last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                            if (daysType == 2)
                            {
                                FilteredData = AllData.Where(x => x.AvlPts < Low && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                            if (daysType == 3)
                            {
                                FilteredData = AllData.Where(x => x.AvlPts < Low && x.DOJ < last180 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                            }
                        }
                        if (type == 4)
                        {
                            FilteredData = AllData.Where(x => x.BurnCount == 0 && x.EarnCount > 0).ToList();
                        }

                        foreach(var item in FilteredData)
                        {
                            NonRedemptionTxn objItem = new NonRedemptionTxn();
                            objItem.EnrolledOutlet = item.OutletName;
                            objItem.MobileNo = item.MobileNo;
                            objItem.MaskedMobileNo = item.MaskedMobileNo;
                            objItem.MemberName = item.Name;
                            objItem.Type = item.Type;
                            objItem.TotalSpend = Convert.ToInt64(item.TotalSpend);
                            objItem.TotalVisit = item.TotalTxnCount;
                            objItem.AvlBalPoints = item.AvlPts;
                            objItem.LastTxnDate = item.LasTTxnDate.HasValue ? item.LasTTxnDate.Value.ToString("MM/dd/yyyy") : "";
                            objNonRedemptionTxn.Add(objItem);
                        }


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
                newexception.AddException(ex, "GetNonRedemptionTxnData");
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
                newexception.AddException(ex, "GetNewRegistrationData");
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
                newexception.AddException(ex, "GetMemberWebPageData");
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
                newexception.AddException(ex, "GetMemberPageData");
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
                newexception.AddException(ex, "GetMemberPageRefData");
            }

            return objMemberPageRefData;
        }

        public MembersInformation GetMemberMisinformationData(string GroupId, string connstr, string loginId)
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
                newexception.AddException(ex, "GetMemberMisinformationData");
            }
            return objMembersInformation;
        }

        public List<DLCCreation> GetDLCCreationData(string GroupId)
        {
            List<DLCCreation> lstData = new List<DLCCreation>();
            var connectionString = CR.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    lstData = context.DLCCreations.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCCreationData");
            }
            return lstData;
        }
        public List<SelectListItem> GetMemberType()
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberType");
            }

            return countriesItem;
        }
        public List<SelectListItem> GetPointsExpiryType()
        {
            List<SelectListItem> countriesItem = new List<SelectListItem>();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsExpiryType");
            }

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
                newexception.AddException(ex, "UpdateDLCCreation");
            }

            return status;
        }

        public DLCCreation GetDLCForEdit(long SlNo, string GroupId)
        {
            DLCCreation objData = new DLCCreation();
            var connStr = CR.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.DLCCreations.Where(x => x.SlNo == SlNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCForEdit");
            }
            return objData;
        }

        public int GetDLCRecordCount(string GroupId)
        {
            int count = 0;
            var connStr = CR.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    count = context.DLCCreations.Count();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCRecordCount");
            }
            return count;
        }

    }
}
