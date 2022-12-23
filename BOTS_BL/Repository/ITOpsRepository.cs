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
                newexception.AddException(ex, "");
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
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.MobileNo == searchData).FirstOrDefault();
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
                newexception.AddException(ex, "");
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
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CardNumber == searchData).FirstOrDefault();
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
                newexception.AddException(ex, "");
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
                newexception.AddException(ex, "");
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
                newexception.AddException(ex, "");
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
                newexception.AddException(ex, GroupId);
            }
            return result;
        }

        public SPResponse AddEarnData(string GroupId, string MobileNo, string OutletId, DateTime TxnDate, DateTime RequestDate, string InvoiceNo, string InvoiceAmt, string IsSMS, decimal Points, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    result = contextNew.Database.SqlQuery<SPResponse>("sp_EarnRW_New_ITOPS @pi_MobileNo, @pi_OutletId, @pi_TxnDate, @pi_RequestDate, @pi_InvoiceNo, @pi_InvoiceAmt, @pi_LoginId, @pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag,@pi_Points",
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
                              new SqlParameter("@pi_Points", Points)).FirstOrDefault<SPResponse>();


                    //DateTime.Now.ToString("yyyy-MM-dd")


                    //if (result.ResponseCode == "00")
                    //{
                    //    status = true;
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
                newexception.AddException(ex, GroupId);
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
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                throw ex;
            }
            return loginidbyoutlet;

        }

        public List<LogDetailsRW> GetLogDetails(string search, string GroupId)
        {
            List<LogDetailsRW> lstLogDetails = new List<LogDetailsRW>();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
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

            return lstLogDetails;
        }

        public bool ModifyTransaction(string GroupId, long TransactionId, decimal points, tblAudit objAudit)
        {
            bool status = false;
            string connStr = objCustRepo.GetCustomerConnString(GroupId);

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
            return status;
        }

        public SPResponse TransferPoints(string GroupId, string MobileNo,  string NewMobileNo, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
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
            return result;
        }



    }

}
