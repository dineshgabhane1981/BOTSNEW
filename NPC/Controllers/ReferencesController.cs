using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace NPC.Controllers
{
    public class ReferencesController : Controller
    {
        ReferencesRepository RR = new ReferencesRepository();
        // GET: References
        public ActionResult Index(string mobileno)
        {
            tblReference objData = new tblReference();

            objData.RefereeMobileNo = mobileno;
            return View(objData);
        }
        public ActionResult SaveReferee(tblReference objRefer)
        {
            var status = RR.InsertReferal(objRefer);
            if (status)
            {
                TempData["Message"] = "Success";

                var from = ConfigurationManager.AppSettings["FrmEmailOnboarding"].ToString();
                var PWD = ConfigurationManager.AppSettings["FrmEmailOnboardingPwd"].ToString();
                MailMessage mail = new MailMessage();
                MailAddress fromMail = new MailAddress(from);
                mail.From = fromMail;

                mail.To.Add("customersuccess@blueocktopus.in");
                mail.CC.Add("dinesh@blueocktopus.in");
                mail.CC.Add("jacqueline@blueocktopus.in");
                mail.CC.Add("punamchandra@blueocktopus.in");
                mail.CC.Add("mahavir@blueocktopus.in");
                
                StringBuilder sb = new StringBuilder();
                sb.Append("We got Customer Referral.");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Customer Name : <b>" + objRefer.ReferredName + "</b>");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Customer Mobile : <b>" + objRefer.ReferredMobileNo + "</b>");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Referred by : <b>" + objRefer.RefereeMobileNo + "</b>");

                //mail.From = from;
                mail.Subject = "Customer Referral";// + GroupDetails.GroupName;
                mail.SubjectEncoding = System.Text.Encoding.Default;
                mail.Body = sb.ToString();
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.zoho.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from, PWD);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
            }
            else
                TempData["Message"] = "Failed";
            objRefer.ReferredMobileNo = "";
            objRefer.ReferredName = "";
            objRefer.RetailName = "";

            return RedirectToAction("Index", new { mobileno = objRefer.RefereeMobileNo });
        }
    }
}