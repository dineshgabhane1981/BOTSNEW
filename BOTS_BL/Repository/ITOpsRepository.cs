using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using System.Threading;
using System.Data;
using System.Web;
using System.Net;

namespace BOTS_BL.Repository
{

    public class ITOpsRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public string GetCustomerAdminEmail(string GroupId)
        {
            string emailId = "";
            try
            {
                using (var context = new CommonDBContext())
                {
                    emailId = context.CustomerLoginDetails.Where(x => x.GroupId == GroupId).Select(y => y.EmailId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerAdminEmail");
            }
            return emailId;
        }
        public MemberData GetChangeNameByMobileNo(string GroupId, string searchData)
        {
            MemberData objMemberData = new MemberData();
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.MobileNo == searchData && x.Status == "00").FirstOrDefault();
                }
                if (objCustomerDetail != null)
                {
                    objMemberData.MemberName = objCustomerDetail.CustomerName;
                    objMemberData.MobileNo = objCustomerDetail.MobileNo;
                    objMemberData.OldMobileNo = objCustomerDetail.OldMobileNo;
                    objMemberData.CardNo = objCustomerDetail.CardNumber;
                    objMemberData.PointsBalance = objCustomerDetail.Points;
                    objMemberData.CustomerId = objCustomerDetail.CustomerId;
                    if (objCustomerDetail.DOJ.HasValue)
                    {
                        objMemberData.EnrolledOn = objCustomerDetail.DOJ.Value.ToString("dd/MM/yyyy");
                    }

                    using (var contextNew = new BOTSDBContext(connStr))
                    {
                        objMemberData.EnrolledOutletName = contextNew.OutletDetails.Where(x => x.OutletId == objCustomerDetail.EnrollingOutlet).Select(y => y.OutletName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameByMobileNo");
            }
            return objMemberData;
        }
        public MemberData GetChangeNameByCardNo(string GroupId, string searchData)
        {
            MemberData objMemberData = new MemberData();
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CardNumber == searchData && x.Status == "00").FirstOrDefault();
                }
                if (objCustomerDetail != null)
                {
                    objMemberData.MemberName = objCustomerDetail.CustomerName;
                    objMemberData.MobileNo = objCustomerDetail.MobileNo;
                    objMemberData.CardNo = objCustomerDetail.CardNumber;
                    objMemberData.PointsBalance = objCustomerDetail.Points;
                    objMemberData.CustomerId = objCustomerDetail.CustomerId;
                    if (objCustomerDetail.DOJ.HasValue)
                    {
                        objMemberData.EnrolledOn = objCustomerDetail.DOJ.Value.ToString("dd/MM/yyyy");
                    }

                    using (var contextNew = new BOTSDBContext(connStr))
                    {
                        objMemberData.EnrolledOutletName = contextNew.OutletDetails.Where(x => x.OutletId == objCustomerDetail.EnrollingOutlet).Select(y => y.OutletName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameByCardNo");
            }
            return objMemberData;
        }
        public bool UpdateSecurityKey(string GroupId, string counterId)
        {
            bool result = false;
            try
            {
                StoreDetail objstore = new StoreDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objstore = contextNew.StoreDetails.Where(x => x.CounterId == counterId).FirstOrDefault();
                    objstore.Status = "01";

                    contextNew.StoreDetails.AddOrUpdate(objstore);
                    contextNew.SaveChanges();

                    result = true;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateSecurityKey");
            }
            return result;
        }
        public bool UpdateNameOfMember(string GroupId, string CustomerId, string Name, tblAudit objAudit)
        {
            bool status = false;
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                    objCustomerDetail.CustomerName = Name;

                    contextNew.CustomerDetails.AddOrUpdate(objCustomerDetail);
                    contextNew.SaveChanges();

                    status = true;

                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateNameOfMember");
            }
            return status;
        }

