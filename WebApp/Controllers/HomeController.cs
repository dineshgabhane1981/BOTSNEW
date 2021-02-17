using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using Newtonsoft.Json;
using WebApp.ViewModel;
using System.Globalization;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        DashboardRepository DR = new DashboardRepository();
        ReportsRepository RR = new ReportsRepository();
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ExecutiveSummary dataDashboard = new ExecutiveSummary();
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);            
            ViewBag.OutletList = lstOutlet;

            return View(dataDashboard);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult GetMemberSegmentResult(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            DashboardMemberSegment dataMemberSegment = new DashboardMemberSegment();
            dataMemberSegment = DR.GetDashboardMemberSegmentData(userDetails.GroupId, OutletId, userDetails.connectionString);

            List<long> dataList = new List<long>();
            
            dataList.Add(dataMemberSegment.NoofMember_Total);
            dataList.Add(dataMemberSegment.NoofMember_Repeat);
            dataList.Add(dataMemberSegment.NoofMember_NeverRedeem);
            dataList.Add(dataMemberSegment.NoofMember_RecentlyEnrolled);
            dataList.Add(dataMemberSegment.NoofMember_OnlyOnce);
            var lstData = string.Join(" ", dataList);
           
            //lstMember = RR.GetMemberList(SearchText);
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetMemberSegmentTxnResult(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<DashboardMemberSegmentTxn> dataMemberSegmentTxn = new List<DashboardMemberSegmentTxn>();
            dataMemberSegmentTxn = DR.GetDashboardMemberSegmentTxnData(userDetails.GroupId, OutletId, userDetails.connectionString);            
            return new JsonResult() { Data = dataMemberSegmentTxn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetOutletEnrolmentResult(string monthFlag)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<DashboardOutletEnrolment> dataOutletEnrolment = new List<DashboardOutletEnrolment>();
            dataOutletEnrolment = DR.GetDashboardOutletEnrolmentData(userDetails.GroupId, monthFlag, userDetails.connectionString);
            List<string> nameList = new List<string>();
            List<long> dataList = new List<long>();
            foreach (var item in dataOutletEnrolment)
            {
                nameList.Add(item.OutletName);
                dataList.Add(item.EnrollmentCount);
            }
            List<object> lstData = new List<object>();
            lstData.Add(nameList);
            lstData.Add(dataList);
            
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetPointsSummaryResult(string monthFlag)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            DashboardPointsSummary dataPointsSummary = new DashboardPointsSummary();
            dataPointsSummary = DR.GetDashboardPointsSummaryData(userDetails.GroupId, monthFlag, userDetails.connectionString);

            List<long> dataList = new List<long>();
            dataList.Add(dataPointsSummary.PointsIssued);
            dataList.Add(dataPointsSummary.PointsRedeemed);
            dataList.Add(dataPointsSummary.PointsExpired);
            dataList.Add(dataPointsSummary.PointsCancelled);
            dataList.Add(dataPointsSummary.PointsBalance);
            var lstData = string.Join(" ", dataList);

            //lstMember = RR.GetMemberList(SearchText);
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetMemberWebPageResult(string profileFlag)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            DashboardMemberWebPage dataMemberWebPage = new DashboardMemberWebPage();
            dataMemberWebPage = DR.GetDashboardMemberWebPageData(userDetails.GroupId, profileFlag, userDetails.connectionString);

            List<long> dataList = new List<long>();
            dataList.Add(dataMemberWebPage.MemberBase);
            dataList.Add(dataMemberWebPage.ReferringBase);
            dataList.Add(dataMemberWebPage.ReferralGenerated);
            dataList.Add(dataMemberWebPage.ReferralTransacted);
            dataList.Add(dataMemberWebPage.ReferralTxnCount);
            dataList.Add(dataMemberWebPage.BusinessGenerated);
            dataList.Add(dataMemberWebPage.ProfileUpdatedCount);
            var lstData = string.Join(" ", dataList);

            //lstMember = RR.GetMemberList(SearchText);
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendOTP(string emailId)
        {
            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");

            var status = DR.InsertOTP(emailId, Convert.ToInt32(sixDigitNumber));

            var senderEmail = new MailAddress("dgabhane@gmail.com", "Dinesh G");
            var receiverEmail = new MailAddress(emailId, "Receiver");
            var password = "Dinesh1981";
            var subject = "Your OTP is here";
            var body = "Your OTP is - " + sixDigitNumber;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            status = true;
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult VerifyOTP(string emailId, string OTP)
        {
            var status = DR.VerifyOTP(emailId, Convert.ToInt32(OTP));
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult ResetNewPassword(string emailId, string newPassword)
        {            
            var status = DR.ResetPassword(emailId, newPassword);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult UpdatePassword(string newPassword)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var status = DR.UpdatePassword(userDetails.LoginId, newPassword);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}