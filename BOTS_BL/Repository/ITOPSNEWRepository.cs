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
using BOTS_BL.Models.IndividualDBModels;

namespace BOTS_BL.Repository
{
    public class ITOPSNEWRepository
    {
        //CustomerRepository objCustRepo = new CustomerRepository();
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
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //objCustomerDetail = contextNew.CustomerDetails.Where(x => x.MobileNo == searchData && x.Status == "00").FirstOrDefault();
                    var CustData = contextNew.View_ITOPSCustData.Where(x => x.MobileNo == searchData).FirstOrDefault();

                    if (CustData != null)
                    {
                        objMemberData.MemberName = CustData.CustomerName;
                        objMemberData.MobileNo = CustData.MobileNo;
                        objMemberData.CardNo = CustData.CardNo;
                        objMemberData.PointsBalance = CustData.Points;
                        objMemberData.EnrolledOn = CustData.DOJ.Value.ToString("dd/MM/yyyy");
                        objMemberData.EnrolledOutletName = CustData.EnrolledOutlet;
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
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CardNumber == searchData && x.Status == "00").FirstOrDefault();
                    var CustData = contextNew.View_ITOPSCustData.Where(x => x.CardNo == searchData).FirstOrDefault();

                    if(CustData != null)
                    {
                        objMemberData.MemberName = CustData.CustomerName;
                        objMemberData.MobileNo = CustData.MobileNo;
                        objMemberData.CardNo = CustData.CardNo;
                        objMemberData.PointsBalance = CustData.Points;
                        objMemberData.EnrolledOn = CustData.DOJ.Value.ToString("dd/MM/yyyy");
                        objMemberData.EnrolledOutletName = CustData.EnrolledOutlet;
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
                tblStoreMaster objstore = new tblStoreMaster();
                tblLoginDetail objLogin = new tblLoginDetail();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objstore = contextNew.tblStoreMasters.Where(x => x.CounterId == counterId).FirstOrDefault();
                    objLogin = contextNew.tblLoginDetails.Where(x => x.LoginId == counterId).FirstOrDefault();
                    if (objstore != null)
                    {
                        objstore.IsActive = false;
                    }
                    if (objLogin != null)
                    {
                        objLogin.Password = "123";
                    }                    
                    contextNew.tblStoreMasters.AddOrUpdate(objstore);
                    contextNew.tblLoginDetails.AddOrUpdate(objLogin);
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
        public bool UpdateNameOfMember(string GroupId, string MobileNo, string Name, tblAudit objAudit)
        {
            bool status = false;
            try
            {
                //CustomerDetail objCustomerDetail = new CustomerDetail();
                tblCustInfo objCustomerDetail = new tblCustInfo();
                tblCustDetailsMaster objCustDetail = new tblCustDetailsMaster();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                    objCustomerDetail = contextNew.tblCustInfoes.Where(x=> x.MobileNo == MobileNo).FirstOrDefault();
                    objCustDetail = contextNew.tblCustDetailsMasters.Where(x=> x.MobileNo == MobileNo).FirstOrDefault();

                    objCustomerDetail.Name = Name;                 
                    objCustDetail.Name = Name;
                   
                    contextNew.tblCustInfoes.AddOrUpdate(objCustomerDetail);
                    contextNew.tblCustDetailsMasters.AddOrUpdate(objCustDetail);
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

                string connStr = GetCustomerConnString(GroupId);
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
                string connStr = GetCustomerConnString(GroupId);

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

        public SPResponse AddRedeemPointsData(string GroupId, string MobileNo, string OutletId, DateTime TxnDate, DateTime RequestDate, string InvoiceNo, string InvoiceAmt, decimal Points, string IsSMS, string TxnType, string PartialEarnPoints, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = GetCustomerConnString(GroupId);
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
                string connStr = GetCustomerConnString(GroupId);
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

        public SPResponse AddSingleCustomerData(string GroupId, tblCustDetailsMaster objCustomer, tblCustInfo objcustInfo, tblCustTxnSummaryMaster objtblCustTxn, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var ObjMobileNo = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNo == null)
                    {
                        var CustomerId = contextNew.tblCustDetailsMasters.OrderByDescending(x => x.Id).Select(y => y.Id).FirstOrDefault();
                        var NewGroupId = GroupId;
                        var NewMobileNo = objCustomer.MobileNo;
                        var NewId = Convert.ToInt64(string.Format("{0}{1}", NewGroupId, NewMobileNo));
                        objCustomer.Id = Convert.ToString(NewId);

                        contextNew.tblCustDetailsMasters.AddOrUpdate(objCustomer);
                        contextNew.SaveChanges();
                        result.ResponseCode = "00";
                        result.ResponseMessage = "Member Added Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }

                    var ObjMobileNo1 = contextNew.tblCustInfoes.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNo1 == null)
                    {
                        contextNew.tblCustInfoes.AddOrUpdate(objcustInfo);
                        contextNew.SaveChanges();
                        result.ResponseCode = "00";
                        result.ResponseMessage = "Member Added Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }

                    var ObjMobileNoNew = contextNew.tblCustTxnSummaryMasters.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNoNew == null)
                    {
                        contextNew.tblCustTxnSummaryMasters.AddOrUpdate(objtblCustTxn);
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

        public SPResponse AddBulkCustomerData(string GroupId, tblCustDetailsMaster objCustomer, tblCustInfo objcustInfo, tblCustTxnSummaryMaster objtblCustTxn, tblBulkCustList objtblBulkCust)
        {
            SPResponse result = new SPResponse();
            try
            {
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var ObjMobileNo = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNo == null)
                    {
                        var NewGroupId = GroupId;
                        var NewMobileNo = objCustomer.MobileNo;
                        var NewId = Convert.ToInt64(string.Format("{0}{1}", NewGroupId, NewMobileNo));
                        objCustomer.Id = Convert.ToString(NewId);

                        contextNew.tblCustDetailsMasters.AddOrUpdate(objCustomer);
                        contextNew.SaveChanges();
                        result.ResponseCode = "00";
                        result.ResponseMessage = "Member Added Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }

                    var ObjMobileNo1 = contextNew.tblCustInfoes.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNo1 == null)
                    {
                        contextNew.tblCustInfoes.AddOrUpdate(objcustInfo);
                        contextNew.SaveChanges();
                        result.ResponseCode = "00";
                        result.ResponseMessage = "Member Added Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }

                    var ObjMobileNoNew = contextNew.tblCustTxnSummaryMasters.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNoNew == null)
                    {
                        contextNew.tblCustTxnSummaryMasters.AddOrUpdate(objtblCustTxn);
                        contextNew.SaveChanges();
                        result.ResponseCode = "00";
                        result.ResponseMessage = "Member Added Successfully";
                    }
                    else
                    {
                        result.ResponseCode = "01";
                        result.ResponseMessage = "Mobile Number Already Exist";
                    }

                    var ObjMobileNoNew1 = contextNew.tblBulkCustLists.Where(x => x.MobileNo == objCustomer.MobileNo).FirstOrDefault();
                    if (ObjMobileNoNew1 == null)
                    {
                        contextNew.tblBulkCustLists.AddOrUpdate(objtblBulkCust);
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
                string connStr =  GetCustomerConnString(GroupId);
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
                //OTPMaintenance objOTP = new OTPMaintenance();
                tblOTPDetail objOTP = new tblOTPDetail();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //objOTP = contextNew.OTPMaintenances.Where(x => x.MobileNo == MobileNo).OrderByDescending(y => y.Datetime).FirstOrDefault();
                    objOTP = contextNew.tblOTPDetails.Where(x => x.MobileNo == MobileNo).OrderByDescending(y => y.Datetime).FirstOrDefault();
                    if (objOTP != null)
                    {
                        objMemberData.MobileNo = objOTP.OTP;
                        objMemberData.EnrolledOn = Convert.ToString(objOTP.Datetime);
                        objMemberData.EnrolledOutletName = objOTP.OutletName;
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
                string connStr =  GetCustomerConnString(GroupId);
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
                string connStr =  GetCustomerConnString(GroupId);
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
                string connStr =  GetCustomerConnString(GroupId);
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
                string connStr =  GetCustomerConnString(GroupId);
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
                string connStr =  GetCustomerConnString(GroupId);
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
                string connStr =  GetCustomerConnString(GroupId);
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

        public List<LoginIdByOutlet> GetLoginIdByOutlet(string GroupId, string outletId)
        {
            List<LoginIdByOutlet> loginidbyoutlet = new List<LoginIdByOutlet>();

            try
            {
                string connStr = GetCustomerConnString(GroupId);
                using (var context = new BOTSDBContext(connStr))
                {                    
                    loginidbyoutlet = context.Database.SqlQuery<LoginIdByOutlet>("select distinct S.CounterId,S.securitykey from tblStoreMaster S inner join tblOutletMaster o on s.OutletId = @outletId where s.CounterId In(select LoginId from tblLoginDetails where LevelIndicator= 05)", new SqlParameter("@outletId", outletId)).ToList<LoginIdByOutlet>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLoginIdByOutlet");
            }
            return loginidbyoutlet;

        }

        public List<tblLogDetail> GetLogDetails(string search, string GroupId)
        {
            //List<LogDetailsRW> lstLogDetails1 = new List<LogDetailsRW>();
            List<tblLogDetail> lstLogDetails = new List<tblLogDetail>();
            string connStr = GetCustomerConnString(GroupId);
            try
            {
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    contextNew.Database.CommandTimeout = 300;
                    //lstLogDetails = contextNew.LogDetailsRWs.Where(x => x.ReceivedData.Contains(search)).ToList();
                    lstLogDetails = contextNew.tblLogDetails.Where(x => x.ReceivedData.Contains(search)).ToList();
                }
                foreach (var item in lstLogDetails)
                {
                    item.datetimestr = item.ServerDatetime.Value.ToString("MM/dd/yyyy");
                }
                lstLogDetails = lstLogDetails.OrderByDescending(x => x.SlNo).ToList();
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
            string connStr =  GetCustomerConnString(GroupId);
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

        public SPResponse TransferPoints(string GroupId, string MobileNo, string NewMobileNo, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();

            string connStr =  GetCustomerConnString(GroupId);
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

        public void SendSMSandWA(string _MobileNo, string _MobileMessage, string _UserName, string _Password, string _Sender, string _Url, string _WAMessage, string _WATokenId)
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

        public bool DeleteUser(string GroupId, string MobileNo, tblAudit objAudit)
        {
            bool status = false;
            try
            {
                tblCustDetailsMaster objCustDetail = new tblCustDetailsMaster();                
                string connStr =  GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustDetail = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    objCustDetail.IsActive = false;

                    contextNew.tblCustDetailsMasters.AddOrUpdate(objCustDetail);
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
                newexception.AddException(ex, "DeleteUser");
            }
            return status;
        }

        public List<SelectListItem> GetBrandList(string GroupId)
        {
            List<SelectListItem> lstBrands = new List<SelectListItem>();
            try
            {
                var connStr = GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var Brands = contextNew.tblBrandMasters.Where(x => x.GroupId == GroupId).ToList();

                    foreach (var item in Brands)
                    {
                        lstBrands.Add(new SelectListItem
                        {
                            Text = item.BrandName,
                            Value = Convert.ToString(item.BrandId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutlet");
            }

            lstBrands = lstBrands.OrderBy(x => x.Text).ToList();

            return lstBrands;
        }
        public List<SelectListItem> GetOutlet(string GroupId)
        {
            List<SelectListItem> lstOutlets = new List<SelectListItem>();
            try
            {
                var connStr = GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var Outlets = contextNew.tblOutletMasters.Where(x => x.GroupId == GroupId).ToList();

                    foreach (var item in Outlets)
                    {
                        lstOutlets.Add(new SelectListItem
                        {
                            Text = item.OutletName,
                            Value = Convert.ToString(item.OutletId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutlet");
            }

            lstOutlets = lstOutlets.OrderBy(x => x.Text).ToList();

            return lstOutlets;
        }
    }
}
