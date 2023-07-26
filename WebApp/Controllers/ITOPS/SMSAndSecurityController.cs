using BOTS_BL;
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
    public class SMSAndSecurityController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        ITOPSNEWRepository NewITOPS = new ITOPSNEWRepository();
        ITCSRepository ITCSR = new ITCSRepository();

        Exceptions newexception = new Exceptions();
        // GET: SMSAndSecurity
        public ActionResult Index()
        {
            var groupId = Convert.ToString(Session["GroupId"]);
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
        public ActionResult SecurityKey()
        {
            var groupId = Convert.ToString(Session["GroupId"]);
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
                newexception.AddException(ex, "SecurityKey");
            }

            return View();
        }
        public ActionResult GetChangeNameData(string MobileNo, string CardNo)
        {
            var GroupId = (string)Session["GroupId"];
            MemberData objCustomerDetail = new MemberData();
            try 
            {
                
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = ITOPS.GetChangeNameByMobileNo(GroupId, MobileNo);
                }
                if (!string.IsNullOrEmpty(CardNo))
                {
                    objCustomerDetail = ITOPS.GetChangeNameByCardNo(GroupId, CardNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameData");
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        public bool ChangeSMSDetails(string jsonData)
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
                bool DisableSMS = false;
                

                foreach (Dictionary<string, object> item in objData)
                {
                    
                    CustomerId = Convert.ToString(item["CustomerId"]);                   
                    string Disable = Convert.ToString(item["Disable"]);                   
                    if (Disable == "1")
                        DisableSMS = true;
                    
                    objAudit.RequestedFor = "Change SMS setting";
                    objAudit.RequestedEntity = "Change SMS setting for - " + CustomerId;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedDate"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;

                    IsSMS = Convert.ToBoolean(item["IsSMS"]);

                }

                result = ITOPS.ChangeSMSDetails(GroupId,CustomerId, DisableSMS, objAudit);
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
                newexception.AddException(ex, "ChangeSMSDetails");
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
        public ActionResult GetLoginIdByOutlets(int outletId)
        {
            var GroupId = (string)Session["GroupId"];
            SPResponse result = new SPResponse();
            ResetSecurityKey objreset = new ResetSecurityKey();
            try
            {
                objreset.lstloginid = ITOPS.GetLoginIdByOutlet(GroupId, outletId);
            }
            catch (Exception ex)
            {

                newexception.AddException(ex, "GetLoginIdByOutlets");
            }

            return Json(objreset, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetOutletByBrandId(string BrandId)
        {
            var GroupId = (string)Session["GroupId"];
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            SPResponse result = new SPResponse();
            var lstoutletlist = RR.GetOutletListByBrandId(BrandId, connStr);
            ViewBag.OutletListByBrand = lstoutletlist;
            return Json(lstoutletlist, JsonRequestBehavior.AllowGet);
        }
        public bool UpdateSecurityKey(string CounterId)
        {
            var GroupId = (string)Session["GroupId"];
            bool result = false;
            try
            {
                result = ITOPS.UpdateSecurityKey(GroupId, CounterId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateSecurityKey");
            }
            return result;
        }

        ///////////// ITOPS New //////////

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
                newexception.AddException(ex, "GetChangeNameDataNew");
            }
            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        public bool ChangeSMSDetailsNew(string jsonData)
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

                string MobileNo = string.Empty;
                bool DisableSMS = false;


                foreach (Dictionary<string, object> item in objData)
                {

                    MobileNo = Convert.ToString(item["CustomerId"]);
                    string Disable = Convert.ToString(item["Disable"]);
                    if (Disable == "1")
                        DisableSMS = true;

                    objAudit.RequestedFor = "Change SMS setting";
                    objAudit.RequestedEntity = "Change SMS setting for - " + MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedDate"]);
                    objAudit.AddedBy = userDetails.LoginId;
                    objAudit.AddedDate = DateTime.Now;

                    IsSMS = Convert.ToBoolean(item["IsSMS"]);

                }

                result = NewITOPS.ChangeSMSDetails(GroupId, MobileNo, DisableSMS, objAudit);
                if (result)
                {
                    var subject = "New Customer Added with Mobile No  - " + MobileNo;
                    var body = "New Customer Added with Mobile No - " + MobileNo;
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
                newexception.AddException(ex, "ChangeSMSDetails");
            }
            return result;
        }
        public ActionResult SecurityKeyNew()
        {
            var groupId = Convert.ToString(Session["GroupId"]);
            try
            {

                string connStr = objCustRepo.GetCustomerConnString(groupId);
                var lstOutlet = NewITOPS.GetOutlet(groupId);
                var lstBrand = NewITOPS.GetBrandList(groupId);
                var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
                ViewBag.OutletList = lstOutlet;
                ViewBag.BranchList = lstBrand;
                ViewBag.GroupId = groupId;
                ViewBag.GroupName = GroupDetails.RetailName;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SecurityKeyNew");
            }

            return View();
        }
        public ActionResult GetLoginIdByOutletsNew(string outletId)
        {
            var GroupId = (string)Session["GroupId"];
            SPResponse result = new SPResponse();
            ResetSecurityKey objreset = new ResetSecurityKey();
            try
            {
                objreset.lstloginid = NewITOPS.GetLoginIdByOutlet(GroupId, outletId);
            }
            catch (Exception ex)
            {

                newexception.AddException(ex, "GetLoginIdByOutletsNew");
            }

            return Json(objreset, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOutletNew()
        {
            var GroupId = (string)Session["GroupId"];
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            SPResponse result = new SPResponse();
            var lstoutletlist =NewITOPS.GetOutlet(GroupId);
            ViewBag.OutletListByBrand = lstoutletlist;
            return Json(lstoutletlist, JsonRequestBehavior.AllowGet);
        }
        public bool UpdateSecurityKeyNew(string CounterId)
        {
            var GroupId = (string)Session["GroupId"];
            bool result = false;
            try
            {
                result = NewITOPS.UpdateSecurityKey(GroupId, CounterId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateSecurityKeyNew");
            }
            return result;
        }
    }
}