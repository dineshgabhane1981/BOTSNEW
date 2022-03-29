using System;
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


namespace WebApp.Controllers.ITOPS
{
    public class TransactionController : Controller
    {
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
                newexception.AddException(ex, groupId);
            }
            return View();
        }
        public ActionResult GetCancelTxnData(string GroupId, string MobileNo, string InvoiceNo)
        {
            var groupId = (string)Session["GroupId"];
            MemberData objCustomerDetail = new MemberData();
            CancelTxnViewModel objData = new CancelTxnViewModel();
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
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModifyTxnData(string GroupId, string TransactionId)
        {
            //string GroupId = "";
            var groupId = (string)Session["GroupId"];
            CancelTxnViewModel objData = new CancelTxnViewModel();
            objData.objCancelTxnModel = ITOPS.GetTransactionByTransactionId(groupId, TransactionId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTransaction(string GroupId, string InvoiceNo, string MobileNo, string InvoiceAmt, string ip_Date, string RequestedBy, string RequestedForum, string RequestedDate)
        {
            SPResponse result = new SPResponse();
            //string GroupId = "";
            try
            {
                var groupId = (string)Session["GroupId"];
                tblAudit objAudit = new tblAudit();
                objAudit.GroupId = groupId;
                objAudit.RequestedFor = "Delete Transaction";
                objAudit.RequestedEntity = "Delete Transaction for Invoice - " + InvoiceNo;
                objAudit.RequestedBy = RequestedBy;
                objAudit.RequestedOnForum = RequestedForum;
                objAudit.RequestedOn = Convert.ToDateTime(RequestedDate);
                bool IsSMS = false;
                var dateCancel = Convert.ToDateTime(ip_Date);
                result = ITOPS.DeleteTransaction(groupId, InvoiceNo, MobileNo, InvoiceAmt, dateCancel, objAudit);
                if (result.ResponseCode == "00")
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
                newexception.AddException(ex, GroupId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
        public bool ModifyTransaction(string jsonData)
        {
            bool result = false;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                string GroupId = "";
                string TransactionId = "";
                decimal points = 0;

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    TransactionId = Convert.ToString(item["TransactionId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Points"])))
                    {
                        points = Convert.ToDecimal(item["Points"]);
                    }
                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Add / Earn";
                    objAudit.RequestedEntity = "Transaction For  - " + TransactionId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);

                }
                result = ITOPS.ModifyTransaction(GroupId, Convert.ToInt64(TransactionId), points, objAudit);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ModifyTransaction");
            }
            return result;
        }
        public ActionResult GetLogDetailData(string GroupId, string search)
        {
            List<LogDetailsRW> lstLogDetails = new List<LogDetailsRW>();
            lstLogDetails = ITOPS.GetLogDetails(search, GroupId);
            return Json(lstLogDetails, JsonRequestBehavior.AllowGet);
        }


    }
}