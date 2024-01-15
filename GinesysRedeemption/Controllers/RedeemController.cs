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
    public class RedeemController : Controller
    {
        CommonFunctions common = new CommonFunctions();
        GinesysRedeemptionRepository GRR = new GinesysRedeemptionRepository();
        Exceptions newexception = new Exceptions();
        // GET: Redeem
        public ActionResult Index(string token)
        {            
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
                    if (item.Contains("billGUID"))
                    {
                        var billGUIDParam = item.Split('=');
                        objRedeemData.billGUID = billGUIDParam[1];
                    }
                    if (item.Contains("PointValue"))
                    {
                        var billPointsValueParam = item.Split('=');
                        objRedeemData.PointsValue = billPointsValueParam[1];
                    }
                    if (item.Contains("Points"))
                    {
                        var billPointsParam = item.Split('=');
                        objRedeemData.Points =  billPointsParam[1];
                    }
                    
                    if (item.Contains("PointToRedeem"))
                    {
                        var billPointsToRedeemParam = item.Split('=');
                        objRedeemData.PointsToRedeem = billPointsToRedeemParam[1];
                    }
                    if (item.Contains("CustomerName"))
                    {
                        var billCustomerNameParam = item.Split('=');
                        objRedeemData.CustomerName = billCustomerNameParam[1];
                    }
                }
                //objRedeemData = GRR.GetCustomerDetails(objRedeemData);

            }
            return View(objRedeemData);
        }
        public ActionResult GetURL()
        {
            var BaseUrl = ConfigurationManager.AppSettings["RedeemBaseUrl"];
            string storeid = "1063888888";
            string billGUID = "D4E8010B-7164-46ED-8F3D-43F8B257291820180309143018907";
            string mobileno = "7709303625";
            string billvalue = "1780";
            string CustomerName = "Dinesh";
            decimal? Points = 10;
            decimal? PointsValue = 1;
            decimal? PointsToRedeem = 10;
            string token = "storeid=" + storeid;
            token += "&mobileno=" + mobileno;
            token += "&billvalue=" + billvalue;
            token += "&billGUID=" + billGUID;
            token += "&Points=" + Points;
            token += "&PointsValue=" + PointsValue;
            token += "&PointsToRedeem=" + PointsToRedeem;
            token += "&CustomerName=" + CustomerName;
            string entoken = common.EncryptString(token);
            string url = BaseUrl + "?token=" + entoken;
            ViewBag.URL = url;
            return View();
        }

        [HttpPost]
        public ActionResult BurnValidation(string Points,string InvoiceAmt,string MobileNo,string StoreId,string BillGUID)
        {
            BurnValidateResponse ObjResponse = new BurnValidateResponse();           
            try 
            {
                ObjResponse = GRR.BurnValidation(StoreId, Points, InvoiceAmt, MobileNo, BillGUID);
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "BurnValidation");
            }

            return new JsonResult() { Data = ObjResponse, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}