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
        public  GinesysRedeemModel GetCustomerDetails(GinesysRedeemModel objData)
        {            
            string groupId = objData.StoreId.Substring(0, 4);
            string connStr = GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    var BlockedPoints = context.tblBurnPtsSoftBlocks.Where(x => x.MobileNo == objData.MobileNo && x.IsActive == true).Sum(y=>y.BurnPoints);

                    objData.Points  = context.tblCustPointsMasters.Where(x => x.MobileNo == objData.MobileNo && x.IsActive == true).Sum(y => y.Points);
                    if (BlockedPoints != null)
                    {
                        objData.Points = objData.Points - BlockedPoints;
                    }
                    objData.PointsValue = context.tblRuleMasters.Select(x => x.PointsAllocation).FirstOrDefault();
                    objData.CustomerName = context.tblCustDetailsMasters.Where(x => x.MobileNo == objData.MobileNo).Select(y => y.Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetails");
            }
            return objData;
        } 

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

                                if (SMSStatus == "SMS")
                                {
                                    string _MobileNo = dt2.Rows[0]["MobileNo"].ToString();
                                    string _MobileMessage = dt2.Rows[0]["SMSScript"].ToString();
                                    string _UserName = dt2.Rows[0]["SMSLoginId"].ToString();
                                    string _Password = dt2.Rows[0]["SMSPassword"].ToString();
                                    string _Sender = dt2.Rows[0]["SMSSenderId"].ToString();
                                    string _Url = dt2.Rows[0]["SMSUrl"].ToString();
                                    //string _SMSBrandId = dt2.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                                    _job.Start();
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

        public void SendSMS(string _MobileNo, string _MobileMessage, string _UserName, string _Password, string _Sender, string _Url)
        {
            string date_00000 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            _MobileMessage = _MobileMessage.Replace("#99", "&");
            _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
            string type_00000 = "text";
            StringBuilder sbposdata_00000 = new StringBuilder();
            sbposdata_00000.AppendFormat("userid={0}", _UserName);
            sbposdata_00000.AppendFormat("&password={0}", _Password);
            sbposdata_00000.AppendFormat("&sendMethod={0}", "quick");
            sbposdata_00000.AppendFormat("&mobile={0}", _MobileNo);
            sbposdata_00000.AppendFormat("&msg={0}", _MobileMessage);
            sbposdata_00000.AppendFormat("&senderid={0}", _Sender);
            sbposdata_00000.AppendFormat("&msgType={0}", type_00000);
            sbposdata_00000.AppendFormat("&format={0}", type_00000);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest httpWReq_00000 = (HttpWebRequest)WebRequest.Create(_Url);
            UTF8Encoding encoding_00000 = new UTF8Encoding();
            byte[] data_00000 = encoding_00000.GetBytes(sbposdata_00000.ToString());
            httpWReq_00000.Method = "POST";
            httpWReq_00000.ContentType = "application/x-www-form-urlencoded";
            httpWReq_00000.ContentLength = data_00000.Length;
            using (Stream stream_00000 = httpWReq_00000.GetRequestStream())
            {
                stream_00000.Write(data_00000, 0, data_00000.Length);
            }
            HttpWebResponse response_00000 = (HttpWebResponse)httpWReq_00000.GetResponse();
            StreamReader reader_00000 = new StreamReader(response_00000.GetResponseStream());
            string responseString_00000 = reader_00000.ReadToEnd();
            reader_00000.Close();
            response_00000.Close();
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
