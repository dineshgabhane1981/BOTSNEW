using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BOTS_BL.Repository;

namespace BusinessLoyaltyWebPage.Controllers
{
    public class HomeController : Controller
    {
       // Exceptions newexception = new Exceptions();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SendMsg(string MobileNo,string CustomerName)
        {
            var Token = "6155a8ef61ecb64aafdb34b4";
            var Url = "https://bs.enotify.app/api/sendText?";       
            string MobileMessage = "Dear " + CustomerName + ",Thanks for sharing your information.One of our representative will call you to take you on your fanchisee Journey - Caramella's http://caramellas.in/";

            SendWhatsAppMessage(MobileNo, MobileMessage, Token,Url);
            var result = "true";          
            return Json(result, JsonRequestBehavior.AllowGet);
        }      

        public void SendWhatsAppMessage(string MobileNo, string MobileMessage, string Token, string _Url)
        {
            string responseString;
            try
            {
                MobileMessage = MobileMessage.Replace("#99", "&");
                MobileMessage = HttpUtility.UrlEncode(MobileMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_Url);
                sbposdata.AppendFormat("token={0}", Token);
                sbposdata.AppendFormat("&phone=91{0}", MobileNo);
                sbposdata.AppendFormat("&message={0}", MobileMessage);

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
                reader.Close();
                response.Close();
            }
            catch (ArgumentException ex)
            {
                //Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                //_job.Start();
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                //Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                //_job.Start();
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                //Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                //_job.Start();
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }
               
        [HttpPost]
        public ActionResult SendEmailAndSMS( string jsonData)
        { 
            var result = "";
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            string mobileNo = "";            
            string CustomerName = "";
            string AreaName = "";

            foreach (Dictionary<string, object> item in objData)
            {
                mobileNo = Convert.ToString(item["MobileNo"]);               
                CustomerName = Convert.ToString(item["Name"]);
                AreaName = Convert.ToString(item["Area"]);
            }
            try
            {
                CustomerRepository CR = new CustomerRepository();
                var status = CR.AddFranchiseeEnquiry(mobileNo, CustomerName, AreaName);

                string from = "report@blueocktopus.in";
                 string To = System.Configuration.ConfigurationManager.AppSettings["emailId"];
               // string To = "ashlesha@blueocktopus.in";
                using (MailMessage mail = new MailMessage(from, To))
                {
                    StringBuilder str = new StringBuilder();                   
                    str.AppendLine("Dear Sir,");
                    str.AppendLine();
                    str.AppendLine("Please find following Franchisee Enquiry");
                    str.AppendLine();
                    str.AppendLine("Name:" + CustomerName + "" );                                        
                    str.AppendLine("Mobile No:" + mobileNo + "");
                    str.AppendLine("Area of Franchisee :" + AreaName + "");
                    str.AppendLine();
                    str.AppendLine("Regards,");                   
                    str.AppendLine(" - Caramella Team");                    

                    mail.Subject = "News Franchisee Lead Through QR-";
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.zoho.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from, "Report@123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                     var resultsms = SendMsg(mobileNo, CustomerName);
                    result = "True";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }            
    
    }
}