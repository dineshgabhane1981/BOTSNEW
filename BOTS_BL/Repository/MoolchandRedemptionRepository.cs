using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models.RetailerWeb;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;

namespace BOTS_BL.Repository
{
    public class MoolchandRedemptionRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();

        public CustomerLoginDetail Authentication(LoginModel objLoginModel)
        {
            tblDatabaseDetail DBDetails = new tblDatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            try
            {
                string connstring = CR.GetCustomerConnString("1063");

                using (var context = new BOTSDBContext(connstring))
                {
                    //userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == objLoginModel.LoginId && a.Password == objLoginModel.Password && a.UserStatus.Value == true).FirstOrDefault();

                    var ObjLoginData = context.tblLoginDetails.Where(x=> x.LoginId == objLoginModel.LoginId  && x.Password == objLoginModel.Password && x.IsActive == true).FirstOrDefault();

                    userDetail.LoginId = ObjLoginData.LoginId;
                    userDetail.Password = ObjLoginData.Password;
                    userDetail.GroupId = ObjLoginData.GroupId;
                    userDetail.connectionString = connstring;

                    //if (userDetail != null)
                    //{
                    //    if (userDetail.GroupId != null)
                    //    {
                    //        using (var contextnew = new CommonDBContext())
                    //        {
                    //            DBDetails = contextnew.tblDatabaseDetails.Where(x => x.GroupId == userDetail.GroupId).FirstOrDefault();
                    //            //CustomerConnString.ConnectionStringCustomer = DBDetails.DBName;

                    //            userDetail.connectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    //        }



                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");

            }
            return userDetail;

        }
        public CustomerDetails GetCustomerDetails(string LoginId,string GroupId, string MobileNo)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            CustomerDetails objData = new CustomerDetails();
            objData.ResponseCode = "01";
            objData.ResponseMessage = "Fail";

            string connstring = CR.GetCustomerConnString(GroupId);


            //string strToday = indianTime.ToString("yyyy-MM-dd");
            try
            {
                using (var context = new BOTSDBContext(connstring))
                {
                    var ObjCustInfo = context.tblCustInfoes.Where(x => x.MobileNo == MobileNo).FirstOrDefault();                   

                    if(ObjCustInfo != null)
                    {
                        var ObjTblCustTxnSummary = context.tblCustTxnSummaryMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                        var LstObj = context.tblCustPointsMasters.Where(x => x.MobileNo == MobileNo && x.StartDate <= indianTime && x.EndDate >= indianTime).ToList();
                        var Points = LstObj.Select(x => x.Points).Sum();
                        var ObjtblCustDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                        objData.CustomerName = ObjCustInfo.Name;
                        objData.MobileNo = ObjCustInfo.MobileNo;
                        if (ObjTblCustTxnSummary != null)
                        {                     
                            objData.TotalSpend = Convert.ToString(ObjTblCustTxnSummary.TotalSpend);
                            objData.LastTxnDate = Convert.ToString(ObjTblCustTxnSummary.LastTxnDate);
                        }
                        
                        objData.PointBalance = Convert.ToString(Points);
                        objData.CardNo = ObjtblCustDetails.CardNo;
                        objData.ResponseCode = "00";
                        objData.ResponseMessage = "Success";
                    }                 
                    
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetailsMoolchandBurnPage");
            }
            
                return objData;
        }

        public BurnValidationResponse BurnValidation(string LoginId,string GroupId, string MobileNo, string InvoiceNo, string InvoiceAmt, string PointsBurn)
        {
            BurnValidationResponse objResponse = new BurnValidationResponse();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            var constr = CR.GetCustomerConnString(GroupId);
            try 
            {
                using (var context = new BOTSDBContext(constr))
                {
                    SqlConnection _Con = new SqlConnection(constr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_BillingBurnValidationWeb", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", LoginId);
                        SqlParameter param2 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param4 = new SqlParameter("pi_MobileNo", MobileNo);
                        SqlParameter param5 = new SqlParameter("pi_BurnPoints", PointsBurn);
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
                            objResponse.ResponseCode = "00";
                            objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            objResponse.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                            objResponse.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["PointsAsAmt"]);
                            objResponse.PointsValue = Convert.ToString(dt1.Rows[0]["PointsAsAmt"]);
                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSWASendStatus"]);
                                
                                if(SMSStatus == "SMS")
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
                                //else
                                //{

                                //}

                            }
                        }

                        else
                        {
                            objResponse.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                        }
                    }


                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "BurnValidation Moolchand Burn Page");
            }

            return objResponse;
        }

        public BurnResponse SaveBurnTxn(string LoginId,string GroupId, string Mobileno, string InvoiceNo, string InvoiceAmt, string PointsBurn)
        {
            BurnResponse R = new BurnResponse();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            try
            {
                using (var context = new CommonDBContext())
                {
                    //string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetCustomerConnString(GroupId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_BillingBurnWeb", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", LoginId);
                        SqlParameter param2 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);                        
                        SqlParameter param4 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param5 = new SqlParameter("pi_BurnPoints", PointsBurn);
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
                            R.ResponseCode = "00";
                            R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);
                            R.PointsRedeemed = Convert.ToString(dt1.Rows[0]["PointsBurned"]);
                            R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);


                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSWASendStatus"]);

                                if (SMSStatus == "SMS")
                                {
                                    string _MobileNo = dt2.Rows[0]["MobileNo"].ToString();
                                    string _MobileMessage = dt2.Rows[0]["SMSScript"].ToString();
                                    string _UserName = dt2.Rows[0]["SMSLoginId"].ToString();
                                    string _SMSAPIKey = dt2.Rows[0]["SMSAPIKey"].ToString();
                                    string _Password = dt2.Rows[0]["SMSPassword"].ToString();
                                    string _Sender = dt2.Rows[0]["SMSSenderId"].ToString();
                                    string _Url = dt2.Rows[0]["SMSUrl"].ToString();
                                    //string _SMSBrandId = dt2.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendTxnSMS(_MobileNo, _MobileMessage, _SMSAPIKey, _Password, _Sender, _Url));
                                    _job.Start();
                                }
                            }
                        }
                        else
                        {
                            R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBurnTxn");
            }
            return R;
        }

        public BurnValidationResponse OTPResend(string LoginId, string GroupId, string MobileNo, string InvoiceNo, string InvoiceAmt, string PointsBurn)
        {
            BurnValidationResponse objResponse = new BurnValidationResponse();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            var constr = CR.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(constr))
                {
                    SqlConnection _Con = new SqlConnection(constr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_BillingBurnValidationWeb", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", LoginId);
                        SqlParameter param2 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param4 = new SqlParameter("pi_MobileNo", MobileNo);
                        SqlParameter param5 = new SqlParameter("pi_BurnPoints", PointsBurn);
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
                            objResponse.ResponseCode = "00";
                            objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            objResponse.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                            objResponse.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["PointsAsAmt"]);
                            objResponse.PointsValue = Convert.ToString(dt1.Rows[0]["PointsAsAmt"]);
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
                                //else
                                //{

                                //}

                            }
                        }

                        else
                        {
                            objResponse.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BurnValidation Moolchand Burn Page");
            }

            return objResponse;
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
