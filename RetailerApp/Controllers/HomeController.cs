﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models.RetailerWeb;
using BOTS_BL.Models;
using System.Web.Script.Serialization;
using System.Data;

namespace RetailerApp.Controllers
{
    public class HomeController : Controller
    {
        RetailerWebRepository RWR = new RetailerWebRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DailySummaryReport()
        {
            List<DailyReport> objData = new List<DailyReport>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            var obj = RWR.DailyReport(userDetails.OutletOrBrandId);

            ViewBag.DailyReport = obj;

            return View("DailySummaryReport");
        }

        public ActionResult MemberDetailedReport()
        {
            return View("MemberDetailedReport");
        }

        public ActionResult CancelTxn()
        {
            return View("CancelTxn");
        }

        [HttpPost]
        public ActionResult GetCustomerDetails(string MobileNo)
        {
            CustomerDetails objData = new CustomerDetails();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData = RWR.GetCustomerDetails(userDetails.OutletOrBrandId, MobileNo);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult EarntxnDetails(string jsonData)
        {
            EarnResponse EResponse = new EarnResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);
                string CustomerName = Convert.ToString(item["CustomerName"]);
                string InvoiceNo = Convert.ToString(item["InvoiceNo"]);
                string InvoiceAmt = Convert.ToString(item["InvoiceAmt"]);
                string DOB = Convert.ToString(item["DOB"]);
                string EmailId = Convert.ToString(item["EmailId"]);
                string Gender = Convert.ToString(item["Gender"]);
                string ADate = Convert.ToString(item["ADate"]);
                string CardNo = Convert.ToString(item["CardNo"]);

                EResponse = RWR.InsertEarnData(userDetails.OutletOrBrandId, MobileNo, CustomerName, InvoiceNo, InvoiceAmt, DOB, EmailId, Gender, ADate, CardNo);
            }            
            return new JsonResult() { Data = EResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult EarntxnDetailsOld(string jsonData)
        {
            EarnResponse EResponse = new EarnResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);
                string InvoiceNo = Convert.ToString(item["InvoiceNo"]);
                string InvoiceAmt = Convert.ToString(item["InvoiceAmt"]);

                EResponse = RWR.InsertEarnDataOld(userDetails.OutletOrBrandId, MobileNo,InvoiceNo, InvoiceAmt);
            }
            return new JsonResult() { Data = EResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult SendBurnValidation(string jsonData)
        {
            BurnValidationResponse BResponse = new BurnValidationResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);
                string InvoiceNo = Convert.ToString(item["InvoiceNo"]);
                string InvoiceAmt = Convert.ToString(item["InvoiceAmt"]); 
                string PointsBurn = Convert.ToString(item["PointsBurn"]);

                BResponse = RWR.BurnValidation(userDetails.OutletOrBrandId, MobileNo, InvoiceNo, InvoiceAmt, PointsBurn);
            }
            return new JsonResult() { Data = BResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult SaveBurnOld(string jsonData)
        {
            BurnResponse BResponse = new BurnResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);
                string InvoiceNo = Convert.ToString(item["InvoiceNo"]);
                string InvoiceAmt = Convert.ToString(item["InvoiceAmt"]);
                string PointsBurn = Convert.ToString(item["PointsBurn"]);

                BResponse = RWR.SaveBurnTxn(userDetails.OutletOrBrandId, MobileNo, InvoiceNo, InvoiceAmt, PointsBurn);
            }
            return new JsonResult() { Data = BResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult MemberDetailReport(string jsonData)
        {
            MDRDetails MDR = new MDRDetails();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);

                 MDR = RWR.MembDetailReprt(userDetails.OutletOrBrandId, MobileNo);
            }
            return new JsonResult() { Data = MDR, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult GetTxnDetails(string jsonData)
        {
            GTDTxnDetails GTD = new GTDTxnDetails();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);

                GTD = RWR.GetTxnDetailsRepo(userDetails.OutletOrBrandId, MobileNo);
            }
            return new JsonResult() { Data = GTD, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult CancelTxnDetails(string jsonData)
        {
            CancelData CTD = new CancelData();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);
                string InvoiceNo = Convert.ToString(item["InvoiceNo"]);

                CTD = RWR.CancelTxn(userDetails.OutletOrBrandId, MobileNo, InvoiceNo);
            }
            return new JsonResult() { Data = CTD, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult ResendOTP(string jsonData)
        {
            BurnValidationResponse BResponse = new BurnValidationResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] obj = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in obj)
            {
                string MobileNo = Convert.ToString(item["MobileNo"]);

                BResponse = RWR.OTPResend(userDetails.OutletOrBrandId, MobileNo);
            }
            return new JsonResult() { Data = BResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}