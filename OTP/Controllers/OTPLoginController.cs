using BOTS_BL.Repository;
using OTP.Models;
using OTP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace OTP.Controllers
{
    public class OTPLoginController : Controller
    {
        OTPRepository OPR = new OTPRepository();
        Exception newexception = new Exception();
        Class1 objdata = new Class1();


        // GET: OTPLogin
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Authenticate(Class1 obj)
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
            var groupId = Convert.ToString(Session["GroupId"]);
            OTPViewModel objdata = new OTPViewModel();
            objdata.lstGroupDetails = OPR.GetGroupDetails(groupId);
            return View(objdata);
        }
    }
}