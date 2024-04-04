using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.RetailerWeb;
using BOTS_BL.Models;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Globalization;

namespace BOTS_BL.Repository
{
    public class RetailerWebRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();

        public List<DynamicFieldInfo>[] DynamicData(string connectionString)
        {
            DynamicFieldInfo Obj = new DynamicFieldInfo();

            List<DynamicFieldInfo> objList2 = new List<DynamicFieldInfo>();
            List<DynamicFieldInfo>[] Obj1 = new List<DynamicFieldInfo>[1000];

            JSONDATA J = new JSONDATA();

            SqlConnection _Con = new SqlConnection(connectionString);
            DataSet retVal = new DataSet();
            DataTable Tbl = new DataTable();
            SqlCommand cmdReport = new SqlCommand("sp_DynamicDataJSON", _Con);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
            try
            {
                using (cmdReport)
                {
                    SqlParameter param1 = new SqlParameter("pi_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);

                    daReport.Fill(retVal);

                    Tbl = retVal.Tables[0];
                    var JsonData = Tbl.Rows[0]["JSONData"];

                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    json_serializer.MaxJsonLength = int.MaxValue;
                    object[] objOutletData = (object[])json_serializer.DeserializeObject((string)JsonData);

                    List<DynamicFieldInfo> objListDymanic = new List<DynamicFieldInfo>();

                    int i = 1;
                    foreach (Dictionary<string, object> item in objOutletData)
                    {
                        DynamicFieldInfo DymFld = new DynamicFieldInfo();
                        DymFld.Fieldid = Convert.ToString(item["Fieldid"]);
                        DymFld.FieldOptionId = Convert.ToString(item["FieldOptionId"]);
                        DymFld.FieldTypeId = Convert.ToString(item["FieldTypeId"]);
                        DymFld.FieldValue = Convert.ToString(item["FieldValue"]);
                        DymFld.FieldName = Convert.ToString(item["FieldName"]);

                        objListDymanic.Add(DymFld);
                    }

                    int C = objListDymanic.GroupBy(data => data.Fieldid).Count();

                    var query = new DynamicFieldInfo[C];

                    List<DynamicFieldInfo> objList1 = new List<DynamicFieldInfo>();

                    var distinctFieldId = objListDymanic?.Select(o => o.Fieldid).Distinct();
                    List<DynamicFieldInfo>[] dynamicFieldInfos = new List<DynamicFieldInfo>[distinctFieldId.Count()];
                    if (distinctFieldId != null && distinctFieldId.Any())
                    {
                        foreach (var fieldid in distinctFieldId)
                        {
                            dynamicFieldInfos[Convert.ToInt16(fieldid) - 1] = objListDymanic?.Where(data => data.Fieldid == fieldid).ToList();
                        }
                    }
                    Obj1 = dynamicFieldInfos;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DynamicData");
            }
            return Obj1;
        }
        public List<DynamicFieldInfo>[] DynamicCustData(string connectionString)
        {
            DynamicFieldInfo Obj = new DynamicFieldInfo();

            List<DynamicFieldInfo> objList2 = new List<DynamicFieldInfo>();
            List<DynamicFieldInfo>[] Obj1 = new List<DynamicFieldInfo>[1000];

            JSONDATA J = new JSONDATA();

            SqlConnection _Con = new SqlConnection(connectionString);
            DataSet retVal = new DataSet();
            DataTable Tbl = new DataTable();
            SqlCommand cmdReport = new SqlCommand("sp_DynamicCustomerDataJSON", _Con);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
            try
            {
                using (cmdReport)
                {
                    SqlParameter param1 = new SqlParameter("pi_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);

                    daReport.Fill(retVal);

                    Tbl = retVal.Tables[0];
                    var JsonData = Tbl.Rows[0]["JSONData"];

                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    json_serializer.MaxJsonLength = int.MaxValue;
                    object[] objOutletData = (object[])json_serializer.DeserializeObject((string)JsonData);

                    List<DynamicFieldInfo> objListDymanic = new List<DynamicFieldInfo>();

                    int i = 1;
                    foreach (Dictionary<string, object> item in objOutletData)
                    {
                        DynamicFieldInfo DymFld = new DynamicFieldInfo();
                        DymFld.Fieldid = Convert.ToString(item["Fieldid"]);
                        DymFld.FieldOptionId = Convert.ToString(item["FieldOptionId"]);
                        DymFld.FieldTypeId = Convert.ToString(item["FieldTypeId"]);
                        DymFld.FieldValue = Convert.ToString(item["FieldValue"]);
                        DymFld.FieldName = Convert.ToString(item["FieldName"]);

                        objListDymanic.Add(DymFld);
                    }

                    int C = objListDymanic.GroupBy(data => data.Fieldid).Count();

                    var query = new DynamicFieldInfo[C];

                    List<DynamicFieldInfo> objList1 = new List<DynamicFieldInfo>();

                    var distinctFieldId = objListDymanic?.Select(o => o.Fieldid).Distinct();
                    List<DynamicFieldInfo>[] dynamicFieldInfos = new List<DynamicFieldInfo>[distinctFieldId.Count()];
                    if (distinctFieldId != null && distinctFieldId.Any())
                    {
                        foreach (var fieldid in distinctFieldId)
                        {
                            dynamicFieldInfos[Convert.ToInt16(fieldid) - 1] = objListDymanic?.Where(data => data.Fieldid == fieldid).ToList();
                        }
                    }
                    Obj1 = dynamicFieldInfos;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DynamicCustData");
            }
            return Obj1;
        }
        public CustomerDetails GetCustomerDetails(string CounterId, string MobileNo)
        {
            CustomerDetails objData = new CustomerDetails();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    //var conStr = CR.GetCustomerConnString(groupId);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_SearchCustomer", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        SqlParameter param2 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param3 = new SqlParameter("pi_SearchData", MobileNo);
                        SqlParameter param4 = new SqlParameter("pi_SearchType", "1");
                        SqlParameter param1 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];

                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            objData.ResponseCode = "00";
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            //DataTable dt3 = retVal.Tables[3];
                            objData.MobileNo = Convert.ToString(dt2.Rows[0]["MobileNo"]);
                            objData.CustomerName = Convert.ToString(dt2.Rows[0]["CustomerName"]);
                            objData.PointBalance = Convert.ToString(dt2.Rows[0]["AvailablePoints"]);

                            objData.CardNo = Convert.ToString(dt1.Rows[0]["LoyaltyCard"]);
                            objData.TotalSpend = Convert.ToString(dt1.Rows[0]["TotalSpendText"]);
                            objData.LastTxnDate = Convert.ToString(dt1.Rows[0]["LastTxnText"]);
                        }

                        else
                        {
                            objData.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            objData.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetails");
            }
            return objData;
        }

        public EarnResponse InsertEarnData(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt, string DynamicData,string DynamicCustData)
        {
            EarnResponse R = new EarnResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_Earn", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param2 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param4 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        SqlParameter param6 = new SqlParameter("pi_json", DynamicData);
                        SqlParameter param7 = new SqlParameter("pi_json1", DynamicCustData);

                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param6);
                        cmdReport.Parameters.Add(param7);

                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            R.ResponseCode = "00";
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            R.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                            R.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                            R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                            R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);

                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusTxn"]);
                                string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusTxn"]);                               

                                if (SMSStatus == "1" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];
                                    
                                    if(groupId == "1007")
                                    {
                                        Thread _job1 = new Thread(() => SendBOTSVideo(dt3));
                                        _job1.Start();
                                    }
                                    else
                                    {
                                        Thread _job = new Thread(() => SendSMSandWA(dt3));
                                        _job.Start();
                                    }                                  
                                }
                                else if (SMSStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
                                    string _MobileMessage = dt3.Rows[0]["MessageTxn"].ToString();
                                    string _UserName = dt3.Rows[0]["UserNameTxn"].ToString();
                                    string _Password = dt3.Rows[0]["PasswordTxn"].ToString();
                                    string _Sender = dt3.Rows[0]["SenderIdTxn"].ToString();
                                    string _Url = dt3.Rows[0]["UrlTxn"].ToString();
                                    string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                    _job.Start();
                                }
                                else if (WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];
                                    
