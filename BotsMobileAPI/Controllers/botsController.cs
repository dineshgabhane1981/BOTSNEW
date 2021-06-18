using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;
using System.Web.Http.Results;
using System.Globalization;

namespace BotsMobileAPI.Controllers
{
    public class botsController : ApiController
    {
        LoginRepository LR = new LoginRepository();
        CustomerRepository CR = new CustomerRepository();
        ReportsRepository RR = new ReportsRepository();
        DashboardRepository DR = new DashboardRepository();
        LoyaltyKPIsRepository LKR = new LoyaltyKPIsRepository();
        CampaignRepository CMPR = new CampaignRepository();
        KeyIndecatorsRepository KR = new KeyIndecatorsRepository();

        Exceptions newexception = new Exceptions();
        [HttpGet]
        public Object GetToken(string username, string password)
        {
            string groupId = LR.AuthenticateUserMobile(username, password);
            if (!string.IsNullOrEmpty(groupId))
            {
                string key = "my_secret_key_98765"; //Secret key which will be used later during validation    
                                                    //var issuer = "http://mysite.com";  //normally this will be your site URL
                                                    //
                                                    //string value = System.Configuration.ConfigurationManager.AppSettings[key];
                var issuer = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];// "https://localhost:44330/";

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                //Create a List of Claims, Keep claims name short    
                var permClaims = new List<Claim>();
                permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                permClaims.Add(new Claim("valid", "1"));
                permClaims.Add(new Claim("userid", "1"));
                permClaims.Add(new Claim("name", "mobileBots"));

                //Create Security Token object by giving required parameters    
                var token = new JwtSecurityToken(issuer, //Issure    
                                issuer,  //Audience    
                                permClaims,
                                expires: DateTime.Now.AddDays(1),
                                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                List<string> responseData = new List<string>();
                responseData.Add(jwt_token);
                responseData.Add(groupId);
                return new { data = responseData };
            }
            else
            {
                return "Invalid Credentials";
            }
        }

