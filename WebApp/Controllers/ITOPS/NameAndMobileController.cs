﻿using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class NameAndMobileController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        ITOPSNEWRepository NewITOPS = new ITOPSNEWRepository();
        // GET: NameAndMobile
        public ActionResult Index(string groupId)
        { 
        try
            {
                if (!string.IsNullOrEmpty(groupId))
                {
                    CommonFunctions common = new CommonFunctions();
                    groupId = common.DecryptString(groupId);
                    Session["GroupId"] = groupId;
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    userDetails.GroupId = groupId;
                    userDetails.connectionString = objCustRepo.GetCustomerConnString(groupId);
                    userDetails.CustomerName = objCustRepo.GetCustomerName(groupId);
                    Session["UserSession"] = userDetails;
                    Session["buttons"] = "ITOPS";
                    ViewBag.GroupId = groupId;
                }
                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Index");
            }
            return View();
        }

        public ActionResult ChangeMobile()
        {
            var groupId = (string)Session["GroupId"];
            try
            {
                ViewBag.GroupId = groupId;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ChangeMobile");
            }
            return View();
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

        [HttpPost]
        public bool ChangeMemberName(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            bool result = false;
            string GroupId = "";
            try
            {
                GroupId = (string)Session["GroupId"];
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

                string CustomerId = "";
                string Name = "";               
               
                foreach (Dictionary<string, object> item in objData)
                {                    
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    Name = Convert.ToString(item["Name"]);                 
                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Name Change";
                    objAudit.RequestedEntity = "CustomerId - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;
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
                newexception.AddException(ex, "ChangeMemberName");
            }
            return result;
        }

        [HttpPost]
        public ActionResult ChangeMemberMobile(string jsonData)
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
                
                foreach (Dictionary<string, object> item in objData)
                {
                    var GroupId = (string)Session["GroupId"];
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);                    

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

                result = ITOPS.UpdateMobileOfMember(groupId, CustomerId, MobileNo, objAudit);
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
                newexception.AddException(ex, "ChangeMemberMobile");
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

        #region ITOPNew
        public ActionResult IndexNew(string groupId)
        {
            try
            {
                if (!string.IsNullOrEmpty(groupId))
                {
                    CommonFunctions common = new CommonFunctions();
                    groupId = common.DecryptString(groupId);
                    Session["GroupId"] = groupId;
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    userDetails.GroupId = groupId;
                    userDetails.connectionString = NewITOPS.GetCustomerConnString(groupId);
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

        public ActionResult ChangeMobileNew()
        {
            var groupId = (string)Session["GroupId"];
            try
            {
                ViewBag.GroupId = groupId;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ChangeMobileNew");
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
                    objCustomerDetail = NewITOPS.GetChangeNameByMobileNo(groupId, MobileNo);
                }
                if (!string.IsNullOrEmpty(CardNo))
                {
                    objCustomerDetail = NewITOPS.GetChangeNameByCardNo(groupId, CardNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameData");
            }
            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool ChangeMemberNameNew(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            bool result = false;
            string GroupId = "";
            try
            {
                GroupId = (string)Session["GroupId"];
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

                string CustomerId = string.Empty;
                string Name = string.Empty;
                string MobileNo = string.Empty;

                foreach (Dictionary<string, object> item in objData)
                {
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    MobileNo = CustomerId.Substring(4);
                    Name = Convert.ToString(item["Name"]);
                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Name Change";
                    objAudit.RequestedEntity = "CustomerId - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);

                }

                result = NewITOPS.UpdateNameOfMember(GroupId, CustomerId, Name, objAudit);
                if (result)
                {
                    var subject = "Customer name changed for MobileNo - " + MobileNo;
                    var body = "Customer name changed for MobileNo - " + MobileNo;
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
                newexception.AddException(ex, "ChangeMemberName");
            }
            return result;
        }
        public ActionResult ChangeMemberMobileNew(string jsonData)
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

                string CustomerId = string.Empty;
                string MobileNo = string.Empty;

                foreach (Dictionary<string, object> item in objData)
                {
                    var GroupId = (string)Session["GroupId"];
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);

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

                result = NewITOPS.UpdateMobileOfMember(groupId, CustomerId, MobileNo, objAudit);
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
                newexception.AddException(ex, "ChangeMemberMobile");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}