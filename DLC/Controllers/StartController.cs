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
                Session["HeaderColor"] = objData.objDashboardConfig.HeaderColor;
                Session["FontColor"] = objData.objDashboardConfig.FontColor;
                Session["LogoUrl"] = objData.objDashboardConfig.UseLogoURL;
                Session["LogoSize"] = objData.objDashboardConfig.UseLogo;
                Session["GroupId"] = groupId;
            }
            ViewBag.Codes = DCR.GetCountryCodes();
            return View(objData);
        }
    
        public ActionResult CheckandSendOTP(string MobileNo)
        {
            string OTPorPAssword = string.Empty;
            string gId = Convert.ToString(Session["GroupId"]);
            OTPorPAssword = DCR.CheckUserAndSendOTP(gId, MobileNo);
            return new JsonResult() { Data = OTPorPAssword, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ValidateUser(string mobileNo, string OtpOrPassword)
        {
            bool IsValid = false;
            string ActionNameFromConfig = string.Empty;
            string gId = Convert.ToString(Session["GroupId"]);
            //If OTP
            var config = DCR.GetDLCDashboardConfig(gId);

            
            //If Password and exist in UserDetails
            if (config.LoginWithOTP=="Password")
            {
                var IsExist = DCR.CheckPasswordExist(gId, mobileNo);
                if (IsExist)
                {
                    IsValid = DCR.ValidateUserByPassword(gId, mobileNo, OtpOrPassword);
                    //redirect to RedirectToPage
                    if (IsValid)
                    {
                        ActionNameFromConfig = GetRedirectActionName(config.RedirectToPage);
                        //return RedirectToAction(ActionNameFromConfig);
                    }
                }
                else
                {
                    //If Password but not exist in UserDetails
                    IsValid = DCR.ValidateUserByOTP(gId, mobileNo, OtpOrPassword);
                    //Redirect to Set Password Screen
                    if (IsValid)
                    {
                        ActionNameFromConfig = "SetPassword";
                    }
                }
            }
            else
            {
                //if Validate using OTP
                IsValid = DCR.ValidateUserByOTP(gId, mobileNo, OtpOrPassword);
                if(IsValid)
                {
                    //redirect to RedirectToPage
                    ActionNameFromConfig = GetRedirectActionName(config.RedirectToPage);
                    //return RedirectToAction(ActionNameFromConfig);
                }
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
            return View();
        }




    }
}