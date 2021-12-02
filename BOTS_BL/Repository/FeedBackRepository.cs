using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using BOTS_BL;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;
using BOTS_BL.Models.SalesLead;
using BOTS_BL.Models.FeedBack;
using System.Web;
using System.Globalization;

namespace BOTS_BL.Repository
{
    public class FeedBackRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public List<SelectListItem> GetLocationList(string GroupId)
        {
            CustomerDetail objcustdetails = new CustomerDetail();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            List<SelectListItem> lstlocation = new List<SelectListItem>();
            using (var context = new BOTSDBContext(connStr))
            {
                var locationnm = context.LocationMasters.ToList().OrderBy(x => x.Location);

                foreach (var item in locationnm)
                {
                    lstlocation.Add(new SelectListItem
                    {
                        Text = item.Location,
                        Value = Convert.ToString(item.LocationId)
                    });
                }
            }
            return lstlocation;
        }
        public CustomerDetailwithFeedback GetCustomerInfo(string mobileNo, string GroupId)
        {
            CustomerDetail objcustdetails = new CustomerDetail();
            CustomerDetailwithFeedback obj = new CustomerDetailwithFeedback();
            FeedBackMaster objfeedback = new FeedBackMaster();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                objfeedback = context.FeedBackMasters.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.FeedBackId).FirstOrDefault();
                var customer = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                if (objfeedback != null && GroupId == "1163")
                {
                    obj.IsFeedBackGiven = true;
                }
                else
                {
                    if (objfeedback != null && objfeedback.DOJ.Value.Date == DateTime.Now.Date)
                    {
                        obj.IsFeedBackGiven = true;
                        if (customer != null)
                        {
                            obj.CustomerName = customer.CustomerName;
                            obj.MobileNo = customer.MobileNo;
                            obj.Points = customer.Points;
                        }
                    }
                    else
                    {
                        obj.IsFeedBackGiven = false;
                        if (customer != null)
                        {
                            obj.CustomerName = customer.CustomerName;
                            obj.MobileNo = customer.MobileNo;
                            obj.Points = customer.Points;
                        }
                    }
                }
            }
            return obj;
        }

        public bool SubmitRating(string mobileNo, int[] ranking, string GroupId, string outletId)
        {
            bool status = false;
            // string smsresponce = "";
            FeedBackMaster objfeedback = new FeedBackMaster();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);

            using (var context = new BOTSDBContext(connStr))
            {
                CustomerDetail objcustdetails = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                int queid = 1;
                int Combinedpoint = 0;
                foreach (int points in ranking)
                {
                    if (points != 0)
                    {
                        if (objcustdetails != null)
                        {
                            objfeedback.CustomerName = objcustdetails.CustomerName;
                        }
                        else
                        {
                            objfeedback.CustomerName = "Member";
                        }
                        objfeedback.MobileNo = mobileNo;
                        objfeedback.QuestionPoints = points.ToString();
                        objfeedback.QuestionId = queid.ToString();
                        objfeedback.OutletId = outletId;
                        objfeedback.DOJ = DateTime.Now;
                        Combinedpoint += points;
                        objfeedback = context.FeedBackMasters.Add(objfeedback);
                        context.SaveChanges();
                        status = true;
                    }
                    queid++;
                }
                if (Combinedpoint <= 4)
                {
                    SMSDetail objsmsdetails = new SMSDetail();
                    FeedBackMobileMaster objmobilemaster = context.FeedBackMobileMasters.Where(x => x.MessageId == "203").FirstOrDefault();
                    SMSEmailMaster objsmsemailmaster = context.SMSEmailMasters.Where(x => x.MessageId == "203").FirstOrDefault();
                    TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

                    string message = objsmsemailmaster.SMS;
                    if (objcustdetails != null)
                    {
                        message = message.Replace("#01", objcustdetails.CustomerName);
                    }
                    else
                    {
                        message = message.Replace("#01", "Member");
                    }

                    message = message.Replace("#30", mobileNo);
                    message = message.Replace("#08", Convert.ToString(date));
                    message = message.Replace("#31", Convert.ToString(ranking[0]));
                    message = message.Replace("#32", Convert.ToString(ranking[1]));

                    objsmsdetails = context.SMSDetails.Where(x => x.OutletId == outletId).FirstOrDefault();
                    SendMessage(objmobilemaster.MobileNo, objsmsdetails.SenderId, message, objsmsdetails.TxnUrl, objsmsdetails.TxnUserName, objsmsdetails.TxnPassword);

                }
            }
            return status;
        }

        public bool SubmitPoints(string MemberName, string Gender, string BirthDt, string mobileNo, string AnniversaryDt, string LiveIn, string Knowabt, string GroupId, string outletid)
        {
            bool status = false;
            // string smsresponce="";
            List<FeedBackMaster> lstfeedback = new List<FeedBackMaster>();
            TransactionMaster objtransactionMaster = new TransactionMaster();
            PointsExpiry objpointsExpiry = new PointsExpiry();
            CustomerDetail objnewcust = new CustomerDetail();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);

            using (var context = new BOTSDBContext(connStr))
            {
                CustomerDetail objcustdetails = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                OutletDetail objoutlet = context.OutletDetails.Where(x => x.OutletId == outletid).FirstOrDefault();
                FeedBackMaster objfeedback = new FeedBackMaster();
                lstfeedback = context.FeedBackMasters.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.DOJ).Take(2).ToList();
                if (objcustdetails != null)
                {
                    if (GroupId != "1163")
                    {
                        var point = objcustdetails.Points;
                        objcustdetails.Points = point + objoutlet.FeedBackPoints;
                        context.CustomerDetails.AddOrUpdate(objcustdetails);
                        context.SaveChanges();
                    }
                    else
                    {
                        if (AnniversaryDt != null)
                        {
                            objcustdetails.AnniversaryDate = Convert.ToDateTime(AnniversaryDt);
                        }
                        objcustdetails.CustomerName = MemberName;
                        context.CustomerDetails.AddOrUpdate(objcustdetails);
                        context.SaveChanges();
                    }
                }
                if (objcustdetails == null)
                {
                    var CustomerId = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                    DateTime datet = new DateTime(1900, 01, 01);
                    var NewId = Convert.ToInt64(CustomerId) + 1;
                    objnewcust.CustomerId = Convert.ToString(NewId);
                    objnewcust.Points = objoutlet.FeedBackPoints;
                    if (string.IsNullOrEmpty(MemberName))
                        objnewcust.CustomerName = "Member";
                    else
                        objnewcust.CustomerName = MemberName;
                    objnewcust.CustomerCategory = null;
                    objnewcust.CardNumber = "";
                    objnewcust.CustomerThrough = "2";

                    objnewcust.DOB = Convert.ToDateTime(BirthDt);
                    objnewcust.MaritalStatus = "";
                    objnewcust.MemberGroupId = "1000";
                    objnewcust.MobileNo = mobileNo;
                    objnewcust.Status = "00";
                    if (AnniversaryDt != null)
                    {
                        objnewcust.AnniversaryDate = Convert.ToDateTime(AnniversaryDt);
                    }
                    objnewcust.DOJ = DateTime.Now;
                    objnewcust.EmailId = "";
                    objnewcust.EnrollingOutlet = outletid;
                    if (string.IsNullOrEmpty(Gender))
                        objnewcust.Gender = "";
                    else
                        objnewcust.Gender = Gender;
                    objnewcust.IsSMS = null;
                    objnewcust.BillingCustomerId = null;

                    context.CustomerDetails.Add(objnewcust);
                    context.SaveChanges();
                }
                if (objcustdetails != null)
                {
                    objtransactionMaster.CustomerId = objcustdetails.CustomerId;
                    objtransactionMaster.CustomerPoints = objcustdetails.Points;
                }
                else
                {
                    objtransactionMaster.CustomerId = objnewcust.CustomerId;
                    objtransactionMaster.CustomerPoints = objnewcust.Points;
                }

                objtransactionMaster.CounterId = outletid + "01";
                objtransactionMaster.MobileNo = mobileNo;
                objtransactionMaster.Datetime = DateTime.Now;
                objtransactionMaster.TransType = "1";
                objtransactionMaster.TransSource = "1";
                if (GroupId != "1163")
                {
                    objtransactionMaster.InvoiceNo = "B_Feedbackpoints";
                }
                else
                {
                    objtransactionMaster.InvoiceNo = "AabharBonus";
                }
                objtransactionMaster.InvoiceAmt = 0;
                objtransactionMaster.Status = "06";
                objtransactionMaster.PointsEarned = objoutlet.FeedBackPoints;
                objtransactionMaster.PointsBurned = 0;
                objtransactionMaster.CampaignPoints = 0;
                objtransactionMaster.TxnAmt = 0;

                objtransactionMaster.Synchronization = "";
                objtransactionMaster.SyncDatetime = null;

                if (GroupId != "1163")
                {
                    context.TransactionMasters.Add(objtransactionMaster);
                    context.SaveChanges();
                }
                if (lstfeedback.Count == 0 && GroupId == "1163")
                {
                    context.TransactionMasters.Add(objtransactionMaster);
                    context.SaveChanges();
                }

                objpointsExpiry.MobileNo = mobileNo;
                objpointsExpiry.CounterId = outletid + "01";
                if (objcustdetails != null)
                {
                    objpointsExpiry.CustomerId = objcustdetails.CustomerId;
                }
                else
                {
                    objpointsExpiry.CustomerId = objnewcust.CustomerId;
                }

                objpointsExpiry.BurnDate = null;
                objpointsExpiry.Datetime = DateTime.Now;
                objpointsExpiry.EarnDate = DateTime.Now;
                DateTime today = DateTime.Today;
                DateTime next = today.AddYears(1);
                var currentmonth = DateTime.DaysInMonth(next.Year, next.Month);

                if (next.Day < currentmonth)
                {
                    var days = (currentmonth - next.Day);
                    next = today.AddDays(days).AddYears(1);
                }
                objpointsExpiry.ExpiryDate = next;
                objpointsExpiry.Points = objoutlet.FeedBackPoints;
                objpointsExpiry.Status = "00";
                if (GroupId != "1163")
                {
                    objpointsExpiry.InvoiceNo = "B_Feedbackpoints";
                }
                else
                {
                    objpointsExpiry.InvoiceNo = "AabharBonus";
                }
                //objpointsExpiry.InvoiceNo = "B_Feedbackpoints";
                objpointsExpiry.GroupId = objoutlet.GroupId;
                objpointsExpiry.OriginalInvoiceNo = "";
                objpointsExpiry.TransRefNo = null;
                if (GroupId != "1163")
                {
                    context.PointsExpiries.Add(objpointsExpiry);
                    context.SaveChanges();
                }
                if (lstfeedback.Count == 0 && GroupId == "1163")
                {
                    context.PointsExpiries.Add(objpointsExpiry);
                    context.SaveChanges();
                }

                if (lstfeedback.Count == 0 && GroupId == "1163")
                {
                    FeedBackMaster feedback = new FeedBackMaster();
                    feedback.MobileNo = mobileNo;
                    feedback.CustomerName = MemberName;
                    feedback.OutletId = outletid;
                    feedback.Location = LiveIn;
                    feedback.HowToKonwAbout = Knowabt;
                    feedback.DOB = Convert.ToDateTime(BirthDt);
                    if (AnniversaryDt != null)
                    {
                        feedback.DOA = Convert.ToDateTime(AnniversaryDt);
                    }
                    feedback.Points = objoutlet.FeedBackPoints;

                    context.FeedBackMasters.AddOrUpdate(feedback);
                    context.SaveChanges();
                    status = true;
                }
                else
                {
                    foreach (var feedback in lstfeedback)
                    {
                        feedback.Location = LiveIn;
                        feedback.HowToKonwAbout = Knowabt;
                        feedback.DOB = Convert.ToDateTime(BirthDt);
                        if (AnniversaryDt != null)
                        {
                            feedback.DOA = Convert.ToDateTime(AnniversaryDt);
                        }
                        feedback.Points = objoutlet.FeedBackPoints;

                        context.FeedBackMasters.AddOrUpdate(feedback);
                        context.SaveChanges();
                        status = true;
                    }
                }
                if (status)
                {
                    // string msgmobileno = "8452047477";
                    string message1;
                    SMSDetail objsmsdetails = new SMSDetail();
                    FeedBackMobileMaster objmobilemaster = new FeedBackMobileMaster();
                    SMSEmailMaster objsmsemailmaster = new SMSEmailMaster();
                    if (GroupId != "1163")
                    {
                        objmobilemaster = context.FeedBackMobileMasters.Where(x => x.MessageId == "202").FirstOrDefault();
                        objsmsemailmaster = context.SMSEmailMasters.Where(x => x.MessageId == "202").FirstOrDefault();
                        string message = objsmsemailmaster.SMS;
                        TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
                        if (objcustdetails != null)
                        {
                            message = message.Replace("#01", objcustdetails.CustomerName);
                        }
                        else
                        {
                            message = message.Replace("#01", "Member");
                        }

                        message = message.Replace("#30", mobileNo);
                        message = message.Replace("#08", Convert.ToString(date));

                        objsmsdetails = context.SMSDetails.Where(x => x.OutletId == outletid).FirstOrDefault();
                        SendMessage(objmobilemaster.MobileNo, objsmsdetails.SenderId, message, objsmsdetails.TxnUrl, objsmsdetails.TxnUserName, objsmsdetails.TxnPassword);
                    }

                    objsmsemailmaster = context.SMSEmailMasters.Where(x => x.MessageId == "201").FirstOrDefault();
                    message1 = objsmsemailmaster.SMS;
                    if (objcustdetails != null)
                    {
                        message1 = message1.Replace("#01", objcustdetails.CustomerName);
                    }
                    else
                    {
                        message1 = message1.Replace("#01", "Member");
                    }
                    SendMessage(mobileNo, objsmsdetails.SenderId, message1, objsmsdetails.TxnUrl, objsmsdetails.TxnUserName, objsmsdetails.TxnPassword);
                }
            }
            return status;
        }

        public void SendMessage(string MobileNo, string Sender, string MobileMessage, string Url, string UserName, string Password)
        {
            try
            {

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
                newexception.AddException(ex, "feedbacksmssend");

            }

        }
    }
}
