using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        LoginRepository LR = new LoginRepository();
        DashboardRepository DR = new DashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: Login
        public ActionResult Index(string MobileNo)
        {
            if (!string.IsNullOrEmpty(MobileNo))
            {
                var userDetails = LR.GetUserDetailsByLoginID(MobileNo);
                if (userDetails != null)
                {
                    if (userDetails.GroupId == "1088")
                    {
                        Session["UserSession"] = userDetails;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            LoginModel objLogin = new LoginModel();
            return View(objLogin);
        }

        public ActionResult UserAuthentication(LoginModel objLogin)
        {
            try
            {
                CustomerLoginDetail userDetails = new CustomerLoginDetail();
                //var status = DR.VerifyOTP(emailId, Convert.ToInt32(OTP));
                if (objLogin.OTP == null)
                {
                    userDetails = LR.AuthenticateUser(objLogin);
                }
                else
                {
                    var status = DR.VerifyOTP(objLogin.LoginId, Convert.ToInt32(objLogin.OTP));
                    if (status)
                    {
                        userDetails = LR.GetUserDetailsByLoginID(objLogin.LoginId);
                    }
                    else
                    {
                        TempData["InvalidUserMessage"] = "There is problem in verifying OTP. Please check once";
                    }
                }
                if (userDetails != null)
                {
                    if (userDetails.LoginId != null)
                    {
                        Session["UserSession"] = userDetails;
                        if (!string.IsNullOrEmpty(userDetails.GroupId))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else// if (string.IsNullOrEmpty(objLogin.OTP))
                        {
                            return RedirectToAction("Index", "CustomerManagement");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(TempData["InvalidUserMessage"])))
                        {
                            TempData["InvalidUserMessage"] = "User Does not Exist";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["InvalidUserMessage"])))
                    {
                        TempData["InvalidUserMessage"] = "User Does not Exist";
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["InvalidUserMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public string CheckUserAndSendOTP(string LoginID)
        {
            string returnString = string.Empty;
            try
            {

                var userDetail = LR.CheckUserType(LoginID);
                if (userDetail != null)
                {
                    if (userDetail.LoginType != "1" && userDetail.LoginType != "5" && userDetail.LoginType != "6" && userDetail.LoginType != "7" && string.IsNullOrEmpty(userDetail.OutletOrBrandId))
                    {
                        var result = new HomeController().SendOTP(LoginID);
                        if (result)
                        {
                            returnString = "OTP";
                        }
                        else
                        {
                            returnString = "error in sending OTP";
                        }
                    }
                    else
                    {
                        returnString = "Password";
                    }
                }
                else
                {
                    returnString = "NoUserFound";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["InvalidUserMessage"] = ex.Message;
                returnString = "There is problem in Validation";
            }
            return returnString;
        }



    }
}