        public SPResponse UpdateMobileOfMember(string GroupId, string CustomerId, string MobileNo, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                List<TransactionMaster> lstObjTxn = new List<TransactionMaster>();
                List<PointsExpiry> lstobjpoints = new List<PointsExpiry>();
                TransferPointsDetail objtransferPointsDetail = new TransferPointsDetail();

                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {

                    var objExisting = contextNew.CustomerDetails.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    if (objExisting == null)
                    {
                        objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                        string oldno = objCustomerDetail.MobileNo;
                        objCustomerDetail.MobileNo = MobileNo;
                        objCustomerDetail.OldMobileNo = oldno;

                        lstObjTxn = contextNew.TransactionMasters.Where(x => x.CustomerId == CustomerId).Take(10000).ToList();
                        if (lstObjTxn != null)
                        {
                            foreach (var trans in lstObjTxn)
                            {
                                trans.MobileNo = MobileNo;
                                contextNew.TransactionMasters.AddOrUpdate(trans);
                                contextNew.SaveChanges();
                            }
                        }

                        lstobjpoints = contextNew.PointsExpiries.Where(x => x.CustomerId == CustomerId).Take(10000).ToList();
                        if (lstobjpoints != null)
                        {
                            foreach (var points in lstobjpoints)
                            {
                                points.MobileNo = MobileNo;
                                contextNew.PointsExpiries.AddOrUpdate(points);
                                contextNew.SaveChanges();
                            }

                        }

                        objtransferPointsDetail.NewMobileNo = MobileNo;
                        objtransferPointsDetail.OldMobileNo = oldno;
                        objtransferPointsDetail.GroupId = GroupId;
                        objtransferPointsDetail.Datetime = DateTime.Now;
                        objtransferPointsDetail.Points = objCustomerDetail.Points;
                        contextNew.TransferPointsDetails.AddOrUpdate(objtransferPointsDetail);
                        contextNew.SaveChanges();

                        contextNew.CustomerDetails.AddOrUpdate(objCustomerDetail);
                        contextNew.SaveChanges();

                        result.ResponseCode = "00";
                        result.ResponseMessage = "Mobile Number Updated Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }


                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateMobileOfMember");
            }
            return result;
        }

        public SPResponse AddEarnData(string GroupId, string MobileNo, string Name, string OutletId, DateTime TxnDate, DateTime RequestDate, string InvoiceNo, string InvoiceAmt, string IsSMS, string Points, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);

                using (var contextNew = new BOTSDBContext(connStr))
                {
                    if (GroupId == "1226")
                    {
                        if (IsSMS == "True")
                        {
                            IsSMS = "1";
                        }
                        else
                        {
                            IsSMS = "0";
                        }
                        SqlConnection _Con = new SqlConnection(connStr);
                        DataSet DT = new DataSet();
                        SqlCommand cmdReport = new SqlCommand("sp_EarnRW_New_ITOPS", _Con);
                        SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                        using (cmdReport)
                        {
                            SqlParameter param1 = new SqlParameter("pi_MobileNo", MobileNo);
                            SqlParameter param2 = new SqlParameter("pi_OutletId", OutletId);
                            SqlParameter param3 = new SqlParameter("pi_TxnDate", TxnDate);
                            SqlParameter param4 = new SqlParameter("pi_RequestDate", RequestDate);
                            SqlParameter param5 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                            SqlParameter param6 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                            SqlParameter param7 = new SqlParameter("pi_LoginId", "");
                            SqlParameter param8 = new SqlParameter("pi_RequestBy", objAudit.RequestedBy);
                            SqlParameter param9 = new SqlParameter("pi_RequestedOnForum", objAudit.RequestedOnForum);
                            SqlParameter param10 = new SqlParameter("pi_SMSFlag", IsSMS);
                            SqlParameter param11 = new SqlParameter("pi_Points", Points);
                            SqlParameter param12 = new SqlParameter("pi_CustomerName", Name);

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

                            daReport.Fill(DT);

                            DataTable dt = DT.Tables[0];
                            string ResCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            result.ResponseCode = ResCode;

                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {

                                if (Convert.ToString(dt.Rows[0]["SMSFlag"]) == "1")
                                {
                                    string SMSStatus = Convert.ToString(dt.Rows[0]["SMSFlag"]);
                                    string WAStatus = Convert.ToString(dt.Rows[0]["WAStatus"]); //MobileNo
                                    string _MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                                    string _MobileMessage = Convert.ToString(dt.Rows[0]["Message"]);
                                    string _UserName = Convert.ToString(dt.Rows[0]["UserName"]);
                                    string _Password = Convert.ToString(dt.Rows[0]["Password"]);
                                    string _Sender = Convert.ToString(dt.Rows[0]["SenderId"]);
                                    string _Url = Convert.ToString(dt.Rows[0]["Url"]);
                                    string _WAMessage = Convert.ToString(dt.Rows[0]["WAMessage"]);
                                    string _WATokenId = Convert.ToString(dt.Rows[0]["WATokenId"]);


                                    if (SMSStatus == "1" && WAStatus == "1")
                                    {
                                        Thread _job = new Thread(() => SendSMSandWA(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _WAMessage, _WATokenId));
                                        _job.Start();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result = contextNew.Database.SqlQuery<SPResponse>("sp_EarnRW_New_ITOPS @pi_MobileNo, @pi_OutletId, @pi_TxnDate, @pi_RequestDate, @pi_InvoiceNo, @pi_InvoiceAmt, @pi_LoginId, @pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag,@pi_Points,@pi_CustomerName",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_OutletId", OutletId),
                              new SqlParameter("@pi_TxnDate", TxnDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_RequestDate", RequestDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_InvoiceNo", InvoiceNo),
                              new SqlParameter("@pi_InvoiceAmt", InvoiceAmt),
                              new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_RequestBy", objAudit.RequestedBy),
                              new SqlParameter("@pi_RequestedOnForum", objAudit.RequestedOnForum),
                              new SqlParameter("@pi_SMSFlag", IsSMS),
                              new SqlParameter("@pi_Points", Points),
                              new SqlParameter("@pi_CustomerName", Name)).FirstOrDefault<SPResponse>();
                    }



                    //DateTime.Now.ToString("yyyy-MM-dd")


                    //if (result.ResponseCode == "00")
                    //{
                    //   // status = true;
                    //   if(result.SMSFlag == "1")
                    //    {
                    //        string _MobileMessage = result.Message;
                    //        //Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _SMSBrandId));
                    //        //_job.Start();
                    //    }
                    //}
                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddEarnData");
            }

            return result;
        }
        //public SPResponse AddRedeemPointsData(string groupId, string mobileNo, string outletId, DateTime dateTime, DateTime now, string invoiceNumber, string invoiceAmount, decimal v1, string v2, string txnType, string v3, tblAudit objAudit)
        //{
        //    throw new NotImplementedException();
        //}

        public SPResponse AddRedeemPointsData(string GroupId, string MobileNo, string OutletId, DateTime TxnDate, DateTime RequestDate, string InvoiceNo, string InvoiceAmt, decimal Points, string IsSMS, string TxnType,string PartialEarnPoints, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    if (GroupId == "1226")
                    {
                        if (IsSMS == "True")
                        {
                            IsSMS = "1";
                        }
                        else
                        {
                            IsSMS = "0";
                        }
                        SqlConnection _Con = new SqlConnection(connStr);
                        DataSet DT = new DataSet();
                        SqlCommand cmdReport = new SqlCommand("sp_BurnRW_New_ITOPS", _Con);
                        SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                        using (cmdReport)
                        {
                            SqlParameter param1 = new SqlParameter("pi_MobileNo", MobileNo);
                            SqlParameter param2 = new SqlParameter("pi_OutletId", OutletId);
                            SqlParameter param3 = new SqlParameter("pi_TxnDate", TxnDate);
                            SqlParameter param4 = new SqlParameter("pi_RequestDate", RequestDate);
                            SqlParameter param5 = new SqlParameter("pi_InvoiceNo", InvoiceNo);
                            SqlParameter param6 = new SqlParameter("pi_InvoiceAmt", InvoiceAmt);
                            SqlParameter param7 = new SqlParameter("pi_RedeemPoints", Points);
                            SqlParameter param8 = new SqlParameter("pi_LoginId", "");
                            SqlParameter param9 = new SqlParameter("pi_PartialEarnPoints", PartialEarnPoints);
                            SqlParameter param10 = new SqlParameter("pi_RequestBy", objAudit.RequestedBy);
                            SqlParameter param11 = new SqlParameter("pi_RequestedOnForum", objAudit.RequestedOnForum);
                            SqlParameter param12 = new SqlParameter("pi_SMSFlag", IsSMS);
                            SqlParameter param13 = new SqlParameter("pi_TxnType", TxnType);

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

                            daReport.Fill(DT);

                            DataTable dt = DT.Tables[0];
                            string ResCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            result.ResponseCode = ResCode;

                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {

                                if (Convert.ToString(dt.Rows[0]["SMSFlag"]) == "1")
                                {
                                    string SMSStatus = Convert.ToString(dt.Rows[0]["SMSFlag"]);
                                    string WAStatus = Convert.ToString(dt.Rows[0]["WAStatus"]); //MobileNo
                                    string _MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                                    string _MobileMessage = Convert.ToString(dt.Rows[0]["Message"]);
                                    string _UserName = Convert.ToString(dt.Rows[0]["UserName"]);
                                    string _Password = Convert.ToString(dt.Rows[0]["Password"]);
                                    string _Sender = Convert.ToString(dt.Rows[0]["SenderId"]);
                                    string _Url = Convert.ToString(dt.Rows[0]["Url"]);
                                    string _WAMessage = Convert.ToString(dt.Rows[0]["WAMessage"]);
                                    string _WATokenId = Convert.ToString(dt.Rows[0]["WATokenId"]);


                                    if (SMSStatus == "1" && WAStatus == "1")
                                    {
                                        Thread _job = new Thread(() => SendSMSandWA(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _WAMessage, _WATokenId));
                                        _job.Start();
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        result = contextNew.Database.SqlQuery<SPResponse>("sp_BurnRW_New_ITOPS @pi_MobileNo, @pi_OutletId, @pi_TxnDate, @pi_RequestDate, @pi_InvoiceNo, @pi_InvoiceAmt,@pi_RedeemPoints, @pi_LoginId,@pi_PartialEarnPoints, @pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag, @pi_TxnType",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_OutletId", OutletId),
                              new SqlParameter("@pi_TxnDate", TxnDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_RequestDate", RequestDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_InvoiceNo", InvoiceNo),
                              new SqlParameter("@pi_InvoiceAmt", InvoiceAmt),
                              new SqlParameter("@pi_RedeemPoints", Points),
                              new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_PartialEarnPoints", PartialEarnPoints),
                              new SqlParameter("@pi_RequestBy", objAudit.RequestedBy),
                              new SqlParameter("@pi_RequestedOnForum", objAudit.RequestedOnForum),
                              new SqlParameter("@pi_SMSFlag", IsSMS),
                              new SqlParameter("@pi_TxnType", TxnType)).FirstOrDefault<SPResponse>();
                    }                 
                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddRedeemPointsData");
            }
            return result;
        }

        public SPResponse AddLoadBonusData(string GroupId, string MobileNo, string OutletId, int BonusPoints, string BonusRemark, DateTime ExpiryDate, string IsSMS, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    result = contextNew.Database.SqlQuery<SPResponse>("sp_BonusPoints_ITOPS @pi_MobileNo, @pi_OutletId, @pi_RequestDate, @pi_ExpiryDate, @pi_BonusRemarks, @pi_BonusPoints,@pi_LoginId, @pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_OutletId", OutletId),
                              new SqlParameter("@pi_RequestDate", objAudit.RequestedOn.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_ExpiryDate", ExpiryDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_BonusRemarks", BonusRemark),
                              new SqlParameter("@pi_BonusPoints", BonusPoints),
                              new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_RequestBy", objAudit.RequestedBy),
                              new SqlParameter("@pi_RequestedOnForum", objAudit.RequestedOnForum),
                              new SqlParameter("@pi_SMSFlag", IsSMS)).FirstOrDefault<SPResponse>();

                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddLoadBonusData");
            }
            return result;
        }

        public SPResponse AddSingleCustomerData(string GroupId, CustomerDetail objCustomer, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var ObjMobileNo = contextNew.CustomerDetails.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNo == null)
                    {
                        var CustomerId = contextNew.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();

                        var NewId = Convert.ToInt64(CustomerId) + 1;
                        objCustomer.CustomerId = Convert.ToString(NewId);
                        objCustomer.Points = 0;

                        contextNew.CustomerDetails.AddOrUpdate(objCustomer);
                        contextNew.SaveChanges();
                        result.ResponseCode = "00";
                        result.ResponseMessage = "Member Added Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }

                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddSingleCustomerData");
            }
            return result;
        }

        public SPResponse AddBulkCustomerData(string GroupId, CustomerDetail objCustomer)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var ObjMobileNo = contextNew.CustomerDetails.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNo == null)
                    {
                        var CustomerId = contextNew.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();

                        var NewId = Convert.ToInt64(CustomerId) + 1;
                        objCustomer.CustomerId = Convert.ToString(NewId);

                        contextNew.CustomerDetails.AddOrUpdate(objCustomer);
                        contextNew.SaveChanges();
                        result.ResponseCode = "00";
                        result.ResponseMessage = "Member Added Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }

                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddBulkCustomerData");
            }
            return result;
        }

        public bool ChangeSMSDetails(string GroupId, string CustomerId, bool Disable, tblAudit objAudit)
        {
            bool status = false;
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var objCustomer = contextNew.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                    objCustomer.IsSMS = Disable;

                    contextNew.CustomerDetails.AddOrUpdate(objCustomer);
                    contextNew.SaveChanges();
                    status = true;
                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ChangeSMSDetails");
            }
            return status;
        }

        public MemberData GetOTPData(string GroupId, string MobileNo)
        {
            MemberData objMemberData = new MemberData();
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                OTPMaintenance objOTP = new OTPMaintenance();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objOTP = contextNew.OTPMaintenances.Where(x => x.MobileNo == MobileNo).OrderByDescending(y => y.Datetime).FirstOrDefault();
                }
                if (objOTP != null)
                {

                    objMemberData.MobileNo = objOTP.OTP;
                    objMemberData.EnrolledOn = Convert.ToString(objOTP.Datetime);
                    var OutletId = objOTP.CounterId.Substring(0, objOTP.CounterId.Length - 2);
                    using (var contextNew = new BOTSDBContext(connStr))
                    {
                        objMemberData.EnrolledOutletName = contextNew.OutletDetails.Where(x => x.OutletId == OutletId).Select(y => y.OutletName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOTPData");
            }
            return objMemberData;
        }

        public CancelTxnModel GetTransactionByInvoiceNo(string GroupId, string InvoiceNo)
        {
            CancelTxnModel objReturn = new CancelTxnModel();
            try
            {
                TransactionMaster objTxn = new TransactionMaster();
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objTxn = contextNew.TransactionMasters.Where(x => x.InvoiceNo == InvoiceNo && x.Status == "06").FirstOrDefault();
                }
                if (objTxn != null)
                {
                    objReturn.InvoiceNo = objTxn.InvoiceNo;
                    objReturn.InvoiceAmt = objTxn.InvoiceAmt;
                    objReturn.MobileNo = objTxn.MobileNo;
                    objReturn.Points = Convert.ToString(objTxn.PointsEarned);
                    objReturn.Datetime = Convert.ToDateTime(objTxn.Datetime).ToString("dd/MM/yyyy HH:mm:ss");
                    objReturn.DatetimeOriginal = Convert.ToString(objTxn.Datetime);
                    var OutletId = objTxn.CounterId.Substring(0, objTxn.CounterId.Length - 2);
                    using (var contextNew = new BOTSDBContext(connStr))
                    {
                        objReturn.OutletName = contextNew.OutletDetails.Where(x => x.OutletId == OutletId).Select(y => y.OutletName).FirstOrDefault();
                        objReturn.TransactionName = contextNew.TransactionTypeMasters.Where(x => x.TransactionType == objTxn.TransType).Select(y => y.TransactionName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionByInvoiceNo");
            }
            return objReturn;
        }
        public CancelTxnModel GetTransactionByTransactionId(string GroupId, string TransactionId)
        {
            CancelTxnModel objReturn = new CancelTxnModel();
            try
            {
                TransactionMaster objTxn = new TransactionMaster();
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                long TxnId = Convert.ToInt64(TransactionId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objTxn = contextNew.TransactionMasters.Where(x => x.SlNo == TxnId).FirstOrDefault();
                }
                if (objTxn != null)
                {
                    objReturn.TransactionId = objTxn.SlNo;
                    objReturn.InvoiceNo = objTxn.InvoiceNo;
                    objReturn.InvoiceAmt = objTxn.InvoiceAmt;
                    objReturn.MobileNo = objTxn.MobileNo;
                    objReturn.Points = Convert.ToString(objTxn.PointsEarned);
                    objReturn.Datetime = Convert.ToDateTime(objTxn.Datetime).ToString("dd/MM/yyyy HH:mm:ss");
                    objReturn.DatetimeOriginal = Convert.ToString(objTxn.Datetime);
                    var OutletId = objTxn.CounterId.Substring(0, objTxn.CounterId.Length - 2);
                    using (var contextNew = new BOTSDBContext(connStr))
                    {
                        objReturn.OutletName = contextNew.OutletDetails.Where(x => x.OutletId == OutletId).Select(y => y.OutletName).FirstOrDefault();
                        objReturn.TransactionName = contextNew.TransactionTypeMasters.Where(x => x.TransactionType == objTxn.TransType).Select(y => y.TransactionName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionByTransactionId");
            }
            return objReturn;
        }
        public CancelTxnModel GetTransactionByInvoiceNoAndMobileNo(string GroupId, string MobileNo, string InvoiceNo)
        {
            CancelTxnModel objReturn = new CancelTxnModel();
            try
            {
                TransactionMaster objTxn = new TransactionMaster();
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objTxn = contextNew.TransactionMasters.Where(x => x.InvoiceNo == InvoiceNo && x.MobileNo == MobileNo && x.Status == "06").FirstOrDefault();
                }
                if (objTxn != null)
                {
                    objReturn.InvoiceNo = objTxn.InvoiceNo;
                    objReturn.InvoiceAmt = objTxn.InvoiceAmt;
                    objReturn.MobileNo = objTxn.MobileNo;
                    objReturn.Points = Convert.ToString(objTxn.PointsEarned);
                    objReturn.Datetime = Convert.ToDateTime(objTxn.Datetime).ToString("dd/MM/yyyy HH:mm:ss");
                    objReturn.DatetimeOriginal = Convert.ToString(objTxn.Datetime);
                    var OutletId = objTxn.CounterId.Substring(0, objTxn.CounterId.Length - 2);
                    using (var contextNew = new BOTSDBContext(connStr))
                    {
                        objReturn.OutletName = contextNew.OutletDetails.Where(x => x.OutletId == OutletId).Select(y => y.OutletName).FirstOrDefault();
                        objReturn.TransactionName = contextNew.TransactionTypeMasters.Where(x => x.TransactionType == objTxn.TransType).Select(y => y.TransactionName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionByInvoiceNoAndMobileNo");
            }
            return objReturn;
        }
        public List<CancelTxnModel> GetTransactionByMobileNo(string GroupId, string MobileNo)
        {
            List<CancelTxnModel> lstObjReturn = new List<CancelTxnModel>();
            try
            {
                List<TransactionMaster> lstObjTxn = new List<TransactionMaster>();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    lstObjTxn = contextNew.TransactionMasters.Where(x => x.MobileNo == MobileNo && x.Status == "06").OrderByDescending(m => m.Datetime).ToList();
                }
                if (lstObjTxn != null)
                {
                    foreach (var objTxn in lstObjTxn)
                    {
                        CancelTxnModel objReturn = new CancelTxnModel();
                        objReturn.TransactionId = objTxn.SlNo;
                        objReturn.InvoiceNo = objTxn.InvoiceNo;
                        objReturn.InvoiceAmt = objTxn.InvoiceAmt;
                        objReturn.MobileNo = objTxn.MobileNo;
                        objReturn.Points = Convert.ToString(objTxn.PointsEarned);
                        objReturn.Datetime = Convert.ToDateTime(objTxn.Datetime).ToString("dd/MM/yyyy HH:mm:ss");
                        var OutletId = objTxn.CounterId.Substring(0, objTxn.CounterId.Length - 2);
                        using (var contextNew = new BOTSDBContext(connStr))
                        {
                            objReturn.OutletName = contextNew.OutletDetails.Where(x => x.OutletId == OutletId).Select(y => y.OutletName).FirstOrDefault();
                            objReturn.TransactionName = contextNew.TransactionTypeMasters.Where(x => x.TransactionType == objTxn.TransType).Select(y => y.TransactionName).FirstOrDefault();
                        }
                        lstObjReturn.Add(objReturn);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionByMobileNo");
            }
            return lstObjReturn;
        }
        public CustomerDetail GetCustomerByMobileNo(string GroupId, string MobileNo)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerByMobileNo");
            }
            return objCustomerDetail;
        }

        public SPResponse DeleteTransaction(string GroupId, string InvoiceNo, string MobileNo, string InvoiceAmt, DateTime ip_Date, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    result = contextNew.Database.SqlQuery<SPResponse>("sp_CancelTxn1_ITOPS @pi_MobileNo, @pi_InvoiceNo, @pi_InvoiceAmt, @pi_RequestDate, @pi_Datetime, @pi_LoginId,@pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_InvoiceNo", InvoiceNo),
                              new SqlParameter("@pi_InvoiceAmt", InvoiceAmt),
                              new SqlParameter("@pi_RequestDate", objAudit.RequestedOn),
                              new SqlParameter("@pi_Datetime", ip_Date),
                              new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_RequestBy", objAudit.RequestedBy),
                              new SqlParameter("@pi_RequestedOnForum", objAudit.RequestedOnForum),
                              new SqlParameter("@pi_SMSFlag", "0")).FirstOrDefault<SPResponse>();

                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DeleteTransaction");
            }
            return result;

        }

        public List<LoginIdByOutlet> GetLoginIdByOutlet(string GroupId, int outletId)
        {
            List<LoginIdByOutlet> loginidbyoutlet = new List<LoginIdByOutlet>();

            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var context = new BOTSDBContext(connStr))
                {
                    loginidbyoutlet = context.Database.SqlQuery<LoginIdByOutlet>("sp_GetLoginIdByOutlet @outletId",
                        // new SqlParameter("@groupId", GroupId),                       
                        new SqlParameter("@outletId", outletId)).ToList<LoginIdByOutlet>();
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLoginIdByOutlet");
            }
            return loginidbyoutlet;

        }

        public List<LogDetailsRW> GetLogDetails(string search, string GroupId)
        {
            List<LogDetailsRW> lstLogDetails = new List<LogDetailsRW>();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    contextNew.Database.CommandTimeout = 300;
                    lstLogDetails = contextNew.LogDetailsRWs.Where(x => x.ReceivedData.Contains(search)).ToList();
                }
                foreach (var item in lstLogDetails)
                {
                    item.datetimestr = item.Datetime.Value.ToString("MM/dd/yyyy");
                }
                lstLogDetails = lstLogDetails.OrderByDescending(x => x.Datetime).ToList();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLogDetails");
            }

            return lstLogDetails;
        }

        public bool ModifyTransaction(string GroupId, long TransactionId, decimal points, tblAudit objAudit)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    using (DbContextTransaction transaction = contextNew.Database.BeginTransaction())
                    {
                        try
                        {
                            var objTransaction = contextNew.TransactionMasters.Where(x => x.SlNo == TransactionId).FirstOrDefault();
                            var customerDetails = contextNew.CustomerDetails.Where(x => x.MobileNo == objTransaction.MobileNo).FirstOrDefault();
                            //var PointExpiry = contextNew.PointsExpiries.Where(x => x.MobileNo == objTransaction.MobileNo && x.Status == "00").ToList();
                            //var NewPointExpiry = contextNew.PointsExpiries.Where(x => x.MobileNo == objTransaction.MobileNo && x.Status == "00").OrderByDescending(y => y.ExpiryDate).FirstOrDefault();

                            var oldTransactionPoints = objTransaction.PointsEarned;

                            objTransaction.PointsEarned = points;
                            contextNew.TransactionMasters.AddOrUpdate(objTransaction);
                            contextNew.SaveChanges();

                            //foreach (var item in PointExpiry)
                            //{
                            //    item.Status = "01";
                            //    contextNew.PointsExpiries.AddOrUpdate(item);
                            //    contextNew.SaveChanges();
                            //}

                            //PointsExpiry objPointExpiry = new PointsExpiry();
                            //objPointExpiry.Points = (customerDetails.Points - oldTransactionPoints) + points;
                            //objPointExpiry.CounterId = NewPointExpiry.CounterId;
                            //objPointExpiry.MobileNo = NewPointExpiry.MobileNo;
                            //objPointExpiry.Datetime = DateTime.Now;
                            //objPointExpiry.EarnDate = NewPointExpiry.EarnDate;
                            //objPointExpiry.ExpiryDate = NewPointExpiry.ExpiryDate;
                            //objPointExpiry.Status = "00";
                            //objPointExpiry.InvoiceNo = NewPointExpiry.InvoiceNo;
                            //objPointExpiry.GroupId = NewPointExpiry.GroupId;
                            //objPointExpiry.CustomerId = NewPointExpiry.CustomerId;

                            //contextNew.PointsExpiries.AddOrUpdate(objPointExpiry);
                            //contextNew.SaveChanges();

                            customerDetails.Points = (customerDetails.Points - oldTransactionPoints) + points;
                            contextNew.CustomerDetails.AddOrUpdate(customerDetails);
                            contextNew.SaveChanges();

                            using (var context = new CommonDBContext())
                            {
                                context.tblAudits.Add(objAudit);
                                context.SaveChanges();
                            }
                            transaction.Commit();
                            status = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            newexception.AddException(ex, GroupId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ModifyTransaction");
            }
            return status;
        }

        public SPResponse TransferPoints(string GroupId, string MobileNo,  string NewMobileNo, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    using (DbContextTransaction transaction = contextNew.Database.BeginTransaction())
                    {
                        try
                        {
                            var objExisting = contextNew.CustomerDetails.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                            var objExistingNew = contextNew.CustomerDetails.Where(x => x.MobileNo == NewMobileNo).FirstOrDefault();
                            if (objExisting != null && objExistingNew != null)
                            {
                                string oldno = objExisting.MobileNo;
                                var transferPoints = objExisting.Points;
                                var AddPoints = objExisting.Points + objExistingNew.Points;
                                objExistingNew.Points = AddPoints;
                                objExisting.Points = 0;

                                contextNew.CustomerDetails.AddOrUpdate(objExistingNew);
                                contextNew.SaveChanges();
                                contextNew.CustomerDetails.AddOrUpdate(objExisting);
                                contextNew.SaveChanges();

                                using (var context = new CommonDBContext())
                                {
                                    context.tblAudits.Add(objAudit);
                                    context.SaveChanges();
                                }

                                TransferPointsDetail transferPointsDetail = new TransferPointsDetail();
                                transferPointsDetail.OldMobileNo = MobileNo;
                                transferPointsDetail.NewMobileNo = NewMobileNo;
                                transferPointsDetail.Points = transferPoints;
                                transferPointsDetail.GroupId = GroupId;
                                transferPointsDetail.Datetime = DateTime.Now;
                                contextNew.TransferPointsDetails.AddOrUpdate(transferPointsDetail);
                                contextNew.SaveChanges();

                                result.ResponseCode = "00";
                                result.ResponseMessage = "Points Transferred Successfully";

                                transaction.Commit();
                            }
                            else
                            {
                                result.ResponseCode = "01";
                                result.ResponseMessage = "Mobile Number does not Exist";
                            }


                        }
                        catch (Exception ex)
                        {
                            newexception.AddException(ex, GroupId);
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "TransferPoints");
            }
            return result;
        }

        public void SendSMSandWA(string _MobileNo, string _MobileMessage, string  _UserName, string  _Password, string  _Sender, string  _Url, string  _WAMessage, string _WATokenId)
        {
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
                Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                _job.Start();
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                _job.Start();
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Thread _job = new Thread(() => SendSMS(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                _job.Start();
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

        }

        public void SendSMS(string _MobileNo, string _MobileMessage, string _UserName, string _Password, string _Sender, string _Url)
        {
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
        }

        public bool DeleteUser(string GroupId, string MobileNo,string CustomerId, tblAudit objAudit)
        {
            bool status = false;
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                List<TransactionMaster> lstTransactionMaster = new List <TransactionMaster>();
                List<PointsExpiry> lstPointExpiry = new List<PointsExpiry>();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                    objCustomerDetail.Status = "01";
                    objCustomerDetail.IsSMS = false;

                    contextNew.CustomerDetails.AddOrUpdate(objCustomerDetail);
                    contextNew.SaveChanges();

                    var TM2 = contextNew.TransactionMasters.Where(x => x.MobileNo == MobileNo).ToList();
                    foreach (var item in TM2)
                    {
                        TransactionMaster objTransactionMaster = new TransactionMaster();
                        objTransactionMaster.SlNo = item.SlNo;
                        objTransactionMaster.CounterId = item.CounterId;
                        objTransactionMaster.MobileNo = item.MobileNo;
                        objTransactionMaster.Datetime = item.Datetime;
                        objTransactionMaster.TransType = item.TransType;
                        objTransactionMaster.TransSource = item.TransSource;
                        objTransactionMaster.InvoiceNo = item.InvoiceNo;
                        objTransactionMaster.InvoiceAmt = item.InvoiceAmt;
                        objTransactionMaster.CustomerId = item.CustomerId;
                        objTransactionMaster.PointsEarned = item.PointsEarned;
                        objTransactionMaster.PointsBurned = item.PointsBurned;
                        objTransactionMaster.CampaignPoints = item.CampaignPoints;
                        objTransactionMaster.TxnAmt = item.TxnAmt;
                        objTransactionMaster.CustomerPoints = item.CustomerPoints;
                        objTransactionMaster.Synchronization = item.Synchronization;
                        objTransactionMaster.SyncDatetime = item.SyncDatetime;
                        objTransactionMaster.Status = "09";
                        contextNew.TransactionMasters.AddOrUpdate(objTransactionMaster);
                        contextNew.SaveChanges();
                    }

                    var PM2 = contextNew.PointsExpiries.Where(x => x.MobileNo == MobileNo && x.Status=="00").ToList();
                    foreach (var item in PM2)
                    {
                        PointsExpiry objPointsExpiry = new PointsExpiry();
                        objPointsExpiry.SlNo = item.SlNo;
                        objPointsExpiry.MobileNo = item.MobileNo;
                        objPointsExpiry.CounterId = item.CounterId;
                        objPointsExpiry.EarnDate = item.EarnDate;
                        objPointsExpiry.Points = item.Points;
                        objPointsExpiry.ExpiryDate = item.ExpiryDate;
                        objPointsExpiry.BurnDate = item.BurnDate;
                        objPointsExpiry.InvoiceNo = item.InvoiceNo;
                        objPointsExpiry.TransRefNo = item.TransRefNo;
                        objPointsExpiry.OriginalInvoiceNo = item.OriginalInvoiceNo;
                        objPointsExpiry.GroupId = item.GroupId;
                        objPointsExpiry.Datetime = item.Datetime;
                        objPointsExpiry.CustomerId = item.CustomerId;
                        objPointsExpiry.Status = "01";
                        contextNew.PointsExpiries.AddOrUpdate(objPointsExpiry);
                        contextNew.SaveChanges();
                    }
                    status = true;

                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DeleteUser");
            }
            return status;
        }


    }

}
