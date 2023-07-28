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
    public class GiftPointsController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        Exceptions newexception = new Exceptions();
        // GET: GiftPoints
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            DLCDashboardContent objData = new DLCDashboardContent();
            objData = DCR.GetDLCDashboardContent(sessionVariables.GroupId, sessionVariables.MobileNo);
            objData.MobileNo = sessionVariables.MobileNo;
            return View(objData);
        }
        public ActionResult GiveGiftPoints(string RecipientName,string RecipientNo,string GiftPoints)
        {           
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var status = DCR.GiveGiftPoints(sessionVariables.MobileNo, sessionVariables.BrandId, RecipientName, RecipientNo, GiftPoints, sessionVariables.GroupId);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}