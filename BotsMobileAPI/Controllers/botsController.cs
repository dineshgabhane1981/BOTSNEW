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

namespace BotsMobileAPI.Controllers
{
    public class botsController : ApiController
    {
        LoginRepository LR = new LoginRepository();
        CustomerRepository CR = new CustomerRepository();
        ReportsRepository RR = new ReportsRepository();
        DashboardRepository DR = new DashboardRepository();
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
    }
}
