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
                objPointExpiry = RR.GetPointExpiryData(GroupId, DateTime.Now.Month, DateTime.Now.Year, connectionString);
                return new { Data = objPointExpiry, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //ponit no 12-no of profile updated,referral generated,memeberbase
        //profileflag:1(referral),0(profile)
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
        [HttpGet]
        public object GetOnlyOnceTxnResult(string GroupId, string outletId, string type)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                if (outletId.Equals("All"))
                {
                    outletId = "";
                }
                List<OnlyOnceTxn> objOnlyOnceTxn = new List<OnlyOnceTxn>();
                objOnlyOnceTxn = KR.GetOnlyOnceTxnData(GroupId, outletId, type, connectionString);              

                return new { Data = objOnlyOnceTxn, MaxJsonLength = Int32.MaxValue };
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
        //pointno 4-sub point 3 more than 6 months
        [HttpGet]
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
        public object GetMemberSearchResult(string searchData, string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                CustomerRepository objCustRepo = new CustomerRepository();
                MemberSearch objMemberSearch = new MemberSearch();
               
                  //  var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    string loginId = string.Empty;
                   
                    if (!string.IsNullOrEmpty(GroupId) && GroupId != "undefined")
                    {
                       // string connStr = objCustRepo.GetCustomerConnString(GroupId);
                        objMemberSearch = RR.GetMeamberSearchData(GroupId, searchData, connectionString, loginId);
                    }
                    else
                    {
                        objMemberSearch = RR.GetMeamberSearchData(GroupId, searchData,connectionString, loginId);
                    }
               
                return new { Data = objMemberSearch, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
        //need to add API
        [HttpGet]
        public object GetSMSCreditBalance(string GroupId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string connectionString = CR.GetCustomerConnString(GroupId);
                var whatsApptoken ="";
                using (var contextNew = new BOTSDBContext(connectionString))
                {                    
                     whatsApptoken = contextNew.SMSDetails.Select(x => x.WhatsappTokenId).FirstOrDefault();
                }
               
                return new { Data = whatsApptoken, MaxJsonLength = Int32.MaxValue };
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

        //this API for sending email of contact us form in blueocktopus website
        [HttpPost]
        public HttpResponseMessage ContactUsFromWebsite(string firstname,string lastname,string emailid,string subject,string emailmessage )
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
                    Msg.From = new MailAddress("info@blueocktopus.in");
                    Msg.To.Add("ashlesha@blueocktopus.in");
                    Msg.Subject = "New Enquiry";
                    Msg.Body = str.ToString();
                    Msg.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.zoho.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("info@blueocktopus.in", "Info@123");
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
            if (User.Identity.IsAuthenticated)
            {
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
            }
            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token or Expired");
        }
        [HttpGet]
        public object VerifyOTP(string emailId, int OTP)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool status = DR.VerifyOTP(emailId, OTP);
                return new { Data = status, MaxJsonLength = Int32.MaxValue };
            }
            return "Invalid Token or Expired";
        }
     }
}



    

