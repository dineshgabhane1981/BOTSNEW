using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;

namespace WebApp.Controllers
{
    public class TelecallerController : Controller
    {
        TelecallerRepository TR = new TelecallerRepository();
        ReportsRepository RR = new ReportsRepository();
        // GET: Telecaller
        public ActionResult Index()
        {
            TelecallerCustomerData objteledata = new TelecallerCustomerData();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstGenderList = RR.GetGenderList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.lstGenderList = lstGenderList;
            return View(objteledata);
        }
        public ActionResult Telecaller()
        {
           // TelecallerCustomerData objteledata = new TelecallerCustomerData();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            
            return View();
        }
        public JsonResult GetCustomerData(string MobileNo)
        {
            TelecallerCustomerData objteledata = new TelecallerCustomerData();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objteledata = TR.GetTelecallerCustomer(MobileNo, userDetails.GroupId, userDetails.connectionString);
            return new JsonResult() { Data = objteledata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };


        }
        
        public ActionResult SaveTelecallerData(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string mobileNo = Convert.ToString(item["MobileNo"]);
                string CustomerNm = Convert.ToString(item["CustNm"]);
                string Gender = Convert.ToString(item["Gender"]);
                DateTime DateofBirth = Convert.ToDateTime(item["DOB"]);
                DateTime DateOfAnni = Convert.ToDateTime(item["DOA"]);
                int PointsGiven = Convert.ToInt32(item["Points"]);
                string OutletId = Convert.ToString(item["outletId"]);
                string Comments = Convert.ToString(item["Comments"]);

                status = TR.SaveTelecallerTracking(userDetails.connectionString, userDetails.LoginId, mobileNo, CustomerNm, Gender, DateofBirth, DateOfAnni, PointsGiven, OutletId, Comments);
            }
                return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public JsonResult GetReportData(string searchData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<TelecallerTracking> lsttelecaller = new List<TelecallerTracking>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                DateTime fromdt = Convert.ToDateTime(item["frmDate"]);
                DateTime todt = Convert.ToDateTime(item["toDate"]);

                lsttelecaller = TR.GetTelecallerReportData(fromdt,todt,userDetails.connectionString,userDetails.GroupId);
            }
            return new JsonResult() { Data = lsttelecaller, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
    }
}