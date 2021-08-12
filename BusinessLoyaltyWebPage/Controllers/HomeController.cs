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
//using BOTS_BL;

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
            var sender = "BLUEOC";
            var Url = "https://http.myvfirst.com/smpp/sendsms?";            
            string MobileMessage = "Dear" + CustomerName + ",Thanks for sharing your information. One of our representative will call you to take your on your Loyalty Journey - Blue Ocktopus";
            //Dear {#var#}, Thanks for sharing your information. One of our representative will call you to take your on your Loyalty Journey - Blue Ocktopus

            SendSucessMessage(MobileNo, sender, MobileMessage, Url);
            var result = "true";          
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void SendSucessMessage(string MobileNo, string Sender, string MobileMessage, string Url)
        {
            try
            {
                var UserName = System.Configuration.ConfigurationManager.AppSettings["SMSUserID"];
                var Password = System.Configuration.ConfigurationManager.AppSettings["SMSPassword"];

                MobileMessage = HttpUtility.UrlEncode(MobileMessage);
                string type1 = "TEXT";
                StringBuilder sbposdata1 = new StringBuilder();
                sbposdata1.AppendFormat("username={0}", UserName);
                sbposdata1.AppendFormat("&password={0}", Password);
                sbposdata1.AppendFormat("&to={0}", MobileNo);
                sbposdata1.AppendFormat("&from={0}", Sender);
                sbposdata1.AppendFormat("&text={0}", MobileMessage);
                sbposdata1.AppendFormat("&dlr-mask={0}", "19");
                sbposdata1.AppendFormat("&dlr-url");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(Url);
                UTF8Encoding encoding1 = new UTF8Encoding();
                byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
                httpWReq1.Method = "POST";
                httpWReq1.ContentType = "application/x-www-form-urlencoded";
                httpWReq1.ContentLength = data1.Length;
                using (Stream stream1 = httpWReq1.GetRequestStream())
                {
                    stream1.Write(data1, 0, data1.Length);
                }
                HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
                StreamReader reader1 = new StreamReader(response1.GetResponseStream());
                string responseString1 = reader1.ReadToEnd();
                reader1.Close();
                response1.Close();
            }
            catch (Exception ex)
            {
                //newexception.AddException(ex, "bulkuploadtest");
            }

        }
        //string mobileNo, string BusinessNm, string CustomerName, string Location,
        [HttpPost]
        public ActionResult SendEmailAndSMS( string jsonData)
        { 
            var result = "";
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            string mobileNo = "";
            string BusinessNm = "";
            string CustomerName = "";
            string Location = "";
            
            foreach (Dictionary<string, object> item in objData)
            {
                mobileNo = Convert.ToString(item["MobileNo"]);
                BusinessNm = Convert.ToString(item["Businessname"]);
                CustomerName = Convert.ToString(item["Name"]);
                Location = Convert.ToString(item["BusiLocation"]);
            }
            try
            {
                string from = "report@blueocktopus.in";
                 string To = System.Configuration.ConfigurationManager.AppSettings["emailId"];
               // string To = "ashlesha@blueocktopus.in";
                using (MailMessage mail = new MailMessage(from, To))
                {
                    StringBuilder str = new StringBuilder();                   
                    str.AppendLine("Dear Sir,");
                    str.AppendLine();
                    str.AppendLine("Please find following Exhibition Data");
                    str.AppendLine();
                    str.AppendLine(" Name:" + CustomerName + "" );  
                    str.AppendLine("Business Name: " + BusinessNm + "");                    
                    str.AppendLine("Mobile No:" + mobileNo + "");
                    str.AppendLine("Location:" + Location + "");                   
                    str.AppendLine();
                    str.AppendLine("Regards,");                   
                    str.AppendLine(" - Blue Ocktopus Team");
                    

                    mail.Subject = "Jewellery Exhibition Lead-" + BusinessNm;
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