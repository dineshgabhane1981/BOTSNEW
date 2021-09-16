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

            }
            return Json(objdashboard, JsonRequestBehavior.AllowGet);


        }
    }
}