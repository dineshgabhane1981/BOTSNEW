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
        DashboardRepository DR = new DashboardRepository();
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
        public JsonResult GetOutletList(string GroupId)
        {                        
            string connectionString = CR.GetCustomerConnString(GroupId);            
            var lstOutlet = RR.GetOutletList(GroupId, connectionString);
            return new JsonResult() { Data = lstOutlet, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpGet]
        public JsonResult GetMemberSegmentResult(string GroupId,string OutletId)
        {
            List<object> lstData = new List<object>();
            List<long> dataList = new List<long>();
            DashboardMemberSegment dataMemberSegment = new DashboardMemberSegment();
            try
            {
                List<string> lstDates = new List<string>();
                string connectionString = CR.GetCustomerConnString(GroupId);
                
                dataMemberSegment = DR.GetDashboardMemberSegmentData(GroupId, OutletId, connectionString, "", "", "");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberSegmentResult");
            }            
            return new JsonResult() { Data = dataMemberSegment, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpGet]
        public JsonResult GetPointsSummaryResult(string GroupId,string monthFlag)
        {
           
            List<long> dataList = new List<long>();
            DashboardPointsSummary dataPointsSummary = new DashboardPointsSummary();
            try
            {
                string loginId = string.Empty;
                string connectionString = CR.GetCustomerConnString(GroupId);
               
                dataPointsSummary = DR.GetDashboardPointsSummaryData(GroupId, monthFlag, connectionString, loginId,"","");

                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsSummaryResult");
            }
            return new JsonResult() { Data = dataPointsSummary, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpGet]
        public JsonResult GetBulkUploadResult(string GroupId)
        {
            List<object> dataList = new List<object>();
            DashboardBulkUpload objDashboardBulkUpload = new DashboardBulkUpload();
            string connectionString = CR.GetCustomerConnString(GroupId);
            try
            {                
                objDashboardBulkUpload = DR.GetDashboardBulkUpload(GroupId, connectionString,"","","");                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBulkUploadResult");
            }
            return new JsonResult() { Data = objDashboardBulkUpload, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}