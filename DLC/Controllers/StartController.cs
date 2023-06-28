using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using DLC.App_Start;
using DLC.ViewModel;
using BOTS_BL.Models;

namespace DLC.Controllers
{
    public class StartController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: Start
        public ActionResult Index(string data)
        {
            var url = Request.Url.ToString();
            string brandId = string.Empty;
            string groupId = string.Empty;
            string source = string.Empty;
            CommonFunctions common = new CommonFunctions();
            if (!string.IsNullOrEmpty(data))
            {
                var parameterStr = common.DecryptString(data);
                var parameters = parameterStr.Split('&');
                foreach (var item in parameters)
                {
                    if (item.Contains("BrandId"))
                    {
                        var brandData = item.Split('=');
                        brandId = brandData[1];
                        groupId = brandId.Substring(0, 4);
                    }
                    if (item.Contains("Source"))
                    {
                        var sourceData = item.Split('=');
                        source = sourceData[1];                    
                    }
                }
            }
            DLCDashboardFrontData objData = new DLCDashboardFrontData();
            if (!string.IsNullOrEmpty(groupId))
            {
                objData.objDashboardConfig = DCR.GetPublishDLCDashboardConfig(groupId);
                objData.lstDLCFrontEndPageData = DCR.GetDLCFrontEndPageData(groupId);
                SessionVariables objVariable = new SessionVariables();
                objVariable.objDashboardConfig = objData.objDashboardConfig;               
                objVariable.GroupId = groupId;
                objVariable.BrandId = brandId;
                objVariable.LoginURL = url;

                Session["SessionVariables"] = objVariable;
            }
            ViewBag.Codes = DCR.GetCountryCodes();
            return View(objData);
        }
    
        public ActionResult CheckandSendOTP(string MobileNo)
        {
            string OTPorPassword = string.Empty;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            OTPorPassword = DCR.CheckUserAndSendOTP(gId, MobileNo);
            return new JsonResult() { Data = OTPorPassword, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ValidateUser(string mobileno, string code, string OtpOrPassword)
        {
            bool IsValid = false;
            string ActionNameFromConfig = string.Empty;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            //If OTP
            var config = DCR.GetDLCDashboardConfig(gId);

            //If Password and exist in UserDetails
            if (config.LoginWithOTP == "Password")
            {
                var custDetails = DCR.CheckPasswordExist(gId, code + mobileno);
                if (custDetails !=null)
                {
                    Hash objHash = new Hash();
                    IsValid = objHash.VerifyHashedPassword(custDetails.Password, OtpOrPassword);
                    //IsValid = DCR.ValidateUserByPassword(gId, mobileNo, OtpOrPassword);
                    //redirect to RedirectToPage
                    if (IsValid)
                    {
                        ActionNameFromConfig = GetRedirectActionName(config.RedirectToPage);
                    }
                }
                else
                {
                    //If Password but not exist in UserDetails
                    IsValid = DCR.ValidateUserByOTP(gId, code + mobileno, OtpOrPassword);
                    //Redirect to Set Password Screen
                    if (IsValid)
                    {
                        ActionNameFromConfig = "Start/SetPassword";
                    }
                }
            }
            else
            {
                //if Validate using OTP
                IsValid = DCR.ValidateUserByOTP(gId, code + mobileno, OtpOrPassword);
                if (IsValid)
                {
                    //redirect to RedirectToPage
                    ActionNameFromConfig = GetRedirectActionName(config.RedirectToPage);
                }
            }
            if(!string.IsNullOrEmpty(ActionNameFromConfig))
            {
                sessionVariables.CountryCode = code;
                sessionVariables.MobileNo = mobileno;
                Session["SessionVariables"] = sessionVariables;
            }
            return new JsonResult() { Data = ActionNameFromConfig, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public string GetRedirectActionName(string pageName)
        {
            string ActionName = string.Empty;
            if (pageName == "Dashboard")
                ActionName = "Dashboard";
            if (pageName == "Update Profile")
                ActionName = "UpdateProfile";
            if (pageName == "Gift Points")
                ActionName = "GiftPoints";
            if (pageName == "Transaction History")
                ActionName = "TransactionHistory";

            return ActionName;
        }
        
        public ActionResult SetPassword()
        {
            DLCDashboardFrontData objData = new DLCDashboardFrontData();
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            ViewBag.MobileNo = sessionVariables.CountryCode + sessionVariables.MobileNo;
            objData.objDashboardConfig = DCR.GetPublishDLCDashboardConfig(gId);
            return View(objData);
        }
    
        public ActionResult InsertPassword(string mobileNo,string password)
        {
            bool status = false;
            Hash objHash = new Hash();
            password = objHash.HashPassword(password);
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            status = DCR.InsertPassword(mobileNo, password, gId);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    
        public ActionResult Logout()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            Session.Abandon();
            return Redirect(sessionVariables.LoginURL);
            //return View();
        }
    }
}