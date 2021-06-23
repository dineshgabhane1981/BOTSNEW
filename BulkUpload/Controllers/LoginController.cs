using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Mail;

namespace WebApp.Controllers
{
    
    public class LoginController : Controller
    {
        Exceptions newexception = new Exceptions();
        // GET: Login
        public ActionResult Index()
        {
            //LoginModel objLogin = new LoginModel();
            return View();
        }
        public ActionResult Home()
        {
            //LoginModel objLogin = new LoginModel();
            return View();
        }
        public JsonResult SendOTP(string MobileNo)
        {
            var sender = "BLUEOC";
            var Url = " https://http2.myvfirst.com/smpp/sendsms?";
            Random random = new Random();
            int otpnum = random.Next(1001,9999);
            string MobileMessage = "Dear Member,"+ otpnum + " is your OTP. Sample SMS for OTP - Blue Ocktopus";
            //string MobileMessage = "Your OTP for data upload is 1234 – Blue Your OTP for data upload is "+ otpnum + "– Blue OcktopusOcktopus";
            SendOTPMessage(MobileNo, sender, MobileMessage, Url);
            var result = "true";
            TempData["OTP"] = otpnum;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void SendOTPMessage(string MobileNo, string Sender, string MobileMessage, string Url)
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
            catch(Exception ex)
            {
                newexception.AddException(ex, "bulkuploadtest");
            }

        }
        
        public ActionResult HomePage(string OTPNO, string MobileNo)
        {

            var result = "";
            if (OTPNO == TempData["OTP"].ToString())
            {
                
                TempData["UserMoB"] = MobileNo;
                Session["mobileno"] = MobileNo;
                result = "Redirect";

            }
            else
            {
                result = "Fail";
            }
            return Json(new { result , url = Url.Action("Home", "Login") });
        }
        public JsonResult SendEmail(string RetailerNm, HttpPostedFileBase file)
        {
            var result = ""; 
            string from = "report@blueocktopus.in"; 
            string To = System.Configuration.ConfigurationManager.AppSettings["emailId"];
            using (MailMessage mail = new MailMessage(from, To))
            {

                string body = @"Dear Sir,

                     " + RetailerNm + " has uploaded the data using Mobile No-" + Session["mobileno"].ToString() + "\r\n  Please Find Attached document  " +

                     "\r\nRegards, - Blue Ocktopus Team";

                mail.Subject = "Bulk Uploaded Data-"+ RetailerNm;
                mail.Body = body;
                if (file != null)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    mail.Attachments.Add(new Attachment(file.InputStream, fileName));
                }
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.zoho.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from, "Report@123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
                // ViewBag.Message = "Sent";
                // return View("Index", objModelMail);
                result = "True";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}