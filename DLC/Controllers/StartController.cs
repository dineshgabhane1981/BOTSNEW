﻿using System;
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
            string mobileNumber = string.Empty;
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
                    if(item.Contains("MobileNo"))
                    {
                        var contact = item.Split('=');
                        mobileNumber = contact[1];
                        DLCDashboardFrontData objData1 = new DLCDashboardFrontData();
                        if (!string.IsNullOrEmpty(groupId))
                        {
                            objData1.objDashboardConfig = DCR.GetPublishDLCDashboardConfig(groupId);
                            if (objData1.objDashboardConfig == null)
                            {
                                return View("UnauthorizedURL");
                            }
                            //objData.lstDLCFrontEndPageData = DCR.GetDLCFrontEndPageData(groupId);
                            SessionVariables objVariable = new SessionVariables();
                            objVariable.objDashboardConfig = objData1.objDashboardConfig;
                            objVariable.GroupId = groupId;
                            objVariable.BrandId = brandId;
                            objVariable.MobileNo = mobileNumber;
                            objVariable.Flag = "true";
                            objVariable.LoginURL = url;
                            objVariable.Source = source;

                            Session["SessionVariables"] = objVariable;

                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return View("UnauthorizedURL");
                        }                        
                    }
                }
            }
            else
            {
                return View("UnauthorizedURL");
            }
            DLCDashboardFrontData objData = new DLCDashboardFrontData();
            if (!string.IsNullOrEmpty(groupId))
            {
                objData.objDashboardConfig = DCR.GetPublishDLCDashboardConfig(groupId);
                if (objData.objDashboardConfig == null)
                {
                    return View("UnauthorizedURL");
                }
                //objData.lstDLCFrontEndPageData = DCR.GetDLCFrontEndPageData(groupId);
                SessionVariables objVariable = new SessionVariables();
                objVariable.objDashboardConfig = objData.objDashboardConfig;
                objVariable.GroupId = groupId;
                objVariable.BrandId = brandId;
                objVariable.Flag = "true";
                objVariable.LoginURL = url;
                objVariable.Source = source;
                Session["SessionVariables"] = objVariable;
            }
            ViewBag.Codes = DCR.GetCountryCodes();
            return View(objData);
        }
        //public ActionResult RedirectToLogin()
        //{
        //    //This need to correct
        //    string customLoginUrl = "http://localhost:44309/Start/?data=KQ3yLweiMnawfNOwhc4so5N/z4mPRIKo7I8sYSeX7NfdjoLKk9h0cICWfQzwyMTfaHw4mFkFqv+4wGXLj6rYOhUyxV/lFtqwq2Mw58FjTcE=";
        //    return Redirect(customLoginUrl);
        //}

        public ActionResult CheckandSendOTP(string MobileNo)
        {
            string OTPorPassword = string.Empty;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            OTPorPassword = DCR.CheckUserAndSendOTP(gId, MobileNo);
            sessionVariables.MobileNo = MobileNo;
            Session["SessionVariables"] = sessionVariables;
            return new JsonResult() { Data = OTPorPassword, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ValidateUser(string mobileno, string code, string OtpOrPassword)
        {
            bool IsValid = false;
            string ActionNameFromConfig = string.Empty;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            string sourcename = sessionVariables.Source;
            //If OTP
            var config = DCR.GetDLCDashboardConfig(gId);

            //If Password and exist in UserDetails
            if (config.LoginWithOTP == "Password")
            {
                var custDetails = DCR.CheckPasswordExist(gId, code + mobileno);
                if (custDetails.SlNo != 0)
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
            if (!string.IsNullOrEmpty(ActionNameFromConfig))
            {
                
                var status = DCR.RegisterCustomer(gId, mobileno, code , sourcename);
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
                ActionName = "PersonalDetails";
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

        public ActionResult ResetPassword()
        {
            DLCDashboardFrontData objData = new DLCDashboardFrontData();
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            ViewBag.MobileNo = sessionVariables.CountryCode + sessionVariables.MobileNo;
            objData.objDashboardConfig = DCR.GetPublishDLCDashboardConfig(gId);
            var smsdetails = DCR.GetSMSDetails(gId);
            var status = DCR.SendOTP(gId, ViewBag.MobileNo, smsdetails);
            return View(objData);
        }
        public ActionResult VerifyOTP(string otp)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            status = DCR.ValidateUserByOTP(gId, sessionVariables.MobileNo, otp);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult InsertPassword(string mobileNo, string password)
        {
            bool status = false;
            Hash objHash = new Hash();
            password = objHash.HashPassword(password);
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            status = DCR.InsertPassword(mobileNo, password, gId);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ResentOTP(string mobileNo)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            string gId = sessionVariables.GroupId;
            var smsdetails = DCR.GetSMSDetails(gId);
            status = DCR.SendOTP(gId, mobileNo, smsdetails);
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