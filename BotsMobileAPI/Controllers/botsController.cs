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
using BOTS_BL.Models.CommonDB;
using System.Net.Mail;
using System.Web.Http.Cors;

//using System.Web.Mvc;

namespace BotsMobileAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class botsController : ApiController
    {
        LoginRepository LR = new LoginRepository();
        CustomerRepository CR = new CustomerRepository();
        ReportsRepository RR = new ReportsRepository();
        DashboardRepository DR = new DashboardRepository();
        LoyaltyKPIsRepository LKR = new LoyaltyKPIsRepository();
        CampaignRepository CMPR = new CampaignRepository();
        KeyIndecatorsRepository KR = new KeyIndecatorsRepository();
        CustomerOnBoardingRepository CBR = new CustomerOnBoardingRepository();

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
                    OutletId = OutletId.ToUpper();
                    if (OutletId.Equals("ALL"))
                    {
                        OutletId = "";
                    }
                    List<string> lstDates = new List<string>();
                    string connectionString = CR.GetCustomerConnString(GroupId);

                    dataMemberSegment = DR.GetDashboardMemberSegmentData(GroupId, OutletId, connectionString, "","","");
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

                    dataPointsSummary = DR.GetDashboardPointsSummaryData(GroupId, monthFlag, connectionString, loginId, "", "");
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
                    objDashboardBulkUpload = DR.GetDashboardBulkUpload(GroupId, connectionString, "", "", "");
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
                    objDashboardRedemption = DR.GetDashboardRedemption(GroupId, "1", connectionString, "", "", "");

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
                    objLoyaltyKPIs = LKR.GetobjLoyaltyKPIsData(GroupId, connectionString, "");
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
                    OutletId = OutletId.ToUpper();
                    if (OutletId.Equals("ALL"))
                    {
                        OutletId = "";
                    }
                    string connectionString = CR.GetCustomerConnString(GroupId);
                    List<DashboardBizShared> lstBizShared = new List<DashboardBizShared>();
                    lstBizShared = DR.GetDashboardBizShared(GroupId, OutletId, connectionString, "", "", "");
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
        //point no 9
        [HttpGet]
        public object Celebrations(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);

                var CelebrationsData = RR.GetCelebrationsData(GroupId, connectionString);

                return new { Data = CelebrationsData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //point no 9
        [HttpGet]
        public object PointsExpiry(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);

                PointExpiryTmp objPointExpiry = new PointExpiryTmp();
                objPointExpiry = RR.GetPointExpiryData(GroupId, DateTime.Now.Month, DateTime.Now.Year, connectionString, "");
                return new { Data = objPointExpiry, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //ponit no 12-no of profile updated,referral generated,memeberbase
        //profileflag:1(referral),0(profile)
        //alternative for enrolled base
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
                    dataMemberWebPage = DR.GetDashboardMemberWebPageData(GroupId, profileFlag, connectionString, "", "", "");

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

        //campaign 1st click data
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

        //get celebration data on 1st click
        [HttpGet]
        public object GetCampaignCelebrationsData(string GroupId, string month, string year)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignCelebrations> objCampaignCelebrations = new List<CampaignCelebrations>();
                objCampaignCelebrations = CMPR.GetCampaignCelebrationsData(GroupId, connectionString, month, year);
                return new { Data = objCampaignCelebrations, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //celebration 2nd click data
        //type 1=birthday,2=anniversary
        [HttpGet]
        public object GetCampaignCelebrationsSecondData(string GroupId, string month, string year, string type)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
                objCampaignCelebrationsData = CMPR.GetCampaignCelebrationsSecondData(GroupId, connectionString, month, year, type);
                return new { Data = objCampaignCelebrationsData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //point expiry 1st click data
        [HttpGet]
        public object GetCampaignPointsExpiryData(string GroupId, string month, string year)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignPointsExpiry> objCampaignPointsExpiry = new List<CampaignPointsExpiry>();
                objCampaignPointsExpiry = CMPR.GetCampaignPointsExpiryData(GroupId, connectionString, month, year);
                return new { Data = objCampaignPointsExpiry, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //point expiry 2nd click data
        [HttpGet]
        public object GetCampaignPointsExpirySecondData(string GroupId, string month, string year, string type)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
                objCampaignCelebrationsData = CMPR.GetCampaignCelebrationsSecondData(GroupId, connectionString, month, year, type);
                return new { Data = objCampaignCelebrationsData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        //Inactive 1st click data
        [HttpGet]
        public object GetCampaignInactiveData(string GroupId, string month, string year)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignInactive> objCampaignInactive = new List<CampaignInactive>();
                objCampaignInactive = CMPR.GetCampaignInactiveData(GroupId, connectionString, month, year);
                return new { Data = objCampaignInactive, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //Inactive 2nd click data
        [HttpGet]
        public object GetCampaignInactiveSecondData(string GroupId, string month, string year)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                List<CampaignInactiveData> objCampaignInactiveData = new List<CampaignInactiveData>();
                objCampaignInactiveData = CMPR.GetCampaignInactiveSecondData(GroupId, connectionString, month, year);
                return new { Data = objCampaignInactiveData, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //campaign 2nd click data
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
        //campaign 3nd click data
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
        //smsblast 1st click data
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
        //smsblast 2nd click data
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
        //smsblast 3rd click data
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
        //slide 13-table and slide 14 all boxes
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

        //pie chart-slide 13
        [HttpGet]
        public object GetMemberMisinformationData(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                MembersInformation objMembersInformation = new MembersInformation();
                objMembersInformation = KR.GetMemberMisinformationData(GroupId, connectionString, "");
                return new { Data = objMembersInformation, MaxJsonLength = Int32.MaxValue };
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
                outletId = outletId.ToUpper();
                if (outletId.Equals("ALL"))
                {
                    outletId = "";
                }
                OnlyOnce objOnlyOnce = new OnlyOnce();
                objOnlyOnce = KR.GetOnlyOnceData(GroupId, outletId, connectionString, "");

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
                objNonRedemptionCls = KR.GetNonRedemptionData(GroupId, connectionString, "");
                return new { Data = objNonRedemptionCls, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //pointno 4-sub point 3 more than 6 months
        [HttpGet]
        public object GetNonTransactingResult(string GroupId, string outletId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                outletId = outletId.ToUpper();
                if (outletId.Equals("ALL"))
                {
                    outletId = "";
                }
                NonTransactingCls objNonTransactingCls = new NonTransactingCls();
                objNonTransactingCls = KR.GetNonTransactingData(GroupId, outletId, connectionString, "");
                return new { Data = objNonTransactingCls, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //ReferAndEarnPoints
        [HttpPost]
        public HttpResponseMessage ReferAndEarnPoints([FromBody] ReferAndEarn referAndEarn)
        {

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    using (var context = new CommonDBContext())
                    {
                        context.ReferAndEarns.Add(referAndEarn);
                        context.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.Created, "Created with SRNo-" + referAndEarn.SlNo.ToString());

                        return message;
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token or Expired");
        }
        //membersearch
        [HttpGet]
        public object GetMemberSearchResult(string SearchData, string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                CustomerRepository objCustRepo = new CustomerRepository();
                MemberSearch objMemberSearch = new MemberSearch();

                string loginId = string.Empty;

                if (!string.IsNullOrEmpty(GroupId) && GroupId != "undefined")
                {

                    objMemberSearch = RR.GetMeamberSearchData(GroupId, SearchData, connectionString, loginId);
                }
                else
                {
                    objMemberSearch = RR.GetMeamberSearchData(GroupId, SearchData, connectionString, loginId);
                }

                return new { Data = objMemberSearch, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object AuthenticateUser(string LoginId, string Password)
        {
            if (User.Identity.IsAuthenticated)
            {
                // string connectionString = CR.GetCustomerConnString(GroupId);
                DatabaseDetail DBDetails = new DatabaseDetail();
                CustomerLoginDetail userDetail = new CustomerLoginDetail();

                var userDetails = LR.AuthenticateUserAPI(LoginId, Password);


                return new { Data = userDetails, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        //this API for sending email from contact us form in blueocktopus website
        [HttpPost]
        public HttpResponseMessage ContactUsFromWebsite(string firstname, string lastname, string emailid, string subject, string emailmessage)
        {
            try
            {

                // var result = "";                   
                // string To = emailto;
                using (MailMessage mail = new MailMessage())
                {

                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear Sir,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Following customer is contacted us through website</td>");
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.Append("Name:" + firstname + lastname);
                    str.Append("</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.Append("Email Id:</br>" + emailid);
                    str.Append("</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.Append("Subject:" + subject);
                    str.Append("</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.Append("Message:" + emailmessage);
                    str.Append("</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.Append("Regards,");
                    str.Append("</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.Append("- Blue Ocktopus Team");
                    str.Append("</td>");
                    str.Append("</tr>");
                    str.Append("</table>");

                    MailMessage Msg = new MailMessage();
                    Msg.From = new MailAddress("blueocktopus2015@gmail.com");
                    Msg.To.Add("ashlesha@blueocktopus.in");
                    Msg.Subject = "New Enquiry";
                    Msg.Body = str.ToString();
                    Msg.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("blueocktopus2015@gmail.com", "blueocktopus$");
                    smtp.Send(Msg);
                    Msg.Dispose();

                }

                var message = Request.CreateResponse(HttpStatusCode.Created, "Email send Sucessfully");

                return message;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }

        [HttpPost]
        public HttpResponseMessage SendOTP(string mobileNo)
        {
            bool status = false;
            //if (User.Identity.IsAuthenticated)
            //{
            CustomerLoginDetail objcustlogin = new CustomerLoginDetail();
            using (var context = new CommonDBContext())
            {

                objcustlogin = context.CustomerLoginDetails.Where(x => x.LoginId == mobileNo).FirstOrDefault();

                if (objcustlogin != null)
                {
                    var sender = "BLUEOC";
                    var Url = " https://http2.myvfirst.com/smpp/sendsms?";
                    Random random = new Random();
                    int randNum = random.Next(1000000);
                    string sixDigitNumber = randNum.ToString("D6");
                    // string sixDigitNumber = "123456";
                    status = DR.InsertOTP(mobileNo, Convert.ToInt32(sixDigitNumber));
                    string MobileMessage = "Dear Member," + sixDigitNumber + " is your OTP. Sample SMS for OTP - Blue Ocktopus";
                    bool result = DR.SendOTPMessage(mobileNo, sender, MobileMessage, Url);
                    // bool result = true;
                    if (result)
                    {
                        var message = Request.CreateResponse(HttpStatusCode.OK);
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "OTP Not Send");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Number is Not Registered");

                }
            }
            //}
            // return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token or Expired");
        }

        [HttpGet]
        public object VerifyOTP(string mobileNo, int OTP)
        {

            bool status = DR.VerifyOTP(mobileNo, OTP);
            if (status)
            {
                CustomerLoginDetail objcustlogin = new CustomerLoginDetail();
                using (var context = new CommonDBContext())
                {
                    objcustlogin = context.CustomerLoginDetails.Where(x => x.LoginId == mobileNo).FirstOrDefault();

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
                    responseData.Add(objcustlogin.GroupId);
                    responseData.Add(objcustlogin.UserName);
                    return new { data = responseData };
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect OTP");
            }

        }
        //slide no 8
        //datarangeflage =0,fromdate and todate blanck
        //datarangeflag =1,fromdate and todate values
        [HttpGet]
        public object GetEnrollBase(string DateRangeFlag, string fromDate, string toDate, string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginId = string.Empty;
                string connectionString = CR.GetCustomerConnString(GroupId);
                //if (userDetails.LevelIndicator == "03" || userDetails.LevelIndicator == "04")
                //{
                //    loginId = userDetails.OutletOrBrandId;
                //}
                List<OutletWise> lstOutlet = new List<OutletWise>();
                lstOutlet = RR.GetOutletWiseList(GroupId, DateRangeFlag, fromDate, toDate, connectionString, loginId);
                OutletWise objSum = new OutletWise();
                foreach (var item in lstOutlet)
                {
                    objSum.TotalMember = (objSum.TotalMember == null ? 0 : objSum.TotalMember) + (item.TotalMember == null ? 0 : item.TotalMember);
                    objSum.TotalTxn = (objSum.TotalTxn == null ? 0 : objSum.TotalTxn) + (item.TotalTxn == null ? 0 : item.TotalTxn);
                    objSum.TotalSpend = (objSum.TotalSpend == null ? 0 : objSum.TotalSpend) + (item.TotalSpend == null ? 0 : item.TotalSpend);
                    objSum.ATS = (objSum.ATS == null ? 0 : objSum.ATS) + (item.ATS == null ? 0 : item.ATS);
                    objSum.NonActive = (objSum.NonActive == null ? 0 : objSum.NonActive) + (item.NonActive == null ? 0 : item.NonActive);
                    objSum.OnlyOnce = (objSum.OnlyOnce == null ? 0 : objSum.OnlyOnce) + (item.OnlyOnce == null ? 0 : item.OnlyOnce);
                    objSum.PointsEarned = (objSum.PointsEarned == null ? 0 : objSum.PointsEarned) + (item.PointsEarned == null ? 0 : item.PointsEarned);
                    objSum.PointsBurned = (objSum.PointsBurned == null ? 0 : objSum.PointsBurned) + (item.PointsBurned == null ? 0 : item.PointsBurned);
                    objSum.PointsCancelled = (objSum.PointsCancelled == null ? 0 : objSum.PointsCancelled) + (item.PointsCancelled == null ? 0 : item.PointsCancelled);
                    objSum.PointsExpired = (objSum.PointsExpired == null ? 0 : objSum.PointsExpired) + (item.PointsExpired == null ? 0 : item.PointsExpired);

                    if (item.TotalMember > 0)
                    {
                        item.NonActivePer = (Convert.ToDecimal(item.NonActive) * 100) / Convert.ToDecimal(item.TotalMember);
                        item.OnlyOncePer = (Convert.ToDecimal(item.OnlyOnce) * 100) / Convert.ToDecimal(item.TotalMember);
                    }
                }
                List<OutletWise> lstOutletFinal = new List<OutletWise>();
                OutletWise objAdmin = new OutletWise();
                objAdmin = lstOutlet.Where(x => x.OutletName.ToLower().IndexOf("admin") >= 0).FirstOrDefault();
                if (objAdmin != null)
                {
                    foreach (var item in lstOutlet)
                    {
                        if (item.OutletId != objAdmin.OutletId)
                        {
                            lstOutletFinal.Add(item);
                        }
                    }
                    lstOutletFinal.Add(objAdmin);
                }
                else
                {
                    lstOutletFinal = lstOutlet;
                }

                int totalCount = lstOutletFinal.Count;
                int nonActiveRed = totalCount * 30 / 100;
                int nonActiveOrange = totalCount * 45 / 100;
                int nonActiveGreen = totalCount - (nonActiveRed + nonActiveOrange);

                var nonActiveRedOutlets = lstOutletFinal.OrderByDescending(x => x.NonActivePer).Take(nonActiveRed).ToList();
                var nonActiveOrangeOutlets = lstOutletFinal.OrderByDescending(x => x.NonActivePer).Skip(nonActiveRed).Take(nonActiveOrange).ToList();
                var nonActiveGreenOutlets = lstOutletFinal.OrderByDescending(x => x.NonActivePer).Skip(nonActiveRed + nonActiveOrange).Take(nonActiveGreen).ToList();

                var onlyOnceRedOutlets = lstOutletFinal.OrderByDescending(x => x.OnlyOncePer).Take(nonActiveRed).ToList();
                var onlyOnceOrangeOutlets = lstOutletFinal.OrderByDescending(x => x.OnlyOncePer).Skip(nonActiveRed).Take(nonActiveOrange).ToList();
                var onlyOnceGreenOutlets = lstOutletFinal.OrderByDescending(x => x.OnlyOncePer).Skip(nonActiveRed + nonActiveOrange).Take(nonActiveGreen).ToList();

                var RedmRedOutlets = lstOutletFinal.OrderBy(x => x.RedemptionRate).Take(nonActiveRed).ToList();
                var RedmOrangeOutlets = lstOutletFinal.OrderBy(x => x.RedemptionRate).Skip(nonActiveRed).Take(nonActiveOrange).ToList();
                var RedmGreenOutlets = lstOutletFinal.OrderBy(x => x.RedemptionRate).Skip(nonActiveRed + nonActiveOrange).Take(nonActiveGreen).ToList();


                foreach (var item in lstOutletFinal)
                {
                    var outletRed = nonActiveRedOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletRed != null)
                    {
                        item.NonActiveColor = "Red";
                    }
                    var outletOrange = nonActiveOrangeOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletOrange != null)
                    {
                        item.NonActiveColor = "Orange";
                    }
                    var outletGreen = nonActiveGreenOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletGreen != null)
                    {
                        item.NonActiveColor = "Green";
                    }

                    var outletOORed = onlyOnceRedOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletOORed != null)
                    {
                        item.OnlyOnceColor = "Red";
                    }
                    var outletOOOrange = onlyOnceOrangeOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletOOOrange != null)
                    {
                        item.OnlyOnceColor = "Orange";
                    }
                    var outletOOGreen = onlyOnceGreenOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletOOGreen != null)
                    {
                        item.OnlyOnceColor = "Green";
                    }

                    var outletRRRed = RedmRedOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletRRRed != null)
                    {
                        item.RedemptionRateColor = "Red";
                    }
                    var outletRROrange = RedmOrangeOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletRROrange != null)
                    {
                        item.RedemptionRateColor = "Orange";
                    }
                    var outletRRGreen = RedmGreenOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                    if (outletRRGreen != null)
                    {
                        item.RedemptionRateColor = "Green";
                    }
                }
                objSum.OutletName = "Total";
                lstOutletFinal.Add(objSum);
                return new { Data = lstOutletFinal, MaxJsonLength = Int32.MaxValue };
                //  return PartialView("_Outletwise", lstOutletFinal);
            }
            return "Invalid Token or Expired";
        }
        //slide no 16-4 tiles
        [HttpGet]
        public object GetCampaignData(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                CampaignTiles objCampaignTiles = new CampaignTiles();
                // var userDetails = (CustomerLoginDetail)Session["UserSession"];
                objCampaignTiles = CMPR.GetCampaignTilesData(GroupId, connectionString);
                List<System.Web.Mvc.SelectListItem> MonthList = new List<System.Web.Mvc.SelectListItem>();

                for (int i = 0; i < 12; i++)
                {
                    MonthList.Add(new System.Web.Mvc.SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddMonths(i).ToString("MMM")),
                        Value = Convert.ToString(DateTime.Now.AddMonths(i).Month)
                    });
                }
                List<System.Web.Mvc.SelectListItem> YearList = new List<System.Web.Mvc.SelectListItem>();
                int year = DateTime.Now.Year;
                for (int i = 0; i <= 9; i++)
                {
                    YearList.Add(new System.Web.Mvc.SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddYears(i).Year.ToString()),
                        Value = Convert.ToString(year + i)
                    });
                }

                objCampaignTiles.lstMonth = MonthList;
                objCampaignTiles.lstYear = YearList;


                return new { Data = objCampaignTiles, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";

        }
        //slide no 17
        [HttpGet]
        public object GetProfile(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                ProfilePage objprofilepage = CR.GetprofilePageData(GroupId);

                return new { Data = objprofilepage, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //slide no 18
        [HttpGet]
        public object GetMonthlySnapShot(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                MobileAppOnceInMonthData objmonthlydata = CR.GetMonthlySnapShotForMobileApp(GroupId);

                return new { Data = objmonthlydata, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpGet]
        public object GetCityList()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<CityDetails> objcity = new List<CityDetails>();
                objcity = CBR.GetCityList();

                return new { Data = objcity, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }

        [HttpPost]
        public HttpResponseMessage MobileAppEmail(string PageName, string mobileNo)
        {
            try
            {
                CustomerLoginDetail objcustlogin = new CustomerLoginDetail();
                using (var context = new CommonDBContext())
                {
                    objcustlogin = context.CustomerLoginDetails.Where(x => x.LoginId == mobileNo).FirstOrDefault();

                    // var result = "";                   
                    // string To = emailto;
                    using (MailMessage mail = new MailMessage())
                    {

                        StringBuilder str = new StringBuilder();
                        str.Append("<table>");
                        str.Append("<tr>");
                        str.AppendLine("<td>Dear Sir,</td>");
                        str.AppendLine("</br>");
                        str.Append("</tr>");
                        str.Append("<tr>");
                        str.AppendLine("<td>Following customer is contacted us through Mobile App</td>");
                        str.Append("<tr>");
                        str.Append("<td>");
                        str.Append("Name:" + objcustlogin.UserName);
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("<tr>");
                        str.Append("<td>");
                        str.Append("Email Id:</br>" + objcustlogin.EmailId);
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("<tr>");
                        str.Append("<td>");
                        str.Append("Mobile No:</br>" + mobileNo);
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("<tr>");
                        str.Append("<td>");
                        str.Append("Subject:" + PageName);
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("<tr>");
                        str.Append("<td>");
                        str.Append("Message:" + "Please contact the customer");
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("<tr>");
                        str.Append("<td>");
                        str.Append("Regards,");
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("<tr>");
                        str.Append("<td>");
                        str.Append("- Blue Ocktopus Team");
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("</table>");

                        MailMessage Msg = new MailMessage();
                        Msg.From = new MailAddress("sblueocktopus@gmail.com");
                        Msg.To.Add("ashlesha@blueocktopus.in");
                        Msg.Subject = "Customer Enquiry-" + PageName;
                        Msg.Body = str.ToString();
                        Msg.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        smtp.Credentials = new System.Net.NetworkCredential("sblueocktopus@gmail.com", "Ocktopus@2016");
                        smtp.Send(Msg);
                        Msg.Dispose();

                    }

                    var message = Request.CreateResponse(HttpStatusCode.Created, "Email send Sucessfully");

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}





