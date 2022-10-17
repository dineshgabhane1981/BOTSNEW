using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;

namespace RetailerApp.Controllers
{
    public class LoginController : Controller
    {
        LoginRepository LR = new LoginRepository();
        DashboardRepository DR = new DashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: Login
        public ActionResult Index()
        {
            LoginModel objLogin = new LoginModel();
            return View(objLogin);
        }

        public ActionResult UserAuthentication(LoginModel objLogin)
        {
            try
            {
                CustomerLoginDetail userDetails = new CustomerLoginDetail();
                userDetails = LR.AuthenticateUser(objLogin);

                if (userDetails != null)
                {
                    if (userDetails.LoginId != null)
                    {
                        Session["UserSession"] = userDetails;
                        tblLoginLog objLogData = new tblLoginLog();
                        objLogData.LoginId = userDetails.LoginId;
                        objLogData.UserName = userDetails.UserName;
                        objLogData.LoggedInTime = DateTime.Now;
                        LR.AddLoginLog(objLogData);

                        if (!string.IsNullOrEmpty(userDetails.GroupId))
                        {
                            return RedirectToAction("Index", "Home");
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
    }
}