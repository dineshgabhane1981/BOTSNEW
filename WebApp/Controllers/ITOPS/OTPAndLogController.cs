using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class OTPAndLogController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        
        // GET: EarnBurn
        string groupId;
        public ActionResult Index()
        {
            var groupId = (string)Session["GroupId"];
            ViewBag.GroupId = groupId;
            return View();
        }

        public ActionResult Log()
        {
            try
            {
                CommonFunctions common = new CommonFunctions();
                //groupId = common.DecryptString(groupId);
                groupId = Session["GroupId"].ToString();
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                var lstOutlet = RR.GetOutletList(groupId, connStr);
                var lstBrand = RR.GetBrandList(groupId, connStr);
                var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
                ViewBag.OutletList = lstOutlet;
                ViewBag.BranchList = lstBrand;
                ViewBag.GroupId = groupId;

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, groupId);
            }

            return View();
        }

        public ActionResult GetOTPData(string MobileNo)
        {
            MemberData objCustomerDetail = new MemberData();
            var GroupId = (string)Session["GroupId"];
            if (!string.IsNullOrEmpty(MobileNo))
            {
                objCustomerDetail = ITOPS.GetOTPData(GroupId, MobileNo);
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTxnLogData(string GroupId, string search)
        {
            List<LogDetailsRW> lstLogDetails = new List<LogDetailsRW>();
            ITOpsRepository ITOPS = new ITOpsRepository();
            lstLogDetails = ITOPS.GetLogDetails(search, GroupId);
            return Json(lstLogDetails, JsonRequestBehavior.AllowGet);

        }
    }
}