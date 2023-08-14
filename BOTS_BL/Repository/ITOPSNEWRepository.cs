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
using BOTS_BL.Models.ITOps;

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
                ITOPSCustData ObjCustData = new ITOPSCustData();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //objCustomerDetail = contextNew.CustomerDetails.Where(x => x.MobileNo == searchData && x.Status == "00").FirstOrDefault();
                    //var CustData = contextNew.View_ITOPSCustData.Where(x => x.MobileNo == searchData).FirstOrDefault();

                    ObjCustData = contextNew.Database.SqlQuery<ITOPSCustData>("select MobileNo,CustomerName,EnrolledOutlet,DOJ,CardNo,CustomerId,Points from View_ITOPSCustData where MobileNo = @MobileNo", new SqlParameter("@MobileNo", searchData)).FirstOrDefault();

                    if (ObjCustData != null)
                    {
                        string Id = GroupId + ObjCustData.MobileNo;
                        objMemberData.MemberName = ObjCustData.CustomerName;
                        objMemberData.MobileNo = ObjCustData.MobileNo;
                        objMemberData.CardNo = ObjCustData.CardNo;
                        objMemberData.PointsBalance = Convert.ToDecimal(ObjCustData.Points);
                        objMemberData.EnrolledOn = ObjCustData.DOJ.Value.ToString("dd/MM/yyyy");
                        objMemberData.EnrolledOutletName = ObjCustData.EnrolledOutlet;
                        objMemberData.CustomerId = Id;
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

            //Int64 Data = Convert.ToInt64(searchData);
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                ITOPSCustData ObjCustData = new ITOPSCustData();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CardNumber == searchData && x.Status == "00").FirstOrDefault();
                    //var CustData = contextNew.View_ITOPSCustData.Where(x => x.CardNo == searchData).FirstOrDefault();
                    ObjCustData = contextNew.Database.SqlQuery<ITOPSCustData>("select MobileNo,CustomerName,EnrolledOutlet,DOJ,CardNo,CustomerId,Points from View_ITOPSCustData where CardNo = '"+ searchData + "' ").FirstOrDefault();

                    if (ObjCustData != null)
                    {
                        objMemberData.MemberName = ObjCustData.CustomerName;
                        objMemberData.MobileNo = ObjCustData.MobileNo;
                        objMemberData.CardNo = ObjCustData.CardNo;
                        objMemberData.PointsBalance = ObjCustData.Points;
                        objMemberData.EnrolledOn = ObjCustData.DOJ.Value.ToString("dd/MM/yyyy");
                        objMemberData.EnrolledOutletName = ObjCustData.EnrolledOutlet;
                        objMemberData.CustomerId = ObjCustData.CustomerId;
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
        public bool UpdateNameOfMember(string GroupId, string CustomerId, string Name, tblAudit objAudit)
        {
            bool status = false;
            string MobileNo = CustomerId.Substring(4);
            try
            {
                //CustomerDetail objCustomerDetail = new CustomerDetail();
                tblCustInfo objCustomerDetail = new tblCustInfo();
                tblCustDetailsMaster objCustDetail = new tblCustDetailsMaster();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //objCustomerDetail = contextNew.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                    objCustomerDetail = contextNew.tblCustInfoes.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    objCustDetail = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();

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
            string DBName = string.Empty;
            SPResponse result = new SPResponse();
            try
            {
                TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

                string OldMobileNo = CustomerId.Substring(4);


                string connStr = GetCustomerConnString(GroupId);

                using (var context = new CommonDBContext())
                {
                    DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                }

                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //var objExisting = contextNew.CustomerDetails.Where(x => x.MobileNo == MobileNo).FirstOrDefault();

                    var objExisting = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();

                    if (objExisting == null)
                    {

                        SqlConnection _Con = new SqlConnection(connStr);
                        DataSet DT = new DataSet();
                        SqlCommand cmdReport = new SqlCommand("sp_ITOPSChangeMobileNo", _Con);
                        SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                        using (cmdReport)
                        {
                            SqlParameter param1 = new SqlParameter("pi_OldMobileNo", OldMobileNo);
                            SqlParameter param2 = new SqlParameter("pi_NewMobileNo", MobileNo);
                            SqlParameter param3 = new SqlParameter("pi_LoginId", objAudit.AddedBy);
                            SqlParameter param4 = new SqlParameter("pi_RequestBy", objAudit.RequestedBy);
                            SqlParameter param5 = new SqlParameter("pi_RequestedOnForum", objAudit.RequestedOnForum);
                            SqlParameter param6 = new SqlParameter("pi_DBName", DBName);
                            SqlParameter param7 = new SqlParameter("pi_INDDatetime", Date);

                            cmdReport.CommandType = CommandType.StoredProcedure;
                            cmdReport.Parameters.Add(param1);
                            cmdReport.Parameters.Add(param2);
                            cmdReport.Parameters.Add(param3);
                            cmdReport.Parameters.Add(param4);
                            cmdReport.Parameters.Add(param5);
                            cmdReport.Parameters.Add(param6);
                            cmdReport.Parameters.Add(param7);

                            daReport.Fill(DT);

                            DataTable dt = DT.Tables[0];
                            string ResCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            result.ResponseCode = ResCode;

                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {

                            }
                        }
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
            string DBName = string.Empty;
            SPResponse result = new SPResponse();

            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

            try
            {
                string connStr = GetCustomerConnString(GroupId);

                using (var context = new CommonDBContext())
                {
                    DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                }

                using (var contextNew = new BOTSDBContext(connStr))
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
                        SqlCommand cmdReport = new SqlCommand("sp_ITOPSEarn", _Con);
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
                            SqlParameter param13 = new SqlParameter("pi_DBName", DBName);
                            SqlParameter param14 = new SqlParameter("pi_INDDatetime", Date);

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
                            cmdReport.Parameters.Add(param14);

                            daReport.Fill(DT);

                            DataTable dt = DT.Tables[0];
                            string ResCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            result.ResponseCode = ResCode;

                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                               if(IsSMS == "1")
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
            string DBName = string.Empty;
            SPResponse result = new SPResponse();

            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

            try
            {
                string connStr = GetCustomerConnString(GroupId);

                using (var context = new CommonDBContext())
                {
                    DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                }

                using (var contextNew = new BOTSDBContext(connStr))
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
                        SqlCommand cmdReport = new SqlCommand("sp_ITOPSBurn", _Con);
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
                            SqlParameter param14 = new SqlParameter("pi_DBName", DBName);
                            SqlParameter param15 = new SqlParameter("pi_INDDatetime", Date);

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
                            cmdReport.Parameters.Add(param14);
                            cmdReport.Parameters.Add(param15);

                            daReport.Fill(DT);

                            DataTable dt = DT.Tables[0];
                            string ResCode = Convert.ToString(dt.Rows[0]["ResponseCode"]);
                            result.ResponseCode = ResCode;

                            if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                            {
                                if (IsSMS == "1")
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
            string DBName = string.Empty;
            SPResponse result = new SPResponse();
            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

            try
            {
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    result = contextNew.Database.SqlQuery<SPResponse>("sp_ITOPSAddBonus @pi_MobileNo, @pi_OutletId, @pi_RequestDate, @pi_ExpiryDate, @pi_BonusRemarks, @pi_BonusPoints,@pi_LoginId, @pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag,@pi_DBName,@pi_INDDatetime",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_OutletId", OutletId),
                              new SqlParameter("@pi_RequestDate", objAudit.RequestedOn.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_ExpiryDate", ExpiryDate.ToString("yyyy-MM-dd")),
                              new SqlParameter("@pi_BonusRemarks", BonusRemark),
                              new SqlParameter("@pi_BonusPoints", BonusPoints),
                              new SqlParameter("@pi_LoginId", ""),
                              new SqlParameter("@pi_RequestBy", objAudit.RequestedBy),
                              new SqlParameter("@pi_RequestedOnForum", objAudit.RequestedOnForum),
                              new SqlParameter("@pi_SMSFlag", IsSMS),
                              new SqlParameter("@pi_DBName", DBName),
                              new SqlParameter("@pi_INDDatetime", Date)).FirstOrDefault<SPResponse>();

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

        public bool ChangeSMSDetails(string GroupId, string MobileNo, bool Disable, tblAudit objAudit)
        {
            bool status = false;
            try
            {
                tblCustDetailsMaster objCustDetail = new tblCustDetailsMaster();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {                    
                    objCustDetail = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    objCustDetail.DisableSMSWAPromo = Disable;

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

        public List<SelectListItem> GetGroupList(string GroupId, string connstr)
        {
            string DBStatus = string.Empty;
            List<SelectListItem> GroupItem = new List<SelectListItem>();
            try
            {
                using (var contextnew = new CommonDBContext())
                {
                    DBStatus = contextnew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.GroupId).FirstOrDefault();
                }
                using (var context = new BOTSDBContext(connstr))
                {

                    if (!string.IsNullOrEmpty(DBStatus))
                    {
                        var LstGroup = context.tblGroupMasters.Where(x => x.GroupId == GroupId).ToList();
                        foreach (var item in LstGroup)
                        {
                            GroupItem.Add(new SelectListItem
                            {
                                Text = item.GroupName,
                                Value = Convert.ToString(item.GroupId)
                            });
                        }
                    }
                    else
                    {
                        var LstGroup = context.Database.SqlQuery<OTPGroupDetails>("select GroupName,GroupId from GroupDetails where GroupId = @GroupId",new SqlParameter("@GroupId", GroupId)).ToList<OTPGroupDetails>();
                        foreach (var item in LstGroup)
                        {
                            GroupItem.Add(new SelectListItem
                            {
                                Text = item.GroupName,
                                Value = Convert.ToString(item.GroupId)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBrandList");
            }
            return GroupItem;

        }

        public List<CommonOTPDetails> GetCommonOTPDetails(string GroupId)
        {
            string DBName = string.Empty;
            List<CommonOTPDetails> Obj = new List<CommonOTPDetails>();
            try
            {
                using (var contextNew = new CommonDBContext())
                {
                    DBName = contextNew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                    Obj = contextNew.Database.SqlQuery<CommonOTPDetails>("select J.LoginId,J.Password,CASE WHEN J.LevelIndicator = 04 THEN 'Outlet' WHEN J.LevelIndicator = 03 THEN 'Brand' WHEN J.LevelIndicator = 02 THEN 'Group' END as LoginLevel, CASE WHEN J.Status = 1 THEN 'Active' WHEN J.Status = 0 THEN 'InActive' END as Status ,J.GroupId, CASE WHEN J.LevelIndicator = 04 THEN J.OutletName WHEN J.LevelIndicator = 03 THEN B.BrandName WHEN J.LevelIndicator = 02 THEN J.GroupName END as LoginType from (select D.LoginId,D.Password,D.LevelIndicator,D.LoginType,D.Status,D.GroupId,D.OutletName,M.GroupName from (select C.LoginId,C.Password,C.LevelIndicator,C.LoginType,C.Status,C.GroupId,L.OutletName as OutletName from CommonOTPCredentialMaster C Left Join " + DBName + ".dbo.tblOutletMaster L on C.LoginType = L.OutletId where C.DBStatus = 'New' and C.GroupId = '" + GroupId + "') as D inner join " + DBName + ".dbo.tblGroupMaster M on D.GroupId = M.GroupId) as J inner join " + DBName + ".dbo.tblBrandMaster B on J.GroupId = B.GroupId").ToList();


                } 

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOTPData");
            }
            return Obj;
        }

        public bool SaveOTPDetails(string GroupId,string LoginType,string LoginId, string Password,tblAudit objAudit)
        {
            string LevelIndicator = string.Empty;
            bool status;
            status = default;

            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

            try
            {
                using(var context = new CommonDBContext())
                {
                    var LoginObj = context.CommonOTPCredentialMasters.Where(x=> x.LoginId == LoginId && x.Password == Password && x.GroupId == GroupId).FirstOrDefault();

                    if(LoginType.Length == 4)
                    {
                        LevelIndicator = "02";
                    }
                    if (LoginType.Length == 5)
                    {
                        LevelIndicator = "03";
                    }
                    if (LoginType.Length == 8)
                    {
                        LevelIndicator = "04";
                    }


                    CommonOTPCredentialMaster Obj = new CommonOTPCredentialMaster();
                    if(LoginObj == null)
                    {
                        Obj.LoginId = LoginId;
                        Obj.Password = Password;
                        Obj.LevelIndicator = LevelIndicator;
                        Obj.CreatedDate = Date;
                        Obj.LoginType = LoginType;
                        Obj.Status = "1";
                        Obj.GroupId = GroupId;
                        Obj.DBStatus = "New";
                        context.CommonOTPCredentialMasters.AddOrUpdate(Obj);
                    }
                    else
                    {
                        LoginObj.LoginId = LoginId;
                        LoginObj.Password = Password;
                        LoginObj.LevelIndicator = LevelIndicator;
                        LoginObj.LoginType = LoginType;
                        LoginObj.GroupId = GroupId;
                        context.CommonOTPCredentialMasters.AddOrUpdate(LoginObj);
                    }

                    context.tblAudits.AddOrUpdate(objAudit);
                    context.SaveChanges();
                    status = true;
                }

            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "SaveOTPDetails");
            }

            return status;
        }

        public CancelTxnModel GetTransactionByInvoiceNo(string GroupId, string InvoiceNo)
        {
            CancelTxnModel objReturn = new CancelTxnModel();
            try
            {                
                ITOPSCustTxnData ObjCustTxnData = new ITOPSCustTxnData();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    ObjCustTxnData = contextNew.Database.SqlQuery<ITOPSCustTxnData>("select InvoiceNo,InvoiceAmt,MobileNo,PointsEarned,TxnDatetime,OutletName,TxnType,SlNo from View_ITOPSCustTxnData where InvoiceNo = @InvoiceNo", new SqlParameter("@InvoiceNo", InvoiceNo)).FirstOrDefault();
                   
                    if (ObjCustTxnData != null)
                    {
                        objReturn.InvoiceNo = ObjCustTxnData.InvoiceNo;
                        objReturn.InvoiceAmt = Convert.ToDecimal(ObjCustTxnData.InvoiceAmt);
                        objReturn.MobileNo = ObjCustTxnData.MobileNo;
                        objReturn.Points = Convert.ToString(ObjCustTxnData.PointsEarned);
                        objReturn.DatetimeOriginal = Convert.ToString(ObjCustTxnData.TxnDatetime);
                        objReturn.OutletName = ObjCustTxnData.OutletName;
                        objReturn.TransactionName = ObjCustTxnData.TxnType;
                        objReturn.TransactionId = ObjCustTxnData.SlNo;
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
                ITOPSCustTxnData ObjCustTxnData = new ITOPSCustTxnData();
                string connStr = GetCustomerConnString(GroupId);
                long TxnId = Convert.ToInt64(TransactionId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    ObjCustTxnData = contextNew.Database.SqlQuery<ITOPSCustTxnData>("select InvoiceNo,InvoiceAmt,MobileNo,PointsEarned,TxnDatetime,OutletName,TxnType,SlNo from View_ITOPSCustTxnData where SlNo = @SlNo", new SqlParameter("@SlNo", TransactionId)).FirstOrDefault();
                    
                    if (ObjCustTxnData != null)
                    {
                        objReturn.InvoiceNo = ObjCustTxnData.InvoiceNo;
                        objReturn.InvoiceAmt = Convert.ToDecimal(ObjCustTxnData.InvoiceAmt);
                        objReturn.MobileNo = ObjCustTxnData.MobileNo;
                        objReturn.Points = Convert.ToString(ObjCustTxnData.PointsEarned);
                        objReturn.DatetimeOriginal = Convert.ToString(ObjCustTxnData.TxnDatetime);
                        objReturn.OutletName = ObjCustTxnData.OutletName;
                        objReturn.TransactionName = ObjCustTxnData.TxnType;
                        objReturn.TransactionId = ObjCustTxnData.SlNo;
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
                   
                    var CustData = contextNew.View_ITOPSCustTxnData.Where(x => x.MobileNo == MobileNo && x.InvoiceNo == InvoiceNo).FirstOrDefault();

                    if (CustData != null)
                    {
                        objReturn.InvoiceNo = CustData.InvoiceNo;
                        objReturn.InvoiceAmt = CustData.InvoiceAmt;
                        objReturn.MobileNo = CustData.MobileNo;
                        objReturn.Points = Convert.ToString(CustData.PointsEarned);
                        objReturn.DatetimeOriginal = CustData.TxnDatetime.Value.ToString("dd/MM/yyyy");
                        objReturn.OutletName = CustData.OutletName;
                        objReturn.TransactionName = CustData.TxnType;
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
                List<ITOPSCustTxnData> lstObjTxn = new List<ITOPSCustTxnData>();
                string connStr = GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))

                {
                    lstObjTxn = contextNew.Database.SqlQuery<ITOPSCustTxnData>("select InvoiceNo,InvoiceAmt,MobileNo,PointsEarned,TxnDatetime,OutletName,TxnType,SlNo from View_ITOPSCustTxnData where MobileNo = @MobileNo", new SqlParameter("@MobileNo", MobileNo)).ToList<ITOPSCustTxnData>();

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
                            objReturn.Datetime = Convert.ToDateTime(objTxn.TxnDatetime).ToString("dd/MM/yyyy HH:mm:ss");
                            objReturn.OutletName = objTxn.OutletName;
                            objReturn.TransactionName = objTxn.TxnType;

                            lstObjReturn.Add(objReturn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionByMobileNo");
            }
            return lstObjReturn;
        }
        public MemberData GetCustomerByMobileNo(string GroupId, string MobileNo)
        {
            MemberData objMemberData = new MemberData();
            CustomerDetail objCustomerDetail = new CustomerDetail();
            try
            {
                string connStr = GetCustomerConnString(GroupId);
                ITOPSCustData ObjCustData = new ITOPSCustData();
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    //var CustData = contextNew.View_ITOPSCustData.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    ObjCustData = contextNew.Database.SqlQuery<ITOPSCustData>("select MobileNo,CustomerName,EnrolledOutlet,DOJ,CardNo,CustomerId,Points from View_ITOPSCustData where MobileNo = @MobileNo", new SqlParameter("@MobileNo", MobileNo)).FirstOrDefault();

                    if (ObjCustData != null)
                    {
                        objMemberData.MemberName = ObjCustData.CustomerName;
                        objMemberData.MobileNo = ObjCustData.MobileNo;
                        objMemberData.CardNo = ObjCustData.CardNo;
                        objMemberData.PointsBalance = ObjCustData.Points;
                        objMemberData.EnrolledOn = ObjCustData.DOJ.Value.ToString("dd/MM/yyyy");
                        objMemberData.EnrolledOutletName = ObjCustData.EnrolledOutlet;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerByMobileNo");
            }
            return objMemberData;
        }

        public SPResponse DeleteTransaction(string GroupId, string InvoiceNo, string MobileNo, string InvoiceAmt, DateTime ip_Date, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();

            string DBName = string.Empty;

            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

            using (var context = new CommonDBContext())
            {
                DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
            }

            try
            {
                string connStr =  GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    result = contextNew.Database.SqlQuery<SPResponse>("sp_ITOPSCancel @pi_MobileNo, @pi_InvoiceNo, @pi_InvoiceAmt, @pi_LoginId,@pi_RequestBy, @pi_RequestedOnForum, @pi_SMSFlag,@pi_DBName,@pi_INDDatetime",
                              new SqlParameter("@pi_MobileNo", MobileNo),
                              new SqlParameter("@pi_InvoiceNo", InvoiceNo),
                              new SqlParameter("@pi_InvoiceAmt", InvoiceAmt),
                              new SqlParameter("@pi_LoginId", objAudit.AddedBy),
                              new SqlParameter("@pi_RequestBy", objAudit.RequestedBy),
                              new SqlParameter("@pi_RequestedOnForum", objAudit.RequestedOnForum),
                              new SqlParameter("@pi_SMSFlag", "0"),
                              new SqlParameter("@pi_DBName", DBName),
                              new SqlParameter("@pi_INDDatetime", Date)
                              ).FirstOrDefault<SPResponse>();

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
                long Points = Convert.ToInt64(points);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    using (DbContextTransaction transaction = contextNew.Database.BeginTransaction())
                    {
                        try
                        {
                            //var objTransaction = contextNew.TransactionMasters.Where(x => x.SlNo == TransactionId).FirstOrDefault();
                            //var customerDetails = contextNew.CustomerDetails.Where(x => x.MobileNo == objTransaction.MobileNo).FirstOrDefault();
                            //var oldTransactionPoints = objTransaction.PointsEarned;

                            var objTxnDetail = contextNew.tblTxnDetailsMasters.Where(x => x.SlNo == TransactionId).FirstOrDefault();
                            var objCustTxnSummary = contextNew.tblCustTxnSummaryMasters.Where(x => x.MobileNo == objTxnDetail.MobileNo).FirstOrDefault();
                            var objCustPointsMaster = contextNew.tblCustPointsMasters.Where(x => x.MobileNo == objTxnDetail.MobileNo).FirstOrDefault();
                            var oldTransactionPoints = objTxnDetail.PointsEarned;

                            objTxnDetail.PointsEarned = Points;
                            objTxnDetail.CustBalancePts = (objTxnDetail.CustBalancePts - oldTransactionPoints) + Points;
                            contextNew.tblTxnDetailsMasters.AddOrUpdate(objTxnDetail);
                            contextNew.SaveChanges();

                            //customerDetails.Points = (customerDetails.Points - oldTransactionPoints) + points;
                            //contextNew.CustomerDetails.AddOrUpdate(customerDetails);
                            //contextNew.SaveChanges();

                            objCustTxnSummary.EarnPts = (Convert.ToInt64(objCustTxnSummary.EarnPts - oldTransactionPoints)) + Points;
                            contextNew.tblCustTxnSummaryMasters.AddOrUpdate(objCustTxnSummary);
                            contextNew.SaveChanges();

                            objCustPointsMaster.Points = (Convert.ToInt64(objCustPointsMaster.Points - oldTransactionPoints)) + Points;
                            contextNew.tblCustPointsMasters.AddOrUpdate(objCustPointsMaster);
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

        public SPResponse TransferPoints(string GroupId, string MobileNo, string NewMobileNo, tblCustDetailsMaster objCustomer, tblCustPointsMaster objCustPointsMaster, tblCustInfo objcustInfo, tblCustTxnSummaryMaster objCustTxnSummaryMaster, tblAudit objAudit)
        {
            SPResponse result = new SPResponse();

            string connStr = GetCustomerConnString(GroupId);
            try
            {
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    using (DbContextTransaction transaction = contextNew.Database.BeginTransaction())
                    {
                        try
                        {
                            // Insert in tblCustDetailsMasters
                            var AdminOutletId = contextNew.tblOutletMasters.Where(x => x.OutletName.Contains("Admin")).Select(y => y.OutletId).FirstOrDefault();
                            var ObjMobileNo = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == NewMobileNo).FirstOrDefault();
                            if (ObjMobileNo == null)
                            {
                                var NewGroupId = GroupId;
                                var NewMobileNo1 = NewMobileNo;
                                var NewId = Convert.ToInt64(string.Format("{0}{1}", NewGroupId, NewMobileNo1));
                                objCustomer.Id = Convert.ToString(NewId);
                                objCustomer.EnrolledOutlet = AdminOutletId;
                                objCustomer.CurrentEnrolledOutlet = AdminOutletId;

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
                            // Insert in tblCustInfoes
                            var ObjCustNoNew = contextNew.tblCustInfoes.Where(x => x.MobileNo == NewMobileNo).FirstOrDefault();
                            if (ObjCustNoNew == null)
                            {
                                var CustNoNew = NewMobileNo;
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
                            // Insert in tblCustTxnSummaryMasters
                            var ObjMobileNoNew1 = contextNew.tblCustTxnSummaryMasters.Where(x => x.MobileNo == NewMobileNo).FirstOrDefault();
                            if (ObjMobileNoNew1 == null)
                            {
                                var MobileNoNew1 = NewMobileNo;
                                contextNew.tblCustTxnSummaryMasters.AddOrUpdate(objCustTxnSummaryMaster);
                                contextNew.SaveChanges();
                                result.ResponseCode = "00";
                                result.ResponseMessage = "Member Added Successfully";
                            }
                            else
                            {
                                result.ResponseCode = "01";
                                result.ResponseMessage = "Mobile Number Already Exist";
                            }
                            // Insert in tblCustPointsMasters
                            var objExisting = contextNew.tblCustPointsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                            var objExistingNew = contextNew.tblCustPointsMasters.Where(x => x.MobileNo == NewMobileNo).FirstOrDefault();
                            if (objExisting != null && objExistingNew != null)
                            {
                                string oldno = objExisting.MobileNo;
                                var transferPoints = objExisting.Points;
                                var AddPoints = objExisting.Points + objExistingNew.Points;
                                objExistingNew.Points = AddPoints;
                                objExisting.Points = 0;
                                tblRuleMaster objRuleMaster = new tblRuleMaster();
                                var ExpiryMonth = contextNew.tblRuleMasters.Where(x => x.GroupId == GroupId).FirstOrDefault();
                                var PointExpiryMonthNew = ExpiryMonth.PointsExpiryMonths;
                                var StartDate = objExistingNew.StartDate;
                                var EndDate = StartDate.Value.AddMonths(Convert.ToInt32(PointExpiryMonthNew));
                                objExistingNew.EndDate = EndDate;

                                contextNew.tblCustPointsMasters.AddOrUpdate(objExistingNew);
                                contextNew.SaveChanges();
                                contextNew.tblCustPointsMasters.AddOrUpdate(objExisting);
                                contextNew.SaveChanges();

                                // Insert in tblPtsTransferDetail
                                tblPtsTransferDetail objtblPtsTransferDetail = new tblPtsTransferDetail();
                                objtblPtsTransferDetail.PtsFromMobileNo = MobileNo;
                                objtblPtsTransferDetail.PtsToMobileNo = NewMobileNo;
                                objtblPtsTransferDetail.PtsTransferred = transferPoints;
                                objtblPtsTransferDetail.TxnDatetime = DateTime.Now;
                                objtblPtsTransferDetail.GroupId = GroupId;
                                objtblPtsTransferDetail.IsActive = true;
                                contextNew.tblPtsTransferDetails.AddOrUpdate(objtblPtsTransferDetail);
                                contextNew.SaveChanges();

                                result.ResponseCode = "00";
                                result.ResponseMessage = "Points Transferred Successfully";
                            }
                            else
                            {
                                var objExisting1 = contextNew.tblCustPointsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                                var objExistingNew1 = contextNew.tblCustPointsMasters.Where(x => x.MobileNo == NewMobileNo).FirstOrDefault();
                                if (objExistingNew1 == null)
                                {
                                    var MobileNo1 = NewMobileNo;
                                    objCustPointsMaster.MobileNo = MobileNo1;
                                    objCustPointsMaster.Points = objExisting1.Points;
                                    objExisting1.Points = 0;
                                    objCustPointsMaster.StartDate = DateTime.Now;
                                    var MobileNoPtsIdNew = MobileNo1 + "Base";
                                    objCustPointsMaster.MobileNoPtsId = MobileNoPtsIdNew;
                                    tblRuleMaster objRuleMaster = new tblRuleMaster();
                                    var ExpiryMonth = contextNew.tblRuleMasters.Where(x => x.GroupId == GroupId).FirstOrDefault();
                                    var PointExpiryMonthNew = ExpiryMonth.PointsExpiryMonths;
                                    objCustPointsMaster.EndDate = objCustPointsMaster.StartDate.Value.AddMonths(Convert.ToInt32(PointExpiryMonthNew));
                                    var EndDate = objCustPointsMaster.EndDate;

                                    contextNew.tblCustPointsMasters.AddOrUpdate(objCustPointsMaster);
                                    contextNew.SaveChanges();

                                    tblPtsTransferDetail objtblPtsTransferDetail = new tblPtsTransferDetail();
                                    objtblPtsTransferDetail.PtsFromMobileNo = MobileNo;
                                    objtblPtsTransferDetail.PtsToMobileNo = NewMobileNo;
                                    objtblPtsTransferDetail.PtsTransferred = objCustPointsMaster.Points;
                                    objtblPtsTransferDetail.TxnDatetime = DateTime.Now;
                                    objtblPtsTransferDetail.GroupId = GroupId;
                                    objtblPtsTransferDetail.IsActive = true;
                                    contextNew.tblPtsTransferDetails.AddOrUpdate(objtblPtsTransferDetail);
                                    contextNew.SaveChanges();

                                    result.ResponseCode = "00";
                                    result.ResponseMessage = "Points Transferred Successfully";
                                    
                                }
                                using (var context = new CommonDBContext())
                                {
                                    context.tblAudits.Add(objAudit);
                                    context.SaveChanges();
                                }
                            }
                            transaction.Commit();
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
