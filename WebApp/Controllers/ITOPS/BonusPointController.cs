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
using System.Data;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using BOTS_BL;
using BOTS_BL.Models;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class BonusPointController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        string groupId;
        // GET: BonusPoint
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

        public ActionResult GetChangeNameData(string MobileNo, string CardNo)
        {
            var GroupId = (string)Session["GroupId"];
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
        public ActionResult LoadBonusData(string jsonData)
        {
            SPResponse result = new SPResponse();
            var GroupId = (string)Session["GroupId"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

                string MobileNo = "";
                int BonusPoints = 0;
                string BonusRemark = "";
                string OutletId = "";
                DateTime ExpiryDate = DateTime.Now;

                foreach (Dictionary<string, object> item in objData)
                {

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
                newexception.AddException(ex, groupId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult CancelTransaction()
        {
            try
            {
                groupId = Session["GroupId"].ToString();
                //CommonFunctions common = new CommonFunctions();
                //groupId = common.DecryptString(groupId);
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
            MemberData objCustomerDetail = new MemberData();
            CancelTxnViewModel objData = new CancelTxnViewModel();
            if (!string.IsNullOrEmpty(InvoiceNo) && !string.IsNullOrEmpty(MobileNo))
            {

                objData.objCancelTxnModel = ITOPS.GetTransactionByInvoiceNoAndMobileNo(GroupId, MobileNo, InvoiceNo);
                objData.objCustomerDetail = ITOPS.GetCustomerByMobileNo(GroupId, objData.objCancelTxnModel.MobileNo);
            }
            else if (!string.IsNullOrEmpty(InvoiceNo))
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

        public ActionResult DeleteTransaction(string GroupId, string InvoiceNo, string MobileNo, string InvoiceAmt, string ip_Date, string RequestedBy, string RequestedForum, string RequestedDate)
        {
            SPResponse result = new SPResponse();
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
                var dateCancel = Convert.ToDateTime(ip_Date);
                result = ITOPS.DeleteTransaction(GroupId, InvoiceNo, MobileNo, InvoiceAmt, dateCancel, objAudit);
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
    }
}