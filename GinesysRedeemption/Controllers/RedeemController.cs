using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GinesysRedeemption.App_Start;
using BOTS_BL.Models;
using BOTS_BL.Repository;

namespace GinesysRedeemption.Controllers
{
    public class RedeemController : Controller
    {
        CommonFunctions common = new CommonFunctions();
        GinesysRedeemptionRepository GRR = new GinesysRedeemptionRepository();
        // GET: Redeem
        public ActionResult Index(string token)
        {
            string storeid = string.Empty;
            string mobileno = string.Empty;
            string billvalue = string.Empty;
            CommonFunctions common = new CommonFunctions();
            GinesysRedeemModel objRedeemData = new GinesysRedeemModel();
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
                }
                objRedeemData = GRR.GetCustomerDetails(objRedeemData);

            }
            return View(objRedeemData);
        }
        public ActionResult GetURL()
        {
            var BaseUrl = ConfigurationManager.AppSettings["RedeemBaseUrl"];
            string storeid = "8888888888";
            string mobileno = "9003552567";
            string billvalue = "2000";
            string token = "storeid=" + storeid;
            token += "&mobileno=" + mobileno;
            token += "&billvalue=" + billvalue;
            string entoken = common.EncryptString(token);
            string url = BaseUrl + "?token=" + entoken;
            ViewBag.URL = url;
            return View();
        }
    }
}