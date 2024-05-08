using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BOTS_BL.Models;

namespace BOTS_BL.Repository
{
    public class RatnaEnterprisesRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        string Stringdtmessage;
        


        public CustomerLoginDetail AuthenticateUser(CustomerLoginDetail objData)
        {
            //DatabaseDetail DBDetails = new DatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //userDetail = context.LoginDetails.Where(a => a.LoginId == objData.LoginId && a.Password == objData.Password).FirstOrDefault();
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == objData.LoginId && a.Password == objData.Password).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");
            }
            return userDetail;

        }
        public RetailBulkResponse BulkTransaction(DataTable dt, string CounterId)
        {
            RetailBulkResponse Obj = new RetailBulkResponse();
            int c = 0;
            Obj.Status = false;
            string MobileNo, StrMobile, TGroupId;
            int Status;
            CultureInfo culture = new CultureInfo("en-IN");
            TGroupId = CounterId.Substring(0, 4);

            try
            {
                Obj.TbleRWCount = Convert.ToString(dt.Rows.Count);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt.Rows[i]["Mobile"]) != "" && Convert.ToString(dt.Rows[i]["Mobile"]) != "Mobile" && i > 0)
                        {
                            
                        var conStr = CR.GetRetailWebConnString(CounterId);
                        using (var context = new BOTSDBContext(conStr))
                        {
                            SqlConnection _Con = new SqlConnection(conStr);
                            DataSet retVal = new DataSet();
                            SqlCommand cmdReport = new SqlCommand("sp_Web_MembershipEarn", _Con);
                            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                            using (cmdReport)
                            {
                                SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                                SqlParameter param2 = new SqlParameter("pi_Date", Convert.ToString(dt.Rows[i]["Date"]));
                                SqlParameter param3 = new SqlParameter("pi_InvoiceNo", Convert.ToString(dt.Rows[i]["Invoice Number"]));
                                SqlParameter param4 = new SqlParameter("pi_Name", dt.Rows[i]["Name"]);
                                StrMobile = Convert.ToString(dt.Rows[i]["Mobile"]);
                                MobileNo = StrMobile;
                                if (StrMobile.Length == 12)
                                {
                                    MobileNo = MobileNo.Substring(2, 10);
                                }

                                SqlParameter param5 = new SqlParameter("pi_MobileNo", MobileNo);
                                SqlParameter param6 = new SqlParameter("pi_InvoiceAmt", dt.Rows[i]["Total"]);
                                SqlParameter param7 = new SqlParameter("pi_Payment", dt.Rows[i]["Payment"]);
                                SqlParameter param8 = new SqlParameter("pi_Redemption", dt.Rows[i]["Redemption"]);

                                cmdReport.CommandType = CommandType.StoredProcedure;
                                cmdReport.Parameters.Add(param1);
                                cmdReport.Parameters.Add(param2);
                                cmdReport.Parameters.Add(param3);
                                cmdReport.Parameters.Add(param4);
                                cmdReport.Parameters.Add(param5);
                                cmdReport.Parameters.Add(param6);
                                cmdReport.Parameters.Add(param7);
                                cmdReport.Parameters.Add(param8);

                                daReport.Fill(retVal);
                                DataTable Data = retVal.Tables[0];

                                Obj.Status = true;

                                if (Convert.ToString(Data.Rows[0]["ResponseCode"]) == "00")
                                {
                                    c++;
                                    DataTable Data1 = retVal.Tables[1];
                                    DataTable Data2 = retVal.Tables[2];

                                    Thread _JobMessage = new Thread(() => MessageDataTable(Data2));
                                    _JobMessage.Start();
                                }
                            }
                        }
                        }
                    }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BulkInsert");
            }
            Obj.DBInsertCount = Convert.ToString(c);
            Obj.DBFailedCount = Convert.ToString(c);
            return Obj;
        }

        public void MessageDataTable(DataTable _dtmessage)
        {
            try
            {
                if (_dtmessage.Rows.Count > 0)
                {
                    string SMSScript, SMSSenderId, WhatsAppScript, OutletId, SMSTemplateId, SMSScriptType, WhatsAppScriptType, SMSVendor, SMSUrl, SMSLoginId, SMSPassword, SMSAPIKey, WhatsAppVendor, WhatsAppUrl, WhatsAppTokenId, IsActiveWhatsApp, IsActiveSMS, VerifiedWhatsAppUrl, VerifiedWhatsAppLoginId, VerifiedWhatsAppPassword, VerifiedWhatsAppAPIKey, DisableCustSMSWA, WhatsAppMessageType, MobileNo, WhatsAppImgUrl, SMSWASendStatus, WhatsAppUserName, WhatsAppPassword;
                    for (int i = 0; i < _dtmessage.Rows.Count; i++)
                    {
                        MobileNo = _dtmessage.Rows[i]["MobileNo"].ToString();
                        SMSScript = _dtmessage.Rows[i]["SMSScript"].ToString();
                        SMSSenderId = _dtmessage.Rows[i]["SMSSenderId"].ToString();
                        WhatsAppScript = _dtmessage.Rows[i]["WhatsAppScript"].ToString();
                        OutletId = _dtmessage.Rows[i]["OutletId"].ToString();
                        SMSTemplateId = _dtmessage.Rows[i]["SMSTemplateId"].ToString();
                        SMSScriptType = _dtmessage.Rows[i]["SMSScriptType"].ToString();
                        WhatsAppScriptType = _dtmessage.Rows[i]["WhatsAppScriptType"].ToString();
                        SMSVendor = _dtmessage.Rows[i]["SMSVendor"].ToString();
                        SMSUrl = _dtmessage.Rows[i]["SMSUrl"].ToString();
                        SMSLoginId = _dtmessage.Rows[i]["SMSLoginId"].ToString();
                        SMSPassword = _dtmessage.Rows[i]["SMSPassword"].ToString();
                        SMSAPIKey = _dtmessage.Rows[i]["SMSAPIKey"].ToString();
                        WhatsAppVendor = _dtmessage.Rows[i]["WhatsAppVendor"].ToString();
                        WhatsAppUrl = _dtmessage.Rows[i]["WhatsAppUrl"].ToString();
                        WhatsAppTokenId = _dtmessage.Rows[i]["WhatsAppTokenId"].ToString();
                        IsActiveWhatsApp = _dtmessage.Rows[i]["IsActiveWhatsApp"].ToString();
                        IsActiveSMS = _dtmessage.Rows[i]["IsActiveSMS"].ToString();
                        VerifiedWhatsAppUrl = _dtmessage.Rows[i]["VerifiedWhatsAppUrl"].ToString();
                        VerifiedWhatsAppLoginId = _dtmessage.Rows[i]["VerifiedWhatsAppLoginId"].ToString();
                        VerifiedWhatsAppPassword = _dtmessage.Rows[i]["VerifiedWhatsAppPassword"].ToString();
                        VerifiedWhatsAppAPIKey = _dtmessage.Rows[i]["VerifiedWhatsAppAPIKey"].ToString();
                        WhatsAppMessageType = _dtmessage.Rows[i]["WhatsAppMessageType"].ToString();
                        SMSWASendStatus = _dtmessage.Rows[i]["SMSWASendStatus"].ToString();

                        if (IsActiveWhatsApp == "1")
                        {
                            if (SMSWASendStatus == "Both" || SMSWASendStatus == "WA")
                            {
                                switch (WhatsAppVendor)
                                {
                                    case "TechnoCore":
                                        if (WhatsAppMessageType == "Text")
                                        {
                                            SendWAText_TechnoCore(MobileNo, WhatsAppScript, WhatsAppTokenId, WhatsAppUrl, SMSScript, SMSTemplateId, SMSScriptType, SMSVendor, SMSUrl, SMSLoginId, SMSPassword, SMSAPIKey, IsActiveSMS, SMSSenderId, SMSWASendStatus);
                                        }
                                        else if (WhatsAppMessageType == "TextWithImage")
                                        {
                                            WhatsAppImgUrl = _dtmessage.Rows[i]["WhatsAppImgUrl"].ToString();
                                            SendWATextWithImage_TechnoCore(MobileNo, WhatsAppScript, WhatsAppTokenId, WhatsAppUrl, SMSScript, SMSTemplateId, SMSScriptType, SMSVendor, SMSUrl, SMSLoginId, SMSPassword, SMSAPIKey, IsActiveSMS, SMSSenderId, WhatsAppImgUrl, SMSWASendStatus);
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                SendSMSMessage(MobileNo, SMSScript, SMSTemplateId, SMSScriptType, SMSVendor, SMSUrl, SMSLoginId, SMSPassword, SMSAPIKey, IsActiveSMS, SMSSenderId);
                            }
                        }
                        else if (IsActiveSMS == "1")
                        {
                            SendSMSMessage(MobileNo, SMSScript, SMSTemplateId, SMSScriptType, SMSVendor, SMSUrl, SMSLoginId, SMSPassword, SMSAPIKey, IsActiveSMS, SMSSenderId);
                        }

                    }
                }
            }
            catch (ArgumentException ex)
            {
                Stringdtmessage = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Stringdtmessage = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Stringdtmessage = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }

        public void SendWAText_TechnoCore(string _MobileNo, string _WAMessage, string _WATokenId, string _WAUrl, string _SMSScript, string _SMSTemplateId, string _SMSScriptType, string _SMSVendor, string _SMSUrl, string _SMSLoginId, string _SMSPassword, string _SMSAPIKey, string _DisableSMS, string _SMSSenderId, string _SMSWASendStatus)
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
                    Thread _job = new Thread(() => SendSMSMessage(_MobileNo, _SMSScript, _SMSTemplateId, _SMSScriptType, _SMSVendor, _SMSUrl, _SMSLoginId, _SMSPassword, _SMSAPIKey, _DisableSMS, _SMSSenderId));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
                }
            }
            catch (WebException ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMSMessage(_MobileNo, _SMSScript, _SMSTemplateId, _SMSScriptType, _SMSVendor, _SMSUrl, _SMSLoginId, _SMSPassword, _SMSAPIKey, _DisableSMS, _SMSSenderId));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMSMessage(_MobileNo, _SMSScript, _SMSTemplateId, _SMSScriptType, _SMSVendor, _SMSUrl, _SMSLoginId, _SMSPassword, _SMSAPIKey, _DisableSMS, _SMSSenderId));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                }
            }
        }

        public void SendWATextWithImage_TechnoCore(string _MobileNo, string _WAMessage, string _WATokenId, string _WAUrl, string _SMSScript, string _SMSTemplateId, string _SMSScriptType, string _SMSVendor, string _SMSUrl, string _SMSLoginId, string _SMSPassword, string _SMSAPIKey, string _DisableSMS, string _SMSSenderId, string _WhatsAppImgUrl, string _SMSWASendStatus)
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
                sbposdata.AppendFormat("&link={0}", _WhatsAppImgUrl);
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
                //var J = JObject.Parse(responseString);
                //string J1 = J["status"].ToString();
                //if (J1 == "error")
                //{
                //    Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _CounterId));
                //    _job.Start();
                //}
                reader.Close();
                response.Close();
            }
            catch (ArgumentException ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMSMessage(_MobileNo, _SMSScript, _SMSTemplateId, _SMSScriptType, _SMSVendor, _SMSUrl, _SMSLoginId, _SMSPassword, _SMSAPIKey, _DisableSMS, _SMSSenderId));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
                }
            }
            catch (WebException ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMSMessage(_MobileNo, _SMSScript, _SMSTemplateId, _SMSScriptType, _SMSVendor, _SMSUrl, _SMSLoginId, _SMSPassword, _SMSAPIKey, _DisableSMS, _SMSSenderId));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (_SMSWASendStatus == "Both")
                {
                    Thread _job = new Thread(() => SendSMSMessage(_MobileNo, _SMSScript, _SMSTemplateId, _SMSScriptType, _SMSVendor, _SMSUrl, _SMSLoginId, _SMSPassword, _SMSAPIKey, _DisableSMS, _SMSSenderId));
                    _job.Start();
                    responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                }
            }
        }

        public void SendSMSMessage(string _MobileNo, string _SMSScript, string _SMSTemplateId, string _SMSScriptType, string _SMSVendor, string _SMSUrl, string _SMSLoginId, string _SMSPassword, string _SMSAPIKey, string _DisableSMS, string _SMSSenderId)
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
                                                "{\"APIKey\":\"" + _SMSAPIKey + "\"," +
                                                "\"SenderId\":\"" + _SMSSenderId + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"0\"," +
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
                                                "{\"APIKey\":\"" + _SMSAPIKey + "\"," +
                                                "\"SenderId\":\"" + _SMSSenderId + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"8\"," +
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
                    case "ThirdParty":
                        if (_SMSScriptType == "Text")
                        {
                            _SMSScript = _SMSScript.Replace("#99", "&");
                            _SMSScript = HttpUtility.UrlEncode(_SMSScript);
                            string type3 = "TEXT";
                            StringBuilder sbposdata3 = new StringBuilder();
                            sbposdata3.AppendFormat("username={0}", _SMSLoginId);
                            sbposdata3.AppendFormat("&pass={0}", _SMSPassword);
                            sbposdata3.AppendFormat("&route={0}", "trans1");
                            sbposdata3.AppendFormat("&senderid={0}", _SMSSenderId);
                            sbposdata3.AppendFormat("&numbers={0}", _MobileNo);
                            sbposdata3.AppendFormat("&message={0}", _SMSScript);
                            HttpWebRequest httpWReq3 = (HttpWebRequest)WebRequest.Create(_SMSUrl);
                            UTF8Encoding encoding3 = new UTF8Encoding();
                            byte[] data3 = encoding3.GetBytes(sbposdata3.ToString());
                            httpWReq3.Method = "POST";
                            httpWReq3.ContentType = "application/x-www-form-urlencoded";
                            httpWReq3.ContentLength = data3.Length;
                            using (Stream stream = httpWReq3.GetRequestStream())
                            {
                                stream.Write(data3, 0, data3.Length);
                            }
                            HttpWebResponse response3 = (HttpWebResponse)httpWReq3.GetResponse();
                            StreamReader reader3 = new StreamReader(response3.GetResponseStream());
                            string responseString3 = reader3.ReadToEnd();
                            reader3.Close();
                            response3.Close();
                            break;
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


    }
}
