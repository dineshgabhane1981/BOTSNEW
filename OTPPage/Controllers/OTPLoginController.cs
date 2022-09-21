

using BOTS_BL.Repository;
using OTPPage.Models;
using OTPPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTPPage.Controllers
{
    public class OTPLoginController : Controller
    {
        OTPRepository OPR = new OTPRepository();
        Exception newexception = new Exception();
        Class2 objdata = new Class2();

        // GET: OTPLogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Authenticate(Class2 obj)
        {
            objdata.LoginId = "123";
            objdata.Password = "123";

            if (objdata.LoginId == obj.LoginId && objdata.Password == obj.Password)
            {
                //Session["GroupId"] = groupId;
                return RedirectToAction("OTPPage");
                //return View("OTPPage");

            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult OTPPage()
        {
            var groupId = "1051";
            OTPViewModel objdata = new OTPViewModel();
            objdata.lstGroupDetails = OPR.GetGroupDetails();
            return View(objdata);
        }
    }
}