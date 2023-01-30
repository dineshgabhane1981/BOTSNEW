using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BOTS_BL.Models;
using BOTS_BL.Repository;

namespace Medration.Controllers
{
    public class SubscriptionController : Controller
    {
        MedrationRepository MR = new MedrationRepository();
        string razorpayKey = ConfigurationManager.AppSettings["razorpayKey"].ToString();
        string secretKey = ConfigurationManager.AppSettings["secretKey"].ToString();
        string plan1 = ConfigurationManager.AppSettings["Plan1"].ToString();
        string plan2 = ConfigurationManager.AppSettings["Plan2"].ToString();
        string plan3 = ConfigurationManager.AppSettings["Plan3"].ToString();
        string plan4 = ConfigurationManager.AppSettings["Plan4"].ToString();
        string discount = ConfigurationManager.AppSettings["Discount"].ToString();
        
        // GET: Subscription
        public ActionResult Index(string plan)
        {
            var NumberOfPerson = Convert.ToInt32(plan);
            if (NumberOfPerson == 1)
                ViewBag.PlanAmount = Convert.ToInt32(plan1);

            if (NumberOfPerson == 2)
                ViewBag.PlanAmount = Convert.ToInt32(plan2); 

            if (NumberOfPerson == 4)
                ViewBag.PlanAmount = Convert.ToInt32(plan3);

            if (NumberOfPerson == 6)
                ViewBag.PlanAmount = Convert.ToInt32(plan4);

            ViewBag.Discount = Convert.ToInt32(discount);
            ViewBag.TotalAmount = ViewBag.PlanAmount * (ViewBag.Discount) / 100;

            ViewBag.NumberOfPerson = NumberOfPerson;
            return View();
        }
        public ActionResult LoadDetails(string num)
        {
            var NumberOfPerson = Convert.ToInt32(num);
            ViewBag.NumberOfPerson = NumberOfPerson;
            return PartialView("_LoadDetailsForSubscription");
        }
        [HttpPost]
        public ActionResult CreateOrder(PaymentInitiateModel _requestData)
        {
            List<tblMedrationSubscription> lstSubscriptionData = new List<tblMedrationSubscription>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objPaymentData = (object[])json_serializer.DeserializeObject(_requestData.jsonData);
            
            foreach (Dictionary<string, object> item in objPaymentData)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(item["name"])) && !string.IsNullOrEmpty(Convert.ToString(item["contactNumber"])) && !string.IsNullOrEmpty(Convert.ToString(item["DOB"])))
                {
                    tblMedrationSubscription itemData = new tblMedrationSubscription();
                    itemData.CustomerName = Convert.ToString(item["name"]);
                    itemData.ContactNo = Convert.ToInt64(item["contactNumber"]);
                    itemData.Email = Convert.ToString(item["email"]);
                    itemData.DOBOriginal = Convert.ToDateTime(item["DOB"]);
                    itemData.IsPrimary = Convert.ToBoolean(item["IsPrimary"]);
                    itemData.PrimaryNo = Convert.ToInt64(_requestData.contactNumber);
                    itemData.PlanId = Convert.ToInt32(item["PlanId"]);
                    if (itemData.PlanId == 1)
                    {
                        itemData.PlanName = "Individual";
                    }
                    if (itemData.PlanId == 2)
                    {
                        itemData.PlanName = "Couple";
                    }
                    if (itemData.PlanId == 4)
                    {
                        itemData.PlanName = "Family";
                    }
                    if (itemData.PlanId == 6)
                    {
                        itemData.PlanName = "Family+";
                    }

                    lstSubscriptionData.Add(itemData);
                }              
            }

            Session["SubscriptionData"] = lstSubscriptionData;
            // Generate random receipt number for order
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(razorpayKey, secretKey);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", _requestData.amount * 100);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                                                 //options.Add("notes", "-- You can put any notes here --");
            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

            // Create order model for return on view
            OrderModel orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = razorpayKey,
                amount = _requestData.amount * 100,
                currency = "INR",
                name = _requestData.name,
                email = _requestData.email,
                contactNumber = _requestData.contactNumber,
                address = _requestData.address,
                description = "Testing description"
            };

            // Return on PaymentPage with Order data
            return View("PaymentPage", orderModel);
        }

        [HttpPost]
        public ActionResult Complete()
        {
            // Payment data comes in url so we have to get it from url

            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];

            // This is orderId
            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(razorpayKey, secretKey);

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];

            //// Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                var CustomerData = (List<tblMedrationSubscription>)Session["SubscriptionData"];
                var status = MR.AddCustomerDetails(CustomerData);
                var mobileNo = Convert.ToString(CustomerData[0].ContactNo);
                var customerName= Convert.ToString(CustomerData[0].CustomerName);
                var token = "63d105f88ddb3ac97d6e2391";
                var url = "https://bo.enotify.app/api/sendText?";
                var message = "Dear "+ customerName + ", Congratulations on your Annual Subscription of MedRation Membership Programme.";
                message += "Below are the subscription details :";
                message += "Type of Subscription :" + CustomerData[0].PlanName;
                message += "Total Members :" + CustomerData.Count;
                message += "(Names of the Members subscribed)";
                var index = 0;
                foreach(var item in CustomerData)
                {
                    index++;
                    message += index+") "+item.CustomerName;
                }
                message += "Validity :" + CustomerData[0].DateAdded.AddDays(365);


                SendWhatsTextOnly1(mobileNo, message, token, url);
                // Create these action method
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public void SendWhatsTextOnly1(string _MobileNo, string _Message, string _Token, string _Url)
        {
            string responseString;
            try
            {
                _Message = _Message.Replace("#99", "&");
                _Message = HttpUtility.UrlEncode(_Message);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_Url);
                sbposdata.AppendFormat("token={0}", _Token);
                sbposdata.AppendFormat("&phone=91{0}", _MobileNo);
                sbposdata.AppendFormat("&message={0}", _Message);
                string Url = sbposdata.ToString();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbposdata.ToString());
                httpWReq.Method = "POST";

                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                //var J = JObject.Parse(responseString);
                //string J1 = J["status"].ToString();
                //if (J1 == "error")
                //{
                //    Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                //    _job.Start();
                //}
                reader.Close();
                response.Close();
            }
            catch (ArgumentException ex)
            {

                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {

                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {

                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }
        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }

        public ActionResult PaymentPage(OrderModel orderModel)
        {
            return View("PaymentPage", orderModel);
        }

        public class OrderModel
        {
            public string orderId { get; set; }
            public string razorpayKey { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string contactNumber { get; set; }
            public string address { get; set; }
            public string description { get; set; }
        }
        public class PaymentInitiateModel
        {
            public string name { get; set; }
            public string email { get; set; }
            public string contactNumber { get; set; }
            public string address { get; set; }
            public int amount { get; set; }
            public int age { get; set; }
            public string jsonData { get; set; }
            public int planId { get; set; }
            public string planName { get; set; }
        }
    }
}