using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
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
            return View();
        }
        public ActionResult ChangeWAScript()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);
        }
        public ActionResult GetWAScripts(int GroupId, string GroupName, string MessageType)
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objWhatsAppSMSMaster = ITCSR.GetWAScripts(GroupId, GroupName, MessageType);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveScripts(int GroupId, string Script, string MessageType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            tblGroupDetail objtblGroupDetail = new tblGroupDetail();
            WhatsAppSMSMaster objWhatsAppSMSMaster = new WhatsAppSMSMaster();
            try
            {
                result = ITCSR.SaveScripts(GroupId, Script, MessageType);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult DisableSMS()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);

        }
        public ActionResult GetChangeNameData(string MobileNo, string GroupId)
        {
            MemberData objCustomerDetail = new MemberData();
            var groupId = (string)Session["GroupId"];
            try
            {
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = ITCSR.GetChangeNameByMobileNo(GroupId, MobileNo);
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
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    DisableSMSWAPromo = Convert.ToBoolean(item["DisableSMSWAPromo"]);

                }

                result = ITCSR.DisablePromotionalSMS(GroupId, MobileNo, DisableSMSWAPromo);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisablePromotionalSMS");
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DisableTransactions()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);
        }
        public ActionResult BlockTransaction(string jsonData)
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
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    DisableSMSWATxn = Convert.ToBoolean(item["DisableSMSWATxn"]);
                }

                result = ITCSR.BlockTransaction(GroupId, MobileNo, DisableSMSWATxn);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BlockTransaction");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeBurnRule()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);
        }
        public ActionResult GetBurnRule(string GroupId)
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objBurnData = ITCSR.GetBurnRule(GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBurnRule(string jsonData)
        {
            tblRuleMaster objRuleMaster = new tblRuleMaster();
            
            bool status = false;
            //var connectionString = CR.GetCustomerConnString(jsonData);
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string GroupId = userDetails.GroupId;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objRuleMaster.GroupId = Convert.ToString(item["GroupId"]);
                    objRuleMaster.BurnMinTxnAmt = Convert.ToDecimal(item["BurnMinTxnAmt"]);
                    objRuleMaster.MinRedemptionPts = Convert.ToDecimal(item["MinRedemptionPts"]);
                    objRuleMaster.MinRedemptionPtsFirstTime = Convert.ToDecimal(item["MinRedemptionPtsFirstTime"]);
                    objRuleMaster.BurnInvoiceAmtPercentage = Convert.ToDecimal(item["BurnInvoiceAmtPercentage"]);
                    objRuleMaster.BurnDBPointsPercentage = Convert.ToDecimal(item["BurnDBPointsPercentage"]);
                   
                }
                
                var connectionString = CR.GetCustomerConnString(objRuleMaster.GroupId);
                var Response = ITCSR.SaveBurnRule(objRuleMaster, connectionString);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDemographicDetails");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ChangeEarnRule()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);

        }
        public ActionResult GetEarnRule(string GroupId)
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objEarndata = ITCSR.GetEarnRule(GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveEarnRule(string jsonData)
        {
            tblRuleMaster objtblRuleMaster = new tblRuleMaster();
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string GroupId = userDetails.GroupId;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objtblRuleMaster.GroupId = Convert.ToString(item["GroupId"]);
                    objtblRuleMaster.EarnMinTxnAmt = Convert.ToDecimal(item["EarnMinTxnAmt"]);
                    objtblRuleMaster.PointsExpiryMonths = Convert.ToInt32(item["PointsExpiryMonths"]);
                    objtblRuleMaster.PointsPercentage = Convert.ToDecimal(item["PointsPercentage"]);
                    objtblRuleMaster.PointsAllocation = Convert.ToDecimal(item["PointsAllocation"]);
                }
                var connectionString = CR.GetCustomerConnString(Convert.ToString(objtblRuleMaster.GroupId));
                var Response = ITCSR.SaveEarnRule(objtblRuleMaster, connectionString);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEarnRule");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExtendPointsExpiry()
        {
            return View();
        }
    }
}