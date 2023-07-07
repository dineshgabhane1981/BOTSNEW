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
            try
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);

                }

                result = ITCSR.DisablePromotionalSMS(GroupId, MobileNo);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisablePromotionalSMS");
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}