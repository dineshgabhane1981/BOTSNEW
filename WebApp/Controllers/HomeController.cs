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
using BOTS_BL;
using System.Text;
using System.IO;
using WebApp.App_Start;
using Rotativa;


namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        DashboardRepository DR = new DashboardRepository();
        ReportsRepository RR = new ReportsRepository();
        Exceptions newexception = new Exceptions();
        public ActionResult Index()
        {
            ExecutiveSummary dataDashboard = new ExecutiveSummary();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                //var lstExecutiveSummData = DR.GetExecutiveSummaryAllData(userDetails.GroupId, userDetails.connectionString);
                var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
                dataDashboard = DR.GetDashboardData(userDetails.GroupId, userDetails.connectionString, userDetails.LoginId, "", "");
                userDetails.IsFeedback = CR.GetIsFeedback(userDetails.GroupId);
                userDetails.IsEvent = CR.GetIsEvent(userDetails.GroupId);
                userDetails.IsCoupon = CR.GetIsCoupon(userDetails.GroupId);
                Session["UserSession"] = userDetails;
                ViewBag.OutletList = lstOutlet;
                ViewBag.OutletCount = lstOutlet.Count;
                dataDashboard.StartDate = DR.GetStartDate(userDetails.connectionString);
                dataDashboard.ToDate = DateTime.Now.ToString("MM-dd-yyyy");
                Session["buttons"] = "Dashboard";

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Index");
            }
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
        public JsonResult GetDashboardDataFiltered(string frmDate, string toDate)
        {
            ExecutiveSummary dataDashboard = new ExecutiveSummary();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                dataDashboard = DR.GetDashboardData(userDetails.GroupId, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardDataFiltered");
            }
            //lstMember = RR.GetMemberList(SearchText);
            return new JsonResult() { Data = dataDashboard, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetMemberSegmentResult(string OutletId, string frmDate, string toDate)
        {
            List<object> lstData = new List<object>();
            List<long> dataList = new List<long>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                List<string> lstDates = new List<string>();

                DashboardMemberSegment dataMemberSegment = new DashboardMemberSegment();
                dataMemberSegment = DR.GetDashboardMemberSegmentData(userDetails.GroupId, OutletId, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);

                if (dataMemberSegment != null)
                {
                    dataList.Add(dataMemberSegment.TotalMember);
                    dataList.Add(dataMemberSegment.RepeatMember);
                    dataList.Add(dataMemberSegment.NeverRedeem);
                    dataList.Add(dataMemberSegment.RecentlyEnrolled);
                    dataList.Add(dataMemberSegment.OnlyOnce);
                    dataList.Add(dataMemberSegment.NonTransacted);
                    lstDates.Add(dataMemberSegment.FromDate);
                    lstDates.Add(dataMemberSegment.ToDate);
                    lstData.Add(dataList);
                    lstData.Add(lstDates);
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberSegmentResult");
            }
            //lstMember = RR.GetMemberList(SearchText);
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetSharedBizResult(string OutletId, string frmDate, string toDate)
        {
            List<object> lstData = new List<object>();
            List<string> nameList = new List<string>();
            List<long> firstList = new List<long>();
            List<long> repeatList = new List<long>();
            List<long> redeemList = new List<long>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                List<DashboardBizShared> lstBizShared = new List<DashboardBizShared>();
                lstBizShared = DR.GetDashboardBizShared(userDetails.GroupId, OutletId, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);
                lstBizShared.Reverse();
                foreach (var item in lstBizShared)
                {
                    nameList.Add(item.MonthYear);
                    firstList.Add(Convert.ToInt64(item.FirstMemberTxn));
                    repeatList.Add(Convert.ToInt64(item.RepeatMemberTxn));
                    redeemList.Add(Convert.ToInt64(item.RedeemTxn));
                }
                lstData.Add(nameList);
                lstData.Add(firstList);
                lstData.Add(repeatList);
                lstData.Add(redeemList);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSharedBizResult");
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetMemberSegmentTxnResult(string OutletId, string frmDate, string toDate)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<DashboardMemberSegmentTxn> dataMemberSegmentTxn = new List<DashboardMemberSegmentTxn>();
            try
            {

                dataMemberSegmentTxn = DR.GetDashboardMemberSegmentTxnData(userDetails.GroupId, OutletId, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberSegmentTxnResult");
            }
            return new JsonResult() { Data = dataMemberSegmentTxn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetOutletEnrolmentResult(string monthFlag, string frmDate, string toDate)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<object> lstData = new List<object>();
            try
            {

                List<DashboardOutletEnrolment> dataOutletEnrolment = new List<DashboardOutletEnrolment>();
                dataOutletEnrolment = DR.GetDashboardOutletEnrolmentData(userDetails.GroupId, monthFlag, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);
                List<string> nameList = new List<string>();
                List<long> dataList = new List<long>();
                foreach (var item in dataOutletEnrolment)
                {
                    if (!item.OutletName.ToLower().Contains("admin"))
                    {
                        nameList.Add(item.OutletName);
                        dataList.Add(item.EnrollmentCount);
                    }
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletEnrolmentResult");
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetPointsSummaryResult(string monthFlag, string frmDate, string toDate)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<long> dataList = new List<long>();
            try
            {
                string loginId = string.Empty;
                if (userDetails.LevelIndicator == "03" || userDetails.LevelIndicator == "04")
                {
                    loginId = userDetails.OutletOrBrandId;
                }
                DashboardPointsSummary dataPointsSummary = new DashboardPointsSummary();
                dataPointsSummary = DR.GetDashboardPointsSummaryData(userDetails.GroupId, monthFlag, userDetails.connectionString, loginId, frmDate, toDate);

                dataList.Add(dataPointsSummary.PointsIssued);
                dataList.Add(dataPointsSummary.PointsRedeemed);
                dataList.Add(dataPointsSummary.PointsExpired);
                dataList.Add(dataPointsSummary.PointsCancelled);
                dataList.Add(dataPointsSummary.PointsBalance);
                var lstData = string.Join(" ", dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsSummaryResult");
            }
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetMemberWebPageResult(string profileFlag, string frmDate, string toDate)
        {
            List<long> dataList = new List<long>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {

                DashboardMemberWebPage dataMemberWebPage = new DashboardMemberWebPage();
                dataMemberWebPage = DR.GetDashboardMemberWebPageData(userDetails.GroupId, profileFlag, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);

                dataList.Add(dataMemberWebPage.MemberBase);
                dataList.Add(dataMemberWebPage.ReferringBase);
                dataList.Add(dataMemberWebPage.ReferralGenerated);
                dataList.Add(dataMemberWebPage.ReferralTransacted);
                dataList.Add(dataMemberWebPage.ReferralTxnCount);
                dataList.Add(dataMemberWebPage.BusinessGenerated);
                dataList.Add(dataMemberWebPage.ProfileUpdatedCount);

                if (dataMemberWebPage.MWPStatus == "No")
                {
                    dataMemberWebPage.MWPStatusCode = 1;
                }
                else
                {
                    dataMemberWebPage.MWPStatusCode = 0;
                }
                dataList.Add(dataMemberWebPage.MWPStatusCode);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberWebPageResult");
            }
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


        public JsonResult GetBulkUploadResult(string frmDate, string toDate)
        {
            List<object> dataList = new List<object>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {

                DashboardBulkUpload objDashboardBulkUpload = new DashboardBulkUpload();
                objDashboardBulkUpload = DR.GetDashboardBulkUpload(userDetails.GroupId, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);
                if (objDashboardBulkUpload != null)
                {
                    dataList.Add(objDashboardBulkUpload.TotalUpload);
                    dataList.Add(objDashboardBulkUpload.UniqueTransacted);
                    dataList.Add(objDashboardBulkUpload.TransactedCount);
                    dataList.Add(objDashboardBulkUpload.BusinessGenerated);
                    dataList.Add(objDashboardBulkUpload.PieChartYellow);
                    dataList.Add(objDashboardBulkUpload.PieChartGreen);
                    dataList.Add(objDashboardBulkUpload.PieChartTotalMemberConverted);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBulkUploadResult");
            }
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetRedemptionResult(string type, string frmDate, string toDate)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<object> dataList = new List<object>();
            try
            {

                DashboardRedemption objDashboardRedemption = new DashboardRedemption();
                objDashboardRedemption = DR.GetDashboardRedemption(userDetails.GroupId, type, userDetails.connectionString, userDetails.LoginId, frmDate, toDate);
                if (objDashboardRedemption != null)
                {
                    dataList.Add(objDashboardRedemption.RedeemedMembers);
                    dataList.Add(objDashboardRedemption.RedemptionTxnCount);
                    dataList.Add(objDashboardRedemption.RedeemedPoints);
                    dataList.Add(objDashboardRedemption.PointsValueRs);
                    dataList.Add(objDashboardRedemption.BusinessGenerated);
                    dataList.Add(objDashboardRedemption.RedeemToInvoice);
                    var remaining = 100 - objDashboardRedemption.PieChartGreenRedemptionRate;
                    dataList.Add(remaining);
                    dataList.Add(objDashboardRedemption.PieChartGreenRedemptionRate);
                    var remaining1 = 100 - objDashboardRedemption.PieChartGreenUniqueMember;
                    dataList.Add(remaining1);
                    dataList.Add(objDashboardRedemption.PieChartGreenUniqueMember);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRedemptionResult");
            }
            return new JsonResult() { Data = dataList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
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
        public bool SendOTP(string emailId)
        {
            bool status = false;
            try
            {
                Random r = new Random();
                int randNum = r.Next(1000000);
                string sixDigitNumber = randNum.ToString("D6");

                var OTPstatus = DR.InsertOTP(emailId, Convert.ToInt32(sixDigitNumber));
                var _MobileMessage = "Dear Member, " + Convert.ToInt32(sixDigitNumber) + "  is your OTP. Sample SMS for OTP - Blue Ocktopus ";
                var _UserName = "blueohttpotp";
                var _Password = "bluoct87";
                var _MobileNo = emailId;
                var _Sender = "BLUEOC";
                var _Url = "http://sms.visionhlt.com:8080/api/mt/SendSMS";
                var _SMSAPIKey = "iK474VMKbkCvi2u8ppclXg";

                status = SendSMS(_MobileMessage, _UserName, _Password, _MobileNo, _Sender, _Url, _SMSAPIKey);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendOTP");
            }
            return status;
            //return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public bool SendSMS(string _MobileMessage, string _UserName, string _Password, string _MobileNo, string _Sender, string _Url, string _SMSAPIKey)
        {
            bool status = false;
            try
            {

                var httpWebRequest_00003 = (HttpWebRequest)WebRequest.Create(_Url);
                httpWebRequest_00003.ContentType = "application/json";
                httpWebRequest_00003.Method = "POST";

                using (var streamWriter_00003 = new StreamWriter(httpWebRequest_00003.GetRequestStream()))
                {

                    string json_00003 = "{\"Account\":" +
                                    "{\"APIKey\":\"" + _SMSAPIKey + "\"," +
                                    "\"SenderId\":\"" + _Sender + "\"," +
                                    "\"Channel\":\"Trans\"," +
                                    "\"DCS\":\"0\"," +
                                    "\"SchedTime\":null," +
                                    "\"GroupId\":null}," +
                                    "\"Messages\":[{\"Number\":\"" + _MobileNo + "\"," +
                                    "\"Text\":\"" + _MobileMessage + "\"}]" +
                                    "}";
                    streamWriter_00003.Write(json_00003);
                }

                var httpResponse_00003 = (HttpWebResponse)httpWebRequest_00003.GetResponse();
                using (var streamReader_00003 = new StreamReader(httpResponse_00003.GetResponseStream()))
                {
                    var result_00003 = streamReader_00003.ReadToEnd();
                }

                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendSMS");
            }
            return status;
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


        public ActionResult ExitDashboard()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            userDetails.GroupId = null;
            Session["UserSession"] = userDetails;
            return RedirectToAction("Index", "CustomerManagement");
        }

        public string GenerateReports()
        {
            string status = "Report Sent";
            try
            {
                var AllCustomer = CR.GetAllCustomer("", "");
                //GeneratePDF("1051");
                foreach (var customer in AllCustomer)
                {
                    SavePDF(Convert.ToString(customer.GroupId));
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GenerateReports");
            }
            return status;
        }
        public ActionResult GeneratePDF(string groupId)
        {
            DashboardSummaryViewModel objData = new DashboardSummaryViewModel();
            try
            {
                objData.CustomerName = CR.GetCustomerName(groupId);
                objData.CustomerLogoURL = CR.GetCustomerLogo(groupId);
                objData.ReportMonth = DateTime.Now.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture) + " - " + DateTime.Now.AddMonths(-1).Year.ToString();
                objData.lstMemberBaseAndTransaction = CR.GetMemberBaseAndTransactions(groupId);
                var connectionString = CR.GetCustomerConnString(groupId);
                var dataDashboard = DR.GetDashboardData(groupId, connectionString, "", "", "");
                TotalStats objStats = new TotalStats();
                objStats.TotalBiz = dataDashboard.TotalBiz;
                objStats.LoyaltyBiz = dataDashboard.LoyaltyBiz;
                objStats.LoyaltyPercentage = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(Convert.ToDouble(dataDashboard.LoyaltyBiz * 100) / Convert.ToDouble(dataDashboard.TotalBiz)));
                objData.objTotalStats = objStats;

                objData.objKeyMetricsTillDate = CR.GetKeyMetrics(groupId);
                objData.lstKeyInfoForNextMonth = CR.GetKeyInfoForNextMonth(groupId);
                objData.lstFestivals = CR.GetFestivalDates();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GeneratePDF");
            }

            return View(objData);
        }
        public void SavePDF(string groupId)
        {
            try
            {
                DashboardSummaryViewModel objData = new DashboardSummaryViewModel();
                objData.CustomerName = CR.GetCustomerName(groupId);
                objData.CustomerLogoURL = CR.GetCustomerLogo(groupId);
                objData.ReportMonth = DateTime.Now.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture) + "_" + DateTime.Now.AddMonths(-1).Year.ToString();
                objData.lstMemberBaseAndTransaction = CR.GetMemberBaseAndTransactions(groupId);

                var conStr = CR.GetCustomerConnString(groupId);
                var dataMemberSegment = DR.GetDashboardMemberSegmentData(groupId, "", conStr, "", "", "");

                objData.lstMemberBaseAndTransaction[1].BaseCount = Convert.ToInt32(dataMemberSegment.TotalMember);

                var connectionString = CR.GetCustomerConnString(groupId);
                var dataDashboard = DR.GetDashboardData(groupId, connectionString, "", "", "");
                TotalStats objStats = new TotalStats();
                objStats.TotalBiz = dataDashboard.TotalBiz;
                objStats.LoyaltyBiz = dataDashboard.LoyaltyBiz;
                objStats.LoyaltyPercentage = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(Convert.ToDouble(dataDashboard.LoyaltyBiz * 100) / Convert.ToDouble(dataDashboard.TotalBiz)));
                objData.objTotalStats = objStats;

                objData.objKeyMetricsTillDate = CR.GetKeyMetrics(groupId);
                objData.lstKeyInfoForNextMonth = CR.GetKeyInfoForNextMonth(groupId);
                objData.lstFestivals = CR.GetFestivalDates();

                var a = new ViewAsPdf();
                a.ViewName = "GeneratePDF";
                a.Model = objData;

                var pdfBytes = a.BuildFile(this.ControllerContext);

                // Optionally save the PDF to server in a proper IIS location.
                var fileName = objData.CustomerName.Trim() + "_" + objData.ReportMonth + ".pdf";
                var path = Server.MapPath("~/DashboardReports/" + fileName);
                System.IO.File.WriteAllBytes(path, pdfBytes);
                var status = SendMessage(groupId, fileName);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SavePDF");
            }
        }

        public bool SendMessage(string GroupId, string fileName)
        {
            bool result = false;
            var WAGroupCode = CR.GetWAGroupCode(GroupId);
            string responseString;
            try
            {
                var path = "https://blueocktopus.in/bots/DashboardReports/" + fileName;
                //var path = "http://localhost:57265/DashboardReports/" + fileName;
                var Month = DateTime.Now.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture) + "_" + DateTime.Now.AddMonths(-1).Year.ToString();
                string _MobileMessage = "Dear Customer, Your *LOYALTY PROGRAM SYNOPSIS* for " + Month + " is attached";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendFileWithCaption?");
                sbposdata.AppendFormat("token={0}", "5fc8ed623629423c01ce4221");
                sbposdata.AppendFormat("&phone={0}", WAGroupCode);

                //sbposdata.AppendFormat("&phone=91{0}", "9834545425");
                sbposdata.AppendFormat("&link={0}", path);
                sbposdata.AppendFormat("&message={0}", _MobileMessage);
                string Url = sbposdata.ToString();
                //this.WriteToFile(Url);
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
                //this.WriteToFile(responseString);

                reader.Close();
                response.Close();
                result = true;
            }
            catch (ArgumentException ex)
            {

                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
                //this.WriteToFile(responseString);
            }
            catch (WebException ex)
            {
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                //this.WriteToFile(responseString);
            }
            catch (Exception ex)
            {
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                //this.WriteToFile(responseString);
            }
            return result;
        }
    }
}