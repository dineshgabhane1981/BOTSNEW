﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Net;
using WebApp.ViewModel;
using System.Data;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using WebApp.App_Start;
using BOTS_BL.Models.IndividualDBModels;

namespace WebApp.Controllers.ITOPS
{
    public class TransactionController : Controller
    {
        ITOPSNEWRepository NEWITOPS = new ITOPSNEWRepository();
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: Transaction
        public ActionResult Index()
        {
            var groupId = (string)Session["GroupId"];
            try
            {

                string connStr = objCustRepo.GetCustomerConnString(groupId);
                var lstOutlet = RR.GetOutletList(groupId, connStr);
                var lstBrand = RR.GetBrandList(groupId, connStr);
                var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
                ViewBag.OutletList = lstOutlet;
                ViewBag.BranchList = lstBrand;
                ViewBag.GroupId = groupId;
                ViewBag.GroupName = GroupDetails.RetailName;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Index");
            }
            return View();
        }
        public ActionResult GetCancelTxnData(string MobileNo, string InvoiceNo)
        {
            var groupId = (string)Session["GroupId"];
            MemberData objCustomerDetail = new MemberData();
            CancelTxnViewModel objData = new CancelTxnViewModel();
            try
            {
                if (!string.IsNullOrEmpty(InvoiceNo) && !string.IsNullOrEmpty(MobileNo))
                {

                    objData.objCancelTxnModel = ITOPS.GetTransactionByInvoiceNoAndMobileNo(groupId, MobileNo, InvoiceNo);
                    objData.objCustomerDetail = ITOPS.GetCustomerByMobileNo(groupId, objData.objCancelTxnModel.MobileNo);
                }
                else if (!string.IsNullOrEmpty(InvoiceNo))
                {
                    objData.objCancelTxnModel = ITOPS.GetTransactionByInvoiceNo(groupId, InvoiceNo);
                    objData.objCustomerDetail = ITOPS.GetCustomerByMobileNo(groupId, objData.objCancelTxnModel.MobileNo);
                }
                else if (!string.IsNullOrEmpty(MobileNo))
                {
                    objData.objCustomerDetail = ITOPS.GetCustomerByMobileNo(groupId, MobileNo);
                    objData.lstCancelTxnModel = ITOPS.GetTransactionByMobileNo(groupId, MobileNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCancelTxnData");
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModifyTxnData(string TransactionId)
        {
            //string GroupId = "";
            var groupId = (string)Session["GroupId"];
            CancelTxnViewModel objData = new CancelTxnViewModel();
            try
            {
                objData.objCancelTxnModel = ITOPS.GetTransactionByTransactionId(groupId, TransactionId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetModifyTxnData");
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public bool ModifyTransaction(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            bool result = false;
            string GroupId = "";
            try
            {
                var groupId = (string)Session["GroupId"];
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                //string GroupId = "";
                string TransactionId = "";
                decimal points = 0;

                foreach (Dictionary<string, object> item in objData)
                {
                    //GroupId = Convert.ToString(item["GroupID"]);
                    TransactionId = Convert.ToString(item["TransactionId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Points"])))
                    {
                        points = Convert.ToDecimal(item["Points"]);
                    }
                    objAudit.GroupId = groupId;
                    objAudit.RequestedFor = "Add / Earn";
                    objAudit.RequestedEntity = "Transaction For  - " + TransactionId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;

                }
                result = ITOPS.ModifyTransaction(groupId, Convert.ToInt64(TransactionId), points, objAudit);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ModifyTransaction");
            }
            return result;
        }

        public void SendEmail(string GroupId, string Subject, string EmailBody)
        {
            var senderEmail = System.Configuration.ConfigurationManager.AppSettings["Email"];
            var senderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];

            var email = new MailAddress(senderEmail, "Support Blue Ocktopus");
            var toemail = ITOPS.GetCustomerAdminEmail(GroupId);
            if (!string.IsNullOrEmpty(toemail))
            {
                var receiverEmail = new MailAddress(toemail);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(email.Address, senderEmailPassword)
                };
                using (var mess = new MailMessage(email, receiverEmail)
                {
                    Subject = Subject,
                    Body = EmailBody,
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                    BodyEncoding = System.Text.Encoding.UTF8
                })
                {
                    smtp.Send(mess);
                }
            }
        }
        public ActionResult PointTransfer()
        {
            var groupId = (string)Session["GroupId"];
            try
            {
                ViewBag.GroupId = groupId;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "PointTransfer");
            }
            return View();
        }
        public ActionResult TransferPoints(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            SPResponse result = new SPResponse();
            var groupId = (string)Session["GroupId"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

                string CustomerId = "";
                string MobileNo = "";
                string NewMobileNo = "";


                foreach (Dictionary<string, object> item in objData)
                {
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    NewMobileNo = Convert.ToString(item["NewMobileNo"]);
                    objAudit.GroupId = groupId;
                    objAudit.RequestedFor = "Mobile Number Change";
                    objAudit.RequestedEntity = "CustomerId - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.TransferPoints(groupId, MobileNo, NewMobileNo, objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "Customer Mobile Number changed for CustomerId - " + CustomerId;
                    var body = "Customer Mobile Number changed for CustomerId - " + CustomerId;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(groupId, subject, body);
                }

                if (IsSMS)
                {
                    //Logic to send SMS to Customer whose Name is changed
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "TransferPoints");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetChangeNameData(string MobileNo, string CardNo)
        {
            MemberData objCustomerDetail = new MemberData();
            var groupId = (string)Session["GroupId"];
            try
            {
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = ITOPS.GetChangeNameByMobileNo(groupId, MobileNo);
                }
                if (!string.IsNullOrEmpty(CardNo))
                {
                    objCustomerDetail = ITOPS.GetChangeNameByCardNo(groupId, CardNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameData");
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }

        /////////// ITOPS NEW ////////
        public ActionResult IndexNew()
        {
            try
            {
                var groupId = (string)Session["GroupId"];
                if (!string.IsNullOrEmpty(groupId))
                {
                    CommonFunctions common = new CommonFunctions();
                    Session["GroupId"] = groupId;
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    userDetails.GroupId = groupId;
                    userDetails.connectionString = NEWITOPS.GetCustomerConnString(groupId);
                    userDetails.CustomerName = objCustRepo.GetCustomerName(groupId);
                    Session["UserSession"] = userDetails;
                    Session["buttons"] = "ITOPS";
                    ViewBag.GroupId = groupId;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "IndexNew");
            }
            return View();
        }

        public ActionResult GetCancelTxnDataNew(string MobileNo, string InvoiceNo)
        {
            var groupId = (string)Session["GroupId"];
            MemberData objCustomerDetails = new MemberData();
            CancelTxnViewModel objData = new CancelTxnViewModel();
            try
            {
                if (!string.IsNullOrEmpty(InvoiceNo) && !string.IsNullOrEmpty(MobileNo))
                {

                    objData.objCancelTxnModel = NEWITOPS.GetTransactionByInvoiceNoAndMobileNo(groupId, MobileNo, InvoiceNo);
                    objData.objMemberData = NEWITOPS.GetChangeNameByMobileNo(groupId, MobileNo);
                }
                else if (!string.IsNullOrEmpty(InvoiceNo))
                {
                    objData.objCancelTxnModel = NEWITOPS.GetTransactionByInvoiceNo(groupId, InvoiceNo);
                    objData.objMemberData = NEWITOPS.GetCustomerByMobileNo(groupId, objData.objCancelTxnModel.MobileNo);
                }
                else if (!string.IsNullOrEmpty(MobileNo))
                {
                    objData.objMemberData = NEWITOPS.GetCustomerByMobileNo(groupId, MobileNo);
                    objData.lstCancelTxnModel = NEWITOPS.GetTransactionByMobileNo(groupId, MobileNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCancelTxnDataNew");
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModifyTxnDataNew(string TransactionId)
        {
            //string GroupId = "";
            var groupId = (string)Session["GroupId"];
            CancelTxnViewModel objData = new CancelTxnViewModel();
            try
            {
                objData.objCancelTxnModel = NEWITOPS.GetTransactionByTransactionId(groupId, TransactionId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetModifyTxnDataNew");
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public bool ModifyTransactionNew(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            bool result = false;
            string GroupId = "";
            try
            {
                var groupId = (string)Session["GroupId"];
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                //string GroupId = "";
                string TransactionId = "";
                decimal points = 0;

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = groupId;
                    TransactionId = Convert.ToString(item["TransactionId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Points"])))
                    {
                        points = Convert.ToDecimal(item["Points"]);
                    }
                    objAudit.GroupId = groupId;
                    objAudit.RequestedFor = "Add / Earn";
                    objAudit.RequestedEntity = "Transaction For  - " + TransactionId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;

                }
                result = NEWITOPS.ModifyTransaction(groupId, Convert.ToInt64(TransactionId), points, objAudit);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ModifyTransaction");
            }
            return result;
        }

        public ActionResult PointTransferNew()
        {
            var groupId = (string)Session["GroupId"];
            try
            {
                ViewBag.GroupId = groupId;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "PointTransfer");
            }
            return View();
        }
        public ActionResult GetChangeNameDataNew(string MobileNo, string CardNo)
        {
            MemberData objCustomerDetail = new MemberData();
            var groupId = (string)Session["GroupId"];
            try
            {
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = NEWITOPS.GetChangeNameByMobileNo(groupId, MobileNo);
                }
                if (!string.IsNullOrEmpty(CardNo))
                {
                    objCustomerDetail = NEWITOPS.GetChangeNameByCardNo(groupId, CardNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameData");
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransferPointsNew(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            SPResponse result = new SPResponse();
            var groupId = (string)Session["GroupId"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblCustDetailsMaster objCustomer = new tblCustDetailsMaster();
                tblCustPointsMaster objCustPointsMaster = new tblCustPointsMaster();
                tblCustInfo objcustInfo = new tblCustInfo();
                tblCustTxnSummaryMaster objCustTxnSummaryMaster = new tblCustTxnSummaryMaster();
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

                string MobileNo = "";
                string NewMobileNo = "";

                foreach (Dictionary<string, object> item in objData)
                {
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    NewMobileNo = Convert.ToString(item["NewMobileNo"]);

                    // tblCustDetailsMaster
                    objCustomer.Name = "Member";
                    objCustomer.MobileNo = Convert.ToString(item["NewMobileNo"]);
                    objCustomer.Tier = "Base";
                    objCustomer.DOJ = DateTime.Now;
                    objCustomer.IsActive = true;
                    objCustomer.DisableTxn = false;
                    objCustomer.DisableSMSWAPromo = false;
                    objCustomer.CountryCode = "91";
                    objCustomer.DisableSMSWATxn = false;

                    // tblCustPointsMaster
                    objCustPointsMaster.MobileNo = Convert.ToString(item["NewMobileNo"]);
                    objCustPointsMaster.PointsType = "Base";
                    objCustPointsMaster.PointsDesc = "Base";
                    objCustPointsMaster.IsActive = true;
                    objCustPointsMaster.MinInvoiceAmtRequired = 0;

                    // tblCustInfo
                    objcustInfo.MobileNo = Convert.ToString(item["NewMobileNo"]);
                    objcustInfo.Name = "Member";

                    // tblCustTxnSummaryMaster
                    objCustTxnSummaryMaster.MobileNo = Convert.ToString(item["NewMobileNo"]);
                    objCustTxnSummaryMaster.TotalSpend = 0;
                    objCustTxnSummaryMaster.TotalTxnCount = 0;
                    objCustTxnSummaryMaster.EarnCount = 0;
                    objCustTxnSummaryMaster.BurnCount = 0;
                    objCustTxnSummaryMaster.SalesReturnCount = 0;
                    objCustTxnSummaryMaster.SalesReturnAmt = 0;
                    objCustTxnSummaryMaster.BurnAmtWithPts = 0;
                    objCustTxnSummaryMaster.BurnAmtWithoutPts = 0;
                    objCustTxnSummaryMaster.BurnPts = 0;
                    objCustTxnSummaryMaster.EarnPts = 0;
                    objCustTxnSummaryMaster.SalesReturnPtsGiven = 0;
                    objCustTxnSummaryMaster.SalesReturnPtsRemoved = 0;

                    // tblAudit
                    objAudit.GroupId = groupId;
                    objAudit.RequestedFor = "Points Transfer";
                    objAudit.RequestedEntity = "PointsTransfer - " + objCustomer.MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }
                result = NEWITOPS.TransferPoints(groupId, MobileNo, NewMobileNo, objCustomer, objCustPointsMaster, objcustInfo, objCustTxnSummaryMaster, objAudit);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "TransferPointsNew");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTestTxn()
        {
            var groupId = (string)Session["GroupId"];
            ViewBag.GroupId = groupId;
            return View();
        }

        public ActionResult DeleteTestTxnData(string GroupId, string MobileNo)
        {
           
            SPResponse result = new SPResponse();
            result = NEWITOPS.DeleteTestTxnData(GroupId, MobileNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeInvoiceMobileNo()
        {
            var groupId = (string)Session["GroupId"];
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            var lstOutlet = RR.GetOutletList(groupId, connStr);
            ViewBag.OutletList = lstOutlet;
            return View();
        }
        public ActionResult GetInvoiceDetails(string InvoiceNo,string OutletId)
        {           
            var groupId = (string)Session["GroupId"];
            tblTxnDetailsMaster objData = new tblTxnDetailsMaster();
            try
            {
                objData = NEWITOPS.GetTxnDetailsByInvoiceNo(groupId, InvoiceNo, OutletId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetModifyTxnDataNew");
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateInvoiceData(string InvoiceNo, string OutletId,string NewMobileNo)
        {
            var groupId = (string)Session["GroupId"];
            SPResponse status = new SPResponse();
            try
            {
                status = NEWITOPS.UpdateInvoiceData(groupId, InvoiceNo, OutletId, NewMobileNo);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetModifyTxnDataNew");
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

    }
}