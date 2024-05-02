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
        string Stringdtmessage;
        int Status;

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
                            objData.PointBalance = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);

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

                    using (var tempcontext = new CommonDBContext())
                    {
                        Status = tempcontext.tblDatabaseDetails.Where(x => x.GroupId == groupId).Count();
                    }

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

                        if(Status > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                DataTable dt1 = retVal.Tables[1];
                                DataTable dt2 = retVal.Tables[2];
                                R.ResponseCode = "00";
                                R.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                                R.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                                R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                                R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);

                                Thread _JobMessage = new Thread(() => MessageDataTable(dt2));
                                _JobMessage.Start();
                            }
                            else
                            {
                                R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            }

                        }
                        else
                        {
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

                                        if (groupId == "1007")
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

                    using (var tempcontext = new CommonDBContext())
                    {
                        Status = tempcontext.tblDatabaseDetails.Where(x => x.GroupId == groupId).Count();
                    }

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

                        if (Status > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                DataTable dt1 = retVal.Tables[1];
                                DataTable dt2 = retVal.Tables[2];
                                R.ResponseCode = "00";
                                R.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                                R.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                                R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                                R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);

                                Thread _JobMessage = new Thread(() => MessageDataTable(dt2));
                                _JobMessage.Start();
                            }
                            else
                            {
                                R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            }
                        }
                        else
                        {
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

                    using (var tempcontext = new CommonDBContext())
                    {
                        Status = tempcontext.tblDatabaseDetails.Where(x => x.GroupId == groupId).Count();
                    }

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

                        if (Status > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                DataTable dt1 = retVal.Tables[1];
                                DataTable dt2 = retVal.Tables[2];
                                R.ResponseCode = "00";
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                                R.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                                R.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["BurnPointsAsAmount"]);
                                R.PointsValue = Convert.ToString(dt1.Rows[0]["PointsValue"]);

                                Thread _JobMessage = new Thread(() => MessageDataTable(dt2));
                                _JobMessage.Start();
                            }
                            else
                            {
                                R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            }

                        }
                        else
                        {
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

                    using (var tempcontext = new CommonDBContext())
                    {
                        Status = tempcontext.tblDatabaseDetails.Where(x => x.GroupId == groupId).Count();
                    }

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

                        if (Status > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                
                                    DataTable dt1 = retVal.Tables[1];
                                    DataTable dt2 = retVal.Tables[2];
                                    R.ResponseCode = "00";
                                    R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                                R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);
                                R.PointsRedeemed = Convert.ToString(dt1.Rows[0]["PointsRedeemed"]);
                                R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);

                                Thread _JobMessage = new Thread(() => MessageDataTable(dt2));
                                    _JobMessage.Start();
                                
                            }
                            else
                            {
                                R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            }
                        }
                        else
                        {
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

                    using (var tempcontext = new CommonDBContext())
                    {
                        Status = tempcontext.tblDatabaseDetails.Where(x => x.GroupId == groupId).Count();
                    }


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

                        if (Status > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                dt1 = retVal.Tables[1];
                                dt2 = retVal.Tables[2];
                                objResponse.ResponseCode = "00";
                                objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                                ObjCancel.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                                ObjCancel.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                                ObjCancel.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                                ObjCancel.InvoiceNo = Convert.ToString(dt1.Rows[0]["InvoiceNo"]);
                                ObjCancel.PointsCredited = Convert.ToString(dt1.Rows[0]["PointsCredited"]);
                                ObjCancel.PointsDebited = Convert.ToString(dt1.Rows[0]["PointsDebited"]);

                                ObjCanData.Cancelresponse = objResponse;
                                ObjCanData.CancelTxn = ObjCancel;

                                Thread _JobMessage = new Thread(() => MessageDataTable(dt2));
                                _JobMessage.Start();
                            }
                            else
                            {
                                objResponse.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                objResponse.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);

                                ObjCanData.Cancelresponse = objResponse;
                            }
                        }
                        else
                        {
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


                    using (var tempcontext = new CommonDBContext())
                    {
                        Status = tempcontext.tblDatabaseDetails.Where(x => x.GroupId == groupId).Count();
                    }

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

                        if (Status > 0)
                        {
                            if(Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                R.ResponseCode = "00";
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                                DataTable dt1 = retVal.Tables[1];
                                DataTable dt2 = retVal.Tables[2];
                                R.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);

                                Thread _JobMessage = new Thread(() => MessageDataTable(dt2));
                                _JobMessage.Start();
                            }
                            else
                            {
                                R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            }
                        }
                        else
                        {
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
                    objData = context.tblMembershipDetails.Where(x => x.MobileNo == MobileNo && x.IsActive == true).OrderByDescending(y=>y.CreatedDate).FirstOrDefault();
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

        public BurnValidationResponse BurnValidationRatnaEnterprise(string CounterId, string Mobileno, string InvoiceAmt, string PointsBurn)
        {
            BurnValidationResponse R = new BurnValidationResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    string groupId = CounterId.Substring(0, 4);
                    var conStr = CR.GetRetailWebConnString(CounterId);

                    using (var tempcontext = new CommonDBContext())
                    {
                        Status = tempcontext.tblDatabaseDetails.Where(x => x.GroupId == groupId).Count();
                    }

                    SqlConnection _Con = new SqlConnection(conStr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_Web_BurnValidationMembership", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_CounterId", CounterId);
                        SqlParameter param2 = new SqlParameter("pi_MobileNo", Mobileno);
                        SqlParameter param3 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                        SqlParameter param4 = new SqlParameter("pi_BurnPoints", PointsBurn);
                        SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];

                        if (Status > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                R.ResponseCode = "00";
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                                DataTable dt1 = retVal.Tables[1];
                                DataTable dt2 = retVal.Tables[2];
                                R.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                                R.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["BurnPointsAsAmount"]);
                                R.PointsValue = Convert.ToString(dt1.Rows[0]["PointsValue"]);

                                Thread _JobMessage = new Thread(() => MessageDataTable(dt2));
                                _JobMessage.Start();
                            }
                            else
                            {
                                R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                                R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                            }
                        }
                        else
                        {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BurnValidation");
            }
            return R;
        }

        public bool SaveMembershipRedeemPoints(string CounterId, string Mobileno, tblMembershipDetail ObjData, string RedeemPoints)
        {
            bool Status;

            Status = default;

            string groupId = CounterId.Substring(0, 4);
            var conStr = CR.GetRetailWebConnString(CounterId);

            tblMembershipDetail objMemberAdd = new tblMembershipDetail();


            using (var context = new BOTSDBContext(conStr))
            {
                var obj = context.tblMembershipDetails.Where(x => x.MobileNo == Mobileno && x.IsActive == true).FirstOrDefault();
                obj.IsActive = false;
                context.SaveChanges();

                var objPoints = context.tblCustPointsMasters.Where(x => x.MobileNo == Mobileno && x.IsActive == true && x.PointsDesc == "Membership").FirstOrDefault();
                objPoints.Points = objPoints.Points - Convert.ToDecimal(RedeemPoints);
                context.SaveChanges();

                context.tblMembershipDetails.Add(ObjData);
                context.SaveChanges();

                Status = true;

            }

            return Status;
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
