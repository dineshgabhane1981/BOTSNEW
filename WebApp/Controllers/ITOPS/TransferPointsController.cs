﻿using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class TransferPointsController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: TransferPoints
        public ActionResult Index(string groupId)
        {
            if (!string.IsNullOrEmpty(groupId))
            {
                CommonFunctions common = new CommonFunctions();
                groupId = common.DecryptString(groupId);
                Session["GroupId"] = groupId;
            }
            return View();
           
        }

        public ActionResult GetChangeNameData(string MobileNo, string CardNo)
        {
            MemberData objCustomerDetail = new MemberData();
            try
            {
                var groupId = (string)Session["GroupId"];
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

        [HttpPost]
        public ActionResult ChangeExistingMemberMobile(string jsonData)
        {
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
                string OldMobileNo = "";

                foreach (Dictionary<string, object> item in objData)
                {
                    var GroupId = (string)Session["GroupId"];
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    OldMobileNo = Convert.ToString(item["OldMobileNo"]);


                    objAudit.GroupId = groupId;
                    objAudit.RequestedFor = "Mobile Number Change";
                    objAudit.RequestedEntity = "CustomerId - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);

                }

                result = ITOPS.UpdateMobileOfExistingMember(groupId, CustomerId, MobileNo, objAudit);
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
                newexception.AddException(ex, "ChangeExistingMemberMobile");
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