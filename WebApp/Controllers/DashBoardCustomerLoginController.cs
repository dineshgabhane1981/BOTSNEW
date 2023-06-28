using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebApp.Controllers
{
    
    public class DashBoardCustomerLoginController : Controller
    {
        ReportsRepository RR = new ReportsRepository();
        DashBoardCustomerLoginRepository DR = new DashBoardCustomerLoginRepository();
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstoutletlist = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.lstoutletlist = lstoutletlist;
            var lstlogintype = DR.GetLoginType();
            ViewBag.lstLogintype = lstlogintype;
            List<DashboardCustomerLogin> objdashboard = new List<DashboardCustomerLogin>();
            objdashboard = DR.GetDashboardcustomerlogin(userDetails.GroupId);
            return View(objdashboard);
           
        }
        public ActionResult Security()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
            return View("MaskSetting", GroupDetails);
        }

        public JsonResult UpdateMasked(string value)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            result = DR.UpdateMaskedValue(Convert.ToInt32(userDetails.GroupId), value);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CustomerLoginList()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<DashboardCustomerLogin> lstcustlogin = new List<DashboardCustomerLogin>();
            lstcustlogin = DR.GetDashboardcustomerlogin(userDetails.GroupId);
            return PartialView("_DashboardCustomerloginList", lstcustlogin);
        }
        [HttpPost]
        public JsonResult GetOutletList()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstSubCallType = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            return new JsonResult() { Data = lstSubCallType, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public bool AddCustomerLogin(string Jsondata)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

           // Jsondata.GroupId = userDetails.GroupId;
           // status = DR.AddDashboardCustomerLogin(objcustomerlogin);
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(Jsondata);
                DashboardCustomerLogin objcustomerlogin = new DashboardCustomerLogin();
                foreach (Dictionary<string, object> item in objData)
                {
                    objcustomerlogin.MobileNo = Convert.ToString(item["Customermobileno"]);
                    objcustomerlogin.OutletOrBrandId = Convert.ToString(item["outletid"]);
                    objcustomerlogin.LoginType = Convert.ToString(item["Logintype"]);
                    objcustomerlogin.GroupId = userDetails.GroupId;
                    objcustomerlogin.CustomerName = userDetails.CustomerName;

                }
                status = DR.AddDashboardCustomerLogin(objcustomerlogin);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddCustomerLogin");
            }
            return status;
        }

        [HttpPost]
        public ActionResult GetDashboardlogins()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<DashboardCustomerLogin> objdashboard = new List<DashboardCustomerLogin>();
            try
            {
                objdashboard = DR.GetDashboardcustomerlogin(userDetails.GroupId);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardlogins");
            }
            return Json(objdashboard, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult SendOTP()
        {
            
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            var Data = DR.SendOTP(userDetails.GroupId);
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
    }
}