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

namespace BOTS_BL.Repository
{
    public class RetailerWebRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();

        public List<DynamicFieldInfo>[] DynamicData(string connectionString)
        {
            DynamicFieldInfo Obj = new DynamicFieldInfo();
            

            //List<DynamicFieldInfo> objListDymanic = new List<DynamicFieldInfo>();


            List<DynamicFieldInfo> objList2 = new List<DynamicFieldInfo>();
            List<DynamicFieldInfo>[] Obj1 = new List<DynamicFieldInfo>[1000];
            //List<JSONDATA> DynJson = new List<JSONDATA>();

            JSONDATA J = new JSONDATA();




            SqlConnection _Con = new SqlConnection(connectionString);
            DataSet retVal = new DataSet();
            DataTable Tbl = new DataTable();
            SqlCommand cmdReport = new SqlCommand("sp_DynamicDataJSON", _Con);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

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

                List<DynamicFieldInfo> objListDymanic  = new List<DynamicFieldInfo>();

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

             return Obj1;
        }
        public CustomerDetails GetCustomerDetails(string CounterId, string MobileNo)
        {
            CustomerDetails objData = new CustomerDetails();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

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
            return objData;
        }

        public EarnResponse InsertEarnData(string CounterId, string Mobileno, string CustomerName, string InvoiceNo, string InvoiceAmt, string DOB, string EmailId, string Gender, string ADate, string CardNo)
        {
            EarnResponse R = new EarnResponse();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

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
                    SqlParameter param5 = new SqlParameter("pi_DOB", DOB);
                    SqlParameter param6 = new SqlParameter("pi_EmailId", EmailId);
                    SqlParameter param7 = new SqlParameter("pi_CustomerName", CustomerName);
                    SqlParameter param8 = new SqlParameter("pi_CardNo", CardNo);
                    SqlParameter param9 = new SqlParameter("pi_Gender", Gender);
                    SqlParameter param10 = new SqlParameter("pi_Anniversary", ADate);
                    SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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

                        if(dt2.Rows.Count > 0)
                        {
                            string SMSStatus = Convert.ToString(dt2.Rows[0]["SMSStatusTxn"]);
                            string WAStatus = Convert.ToString(dt2.Rows[0]["WAStatusTxn"]);
                            //if (SMSStatus == "1" && WAStatus == "1")
                            //{

                            //}
                            DataTable dt3 = retVal.Tables[3];
                            Thread _job = new Thread(() => SendMessage(dt3));
                            _job.Start();
                        }


                    }
                }
            }
            return R;
        }

        public EarnResponse InsertEarnDataOld(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt,string DynamicData)
        {
            EarnResponse R = new EarnResponse();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

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
                    SqlParameter param5 = new SqlParameter("pi_DOB", "");
                    SqlParameter param6 = new SqlParameter("pi_EmailId", "");
                    SqlParameter param7 = new SqlParameter("pi_CustomerName", "");
                    SqlParameter param8 = new SqlParameter("pi_CardNo", "");
                    SqlParameter param9 = new SqlParameter("pi_Gender", "");
                    SqlParameter param10 = new SqlParameter("pi_Anniversary", "");
                    //SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss"));
                    SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    SqlParameter param12 = new SqlParameter("pi_json", DynamicData);

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
                            //if (SMSStatus == "1" && WAStatus == "1")
                            //{

                            //}
                            DataTable dt3 = retVal.Tables[3];
                            Thread _job = new Thread(() => SendMessage(dt3));
                            _job.Start();
                        }
                    }
                    else
                    {
                        R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                        R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                    }
                }
            }
            return R;
        }

        public BurnValidationResponse BurnValidation(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt, string PointsBurn)
        {
            BurnValidationResponse R = new BurnValidationResponse();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

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
                    SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);
                    cmdReport.Parameters.Add(param2);
                    cmdReport.Parameters.Add(param3);
                    cmdReport.Parameters.Add(param4);
                    cmdReport.Parameters.Add(param5);
                    cmdReport.Parameters.Add(param11);
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
                            //if (SMSStatus == "1" && WAStatus == "1")
                            //{

                            //}
                            DataTable dt3 = retVal.Tables[3];
                            Thread _job = new Thread(() => SendMessageOTP(dt3));
                            _job.Start();
                        }
                    }
                    else
                    {
                        R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                        R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                    }
                }
            }
            return R;
        }

        public BurnResponse SaveBurnTxn(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt, string PointsBurn)
        {
            BurnResponse R = new BurnResponse();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

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

                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);
                    cmdReport.Parameters.Add(param2);
                    cmdReport.Parameters.Add(param3);
                    cmdReport.Parameters.Add(param4);
                    cmdReport.Parameters.Add(param5);
                    cmdReport.Parameters.Add(param11);
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
                            //if (SMSStatus == "1" && WAStatus == "1")
                            //{

                            //}
                            DataTable dt3 = retVal.Tables[3];
                            Thread _job = new Thread(() => SendMessage(dt3));
                            _job.Start();
                        }
                    }
                    else
                    {
                        R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                        R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                    }
                }
            }
            return R;
        }

        public List<DailyReport> DailyReport(string CounterId)
        {
            List<DailyReport> LtObj = new List<DailyReport>();
            DailyReport obj = new DailyReport();

            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                string outletId = CounterId.Substring(0, 8);
                var conStr = CR.GetCustomerConnString(groupId);

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
            return LtObj;
        }

        public MDRDetails MembDetailReprt(string CounterId, string Mobileno)
        {
            MDRDetails ObjMDRDetails = new MDRDetails();
            MDRResponse objMDRResponse = new MDRResponse();
            MDRData objMDRData = new MDRData();
            List<MDRTxnDetails> LstMDRData = new List<MDRTxnDetails>();

            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                string outletId = CounterId.Substring(0, 8);
                var conStr = CR.GetCustomerConnString(groupId);

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
                                objMDRData.EnrolledOn = Convert.ToString(dt1.Rows[i]["EnrolledOn"]);
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
            return ObjMDRDetails;
        }

        public GTDTxnDetails GetTxnDetailsRepo(string CounterId, string Mobileno)
        {
            GTDTxnDetails ObjGTDDetails = new GTDTxnDetails();
            Response objResponse = new Response();
            GTDData objGTDData = new GTDData();
            List<GTDTxnData> LstGTDTxn = new List<GTDTxnData>();

            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                string outletId = CounterId.Substring(0, 8);
                var conStr = CR.GetCustomerConnString(groupId);

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
            return ObjGTDDetails;
        }

        public CancelData CancelTxn(string CounterId, string Mobileno, string InvoiceNo)
        {
            CancelTxnDetails ObjCancel = new CancelTxnDetails();
            Response objResponse = new Response();
            CancelData ObjCanData = new CancelData();

            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                string outletId = CounterId.Substring(0, 8);
                var conStr = CR.GetCustomerConnString(groupId);

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
                            //if (SMSStatus == "1" && WAStatus == "1")
                            //{

                            //}
                            DataTable dt3 = retVal.Tables[3];
                            Thread _job = new Thread(() => SendMessage(dt3));
                            _job.Start();
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
            return ObjCanData;
        }
        public BurnValidationResponse OTPResend(string CounterId, string Mobileno)
        {
            BurnValidationResponse R = new BurnValidationResponse();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

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
                            //if (SMSStatus == "1" && WAStatus == "1")
                            //{

                            //}
                            DataTable dt3 = retVal.Tables[3];
                            Thread _job = new Thread(() => SendMessageOTP(dt3));
                            _job.Start();
                        }

                    }
                    else
                    {
                        R.ResponseCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                        R.ResponseMessage = Convert.ToString(dt.Rows[0]["ResponseMessage"]);
                    }
                }
            }
            return R;
        }
        public void SendMessage(DataTable dt3)
        {
            string _MobileNo = dt3.Rows[0]["CommMobileNoTxn"].ToString();
            string _WATokenId = dt3.Rows[0]["WATokenId"].ToString();
            string _WAMessage = dt3.Rows[0]["WhatsAppMessage"].ToString();

                Thread _job = new Thread(() => WAText(_MobileNo, _WATokenId, _WAMessage));
                _job.Start();
            
        }
        public void SendMessageOTP(DataTable dt3)
        {
            string _MobileNo = dt3.Rows[0]["CommMobileNoOTP"].ToString();
            string _WATokenId = dt3.Rows[0]["WATokenId"].ToString();
            string _WAMessage = dt3.Rows[0]["MessageOTP"].ToString();

            Thread _job = new Thread(() => WAText(_MobileNo, _WATokenId, _WAMessage));
            _job.Start();

        }
        public void WAText(string MobileNo,string WATokenId,string WAMessage)
        {
            string responseString;
            try
            {

                WAMessage = WAMessage.Replace("#99", "&");
                WAMessage = HttpUtility.UrlEncode(WAMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", WATokenId);
                sbposdata.AppendFormat("&phone=91{0}", MobileNo);
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
    }
    
}
