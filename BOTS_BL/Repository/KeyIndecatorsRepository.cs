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
        public OnlyOnce GetOnlyOnceData(string GroupId, string outletId, string connstr, string loginId)
        {
            OnlyOnce objOnlyOnce = new OnlyOnce();
            try
            {
                string DBName = string.Empty;
                using (var contextnew = new CommonDBContext())
                {
                    DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                    objOnlyOnce = contextnew.Database.SqlQuery<OnlyOnce>("sp_OnlyOnceNew @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_DBName",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_OutletId", outletId),
                        new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<OnlyOnce>();
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
                using (var contextnew = new CommonDBContext())
                {
                    var DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();

                    objOnlyOnceTxn = contextnew.Database.SqlQuery<OnlyOnceTxn>("sp_OnlyOnceNew1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId, @pi_Type, @pi_DBName",
                  new SqlParameter("@pi_GroupId", GroupId),
                  new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                  new SqlParameter("@pi_LoginId", loginId),
                  new SqlParameter("@pi_OutletId", outletId),
                  new SqlParameter("@pi_Type", type),
                  new SqlParameter("@pi_DBName", DBName)).ToList<OnlyOnceTxn>();
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
                using (var contextnew = new CommonDBContext())
                {
                    var DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                    objNonTransacting = contextnew.Database.SqlQuery<NonTransactingCls>("sp_NonTransacting @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId,@pi_DBName",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_OutletId", outletId),
                        new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<NonTransactingCls>();
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
                using (var contextnew = new CommonDBContext())
                {
                    var DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                    objNonTransactingTxn = contextnew.Database.SqlQuery<NonTransactingTxn>("sp_NonTransacting1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId,@pi_DBName, @pi_Type",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_LoginId", loginId),
                        new SqlParameter("@pi_OutletId", outletId),
                        new SqlParameter("@pi_DBName", DBName),
                        new SqlParameter("@pi_Type", type)).ToList<NonTransactingTxn>();
                    objNonTransactingTxn = objNonTransactingTxn.OrderBy(x => x.LastTxnDate).ToList();
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNonRedemptionData");
            }

            return objNonRedemption;
        }
        /// <summary>
        /// Added by Omkar on 13th June 2024
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public NonRedemptionCls GetNonRedemptionDataBySP(string GroupId, string loginId)
        {
            NonRedemptionCls objNonRedemption = new NonRedemptionCls();
            try
            {
                using (var contextnew = new CommonDBContext())
                {
                    List<MemberListAllData> AllData = new List<MemberListAllData>();
                    var DBName = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                    
                    var SPData = contextnew.Database.SqlQuery<object>("sp_NonRedeemtion @pi_GroupId,  @pi_LoginId, @pi_DBName",
                            new SqlParameter("@pi_GroupId", GroupId),
                            new SqlParameter("@pi_LoginId", loginId),                          
                            new SqlParameter("@pi_DBName", DBName)).ToList();

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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNonRedemptionDataBySP");
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
                            FilteredData = AllData.Where(x => x.AvlPts >= Medium && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
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
                            FilteredData = AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ >= last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                        }
                        if (daysType == 2)
                        {
                            FilteredData = AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ >= last180 && x.DOJ < last90 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
                        }
                        if (daysType == 3)
                        {
                            FilteredData = AllData.Where(x => x.AvlPts >= Low && x.AvlPts < Medium && x.DOJ < last180 && x.BurnCount == 0 && x.EarnCount > 0).ToList();
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

                    foreach (var item in FilteredData)
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


                    //}
                    //else
                    //{
                    //    objNonRedemptionTxn = context.Database.SqlQuery<NonRedemptionTxn>("sp_BOTS_NonRedeeming1 @pi_GroupId, @pi_Date, @pi_LoginId,@pi_Type,@pi_DaysType",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    //    new SqlParameter("@pi_LoginId", ""),
                    //    new SqlParameter("@pi_Type", type),
                    //    new SqlParameter("@pi_DaysType", daysType)).ToList<NonRedemptionTxn>();
                    //}

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
                    var AllData = DR.GetExecutiveSummaryAllData(GroupId, connstr);
                    var ReferralBase = context.Database.SqlQuery<tblDLCReportingData>("select ReferredByMobileNo,ReferredByName,ReferredDate,ReferralMobileNo,ReferralName,ConvertedStatus,ReferralTotalTxnCount,ReferralTotalSpend from tblDLCReporting").ToList();

                    objMemberPage.ReferringBase = ReferralBase.Select(x => x.ReferredByMobileNo).Distinct().Count();
                    var TotalBase = AllData.Select(x => x.MobileNo).Count();
                    objMemberPage.ReferringBasePercentage = ((objMemberPage.ReferringBase / TotalBase) * 100);
                    objMemberPage.RefGen = ReferralBase.Select(x => x.ReferralMobileNo).Distinct().Count();
                    var RefCoverted = ReferralBase.Where(x => x.ConvertedStatus == true).Count();

                    if (objMemberPage.RefGen > 0 && RefCoverted > 0)
                    {
                        objMemberPage.RefGenConPercentage = ((RefCoverted / objMemberPage.RefGen) * 100);
                    }
                    else
                    {
                        objMemberPage.RefGenConPercentage = 0;
                    }
                    objMemberPage.Con = RefCoverted;
                    objMemberPage.NewRegistration = AllData.Where(y => y.EnrolledBy == "DLCWalkIn" || y.EnrolledBy == "DLCScoialMedia").Select(x => x.MobileNo).Count();
                    //objMemberPage.NewRegistration = NewRegCount;
                    objMemberPage.RefBiz = Convert.ToInt64(AllData.Where(x => x.EnrolledBy == "DLCReferral").Select(y => y.TotalSpend).Sum());
                    objMemberPage.NewRegistrationBiz = Convert.ToInt64(AllData.Where(y => y.EnrolledBy == "DLCWalkIn" || y.EnrolledBy == "DLCScoialMedia").Select(x => x.TotalSpend).Sum());
                    //objMemberPage.ProfileUpdateCount = context.tblProfileUpdateMasters.Select(x => x.MobileNo).Distinct().Count();
                    objMemberPage.ProfileUpdateCount = context.tblDLCProfileUpdatedLists.Select(x => x.MobileNo).Distinct().Count();
                    if (objMemberPage.ProfileUpdateCount > 0)
                    {
                        objMemberPage.ProfileUpdatePercentage = ((objMemberPage.ProfileUpdateCount / TotalBase) * 100);
                    }
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
                    //if(GroupId == "1087")
                    //{
                    if (type == "1")
                    {
                        var ListReferringBase = context.Database.SqlQuery<ListReferringBase>("select D.ReferredByMobileNo,Min(D.ReferredByName) as ReferredByName,Count(D.ReferredByMobileNo) as ReferralsGenerated,sum(D.ReferralTotalTxnCount) as ReferralTotalTxnCount,sum(D.ReferralTotalSpend) as ReferralTotalSpend from tblDLCReporting D Group by D.ReferredByMobileNo");

                        foreach (var item in ListReferringBase)
                        {
                            MemberPageRefData Obj = new MemberPageRefData();
                            Obj.MobileNo = item.ReferredByMobileNo;
                            Obj.MemberName = item.ReferredByName;
                            Obj.ReferralGenerated = Convert.ToInt64(item.ReferralsGenerated);
                            Obj.ReferralTransacted = Convert.ToInt64(item.ReferralTotalTxnCount);
                            Obj.BusinessGenerated = Convert.ToInt64(item.ReferralTotalSpend);
                            objMemberPageRefData.Add(Obj);
                        }

                    }
                    else if (type == "2")
                    {

                    }
                    else
                    {

                    }

                    //}
                    //else
                    //{
                    //    objMemberPageRefData = context.Database.SqlQuery<MemberPageRefData>("sp_BOTS_MemberWebPage2 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Type",
                    //    new SqlParameter("@pi_GroupId", GroupId),
                    //    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    //    new SqlParameter("@pi_LoginId", ""),
                    //    new SqlParameter("@pi_Type", type)).ToList<MemberPageRefData>();
                    //}

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
            List<CustInformatonData> LstCustInfo = new List<CustInformatonData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    DateTime DummyDate = new DateTime(1900, 1, 1);
                    DateTime Today = DateTime.Now;
                    var AllData = DR.GetExecutiveSummaryAllData(GroupId, connstr);
                    objMembersInformation.TotalMemberCount = AllData.Count();
                    objMembersInformation.NameCount = AllData.Where(x => x.Name != "Member" && x.Name.Length <= 3).Count();
                    objMembersInformation.NameCount_Percentage = ((objMembersInformation.NameCount * 100) / objMembersInformation.TotalMemberCount);
                    var CustAllData = context.Database.SqlQuery<CustInformatonData>("select C.MobileNo,C.DOB,C.AnniversaryDate,C.Gender,C.DOJ from View_CustDetailsMaster C").ToList();

                    objMembersInformation.GenderCount = CustAllData.Where(x => x.Gender == "M" || x.Gender == "F" || x.Gender == "O").Count();
                    objMembersInformation.GenderCount_Percentage = ((objMembersInformation.GenderCount * 100) / objMembersInformation.TotalMemberCount);
                    var LstDOB = CustAllData.Where(x => x.DOB.HasValue).ToList();
                    var LstDOA = CustAllData.Where(x => x.AnniversaryDate.HasValue).ToList();
                    objMembersInformation.BirthDateCount = LstDOB.Where(x => x.DOB.Value != DummyDate).Count();
                    objMembersInformation.BirthDateCount_Percentage = ((objMembersInformation.BirthDateCount * 100) / objMembersInformation.TotalMemberCount);
                    objMembersInformation.AnniversaryDateCount = LstDOA.Where(x => x.AnniversaryDate.Value != DummyDate).Count();
                    objMembersInformation.AnniversaryDateCount_Percentage = ((objMembersInformation.AnniversaryDateCount * 100) / objMembersInformation.TotalMemberCount);
                    objMembersInformation.MaritalStatusCount = objMembersInformation.AnniversaryDateCount;
                    objMembersInformation.MaritalStatusCount_Percentage = objMembersInformation.AnniversaryDateCount_Percentage;
                    objMembersInformation.GenderSplitCount_Female = CustAllData.Where(x => x.Gender == "F").Count();
                    //objMembersInformation.GenderSplitCount_Female = 150;
                    objMembersInformation.GenderSplitCount_Male = CustAllData.Where(x => x.Gender == "M").Count();
                    objMembersInformation.GenderSplitCount_Others = CustAllData.Where(x => x.Gender == "O").Count();
                    //objMembersInformation.GenderSplitCount_Others = 100;
                    objMembersInformation.GenderSplitCount_Null = (objMembersInformation.TotalMemberCount - objMembersInformation.GenderCount);
                    objMembersInformation.MaritalStatusCount_M = objMembersInformation.AnniversaryDateCount;
                    objMembersInformation.MaritalStatusCount_U = objMembersInformation.TotalMemberCount - objMembersInformation.AnniversaryDateCount;
                    objMembersInformation.MaritalStatusCount_Null = objMembersInformation.MaritalStatusCount_U;
                    CustAllData = CustAllData.Where(x => x.DOB.HasValue).ToList();
                    foreach (var item in CustAllData)
                    {
                        CustInformatonData Obj = new CustInformatonData();

                        Obj.Age = (Today.Year - item.DOB.Value.Year);

                        LstCustInfo.Add(Obj);
                    }

                    objMembersInformation.Age18to25 = LstCustInfo.Where(x => x.Age >= 18 && x.Age <= 25).Count();
                    objMembersInformation.Age26to35 = LstCustInfo.Where(x => x.Age >= 26 && x.Age <= 35).Count();
                    objMembersInformation.Age36to45 = LstCustInfo.Where(x => x.Age >= 36 && x.Age <= 45).Count();
                    objMembersInformation.Age46to55 = LstCustInfo.Where(x => x.Age >= 46 && x.Age <= 55).Count();
                    objMembersInformation.Age55Above = LstCustInfo.Where(x => x.Age >= 55).Count();

                    //}
                    //else
                    //{
                    //    objMembersInformation = context.Database.SqlQuery<MembersInformation>("sp_BOTS_MemberInformation @pi_GroupId, @pi_Date, @pi_LoginId",
                    //   new SqlParameter("@pi_GroupId", GroupId),
                    //   new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    //   new SqlParameter("@pi_LoginId", "")).FirstOrDefault<MembersInformation>();
                    //}

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
