using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BOTS_BL.Repository
{
    public class DLCConfigRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        DashboardRepository DR = new DashboardRepository();
        Exceptions newexception = new Exceptions();
        public bool SaveDLCDashboardConfig(string GroupId, tblDLCDashboardConfig objDashboard)
        {
            bool result = false;
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    context.tblDLCDashboardConfigs.AddOrUpdate(objDashboard);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCDashboardConfig");
            }
            return result;
        }
        public tblDLCDashboardConfig GetDLCDashboardConfig(string GroupId)
        {
            tblDLCDashboardConfig objData = new tblDLCDashboardConfig();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.tblDLCDashboardConfigs.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCDashboardConfig");
            }

            return objData;
        }
        public List<tblDLCProfileUpdateConfig> GetProfileData(string GroupId)
        {
            List<tblDLCProfileUpdateConfig> lstData = new List<tblDLCProfileUpdateConfig>();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);

            using (var context = new BOTSDBContext(connStr))
            {
                lstData = context.tblDLCProfileUpdateConfigs.ToList();
            }
            return lstData;
        }
        public bool UpdateProfileData(string GroupId, List<tblDLCProfileUpdateConfig> objData)
        {
            bool status = false;
            var connstr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    foreach (var item in objData)
                    {
                        context.tblDLCProfileUpdateConfigs.AddOrUpdate(item);
                        context.SaveChanges();
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateProfileData");
            }
            return status;

        }

        public bool ProfileDataInsert(string Groupid, string Name, string NameMandStat, string Gender, string GenderMandStat, string BirthDate, string BirthMandStat, string Marrital, string MargMandStat, string Area, string AreaMandStat, string City, string CityMandStat, string Pincode, string PinMandStat, string Email, string MailMandStat)
        {
            bool status = false;
            var connstr = objCustRepo.GetCustomerConnString(Groupid);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    SqlConnection _Con = new SqlConnection(connstr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_DLCProfileUpdateInsert", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_Groupid", Groupid);
                        SqlParameter param2 = new SqlParameter("pi_Name", Name);
                        SqlParameter param3 = new SqlParameter("pi_NameMandStat", NameMandStat);
                        SqlParameter param4 = new SqlParameter("pi_Gender", Gender);
                        SqlParameter param5 = new SqlParameter("pi_GenderMandStat", GenderMandStat);
                        SqlParameter param6 = new SqlParameter("pi_BirthDate", BirthDate);
                        SqlParameter param7 = new SqlParameter("pi_BirthMandStat", BirthMandStat);
                        SqlParameter param8 = new SqlParameter("pi_Marrital", Marrital);
                        SqlParameter param9 = new SqlParameter("pi_MargMandStat", MargMandStat);
                        SqlParameter param10 = new SqlParameter("pi_Area", Area);
                        SqlParameter param11 = new SqlParameter("pi_AreaMandStat", AreaMandStat);
                        SqlParameter param12 = new SqlParameter("pi_City", City);
                        SqlParameter param13 = new SqlParameter("pi_CityMandStat", CityMandStat);
                        SqlParameter param14 = new SqlParameter("pi_Pincode", Pincode);
                        SqlParameter param15 = new SqlParameter("pi_PinMandStat", PinMandStat);
                        SqlParameter param16 = new SqlParameter("pi_Email", Email);
                        SqlParameter param17 = new SqlParameter("pi_MailMandStat", MailMandStat);

                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param6);
                        cmdReport.Parameters.Add(param7);
                        cmdReport.Parameters.Add(param8);
                        cmdReport.Parameters.Add(param9);
                        cmdReport.Parameters.Add(param10);
                        cmdReport.Parameters.Add(param11);
                        cmdReport.Parameters.Add(param12);
                        cmdReport.Parameters.Add(param13);
                        cmdReport.Parameters.Add(param14);
                        cmdReport.Parameters.Add(param15);
                        cmdReport.Parameters.Add(param16);
                        cmdReport.Parameters.Add(param17);

                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["Result"]) == "1")
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "ProfileDataInsert");
            }


            return status;
        }

        public bool PublishDLCDashboardConfig(CustomerLoginDetail userDetails)
        {
            bool status = false;
            var connstr = objCustRepo.GetCustomerConnString(userDetails.GroupId);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    var configData = context.tblDLCDashboardConfigs.FirstOrDefault();

                    var configPData = context.tblDLCDashboardConfig_Publish.FirstOrDefault();
                    tblDLCDashboardConfig_Publish objData = new tblDLCDashboardConfig_Publish();
                    if (configPData != null)
                        objData.SlNo = configPData.SlNo;

                    objData.LoginWithOTP = configData.LoginWithOTP;
                    objData.RedirectToPage = configData.RedirectToPage;
                    objData.AddPersonalDetails = configData.AddPersonalDetails;
                    objData.AddGiftPoints = configData.AddGiftPoints;
                    objData.AddReferFriend = configData.AddReferFriend;
                    objData.ShowLogoToFooter = configData.ShowLogoToFooter;
                    objData.CollectPersonalDataRandomly = configData.CollectPersonalDataRandomly;
                    objData.UseLogo = configData.UseLogo;
                    objData.UseLogoURL = configData.UseLogoURL;
                    objData.PrefferedLanguage = configData.PrefferedLanguage;
                    objData.CountryCode = configData.CountryCode;
                    objData.HeaderColor = configData.HeaderColor;
                    objData.FontColor = configData.FontColor;
                    objData.AddedBy = userDetails.UserId;
                    objData.AddedDate = DateTime.Now;
                    context.tblDLCDashboardConfig_Publish.AddOrUpdate(objData);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "PublishDLCDashboardConfig");
            }
            return status;
        }

        public bool PublishDLCProfileUpdate(string Groupid)
        {
            bool status = false;
            var connstr = objCustRepo.GetCustomerConnString(Groupid);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    SqlConnection _Con = new SqlConnection(connstr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_DLCProfileUpdatePublish", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_Groupid", Groupid);

                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);

                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["Result"]) == "1")
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "PublishDLCProfileUpdate");
            }


            return status;
        }

        public tblDLCDashboardConfig_Publish GetPublishDLCDashboardConfig(string groupId)
        {
            tblDLCDashboardConfig_Publish objData = new tblDLCDashboardConfig_Publish();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.tblDLCDashboardConfig_Publish.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPublishDLCDashboardConfig");
            }
            return objData;
        }
        public List<tblDLCFrontEndPageDataNew> GetDLCFrontEndPageData(string groupId)
        {
            List<tblDLCFrontEndPageDataNew> objData = new List<tblDLCFrontEndPageDataNew>();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.tblDLCFrontEndPageDataNews.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCFrontEndPageData");
            }
            return objData;
        }
        public List<tblCountyCode> GetCountryCodes()
        {
            List<tblCountyCode> lstCodes = new List<tblCountyCode>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstCodes = context.tblCountyCodes.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCountryCodes");
            }
            return lstCodes;
        }

        public string CheckUserAndSendOTP(string groupId, string mobileNo)
        {
            string status = string.Empty;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    var IsOTP = context.tblDLCDashboardConfig_Publish.Where(x => x.LoginWithOTP == "OTP").FirstOrDefault();
                    if (IsOTP != null)
                    {
                        var smsDetails = context.SMSDetails.FirstOrDefault();
                        //Send OTP
                        var result = SendOTP(groupId, mobileNo, smsDetails);
                        status = "OTP";
                    }
                    else
                    {
                        var userDetails = context.tblDLCUserDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                        if (userDetails == null)
                        {
                            var smsDetails = context.SMSDetails.FirstOrDefault();
                            //Send OTP
                            var result = SendOTP(groupId, mobileNo, smsDetails);
                            status = "OTP";
                        }
                        else
                        {
                            status = "Password";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CheckUserAndSendOTP");
            }

            return status;
        }
        public bool SendOTP(string groupId, string MobileNo, SMSDetail smsDetail)
        {
            bool result = false;
            try
            {
                Random r = new Random();
                int randNum = r.Next(10000);
                string fourDigitNumber = randNum.ToString("0000");
                var OTPstatus = InsertOTP(groupId, MobileNo, Convert.ToInt32(fourDigitNumber));

                var _MobileMessage = "Dear Member, " + Convert.ToInt32(fourDigitNumber) + "  is your OTP. Sample SMS for OTP - Blue Ocktopus ";
                var _UserName = smsDetail.OTPUserName;
                var _Password = smsDetail.OTPPassword;
                var _MobileNo = MobileNo;
                var _Sender = smsDetail.SenderId;
                var _Url = smsDetail.OTPUrl;

                result = SendSMS(_MobileMessage, _UserName, _Password, _MobileNo, _Sender, _Url);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendOTP");
            }

            return result;
        }

        public bool InsertOTP(string groupid, string mobileno, int otp)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupid);

            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    OTPMaintenance objData = new OTPMaintenance();
                    objData.MobileNo = mobileno;
                    objData.Datetime = DateTime.Now;
                    objData.CounterId = groupid;
                    objData.OTP = Convert.ToString(otp);
                    context.OTPMaintenances.AddOrUpdate(objData);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendOTP");
            }
            return status;
        }
        public bool SendSMS(string _MobileMessage, string _UserName, string _Password, string _MobileNo, string _Sender, string _Url)
        {
            bool status = false;
            try
            {
                _MobileMessage = _MobileMessage.Replace("#99", "&");
                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                string type1 = "TEXT";
                StringBuilder sbposdata1 = new StringBuilder();
                sbposdata1.AppendFormat("username={0}", _UserName);
                sbposdata1.AppendFormat("&password={0}", _Password);
                sbposdata1.AppendFormat("&to={0}", _MobileNo);
                sbposdata1.AppendFormat("&from={0}", _Sender);//BLUEOC
                sbposdata1.AppendFormat("&text={0}", _MobileMessage);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(_Url);
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
                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendSMS");
            }
            return status;
        }

        public bool CheckPasswordExist(string groupId, string mobileNo)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var userDetails = context.tblDLCUserDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                if (userDetails != null)
                {
                    status = true;
                }
            }
            return status;
        }
        public bool ValidateUserByPassword(string groupId, string mobileNo, string Password)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var userDetails = context.tblDLCUserDetails.Where(x => x.MobileNo == mobileNo && x.Password== Password).FirstOrDefault();
                if (userDetails != null)
                {
                    status = true;
                }
            }
            return status;
        }

        public bool ValidateUserByOTP(string groupId, string mobileNo, string Otp)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var isValidOTP = context.OTPMaintenances.Where(x => x.MobileNo == mobileNo && x.OTP == Otp).FirstOrDefault();
                if (isValidOTP != null)
                {
                    status = true;
                }
            }
            return status;
        }
    }
}
