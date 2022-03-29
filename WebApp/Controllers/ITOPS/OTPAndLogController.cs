using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.ITOPS
{
    public class OTPAndLogController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: OTPAndLog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Log()
        {
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
    }
}