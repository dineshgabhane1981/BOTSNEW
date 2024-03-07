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
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult RegisterNewUser(string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            tblMembershipDetail objDetails = new tblMembershipDetail();
            tblCouponRule objRule = new tblCouponRule();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objRule.EarnRuleName = Convert.ToString(item["CustomerName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnInvoiceAmountFrom"])))
                        objRule.EarnInvoiceAmountFrom = Convert.ToDecimal(item["EarnInvoiceAmountFrom"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnInvoiceAmountTo"])))
                        objRule.EarnInvoiceAmountTo = Convert.ToDecimal(item["EarnInvoiceAmountTo"]);
                    objRule.EarnDay = Convert.ToString(item["EarnDay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnCategory"])) && Convert.ToString(item["EarnCategory"]) != "All Category")
                        objRule.EarnCategory = Convert.ToInt32(item["EarnCategory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnProduct"])) && Convert.ToString(item["EarnProduct"]) != "All Product")
                        objRule.EarnProduct = Convert.ToInt32(item["EarnProduct"]);
                    objRule.EarnOutlet = Convert.ToString(item["EarnOutlet"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemInvoiceAmountFrom"])))
                        objRule.RedeemInvoiceAmountFrom = Convert.ToDecimal(item["RedeemInvoiceAmountFrom"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemInvoiceAmountTo"])))
                        objRule.RedeemInvoiceAmountTo = Convert.ToDecimal(item["RedeemInvoiceAmountTo"]);
                    objRule.RedeemDay = Convert.ToString(item["RedeemDay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemCategory"])) && Convert.ToString(item["RedeemCategory"]) != "All Category")
                        objRule.RedeemCategory = Convert.ToInt32(item["RedeemCategory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemProduct"])) && Convert.ToString(item["RedeemProduct"]) != "All Product")
                        objRule.RedeemProduct = Convert.ToInt32(item["RedeemProduct"]);
                    objRule.RedeemOutlet = Convert.ToString(item["RedeemOutlet"]);
                    objRule.IsActive = true;
                    objRule.AddedDate = DateTime.Now;
                    objRule.AddedBy = userDetails.LoginId;
                    objRule.IsOnlyCoupon = Convert.ToBoolean(item["IsOnlyCoupon"]);
                    objRule.CouponValue = Convert.ToInt32(item["CouponValue"]);
                    objRule.ExpiryDays = Convert.ToInt32(item["ExpiryDays"]);
                    objRule.OfferCode = Convert.ToString(item["OfferCode"]);
                }
                //result = CR.SaveCouponEarnRule(objRule, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}