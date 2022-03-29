using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;
using System.Net.Mail;
using System.Net;
using WebApp.ViewModel;
using System.Data;
using System.Text.RegularExpressions;
using ClosedXML.Excel;

namespace WebApp.Controllers.ITOPS
{

    public class EarnBurnController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: EarnBurn
        
        public ActionResult Index()
        {
            string groupId = Convert.ToString(Session["GroupId"]);
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

        [HttpPost]
        public ActionResult GetData(string GroupId, string MobileNo, string CardNo)
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

        public ActionResult Burn()
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
        public ActionResult RedeemPointsData(string jsonData)
        {
            SPResponse result = new SPResponse();
            string GroupId = "";
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

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

        [HttpPost]
        public ActionResult AddEarnData(string jsonData)
        {
            SPResponse result = new SPResponse();
            string GroupId = "";
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

                string MobileNo = "";
                string TransactionDate = "";
                string InvoiceNumber = "";
                string InvoiceAmount = "";
                string OutletId = "";
                decimal points = 0;

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    OutletId = Convert.ToString(item["OutletId"]);
                    TransactionDate = Convert.ToString(item["TransactionDate"]);
                    InvoiceNumber = Convert.ToString(item["InvoiceNumber"]);
                    InvoiceAmount = Convert.ToString(item["InvoiceAmount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Points"])))
                    {
                        points = Convert.ToDecimal(item["Points"]);
                    }
                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Add / Earn";
                    objAudit.RequestedEntity = "Mobile No - " + MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.AddEarnData(GroupId, MobileNo, OutletId, Convert.ToDateTime(TransactionDate), DateTime.Now, InvoiceNumber, InvoiceAmount, Convert.ToString(IsSMS), points, objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "Earning updated for mobile no  - " + MobileNo;
                    var body = "Earning updated for mobile no - " + MobileNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    //SendEmail(GroupId, subject, body);
                }

                if (IsSMS)
                {
                    //Logic to send SMS to Customer whose Name is changed
                }
            }
            catch (Exception ex)
            {

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }        
}
