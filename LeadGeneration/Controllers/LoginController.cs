using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;

namespace LeadGeneration.Controllers
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
                    if (userDetails.LoginId != null && (userDetails.LoginType == "5" || userDetails.LoginType == "8" || userDetails.LoginType == "1"))
                    {
                        Session["UserSession"] = userDetails;

                        return RedirectToAction("Index", "Lead");
                    }
                    else
                    {
                        TempData["InvalidUserMessage"] = "User Does not Exist"; 
                    }
                }
                else
                {
                    TempData["InvalidUserMessage"] = "User Does not Exist";
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