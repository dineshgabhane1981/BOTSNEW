using BOTS_BL.Models;
using BOTS_BL.Models.RetailerWeb;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MoolchandRedemption.Controllers
{
    
    public class HomeController : Controller
    {
        MoolchandRedemptionRepository MRR = new MoolchandRedemptionRepository();
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        [HttpPost]
        public ActionResult GetCustomerDetails(string MobileNo)
        {
            CustomerDetails objData = new CustomerDetails();
            CustDetails objCustData = new CustDetails();

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData = MRR.GetCustomerDetails(userDetails.LoginId,userDetails.GroupId, MobileNo);

            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
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
                //string DynamicDatas = Convert.ToString(item["DynamicData"]);

                BResponse = MRR.BurnValidation(userDetails.LoginId, userDetails.GroupId, MobileNo, InvoiceNo, InvoiceAmt, PointsBurn);
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
                
                BResponse = MRR.SaveBurnTxn(userDetails.LoginId, userDetails.GroupId, MobileNo, InvoiceNo, InvoiceAmt, PointsBurn);
            }
            return new JsonResult() { Data = BResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
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
                string InvoiceNo = Convert.ToString(item["InvoiceNo"]);
                string InvoiceAmt = Convert.ToString(item["InvoiceAmt"]);
                string PointsBurn = Convert.ToString(item["PointsBurn"]);

                BResponse = MRR.OTPResend(userDetails.LoginId, userDetails.GroupId, MobileNo, InvoiceNo, InvoiceAmt, PointsBurn);
            }
            return new JsonResult() { Data = BResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}