                                    if (groupId == "1007")
                                    {
                                        Thread _job1 = new Thread(() => SendBOTSVideo(dt3));
                                        _job1.Start();
                                    }
                                    else
                                    {
                                        Thread _job = new Thread(() => SendWAMessage(dt3));
                                        _job.Start();

                                    }
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
                newexception.AddException(ex, "InsertEarnData");
            }
            return R;
        }

        public EarnResponse InsertEarnDataOld(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt, string DynamicData)
        {
            EarnResponse R = new EarnResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_EarnOld", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param2 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param4 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        SqlParameter param6 = new SqlParameter("pi_json", DynamicData);

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
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            R.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                            R.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                            R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                            R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);

                            

                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusTxn"]);
                                string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusTxn"]);

                                if (SMSStatus == "1" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendSMSandWA(dt3));
                                    _job.Start();
                                }
                                else if (SMSStatus == "1" && WAStatus == "0")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
                                    string _MobileMessage = dt3.Rows[0]["MessageTxn"].ToString();
                                    string _UserName = dt3.Rows[0]["UserNameTxn"].ToString();
                                    string _Password = dt3.Rows[0]["PasswordTxn"].ToString();
                                    string _Sender = dt3.Rows[0]["SenderIdTxn"].ToString();
                                    string _Url = dt3.Rows[0]["UrlTxn"].ToString();
                                    string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                    _job.Start();
                                }
                                else if (SMSStatus == "0" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendWAMessage(dt3));
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
                newexception.AddException(ex, "InsertEarnDataOld");
            }
            return R;
        }

        public BurnValidationResponse BurnValidation(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt, string PointsBurn,string DynamicData)
        {
            BurnValidationResponse R = new BurnValidationResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_BurnValidation", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param2 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param4 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param5 = new SqlParameter("pi_BurnPoints", PointsBurn);
                        SqlParameter param6 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        SqlParameter param7 = new SqlParameter("pi_jsondata", DynamicData);


                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param6);
                        cmdReport.Parameters.Add(param7);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            R.ResponseCode = "00";
                            R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            R.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                            R.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["BurnPointsAsAmount"]);
                            R.PointsValue = Convert.ToString(dt1.Rows[0]["PointsValue"]);

                            

                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusOTP"]);
                                string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusOTP"]);

                                if (SMSStatus == "1" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendSMSandWA(dt3));
                                    _job.Start();
                                }
                                else if (SMSStatus == "1" && WAStatus == "0")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    string _MobileNo = dt3.Rows[0]["CommMobileNoOTP"].ToString();
                                    string _MobileMessage = dt3.Rows[0]["MessageOTP"].ToString();
                                    string _UserName = dt3.Rows[0]["UserNameOTP"].ToString();
                                    string _Password = dt3.Rows[0]["PasswordOTP"].ToString();
                                    string _Sender = dt3.Rows[0]["SenderIdOTP"].ToString();
                                    string _Url = dt3.Rows[0]["UrlOTP"].ToString();
                                    string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                    _job.Start();
                                }
                                else if (SMSStatus == "0" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendWAMessage(dt3));
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
                newexception.AddException(ex, "BurnValidation");
            }
            return R;
        }

        public BurnResponse SaveBurnTxn(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt, string PointsBurn, string DynamicData)
        {
            BurnResponse R = new BurnResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_Burn", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param2 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param4 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param5 = new SqlParameter("pi_BurnPoints", PointsBurn);
                        SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        SqlParameter param12 = new SqlParameter("pi_json", DynamicData);

                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param11);
                        cmdReport.Parameters.Add(param12);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];

                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            R.ResponseCode = "00";
                            R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);
                            R.PointsRedeemed = Convert.ToString(dt1.Rows[0]["PointsRedeemed"]);
                            R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);


                            if (dt2.Rows.Count > 0)
                            {
                               

                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusTxn"]);
                                string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusTxn"]);
                                if (SMSStatus == "1" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendSMSandWA(dt3));
                                    _job.Start();
                                }
                                else if (SMSStatus == "1" && WAStatus == "0")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
                                    string _MobileMessage = dt3.Rows[0]["MessageTxn"].ToString();
                                    string _UserName = dt3.Rows[0]["UserNameTxn"].ToString();
                                    string _Password = dt3.Rows[0]["PasswordTxn"].ToString();
                                    string _Sender = dt3.Rows[0]["SenderIdTxn"].ToString();
                                    string _Url = dt3.Rows[0]["UrlTxn"].ToString();
                                    string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                    _job.Start();
                                }
                                else if (SMSStatus == "0" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendWAMessage(dt3));
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

        public List<DailyReport> DailyReport(string CounterId)
        {
            List<DailyReport> LtObj = new List<DailyReport>();
            DailyReport obj = new DailyReport();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    string outletId = CounterId.Substring(0, 8);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_Summary", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        //SqlParameter param2 = new SqlParameter("pi_LoginType", "1");
                        SqlParameter param3 = new SqlParameter("pi_OutletId", outletId);
                        SqlParameter param4 = new SqlParameter("pi_ReportType", "1");
                        SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        //cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        for (int i = 0; i < 1; i++)
                        {
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                obj.ResponseCode = "00";
                                obj.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                                DataTable dt1 = retVal.Tables[1];
                                //DataTable dt2 = retVal.Tables[2];
                                obj.Counterid = Convert.ToString(dt1.Rows[0]["Counterid"]);
                                obj.TotalEnrolment = Convert.ToString(dt1.Rows[0]["TotalEnrolment"]);
                                obj.EarnTxnCount = Convert.ToString(dt1.Rows[0]["EarnTxnCount"]);
                                obj.EarnTxnAmt = Convert.ToString(dt1.Rows[0]["EarnTxnAmt"]);
                                obj.EarnPoints = Convert.ToString(dt1.Rows[0]["EarnPoints"]);
                                obj.BurnTxnCount = Convert.ToString(dt1.Rows[0]["BurnTxnCount"]);
                                obj.BurnTxnAmt = Convert.ToString(dt1.Rows[0]["BurnTxnAmt"]);
                                obj.BurnPoints = Convert.ToString(dt1.Rows[0]["BurnPoints"]);
                                obj.TotalInvoiceAmt = Convert.ToString(dt1.Rows[0]["TotalInvoiceAmt"]);

                                LtObj.Add(obj);
                            }
                            else
                            {
                                obj.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                obj.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);

                                LtObj.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DailyReport");
            }
            return LtObj;
        }

        public MDRDetails MembDetailReprt(string CounterId, string Mobileno)
        {
            MDRDetails ObjMDRDetails = new MDRDetails();
            MDRResponse objMDRResponse = new MDRResponse();
            MDRData objMDRData = new MDRData();
            List<MDRTxnDetails> LstMDRData = new List<MDRTxnDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    string outletId = CounterId.Substring(0, 8);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_MemberDetailedTxn", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        //SqlParameter param2 = new SqlParameter("pi_LoginType", "1");
                        SqlParameter param3 = new SqlParameter("pi_SearchData", Mobileno);
                        SqlParameter param4 = new SqlParameter("pi_SearchType", "1");
                        SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        //cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (retVal.Tables.Count > 1)
                        {
                            dt1 = retVal.Tables[1];
                            dt2 = retVal.Tables[2];
                        }



                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            MDRTxnDetails obj = new MDRTxnDetails();
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                if (i == 0)
                                {
                                    //ObjMDRDetails.ObjMDRResponse.ResponseCode = Convert.ToString(dt.Rows[i]["ResponseCode"]);
                                    objMDRResponse.ResponseCode = "00";
                                    objMDRResponse.ResponseMessage = "Success";
                                    ObjMDRDetails.ObjMDRResponse = objMDRResponse;

                                    objMDRData.Mobileno = Convert.ToString(dt1.Rows[i]["Mobileno"]);
                                    objMDRData.CustomerName = Convert.ToString(dt1.Rows[i]["CustomerName"]);
                                    objMDRData.AvailablePoints = Convert.ToString(dt1.Rows[i]["AvailablePoints"]);
                                    objMDRData.CardNo = Convert.ToString(dt1.Rows[i]["CardNo"]);
                                    objMDRData.EnrolledOutlet = Convert.ToString(dt1.Rows[i]["EnrolledOutlet"]);
                                    objMDRData.EnrolledOn = Convert.ToDateTime(dt1.Rows[i]["EnrolledOn"]).ToString("MM/dd/yyyy");
                                    if (Convert.ToDateTime(dt1.Rows[i]["DOB"]).Month == DateTime.Now.Month)
                                    {
                                        objMDRData.DOB = Convert.ToDateTime(dt1.Rows[i]["DOB"]).ToString("MM/dd/yyyy");
                                    }
                                    ObjMDRDetails.MDRData = objMDRData;
                                }

                                obj.Date = Convert.ToString(dt2.Rows[i]["Date"]);
                                obj.InvoiceNo = Convert.ToString(dt2.Rows[i]["InvoiceNo"]);
                                obj.InvoiceAmt = Convert.ToString(dt2.Rows[i]["InvoiceAmt"]);
                                obj.Points = Convert.ToString(dt2.Rows[i]["Points"]);
                                obj.Type = Convert.ToString(dt2.Rows[i]["Type"]);

                                LstMDRData.Add(obj);
                                //ObjMDRDetails.ListMDRTxnDetails.Add(obj);
                            }
                        }
                        ObjMDRDetails.ListMDRTxnDetails = LstMDRData;

                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) != "00")
                        {
                            // MDRDetails obj1 = new MDRDetails();
                            //MDRResponse obj1 = new MDRResponse();
                            objMDRResponse.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            objMDRResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);

                            ObjMDRDetails.ObjMDRResponse = objMDRResponse;
                            //LtObj.Add(obj1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "MembDetailReprt");
            }
            return ObjMDRDetails;
        }

        public GTDTxnDetails GetTxnDetailsRepo(string CounterId, string Mobileno)
        {
            GTDTxnDetails ObjGTDDetails = new GTDTxnDetails();
            Response objResponse = new Response();
            GTDData objGTDData = new GTDData();
            List<GTDTxnData> LstGTDTxn = new List<GTDTxnData>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    string outletId = CounterId.Substring(0, 8);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_GetTxnDetails", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        //SqlParameter param2 = new SqlParameter("pi_LoginType", "1");
                        SqlParameter param3 = new SqlParameter("pi_SearchData", Mobileno);
                        SqlParameter param4 = new SqlParameter("pi_SearchType", "1");
                        SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        //cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (retVal.Tables.Count > 1)
                        {
                            dt1 = retVal.Tables[1];
                            dt2 = retVal.Tables[2];
                        }

                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            GTDTxnData obj = new GTDTxnData();
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                if (i == 0)
                                {
                                    //ObjMDRDetails.ObjMDRResponse.ResponseCode = Convert.ToString(dt.Rows[i]["ResponseCode"]);
                                    objResponse.ResponseCode = "00";
                                    objResponse.ResponseMessage = "Success";
                                    ObjGTDDetails.GTDResponse = objResponse;

                                    objGTDData.Mobileno = Convert.ToString(dt1.Rows[i]["MobileNo"]);
                                    objGTDData.CustomerName = Convert.ToString(dt1.Rows[i]["CustomerName"]);
                                    // objGTDData.TxnCount = Convert.ToString(dt1.Rows[i]["EnrolledOutlet"]);
                                    objGTDData.CardNo = Convert.ToString(dt1.Rows[i]["CardNo"]);
                                    objGTDData.TxnCount = Convert.ToString(dt1.Rows[i]["TxnCount"]);

                                    ObjGTDDetails.GTDDataobj = objGTDData;
                                }

                                obj.Date = Convert.ToString(dt2.Rows[i]["Date"]);
                                obj.Invoiceno = Convert.ToString(dt2.Rows[i]["InvoiceNo"]);
                                obj.InvoiceAmt = Convert.ToString(dt2.Rows[i]["InvoiceAmt"]);
                                obj.Points = Convert.ToString(dt2.Rows[i]["Points"]);
                                obj.Type = Convert.ToString(dt2.Rows[i]["Type"]);

                                LstGTDTxn.Add(obj);
                                //ObjMDRDetails.ListMDRTxnDetails.Add(obj);
                            }
                        }
                        ObjGTDDetails.LstGTDTxnData = LstGTDTxn;

                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) != "00")
                        {
                            // MDRDetails obj1 = new MDRDetails();
                            //MDRResponse obj1 = new MDRResponse();
                            objResponse.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);

                            ObjGTDDetails.GTDResponse = objResponse;
                            //LtObj.Add(obj1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTxnDetailsRepo");
            }
            return ObjGTDDetails;
        }

        public CancelData CancelTxn(string CounterId, string Mobileno, string InvoiceNo)
        {
            CancelTxnDetails ObjCancel = new CancelTxnDetails();
            Response objResponse = new Response();
            CancelData ObjCanData = new CancelData();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    string outletId = CounterId.Substring(0, 8);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_Cancel", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        //SqlParameter param2 = new SqlParameter("pi_LoginType", "1");
                        SqlParameter param3 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param4 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        //cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        //if (retVal.Tables.Count > 1)
                        //{
                        //    dt1 = retVal.Tables[1];
                        //    dt2 = retVal.Tables[2];
                        //}

                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            objResponse.ResponseCode = "00";
                            objResponse.ResponseMessage = "Success";
                            dt1 = retVal.Tables[1];
                            dt2 = retVal.Tables[2];
                            ObjCancel.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                            ObjCancel.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                            ObjCancel.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                            ObjCancel.InvoiceNo = Convert.ToString(dt1.Rows[0]["InvoiceNo"]);
                            ObjCancel.PointsCredited = Convert.ToString(dt1.Rows[0]["PointsCredited"]);
                            ObjCancel.PointsDebited = Convert.ToString(dt1.Rows[0]["PointsDebited"]);

                            ObjCanData.Cancelresponse = objResponse;
                            ObjCanData.CancelTxn = ObjCancel;

                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusTxn"]);
                                string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusTxn"]);

                                

                                if (SMSStatus == "1" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendSMSandWA(dt3));
                                    _job.Start();
                                }
                                else if (SMSStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
                                    string _MobileMessage = dt3.Rows[0]["MessageTxn"].ToString();
                                    string _UserName = dt3.Rows[0]["UserNameTxn"].ToString();
                                    string _Password = dt3.Rows[0]["PasswordTxn"].ToString();
                                    string _Sender = dt3.Rows[0]["SenderIdTxn"].ToString();
                                    string _Url = dt3.Rows[0]["UrlTxn"].ToString();
                                    string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                    _job.Start();
                                }
                                else if (WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendWAMessage(dt3));
                                    _job.Start();
                                }
                            }
                        }
                        else
                        {
                            objResponse.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);

                            ObjCanData.Cancelresponse = objResponse;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CancelTxn");
            }
            return ObjCanData;
        }
        public BurnValidationResponse OTPResend(string CounterId, string Mobileno)
        {
            BurnValidationResponse R = new BurnValidationResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_OTP", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param2 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param3 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);

                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            R.ResponseCode = "00";
                            R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            R.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);

                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusOTP"]);
                                string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusOTP"]);

                                if (SMSStatus == "1" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendSMSandWA(dt3));
                                    _job.Start();
                                }
                                else if (SMSStatus == "1" && WAStatus == "0")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    string _MobileNo = dt3.Rows[0]["CommMobileNoOTP"].ToString();
                                    string _MobileMessage = dt3.Rows[0]["MessageOTP"].ToString();
                                    string _UserName = dt3.Rows[0]["UserNameOTP"].ToString();
                                    string _Password = dt3.Rows[0]["PasswordOTP"].ToString();
                                    string _Sender = dt3.Rows[0]["SenderIdOTP"].ToString();
                                    string _Url = dt3.Rows[0]["UrlOTP"].ToString();
                                    string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                    _job.Start();
                                }
                                else if (SMSStatus == "0" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendWAMessage(dt3));
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
                newexception.AddException(ex, "OTPResend");
            }
            return R;
        }

        public bool AddMembership(tblMembershipDetail objDetails, tblCustDetailsMaster objCustDetails, tblCustInfo objCustInfo, tblCustTxnSummaryMaster objCustTxnSummary, string connstr)
        {
            bool result = false;
            try
            {                
                using (var context = new BOTSDBContext(connstr))
                {
                    context.tblMembershipDetails.Add(objDetails);
                    context.SaveChanges();

                    context.tblCustDetailsMasters.Add(objCustDetails);
                    context.SaveChanges();

                    context.tblCustInfoes.Add(objCustInfo);
                    context.SaveChanges();

                    context.tblCustTxnSummaryMasters.Add(objCustTxnSummary);
                    context.SaveChanges();

                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddMembership");
            }
            return result;
        }
        public tblMembershipDetail GetMembershipDetail(string connstr, string MobileNo)
        {
            tblMembershipDetail objData = new tblMembershipDetail();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objData = context.tblMembershipDetails.Where(x => x.MobileNo == MobileNo).OrderByDescending(y=>y.CreatedDate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddMembership");
            }
            return objData;
        }

        public RetailBulkResponse BulkTransaction(DataTable dt,string CounterId)
        {
            RetailBulkResponse Obj = new RetailBulkResponse();
            int c = 0;
            Obj.Status = false;
            string StrDate, Strtime,TDatetime;
            CultureInfo culture = new CultureInfo("en-IN");
            DateTime tempDatetime, TDate; 

            try
            {
                Obj.TbleRWCount = Convert.ToString(dt.Rows.Count);

                int ValidRow = dt.Select("mobile <> '' AND mobile <> 'mobile'").Count();
                if(ValidRow > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt.Rows[i]["mobile"]) != "" && Convert.ToString(dt.Rows[i]["mobile"]) != "mobile" && i > 0 )
                        {
                            //var conStr = ConfigurationManager.ConnectionStrings["BOTSDBContext"].ToString();
                            var conStr = CR.GetRetailWebConnString(CounterId);

                            using (var context = new BOTSDBContext(conStr))
                            {
                                SqlConnection _Con = new SqlConnection(conStr);
                                DataSet retVal = new DataSet();
                                SqlCommand cmdReport = new SqlCommand("sp_RetailAppTxnUpload", _Con);
                                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                                using (cmdReport)
                                {
                                    SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                                    SqlParameter param2 = new SqlParameter("pi_MobileNo", Convert.ToString(dt.Rows[i]["mobile"]));
                                    SqlParameter param3 = new SqlParameter("pi_CustomerName", Convert.ToString(dt.Rows[i]["csname"]));
                                    StrDate = Convert.ToString(dt.Rows[i]["vdate"]);
                                    Strtime = Convert.ToString(dt.Rows[i]["ctime"]);
                                    TDatetime = StrDate + " " + Strtime;
                                    tempDatetime = Convert.ToDateTime(TDatetime, culture);
                                    SqlParameter param4 = new SqlParameter("pi_Datetime", tempDatetime);
                                    SqlParameter param5 = new SqlParameter("pi_Mode", dt.Rows[i]["mode"]);
                                    SqlParameter param6 = new SqlParameter("pi_InvoiceAmt", dt.Rows[i]["amt"]);

                                    SqlParameter param7 = new SqlParameter("pi_InvoiceNo", dt.Rows[i]["billno"]);

                                    cmdReport.CommandType = CommandType.StoredProcedure;
                                    cmdReport.Parameters.Add(param1);
                                    cmdReport.Parameters.Add(param2);
                                    cmdReport.Parameters.Add(param3);
                                    cmdReport.Parameters.Add(param4);
                                    cmdReport.Parameters.Add(param5);
                                    cmdReport.Parameters.Add(param6);
                                    cmdReport.Parameters.Add(param7);

                                    daReport.Fill(retVal);
                                    DataTable Data = retVal.Tables[0];

                                    Obj.Status = true;

                                    if (Convert.ToString(Data.Rows[0]["ResponseCode"]) == "00")
                                    {
                                        c++;
                                        DataTable dt1 = retVal.Tables[1];


                                        if (dt1.Rows.Count > 0)
                                        {
                                            string SMSStatus = Convert.ToString(dt1.Rows[0]["SMSStatusTxn"]);
                                            string WAStatus = Convert.ToString(dt1.Rows[0]["WAStatusTxn"]);

                                            if (SMSStatus == "1" && WAStatus == "1")
                                            {
                                                DataTable dt2 = retVal.Tables[2];

                                                Thread _job = new Thread(() => SendSMSandWA(dt2));
                                                _job.Start();
                                            }
                                            else if (SMSStatus == "1" && WAStatus == "0")
                                            {
                                                DataTable dt2 = retVal.Tables[2];

                                                string _MobileNo = dt2.Rows[0]["CommMobileNoTxn"].ToString();
                                                string _MobileMessage = dt2.Rows[0]["MessageTxn"].ToString();
                                                string _UserName = dt2.Rows[0]["UserNameTxn"].ToString();
                                                string _Password = dt2.Rows[0]["PasswordTxn"].ToString();
                                                string _Sender = dt2.Rows[0]["SenderIdTxn"].ToString();
                                                string _Url = dt2.Rows[0]["UrlTxn"].ToString();
                                                string _SMSBrandId = dt2.Rows[0]["SMSBrandId"].ToString();
                                                Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                                _job.Start();
                                            }
                                            else if (SMSStatus == "0" && WAStatus == "1")
                                            {
                                                DataTable dt2 = retVal.Tables[2];

                                                Thread _job = new Thread(() => SendWAMessage(dt2));
                                                _job.Start();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Obj.Status = true;

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

        public BurnValidationResponse BurnValidationRatnaEnterprise(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt, string PointsBurn, string DynamicData)
        {
            BurnValidationResponse R = new BurnValidationResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_BurnValidation", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param2 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                        SqlParameter param4 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param5 = new SqlParameter("pi_BurnPoints", PointsBurn);
                        SqlParameter param6 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        SqlParameter param7 = new SqlParameter("pi_jsondata", DynamicData);


                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param6);
                        cmdReport.Parameters.Add(param7);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                        {
                            R.ResponseCode = "00";
                            R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            DataTable dt1 = retVal.Tables[1];
                            DataTable dt2 = retVal.Tables[2];
                            R.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                            R.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["BurnPointsAsAmount"]);
                            R.PointsValue = Convert.ToString(dt1.Rows[0]["PointsValue"]);



                            if (dt2.Rows.Count > 0)
                            {
                                string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusOTP"]);
                                string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusOTP"]);

                                if (SMSStatus == "1" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendSMSandWA(dt3));
                                    _job.Start();
                                }
                                else if (SMSStatus == "1" && WAStatus == "0")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    string _MobileNo = dt3.Rows[0]["CommMobileNoOTP"].ToString();
                                    string _MobileMessage = dt3.Rows[0]["MessageOTP"].ToString();
                                    string _UserName = dt3.Rows[0]["UserNameOTP"].ToString();
                                    string _Password = dt3.Rows[0]["PasswordOTP"].ToString();
                                    string _Sender = dt3.Rows[0]["SenderIdOTP"].ToString();
                                    string _Url = dt3.Rows[0]["UrlOTP"].ToString();
                                    string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();
                                    Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                                    _job.Start();
                                }
                                else if (SMSStatus == "0" && WAStatus == "1")
                                {
                                    DataTable dt3 = retVal.Tables[3];

                                    Thread _job = new Thread(() => SendWAMessage(dt3));
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
                newexception.AddException(ex, "BurnValidation");
            }
            return R;
        }
        public void SendSMSandWA(DataTable dt3)
        {
            string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
            string _WATokenId = dt3.Rows[0]["WATokenId"].ToString();
            string _WAMessage = dt3.Rows[0]["WhatsAppMessage"].ToString();
            string _MobileMessage = dt3.Rows[0]["MessageTxn"].ToString();
            string _UserName = dt3.Rows[0]["UserNameTxn"].ToString();
            string _Password = dt3.Rows[0]["PasswordTxn"].ToString();
            string _Sender = dt3.Rows[0]["SenderIdTxn"].ToString();
            string _Url = dt3.Rows[0]["UrlTxn"].ToString();
            string _SMSBrandId = dt3.Rows[0]["SMSBrandId"].ToString();

            string responseString;
            try
            {

                _WAMessage = _WAMessage.Replace("#99", "&");
                _WAMessage = HttpUtility.UrlEncode(_WAMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", _WATokenId);
                sbposdata.AppendFormat("&phone=91{0}", _MobileNo);
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
                Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                _job.Start();
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                _job.Start();
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                _job.Start();
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

        }
        public void SendMessageOTP(DataTable dt3)
        {
            string _MobileNo = dt3.Rows[0]["CommMobileNoOTP"].ToString();
            string _WATokenId = dt3.Rows[0]["WATokenId"].ToString();
            string _WAMessage = dt3.Rows[0]["MessageOTP"].ToString();

            //Thread _job = new Thread(() => WAText(_MobileNo, _WATokenId, _WAMessage));
            //_job.Start();

        }
        public void SendWAMessage(DataTable dt3)
        {
            string responseString;
            string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
            string _WATokenId = dt3.Rows[0]["WATokenId"].ToString();
            string _WAMessage = dt3.Rows[0]["WhatsAppMessage"].ToString();
            try
            {

                _WAMessage = _WAMessage.Replace("#99", "&");
                _WAMessage = HttpUtility.UrlEncode(_WAMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", _WATokenId);
                sbposdata.AppendFormat("&phone=91{0}", _MobileNo);
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

        public void SendBOTSVideo(DataTable dt3)
        {
            string responseString;
            string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
            string _WATokenId = dt3.Rows[0]["WATokenId"].ToString();
            string _WAMessage = dt3.Rows[0]["WhatsAppMessage"].ToString();
            try
            {

                _WAMessage = _WAMessage.Replace("#99", "&");
                _WAMessage = HttpUtility.UrlEncode(_WAMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendFileWithCaption?");
                sbposdata.AppendFormat("token={0}", _WATokenId);
                sbposdata.AppendFormat("&phone=91{0}", _MobileNo);
                sbposdata.AppendFormat("&message={0}", _WAMessage);
                sbposdata.AppendFormat("&link={0}", "https://blueocktopus.in/BitlyImages/BOTS_Introduction.MP4");
                
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

        public void SendSMS(string _MobileNo, string _MobileMessage, string _UserName, string _Password, string _Sender, string _Url,string _SMSBrandId)
        {
                    switch (_SMSBrandId)
                    {
                        case "00001": //Techno Core Unicode
                            string date_00001 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                            _MobileMessage = _MobileMessage.Replace("#99", "&");
                            _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                            string type_00001 = "unicode";
                            StringBuilder sbposdata_00001 = new StringBuilder();
                            sbposdata_00001.AppendFormat("userid={0}", _UserName);
                            sbposdata_00001.AppendFormat("&password={0}", _Password);
                            sbposdata_00001.AppendFormat("&sendMethod={0}", "quick");
                            sbposdata_00001.AppendFormat("&mobile={0}", _MobileNo);
                            sbposdata_00001.AppendFormat("&msg={0}", _MobileMessage);
                            sbposdata_00001.AppendFormat("&senderid={0}", _Sender);
                            sbposdata_00001.AppendFormat("&msgType={0}", type_00001);
                            sbposdata_00001.AppendFormat("&format={0}", type_00001);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_00001 = (HttpWebRequest)WebRequest.Create(_Url);
                            UTF8Encoding encoding_00001 = new UTF8Encoding();
                            byte[] data_00001 = encoding_00001.GetBytes(sbposdata_00001.ToString());
                            httpWReq_00001.Method = "POST";
                            httpWReq_00001.ContentType = "application/x-www-form-urlencoded";
                            httpWReq_00001.ContentLength = data_00001.Length;
                            using (Stream stream_00001 = httpWReq_00001.GetRequestStream())
                            {
                                stream_00001.Write(data_00001, 0, data_00001.Length);
                            }
                            HttpWebResponse response_00001 = (HttpWebResponse)httpWReq_00001.GetResponse();
                            StreamReader reader_00001 = new StreamReader(response_00001.GetResponseStream());
                            string responseString_00001 = reader_00001.ReadToEnd();
                            reader_00001.Close();
                            response_00001.Close();
                            break;
                        case "00002": // Value First Unicode
                            string date_00002 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                            _MobileMessage = _MobileMessage.Replace("#99", "&");
                            _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                            string type_00002 = "unicode";
                            StringBuilder sbposdata_00002 = new StringBuilder();
                            sbposdata_00002.AppendFormat("userid={0}", _UserName);
                            sbposdata_00002.AppendFormat("&password={0}", _Password);
                            sbposdata_00002.AppendFormat("&sendMethod={0}", "quick");
                            sbposdata_00002.AppendFormat("&mobile={0}", _MobileNo);
                            sbposdata_00002.AppendFormat("&msg={0}", _MobileMessage);
                            sbposdata_00002.AppendFormat("&senderid={0}", _Sender);
                            sbposdata_00002.AppendFormat("&msgType={0}", type_00002);
                            sbposdata_00002.AppendFormat("&format={0}", type_00002);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_00002 = (HttpWebRequest)WebRequest.Create(_Url);
                            UTF8Encoding encoding_00002 = new UTF8Encoding();
                            byte[] data_00002 = encoding_00002.GetBytes(sbposdata_00002.ToString());
                            httpWReq_00002.Method = "POST";
                            httpWReq_00002.ContentType = "application/x-www-form-urlencoded";
                            httpWReq_00002.ContentLength = data_00002.Length;
                            using (Stream stream_00002 = httpWReq_00002.GetRequestStream())
                            {
                                stream_00002.Write(data_00002, 0, data_00002.Length);
                            }
                            HttpWebResponse response_00002 = (HttpWebResponse)httpWReq_00002.GetResponse();
                            StreamReader reader_00002 = new StreamReader(response_00002.GetResponseStream());
                            string responseString_00002 = reader_00002.ReadToEnd();
                            reader_00002.Close();
                            response_00002.Close();
                            break;
                        case "00003": //Vision HLT English
                            var httpWebRequest_00003 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00003.ContentType = "application/json";
                            httpWebRequest_00003.Method = "POST";

                            using (var streamWriter_00003 = new StreamWriter(httpWebRequest_00003.GetRequestStream()))
                            {

                                string json_00003 = "{\"Account\":" +
                                                "{\"APIKey\":\"" + _Password + "\"," +
                                                "\"SenderId\":\"" + _Sender + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"0\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
                                                "\"Text\":\"" + _MobileMessage + "\"}]" +
                                                "}";
                                streamWriter_00003.Write(json_00003);
                            }

                            var httpResponse_00003 = (HttpWebResponse)httpWebRequest_00003.GetResponse();
                            using (var streamReader_00003 = new StreamReader(httpResponse_00003.GetResponseStream()))
                            {
                                var result_00003 = streamReader_00003.ReadToEnd();
                            }
                            break;
                        case "00004": //Vision HLT Unicode
                            var httpWebRequest_00004 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00004.ContentType = "application/json";
                            httpWebRequest_00004.Method = "POST";

                            using (var streamWriter_00004 = new StreamWriter(httpWebRequest_00004.GetRequestStream()))
                            {

                                string json_00004 = "{\"Account\":" +
                                                "{\"APIKey\":\"" + _Password + "\"," +
                                                "\"SenderId\":\"" + _Sender + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"8\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
                                                "\"Text\":\"" + _MobileMessage + "\"}]" +
                                                "}";
                                streamWriter_00004.Write(json_00004);
                            }

                            var httpResponse_00004 = (HttpWebResponse)httpWebRequest_00004.GetResponse();
                            using (var streamReader_00004 = new StreamReader(httpResponse_00004.GetResponseStream()))
                            {
                                var result_00004 = streamReader_00004.ReadToEnd();
                            }
                            break;
                        case "00005": //Pinnacle English
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            var httpWebRequest_00005 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00005.ContentType = "application/json";
                            httpWebRequest_00005.Headers.Add("Apikey", _Password);
                            httpWebRequest_00005.Method = "POST";
                            _MobileMessage = _MobileMessage.Replace("#99", "&");

                            using (var streamWriter_00005 = new StreamWriter(httpWebRequest_00005.GetRequestStream()))
                            {

                                string json_00005 = "{\"sender\":\"" + _Sender + "\"," +
                                "\"message\":[{\"number\":\"91" + _MobileNo + "\"," +
                                 "\"text\":\"" + _MobileMessage + "\"}]," + "\"messagetype\":\"TXT\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
                                streamWriter_00005.Write(json_00005);
                            }

                            var httpResponse_00005 = (HttpWebResponse)httpWebRequest_00005.GetResponse();
                            using (var streamReader_00005 = new StreamReader(httpResponse_00005.GetResponseStream()))
                            {
                                var result_00005 = streamReader_00005.ReadToEnd();
                            }
                            break;
                        case "00006": //Pinnacle Unicode
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            var httpWebRequest_00006 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00006.ContentType = "application/json";
                            httpWebRequest_00006.Headers.Add("Apikey", _Password);
                            httpWebRequest_00006.Method = "POST";
                            _MobileMessage = _MobileMessage.Replace("#99", "&");

                            using (var streamWriter_00006 = new StreamWriter(httpWebRequest_00006.GetRequestStream()))
                            {

                                string json_00006 = "{\"sender\":\"" + _Sender + "\"," +
                                "\"message\":[{\"number\":\"91" + _MobileNo + "\"," +
                                 "\"text\":\"" + _MobileMessage + "\"}]," + "\"messagetype\":\"TXT\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
                                streamWriter_00006.Write(json_00006);
                            }

                            var httpResponse_00006 = (HttpWebResponse)httpWebRequest_00006.GetResponse();
                            using (var streamReader_00006 = new StreamReader(httpResponse_00006.GetResponseStream()))
                            {
                                var result_00006 = streamReader_00006.ReadToEnd();
                            }
                            break;
                        default:
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
                    break;
                    }       
        }


        //public void SendSMSMessageTxn(string _MobileNo, string _MobileMessage, string _UserName, string _Password, string _Sender, string _Url, string _CounterId)
        //{
        //    string _responseData;
        //    try
        //    {
        //        switch (_CounterId.Substring(0, 5))
        //        {
        //            case "10851"://Govind Dande
        //                var httpWebRequest_10851 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_10851.ContentType = "application/json";
        //                httpWebRequest_10851.Method = "POST";
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                using (var streamWriter_10851 = new StreamWriter(httpWebRequest_10851.GetRequestStream()))
        //                {
        //                    string json_10851 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"8\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_10851.Write(json_10851);
        //                }
        //                var httpResponse_10851 = (HttpWebResponse)httpWebRequest_10851.GetResponse();
        //                using (var streamReader_10851 = new StreamReader(httpResponse_10851.GetResponseStream()))
        //                {
        //                    var result_10851 = streamReader_10851.ReadToEnd();
        //                }
        //                break;
        //            case "10931": // Banthiya
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                //_MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_10931 = "TEXT";
        //                StringBuilder sbposdata_10931 = new StringBuilder();
        //                sbposdata_10931.AppendFormat("apikey={0}", _UserName);
        //                sbposdata_10931.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_10931.AppendFormat("&dest_mobileno={0}", _MobileNo);
        //                sbposdata_10931.AppendFormat("&msgtype={0}", "UNI");
        //                sbposdata_10931.AppendFormat("&message={0}", _MobileMessage);
        //                sbposdata_10931.AppendFormat("&response={0}", "Y");

        //                HttpWebRequest httpWReq_10931 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_10931 = new UTF8Encoding();
        //                byte[] data_10931 = encoding_10931.GetBytes(sbposdata_10931.ToString());
        //                httpWReq_10931.Method = "POST";
        //                httpWReq_10931.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_10931.ContentLength = data_10931.Length;
        //                using (Stream stream_10931 = httpWReq_10931.GetRequestStream())
        //                {
        //                    stream_10931.Write(data_10931, 0, data_10931.Length);
        //                }
        //                HttpWebResponse response_10931 = (HttpWebResponse)httpWReq_10931.GetResponse();
        //                StreamReader reader_10931 = new StreamReader(response_10931.GetResponseStream());
        //                responseString = reader_10931.ReadToEnd();
        //                reader_10931.Close();
        //                response_10931.Close();

        //                break;
        //            case "11321": // Sree swami Purandare Jewellers
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11321 = "TEXT";
        //                StringBuilder sbposdata_11321 = new StringBuilder();
        //                sbposdata_11321.AppendFormat("user={0}", _UserName);
        //                sbposdata_11321.AppendFormat("&pwd={0}", _Password);
        //                sbposdata_11321.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11321.AppendFormat("&CountryCode={0}", "91");
        //                sbposdata_11321.AppendFormat("&mobileno={0}", _MobileNo);
        //                sbposdata_11321.AppendFormat("&msgtext={0}", _MobileMessage);
        //                sbposdata_11321.AppendFormat("&smstype={0}", "0");
        //                sbposdata_11321.AppendFormat("&pe_id={0}", "1701161580963320685");
        //                HttpWebRequest httpWReq_11321 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11321 = new UTF8Encoding();
        //                byte[] data_11321 = encoding_11321.GetBytes(sbposdata_11321.ToString());
        //                httpWReq_11321.Method = "POST";
        //                httpWReq_11321.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11321.ContentLength = data_11321.Length;
        //                using (Stream stream_11321 = httpWReq_11321.GetRequestStream())
        //                {
        //                    stream_11321.Write(data_11321, 0, data_11321.Length);
        //                }
        //                HttpWebResponse response_11321 = (HttpWebResponse)httpWReq_11321.GetResponse();
        //                StreamReader reader_11321 = new StreamReader(response_11321.GetResponseStream());
        //                responseString = reader_11321.ReadToEnd();
        //                reader_11321.Close();
        //                response_11321.Close();
        //                break;
        //            case "11561"://MGKajaelSons
        //                string date_11561 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11561 = "unicode";
        //                StringBuilder sbposdata_11561 = new StringBuilder();
        //                sbposdata_11561.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11561.AppendFormat("&password={0}", _Password);
        //                sbposdata_11561.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11561.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11561.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11561.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11561.AppendFormat("&msgType={0}", type_11561);
        //                sbposdata_11561.AppendFormat("&format={0}", type_11561);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11561 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11561 = new UTF8Encoding();
        //                byte[] data_11561 = encoding_11561.GetBytes(sbposdata_11561.ToString());
        //                httpWReq_11561.Method = "POST";
        //                httpWReq_11561.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11561.ContentLength = data_11561.Length;
        //                using (Stream stream_11561 = httpWReq_11561.GetRequestStream())
        //                {
        //                    stream_11561.Write(data_11561, 0, data_11561.Length);
        //                }
        //                HttpWebResponse response_11561 = (HttpWebResponse)httpWReq_11561.GetResponse();
        //                StreamReader reader_11561 = new StreamReader(response_11561.GetResponseStream());
        //                string responseString_11561 = reader_11561.ReadToEnd();
        //                reader_11561.Close();
        //                response_11561.Close();
        //                break;
        //            case "11441": // Mhaswadkar Jewellers
        //                string date_11441 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11441 = "text";
        //                StringBuilder sbposdata_11441 = new StringBuilder();
        //                sbposdata_11441.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11441.AppendFormat("&password={0}", _Password);
        //                sbposdata_11441.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11441.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11441.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11441.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11441.AppendFormat("&msgType={0}", type_11441);
        //                sbposdata_11441.AppendFormat("&format={0}", type_11441);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11441 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11441 = new UTF8Encoding();
        //                byte[] data_11441 = encoding_11441.GetBytes(sbposdata_11441.ToString());
        //                httpWReq_11441.Method = "POST";
        //                httpWReq_11441.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11441.ContentLength = data_11441.Length;
        //                using (Stream stream_11441 = httpWReq_11441.GetRequestStream())
        //                {
        //                    stream_11441.Write(data_11441, 0, data_11441.Length);
        //                }
        //                HttpWebResponse response_11441 = (HttpWebResponse)httpWReq_11441.GetResponse();
        //                StreamReader reader_11441 = new StreamReader(response_11441.GetResponseStream());
        //                string responseString_11441 = reader_11441.ReadToEnd();
        //                reader_11441.Close();
        //                response_11441.Close();
        //                break;
        //            case "11491": // Ratnatray Jewellers
        //                string date_11491 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11491 = "unicode";
        //                StringBuilder sbposdata_11491 = new StringBuilder();
        //                sbposdata_11491.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11491.AppendFormat("&password={0}", _Password);
        //                sbposdata_11491.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11491.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11491.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11491.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11491.AppendFormat("&msgType={0}", type_11491);
        //                sbposdata_11491.AppendFormat("&format={0}", type_11491);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11491 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11491 = new UTF8Encoding();
        //                byte[] data_11491 = encoding_11491.GetBytes(sbposdata_11491.ToString());
        //                httpWReq_11491.Method = "POST";
        //                httpWReq_11491.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11491.ContentLength = data_11491.Length;
        //                using (Stream stream_11491 = httpWReq_11491.GetRequestStream())
        //                {
        //                    stream_11491.Write(data_11491, 0, data_11491.Length);
        //                }
        //                HttpWebResponse response_11491 = (HttpWebResponse)httpWReq_11491.GetResponse();
        //                StreamReader reader_11491 = new StreamReader(response_11491.GetResponseStream());
        //                string responseString_11491 = reader_11491.ReadToEnd();
        //                reader_11491.Close();
        //                response_11491.Close();
        //                break;
        //            case "11791": // Kiran Jewellers
        //                string date_11791 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11791 = "text";
        //                StringBuilder sbposdata_11791 = new StringBuilder();
        //                sbposdata_11791.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11791.AppendFormat("&password={0}", _Password);
        //                sbposdata_11791.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11791.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11791.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11791.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11791.AppendFormat("&msgType={0}", type_11791);
        //                sbposdata_11791.AppendFormat("&format={0}", type_11791);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11791 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11791 = new UTF8Encoding();
        //                byte[] data_11791 = encoding_11791.GetBytes(sbposdata_11791.ToString());
        //                httpWReq_11791.Method = "POST";
        //                httpWReq_11791.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11791.ContentLength = data_11791.Length;
        //                using (Stream stream_11791 = httpWReq_11791.GetRequestStream())
        //                {
        //                    stream_11791.Write(data_11791, 0, data_11791.Length);
        //                }
        //                HttpWebResponse response_11791 = (HttpWebResponse)httpWReq_11791.GetResponse();
        //                StreamReader reader_11791 = new StreamReader(response_11791.GetResponseStream());
        //                string responseString_11791 = reader_11791.ReadToEnd();
        //                reader_11791.Close();
        //                response_11791.Close();
        //                break;
        //            case "11821": // Devi Jewellers
        //                string date_11821 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11821 = "text";
        //                StringBuilder sbposdata_11821 = new StringBuilder();
        //                sbposdata_11821.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11821.AppendFormat("&password={0}", _Password);
        //                sbposdata_11821.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11821.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11821.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11821.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11821.AppendFormat("&msgType={0}", type_11821);
        //                sbposdata_11821.AppendFormat("&format={0}", type_11821);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11821 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11821 = new UTF8Encoding();
        //                byte[] data_11821 = encoding_11821.GetBytes(sbposdata_11821.ToString());
        //                httpWReq_11821.Method = "POST";
        //                httpWReq_11821.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11821.ContentLength = data_11821.Length;
        //                using (Stream stream_11821 = httpWReq_11821.GetRequestStream())
        //                {
        //                    stream_11821.Write(data_11821, 0, data_11821.Length);
        //                }
        //                HttpWebResponse response_11821 = (HttpWebResponse)httpWReq_11821.GetResponse();
        //                StreamReader reader_11821 = new StreamReader(response_11821.GetResponseStream());
        //                string responseString_11821 = reader_11821.ReadToEnd();
        //                reader_11821.Close();
        //                response_11821.Close();
        //                break;
        //            case "11841": // Om Jewellers
        //                string date_11841 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11841 = "text";
        //                StringBuilder sbposdata_11841 = new StringBuilder();
        //                sbposdata_11841.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11841.AppendFormat("&password={0}", _Password);
        //                sbposdata_11841.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11841.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11841.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11841.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11841.AppendFormat("&msgType={0}", type_11841);
        //                sbposdata_11841.AppendFormat("&format={0}", type_11841);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11841 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11841 = new UTF8Encoding();
        //                byte[] data_11841 = encoding_11841.GetBytes(sbposdata_11841.ToString());
        //                httpWReq_11841.Method = "POST";
        //                httpWReq_11841.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11841.ContentLength = data_11841.Length;
        //                using (Stream stream_11841 = httpWReq_11841.GetRequestStream())
        //                {
        //                    stream_11841.Write(data_11841, 0, data_11841.Length);
        //                }
        //                HttpWebResponse response_11841 = (HttpWebResponse)httpWReq_11841.GetResponse();
        //                StreamReader reader_11841 = new StreamReader(response_11841.GetResponseStream());
        //                string responseString_11841 = reader_11841.ReadToEnd();
        //                reader_11841.Close();
        //                response_11841.Close();
        //                break;
        //            case ""://VisionHLT English
        //                var httpWebRequest_11671 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_11671.ContentType = "application/json";
        //                httpWebRequest_11671.Method = "POST";

        //                using (var streamWriter_11671 = new StreamWriter(httpWebRequest_11671.GetRequestStream()))
        //                {

        //                    string json_11671 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_11671.Write(json_11671);
        //                }

        //                var httpResponse_11671 = (HttpWebResponse)httpWebRequest_11671.GetResponse();
        //                using (var streamReader_11671 = new StreamReader(httpResponse_11671.GetResponseStream()))
        //                {
        //                    var result_11671 = streamReader_11671.ReadToEnd();
        //                }

        //                break;
        //            case "12061": // Shuddhohum Jewellers
        //                string date_12061 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_12061 = "text";
        //                StringBuilder sbposdata_12061 = new StringBuilder();
        //                sbposdata_12061.AppendFormat("userid={0}", _UserName);
        //                sbposdata_12061.AppendFormat("&password={0}", _Password);
        //                sbposdata_12061.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_12061.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_12061.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_12061.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_12061.AppendFormat("&msgType={0}", type_12061);
        //                sbposdata_12061.AppendFormat("&format={0}", type_12061);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_12061 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_12061 = new UTF8Encoding();
        //                byte[] data_12061 = encoding_12061.GetBytes(sbposdata_12061.ToString());
        //                httpWReq_12061.Method = "POST";
        //                httpWReq_12061.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_12061.ContentLength = data_12061.Length;
        //                using (Stream stream_12061 = httpWReq_12061.GetRequestStream())
        //                {
        //                    stream_12061.Write(data_12061, 0, data_12061.Length);
        //                }
        //                HttpWebResponse response_12061 = (HttpWebResponse)httpWReq_12061.GetResponse();
        //                StreamReader reader_12061 = new StreamReader(response_12061.GetResponseStream());
        //                string responseString_12061 = reader_12061.ReadToEnd();
        //                reader_12061.Close();
        //                response_12061.Close();
        //                break;
        //            case "12081"://NVDeviSaraf
        //                var httpWebRequest_12081 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12081.ContentType = "application/json";
        //                httpWebRequest_12081.Method = "POST";

        //                using (var streamWriter_12081 = new StreamWriter(httpWebRequest_12081.GetRequestStream()))
        //                {

        //                    string json_12081 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12081.Write(json_12081);
        //                }

        //                var httpResponse_12081 = (HttpWebResponse)httpWebRequest_12081.GetResponse();
        //                using (var streamReader_12081 = new StreamReader(httpResponse_12081.GetResponseStream()))
        //                {
        //                    var result_12081 = streamReader_12081.ReadToEnd();
        //                }

        //                break;
        //            case "10741"://MBAshtekar
        //                var httpWebRequest_10741 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_10741.ContentType = "application/json";
        //                httpWebRequest_10741.Method = "POST";

        //                using (var streamWriter_10741 = new StreamWriter(httpWebRequest_10741.GetRequestStream()))
        //                {

        //                    string json_10741 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_10741.Write(json_10741);
        //                }

        //                var httpResponse_10741 = (HttpWebResponse)httpWebRequest_10741.GetResponse();
        //                using (var streamReader_10741 = new StreamReader(httpResponse_10741.GetResponseStream()))
        //                {
        //                    var result_10741 = streamReader_10741.ReadToEnd();
        //                }

        //                break;
        //            case "11251": // HemantJewellers
        //                string date_11251 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11251 = "text";
        //                StringBuilder sbposdata_11251 = new StringBuilder();
        //                sbposdata_11251.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11251.AppendFormat("&password={0}", _Password);
        //                sbposdata_11251.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11251.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11251.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11251.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11251.AppendFormat("&msgType={0}", type_11251);
        //                sbposdata_11251.AppendFormat("&format={0}", type_11251);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11251 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11251 = new UTF8Encoding();
        //                byte[] data_11251 = encoding_11251.GetBytes(sbposdata_11251.ToString());
        //                httpWReq_11251.Method = "POST";
        //                httpWReq_11251.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11251.ContentLength = data_11251.Length;
        //                using (Stream stream_11251 = httpWReq_11251.GetRequestStream())
        //                {
        //                    stream_11251.Write(data_11251, 0, data_11251.Length);
        //                }
        //                HttpWebResponse response_11251 = (HttpWebResponse)httpWReq_11251.GetResponse();
        //                StreamReader reader_11251 = new StreamReader(response_11251.GetResponseStream());
        //                string responseString_11251 = reader_11251.ReadToEnd();
        //                reader_11251.Close();
        //                response_11251.Close();
        //                break;
        //            case "12051"://HKSarafSons
        //                var httpWebRequest_12051 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12051.ContentType = "application/json";
        //                httpWebRequest_12051.Method = "POST";

        //                using (var streamWriter_12051 = new StreamWriter(httpWebRequest_12051.GetRequestStream()))
        //                {

        //                    string json_12051 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12051.Write(json_12051);
        //                }

        //                var httpResponse_12051 = (HttpWebResponse)httpWebRequest_12051.GetResponse();
        //                using (var streamReader_12051 = new StreamReader(httpResponse_12051.GetResponseStream()))
        //                {
        //                    var result_12051 = streamReader_12051.ReadToEnd();
        //                }

        //                break;
        //            case "12181"://Darshan
        //                var httpWebRequest_12181 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12181.ContentType = "application/json";
        //                httpWebRequest_12181.Method = "POST";
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                using (var streamWriter_12181 = new StreamWriter(httpWebRequest_12181.GetRequestStream()))
        //                {

        //                    string json_12181 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12181.Write(json_12181);
        //                }

        //                var httpResponse_12181 = (HttpWebResponse)httpWebRequest_12181.GetResponse();
        //                using (var streamReader_12181 = new StreamReader(httpResponse_12181.GetResponseStream()))
        //                {
        //                    var result_12181 = streamReader_12181.ReadToEnd();
        //                }

        //                break;
        //            case "12021"://JyotichandBhaichandSaraf
        //                var httpWebRequest_12021 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12021.ContentType = "application/json";
        //                httpWebRequest_12021.Method = "POST";

        //                using (var streamWriter_12021 = new StreamWriter(httpWebRequest_12021.GetRequestStream()))
        //                {

        //                    string json_12021 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12021.Write(json_12021);
        //                }

        //                var httpResponse_12021 = (HttpWebResponse)httpWebRequest_12021.GetResponse();
        //                using (var streamReader_12021 = new StreamReader(httpResponse_12021.GetResponseStream()))
        //                {
        //                    var result_12021 = streamReader_12021.ReadToEnd();
        //                }

        //                break;
        //            case "11801": // JK Devi Jewellers
        //                string date_11801 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11801 = "text";
        //                StringBuilder sbposdata_11801 = new StringBuilder();
        //                sbposdata_11801.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11801.AppendFormat("&password={0}", _Password);
        //                sbposdata_11801.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11801.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11801.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11801.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11801.AppendFormat("&msgType={0}", type_11801);
        //                sbposdata_11801.AppendFormat("&format={0}", type_11801);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11801 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11801 = new UTF8Encoding();
        //                byte[] data_11801 = encoding_11801.GetBytes(sbposdata_11801.ToString());
        //                httpWReq_11801.Method = "POST";
        //                httpWReq_11801.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11801.ContentLength = data_11801.Length;
        //                using (Stream stream_11801 = httpWReq_11801.GetRequestStream())
        //                {
        //                    stream_11801.Write(data_11801, 0, data_11801.Length);
        //                }
        //                HttpWebResponse response_11801 = (HttpWebResponse)httpWReq_11801.GetResponse();
        //                StreamReader reader_11801 = new StreamReader(response_11801.GetResponseStream());
        //                string responseString_11801 = reader_11801.ReadToEnd();
        //                reader_11801.Close();
        //                response_11801.Close();
        //                break;
        //            case "11781": // Londe Jewellers
        //                string date_11781 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type_11781 = "unicode";
        //                StringBuilder sbposdata_11781 = new StringBuilder();
        //                sbposdata_11781.AppendFormat("userid={0}", _UserName);
        //                sbposdata_11781.AppendFormat("&password={0}", _Password);
        //                sbposdata_11781.AppendFormat("&sendMethod={0}", "quick");
        //                sbposdata_11781.AppendFormat("&mobile={0}", _MobileNo);
        //                sbposdata_11781.AppendFormat("&msg={0}", _MobileMessage);
        //                sbposdata_11781.AppendFormat("&senderid={0}", _Sender);
        //                sbposdata_11781.AppendFormat("&msgType={0}", type_11781);
        //                sbposdata_11781.AppendFormat("&format={0}", type_11781);
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq_11781 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding_11781 = new UTF8Encoding();
        //                byte[] data_11781 = encoding_11781.GetBytes(sbposdata_11781.ToString());
        //                httpWReq_11781.Method = "POST";
        //                httpWReq_11781.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq_11781.ContentLength = data_11781.Length;
        //                using (Stream stream_11781 = httpWReq_11781.GetRequestStream())
        //                {
        //                    stream_11781.Write(data_11781, 0, data_11781.Length);
        //                }
        //                HttpWebResponse response_11781 = (HttpWebResponse)httpWReq_11781.GetResponse();
        //                StreamReader reader_11781 = new StreamReader(response_11781.GetResponseStream());
        //                string responseString_11781 = reader_11781.ReadToEnd();
        //                reader_11781.Close();
        //                response_11781.Close();
        //                break;
        //            case "12221"://OmkarGoldandSilverPalace
        //                var httpWebRequest_12221 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12221.ContentType = "application/json";
        //                httpWebRequest_12221.Method = "POST";

        //                using (var streamWriter_12221 = new StreamWriter(httpWebRequest_12221.GetRequestStream()))
        //                {

        //                    string json_12221 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"8\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12221.Write(json_12221);
        //                }

        //                var httpResponse_12221 = (HttpWebResponse)httpWebRequest_12221.GetResponse();
        //                using (var streamReader_12221 = new StreamReader(httpResponse_12221.GetResponseStream()))
        //                {
        //                    var result_12221 = streamReader_12221.ReadToEnd();
        //                }

        //                break;
        //            case "12351"://OdhekarJeweller
        //                var httpWebRequest_12351 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12351.ContentType = "application/json";
        //                httpWebRequest_12351.Method = "POST";
        //                using (var streamWriter_12351 = new StreamWriter(httpWebRequest_12351.GetRequestStream()))
        //                {
        //                    string json_12351 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12351.Write(json_12351);
        //                }
        //                var httpResponse_12351 = (HttpWebResponse)httpWebRequest_12351.GetResponse();
        //                using (var streamReader_12351 = new StreamReader(httpResponse_12351.GetResponseStream()))
        //                {
        //                    var result_12351 = streamReader_12351.ReadToEnd();
        //                }
        //                break;
        //            case "12381"://KheradkarSaraf
        //                var httpWebRequest_12381 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12381.ContentType = "application/json";
        //                httpWebRequest_12381.Method = "POST";
        //                using (var streamWriter_12381 = new StreamWriter(httpWebRequest_12381.GetRequestStream()))
        //                {
        //                    string json_12381 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12381.Write(json_12381);
        //                }
        //                var httpResponse_12381 = (HttpWebResponse)httpWebRequest_12381.GetResponse();
        //                using (var streamReader_12381 = new StreamReader(httpResponse_12381.GetResponseStream()))
        //                {
        //                    var result_12381 = streamReader_12381.ReadToEnd();
        //                }
        //                break;
        //            case "12441"://SBJewellersBaramatikar
        //                var httpWebRequest_12441 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12441.ContentType = "application/json";
        //                httpWebRequest_12441.Method = "POST";
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                using (var streamWriter_12441 = new StreamWriter(httpWebRequest_12441.GetRequestStream()))
        //                {
        //                    string json_12441 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12441.Write(json_12441);
        //                }
        //                var httpResponse_12441 = (HttpWebResponse)httpWebRequest_12441.GetResponse();
        //                using (var streamReader_12441 = new StreamReader(httpResponse_12441.GetResponseStream()))
        //                {
        //                    var result_12441 = streamReader_12441.ReadToEnd();
        //                }
        //                break;
        //            case "11481"://MTD
        //                var httpWebRequest_11481 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_11481.ContentType = "application/json";
        //                httpWebRequest_11481.Method = "POST";
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                using (var streamWriter_11481 = new StreamWriter(httpWebRequest_11481.GetRequestStream()))
        //                {
        //                    string json_11481 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"0\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_11481.Write(json_11481);
        //                }
        //                var httpResponse_11481 = (HttpWebResponse)httpWebRequest_11481.GetResponse();
        //                using (var streamReader_11481 = new StreamReader(httpResponse_11481.GetResponseStream()))
        //                {
        //                    var result_11481 = streamReader_11481.ReadToEnd();
        //                }
        //                break;
        //            case "12531"://OmarsonsJewellers Pinnacle
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                var httpWebRequest_12531 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12531.ContentType = "application/json";
        //                httpWebRequest_12531.Headers.Add("Apikey", _Password);
        //                httpWebRequest_12531.Method = "POST";

        //                using (var streamWriter_12531 = new StreamWriter(httpWebRequest_12531.GetRequestStream()))
        //                {

        //                    string json_12531 = "{\"sender\":\"" + _Sender + "\"," +
        //                    "\"message\":[{\"number\":\"91" + _MobileNo + "\"," +
        //                     "\"text\":\"" + _MobileMessage + "\"}]," + "\"messagetype\":\"TXT\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
        //                    streamWriter_12531.Write(json_12531);
        //                }

        //                var httpResponse_12531 = (HttpWebResponse)httpWebRequest_12531.GetResponse();
        //                using (var streamReader_12531 = new StreamReader(httpResponse_12531.GetResponseStream()))
        //                {
        //                    var result_12531 = streamReader_12531.ReadToEnd();
        //                }
        //                break;
        //            case "12611"://BedreBandhuSuvarnkar
        //                var httpWebRequest_12611 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12611.ContentType = "application/json";
        //                httpWebRequest_12611.Method = "POST";
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                // _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                using (var streamWriter_12611 = new StreamWriter(httpWebRequest_12611.GetRequestStream()))
        //                {
        //                    string json_12611 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"8\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12611.Write(json_12611);
        //                }
        //                var httpResponse_12611 = (HttpWebResponse)httpWebRequest_12611.GetResponse();
        //                using (var streamReader_12611 = new StreamReader(httpResponse_12611.GetResponseStream()))
        //                {
        //                    var result_12611 = streamReader_12611.ReadToEnd();
        //                }
        //                break;
        //            case "12641"://RatnaparkhiJewellers
        //                var httpWebRequest_12641 = (HttpWebRequest)WebRequest.Create(_Url);
        //                httpWebRequest_12641.ContentType = "application/json";
        //                httpWebRequest_12641.Method = "POST";
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                // _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                using (var streamWriter_12641 = new StreamWriter(httpWebRequest_12641.GetRequestStream()))
        //                {
        //                    string json_12641 = "{\"Account\":" +
        //                                    "{\"APIKey\":\"" + _Password + "\"," +
        //                                    "\"SenderId\":\"" + _Sender + "\"," +
        //                                    "\"Channel\":\"Trans\"," +
        //                                    "\"DCS\":\"8\"," +
        //                                    "\"SchedTime\":null," +
        //                                    "\"GroupId\":null}," +
        //                                    "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
        //                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
        //                                    "}";
        //                    streamWriter_12641.Write(json_12641);
        //                }
        //                var httpResponse_12641 = (HttpWebResponse)httpWebRequest_12641.GetResponse();
        //                using (var streamReader_12641 = new StreamReader(httpResponse_12641.GetResponseStream()))
        //                {
        //                    var result_12641 = streamReader_12641.ReadToEnd();
        //                }
        //                break;
        //            default:
        //                _MobileMessage = _MobileMessage.Replace("#99", "&");
        //                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
        //                string type1 = "TEXT";
        //                StringBuilder sbposdata1 = new StringBuilder();
        //                sbposdata1.AppendFormat("username={0}", _UserName);
        //                sbposdata1.AppendFormat("&password={0}", _Password);
        //                sbposdata1.AppendFormat("&to={0}", _MobileNo);
        //                sbposdata1.AppendFormat("&from={0}", _Sender);
        //                sbposdata1.AppendFormat("&text={0}", _MobileMessage);
        //                sbposdata1.AppendFormat("&dlr-mask={0}", "19");
        //                sbposdata1.AppendFormat("&dlr-url");
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
        //                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //                HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(_Url);
        //                UTF8Encoding encoding1 = new UTF8Encoding();
        //                byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
        //                httpWReq1.Method = "POST";
        //                httpWReq1.ContentType = "application/x-www-form-urlencoded";
        //                httpWReq1.ContentLength = data1.Length;
        //                using (Stream stream1 = httpWReq1.GetRequestStream())
        //                {
        //                    stream1.Write(data1, 0, data1.Length);
        //                }
        //                HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
        //                StreamReader reader1 = new StreamReader(response1.GetResponseStream());
        //                string responseString1 = reader1.ReadToEnd();
        //                reader1.Close();
        //                response1.Close();
        //                break;
        //        }
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        _responseData = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
        //    }
        //    catch (WebException ex)
        //    {
        //        _responseData = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _responseData = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
        //    }
        //}

    }

}
