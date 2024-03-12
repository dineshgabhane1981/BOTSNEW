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
                    objCustDetails.Gender = Convert.ToString(item["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["DOB"])))
                        objCustDetails.DOB = Convert.ToDateTime(item["DOB"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["DOA"])))
                        objCustDetails.AnniversaryDate = Convert.ToDateTime(item["DOA"]);

                    objCustInfo.MobileNo = Convert.ToString(item["MobileNo"]);
                    objCustInfo.Name = Convert.ToString(item["CustomerName"]);

                    objCustTxnSummary.MobileNo = Convert.ToString(item["MobileNo"]);



                }
                result = RWR.AddMembership(objDetails, objCustDetails, objCustInfo, objCustTxnSummary, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult SendMemberOTP(string MobileNo)
        {
            bool result = false;
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}