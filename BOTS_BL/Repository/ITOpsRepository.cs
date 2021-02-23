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
                newexception.AddException(ex);
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
                newexception.AddException(ex);
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
                newexception.AddException(ex);
            }
            return objMemberData;
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
                newexception.AddException(ex);
            }
            return status;
        }

        public bool UpdateMobileOfMember(string GroupId, string CustomerId, string MobileNo, tblAudit objAudit)
        {
            bool status = false;
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                    objCustomerDetail.MobileNo = MobileNo;

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
                newexception.AddException(ex);
            }
            return status;
        }

        public SPResponse AddEarnData(string GroupId, string MobileNo, string OutletId, DateTime TxnDate, DateTime RequestDate, string InvoiceNo, string InvoiceAmt, string IsSMS, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    result = contextNew.Database.SqlQuery<SPResponse>("sp_EarnRW_New_ITOPS @pi_MobileNo, @pi_OutletId, @pi_TxnDate, @pi_RequestDate, @pi_InvoiceNo, @pi_InvoiceAmt, @pi_LoginId, @pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_OutletId", OutletId),
                              new SqlParameter("@pi_TxnDate", TxnDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_RequestDate", RequestDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_InvoiceNo", InvoiceNo),
                              new SqlParameter("@pi_InvoiceAmt", InvoiceAmt),
                              new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_RequestBy", objAudit.RequestedBy),
                              new SqlParameter("@pi_RequestedOnForum", objAudit.RequestedOnForum),
                              new SqlParameter("@pi_SMSFlag", IsSMS)).FirstOrDefault<SPResponse>();
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
                newexception.AddException(ex);
            }

            return result;
        }

        public SPResponse AddRedeemPointsData(string GroupId, string MobileNo, string OutletId, DateTime TxnDate, DateTime RequestDate, string InvoiceNo, string InvoiceAmt, decimal Points, string IsSMS, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    result = contextNew.Database.SqlQuery<SPResponse>("sp_BurnRW_New_ITOPS @pi_MobileNo, @pi_OutletId, @pi_TxnDate, @pi_RequestDate, @pi_InvoiceNo, @pi_InvoiceAmt,@pi_RedeemPoints, @pi_LoginId, @pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_OutletId", OutletId),
                              new SqlParameter("@pi_TxnDate", TxnDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_RequestDate", RequestDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_InvoiceNo", InvoiceNo),
                              new SqlParameter("@pi_InvoiceAmt", InvoiceAmt),
                              new SqlParameter("@pi_RedeemPoints", Points),
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
                newexception.AddException(ex);
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
                newexception.AddException(ex);
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
                newexception.AddException(ex);
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
                newexception.AddException(ex);
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
                newexception.AddException(ex);
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
                    objTxn = contextNew.TransactionMasters.Where(x => x.InvoiceNo == InvoiceNo).FirstOrDefault();
                }
                if (objTxn != null)
                {
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
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
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
                    lstObjTxn = contextNew.TransactionMasters.Where(x => x.MobileNo == MobileNo).ToList();
                }
                if (lstObjTxn != null)
                {
                    foreach (var objTxn in lstObjTxn)
                    {
                        CancelTxnModel objReturn = new CancelTxnModel();
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
                newexception.AddException(ex);
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
                newexception.AddException(ex);
            }
            return objCustomerDetail;
        }

        public bool DeleteTransaction(string GroupId, string InvoiceNo, tblAudit objAudit)
        {
            bool result = false;
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var transaction = contextNew.TransactionMasters.Where(x => x.InvoiceNo == InvoiceNo).FirstOrDefault();
                    contextNew.TransactionMasters.Remove(transaction);
                    contextNew.SaveChanges();
                    result = true;
                }
                using (var context = new CommonDBContext())
                {
                    context.tblAudits.Add(objAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return result;

        }

    }

}
