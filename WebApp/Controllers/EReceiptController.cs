using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;
using System.Data;
using System.Web.Script.Serialization;
using WebApp.App_Start;
using WebApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace WebApp.Controllers
{
    public class EReceiptController : Controller
    {
        EReceiptRepository ERR = new EReceiptRepository();
        CommonFunctions common = new CommonFunctions();
        // GET: EReceipt
        public ActionResult GetReceiptLink()
        {
            var BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            string groupId = "1378";
            string invoiceNo = "Z7201009O7A00008";
            string token = "groupId=" + groupId;
            token += "&invoiceNo=" + invoiceNo;
            string entoken = common.EncryptString(token);
            string url = BaseUrl + "EReceipt/?data=" + entoken;
            ViewBag.URL = url;
            return View();
        }

        public ActionResult Index(string data)
        {
            string invoiceNo = string.Empty;
            string groupId = string.Empty;
            if (!string.IsNullOrEmpty(data))
            {
                var parameterStr = common.DecryptString(data);
                var parameters = parameterStr.Split('&');
                foreach (var item in parameters)
                {
                    if (item.Contains("groupId"))
                    {
                        var groupIdParam = item.Split('=');
                        groupId = groupIdParam[1];
                    }
                    if (item.Contains("invoiceNo"))
                    {
                        var invoiceNoParam = item.Split('=');
                        invoiceNo = invoiceNoParam[1];
                    }
                }
            }

            var objData = ERR.GetEReceiptJSON(invoiceNo, groupId);
            return View(objData);
        }

        public ActionResult ConfigureEReceipt()
        {
            EReceiptViewModel objData = new EReceiptViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData.objEReceiptConfig = ERR.GetEReceiptConfig(userDetails.connectionString);

            return View(objData);
        }
        [HttpPost]
        public JsonResult SaveConfig(string jsonData)
        {
            bool status = true;
            tblEReceiptConfig objConfig = new tblEReceiptConfig();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                objConfig.SlNo = Convert.ToInt32(item["SlNo"]);
                objConfig.BrandName = Convert.ToString(item["BrandName"]);
                objConfig.WebsiteURL = Convert.ToString(item["WebsiteURL"]);
                objConfig.ThankyouMessage = Convert.ToString(item["ThankyouMessage"]);
                objConfig.IsSocialMedia = Convert.ToBoolean(item["IsSocialMedia"]);
                objConfig.FacebookUrl = Convert.ToString(item["FacebookUrl"]);
                objConfig.TwitterUrl = Convert.ToString(item["TwitterUrl"]);
                objConfig.InstagramUrl = Convert.ToString(item["InstagramUrl"]);
                objConfig.WhatsappUrl = Convert.ToString(item["WhatsappUrl"]);                
                objConfig.TermsAndConditions = Convert.ToString(item["TermsAndConditions"]);
                objConfig.CustomerServiceEmail = Convert.ToString(item["CustomerServiceEmail"]);
                objConfig.CustomerServiceContact = Convert.ToString(item["CustomerServiceContact"]);
                objConfig.AddedBy = userDetails.LoginId;
                objConfig.UpdatedDate = DateTime.Now;
                status = ERR.SaveConfig(objConfig, userDetails.connectionString);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = System.Int32.MaxValue };
        }

    }
}