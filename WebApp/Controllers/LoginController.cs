using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        LoginRepository LR = new LoginRepository();
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
                var userDetails = LR.AuthenticateUser(objLogin);

                if (userDetails != null)
                {
                    Session["UserSession"] = userDetails;
                    if (!string.IsNullOrEmpty(userDetails.GroupId))
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("Index", "CustomerManagement");
                }
                else
                {
                    TempData["InvalidUserMessage"] = "User Does not Exist";
                }
            }
            catch (Exception ex)
            {
                TempData["InvalidUserMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}