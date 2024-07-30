using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
                    objData.PersonalDetailsPoints = configData.PersonalDetailsPoints;
                    objData.ReferPoints = configData.ReferPoints;
                    objData.GiftPoints = configData.GiftPoints;

                    objData.IsExtraWidgetText1 = configData.IsExtraWidgetText1;
                    objData.ExtraWidgetText1 = configData.ExtraWidgetText1;
                    objData.ExtraWidgetPoints1 = configData.ExtraWidgetPoints1;

                    objData.IsExtraWidgetText2 = configData.IsExtraWidgetText2;
                    objData.ExtraWidgetText2 = configData.ExtraWidgetText2;
                    objData.ExtraWidgetPoints2 = configData.ExtraWidgetPoints2;

                    objData.IsExtraWidgetText3 = configData.IsExtraWidgetText3;
                    objData.ExtraWidgetText3 = configData.ExtraWidgetText3;
                    objData.ExtraWidgetPoints3 = configData.ExtraWidgetPoints3;

                    objData.PrefferedLanguage = configData.PrefferedLanguage;
                    objData.CountryCode = configData.CountryCode;
                    objData.HeaderColor = configData.HeaderColor;
                    objData.FontColor = configData.FontColor;
                    objData.AddedBy = userDetails.UserId;
                    objData.AddedDate = DateTime.Now;
                    objData.UseCard = configData.UseCard;
                    objData.UseCardURL = configData.UseCardURL;
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
                    var publishedData = context.tblDLCProfileUpdateConfig_Publish.ToList();
                    foreach (var item in publishedData)
                    {
                        context.tblDLCProfileUpdateConfig_Publish.Remove(item);
                        context.SaveChanges();
                    }
                    var profileData = context.tblDLCProfileUpdateConfigs.ToList();
                    foreach (var item in profileData)
                    {
                        tblDLCProfileUpdateConfig_Publish newItem = new tblDLCProfileUpdateConfig_Publish();
                        newItem.FieldName = item.FieldName;
                        newItem.DisplayStatus = item.IsDisplay;
                        newItem.MandStatus = item.IsMandatory;
                        context.tblDLCProfileUpdateConfig_Publish.Add(newItem);
                        context.SaveChanges();
                    }
                    status = true;
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

        public List<tblDLCProfileUpdateConfig_Publish> GetPublishDLCProfileConfig(string groupId, string MobileNo)
        {
            List<tblDLCProfileUpdateConfig_Publish> objData = new List<tblDLCProfileUpdateConfig_Publish>();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.tblDLCProfileUpdateConfig_Publish.ToList();
                    var custData = context.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    //var childData = context.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    foreach (var item in objData)
                    {
                        if (item.FieldName == "Name")
                            item.Value = custData.Name;
                        if (item.FieldName == "Gender")
                            item.Value = custData.Gender;
                        if (item.FieldName == "DateOfBirth")
                        {
                            if (custData.DOB.HasValue)
                                item.Value = custData.DOB.Value.ToString("yyyy/MM/dd");
                        }
                        //if (item.FieldName == "MaritalStatus")
                        //    item.Value = custData.MaritalStatus;
                        if (item.FieldName == "Email")
                            item.Value = custData.Email;
                        //if (item.FieldName == "Area")
                        //    item.Value = custData.Area;
                        //if (item.FieldName == "City")
                        //    item.Value = custData.City;
                        //if (item.FieldName == "Pincode")
                        //    item.Value = custData.Pincode;
                    }
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
                        var smsDetails = context.tblSMSWhatsAppCredentials.FirstOrDefault();
                        //Send OTP
                        var result = SendOTP(groupId, mobileNo, smsDetails);
                        status = "OTP";
                    }
                    else
                    {
                        var userDetails = context.tblDLCUserDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                        if (userDetails == null)
                        {
                            var smsDetails = context.tblSMSWhatsAppCredentials.FirstOrDefault();
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

        public tblSMSWhatsAppCredential GetSMSDetails(string groupid)
        {
            tblSMSWhatsAppCredential smsDetails = new tblSMSWhatsAppCredential();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(groupid);
                using (var context = new BOTSDBContext(connStr))
                {
                    smsDetails = context.tblSMSWhatsAppCredentials.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSMSDetails");
            }

            return smsDetails;
        }
        public bool SendOTP(string groupId, string MobileNo, tblSMSWhatsAppCredential smsDetail)
        {
            bool result = false;
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                using (var context = new BOTSDBContext(connStr))
                {
                    Random r = new Random();
                    int randNum = r.Next(10000);
                    string fourDigitNumber = randNum.ToString("0000");
                    var OTPstatus = InsertOTP(groupId, MobileNo, Convert.ToInt32(fourDigitNumber));
                    var DLCSMSWAScript = context.tblDLCSMSWAScriptMasters.Where(x => x.DLCMessageId == 107).FirstOrDefault();
                    var _MobileMessage = DLCSMSWAScript.DLCSMSScript;//"Dear Member, " + Convert.ToInt32(fourDigitNumber) + "  is your OTP. Sample SMS for OTP - Blue Ocktopus ";
                    _MobileMessage = _MobileMessage.Replace("#14", fourDigitNumber);
                    var _UserName = smsDetail.SMSLoginId;
                    var _Password = smsDetail.SMSPassword;
                    var _MobileNo = MobileNo;
                    var _Sender = smsDetail.SMSSenderId;
                    var _Url = smsDetail.SMSUrl;
                    newexception.AddDummyException(_MobileMessage);
                    result = SendSMS(_MobileMessage, _UserName, _Password, _MobileNo, _Sender, _Url, smsDetail.SMSVendor, smsDetail.SMSAPIKey);
                }
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
                    tblOTPDetail objData = new tblOTPDetail();
                    objData.MobileNo = mobileno;
                    objData.Datetime = DateTime.Now;
                    objData.CounterId = groupid;
                    objData.OTP = Convert.ToString(otp);
                    context.tblOTPDetails.AddOrUpdate(objData);
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
        public bool SendSMS(string _MobileMessage, string _UserName, string _Password, string _MobileNo, string _Sender, string _Url, string _SMSVendor, string _SMSAPIKey)
        {
            bool status = false;
            try
            {
                switch (_SMSVendor)
                {
                    case "TechnoCore":

                        string date_TechnoCoreText = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

                        _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                        string type_TechnoCoreText = "text";
                        StringBuilder sbposdata_TechnoCoreText = new StringBuilder();
                        sbposdata_TechnoCoreText.AppendFormat("userid={0}", _UserName);
                        sbposdata_TechnoCoreText.AppendFormat("&password={0}", _Password);
                        sbposdata_TechnoCoreText.AppendFormat("&sendMethod={0}", "quick");
                        sbposdata_TechnoCoreText.AppendFormat("&mobile={0}", _MobileNo);
                        sbposdata_TechnoCoreText.AppendFormat("&msg={0}", _MobileMessage);
                        sbposdata_TechnoCoreText.AppendFormat("&senderid={0}", _Sender);
                        sbposdata_TechnoCoreText.AppendFormat("&msgType={0}", type_TechnoCoreText);
                        sbposdata_TechnoCoreText.AppendFormat("&format={0}", type_TechnoCoreText);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                        HttpWebRequest httpWReq_TechnoCoreText = (HttpWebRequest)WebRequest.Create(_Url);
                        UTF8Encoding encoding_TechnoCoreText = new UTF8Encoding();
                        byte[] data_TechnoCoreText = encoding_TechnoCoreText.GetBytes(sbposdata_TechnoCoreText.ToString());
                        httpWReq_TechnoCoreText.Method = "POST";
                        httpWReq_TechnoCoreText.ContentType = "application/x-www-form-urlencoded";
                        httpWReq_TechnoCoreText.ContentLength = data_TechnoCoreText.Length;
                        using (Stream stream_TechnoCoreText = httpWReq_TechnoCoreText.GetRequestStream())
                        {
                            stream_TechnoCoreText.Write(data_TechnoCoreText, 0, data_TechnoCoreText.Length);
                        }
                        HttpWebResponse response_TechnoCoreText = (HttpWebResponse)httpWReq_TechnoCoreText.GetResponse();
                        StreamReader reader_TechnoCoreText = new StreamReader(response_TechnoCoreText.GetResponseStream());
                        string responseString_TechnoCoreText = reader_TechnoCoreText.ReadToEnd();
                        reader_TechnoCoreText.Close();
                        response_TechnoCoreText.Close();
                        break;
                    case "Vision":

                        var httpWebRequest_VisionText = (HttpWebRequest)WebRequest.Create(_Url);
                        httpWebRequest_VisionText.ContentType = "application/json";
                        httpWebRequest_VisionText.Method = "POST";

                        using (var streamWriter_VisionText = new StreamWriter(httpWebRequest_VisionText.GetRequestStream()))
                        {
                            string json_VisionText = "{\"Account\":" +
                                            "{\"APIKey\":\"" + _SMSAPIKey + "\"," +
                                            "\"SenderId\":\"" + _Sender + "\"," +
                                            "\"Channel\":\"Trans\"," +
                                            "\"DCS\":\"0\"," +
                                            "\"SchedTime\":null," +
                                            "\"GroupId\":null}," +
                                            "\"Messages\":[{\"Number\":\"" + _MobileNo + "\"," +
                                            "\"Text\":\"" + _MobileMessage + "\"}]" +
                                            "}";
                            streamWriter_VisionText.Write(json_VisionText);
                        }
                        var httpResponse_VisionText = (HttpWebResponse)httpWebRequest_VisionText.GetResponse();
                        using (var streamReader_VisionText = new StreamReader(httpResponse_VisionText.GetResponseStream()))
                        {
                            var result_VisionText = streamReader_VisionText.ReadToEnd();
                        }

                        break;
                    case "Pinnacle":
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                        var httpWebRequest_PinnacleText = (HttpWebRequest)WebRequest.Create(_Url);
                        httpWebRequest_PinnacleText.ContentType = "application/json";
                        httpWebRequest_PinnacleText.Headers.Add("Apikey", _Password);
                        httpWebRequest_PinnacleText.Method = "POST";

                        using (var streamWriter_PinnacleText = new StreamWriter(httpWebRequest_PinnacleText.GetRequestStream()))
                        {
                            string json_PinnacleText = "{\"sender\":\"" + _Sender + "\"," +
                            "\"message\":[{\"number\":\"" + _MobileNo + "\"," +
                             "\"text\":\"" + _MobileMessage + "\"}]," + "\"messagetype\":\"TXT\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
                            streamWriter_PinnacleText.Write(json_PinnacleText);
                        }
                        var httpResponse_PinnacleText = (HttpWebResponse)httpWebRequest_PinnacleText.GetResponse();
                        using (var streamReader_PinnacleText = new StreamReader(httpResponse_PinnacleText.GetResponseStream()))
                        {
                            var result_PinnacleText = streamReader_PinnacleText.ReadToEnd();
                        }

                        break;
                    case "ValueFirst":
                        string type_ValueFirstText = "TEXT";
                        StringBuilder sbposdata_ValueFirstText = new StringBuilder();
                        sbposdata_ValueFirstText.AppendFormat("username={0}", _UserName);
                        sbposdata_ValueFirstText.AppendFormat("&password={0}", _Password);
                        sbposdata_ValueFirstText.AppendFormat("&to={0}", _MobileNo);
                        sbposdata_ValueFirstText.AppendFormat("&from={0}", _Sender);
                        sbposdata_ValueFirstText.AppendFormat("&text={0}", _MobileMessage);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                        HttpWebRequest httpWReq_ValueFirstText = (HttpWebRequest)WebRequest.Create(_Url);
                        UTF8Encoding encoding_ValueFirstText = new UTF8Encoding();
                        byte[] data_ValueFirstText = encoding_ValueFirstText.GetBytes(sbposdata_ValueFirstText.ToString());
                        httpWReq_ValueFirstText.Method = "POST";
                        httpWReq_ValueFirstText.ContentType = "application/x-www-form-urlencoded";
                        httpWReq_ValueFirstText.ContentLength = data_ValueFirstText.Length;
                        using (Stream stream_ValueFirstText = httpWReq_ValueFirstText.GetRequestStream())
                        {
                            stream_ValueFirstText.Write(data_ValueFirstText, 0, data_ValueFirstText.Length);
                        }
                        HttpWebResponse response_ValueFirstText = (HttpWebResponse)httpWReq_ValueFirstText.GetResponse();
                        StreamReader reader_ValueFirstText = new StreamReader(response_ValueFirstText.GetResponseStream());
                        string responseString_ValueFirstText = reader_ValueFirstText.ReadToEnd();
                        reader_ValueFirstText.Close();
                        response_ValueFirstText.Close();
                        break;
                }




                //_MobileMessage = _MobileMessage.Replace("#99", "&");
                //_MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                //string type1 = "TEXT";
                //StringBuilder sbposdata1 = new StringBuilder();
                //sbposdata1.AppendFormat("username={0}", _UserName);
                //sbposdata1.AppendFormat("&password={0}", _Password);
                //sbposdata1.AppendFormat("&to={0}", _MobileNo);
                //sbposdata1.AppendFormat("&from={0}", _Sender);//BLUEOC
                //sbposdata1.AppendFormat("&text={0}", _MobileMessage);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(_Url);
                //UTF8Encoding encoding1 = new UTF8Encoding();
                //byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
                //httpWReq1.Method = "POST";
                //httpWReq1.ContentType = "application/x-www-form-urlencoded";
                //httpWReq1.ContentLength = data1.Length;
                //newexception.AddDummyException("Start SMS");

                //using (Stream stream1 = httpWReq1.GetRequestStream())
                //{
                //    stream1.Write(data1, 0, data1.Length);
                //}
                //HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
                //StreamReader reader1 = new StreamReader(response1.GetResponseStream());
                //string responseString1 = reader1.ReadToEnd();
                //reader1.Close();
                //response1.Close();
                status = true;
                newexception.AddDummyException("End SMS");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendSMS");
            }
            return status;
        }

        public tblDLCUserDetail CheckPasswordExist(string groupId, string mobileNo)
        {
            tblDLCUserDetail objData = new tblDLCUserDetail();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var userDetails = context.tblDLCUserDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                if (userDetails != null)
                {
                    objData = userDetails;
                }
            }
            return objData;
        }
        public bool ValidateUserByPassword(string groupId, string mobileNo, string Password)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var userDetails = context.tblDLCUserDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                if (userDetails != null)
                {
                    if (Password == userDetails.Password)
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
                var isValidOTP = context.tblOTPDetails.Where(x => x.MobileNo == mobileNo && x.OTP == Otp).FirstOrDefault();
                if (isValidOTP != null)
                {
                    //Add Customer
                    status = true;
                }
            }
            return status;
        }

        public bool RegisterCustomer(string groupId, string mobileNo, string countryCode ,string source)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    tblCustDetailsMaster objCust = new tblCustDetailsMaster();
                    var outletId = context.tblOutletMasters.Where(x => x.OutletName.ToLower().Contains("admin")).Select(y => y.OutletId).FirstOrDefault();

                    objCust.MobileNo = mobileNo;
                    objCust.Name = "Member";
                    objCust.Id = mobileNo + groupId;
                    objCust.EnrolledOutlet = outletId;
                    objCust.DOJ = DateTime.Now;
                    objCust.EnrolledBy = source?? "DLCWalkIn";
                    objCust.IsActive = true;
                    objCust.DisableTxn = false;
                    objCust.DisableSMSWAPromo = false;
                    objCust.DisableSMSWATxn = false;
                    objCust.CountryCode = countryCode;
                    objCust.CurrentEnrolledOutlet = outletId;
                    context.tblCustDetailsMasters.Add(objCust);
                    context.SaveChanges();

                    tblCustInfo objInfo = new tblCustInfo();
                    objInfo.MobileNo = mobileNo;
                    objInfo.Name = "Member";
                    context.tblCustInfoes.Add(objInfo);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "RegisterCustomer " + groupId);
            }
            return status;
        }

        public bool InsertPassword(string mobileNo, string password, string groupId)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    var userdetail = context.tblDLCUserDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    if (userdetail != null)
                    {
                        userdetail.Password = password.Trim();
                        userdetail.UpdatedDate = DateTime.Now;
                        context.tblDLCUserDetails.AddOrUpdate(userdetail);
                        context.SaveChanges();
                    }
                    else
                    {
                        tblDLCUserDetail objData = new tblDLCUserDetail();
                        objData.MobileNo = mobileNo.Trim();
                        objData.Password = password.Trim();
                        objData.AddedDate = DateTime.Now;
                        context.tblDLCUserDetails.AddOrUpdate(objData);
                        context.SaveChanges();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "InsertPassword");
                }
            }
            return status;
        }

        public DLCDashboardContent GetDLCDashboardContent(string groupid, string mobileno)
        {
            DLCDashboardContent objData = new DLCDashboardContent();
            string connStr = objCustRepo.GetCustomerConnString(groupid);
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    //objData.EarnPoints = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileno).Select(y => y.po).FirstOrDefault();
                    objData.EarnPoints = context.tblCustPointsMasters.Where(x => x.MobileNo == mobileno && x.IsActive == true).Sum(y => y.Points);
                    objData.BasePoints = context.tblCustPointsMasters.Where(x => x.MobileNo == mobileno && x.IsActive == true && x.PointsType== "Base").Sum(y => y.Points);

                    objData.EarnPercentage = context.tblRuleMasters.Select(x => x.PointsPercentage).FirstOrDefault();
                    objData.PointsToRS = context.tblRuleMasters.Select(x => x.PointsAllocation).FirstOrDefault();
                    var outletDetails = context.tblOutletMasters.FirstOrDefault();
                    objData.OutletName = outletDetails.OutletName;
                    objData.OutletAddress = outletDetails.Address;
                    objData.OutletLongitude = outletDetails.Longitude;
                    objData.OutletLatitude = outletDetails.Latitude;
                    var objCust= context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileno).FirstOrDefault();
                    //var optout = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileno).Select(y => y.DisableSMSWAPromo).FirstOrDefault();
                    //var custumerName = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileno).Select(y => y.Name).FirstOrDefault();
                    objData.CustomerName = objCust.Name;
                    //var pointsinRs = objData.EarnPoints * PointsToRS;
                    //objData.PointsToRS = pointsinRs;
                    if (objCust.DisableSMSWAPromo.HasValue)
                    {
                        if (objCust.DisableSMSWAPromo.Value)
                        {
                            objData.IsOptout = true;
                        }
                        else
                        {
                            objData.IsOptout = false;
                        }
                    }
                    else
                    {
                        objData.IsOptout = false;
                    }
                    if (objCust.DisableSMSWATxn.HasValue)
                    {
                        if (objCust.DisableSMSWATxn.Value)
                        {
                            objData.IsOptoutTxn = true;
                        }
                        else
                        {
                            objData.IsOptoutTxn = false;
                        }
                    }
                    else
                    {
                        objData.IsOptoutTxn = false;
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetDLCDashboardContent");
                }
            }

            return objData;
        }
        
        public bool UpdateOptout(string groupId, string mobileNo, bool optout, bool isTxn)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    if (custDetails != null)
                    {
                        if (isTxn)
                            custDetails.DisableSMSWATxn = optout;
                        else
                            custDetails.DisableSMSWAPromo = optout;
                        context.tblCustDetailsMasters.AddOrUpdate(custDetails);
                        context.SaveChanges();
                        status = true;
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UpdateOptout" + groupId);
                }
            }

            return status;
        }
        //public bool Update1Optout(string groupId, string mobileNo, bool optout)
        //{
        //    bool status = false;
        //    string connStr = objCustRepo.GetCustomerConnString(groupId);
        //    using (var context = new BOTSDBContext(connStr))
        //    {
        //        try
        //        {
        //            var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
        //            if (custDetails != null)
        //            {
        //                custDetails.DisableSMSWAPromo = optout;
        //                context.tblCustDetailsMasters.AddOrUpdate(custDetails);
        //                context.SaveChanges();
        //                status = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "UpdateOptout" + groupId);
        //        }
        //    }

        //    return status;
        //}

        //public bool GetOptout(string groupId, string mobileNo)
        //{
        //    var status = false;
        //    string connStr = objCustRepo.GetCustomerConnString(groupId);
        //    using (var context = new BOTSDBContext(connStr))
        //    {
        //        try
        //        {
        //            var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
        //            if (custDetails != null)
        //            {
                        
        //                    status = custDetails.DisableSMSWAPromo ?? false;
                        
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "GetOptout" + groupId);
        //        }

        //    }

        //    return status;
        //}

        //public bool Getstatus(string groupId, string mobileNo)
        //{
        //    var status = false;
        //    string connStr = objCustRepo.GetCustomerConnString(groupId);
        //    using (var context = new BOTSDBContext(connStr))
        //    {
        //        try
        //        {
        //            var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
        //            if (custDetails != null)
        //            {
        //                status = custDetails.DisableTxn ?? false;
                        
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "Getstatus" + groupId);
        //        }
        //    }

        //    return status;
        //}
        
        public DLCSPResponse GiveGiftPoints(string MobileNo, string BrandId, string RecipientName, string RecipientNo, string GiftPoints, string groupId)
        {
            DLCSPResponse objResult = new DLCSPResponse();
            string DBName = String.Empty;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new CommonDBContext())
            {
                DBName = context.tblDatabaseDetails.Where(x => x.GroupId == groupId).Select(y => y.DBName).FirstOrDefault();
            }
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    var result = context.Database.SqlQuery<DLCSPResponse>("sp_DLCGiftingPoints @pi_MobileNo, @pi_BrandId, @pi_Datetime, @pi_GiftingPersonMobileNo, @pi_GiftingPersonName, @pi_GiftingPoints,@pi_OTPValue,@pi_DBName",
                               new SqlParameter("@pi_MobileNo", MobileNo),
                               new SqlParameter("@pi_BrandId", BrandId),
                               new SqlParameter("@pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd")),
                               new SqlParameter("@pi_GiftingPersonMobileNo", RecipientNo),
                               new SqlParameter("@pi_GiftingPersonName", RecipientName),
                               new SqlParameter("@pi_GiftingPoints", GiftPoints),
                               new SqlParameter("@pi_OTPValue", ""),
                               new SqlParameter("@pi_DBName", DBName)).ToList<DLCSPResponse>();

                    foreach (var item in result)
                    {
                        if (item.ResponseCode == "1")
                        {
                            if (!string.IsNullOrEmpty(item.WATokenId))
                            {
                                var result1 = SendMessage(item.WAMessage, item.WATokenId, item.WAUrl, item.MobileNo);
                            }
                        }
                        objResult = item;
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GiveGiftPoints" + groupId);
                }
            }
            return objResult;
        }
        public bool SendMessage(string WAMessage, string WATokenId, string WAUrl, string MobileNo)
        {
            bool result = false;

            string responseString;
            try
            {

                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(WAUrl);
                sbposdata.AppendFormat("token={0}", WATokenId);
                sbposdata.AppendFormat("&phone={0}", MobileNo);
                sbposdata.AppendFormat("&message={0}", WAMessage);
                string Url = sbposdata.ToString();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbposdata.ToString());
                httpWReq.Method = "POST";

                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                //this.WriteToFile(responseString);

                reader.Close();
                response.Close();
                result = true;
            }
            catch (ArgumentException ex)
            {

                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
                //this.WriteToFile(responseString);
            }
            catch (WebException ex)
            {
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                //this.WriteToFile(responseString);
            }
            catch (Exception ex)
            {
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                //this.WriteToFile(responseString);
            }
            return result;
        }
        public bool DLCReferFriend(string groupId, string MobileNo, string BrandId, string firstMobileNo, string firstName, string secondMobileNo, string secondName, string thirdMobileNo, string thirdName)
        {
            bool status = false;
            string DBName = String.Empty;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new CommonDBContext())
            {
                DBName = context.tblDatabaseDetails.Where(x => x.GroupId == groupId).Select(y => y.DBName).FirstOrDefault();
            }
            using (var context = new BOTSDBContext(connStr))
            {
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("yyyy-MM-dd");
                try
                {
                    var result = context.Database.SqlQuery<DLCSPResponse>("sp_DLCRefer @pi_MobileNo, @pi_BrandId, @pi_Datetime, " +
                        "@pi_1stMobileNo, @pi_1stName, @pi_2ndMobileNo,@pi_2ndName,@pi_3rdMobileNo,@pi_3rdName, @pi_DBName",
                                  new SqlParameter("@pi_MobileNo", MobileNo),
                                  new SqlParameter("@pi_BrandId", BrandId),
                                  new SqlParameter("@pi_Datetime", formattedDate),
                                  new SqlParameter("@pi_1stMobileNo", firstMobileNo),
                                  new SqlParameter("@pi_1stName", firstName),
                                  new SqlParameter("@pi_2ndMobileNo", secondMobileNo),
                                  new SqlParameter("@pi_2ndName", secondName),
                                  new SqlParameter("@pi_3rdMobileNo", thirdMobileNo),
                                  new SqlParameter("@pi_3rdName", thirdName),
                                   new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<DLCSPResponse>();
                    if (result.ResponseCode=="1")
                        status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "DLCReferFriend" + groupId);
                }
            }
            return status;
        }

        public List<tblDLCFrontEndPageDataTNC> GetTNC(string groupId)
        {
            List<tblDLCFrontEndPageDataTNC> lstData = new List<tblDLCFrontEndPageDataTNC>();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    lstData = context.tblDLCFrontEndPageDataTNCs.ToList();
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetTNC" + groupId);
                }
                return lstData;
            }
        }
        public List<tblDLCFrontEndPageDataReferTNC> GetMWPReferTNC(string groupId)
        {
            List<tblDLCFrontEndPageDataReferTNC> lstData = new List<tblDLCFrontEndPageDataReferTNC>();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    lstData = context.tblDLCFrontEndPageDataReferTNCs.ToList();
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetMWPReferTNC " + groupId);
                }
                return lstData;
            }
        }

        public bool UpdateDLCProfileData(string groupId, DLCProfileData objData)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            string DBName = String.Empty;
            using (var context = new CommonDBContext())
            {
                DBName = context.tblDatabaseDetails.Where(x => x.GroupId == groupId).Select(y => y.DBName).FirstOrDefault();
            }

            using (var context = new BOTSDBContext(connStr))
            {                
                string DOB = string.Empty;
                if(!string.IsNullOrEmpty(objData.DateOfBirth))
                {
                    DOB = Convert.ToDateTime(objData.DateOfBirth).ToString("yyyy-MM-dd");
                }
                try
                {
                    var result = context.Database.SqlQuery<DLCSPResponse>("sp_DLCProfileUpdate @pi_MobileNo, @pi_BrandId, @pi_Datetime, " +
                       "@pi_Name, @pi_Gender, @pi_DOB, @pi_Email, @pi_Pincode, @pi_MaritalStatus, @pi_AnniversaryDate, @pi_Address, @pi_ChildCount, " +
                       "@pi_Child1DOB, @pi_Child2DOB, @pi_Child3DOB ,@pi_City, @pi_LanguagePreferred, @pi_Religion, @pi_DBName, @pi_Area",
                                 new SqlParameter("@pi_MobileNo", objData.MobileNo),
                                 new SqlParameter("@pi_BrandId", objData.BrandId),
                                 new SqlParameter("@pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd")),
                                 new SqlParameter("@pi_Name", objData.Name),
                                 new SqlParameter("@pi_Gender", objData.Gender ?? ""),
                                 new SqlParameter("@pi_DOB", DOB),
                                 new SqlParameter("@pi_Email", objData.Email ?? ""),
                                 new SqlParameter("@pi_Pincode", objData.Pincode),
                                 new SqlParameter("@pi_MaritalStatus", objData.MaritalStatus),
                                 new SqlParameter("@pi_AnniversaryDate", ""),
                                 new SqlParameter("@pi_Address", objData.Area),
                                 new SqlParameter("@pi_ChildCount", ""),
                                 new SqlParameter("@pi_Child1DOB", ""),
                                 new SqlParameter("@pi_Child2DOB", ""),
                                 new SqlParameter("@pi_Child3DOB", ""),
                                 new SqlParameter("@pi_City", objData.City ?? ""),
                                 new SqlParameter("@pi_LanguagePreferred", ""),
                                 new SqlParameter("@pi_Religion", ""),
                                 new SqlParameter("@pi_DBName", DBName),
                                 new SqlParameter("@pi_Area", objData.Area ?? "")).FirstOrDefault<DLCSPResponse>();

                    if (result.ResponseCode == "0")
                        status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UpdateDLCProfileData" + groupId);
                }
            }
            return status;
        }

        public List<SelectListItem> GetBrandsByGroupId(string groupId)
        {
            List<SelectListItem> lstBrands = new List<SelectListItem>();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var Brands = context.tblBrandMasters.Where(x => x.GroupId == groupId).ToList();
                foreach (var item in Brands)
                {
                    lstBrands.Add(new SelectListItem
                    {
                        Text = item.BrandName,
                        Value = Convert.ToString(item.BrandId)
                    });
                }
            }
            return lstBrands;
        }

        public List<tblDLCCampaignMaster> GetDlcLinks(string connStr)
        {
            List<tblDLCCampaignMaster> lstDlcLinks = new List<tblDLCCampaignMaster>();
            using (var context = new BOTSDBContext(connStr))
            {
                lstDlcLinks = context.tblDLCCampaignMasters.ToList();
            }
            return lstDlcLinks;
        }
        public tblDLCCampaignMaster GetDlcLinkByName(string connStr, string DlcName)
        {
            tblDLCCampaignMaster objDlcLink = new tblDLCCampaignMaster();
            using (var context = new BOTSDBContext(connStr))
            {
                objDlcLink = context.tblDLCCampaignMasters.Where(x => x.DLCName == DlcName).FirstOrDefault();
            }
            return objDlcLink;
        }
        public bool SaveDLCLink(string connStr, string DLcName, string DlcLink, string StartDate, string EndDate)
        {
            bool result = false;
            using (var context = new BOTSDBContext(connStr))
            {
                tblDLCCampaignMaster objData = new tblDLCCampaignMaster();
                objData.DLCName = DLcName;
                objData.DLCLink = DlcLink;

                if (!string.IsNullOrEmpty(StartDate))
                    objData.StartDate = Convert.ToDateTime(StartDate);
                if (!string.IsNullOrEmpty(EndDate))
                    objData.EndDate = Convert.ToDateTime(EndDate);
                objData.CreatedDate = DateTime.Now;
                context.tblDLCCampaignMasters.Add(objData);
                context.SaveChanges();

                result = true;
            }
            return result;
        }

    }
}
