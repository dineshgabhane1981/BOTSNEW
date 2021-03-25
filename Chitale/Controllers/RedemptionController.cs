using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Chitale.Controllers
{
    public class RedemptionController : Controller
    {
        RedemptionRepository RR = new RedemptionRepository();
        ChitaleException newexception = new ChitaleException();
        // GET: Redemption
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RedeemedList()
        {
            return View();
        }
        public ActionResult RedemptionValues()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetRedeemptionData(string type)
        {
            RedemptionValue objData = new RedemptionValue();
            objData = RR.GetRedeemptionData(type);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GenerateOTP(string jsonData)
        {
            bool status = false;
            try
            {
                List<GenerateOTPList> objOTPData = new List<GenerateOTPList>();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    string Type = Convert.ToString(item["Type"]);
                    string CashIncentive = Convert.ToString(item["CashIncentive"]);
                    string Infrastructure = Convert.ToString(item["Infrastructure"]);
                    string Deposit = Convert.ToString(item["Deposit"]);
                    string Promotion = Convert.ToString(item["Promotion"]);
                    objOTPData = RR.GenerateOTP(Type, CashIncentive, Infrastructure, Deposit, Promotion);
                }
                foreach(var item in objOTPData)
                {
                   
                    string _Url = item.Url;
                    string _MobileMessage = item.MessageText;
                    _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                    string type1 = "TEXT";
                    StringBuilder sbposdata1 = new StringBuilder();
                    sbposdata1.AppendFormat("username={0}", item.UserName);
                    sbposdata1.AppendFormat("&password={0}", item.Password);
                    sbposdata1.AppendFormat("&to={0}", item.MobileNo);
                    sbposdata1.AppendFormat("&from={0}", "CHiTLE");
                    sbposdata1.AppendFormat("&text={0}", _MobileMessage);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(_Url);
                    UTF8Encoding encoding1 = new UTF8Encoding();
                    byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
                    httpWReq1.Method = "POST";
                    httpWReq1.ContentType = "application/x-www-form-urlencoded";
                    httpWReq1.ContentLength = data1.Length;
                    using (Stream stream1 = httpWReq1.GetRequestStream())
                    {
                        stream1.Write(data1, 0, data1.Length);
                    }
                    //HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
                    //StreamReader reader1 = new StreamReader(response1.GetResponseStream());
                    //string responseString1 = reader1.ReadToEnd();
                    //reader1.Close();
                    //response1.Close();
                }
                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        [HttpPost]
        public JsonResult ValidateOTP(string jsonData)
        {
            ChitaleSPResponse objOTPData = new ChitaleSPResponse();            
            try
            {
                
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    string Type = Convert.ToString(item["Type"]);
                    string CashIncentive = Convert.ToString(item["CashIncentive"]);
                    string Infrastructure = Convert.ToString(item["Infrastructure"]);
                    string Deposit = Convert.ToString(item["Deposit"]);
                    string Promotion = Convert.ToString(item["Promotion"]);
                    string OTP1 = Convert.ToString(item["OTP1"]);
                    string OTP2 = Convert.ToString(item["OTP2"]);

                    objOTPData = RR.ValidateOTP(Type, CashIncentive, Infrastructure, Deposit, Promotion, OTP1, OTP2);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = objOTPData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


    }
}