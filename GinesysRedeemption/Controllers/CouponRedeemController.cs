using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GinesysRedeemption.App_Start;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;

namespace GinesysRedeemption.Controllers
{
    public class CouponRedeemController : Controller
    {
        CommonFunctions common = new CommonFunctions();
        GinesysRedeemptionRepository GRR = new GinesysRedeemptionRepository();
        Exceptions newexception = new Exceptions();
        // GET: CouponRedeem
        public ActionResult Index(string token)
        {
            CommonFunctions common = new CommonFunctions();
            GinesysRedeemCouponModel objRedeemData = new GinesysRedeemCouponModel();
            if (!string.IsNullOrEmpty(token))
            {
                var parameterStr = common.DecryptString(token);
                var parameters = parameterStr.Split('&');
                foreach (var item in parameters)
                {
                    if (item.Contains("storeid"))
                    {
                        var storeIdParam = item.Split('=');
                        objRedeemData.StoreId = storeIdParam[1];
                    }
                    if (item.Contains("mobileno"))
                    {
                        var mobilenoParam = item.Split('=');
                        objRedeemData.MobileNo = mobilenoParam[1];
                    }
                    if (item.Contains("billvalue"))
                    {
                        var billvalueParam = item.Split('=');
                        objRedeemData.InvoiceAmount = billvalueParam[1];
                    }
                    if (item.Contains("billGUID"))
                    {
                        var billGUIDParam = item.Split('=');
                        objRedeemData.billGUID = billGUIDParam[1];
                    }                    
                    if (item.Contains("CustomerName"))
                    {
                        var billCustomerNameParam = item.Split('=');
                        objRedeemData.CustomerName = billCustomerNameParam[1];
                    }
                }
            }
            return View(objRedeemData);
        }
        public ActionResult GetURL()
        {
            var BaseUrl = ConfigurationManager.AppSettings["RedeemCouponBaseUrl"];
            string storeid = "1002888888";
            string billGUID = "D4E8010B-7164-46ED-8F3D-43F8B257291820180309143018907";
            string mobileno = "9834545437";
            string billvalue = "1780";
            string CustomerName = "Tushar";            
            string token = "storeid=" + storeid;
            token += "&mobileno=" + mobileno;
            token += "&billvalue=" + billvalue;
            token += "&billGUID=" + billGUID;            
            token += "&CustomerName=" + CustomerName;
            string entoken = common.EncryptString(token);
            string url = BaseUrl + "?token=" + entoken;
            ViewBag.URL = url;
            return View();
        }

        [HttpPost]
        public ActionResult RedeemCoupon(string Coupon, string InvoiceAmt, string MobileNo, string StoreId, string BillGUID)
        {
            BurnValidateResponse ObjResponse = new BurnValidateResponse();
            try
            {
                ObjResponse = GRR.BurnCouponValidation(StoreId, Coupon, InvoiceAmt, MobileNo, BillGUID);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BurnValidation");
            }

            return new JsonResult() { Data = ObjResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}