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
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BOTS_BL.Repository
{
    public class GinesysRedeemptionRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public string GetCustomerConnString(string GroupId)
        {
            string ConnectionString = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {                    
                    var DBDetails = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault(); 
                    if (DBDetails != null)
                    {                        
                        ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    }                   
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerConnString");
            }
            return ConnectionString;
        }
        //public  GinesysRedeemModel GetCustomerDetails(GinesysRedeemModel objData)
        //{            
        //    string groupId = objData.StoreId.Substring(0, 4);
        //    string connStr = GetCustomerConnString(groupId);
        //    try
        //    {
        //        using (var context = new BOTSDBContext(connStr))
        //        {
        //            context.Database.CommandTimeout = 180;

        //            var BlockedPoints = context.tblBurnPtsSoftBlocks.Where(x => x.MobileNo == objData.MobileNo && x.IsActive == true).Sum(y=>y.BurnPoints);

        //            objData.Points  = context.tblCustPointsMasters.Where(x => x.MobileNo == objData.MobileNo && x.IsActive == true).Sum(y => y.Points);
        //            if (BlockedPoints != null)
        //            {
        //                objData.Points = objData.Points - BlockedPoints;
        //            }
        //            var rules =context.tblRuleMasters.FirstOrDefault();
        //            if(rules!=null)
        //            {
        //                objData.PointsValue = rules.PointsAllocation;
        //                objData.PointsToRedeem = Convert.ToDecimal(objData.InvoiceAmount) *(rules.BurnInvoiceAmtPercentage/100);
        //            }
                    
        //            objData.CustomerName = context.tblCustDetailsMasters.Where(x => x.MobileNo == objData.MobileNo).Select(y => y.Name).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        newexception.AddException(ex, "GetCustomerDetails");
        //    }
        //    return objData;
        //} 

        public BurnValidateResponse BurnValidation(string storeid, string Points, string InvoiceAmt, string MobileNo, string BillGUID)
        {
            BurnValidateResponse obj = new BurnValidateResponse();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            string groupId = storeid.Substring(0, 4);
            string connStr = GetCustomerConnString(groupId);
            
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    SqlConnection _Con = new SqlConnection(connStr);
                    
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_BillingBurnValidationWeb", _Con);
                    cmdReport.CommandTimeout = 300;
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", storeid);
                        SqlParameter param2 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", BillGUID);
                        SqlParameter param4 = new SqlParameter("pi_MobileNo", MobileNo);
                        SqlParameter param5 = new SqlParameter("pi_BurnPoints", Points);
                        SqlParameter param6 = new SqlParameter("pi_INDDatetime", indianTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param6);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];

                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            obj.ResponseCode = "00";
                            obj.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            obj.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                            obj.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["PointsAsAmt"]);
                            obj.PointsValue = Convert.ToString(dt1.Rows[0]["PointsAsAmt"]);
                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSWASendStatus"]);

                                //if (SMSStatus == "SMS")
                                //{
                                //    string _MobileNo = dt2.Rows[0]["MobileNo"].ToString();
                                //    string _MobileMessage = dt2.Rows[0]["SMSScript"].ToString();
                                //    string _UserName = dt2.Rows[0]["SMSLoginId"].ToString();
                                //    string _Password = dt2.Rows[0]["SMSPassword"].ToString();
                                //    string _Sender = dt2.Rows[0]["SMSSenderId"].ToString();
                                //    string _Url = dt2.Rows[0]["SMSUrl"].ToString();
                                //    //string _SMSBrandId = dt2.Rows[0]["SMSBrandId"].ToString();
                                //    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                                //    _job.Start();
                                //}

                                switch (SMSStatus)
                                {
                                    case "SMS":
                                           string _MobileNo = dt2.Rows[0]["MobileNo"].ToString();
                                           string _MobileMessage = dt2.Rows[0]["SMSScript"].ToString();
                                           string _UserName = dt2.Rows[0]["SMSLoginId"].ToString();
                                           string _Password = dt2.Rows[0]["SMSPassword"].ToString();
                                           string _Sender = dt2.Rows[0]["SMSSenderId"].ToString();
                                           string _Url = dt2.Rows[0]["SMSUrl"].ToString();
                                           string _SMSVendor = dt2.Rows[0]["SMSVendor"].ToString();
                                           string _SMSScriptType = dt2.Rows[0]["SMSScriptType"].ToString();

                                             Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSVendor, _SMSScriptType));
                                            _job.Start();
                                        break;
                                    case "WA":
                                           string _MobileNoWA = dt2.Rows[0]["MobileNo"].ToString();
                                           string _MobileMessageWA = dt2.Rows[0]["SMSScript"].ToString();
                                           string _WATokenId = dt2.Rows[0]["WhatsAppTokenId"].ToString();
                                           string _WAUrl = dt2.Rows[0]["WhatsAppUrl"].ToString();

                                             Thread _jobWA = new Thread(() => SendWA(_MobileNoWA, _MobileMessageWA, _WATokenId, _WAUrl));
                                            _jobWA.Start();
                                        break;
                                    default:

                                          string _MobileNoSMSWA = dt2.Rows[0]["MobileNo"].ToString();
                                          string _MobileMessageSMS = dt2.Rows[0]["SMSScript"].ToString();
                                          string _UserNameSMSWA = dt2.Rows[0]["SMSLoginId"].ToString();
                                          string _PasswordSMSWA = dt2.Rows[0]["SMSPassword"].ToString();
                                          string _SenderSMSWA = dt2.Rows[0]["SMSSenderId"].ToString();
                                          string _UrlSMSWA = dt2.Rows[0]["SMSUrl"].ToString();
                                          string _SMSAPIKey = dt2.Rows[0]["SMSAPIKey"].ToString();
                                          string _SMSVendorSMSWA = dt2.Rows[0]["SMSVendor"].ToString();
                                          string _SMSScriptTypeSMSWA = dt2.Rows[0]["SMSScriptType"].ToString();
                                          string _MobileMessageWASMSWA = dt2.Rows[0]["WhatsAppScript"].ToString();
                                          string _WATokenIdSMSWA = dt2.Rows[0]["WhatsAppTokenId"].ToString();
                                          string _WAUrlSMSWA = dt2.Rows[0]["WhatsAppUrl"].ToString();
                                          string _SMSTemplateId = string.Empty;
                                          string _DisableSMS = string.Empty;


                                        Thread _jobSMSWA = new Thread(() => SendSMSWA(_MobileNoSMSWA, _MobileMessageWASMSWA, _WATokenIdSMSWA, _WAUrlSMSWA, _MobileMessageSMS, _SMSTemplateId, _SMSScriptTypeSMSWA, _SMSVendorSMSWA, _UrlSMSWA, _UserNameSMSWA, _PasswordSMSWA,  _SMSAPIKey, _DisableSMS , _SenderSMSWA, SMSStatus));
                                        _jobSMSWA.Start();
                                        break;
                                }
                            }
                        }
                        else
                        {
                            obj.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            obj.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "BurnValidation");
            }
            return obj;
        }

        public BurnValidateResponse BurnCouponValidation(string storeid, string Coupon, string InvoiceAmt, string MobileNo, string BillGUID)
        {
            BurnValidateResponse obj = new BurnValidateResponse();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            string groupId = storeid.Substring(0, 4);
            string connStr = GetCustomerConnString(groupId);

            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    SqlConnection _Con = new SqlConnection(connStr);

                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_BillingBurnValidationCouponWeb", _Con);
                    cmdReport.CommandTimeout = 300;
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", storeid);
                        SqlParameter param2 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", BillGUID);
                        SqlParameter param4 = new SqlParameter("pi_MobileNo", MobileNo);
                        SqlParameter param5 = new SqlParameter("pi_CouponCode", Coupon);
                        SqlParameter param6 = new SqlParameter("pi_INDDatetime", indianTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param6);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];

                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            obj.ResponseCode = "00";
                            obj.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                           // DataTable dt2 = retVal.Tables[2];
                            obj.BurnCouponAmount = Convert.ToString(dt1.Rows[0]["CouponAmt"]);
                            if (Convert.ToBoolean(dt1.Rows[0]["AllowPointAccrual"]))
                                obj.AllowPointAccrual = 0;
                            else
                                obj.AllowPointAccrual = 1;

                            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["OfferCode"])))
                                obj.OfferCode = Convert.ToString(dt1.Rows[0]["OfferCode"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["MinRedemptionValue"])))
                                obj.MinVal = Convert.ToString(dt1.Rows[0]["MinRedemptionValue"]);
                            else
                                obj.MinVal = "0";

                            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["MaxRedemptionValue"])))
                                obj.MaxVal = Convert.ToString(dt1.Rows[0]["MaxRedemptionValue"]);
                            else
                                obj.MaxVal = "0";

                            //if (dt2.Rows.Count > 0)
                            //{
                            //    string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSWASendStatus"]);

                            //    //if (SMSStatus == "SMS")
                            //    //{
                            //    //    string _MobileNo = dt2.Rows[0]["MobileNo"].ToString();
                            //    //    string _MobileMessage = dt2.Rows[0]["SMSScript"].ToString();
                            //    //    string _UserName = dt2.Rows[0]["SMSLoginId"].ToString();
                            //    //    string _Password = dt2.Rows[0]["SMSPassword"].ToString();
                            //    //    string _Sender = dt2.Rows[0]["SMSSenderId"].ToString();
                            //    //    string _Url = dt2.Rows[0]["SMSUrl"].ToString();
                            //    //    //string _SMSBrandId = dt2.Rows[0]["SMSBrandId"].ToString();
                            //    //    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                            //    //    _job.Start();
                            //    //}

                            //    switch (SMSStatus)
                            //    {
                            //        case "SMS":
                            //            string _MobileNo = dt2.Rows[0]["MobileNo"].ToString();
                            //            string _MobileMessage = dt2.Rows[0]["SMSScript"].ToString();
                            //            string _UserName = dt2.Rows[0]["SMSLoginId"].ToString();
                            //            string _Password = dt2.Rows[0]["SMSPassword"].ToString();
                            //            string _Sender = dt2.Rows[0]["SMSSenderId"].ToString();
                            //            string _Url = dt2.Rows[0]["SMSUrl"].ToString();
                            //            string _SMSVendor = dt2.Rows[0]["SMSVendor"].ToString();
                            //            string _SMSScriptType = dt2.Rows[0]["SMSScriptType"].ToString();

                            //            Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSVendor, _SMSScriptType));
                            //            _job.Start();
                            //            break;
                            //        case "WA":
                            //            string _MobileNoWA = dt2.Rows[0]["MobileNo"].ToString();
                            //            string _MobileMessageWA = dt2.Rows[0]["SMSScript"].ToString();
                            //            string _WATokenId = dt2.Rows[0]["WhatsAppTokenId"].ToString();
                            //            string _WAUrl = dt2.Rows[0]["WhatsAppUrl"].ToString();

                            //            Thread _jobWA = new Thread(() => SendWA(_MobileNoWA, _MobileMessageWA, _WATokenId, _WAUrl));
                            //            _jobWA.Start();
                            //            break;
                            //        default:

                            //            string _MobileNoSMSWA = dt2.Rows[0]["MobileNo"].ToString();
                            //            string _MobileMessageSMS = dt2.Rows[0]["SMSScript"].ToString();
                            //            string _UserNameSMSWA = dt2.Rows[0]["SMSLoginId"].ToString();
                            //            string _PasswordSMSWA = dt2.Rows[0]["SMSPassword"].ToString();
                            //            string _SenderSMSWA = dt2.Rows[0]["SMSSenderId"].ToString();
                            //            string _UrlSMSWA = dt2.Rows[0]["SMSUrl"].ToString();
                            //            string _SMSAPIKey = dt2.Rows[0]["SMSAPIKey"].ToString();
                            //            string _SMSVendorSMSWA = dt2.Rows[0]["SMSVendor"].ToString();
                            //            string _SMSScriptTypeSMSWA = dt2.Rows[0]["SMSScriptType"].ToString();
                            //            string _MobileMessageWASMSWA = dt2.Rows[0]["WhatsAppScript"].ToString();
                            //            string _WATokenIdSMSWA = dt2.Rows[0]["WhatsAppTokenId"].ToString();
                            //            string _WAUrlSMSWA = dt2.Rows[0]["WhatsAppUrl"].ToString();
                            //            string _SMSTemplateId = string.Empty;
                            //            string _DisableSMS = string.Empty;


                            //            Thread _jobSMSWA = new Thread(() => SendSMSWA(_MobileNoSMSWA, _MobileMessageWASMSWA, _WATokenIdSMSWA, _WAUrlSMSWA, _MobileMessageSMS, _SMSTemplateId, _SMSScriptTypeSMSWA, _SMSVendorSMSWA, _UrlSMSWA, _UserNameSMSWA, _PasswordSMSWA, _SMSAPIKey, _DisableSMS, _SenderSMSWA, SMSStatus));
                            //            _jobSMSWA.Start();
                            //            break;
                            //    }
                            //}
                        }
                        else
                        {
                            obj.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            obj.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BurnValidation");
            }
            return obj;
        }

        public void SendSMS(string _MobileNo, string _SMSScript, string _SMSLoginId, string _SMSPassword, string _SMSSenderId, string _SMSUrl, string _SMSVendor, string _SMSScriptType)
        {
            string responseString;
            try
            {
                switch (_SMSVendor)
                {
                    case "TechnoCore":
                        if (_SMSScriptType == "Text")
                        {
                            string date_TechnoCoreText = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            _SMSScript = HttpUtility.UrlEncode(_SMSScript);
                            string type_TechnoCoreText = "text";
                            StringBuilder sbposdata_TechnoCoreText = new StringBuilder();
                            sbposdata_TechnoCoreText.AppendFormat("userid={0}", _SMSLoginId);
                            sbposdata_TechnoCoreText.AppendFormat("&password={0}", _SMSPassword);
                            sbposdata_TechnoCoreText.AppendFormat("&sendMethod={0}", "quick");
                            sbposdata_TechnoCoreText.AppendFormat("&mobile={0}", _MobileNo);
                            sbposdata_TechnoCoreText.AppendFormat("&msg={0}", _SMSScript);
                            sbposdata_TechnoCoreText.AppendFormat("&senderid={0}", _SMSSenderId);
                            sbposdata_TechnoCoreText.AppendFormat("&msgType={0}", type_TechnoCoreText);
                            sbposdata_TechnoCoreText.AppendFormat("&format={0}", type_TechnoCoreText);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_TechnoCoreText = (HttpWebRequest)WebRequest.Create(_SMSUrl);
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
                        }
                        else
                        {
                            string date_TechnoCoreUnicode = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            _SMSScript = HttpUtility.UrlEncode(_SMSScript);
                            string type_TechnoCoreUnicode = "unicode";
                            StringBuilder sbposdata_TechnoCoreUnicode = new StringBuilder();
                            sbposdata_TechnoCoreUnicode.AppendFormat("userid={0}", _SMSLoginId);
                            sbposdata_TechnoCoreUnicode.AppendFormat("&password={0}", _SMSPassword);
                            sbposdata_TechnoCoreUnicode.AppendFormat("&sendMethod={0}", "quick");
                            sbposdata_TechnoCoreUnicode.AppendFormat("&mobile={0}", _MobileNo);
                            sbposdata_TechnoCoreUnicode.AppendFormat("&msg={0}", _SMSScript);
                            sbposdata_TechnoCoreUnicode.AppendFormat("&senderid={0}", _SMSSenderId);
                            sbposdata_TechnoCoreUnicode.AppendFormat("&msgType={0}", type_TechnoCoreUnicode);
                            sbposdata_TechnoCoreUnicode.AppendFormat("&format={0}", type_TechnoCoreUnicode);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_TechnoCoreUnicode = (HttpWebRequest)WebRequest.Create(_SMSUrl);
                            UTF8Encoding encoding_TechnoCoreUnicode = new UTF8Encoding();
                            byte[] data_TechnoCoreUnicode = encoding_TechnoCoreUnicode.GetBytes(sbposdata_TechnoCoreUnicode.ToString());
                            httpWReq_TechnoCoreUnicode.Method = "POST";
                            httpWReq_TechnoCoreUnicode.ContentType = "application/x-www-form-urlencoded";
                            httpWReq_TechnoCoreUnicode.ContentLength = data_TechnoCoreUnicode.Length;
                            using (Stream stream_TechnoCoreUnicode = httpWReq_TechnoCoreUnicode.GetRequestStream())
                            {
                                stream_TechnoCoreUnicode.Write(data_TechnoCoreUnicode, 0, data_TechnoCoreUnicode.Length);
                            }
                            HttpWebResponse response_TechnoCoreUnicode = (HttpWebResponse)httpWReq_TechnoCoreUnicode.GetResponse();
                            StreamReader reader_TechnoCoreUnicode = new StreamReader(response_TechnoCoreUnicode.GetResponseStream());
                            string responseString_TechnoCoreUnicode = reader_TechnoCoreUnicode.ReadToEnd();
                            reader_TechnoCoreUnicode.Close();
                            response_TechnoCoreUnicode.Close();
                        }
                        break;
                    case "Vision":
                        if (_SMSScriptType == "Text")
                        {
                            var httpWebRequest_VisionText = (HttpWebRequest)WebRequest.Create(_SMSUrl);
                            httpWebRequest_VisionText.ContentType = "application/json";
                            httpWebRequest_VisionText.Method = "POST";
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            using (var streamWriter_VisionText = new StreamWriter(httpWebRequest_VisionText.GetRequestStream()))
                            {
                                string json_VisionText = "{\"Account\":" +
                                                "{\"APIKey\":\"" + _SMSPassword + "\"," +
                                                "\"SenderId\":\"" + _SMSSenderId + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"0\"," +
                                                "\"Route\":\"13\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"" + _MobileNo + "\"," +
                                                "\"Text\":\"" + _SMSScript + "\"}]" +
                                                "}";
                                streamWriter_VisionText.Write(json_VisionText);
                            }
                            var httpResponse_VisionText = (HttpWebResponse)httpWebRequest_VisionText.GetResponse();
                            using (var streamReader_VisionText = new StreamReader(httpResponse_VisionText.GetResponseStream()))
                            {
                                var result_VisionText = streamReader_VisionText.ReadToEnd();
                            }
                        }
                        else
                        {
                            var httpWebRequest_VisionUniCode = (HttpWebRequest)WebRequest.Create(_SMSUrl);
                            httpWebRequest_VisionUniCode.ContentType = "application/json";
                            httpWebRequest_VisionUniCode.Method = "POST";
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            using (var streamWriter_VisionUniCode = new StreamWriter(httpWebRequest_VisionUniCode.GetRequestStream()))
                            {
                                string json_VisionUniCode = "{\"Account\":" +
                                                "{\"APIKey\":\"" + _SMSPassword + "\"," +
                                                "\"SenderId\":\"" + _SMSSenderId + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"8\"," +
                                                "\"Route\":\"13\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"" + _MobileNo + "\"," +
                                                "\"Text\":\"" + _SMSScript + "\"}]" +
                                                "}";
                                streamWriter_VisionUniCode.Write(json_VisionUniCode);
                            }
                            var httpResponse_VisionUniCode = (HttpWebResponse)httpWebRequest_VisionUniCode.GetResponse();
                            using (var streamReader_VisionUniCode = new StreamReader(httpResponse_VisionUniCode.GetResponseStream()))
                            {
                                var result_VisionUniCode = streamReader_VisionUniCode.ReadToEnd();
                            }
                        }
                        break;
                    case "Pinnacle":
                        if (_SMSScriptType == "Text")
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            var httpWebRequest_PinnacleText = (HttpWebRequest)WebRequest.Create(_SMSUrl);
                            httpWebRequest_PinnacleText.ContentType = "application/json";
                            httpWebRequest_PinnacleText.Headers.Add("Apikey", _SMSPassword);
                            httpWebRequest_PinnacleText.Method = "POST";
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            using (var streamWriter_PinnacleText = new StreamWriter(httpWebRequest_PinnacleText.GetRequestStream()))
                            {
                                string json_PinnacleText = "{\"sender\":\"" + _SMSSenderId + "\"," +
                                "\"message\":[{\"number\":\"" + _MobileNo + "\"," +
                                 "\"text\":\"" + _SMSScript + "\"}]," + "\"messagetype\":\"TXT\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
                                streamWriter_PinnacleText.Write(json_PinnacleText);
                            }
                            var httpResponse_PinnacleText = (HttpWebResponse)httpWebRequest_PinnacleText.GetResponse();
                            using (var streamReader_PinnacleText = new StreamReader(httpResponse_PinnacleText.GetResponseStream()))
                            {
                                var result_PinnacleText = streamReader_PinnacleText.ReadToEnd();
                            }
                        }
                        else
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            var httpWebRequest_PinnacleUnicode = (HttpWebRequest)WebRequest.Create(_SMSUrl);
                            httpWebRequest_PinnacleUnicode.ContentType = "application/json";
                            httpWebRequest_PinnacleUnicode.Headers.Add("Apikey", _SMSPassword);
                            httpWebRequest_PinnacleUnicode.Method = "POST";
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            using (var streamWriter_PinnacleUnicode = new StreamWriter(httpWebRequest_PinnacleUnicode.GetRequestStream()))
                            {
                                string json_PinnacleUnicode = "{\"sender\":\"" + _SMSSenderId + "\"," +
                                "\"message\":[{\"number\":\"" + _MobileNo + "\"," +
                                 "\"text\":\"" + _SMSScript + "\"}]," + "\"messagetype\":\"UNI\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
                                streamWriter_PinnacleUnicode.Write(json_PinnacleUnicode);
                            }
                            var httpResponse_PinnacleUnicode = (HttpWebResponse)httpWebRequest_PinnacleUnicode.GetResponse();
                            using (var streamReader_PinnacleUnicode = new StreamReader(httpResponse_PinnacleUnicode.GetResponseStream()))
                            {
                                var result_PinnacleUnicode = streamReader_PinnacleUnicode.ReadToEnd();
                            }
                        }
                        break;
                    case "ValueFirst":
                        if (_SMSScriptType == "Text")
                        {
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            _SMSScript = HttpUtility.UrlEncode(_SMSScript);
                            string type_ValueFirstText = "TEXT";
                            StringBuilder sbposdata_ValueFirstText = new StringBuilder();
                            sbposdata_ValueFirstText.AppendFormat("username={0}", _SMSLoginId);
                            sbposdata_ValueFirstText.AppendFormat("&password={0}", _SMSPassword);
                            sbposdata_ValueFirstText.AppendFormat("&to={0}", _MobileNo);
                            sbposdata_ValueFirstText.AppendFormat("&from={0}", _SMSSenderId);
                            sbposdata_ValueFirstText.AppendFormat("&text={0}", _SMSScript);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_ValueFirstText = (HttpWebRequest)WebRequest.Create(_SMSUrl);
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
                        }
                        else
                        {
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            _SMSScript = HttpUtility.UrlEncode(_SMSScript);
                            string type_ValueFirstUnicode = "UNICODE";
                            StringBuilder sbposdata_ValueFirstUnicode = new StringBuilder();
                            sbposdata_ValueFirstUnicode.AppendFormat("username={0}", _SMSLoginId);
                            sbposdata_ValueFirstUnicode.AppendFormat("&password={0}", _SMSPassword);
                            sbposdata_ValueFirstUnicode.AppendFormat("&to={0}", _MobileNo);
                            sbposdata_ValueFirstUnicode.AppendFormat("&from={0}", _SMSSenderId);
                            sbposdata_ValueFirstUnicode.AppendFormat("&text={0}", _SMSScript);
                            sbposdata_ValueFirstUnicode.AppendFormat("&code={0}", "3");
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_ValueFirstUnicode = (HttpWebRequest)WebRequest.Create(_SMSUrl);
                            UTF8Encoding encoding_ValueFirstUnicode = new UTF8Encoding();
                            byte[] data_ValueFirstUnicode = encoding_ValueFirstUnicode.GetBytes(sbposdata_ValueFirstUnicode.ToString());
                            httpWReq_ValueFirstUnicode.Method = "POST";
                            httpWReq_ValueFirstUnicode.ContentType = "application/x-www-form-urlencoded";
                            httpWReq_ValueFirstUnicode.ContentLength = data_ValueFirstUnicode.Length;
                            using (Stream stream_ValueFirstUnicode = httpWReq_ValueFirstUnicode.GetRequestStream())
                            {
                                stream_ValueFirstUnicode.Write(data_ValueFirstUnicode, 0, data_ValueFirstUnicode.Length);
                            }
                            HttpWebResponse response_ValueFirstUnicode = (HttpWebResponse)httpWReq_ValueFirstUnicode.GetResponse();
                            StreamReader reader_ValueFirstUnicode = new StreamReader(response_ValueFirstUnicode.GetResponseStream());
                            string responseString_ValueFirstUnicode = reader_ValueFirstUnicode.ReadToEnd();
                            reader_ValueFirstUnicode.Close();
                            response_ValueFirstUnicode.Close();
                        }
                        break;
                }

            }
            catch (ArgumentException ex)
            {
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }

        public void SendWA(string _MobileNoWA, string _MobileMessageWA, string _WATokenId, string _WAUrl)
        {
            string responseString;
            try
            {
                _MobileMessageWA = _MobileMessageWA.Replace("#99", "&");
                _MobileMessageWA = HttpUtility.UrlEncode(_MobileMessageWA);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_WAUrl);
                sbposdata.AppendFormat("token={0}", _WATokenId);
                sbposdata.AppendFormat("&phone={0}", _MobileNoWA);
                sbposdata.AppendFormat("&message={0}", _MobileMessageWA);
                sbposdata.AppendFormat("&wacheck={0}", "true");
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
                reader.Close();
                response.Close();

            }
            catch (ArgumentException ex)
            {
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

        }

        public void SendSMSWA(string _MobileNo, string _WAMessage, string _WATokenId, string _WAUrl, string _SMSScript, string _SMSTemplateId, string _SMSScriptType, string _SMSVendor, string _SMSUrl, string _SMSLoginId, string _SMSPassword, string _SMSAPIKey, string _DisableSMS, string _SMSSenderId, string _SMSWASendStatus)
        {
            string responseString;
            try
            {
                _WAMessage = _WAMessage.Replace("#99", "&");
                _WAMessage = HttpUtility.UrlEncode(_WAMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_WAUrl);
                sbposdata.AppendFormat("token={0}", _WATokenId);
                sbposdata.AppendFormat("&phone={0}", _MobileNo);
                sbposdata.AppendFormat("&message={0}", _WAMessage);
                sbposdata.AppendFormat("&wacheck={0}", "true");
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
                
                reader.Close();
                response.Close();
            }
            catch (ArgumentException ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMS(_MobileNo, _SMSScript, _SMSLoginId, _SMSPassword, _SMSSenderId, _SMSUrl, _SMSVendor, _SMSScriptType));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
                }
            }
            catch (WebException ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMS(_MobileNo, _SMSScript, _SMSLoginId, _SMSPassword, _SMSSenderId, _SMSUrl, _SMSVendor, _SMSScriptType));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMS(_MobileNo, _SMSScript, _SMSLoginId, _SMSPassword, _SMSSenderId,_SMSUrl, _SMSVendor, _SMSScriptType));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                }
            }
        }

        public void SendTxnSMS(string _MobileNo, string _MobileMessage, string _SMSAPIKey, string _Password, string _Sender, string _Url)
        {
            var httpWebRequest_00003 = (HttpWebRequest)WebRequest.Create(_Url);
            httpWebRequest_00003.ContentType = "application/json";
            httpWebRequest_00003.Method = "POST";

            using (var streamWriter_00003 = new StreamWriter(httpWebRequest_00003.GetRequestStream()))
            {

                string json_00003 = "{\"Account\":" +
                                "{\"APIKey\":\"" + _SMSAPIKey + "\"," +
                                "\"SenderId\":\"" + _Sender + "\"," +
                                "\"Channel\":\"Trans\"," +
                                "\"DCS\":\"0\"," +
                                "\"SchedTime\":null," +
                                "\"GroupId\":null}," +
                                "\"Messages\":[{\"Number\":\"" + _MobileNo + "\"," +
                                "\"Text\":\"" + _MobileMessage + "\"}]" +
                                "}";
                streamWriter_00003.Write(json_00003);
            }

            var httpResponse_00003 = (HttpWebResponse)httpWebRequest_00003.GetResponse();
            using (var streamReader_00003 = new StreamReader(httpResponse_00003.GetResponseStream()))
            {
                var result_00003 = streamReader_00003.ReadToEnd();
            }
        }
    }
}
