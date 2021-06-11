using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;
using BOTS_BL;
using WebApp.App_Start;

namespace WebApp.Controllers
{   
    public class botsapiController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        ReportsRepository RR = new ReportsRepository();
        Exceptions newexception = new Exceptions();
        LoginRepository LR = new LoginRepository();
        // GET: botsapi
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetGroupId(string loginId, string password)
        {
            string groupId = LR.AuthenticateUserMobile(loginId, password);
            return Json(groupId, JsonRequestBehavior.AllowGet);
        }        

        [HttpGet]
        public JsonResult GetMemberDataResult(string GroupId, string SearchText)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (SearchText.Equals("All"))
            {
                SearchText = "";
            }
            string connectionString = CR.GetCustomerConnString(GroupId);
            List<MemberList> lstMember = new List<MemberList>();
            lstMember = RR.GetMemberList(GroupId, SearchText, connectionString);
            return new JsonResult() { Data = lstMember, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}