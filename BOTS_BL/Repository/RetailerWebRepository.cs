using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.RetailerWeb;
using BOTS_BL.Models;
using System.Data.SqlClient;
using System.Data;

namespace BOTS_BL.Repository
{
    public class RetailerWebRepository
    {
        CustomerRepository CR = new CustomerRepository();
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
                    SqlParameter param1 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));
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
                        objData.MobileNo = Convert.ToString(dt2.Rows[0]["MobileNo"]);
                        objData.CustomerName = Convert.ToString(dt2.Rows[0]["CustomerName"]);
                        objData.PointBalance = Convert.ToString(dt2.Rows[0]["AvailablePoints"]);

                        objData.CardNo = Convert.ToString(dt1.Rows[0]["LoyaltyCard"]);
                        objData.TotalSpend = Convert.ToString(dt1.Rows[0]["TotalSpendText"]);
                        objData.LastTxnDate = Convert.ToString(dt1.Rows[0]["LastTxnText"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "06")
                    {
                        objData.ResponseCode = "06";
                    }
                }
            }
            return objData;
        }

        public EarnResponse InsertEarnData(string CounterId, string Mobileno,string CustomerName,string InvoiceNo,string InvoiceAmt,string DOB, string EmailId, string Gender, string ADate, string CardNo )
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
                    SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));
                    
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
                        //DataTable dt2 = retVal.Tables[2];
                        R.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                        R.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                        R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                        R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);
                    }
                }
            }
            return R;
        }

        public EarnResponse InsertEarnDataOld(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt)
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
                    SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));

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
                        //DataTable dt2 = retVal.Tables[2];
                        R.MobileNo = Convert.ToString(dt1.Rows[0]["MobileNo"]);
                        R.CustomerName = Convert.ToString(dt1.Rows[0]["CustomerName"]);
                        R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
                        R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);
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

        public BurnValidationResponse BurnValidation(string CounterId, string Mobileno, string InvoiceNo, string InvoiceAmt,string PointsBurn)
        {
            BurnValidationResponse R = new BurnValidationResponse();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

                SqlConnection _Con = new SqlConnection(conStr);
                DataSet retVal = new DataSet();
                SqlCommand cmdReport = new SqlCommand("sp_WAb_BurnValidation", _Con);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    SqlParameter param1  = new SqlParameter("pi_CounterId", CounterId);
                    SqlParameter param2  = new SqlParameter("pi_MobileNo", Mobileno);
                    SqlParameter param3  = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                    SqlParameter param4  = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                    SqlParameter param5  = new SqlParameter("pi_BurnPoints", PointsBurn);
                    SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));

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
                        //DataTable dt2 = retVal.Tables[2];
                        R.OTPValue = Convert.ToString(dt1.Rows[0]["OTPValue"]);
                        R.BurnPointsAsAmount = Convert.ToString(dt1.Rows[0]["BurnPointsAsAmount"]);
                        R.PointsValue = Convert.ToString(dt1.Rows[0]["PointsValue"]);
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
                    SqlParameter param11 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));

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
                        //DataTable dt2 = retVal.Tables[2];
                        R.PointsEarned = Convert.ToString(dt1.Rows[0]["PointsEarned"]);
                        R.PointsRedeemed = Convert.ToString(dt1.Rows[0]["PointsRedeemed"]);
                        R.AvailablePoints = Convert.ToString(dt1.Rows[0]["AvailablePoints"]);
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
            List<DailyReport> LtObj= new List<DailyReport>();
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
                    SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);
                    //cmdReport.Parameters.Add(param2);
                    cmdReport.Parameters.Add(param3);
                    cmdReport.Parameters.Add(param4);
                    cmdReport.Parameters.Add(param5);
                    daReport.Fill(retVal);

                    DataTable dt = retVal.Tables[0];
                    for (int i = 0; i< 1;i++)
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

        public MDRDetails MembDetailReprt(string CounterId,string Mobileno)
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
                    SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));
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
                            if(i == 0)
                            {
                                //ObjMDRDetails.ObjMDRResponse.ResponseCode = Convert.ToString(dt.Rows[i]["ResponseCode"]);
                                objMDRResponse.ResponseCode =  "00";
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
                    SqlParameter param5 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));
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
    }
}
