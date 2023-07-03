using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.Controllers
{
    public class ReferEarnController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        Exceptions newexception = new Exceptions();
        ReportsRepository RR = new ReportsRepository();
        // GET: ReferEarn
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var lstData = DCR.GetMWPReferTNC(sessionVariables.GroupId);
            return View(lstData);
        }
        public ActionResult DLCReferFriend(string FirstName, string FirstMobile, string SecondName, string SecondMobile, string ThirdName, string ThirdMobile)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            status = DCR.DLCReferFriend(sessionVariables.GroupId, sessionVariables.MobileNo, sessionVariables.BrandId, FirstMobile, FirstName, SecondMobile, SecondName, ThirdMobile, ThirdName);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}