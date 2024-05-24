using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.ViewModel;

namespace WebApp.Controllers.ITCS
{
    public class IndividualDBConfigController : Controller
    {
        ITCSRepository ITCSR = new ITCSRepository();
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: IndividualDBConfig
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var GroupId = userDetails.GroupId;
            return View();
        }
        public ActionResult ChangeScript()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            return View(objData);
        }
        public JsonResult GetScript(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstScripts = ITCSR.GetScript(userDetails.GroupId, OutletId);
            return new JsonResult() { Data = lstScripts, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetTransactionalScripts(string OutletId, string MessageType)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objSMSWhatsAppScriptMaster = ITCSR.GetTransactionalScripts(Convert.ToInt32(userDetails.GroupId), OutletId, MessageType);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveWATransactionalScripts(int OutletId, string Script, string MessageType, string ScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveWATransactionalScripts(Convert.ToInt32(userDetails.GroupId), Convert.ToString(OutletId), Script, MessageType, ScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change WA Transactional Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SaveSMSTransactionalScripts(int OutletId, string Script, string MessageType,string ScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveSMSTransactionalScripts(Convert.ToInt32(userDetails.GroupId), Convert.ToString(OutletId), Script, MessageType, ScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change SMS Transactional Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetTransactionalSMSWASendStatus(string OutletId, string MessageType)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objSMSWhatsAppScriptMaster = ITCSR.GetTransactionalSMSWASendStatus(Convert.ToInt32(userDetails.GroupId), OutletId, MessageType);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSMSWASendStatus(int OutletId, string MessageType,string SendStatus)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveSMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Convert.ToString(OutletId),MessageType, SendStatus);
                //tblAuditC objData = new tblAuditC();
                //objData.GroupId = Convert.ToString(userDetails.GroupId);
                //objData.RequestedFor = "Change SMS Transactional Script";
                //objData.RequestedBy = userDetails.UserName;
                //objData.RequestedDate = DateTime.Now;
                //ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetBirthdayScript()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstBirthdayScripts = ITCSR.GetBirthdayScript(userDetails.GroupId);
            return new JsonResult() { Data = lstBirthdayScripts, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetBirthdaySMSWAScript(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objBirthdaySMSWAScript = ITCSR.GetBirthdaySMSWAScript(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveWABirthdayScripts(string Script1, string Name1,string BirthdayWAScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveWABirthdayScripts(Convert.ToInt32(userDetails.GroupId), Script1, Name1, BirthdayWAScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change Birthday WA Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SaveSMSBirthdayScripts(string Script1, string Name1,string BirthdaySMSScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveSMSBirthdayScripts(Convert.ToInt32(userDetails.GroupId), Script1, Name1, BirthdaySMSScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change Birthday SMS Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetBirthdaySMSWASendStatus(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objBirthdaySMSWAScript = ITCSR.GetBirthdaySMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveBirthdaySMSWASendStatus(string Name1,string SendStatus)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveBirthdaySMSWASendStatus(Convert.ToInt32(userDetails.GroupId),Name1,SendStatus);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change SMS Transactional Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetAnniversaryScript()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstAnniversaryScripts = ITCSR.GetAnniversaryScript(userDetails.GroupId);
            return new JsonResult() { Data = lstAnniversaryScripts, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetAnniversarySMSWAScript(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objAnniversarySMSWAScript = ITCSR.GetAnniversarySMSWAScript(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveWAAnniversaryScripts(string Script2, string Name2,string AnniversaryWAScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveWAAnniversaryScripts(Convert.ToInt32(userDetails.GroupId), Script2, Name2, AnniversaryWAScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change Anniversary WA Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SaveSMSAnniversaryScripts(string Script2, string Name2,string AnniversarySMSScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveSMSAnniversaryScripts(Convert.ToInt32(userDetails.GroupId), Script2, Name2, AnniversarySMSScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change Anniversary SMS Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetAnniversarySMSWASendStatus(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objAnniversarySMSWAScript = ITCSR.GetAnniversarySMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAnniversarySMSWASendStatus(string Name2, string SendStatus)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveAnniversarySMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Name2, SendStatus);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change SMS Transactional Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetInactiveScript()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstInactiveScripts = ITCSR.GetInactiveScript(userDetails.GroupId);
            return new JsonResult() { Data = lstInactiveScripts, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetInactiveSMSWAScript(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objInActiveSMSWAScript = ITCSR.GetInactiveSMSWAScript(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveWAInactiveScripts(string Script3, string Name3,string InactiveWAScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveWAInactiveScripts(Convert.ToInt32(userDetails.GroupId), Script3, Name3, InactiveWAScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change Inactive WA Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWAInactiveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SaveSMSInactiveScripts(string Script3, string Name3,string InactiveSMSScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveSMSInactiveScripts(Convert.ToInt32(userDetails.GroupId), Script3, Name3, InactiveSMSScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change Inactive SMS Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSInactiveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetInactiveSMSWASendStatus(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objInActiveSMSWAScript = ITCSR.GetInactiveSMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveInactiveSMSWASendStatus(string Name3, string SendStatus)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveInactiveSMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Name3, SendStatus);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change SMS Transactional Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetPointsExpiryScript()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstPointsExpiryScripts = ITCSR.GetPointsExpiryScript(userDetails.GroupId);
            return new JsonResult() { Data = lstPointsExpiryScripts, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetPointsExpirySMSWAScripts(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objPointsExpirySMSWAScript = ITCSR.GetPointsExpirySMSWAScripts(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveWAPointsExpiryScripts(string Script4, string Name4,string PointsExpiryWAScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveWAPointsExpiryScripts(Convert.ToInt32(userDetails.GroupId), Script4, Name4, PointsExpiryWAScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Save WA Points Expiry Scripts";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SaveSMSPointsExpiryScripts(string Script4, string Name4,string PointsExpirySMSScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveSMSPointsExpiryScripts(Convert.ToInt32(userDetails.GroupId), Script4, Name4, PointsExpirySMSScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Save SMS Points Expiry Scripts";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetPointsExpirySMSWASendStatus(string Name)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objPointsExpirySMSWAScript = ITCSR.GetPointsExpirySMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Name);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SavePointsExpirySMSWASendStatus(string Name4, string SendStatus)

        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SavePointsExpirySMSWASendStatus(Convert.ToInt32(userDetails.GroupId), Name4, SendStatus);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change SMS Transactional Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetDLCScript()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstDLCNewScripts = ITCSR.GetDLCScript(userDetails.GroupId);
            return new JsonResult() { Data = lstDLCNewScripts, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetDLCSMSWAScripts(string DLCMessageType)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objDLCSMSWAScriptMaster = ITCSR.GetDLCSMSWAScripts(Convert.ToInt32(userDetails.GroupId), DLCMessageType);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveWADLCScripts(string DLCMessageType, string DLCScript,string DLCWAScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveWADLCScripts(Convert.ToInt32(userDetails.GroupId), DLCMessageType, DLCScript, DLCWAScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Save WA DLC Scripts";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCSMSScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SaveSMSDLCScripts(string DLCMessageType, string DLCScript,string DLCSMSScriptType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveSMSDLCScripts(Convert.ToInt32(userDetails.GroupId), DLCMessageType, DLCScript, DLCSMSScriptType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Save SMS DLC Scripts";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCSMSScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetDLCSMSWASendStatus(string DLCMessageType)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objDLCSMSWAScriptMaster = ITCSR.GetDLCSMSWASendStatus(Convert.ToInt32(userDetails.GroupId), DLCMessageType);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDLCSMSWASendStatus(string DLCMessageType, string SendStatus)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                result = ITCSR.SaveDLCSMSWASendStatus(Convert.ToInt32(userDetails.GroupId), DLCMessageType, SendStatus);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Save DLC SMS WA Send Status";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetOutletDetails()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutletDetails = ITCSR.GetOutletDetails(userDetails.GroupId);
            return new JsonResult() { Data = lstOutletDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetSMSCredentials(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objSMSCredential = ITCSR.GetSMSCredentials(OutletId,userDetails.GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSMSCredentials(string jsonData)
        {
            tblSMSWhatsAppCredential objtblSMSWhatsAppCredential = new tblSMSWhatsAppCredential();
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    objtblSMSWhatsAppCredential.OutletId = Convert.ToString(item["OutletId"]);
                    objtblSMSWhatsAppCredential.SMSVendor = Convert.ToString(item["SMSVendor"]);
                    objtblSMSWhatsAppCredential.SMSUrl = Convert.ToString(item["SMSUrl"]);
                    objtblSMSWhatsAppCredential.SMSLoginId = Convert.ToString(item["SMSLoginId"]);
                    objtblSMSWhatsAppCredential.SMSPassword = Convert.ToString(item["SMSPassword"]);
                    objtblSMSWhatsAppCredential.SMSAPIKey = Convert.ToString(item["SMSAPIKey"]);
                    objtblSMSWhatsAppCredential.SMSSenderId = Convert.ToString(item["SMSSenderId"]);
                    objtblSMSWhatsAppCredential.IsActiveSMS = Convert.ToBoolean(item["IsActiveSMS"]);
                }
                var Response = ITCSR.SaveSMSCredentials(objtblSMSWhatsAppCredential,userDetails.GroupId);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Save SMS Credentials";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEarnRule");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult DisableSMS()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);
        }
        public ActionResult GetChangeNameData(string MobileNo)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            MemberData objCustomerDetail = new MemberData();            
            try
            {
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = ITCSR.GetChangeNameByMobileNo(userDetails.GroupId, MobileNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameData");
            }
            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DisablePromotionalSMS(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var result = false;
            string GroupId, MobileNo;
            GroupId = string.Empty;
            MobileNo = string.Empty;
            bool DisableSMSWAPromo = default;
            try
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = userDetails.GroupId;
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    DisableSMSWAPromo = Convert.ToBoolean(item["DisableSMSWAPromo"]);

                }

                result = ITCSR.DisablePromotionalSMS(GroupId, MobileNo, DisableSMSWAPromo);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Disable WaPromo -" + MobileNo;
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisablePromotionalSMS");
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DisableTxnSMS()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            return View(objData);
        }
        public ActionResult DisableTransactionalSMS(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var result = false;
            string GroupId, MobileNo;
            GroupId = string.Empty;
            MobileNo = string.Empty;
            bool DisableSMSWATxn = default;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = userDetails.GroupId;
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    DisableSMSWATxn = Convert.ToBoolean(item["DisableSMSWATxn"]);
                }

                result = ITCSR.DisableTransactionalSMS(GroupId, MobileNo, DisableSMSWATxn);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Disable Txn -" + MobileNo;
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableTransactionalSMS");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DisableTransactions()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            return View(objData);
        }
        public ActionResult DisablePointsEarning(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var result = false;
            string GroupId, MobileNo;
            GroupId = string.Empty;
            MobileNo = string.Empty;
            bool DisableTxn = default;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = userDetails.GroupId;
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    DisableTxn = Convert.ToBoolean(item["DisableTxn"]);
                }

                result = ITCSR.DisableTransactions(GroupId, MobileNo, DisableTxn);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Disable Txn -" + MobileNo;
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisablePointsEarning");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeBurnRule()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);
        }
        public ActionResult GetBurnRule()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objBurnData = ITCSR.GetBurnRule(userDetails.GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveBurnRule(string jsonData)
        {
            tblRuleMaster objRuleMaster = new tblRuleMaster();

            bool status = false;            
            var userDetails = (CustomerLoginDetail)Session["UserSession"];            
            
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    objRuleMaster.GroupId = userDetails.GroupId;
                    objRuleMaster.BurnMinTxnAmt = Convert.ToDecimal(item["BurnMinTxnAmt"]);
                    objRuleMaster.MinRedemptionPts = Convert.ToDecimal(item["MinRedemptionPts"]);
                    objRuleMaster.MinRedemptionPtsFirstTime = Convert.ToDecimal(item["MinRedemptionPtsFirstTime"]);
                    objRuleMaster.BurnInvoiceAmtPercentage = Convert.ToDecimal(item["BurnInvoiceAmtPercentage"]);
                    objRuleMaster.BurnDBPointsPercentage = Convert.ToDecimal(item["BurnDBPointsPercentage"]);
                    objRuleMaster.OldBurnMinTxnAmt = Convert.ToDecimal(item["OldBurnMinTxnAmt"]);
                    objRuleMaster.OldMinRedemptionPts = Convert.ToDecimal(item["OldMinRedemptionPts"]);
                    objRuleMaster.OldMinRedemptionPtsFirstTime = Convert.ToDecimal(item["OldMinRedemptionPtsFirstTime"]);
                    objRuleMaster.OldBurnInvoiceAmtPercentage = Convert.ToDecimal(item["OldBurnInvoiceAmtPercentage"]);
                    objRuleMaster.OldBurnDBPointsPercentage = Convert.ToDecimal(item["OldBurnDBPointsPercentage"]);
                }

                var connectionString = CR.GetCustomerConnString(objRuleMaster.GroupId);
                var Response = ITCSR.SaveBurnRule(objRuleMaster, connectionString, userDetails.UserName);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Burn Rule";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBurnRule");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ChangeEarnRule()
        {
            Earndata obj = new Earndata();
            ProgrammeViewModel objData = new ProgrammeViewModel();
     
            return View(objData);

        }
        public ActionResult GetEarnRule()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objEarndata = ITCSR.GetEarnRule(userDetails.GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveEarnRule(string jsonData)
        {
            tblRuleMaster objtblRuleMaster = new tblRuleMaster();
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    objtblRuleMaster.GroupId = userDetails.GroupId;
                    objtblRuleMaster.RuleName = Convert.ToString(item["RuleName"]);
                    objtblRuleMaster.StartDate = Convert.ToDateTime(item["StartDate"]);
                    objtblRuleMaster.EndDate = Convert.ToDateTime(item["EndDate"]);
                    objtblRuleMaster.EarnMinTxnAmt = Convert.ToDecimal(item["EarnMinTxnAmt"]);
                    objtblRuleMaster.PointsExpiryMonths = Convert.ToInt32(item["PointsExpiryMonths"]);
                    objtblRuleMaster.PointsPercentage = Convert.ToDecimal(item["PointsPercentage"]);
                    objtblRuleMaster.PointsAllocation = Convert.ToDecimal(item["PointsAllocation"]);
                    objtblRuleMaster.Revolving = Convert.ToBoolean(item["Revolving"]);
                    objtblRuleMaster.OldEarnMinTxnAmt = Convert.ToDecimal(item["OldEarnMinTxnAmt"]);
                    objtblRuleMaster.OldPointsExpiryMonths = Convert.ToInt32(item["OldPointsExpiryMonths"]);
                    objtblRuleMaster.OldPointsAllocation = Convert.ToDecimal(item["OldPointsAllocation"]);
                    objtblRuleMaster.OldPointsPercentage = Convert.ToDecimal(item["OldPointsPercentage"]);
                    objtblRuleMaster.OldRevolvingStatus = Convert.ToBoolean(item["OldRevolvingStatus"]);

                    //
                    objtblRuleMaster.BurnMinTxnAmt = Convert.ToDecimal(item["BurnMinTxnAmt"]);
                    objtblRuleMaster.MinRedemptionPts = Convert.ToDecimal(item["MinRedemptionPts"]);
                    objtblRuleMaster.MinRedemptionPtsFirstTime = Convert.ToDecimal(item["MinRedemptionPtsFirstTime"]);
                    objtblRuleMaster.BurnInvoiceAmtPercentage = Convert.ToDecimal(item["BurnInvoiceAmtPercentage"]);
                    objtblRuleMaster.BurnDBPointsPercentage = Convert.ToDecimal(item["BurnDBPointsPercentage"]);
                    objtblRuleMaster.OldBurnMinTxnAmt = Convert.ToDecimal(item["OldBurnMinTxnAmt"]);
                    objtblRuleMaster.OldMinRedemptionPts = Convert.ToDecimal(item["OldMinRedemptionPts"]);
                    objtblRuleMaster.OldMinRedemptionPtsFirstTime = Convert.ToDecimal(item["OldMinRedemptionPtsFirstTime"]);
                    objtblRuleMaster.OldBurnInvoiceAmtPercentage = Convert.ToDecimal(item["OldBurnInvoiceAmtPercentage"]);
                    objtblRuleMaster.OldBurnDBPointsPercentage = Convert.ToDecimal(item["OldBurnDBPointsPercentage"]);

                }


                var connectionString = CR.GetCustomerConnString(Convert.ToString(objtblRuleMaster.GroupId));
                var Response = ITCSR.SaveEarnRule(objtblRuleMaster, connectionString, userDetails.UserName);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Earn Burn Rule";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEarnRule");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ExtendPointsExpiry()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();                       
            return View(objData);
        }
        public ActionResult GetPointExpiryDetails(string mobileNo)
        {
            PointExpiryDummyModel objPointsExpiry = new PointExpiryDummyModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objPointsExpiry = ITCSR.GetPointExpiryDetails(userDetails.GroupId, mobileNo);
            return new JsonResult() { Data = objPointsExpiry, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ChangeRedeemptionOTP()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);
        }
        public JsonResult GetOutlet()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutletDetails = ITCSR.GetOutlet(userDetails.GroupId);
            return new JsonResult() { Data = lstOutletDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetDefaultOTP(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objOTPData = ITCSR.GetDefaultOTP(OutletId, userDetails.GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDefaultOTP(string jsonData)
        {
            tblOutletMaster objOutletMaster = new tblOutletMaster();

            bool status = false;            
            var userDetails = (CustomerLoginDetail)Session["UserSession"];            
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objOutletMaster.GroupId = userDetails.GroupId;
                    objOutletMaster.OutletId = Convert.ToString(item["OutletId"]);
                    objOutletMaster.DefaultOTP = Convert.ToString(item["DefaultOTP"]);
                }
                var connectionString = CR.GetCustomerConnString(userDetails.GroupId);
                var Response = ITCSR.SaveDefaultOTP(objOutletMaster, connectionString);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Redeemption OTPm";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDefaultOTP");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }    
        public ActionResult UpdateExpiryPointsDate(string mobileNo,string expiryDate)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                status = ITCSR.UpdateExpiryPointsDate(mobileNo, userDetails.GroupId, expiryDate);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Update Expiry Points Date -" + mobileNo;
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateExpiryPointsDate");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetExpiryDataByDateRange(string fromDate,string toDate)
        {
            List<PointExpiryDummyModel> lstData = new List<PointExpiryDummyModel>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            lstData = ITCSR.GetPointExpiryDateRange(userDetails.GroupId, fromDate, toDate);
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult UpdateExpiryPointsRangeDate(string fromDate, string toDate, string updateDate)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                status = ITCSR.UpdateExpiryPointsRangeDate(userDetails.GroupId, fromDate, toDate, updateDate);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Update Expiry Points Range Date ";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateExpiryPointsRangeDate");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetCampaignList()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstData = ITCSR.GetCampaignList(userDetails.GroupId);
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetCampaignDetails(string campaignName)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var objDataNew = ITCSR.GetCamaignPointExpiryDetails(userDetails.GroupId, campaignName);
            return new JsonResult() { Data = objDataNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult UpdateCampaignDetails(string campaignName,string updateDate)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            status = ITCSR.UpdateCammpaignExpiryDate(userDetails.GroupId, campaignName, updateDate);
            tblAuditC obj = new tblAuditC();
            obj.GroupId = Convert.ToString(userDetails.GroupId);
            obj.RequestedFor = "Update Campaign Details -" + campaignName;
            obj.RequestedBy = userDetails.UserName;
            obj.RequestedDate = DateTime.Now;
            ITCSR.AddCSLog(obj);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SlabWiseReport()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            return View(objData);
        }
        public JsonResult GetTierList()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstTierDetails = ITCSR.GetTierList(userDetails.GroupId);
            return new JsonResult() { Data = lstTierDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetSlabWiseReport(string Tier)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstMemberData = ITCSR.GetSlabWiseReport(userDetails.GroupId, Tier);
            return PartialView("_Slabwise", objData);
        }
        public ActionResult ExportToExcelSlabMemberList(string Tier)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                List<MemberData> lstMember = new List<MemberData>();
                lstMember = ITCSR.GetSlabWiseReport(userDetails.GroupId, Tier);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(MemberData));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (MemberData item in lstMember)
                {
                    DataRow row = table.NewRow();                                  
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }

                table.Columns.Remove("OldMobileNo");
                table.Columns.Remove("CardNo");
                table.Columns.Remove("EnrolledOutletName");
                table.Columns.Remove("EnrolledOn");
                table.Columns.Remove("CustomerId");
                table.Columns.Remove("DisableSMSWAPromo");
                table.Columns.Remove("DisableSMSWATxn");                
                
                string ReportName = "MemberData";
                    string fileName = "BOTS_" + ReportName + ".xlsx";
                    using (XLWorkbook wb = new XLWorkbook())
                    {                       
                        table.TableName = ReportName;

                        IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                        worksheet.Cell(1, 1).Value = "Report Name";
                        worksheet.Cell(1, 2).Value = "Member Data";
                        worksheet.Cell(2, 1).Value = "Date";
                        worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                        worksheet.Cell(3, 1).Value = "Filter";

                        worksheet.Cell(5, 1).InsertTable(table);                        
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);

                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelSlabMemberList");
                return null;
            }
        }
        public ActionResult ChangeDemographicDetails()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();           
            return View(objData);
        }
        public ActionResult GetDemographicDetails(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objDemographicData = ITCSR.GetDemographicDetails(userDetails.GroupId, OutletId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDemographicDetails(string jsonData)
        {
            tblGroupOwnerInfo objGroupOwnerInfo = new tblGroupOwnerInfo();
            tblOutletMaster objOutletMaster = new tblOutletMaster();
            bool status = false;
            
            var userDetails = (CustomerLoginDetail)Session["UserSession"];           
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objGroupOwnerInfo.GroupId = userDetails.GroupId;                    
                    objGroupOwnerInfo.MobileNo = Convert.ToString(item["MobileNo"]);
                    objGroupOwnerInfo.AlternateNo = Convert.ToString(item["AlternateNo"]);
                    objGroupOwnerInfo.Email = Convert.ToString(item["Email"]);
                    objGroupOwnerInfo.Address = Convert.ToString(item["Address"]);
                    objGroupOwnerInfo.DOB = Convert.ToDateTime(item["DOB"]);
                    objGroupOwnerInfo.DOA = Convert.ToDateTime(item["DOA"]);
                    objGroupOwnerInfo.Gender = Convert.ToString(item["Gender"]);
                    objGroupOwnerInfo.Name = Convert.ToString(item["Name"]);

                    objOutletMaster.OutletId = Convert.ToString(item["OutletId"]);
                    objOutletMaster.StoreAnniversaryDate = Convert.ToDateTime(item["StoreAnniversary"]);
                }                
                var connectionString = CR.GetCustomerConnString(objGroupOwnerInfo.GroupId);
                var Response = ITCSR.SaveDemographicDetails(objGroupOwnerInfo, objOutletMaster, connectionString);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Demographic Details";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDemographicDetails");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ChangeOutletDetails()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            return View(objData);
        }
        public ActionResult SaveOutletDetails(string jsonData)
        {
            bool status = false;

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    tblOutletMaster objOutletMaster = new tblOutletMaster();
                    objOutletMaster.OutletId = Convert.ToString(item["Outlet_ID"]);
                    objOutletMaster.OutletName = Convert.ToString(item["Outlet_name"]);
                    objOutletMaster.BrandId = Convert.ToString(item["Brand_ID"]);
                    objOutletMaster.GroupId = Convert.ToString(item["Group_ID"]);
                    objOutletMaster.IsActive = true;
                    objOutletMaster.Address = Convert.ToString(item["_Address"]);
                    objOutletMaster.City = Convert.ToString(item["_City"]);
                    objOutletMaster.Area = Convert.ToString(item["_Area"]);
                    objOutletMaster.Phone = Convert.ToString(item["_Phone"]);
                    objOutletMaster.Pincode = Convert.ToString(item["_Pincode"]);
                    objOutletMaster.DefaultOTP = Convert.ToString(item["Default_otp"]);
                    objOutletMaster.Latitude = Convert.ToString(item["_Latitude"]);
                    objOutletMaster.Longitude = Convert.ToString(item["_Longitude"]);
                    objOutletMaster.InvoiceDate = Convert.ToDateTime(item["Invoice_date"]);
                    objOutletMaster.LiveDate = Convert.ToDateTime(item["Live_date"]);
                    objOutletMaster.StoreAnniversaryDate = Convert.ToDateTime(item["Store_adate"]);

                    var connectionString = CR.GetCustomerConnString(objOutletMaster.GroupId);
                    var response = ITCSR.SaveOutletDetails(objOutletMaster, connectionString);
                    status = response;
                }
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Outlet Details";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveOutletDetails");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetOutletDetailsDATA(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objOutletData = ITCSR.GetOutletDetails(userDetails.GroupId, OutletId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAssignValueForOutlet(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objOutletData = ITCSR.GetAssignOutletDetails(userDetails.GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOutletCrediantialDetails(string jsonData)
        {
            tblGroupOwnerInfo objGroupOwnerInfo = new tblGroupOwnerInfo();
            bool status = false;

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    tblSMSWhatsAppCredential objCrediantialMaster = new tblSMSWhatsAppCredential();
                    objGroupOwnerInfo.GroupId = userDetails.GroupId;
                    objCrediantialMaster.OutletId = item["Outlet_IDCR"]?.ToString();
                    objCrediantialMaster.SMSVendor = item["Sms_VendorCR"]?.ToString();
                    objCrediantialMaster.SMSUrl = item["SMSUrlCR"]?.ToString();
                    objCrediantialMaster.SMSLoginId = item["SMSLoginIdCR"]?.ToString();
                    objCrediantialMaster.WhatsAppMessageType = "Text";
                    objCrediantialMaster.SMSPassword = item["SMSPassCR"]?.ToString();
                    objCrediantialMaster.SMSAPIKey = item["ApiKeyCR"]?.ToString();
                    objCrediantialMaster.WhatsAppVendor = item["WhatsappVendorCR"]?.ToString();
                    objCrediantialMaster.WhatsAppUrl = item["WhatsappUrlCR"]?.ToString();
                    objCrediantialMaster.WhatsAppTokenId = item["WhatsappTIDCR"]?.ToString();
                    objCrediantialMaster.VerifiedWhatsAppUrl = item["VWAWhatsappUrlCR"]?.ToString();
                    objCrediantialMaster.VerifiedWhatsAppLoginId = item["VWALoginidDCR"]?.ToString();
                    objCrediantialMaster.VerifiedWhatsAppPassword = item["VWAPasswordCR"]?.ToString();
                    objCrediantialMaster.VerifiedWhatsAppAPIKey = item["VWAAPIKeyCR"]?.ToString();
                    objCrediantialMaster.SMSSenderId = item["VWASenderidCR"]?.ToString();
                    objCrediantialMaster.VerifiedWhatsAppVendor = item["VWAVendorCR"]?.ToString();

                    var connectionString = CR.GetCustomerConnString(objGroupOwnerInfo.GroupId);
                    var response = ITCSR.SaveOutletCrediantialDetails(objCrediantialMaster, connectionString);
                    status = response;
                }
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Outlet Details";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveOutletDetails");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SaveOutletStoreDetails(string jsonData)
        {
            tblGroupOwnerInfo objGroupOwnerInfo = new tblGroupOwnerInfo();
            bool status = false;

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {

                    tblStoreMaster objCrediantialMaster = new tblStoreMaster();
                    objGroupOwnerInfo.GroupId = userDetails.GroupId;
                    objCrediantialMaster.CounterId = Convert.ToString(item["Counter_IDST"]);
                    objCrediantialMaster.CounterType = Convert.ToString(item["Counter_TypeST"]);
                    objCrediantialMaster.Securitykey = Convert.ToString(item["Security_KeyST"]);
                    objCrediantialMaster.OutletId = Convert.ToString(item["Outlet_IDST"]);
                    objCrediantialMaster.IsActive = true;
                    objCrediantialMaster.CreatedDate = DateTime.Today;


                    var connectionString = CR.GetCustomerConnString(objGroupOwnerInfo.GroupId);
                    var response = ITCSR.SaveOutletStoreDetails(objCrediantialMaster, connectionString, objGroupOwnerInfo.GroupId,userDetails.UserName,userDetails.EmailId);
                    status = response;
                }
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Outlet Details";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveOutletDetails");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult UploadCelebrationData()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);
        }
        public bool UploadDocument()
        {
            bool status = false;
            DataSet ds = new DataSet();
            HttpPostedFileBase files = Request.Files[0];
            string fileName = Request.Files[0].FileName;
            var path = ConfigurationManager.AppSettings["CampaignFileUpload"].ToString();
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            var fullFilePath = path + "/" + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + fileName;
            files.SaveAs(fullFilePath);
            var conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;
                        //Get the name of First Sheet.
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(ds);
                        connExcel.Close();
                    }
                }
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                if (!string.IsNullOrEmpty(Convert.ToString(dr["MobileNo"])))
                {
                    status = ITCSR.UpdateCelebrationData(userDetails.GroupId, Convert.ToString(dr["MobileNo"]), Convert.ToString(dr["CustomerName"]), Convert.ToString(dr["DOB"]), Convert.ToString(dr["DOA"]));
                    tblAuditC obj = new tblAuditC();
                    obj.GroupId = Convert.ToString(userDetails.GroupId);
                    obj.RequestedFor = "Upload Celebration Data";
                    obj.RequestedBy = userDetails.UserName;
                    obj.RequestedDate = DateTime.Now;
                    ITCSR.AddCSLog(obj);
                }
            }
            return status;
        }

    }

}