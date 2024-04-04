using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models.RetailerWeb;
using BOTS_BL.Models;
using System.Web.Script.Serialization;
using System.Data;
using Newtonsoft.Json.Linq;
using BOTS_BL;

namespace RetailerApp.Controllers
{
    public class MembershipController : Controller
    {
        Exceptions newexception = new Exceptions();
        RetailerWebRepository RWR = new RetailerWebRepository();
        EventsRepository ER = new EventsRepository();
        // GET: Membership
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            return View();
        }
        [HttpPost]
        public ActionResult GetCustomerBasicDetails(string MobileNo)
        {
            CustomerDetails objData = new CustomerDetails();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData = RWR.GetCustomerDetails(userDetails.OutletOrBrandId, MobileNo);
            var data = RWR.GetMembershipDetail(userDetails.connectionString, MobileNo);
            if (data != null)
            {
                objData.PackageAmount = Convert.ToString(data.PackageType);
                objData.PackageRemainingAmount = Convert.ToString(data.RemainingAmount);
            }
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult RegisterNewUser(string jsonData)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            tblMembershipDetail objDetails = new tblMembershipDetail();
            tblCustDetailsMaster objCustDetails = new tblCustDetailsMaster();
            tblCustInfo objCustInfo = new tblCustInfo();
            tblCustTxnSummaryMaster objCustTxnSummary = new tblCustTxnSummaryMaster();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objDetails.MobileNo = Convert.ToString(item["MobileNo"]);
                    objDetails.PackageType = Convert.ToDecimal(item["Package"]);
                    objDetails.PackageValidity = Convert.ToDateTime(item["Validity"]);
                    objDetails.CreatedDate = DateTime.Now;
                    objDetails.CreatedBy = userDetails.LoginId;

                    objCustDetails.MobileNo = Convert.ToString(item["MobileNo"]);
                    objCustDetails.Name = Convert.ToString(item["CustomerName"]);
                    objCustDetails.Id = Convert.ToString(userDetails.GroupId) + Convert.ToString(item["MobileNo"]);
                    objCustDetails.EnrolledOutlet = Convert.ToString(userDetails.OutletOrBrandId).Substring(0,7);
                    objCustDetails.DOJ = ER.IndianDatetime();
                    objCustDetails.EnrolledBy = "WalkIn";
                    objCustDetails.CurrentEnrolledOutlet = Convert.ToString(userDetails.OutletOrBrandId).Substring(0, 7);
                    objCustDetails.IsActive = true;
                    objCustDetails.DisableTxn = false;
                    objCustDetails.DisableSMSWAPromo = false;
                    objCustDetails.Gender = Convert.ToString(item["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["DOB"])))
                        objCustDetails.DOB = Convert.ToDateTime(item["DOB"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["DOA"])))
                        objCustDetails.AnniversaryDate = Convert.ToDateTime(item["DOA"]);
                    objCustDetails.Tier = "Membership Customer";
                    objCustDetails.DisableSMSWATxn = false;

                    objCustInfo.MobileNo = Convert.ToString(item["MobileNo"]);
                    objCustInfo.Name = Convert.ToString(item["CustomerName"]);

                    objCustTxnSummary.MobileNo = Convert.ToString(item["MobileNo"]);
                    objCustTxnSummary.FirstTxnDate = ER.IndianDatetime();
                    objCustTxnSummary.LastTxnDate = ER.IndianDatetime();
                    objCustTxnSummary.TotalSpend = 0;
                    objCustTxnSummary.TotalTxnCount = 0;
                    objCustTxnSummary.EarnCount = 0;
                    objCustTxnSummary.BurnCount = 0;
                    objCustTxnSummary.SalesReturnCount = 0;
                    objCustTxnSummary.SalesReturnAmt = 0;
                    objCustTxnSummary.BurnAmtWithPts = 0;
                    objCustTxnSummary.BurnAmtWithoutPts = 0;
                    objCustTxnSummary.BurnPts = 0;
                    objCustTxnSummary.EarnPts = 0;
                    objCustTxnSummary.SalesReturnPtsGiven = 0;
                    objCustTxnSummary.SalesReturnPtsRemoved = 0;
                }
                result = RWR.AddMembership(objDetails, objCustDetails, objCustInfo, objCustTxnSummary, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult SendMemberOTP(string MobileNo,string Packageamount,string ValidityOld,string PointsRedeem)
        {
            string result, DummyInvoiceNo;
            DummyInvoiceNo = string.Empty;
            result = "1234";
            CustomerDetails objData = new CustomerDetails();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            //result = RWR.BurnValidationRatnaEnterprise(userDetails.OutletOrBrandId, MobileNo, DummyInvoiceNo, Packageamount,);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}