        [HttpGet]
        public Object GetOutletList(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                var lstOutlet = RR.GetOutletList(GroupId, connectionString);
                return new { data = lstOutlet, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public Object GetMemberSegmentResult(string GroupId, string OutletId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<object> lstData = new List<object>();
                List<long> dataList = new List<long>();
                DashboardMemberSegment dataMemberSegment = new DashboardMemberSegment();
                try
                {
                    List<string> lstDates = new List<string>();
                    string connectionString = CR.GetCustomerConnString(GroupId);

                    dataMemberSegment = DR.GetDashboardMemberSegmentData(GroupId, OutletId, connectionString);
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return new { Data = dataMemberSegment, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        //blank or 1
        [HttpGet]
        public Object GetPointsSummaryResult(string GroupId, string monthFlag)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<long> dataList = new List<long>();
                DashboardPointsSummary dataPointsSummary = new DashboardPointsSummary();
                try
                {
                    string loginId = string.Empty;
                    string connectionString = CR.GetCustomerConnString(GroupId);

                    dataPointsSummary = DR.GetDashboardPointsSummaryData(GroupId, monthFlag, connectionString, loginId);
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return new { Data = dataPointsSummary, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public Object GetBulkUploadResult(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<object> dataList = new List<object>();
                DashboardBulkUpload objDashboardBulkUpload = new DashboardBulkUpload();
                string connectionString = CR.GetCustomerConnString(GroupId);
                try
                {
                    objDashboardBulkUpload = DR.GetDashboardBulkUpload(GroupId, connectionString);
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return new { Data = objDashboardBulkUpload, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        //point no 12,2-redemption rate 
        [HttpGet]
        public object GetRedemptionResult(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                DashboardRedemption objDashboardRedemption = new DashboardRedemption();
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<object> dataList = new List<object>();
                try
                {
                    objDashboardRedemption = DR.GetDashboardRedemption(GroupId, "1", connectionString);

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return new { Data = objDashboardRedemption, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object LoyaltyPerformance(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                LoyaltyKPIs objLoyaltyKPIs = new LoyaltyKPIs();
                try
                {
                    string connectionString = CR.GetCustomerConnString(GroupId);
                    objLoyaltyKPIs = LKR.GetobjLoyaltyKPIsData(GroupId, connectionString);
                    var Sum = objLoyaltyKPIs.Redemption + objLoyaltyKPIs.Referrals + objLoyaltyKPIs.Campaigns + objLoyaltyKPIs.SMSBlastWA + objLoyaltyKPIs.NewMWPRegistration;

                    objLoyaltyKPIs.RedemptionPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.Redemption) / Convert.ToDecimal(Sum)) * 100), 2);
                    objLoyaltyKPIs.ReferralsPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.Referrals) / Convert.ToDecimal(Sum)) * 100), 2);
                    objLoyaltyKPIs.CampaignsPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.Campaigns) / Convert.ToDecimal(Sum)) * 100), 2);
                    objLoyaltyKPIs.SMSBlastWAPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.SMSBlastWA) / Convert.ToDecimal(Sum)) * 100), 2);
                    objLoyaltyKPIs.NewMWPRegistrationPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.NewMWPRegistration) / Convert.ToDecimal(Sum)) * 100), 2);
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return new { Data = objLoyaltyKPIs, MaxJsonLength = Int32.MaxValue };
            }

            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetSharedBizResult(string GroupId, string OutletId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<object> lstData = new List<object>();
                List<string> nameList = new List<string>();
                List<long> firstList = new List<long>();
                List<long> repeatList = new List<long>();
                List<long> redeemList = new List<long>();

                try
                {
                    string connectionString = CR.GetCustomerConnString(GroupId);
                    List<DashboardBizShared> lstBizShared = new List<DashboardBizShared>();
                    lstBizShared = DR.GetDashboardBizShared(GroupId, OutletId, connectionString);
                    lstBizShared.Reverse();
                    foreach (var item in lstBizShared)
                    {
                        nameList.Add(item.MonthYear);
                        firstList.Add(Convert.ToInt64(item.FirstMemberTxn));
                        repeatList.Add(Convert.ToInt64(item.RepeatMemberTxn));
                        redeemList.Add(Convert.ToInt64(item.RedeemTxn));
                    }
                    lstData.Add(nameList);
                    lstData.Add(firstList);
                    lstData.Add(repeatList);
                    lstData.Add(redeemList);

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return new { Data = lstData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetCelebrationsTxnResult(string GroupId, int month, int type)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CelebrationsMoreDetails> objCelebrationsMoreDetails = new List<CelebrationsMoreDetails>();
                objCelebrationsMoreDetails = RR.GetCelebrationsTxnData(GroupId, month, type, connectionString);
                return new { Data = objCelebrationsMoreDetails, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        
        [HttpGet]
        public object GetPointsExpiryDataResult(string GroupId, int month, int year)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                PointExpiryTmp objPointExpiry = new PointExpiryTmp();
                objPointExpiry = RR.GetPointExpiryData(GroupId, month, year, connectionString);
                return new { Data = objPointExpiry, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //ponit no 12-no of profile updated,referral generated,memeberbase
        [HttpGet]
        public object GetMemberWebPageResult(string GroupId, string profileFlag)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<long> dataList = new List<long>();
                // var userDetails = (CustomerLoginDetail)Session["UserSession"];
                string connectionString = CR.GetCustomerConnString(GroupId);
                try
                {

                    DashboardMemberWebPage dataMemberWebPage = new DashboardMemberWebPage();
                    dataMemberWebPage = DR.GetDashboardMemberWebPageData(GroupId, profileFlag, connectionString);

                    dataList.Add(dataMemberWebPage.MemberBase);
                    dataList.Add(dataMemberWebPage.ReferringBase);
                    dataList.Add(dataMemberWebPage.ReferralGenerated);
                    dataList.Add(dataMemberWebPage.ReferralTransacted);
                    dataList.Add(dataMemberWebPage.ReferralTxnCount);
                    dataList.Add(dataMemberWebPage.BusinessGenerated);
                    dataList.Add(dataMemberWebPage.ProfileUpdatedCount);

                    if (dataMemberWebPage.MWPStatus == "No")
                    {
                        dataMemberWebPage.MWPStatusCode = 1;
                    }
                    else
                    {
                        dataMemberWebPage.MWPStatusCode = 0;
                    }
                    dataList.Add(dataMemberWebPage.MWPStatusCode);

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return new { Data = dataList, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetCampaignFirstData(string GroupId, string month, string year)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<Campaign> objCampaignData = new List<Campaign>();
                objCampaignData = CMPR.GetCampaignFirstData(GroupId, connectionString, month, year);
                return new { Data = objCampaignData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetCampaignSecondData(string GroupId, string month, string year, string CampaignId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
                objCampaignData = CMPR.GetCampaignSecondData(GroupId, connectionString, month, year, CampaignId);
                return new { Data = objCampaignData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        [HttpGet]
        public object GetCampaignThirdData(string GroupId, string month, string year, string CampaignId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignThird> objCampaignData = new List<CampaignThird>();
                objCampaignData = CMPR.GetCampaignThirdData(GroupId, connectionString, month, year, CampaignId);
                return new { Data = objCampaignData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetCampaignSMSBlastFirstData(string GroupId, string month, string year)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignSMSBlastFirst> objCampaignSMSBlastFirstData = new List<CampaignSMSBlastFirst>();
                objCampaignSMSBlastFirstData = CMPR.GetCampaignSMSBlastFirstData(GroupId, connectionString, month, year);
                return new { Data = objCampaignSMSBlastFirstData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetSMSBlastsSecondData(string GroupId, string month, string year, string CampaignId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
                objCampaignData = CMPR.GetSMSBlastsSecondData(GroupId, connectionString, month, year, CampaignId);
                return new { Data = objCampaignData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetSMSBlastsThirdData(string GroupId, string month, string year, string CampaignId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignThird> objCampaignData = new List<CampaignThird>();
                objCampaignData = CMPR.GetSMSBlastsThirdData(GroupId, connectionString, month, year, CampaignId);
                return new { Data = objCampaignData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object MemberPage(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                MemberPage objMemberPage = new MemberPage();
                objMemberPage = KR.GetMemberPageData(GroupId, connectionString);
                return new { Data = objMemberPage, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        //pointno 4-sub point 1
        [HttpGet]
        public object GetOnlyOnceResult(string GroupId, string outletId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                if (outletId.Equals("All"))
                {
                    outletId = "";
                }
                OnlyOnce objOnlyOnce = new OnlyOnce();
                objOnlyOnce = KR.GetOnlyOnceData(GroupId, outletId, connectionString);

                objOnlyOnce.TotalMemberStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.TotalMember));
                objOnlyOnce.OnlyOnceMemberStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.OnlyOnceMember));
                objOnlyOnce.RecentVisitHighStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.RecentVisitHigh));
                objOnlyOnce.RecentVisitLowStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.RecentVisitLow));
                objOnlyOnce.NotSeenHighStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.NotSeenHigh));
                objOnlyOnce.NotSeenLowStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.NotSeenLow));

                return new { Data = objOnlyOnce, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //pointno 4-sub point 2
        [HttpGet]
        public object NonRedeeming(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                NonRedemptionCls objNonRedemptionCls = new NonRedemptionCls();
                objNonRedemptionCls = KR.GetNonRedemptionData(GroupId, connectionString);
                return new { Data = objNonRedemptionCls, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //pointno 4-sub point 3
        [HttpPost]
        public object GetNonTransactingResult(string GroupId, string outletId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                if (outletId.Equals("All"))
                {
                    outletId = "";
                }
                NonTransactingCls objNonTransactingCls = new NonTransactingCls();
                objNonTransactingCls = KR.GetNonTransactingData(GroupId, outletId, connectionString);
                return new { Data = objNonTransactingCls, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

    }
}



    

