using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using WebApp.App_Start;
using BOTS_BL.Models.IndividualDBModels;
using WebApp.ViewModel;

namespace WebApp.Controllers.ITOPS
{
    public class OTPAndLogController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        ITOPSNEWRepository NewITOPS = new ITOPSNEWRepository();

        // GET: EarnBurn
        string groupId;
        public ActionResult Index()
        {
            var groupId = (string)Session["GroupId"];
            try
            {
                ViewBag.GroupId = groupId;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Index");
            }
            return View();
        }

        public ActionResult Log()
        {
            try
            {
                CommonFunctions common = new CommonFunctions();
                //groupId = common.DecryptString(groupId);
                groupId = Session["GroupId"].ToString();
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                var lstOutlet = RR.GetOutletList(groupId, connStr);
                var lstBrand = RR.GetBrandList(groupId, connStr);
                var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
                ViewBag.OutletList = lstOutlet;
                ViewBag.BranchList = lstBrand;
                ViewBag.GroupId = groupId;

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Log");
            }

            return View();
        }

        public ActionResult GetOTPData(string MobileNo)
        {
            MemberData objCustomerDetail = new MemberData();
            try
            {
                var GroupId = (string)Session["GroupId"];
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = ITOPS.GetOTPData(GroupId, MobileNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOTPData");
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTxnLogData(string GroupId, string search)
        {
            List<LogDetailsRW> lstLogDetails = new List<LogDetailsRW>();
            ITOpsRepository ITOPS = new ITOpsRepository();           
            try
            {
                lstLogDetails = ITOPS.GetLogDetails(search, GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTxnLogData");
            }
            return Json(lstLogDetails, JsonRequestBehavior.AllowGet);

        }

        #region ITOPSNew

        public ActionResult IndexNew()
        {
            var groupId = (string)Session["GroupId"];
            try
            {
                ViewBag.GroupId = groupId;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Index");
            }
            return View();
        }

        public ActionResult LogNew()
        {
            try
            {
                CommonFunctions common = new CommonFunctions();
                //groupId = common.DecryptString(groupId);
                groupId = Session["GroupId"].ToString();
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                var lstOutlet = RR.GetOutletList(groupId, connStr);
                var lstBrand = RR.GetBrandList(groupId, connStr);
                var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
                ViewBag.OutletList = lstOutlet;
                ViewBag.BranchList = lstBrand;
                ViewBag.GroupId = groupId;

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Log");
            }

            return View();
        }

        public ActionResult GetOTPDataNew(string MobileNo)
        {
            MemberData objCustomerDetail = new MemberData();
            try
            {
                var GroupId = (string)Session["GroupId"];
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = NewITOPS.GetOTPData(GroupId, MobileNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOTPData");
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTxnLogDataNew(string GroupId, string search)
        {
            List<tblLogDetail> lstLogDetails = new List<tblLogDetail>();
            ITOpsRepository ITOPS = new ITOpsRepository();
            try
            {
                lstLogDetails = NewITOPS.GetLogDetails(search, GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTxnLogData");
            }
            return Json(lstLogDetails, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EnableDisableOTPNew()
        {

            ITOPSNewViewModel LstOTPDetails = new ITOPSNewViewModel();

            try
            {          
                groupId = Session["GroupId"].ToString();
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                LstOTPDetails.ObjLstOTPDetails =  NewITOPS.GetCommonOTPDetails(groupId);
                LstOTPDetails.GroupId = groupId;             
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableDisableOTPNew");
            }

            return View(LstOTPDetails);
        }

        public ActionResult GetLoginType(string GroupId,string LoginLevel)
        {
            List<SelectListItem> LstDetails = new List<SelectListItem>();
            try
            {
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                if (!string.IsNullOrEmpty(GroupId))
                {
                    if (LoginLevel == "1")
                    {
                        LstDetails = NewITOPS.GetGroupList(GroupId, connStr);
                    }
                    else if (LoginLevel == "2")
                    {
                        LstDetails = RR.GetBrandList(GroupId, connStr);
                    }
                    else if (LoginLevel == "3")
                    {
                        LstDetails = RR.GetOutletList(GroupId, connStr);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLoginType");
            }

            return Json(LstDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOTPCredential(string GroupId, string LoginType, string LoginId, string Password,string RequestedBy,string RequestedOn)
        {
            bool status;
            status = default;
            tblAudit objAudit = new tblAudit();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
            try
            {
                objAudit.GroupId = GroupId;
                objAudit.RequestedFor = "Add OTP Credential";
                objAudit.RequestedEntity = "LoginType - " + LoginType  + " LoginId - " + LoginId + " Password - "+ Password;
                objAudit.RequestedBy = RequestedBy;
                objAudit.RequestedOnForum = RequestedOn;
                objAudit.RequestedOn = Date;
                objAudit.AddedBy = userDetails.LoginId;
                objAudit.AddedDate = Date;
                

                if (!string.IsNullOrEmpty(GroupId))
                {
                    status = NewITOPS.SaveOTPDetails(GroupId, LoginType, LoginId, Password, objAudit);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveOTPCredential");
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}