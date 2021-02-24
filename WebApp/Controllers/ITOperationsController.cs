using BOTS_BL;
using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Net;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class ITOperationsController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: ITOperations
        public ActionResult Index(string groupId)
        {
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            var lstOutlet = RR.GetOutletList(groupId, connStr);
            var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
            ViewBag.OutletList = lstOutlet;
            ViewBag.GroupId = groupId;
            ViewBag.GroupName = GroupDetails.RetailName;
            return View();
        }

        public ActionResult GetChangeNameData(string GroupId, string MobileNo, string CardNo)
        {
            MemberData objCustomerDetail = new MemberData();
            if (!string.IsNullOrEmpty(MobileNo))
            {
                objCustomerDetail = ITOPS.GetChangeNameByMobileNo(GroupId, MobileNo);
            }
            if (!string.IsNullOrEmpty(CardNo))
            {
                objCustomerDetail = ITOPS.GetChangeNameByCardNo(GroupId, CardNo);
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool ChangeMemberName(string jsonData)
        {
            bool result = false;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;
                string GroupId = "";
                string CustomerId = "";
                string Name = "";
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    Name = Convert.ToString(item["Name"]);
                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Name Change";
                    objAudit.RequestedEntity = "CustomerId - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.UpdateNameOfMember(GroupId, CustomerId, Name, objAudit);
                if (result)
                {
                    var subject = "Customer name changed for CustomerId - " + CustomerId;
                    var body = "Customer name changed for CustomerId - " + CustomerId;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }

                if (IsSMS)
                {
                    //Logic to send SMS to Customer whose Name is changed
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return result;
        }

        [HttpPost]
        public ActionResult ChangeMemberMobile(string jsonData)
        {
            SPResponse result = new SPResponse();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;
                string GroupId = "";
                string CustomerId = "";
                string MobileNo = "";
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);

                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Mobile Number Change";
                    objAudit.RequestedEntity = "CustomerId - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.UpdateMobileOfMember(GroupId, CustomerId, MobileNo, objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "Customer Mobile Number changed for CustomerId - " + CustomerId;
                    var body = "Customer Mobile Number changed for CustomerId - " + CustomerId;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }

                if (IsSMS)
                {
                    //Logic to send SMS to Customer whose Name is changed
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEarnData(string jsonData)
        {
            SPResponse result = new SPResponse();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;
                string GroupId = "";
                string MobileNo = "";
                string TransactionDate = "";
                string InvoiceNumber = "";
                string InvoiceAmount = "";
                string OutletId = "";

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    OutletId = Convert.ToString(item["OutletId"]);
                    TransactionDate = Convert.ToString(item["TransactionDate"]);
                    InvoiceNumber = Convert.ToString(item["InvoiceNumber"]);
                    InvoiceAmount = Convert.ToString(item["InvoiceAmount"]);

                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Add / Earn";
                    objAudit.RequestedEntity = "Mobile No - " + MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.AddEarnData(GroupId, MobileNo, OutletId, Convert.ToDateTime(TransactionDate), DateTime.Now, InvoiceNumber, InvoiceAmount, Convert.ToString(IsSMS), objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "Earning updated for mobile no  - " + MobileNo;
                    var body = "Earning updated for mobile no - " + MobileNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }

                if (IsSMS)
                {
                    //Logic to send SMS to Customer whose Name is changed
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RedeemPointsData(string jsonData)
        {
            SPResponse result = new SPResponse();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;
                string GroupId = "";
                string MobileNo = "";
                string TransactionDate = "";
                string InvoiceNumber = "";
                string InvoiceAmount = "";
                string OutletId = "";
                string PointsToRedeem = "";
                string TxnType = "";

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    OutletId = Convert.ToString(item["OutletId"]);
                    TransactionDate = Convert.ToString(item["TransactionDate"]);
                    InvoiceNumber = Convert.ToString(item["InvoiceNumber"]);
                    InvoiceAmount = Convert.ToString(item["InvoiceAmount"]);
                    PointsToRedeem = Convert.ToString(item["RedeemPoints"]);
                    TxnType = Convert.ToString(item["TxnType"]);

                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Redeem Point";
                    objAudit.RequestedEntity = "Mobile No - " + MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);

                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.AddRedeemPointsData(GroupId, MobileNo, OutletId, Convert.ToDateTime(TransactionDate), DateTime.Now, InvoiceNumber, InvoiceAmount, Convert.ToDecimal(PointsToRedeem), Convert.ToString(IsSMS), TxnType, objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "Points Redeem for mobile no  - " + MobileNo;
                    var body = "Points Redeem for mobile no - " + MobileNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }

                if (IsSMS)
                {
                    //Logic to send SMS to Customer whose Name is changed
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadBonusData(string jsonData)
        {
            SPResponse result = new SPResponse();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;
                string GroupId = "";
                string MobileNo = "";
                int BonusPoints = 0;
                string BonusRemark = "";
                string OutletId = "";
                DateTime ExpiryDate = DateTime.Now;

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    OutletId = Convert.ToString(item["OutletId"]);
                    BonusPoints = Convert.ToInt32(item["BonusPoints"]);
                    BonusRemark = Convert.ToString(item["BonusRemark"]);
                    ExpiryDate = Convert.ToDateTime(item["ExpiryDate"]);

                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Load Bonus";
                    objAudit.RequestedEntity = "Load Bonus for - " + MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedDate"]);

                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.AddLoadBonusData(GroupId, MobileNo, OutletId, BonusPoints, BonusRemark, ExpiryDate, Convert.ToString(IsSMS), objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "Points Loaded for mobile no  - " + MobileNo;
                    var body = "Points Loaded for mobile no - " + MobileNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }

                if (Convert.ToBoolean(IsSMS))
                {
                    //Logic to send SMS to Customer whose Name is changed
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSingleMember(string jsonData)
        {
            SPResponse result = new SPResponse();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                CustomerDetail objCustomer = new CustomerDetail();
                string GroupId = "";

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    objCustomer.MobileNo = Convert.ToString(item["MobileNo"]);
                    objCustomer.CardNumber = Convert.ToString(item["CardNo"]);
                    objCustomer.CustomerName = Convert.ToString(item["FullName"]);
                    objCustomer.Gender = Convert.ToString(item["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["BirthDay"])))
                    {
                        objCustomer.DOB = Convert.ToDateTime(item["BirthDay"]);
                    }
                    objCustomer.CustomerThrough = Convert.ToString(item["Source"]);
                    objCustomer.EnrollingOutlet = Convert.ToString(item["OutletId"]);
                    objCustomer.MemberGroupId = "1000";
                    objCustomer.DOJ = DateTime.Now;
                    objCustomer.Status = "00";

                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "User Added";
                    objAudit.RequestedEntity = "User Added - " + objCustomer.MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedDate"]);

                }

                result = ITOPS.AddSingleCustomerData(GroupId, objCustomer, objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "New Customer Added with Mobile No  - " + objCustomer.MobileNo;
                    var body = "New Customer Added with Mobile No - " + objCustomer.MobileNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }

                //if (Convert.ToBoolean(IsSMS))
                //{
                //    //Logic to send SMS to Customer whose Name is changed
                //}

            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool ChangeSMSDetails(string jsonData)
        {
            bool result = false;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;
                string GroupId = "";
                string CustomerId = "";
                bool DisableSMS = false;
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    string Disable = Convert.ToString(item["Disable"]);
                    if (Disable == "1")
                        DisableSMS = true;

                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Change SMS setting";
                    objAudit.RequestedEntity = "Change SMS setting for - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedDate"]);

                    IsSMS = Convert.ToBoolean(item["IsSMS"]);

                }

                result = ITOPS.ChangeSMSDetails(GroupId, CustomerId, DisableSMS, objAudit);
                if (result)
                {
                    var subject = "New Customer Added with Mobile No  - " + CustomerId;
                    var body = "New Customer Added with Mobile No - " + CustomerId;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }
                if (Convert.ToBoolean(IsSMS))
                {
                    //Logic to send SMS to Customer whose Name is changed
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return result;
        }

        public ActionResult GetOTPData(string GroupId, string MobileNo)
        {
            MemberData objCustomerDetail = new MemberData();
            if (!string.IsNullOrEmpty(MobileNo))
            {
                objCustomerDetail = ITOPS.GetOTPData(GroupId, MobileNo);
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCancelTxnData(string GroupId, string MobileNo, string InvoiceNo)
        {
            MemberData objCustomerDetail = new MemberData();
            CancelTxnViewModel objData = new CancelTxnViewModel();
            if (!string.IsNullOrEmpty(InvoiceNo))
            {
                objData.objCancelTxnModel = ITOPS.GetTransactionByInvoiceNo(GroupId, InvoiceNo);
                objData.objCustomerDetail = ITOPS.GetCustomerByMobileNo(GroupId, objData.objCancelTxnModel.MobileNo);
            }
            else if (!string.IsNullOrEmpty(MobileNo))
            {
                objData.objCustomerDetail = ITOPS.GetCustomerByMobileNo(GroupId, MobileNo);
                objData.lstCancelTxnModel = ITOPS.GetTransactionByMobileNo(GroupId, MobileNo);
            }

            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public bool DeleteTransaction(string GroupId, string InvoiceNo,string RequestedBy, string RequestedForum, string RequestedDate)
        {
            bool result = false;
            try
            {
                tblAudit objAudit = new tblAudit();
                objAudit.GroupId = GroupId;
                objAudit.RequestedFor = "Delete Transaction";
                objAudit.RequestedEntity = "Delete Transaction for Invoice - " + InvoiceNo;
                objAudit.RequestedBy = RequestedBy;
                objAudit.RequestedOnForum = RequestedForum;
                objAudit.RequestedOn = Convert.ToDateTime(RequestedDate);
                bool IsSMS = false;

                result = ITOPS.DeleteTransaction(GroupId, InvoiceNo, objAudit);
                if (result)
                {
                    var subject = "Transaction Deleted for  - " + InvoiceNo;
                    var body = "Transaction Deleted for - " + InvoiceNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }
                if (Convert.ToBoolean(IsSMS))
                {
                    //Logic to send SMS to Customer whose Name is changed
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
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

    }
}