using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        CustomerRepository objCustRepo = new CustomerRepository();
        // GET: ReferEarn
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var lstData = DCR.GetMWPReferTNC(sessionVariables.GroupId);
            return View(lstData);
        }
        [HttpPost]
        public ActionResult DLCReferFriend(List<FriendData> friends)
        {
            //bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var status = DCR.DLCReferFriend(sessionVariables.GroupId, sessionVariables.MobileNo, sessionVariables.BrandId, friends);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        //public List<MWP_ReferTNC> GetMWPReferTNC(string groupId)
        //{
        //    List<MWP_ReferTNC> lstData = new List<MWP_ReferTNC>();
        //    string connStr = objCustRepo.GetCustomerConnString(groupId);
        //    using (var context = new BOTSDBContext(connStr))
        //    {
        //        try
        //        {
        //            lstData = context.MWP_ReferTNC.ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "GetMWPReferTNC " + groupId);
        //        }
        //        return lstData;
        //    }
        //}

        
    }
}