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
using BOTS_BL.Models;

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

        public string SubmitRating(string mobileNo, int[] ranking, string GroupId, string outletId)
        {
            string status ="false";
            // string smsresponce = "";
            FeedBackMaster objfeedback = new FeedBackMaster();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
            using (var context = new BOTSDBContext(connStr))
            {
                CustomerDetail objcustdetails = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                OutletDetail objoutlet = context.OutletDetails.Where(x => x.OutletId == outletId).FirstOrDefault();
                TransactionMaster objtransactionMaster = new TransactionMaster();
                PointsExpiry objpointsExpiry = new PointsExpiry();
                var transaction = context.TransactionMasters.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.Datetime).Take(2).ToList();
                var pointexpiry = context.PointsExpiries.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.Datetime).Take(2).ToList();
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
                       
                        objfeedback.DOJ = date;
                        Combinedpoint += points;
                        objfeedback = context.FeedBackMasters.Add(objfeedback);
                        context.SaveChanges();
                        status = "true";
                    }
                    queid++;
                }
                if (Combinedpoint <= 4)
                {
                    SMSDetail objsmsdetails = new SMSDetail();
                    FeedBackMobileMaster objmobilemaster = context.FeedBackMobileMasters.Where(x => x.MessageId == "203").FirstOrDefault();
                    SMSEmailMaster objsmsemailmaster = context.SMSEmailMasters.Where(x => x.MessageId == "203").FirstOrDefault();
                    

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
                   // SendMessage(objmobilemaster.MobileNo, objsmsdetails.SenderId, message, objsmsdetails.TxnUrl, objsmsdetails.TxnUserName, objsmsdetails.TxnPassword);
                    SendMessageVision(objsmsdetails.TxnUrl, objmobilemaster.MobileNo, message, objsmsdetails.SenderId, objsmsdetails.TxnPassword);
                }
                if (transaction.Count > 0)
                {
                    if (transaction[0].InvoiceNo == "B_Feedbackpoints" && pointexpiry[0].InvoiceNo == "B_Feedbackpoints")
                    {
                        var point = objcustdetails.Points;
                        objcustdetails.Points = point + objoutlet.FeedBackPoints;
                        context.CustomerDetails.AddOrUpdate(objcustdetails);
                        context.SaveChanges();
                        objtransactionMaster.CustomerId = objcustdetails.CustomerId;
                        objtransactionMaster.CustomerPoints = objcustdetails.Points;
                        objtransactionMaster.CounterId = outletId + "01";
                        objtransactionMaster.MobileNo = mobileNo;
                        objtransactionMaster.Datetime = date;
                        objtransactionMaster.TransType = "1";
                        objtransactionMaster.TransSource = "1";
                        objtransactionMaster.InvoiceNo = "B_Feedbackpoints";
                        objtransactionMaster.InvoiceAmt = 0;
                        objtransactionMaster.Status = "06";
                        objtransactionMaster.PointsEarned = objoutlet.FeedBackPoints;
                        objtransactionMaster.PointsBurned = 0;
                        objtransactionMaster.CampaignPoints = 0;
                        objtransactionMaster.TxnAmt = 0;
                        objtransactionMaster.Synchronization = "";
                        objtransactionMaster.SyncDatetime = null;
                        context.TransactionMasters.Add(objtransactionMaster);
                        context.SaveChanges();
                        objpointsExpiry.MobileNo = mobileNo;
                        objpointsExpiry.CounterId = outletId + "01";
                        objpointsExpiry.CustomerId = objcustdetails.CustomerId;
                        objpointsExpiry.BurnDate = null;
                        objpointsExpiry.Datetime = date;
                        objpointsExpiry.EarnDate = date;
                        // DateTime today = date;
                        DateTime next = date.AddYears(1);
                        var currentmonth = DateTime.DaysInMonth(next.Year, next.Month);

                        if (next.Day < currentmonth)
                        {
                            var days = (currentmonth - next.Day);
                            next = date.AddDays(days).AddYears(1);
                        }
                        objpointsExpiry.ExpiryDate = next;
                        objpointsExpiry.Points = objoutlet.FeedBackPoints;
                        objpointsExpiry.Status = "00";
                        objpointsExpiry.InvoiceNo = "B_Feedbackpoints";
                        objpointsExpiry.GroupId = objoutlet.GroupId;
                        objpointsExpiry.OriginalInvoiceNo = "";
                        objpointsExpiry.TransRefNo = null;
                        context.PointsExpiries.Add(objpointsExpiry);
                        context.SaveChanges();
                        status = "pointsGiven";
                    }
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
            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
            using (var context = new BOTSDBContext(connStr))
            {
                CustomerDetail objcustdetails = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                OutletDetail objoutlet = context.OutletDetails.Where(x => x.OutletId == outletid).FirstOrDefault();
                FeedBackMaster objfeedback = new FeedBackMaster();
                lstfeedback = context.FeedBackMasters.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.DOJ).Take(2).ToList();
                if (objcustdetails != null)
                {
                    if (AnniversaryDt != null)
                    {
                        objcustdetails.AnniversaryDate = Convert.ToDateTime(AnniversaryDt);
                    }
                    var point = objcustdetails.Points;
                    objcustdetails.Points = point + objoutlet.FeedBackPoints;
                    context.CustomerDetails.AddOrUpdate(objcustdetails);
                    context.SaveChanges();
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
                    if (BirthDt != "")
                    {
                        objnewcust.DOB = Convert.ToDateTime(BirthDt);
                    }
                    objnewcust.MaritalStatus = "";
                    objnewcust.MemberGroupId = "1000";
                    objnewcust.MobileNo = mobileNo;
                    objnewcust.Status = "00";
                    if (AnniversaryDt != null)
                    {
                        objnewcust.AnniversaryDate = Convert.ToDateTime(AnniversaryDt);
                    }
                    objnewcust.DOJ = date;
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
                objtransactionMaster.Datetime = date;
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
                objpointsExpiry.Datetime = date;
                objpointsExpiry.EarnDate = date;
               // DateTime today = date;
                DateTime next = date.AddYears(1);
                var currentmonth = DateTime.DaysInMonth(next.Year, next.Month);

                if (next.Day < currentmonth)
                {
                    var days = (currentmonth - next.Day);
                    next = date.AddDays(days).AddYears(1);
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
                        if (BirthDt != "")
                        {
                            feedback.DOB = Convert.ToDateTime(BirthDt);
                        }
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
                        //TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        //DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
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
                        // SendBulkSMSMessageTxn(objmobilemaster.MobileNo, objsmsdetails.SenderId, message);
                        SendBulkSMSMessagevision(objmobilemaster.MobileNo, objsmsdetails.SenderId, message);

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
                    // SendMessage(mobileNo, objsmsdetails.SenderId, message1, objsmsdetails.TxnUrl, objsmsdetails.TxnUserName, objsmsdetails.TxnPassword);
                    SendMessageVision(objsmsdetails.TxnUrl, mobileNo, message1, objsmsdetails.SenderId, objsmsdetails.TxnPassword);

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
        //send bulk sms through technocore
        public void SendBulkSMSMessageTxn(string MobileNo, string Sender, string MobileMessage)
        {
            string responseString;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("https://203.212.70.210/smpp/sendsms?username=bluhtployalty&password=blue8621&to=" + MobileNo + "&from="+ Sender + "&text=" + MobileMessage + "&category=bulk");
                UTF8Encoding encoding = new UTF8Encoding();
                // byte[] data = encoding.GetBytes(sbposdata.ToString());
                httpWReq.Method = "GET";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                //httpWReq.ContentLength = data.Length;
                //using (Stream stream = httpWReq.GetRequestStream())
                //{
                //    stream.Write(data, 0, data.Length);
                //}
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
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
        //send sms through vender vision
        public void SendMessageVision(string Url, string MobileNo, string MobileMessage, string SenderId, string Password)
        { 
            var httpWebRequest_12211 = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest_12211.ContentType = "application/json";
                            httpWebRequest_12211.Method = "POST";
                            using (var streamWriter_12211 = new StreamWriter(httpWebRequest_12211.GetRequestStream()))
                            {
                                string json_12211 = "{\"Account\":" +
                                                "{\"APIKey\":\"" + Password + "\"," +
                                                "\"SenderId\":\"" + SenderId + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"0\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"91" + MobileNo + "\"," +
                                                "\"Text\":\"" + MobileMessage + "\"}]" +
                                                "}";
                                streamWriter_12211.Write(json_12211);
                            }
                var httpResponse_12211 = (HttpWebResponse)httpWebRequest_12211.GetResponse();
                using (var streamReader_12211 = new StreamReader(httpResponse_12211.GetResponseStream()))
                {
                    var result_12211 = streamReader_12211.ReadToEnd();
                }
        }
        //send bulk sms through vision
        public void SendBulkSMSMessagevision(string MobileNo, string Sender, string MobileMessage)
        {
            string responseString;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
              //  HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("https://203.212.70.210/smpp/sendsms?username=bluhtployalty&password=blue8621&to=" + MobileNo + "&from=" + Sender + "&text=" + MobileMessage + "&category=bulk");
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("https://sms.visionhlt.com/api/mt/SendSms?User=Goldmart&Password=goldmart&SenderId=GOLDMT&Channel=Trans&DCS=0&FlashSms=0&Number=" +MobileNo + "&Text=" +MobileMessage +"");
                UTF8Encoding encoding = new UTF8Encoding();               
                httpWReq.Method = "GET";
                httpWReq.ContentType = "application/json";                
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
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
    }
}
