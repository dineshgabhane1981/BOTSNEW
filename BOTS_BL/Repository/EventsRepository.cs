using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BOTS_BL.Models.EventModule;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Web;
using System.Net;
using System.IO;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Data;

namespace BOTS_BL.Repository
{
    public class EventsRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        public List<tblGroupDetail> GetNeverOptForGroups(bool status)
        {
            List<tblGroupDetail> lstData = new List<tblGroupDetail>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (status)
                        lstData = context.tblGroupDetails.Where(x => x.IsEvent == true).ToList();
                    else
                        lstData = context.tblGroupDetails.Where(x => x.IsEvent == null || x.IsEvent == false).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return lstData;
        }

        public bool EnableEventModule(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsEvent = true;
                        context.SaveChanges();
                        status = true;
                    }
                    if (status)
                    {
                        string connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                        using (var contextdb = new BOTSDBContext(connStr))
                        {
                            string tableScript = "CREATE TABLE [dbo].[EventDetails]([EventId][bigint] IDENTITY(1, 1) NOT NULL,[GroupId] [int] NOT NULL,[EventName] [nvarchar](max)NULL,[Place] [nvarchar](500) NULL," +
                                "[EventType] [nvarchar](500) NULL,[EventStartDate] [date] NULL,[EventEndDate] [date] NULL,[BonusPoints] [int] NULL,[PointsExpiryDays] [int] NULL,[1stRemBefore] [int] NULL," +
                                "[1stReminderScript] [nvarchar](max)NULL,[2ndRemBefore] [int] NULL,[2ndReminderScript] [nvarchar](max)NULL,[Desciption] [nvarchar](max)NULL,[AddedBy] [nvarchar](50) NOT NULL," +
                                "[Addeddate] [date] NOT NULL,[Status] [varchar](20) NULL,[BonusMessageScript] [nvarchar](max)NULL,CONSTRAINT[PK_EventDetails] PRIMARY KEY CLUSTERED([EventId] ASC)WITH(PAD_INDEX = OFF," +
                                " STATISTICS_NORECOMPUTE = OFF,IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";

                            tableScript += "CREATE TABLE [dbo].[EventMemberDetails]([SLno][int] IDENTITY(1, 1) NOT NULL,[GroupId] [int] NOT NULL, [EventId] [int] NOT NULL," +
                                "[Mobileno] [varchar](10) NULL,[Name] [nvarchar](500) NULL,[Gender] [varchar](20) NULL,[DOB] [date] NULL,[DOA] [date] NULL," +
                                "[Address] [nvarchar](max)NULL,[EmailId] [nvarchar](500) NULL,[AlternateNo] [varchar](10) NULL,[PointsGiven] [numeric](18, 2) NULL,[Place] [nvarchar](max)NULL," +
                                "[DateOfRegistration] [datetime] NULL,[CustomerType] [varchar](10) NULL,[EventName] [nvarchar](500) NULL,[FirstRemSentDate] [date] NULL,[SecondRemSentDate] [date] NULL," +
                                "[FirstRemDate] [date] NULL,[SecondRemDate] [date] NULL,CONSTRAINT[PK_EventMemberDetails] PRIMARY KEY CLUSTERED([SLno] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF," +
                                " IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";

                            contextdb.Database.CreateIfNotExists();
                            contextdb.Database.ExecuteSqlCommand(tableScript);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableEventModule");
            }
            return status;
        }

        public bool DisableEventModule(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsEvent = false;
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableEventModule");
            }
            return status;
        }

        public bool SaveEventData(EventDetail Obj, string connectionstring)
        {
            bool status = false;

            try
            {
                using (var context = new BOTSDBContext(connectionstring))
                {
                    context.EventDetails.AddOrUpdate(Obj);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEventData");
            }
            return status;
        }

        public List<EventDetail> GetListEvents(string GroupId, string connectionString)
        {
            List<EventDetail> listEvent = new List<EventDetail>();

            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    listEvent = context.EventDetails.Where(x => x.Status == null || x.Status != "Deleted").ToList();
                    listEvent = listEvent.OrderByDescending(x => x.Addeddate).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetListEvents");
            }

            return listEvent;
        }

        public bool EventDelete(string EventId, string GroupId, string connectionstring)
        {
            bool status = false;
            try
            {
                EventDetail ObjEventDetails = new EventDetail();
                using (var context = new BOTSDBContext(connectionstring))
                {
                    int varid = Convert.ToInt32(EventId);
                    ObjEventDetails = context.EventDetails.Where(x => x.EventId == varid).FirstOrDefault();
                    ObjEventDetails.Status = "Deleted";

                    context.EventDetails.AddOrUpdate(ObjEventDetails);
                    context.SaveChanges();
                }
                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EventDelete");
            }
            return status;
        }

        public EventDetail GetEditEvents(string groupId, string eventid, string connectionString)
        {
            EventDetail obj = new EventDetail();

            int Id = Convert.ToInt32(eventid);
            using (var context = new BOTSDBContext(connectionString))
            {
                obj = context.EventDetails.Where(x => x.EventId == Id).FirstOrDefault();
            }
            if (obj.Addeddate != null)
            {
                obj.strEventdate = obj.Addeddate.ToString("yyyy/MM/dd");
                obj.strEventStartdate = obj.EventStartDate.Value.ToString("yyyy/MM/dd");
                obj.strEventEnddate = obj.EventEndDate.Value.ToString("yyyy/MM/dd");
            }

            return obj;
        }

        public EventModuleData GetCustomerDetails(string groupId, string Mobileno, string Place, string EventId, string connectionString)
        {
            EventModuleData obj = new EventModuleData();
            EventModuleExtraData ExtObj = new EventModuleExtraData();
            int eventId = Convert.ToInt32(EventId);
            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    var statusavailable = context.EventMemberDetails.Where(e => e.Mobileno == Mobileno).Select(y => y.Mobileno).FirstOrDefault();
                    var pointsexp = context.tblRuleMasters.Select(e => e.PointsExpiryMonths).FirstOrDefault();
                    int PointExp = Convert.ToInt32(pointsexp);
                    obj = context.Database.SqlQuery<EventModuleData>("select T4.MobileNo,T4.Name,T4.Gender,T4.DOB,T4.AnniversaryDate,T4.Email,T4.AlternateMobileNo,T4.Points,T4.PointExp,T4.Address,L.LastTxnDate from (select T3.MobileNo,T3.Name,T3.Gender,T3.DOB,T3.AnniversaryDate,T3.Email,T3.AlternateMobileNo,T3.Points,T3.PointExp,CC.Address  from (select  T1.MobileNo,T1.Name,T1.Gender,T1.DOB,T1.AnniversaryDate,T1.Email,T1.AlternateMobileNo,T1.Points,P1.PointExp from (select T.MobileNo,T.Name,T.Gender,T.DOB,T.AnniversaryDate,T.Email,T.OldMobileNo as AlternateMobileNo,P.Points from tblCustDetailsMaster T left join "+
                            "(select Mobileno, sum(points) as Points from tblCustPointsMaster where IsActive = 1 and MobileNo = @Mobileno group by Mobileno) as P on T.MobileNo = P.MobileNo where T.MobileNo = @Mobileno) as T1 "+
                            "left join(select Mobileno, Max(EndDate) as PointExp from tblCustPointsMaster where IsActive = 1 and PointsType = 'Base' and MobileNo = @Mobileno group by Mobileno) as P1 "+
                            "on T1.Mobileno = P1.Mobileno ) as T3 Left join(select top(1) Mobileno,Address from CustomerChild where MobileNo = @Mobileno) as CC on T3.Mobileno = CC.Mobileno) as T4 left join(select MobileNo, LastTxnDate from tblCustTxnSummaryMaster where MobileNo = @Mobileno) as L "+
                            "on T4.MobileNo = L.MobileNo", new SqlParameter("@Mobileno", Mobileno)).FirstOrDefault();
                    ExtObj = context.Database.SqlQuery<EventModuleExtraData>("select top(1) FirstName,MiddleName,SurName,Area,City,Pincode,State,AlternateNo from EventMemberDetails where Mobileno = @Mobileno Order by SLno desc", new SqlParameter("@Mobileno", Mobileno)).FirstOrDefault();
                    
                    if(ExtObj != null)
                    {
                        obj.FirstName = ExtObj.FirstName;
                        obj.MiddleName = ExtObj.MiddleName;
                        obj.SurName = ExtObj.SurName;
                        obj.Area = ExtObj.Area;
                        obj.City = ExtObj.City;
                        //if (!string.IsNullOrEmpty(ExtObj.Pincode.ToString()))
                        //{
                            obj.Pincode = ExtObj.Pincode;
                        //}

                        obj.State = ExtObj.State;
                        obj.AlternateMobileno = Convert.ToString(ExtObj.AlternateNo);
                    }
                    
                    if (statusavailable != null)
                    {
                        obj.CustomerAvailFlag = statusavailable;
                    }

                    if (obj != null)
                    {
                        if (obj.LastTxnDate.HasValue)
                        {
                            obj.strLsttxndate = obj.LastTxnDate.Value.ToString("MM/dd/yyyy");
                        }
                        if (obj.PointExp.HasValue)
                        {
                            obj.strPointExp = obj.PointExp.Value.ToString("MM/dd/yyyy");
                        }
                        if (obj.DOB.HasValue)
                        {
                            obj.strDOB = obj.DOB.Value.ToString("yyyy/MM/dd");
                        }
                        if (obj.AnniversaryDate.HasValue)
                        {
                            obj.strDOA = obj.AnniversaryDate.Value.ToString("yyyy/MM/dd");
                        }
                    }

                    if (obj == null)
                    {
                        obj = new EventModuleData();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetails");
            }
            return obj;
        }

        public bool SaveNewMemberData(EventMemberDetail objData, tblCustDetailsMaster objCustomerDetail, CustomerChild objCustomerChild, TransactionMaster objTM, string connectionstring,int GroupId)
        {
            bool result = false;
            string Message;
            Message = string.Empty;
            tblCustDetailsMaster TM2 = new tblCustDetailsMaster();
            CustomerChild objdata1 = new CustomerChild();
            tblOutletMaster objdata = new tblOutletMaster();
            tblTxnDetailsMaster obj = new tblTxnDetailsMaster();
            tblCustPointsMaster obj1 = new tblCustPointsMaster();
            using (var context = new BOTSDBContext(connectionstring))
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var bonusPoints = context.EventDetails.Where(x => x.EventId == objData.EventId).Select(y => y.BonusPoints).FirstOrDefault();
                        var existingCust = context.tblCustDetailsMasters.Where(x => x.MobileNo == objCustomerDetail.MobileNo).FirstOrDefault();

                        var ExpiryDays = context.EventDetails.Where(x => x.EventId == objData.EventId).Select(y => y.PointsExpiryDays).FirstOrDefault();
                        var Status = context.EventMemberDetails.Where(x => x.Mobileno == objData.Mobileno).Select(y => y.Mobileno).FirstOrDefault();
                        var EventName = context.EventDetails.Where(x => x.EventId == objData.EventId).Select(y => y.EventName).FirstOrDefault();
                        if (existingCust == null)
                        {
                            objData.CustomerType = "New";
                        }
                        else
                        {
                            objData.CustomerType = "Existing";
                        }
                        if (Status == null)
                        {
                            objData.PointsGiven = bonusPoints;
                            var objEventDetails = context.EventDetails.Where(e => e.EventId == objData.EventId).FirstOrDefault();

                            DateTime Expirydate = objData.DateOfRegistration.Value.AddDays(Convert.ToInt32(objEventDetails.PointsExpiryDays));
                            objData.FirstRemDate = Expirydate.AddDays(-Convert.ToInt32(objEventDetails.C1stRemBefore));
                            objData.SecondRemDate = Expirydate.AddDays(-Convert.ToInt32(objEventDetails.C2ndRemBefore));
                             
                        }
                        else
                        {
                            bonusPoints = 0;

                            objData.PointsGiven = bonusPoints;

                            var objEventDetails = context.EventDetails.Where(e => e.EventId == objData.EventId).FirstOrDefault();

                            DateTime Expirydate = objData.DateOfRegistration.Value.AddDays(Convert.ToInt32(objEventDetails.PointsExpiryDays));
                            objData.FirstRemDate = Expirydate.AddDays(-Convert.ToInt32(objEventDetails.C1stRemBefore));
                            objData.SecondRemDate = Expirydate.AddDays(-Convert.ToInt32(objEventDetails.C2ndRemBefore));
                             
                        }
                        objData.EventName = EventName;

                        context.EventMemberDetails.AddOrUpdate(objData);
                        context.SaveChanges();

                        if (objCustomerDetail.Gender == "Male")
                        {
                            objCustomerDetail.Gender = "M";
                        }
                        else
                        {
                            objCustomerDetail.Gender = "F";
                        }

                        if (existingCust == null)
                        {
                            var CustomerId = context.tblCustDetailsMasters.OrderByDescending(x => x.Id).Select(y => y.Id).FirstOrDefault();
                            var AdminOutletId = context.tblOutletMasters.Where(x => x.OutletName.Contains("Admin")).Select(y => y.OutletId).FirstOrDefault();
                            TM2.MobileNo = objCustomerDetail.MobileNo;
                            TM2.Name = objCustomerDetail.Name;
                            TM2.Id = Convert.ToString(Convert.ToInt64(CustomerId) + 1);
                            TM2.CardNo = objCustomerDetail.CardNo;
                            TM2.Email = objCustomerDetail.Email;
                            TM2.DOB = objCustomerDetail.DOB;
                            TM2.AnniversaryDate = objCustomerDetail.AnniversaryDate;
                            TM2.Gender = objCustomerDetail.Gender;
                            TM2.DOJ = IndianDatetime();
                            TM2.EnrolledOutlet = AdminOutletId;
                            
                            TM2.EnrolledBy = "Event";
                            TM2.IsActive = true;
                            
                            //TM2.Points = bonusPoints;

                        }
                        else
                        {
                            TM2 = existingCust;
                            TM2.Name = objCustomerDetail.Name;
                            TM2.CardNo = objCustomerDetail.CardNo;
                            TM2.Email = objCustomerDetail.Email;
                            TM2.DOB = objCustomerDetail.DOB;
                            TM2.AnniversaryDate = objCustomerDetail.AnniversaryDate;
                            TM2.Gender = objCustomerDetail.Gender;
                            //TM2.OldMobileNo = objCustomerDetail.OldMobileNo;

                            //if (Status == null)
                            //{
                            //    TM2.Points = TM2.Points + bonusPoints;
                            //}
                            //else
                            //{
                            //    TM2.Points = TM2.Points;
                            //}
                        }
                        context.tblCustDetailsMasters.AddOrUpdate(TM2);
                        context.SaveChanges();
                        //string MobNo = Convert.ToString(objCustomerChild.MobileNo);
                        var existingCust1 = context.CustomerChilds.Where(x => x.MobileNo == objCustomerChild.MobileNo).FirstOrDefault();
                        if (existingCust1 == null)
                        {
                            var CustomerId1 = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                            objdata1.MobileNo = objCustomerDetail.MobileNo;
                            objdata1.CustomerId = Convert.ToString(Convert.ToInt64(CustomerId1));
                            objdata1.ChildCount = objCustomerChild.ChildCount;
                            objdata1.Child1DOB = objCustomerChild.Child1DOB;
                            objdata1.Pincode = objCustomerChild.Pincode;
                            objdata1.Address = objCustomerChild.Address;
                            objdata1.PromotionalSMS = objCustomerChild.PromotionalSMS;
                            objdata1.Child2DOB = objCustomerChild.Child2DOB;
                            objdata1.Child3DOB = objCustomerChild.Child3DOB;
                            objdata1.City = objCustomerChild.City;
                            objdata1.LanguagePreferred = objCustomerChild.LanguagePreferred;
                            objdata1.Religion = objCustomerChild.Religion;
                            objdata1.Area = objCustomerChild.Area;
                        }
                        else
                        {
                            objdata1 = existingCust1;
                            //objdata1.MobileNo = objCustomerChild.MobileNo;
                            //objdata1.CustomerId = objCustomerChild.CustomerId;
                            objdata1.Address = objCustomerChild.Address;

                        }
                        //context.CustomerChilds.AddOrUpdate(objdata1);
                        //context.SaveChanges();

                        if (Status == null)
                        {
                            obj.CounterId = (TM2.EnrolledOutlet + "01");
                            obj.MobileNo = TM2.MobileNo;
                            obj.TxnDatetime = DateTime.Now;
                            obj.TxnType = "1";
                            obj.TxnBy = "Event";
                            obj.InvoiceNo = "Bonus";
                            obj.InvoiceAmt = 0;
                            obj.IsActive = true;
                            
                            obj.PointsEarned = bonusPoints;
                            obj.PointsBurned = 0;
                            obj.CampaignPoints = 0;
                            obj.InvoiceAmt = 0;
                            obj.PointsEarned = bonusPoints;
                            obj.MobileNoInvId = obj.MobileNo + TM2.EnrolledOutlet + obj.InvoiceNo + obj.TxnDatetime + obj.InvoiceAmt;

                            context.tblTxnDetailsMasters.AddOrUpdate(obj);
                            context.SaveChanges();

                            obj1.MobileNo = TM2.MobileNo;
                           
                            obj1.StartDate = DateTime.Now;
                            obj1.EndDate = obj1.StartDate.Value.AddDays(Convert.ToInt32(ExpiryDays));
                            obj1.Points = bonusPoints;
                            obj1.PointsType = "Bonus";
                            obj1.PointsDesc = "Event";
                            obj1.IsActive = true;
                            obj1.MinInvoiceAmtRequired = 0;
                            obj1.MobileNoPtsId = obj1.MobileNo + "Event";

                            context.tblCustPointsMasters.AddOrUpdate(obj1);
                            context.SaveChanges();

                        }

                        result = true;

                        transaction.Commit();

                        if (Status == null)
                        {
                            using (var context1 = new CommonDBContext())
                            {
                                EventDetail ObjMsgData = new EventDetail();
                                string groupid = Convert.ToString(objData.GroupId);
                                var WATokenid = context1.CommonWAInstanceMasters.Where(e => e.GroupId == groupid).Select(y => y.TokenId).FirstOrDefault();
                                var ObjEventDetail = context.EventDetails.Where(x => x.EventId == objData.EventId).FirstOrDefault();
                                Message = ObjEventDetail.BonusMessageScript;
                                if (!string.IsNullOrEmpty(Message))
                                {
                                    SendWAMessage(ObjEventDetail, objData, WATokenid);
                                }
                            }
                        }


                        if(GroupId == 1085)// Code for Dande API
                        {
                            Thread _job1 = new Thread(() => SendDetailsToPOS(objData));
                            _job1.Start();
                        }       
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "SaveNewMemberData");
                    }
                }
            }


            return result;
        }

        public List<EventMemberDetail> GetEventReport(string groupid, string fromDate, string toDate)
        {
            List<EventMemberDetail> lstReportData = new List<EventMemberDetail>();
            var connStr = CR.GetCustomerConnString(groupid);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        var fDate = Convert.ToDateTime(fromDate);
                        var tDate = Convert.ToDateTime(toDate).AddDays(1);
                        lstReportData = context.EventMemberDetails.Where(x => x.DateOfRegistration >= fDate && x.DateOfRegistration <= tDate).ToList();
                    }
                    else
                        lstReportData = context.EventMemberDetails.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetEventReport");
            }
            return lstReportData;
        }

        public string GetLogo(string groupId)
        {
            string logoUrl = string.Empty;

            string connectionString = CR.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    var objbrandDetail = context.tblBrandMasters.Where(x => x.GroupId == groupId).FirstOrDefault();
                    logoUrl = objbrandDetail.BrandLogoUrl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLogo");
            }
            return logoUrl;

        }

        public void SendWAMessage(EventDetail objMsg, EventMemberDetail objDetail, string WATokenid)
        {
            string responseString;

            try
            {
                objMsg.BonusMessageScript = objMsg.BonusMessageScript.Replace("#01", objDetail.Name);
                objMsg.BonusMessageScript = objMsg.BonusMessageScript.Replace("#06", Convert.ToString(objDetail.PointsGiven));
                objMsg.BonusMessageScript = HttpUtility.UrlEncode(objMsg.BonusMessageScript);
                //string type = "TEXT";

                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", WATokenid);
                sbposdata.AppendFormat("&phone=91{0}", objDetail.Mobileno);
                sbposdata.AppendFormat("&message={0}", objMsg.BonusMessageScript);

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

        public List<tblGroupDetail> GetAllEventCustomer()
        {
            List<tblGroupDetail> objGroupList = new List<tblGroupDetail>();

            try
            {
                using (var context = new CommonDBContext())
                {
                    objGroupList = context.tblGroupDetails.Where(e => e.IsEvent.Value == true).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllEventCustomer");
            }
            return objGroupList;
        }

        public List<EventManagementReport> GetCustomEventReport()
        {
            List<EventManagementReport> LstData = new List<EventManagementReport>();

            try
            {
                using (var context = new CommonDBContext())
                {
                    LstData = context.EventManagementReports.Where(x => x.ReportStatus == true).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomEventReport");
            }
            return LstData;
        }

        public List<EventDetail> EventReportData(string groupid)
        {
            List<EventDetail> lstReportEventDate = new List<EventDetail>();
            var connStr = CR.GetCustomerConnString(groupid);
            string Today = IndianDatetime().ToString("yyyy-MM-dd");
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime dateVal = DateTime.ParseExact(Today, "yyyy-MM-dd", culture);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    lstReportEventDate = context.EventDetails.Where(x => x.EventStartDate <= dateVal && x.EventEndDate >= dateVal && x.Status == "Started").ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EventReportData");
            }
            return lstReportEventDate;
        }

        public DataSet EventCustReportData(string groupid)
        {
            DataTable Dt = new DataTable();
            DataSet DT = new DataSet();
            var connStr = CR.GetCustomerConnString(groupid);
            string Today = IndianDatetime().ToString("yyyy-MM-dd");
            try
            {
                    SqlConnection _Con = new SqlConnection(connStr);
                    
                    SqlCommand cmdReport = new SqlCommand("sp_EventReportMonthly", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                       SqlParameter param1 = new SqlParameter("pi_Date", Today);
                       cmdReport.CommandType = CommandType.StoredProcedure;
                       cmdReport.Parameters.Add(param1);

                       daReport.Fill(DT);
                    }       
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EventCustReportData");
            }

            return DT;
        }

        public List<EventMemberDetail> EventMemberData(string GroupId, string EventId)
        {
            List<EventMemberDetail> lstReportData = new List<EventMemberDetail>();
            int eventid = Convert.ToInt32(EventId);
            var connStr = CR.GetCustomerConnString(GroupId);
            string Today = IndianDatetime().ToString("yyyy-MM-dd");
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime dateVal = DateTime.ParseExact(Today, "yyyy-MM-dd", culture);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    lstReportData = context.EventMemberDetails.Where(x => EntityFunctions.TruncateTime(x.DateOfRegistration) == dateVal && x.EventId == eventid).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EventMemberData");
            }


            return lstReportData;
        }

        public string GetGroupdetails(string groupid)
        {
            string GroupName;
            GroupName = string.Empty;
            List<tblGroupDetail> lstGroupdetails = new List<tblGroupDetail>();
            int Groupid = Convert.ToInt32(groupid);
            try
            {
                using (var context = new CommonDBContext())
                {
                    var GroupName1 = context.tblGroupDetails.Where(e => e.GroupId == Groupid).FirstOrDefault();
                    GroupName = Convert.ToString(GroupName1.RetailName);
                }


            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupdetails");
            }
            return GroupName;
        }

        public List<ReminderData> FirstRemainderData(string groupid, string EventId)
        {
            List<EventModuleMessageData> lstmsgdata = new List<EventModuleMessageData>();
            List<EventMemberDetail> lstMemberdetails = new List<EventMemberDetail>();
            List<ReminderData> obj = new List<ReminderData>();
            List<ReminderData> objData = new List<ReminderData>();

            string Today = IndianDatetime().ToString("yyyy-MM-dd");
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime dateVal = DateTime.ParseExact(Today, "yyyy-MM-dd", culture);
            var connStr = CR.GetCustomerConnString(groupid);

            int GroupidInt = Convert.ToInt32(groupid);
            int EventIdInt = Convert.ToInt32(EventId);
            string WATokenid, FirstRemainderscript;
            int ExpDays;
            WATokenid = string.Empty;
            FirstRemainderscript = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objWA = context.CommonWAInstanceMasters.Where(e => e.GroupId == groupid).Select(y => y.TokenId).FirstOrDefault();
                    WATokenid = Convert.ToString(objWA);
                    //WATokenid = "5fc8ed623629423c01ce4221";
                }

                using (var context = new BOTSDBContext(connStr))
                {
                    var obj1 = context.EventDetails.Where(e => e.GroupId == GroupidInt && e.EventId == EventIdInt).FirstOrDefault();
                    FirstRemainderscript = obj1.C1stReminderScript;
                    string EventStartDate = obj1.EventStartDate.Value.ToString("yyyy-MM-dd");
                    ExpDays = Convert.ToInt32(obj1.PointsExpiryDays);
                    DateTime dateVal1 = DateTime.ParseExact(Today, "yyyy-MM-dd", culture);
                    obj = context.Database.SqlQuery<ReminderData>("select E.Mobileno,E.Name,E.PointsGiven,cast(E.DateOfRegistration as date) as DateOfRegistration,E.EventId from EventMemberDetails E where E.FirstRemDate = @Today and E.EventId = @EvenId and E.FirstRemSentDate is NULL and E.PointsGiven > 0 and E.Mobileno not in (select T.Mobileno from TransactionMaster T where (cast(T.Datetime as Date) between cast(@EventStartDate as date) and cast(@Today as Date)) and T.TransType = '2')", new SqlParameter("@Today", dateVal), new SqlParameter("@EventStartDate", dateVal1), new SqlParameter("@EvenId", EventIdInt)).ToList();
                }

                foreach (var item in obj)
                {
                    ReminderData Data = new ReminderData();
                    Data.Mobileno = item.Mobileno;
                    Data.Name = item.Name;
                    Data.PointsGiven = item.PointsGiven;
                    Data.FirstReminderScript = FirstRemainderscript;
                    Data.Tokenid = WATokenid;
                    Data.ExpDate = item.DateOfRegistration.AddDays(ExpDays);

                    objData.Add(Data);

                    using (var context = new BOTSDBContext(connStr))
                    {
                        var Lst = context.EventMemberDetails.Where(e => e.Mobileno == item.Mobileno && e.PointsGiven != 0 && e.EventId == item.EventId).FirstOrDefault();
                        Lst.FirstRemSentDate = dateVal;
                        context.EventMemberDetails.AddOrUpdate(Lst);
                        context.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "FirstRemainderData");
            }

            return objData;
        }

        public List<ReminderData> SecondRemainderData(string groupid, string EventId)
        {
            List<EventModuleMessageData> lstmsgdata = new List<EventModuleMessageData>();
            List<EventMemberDetail> lstMemberdetails = new List<EventMemberDetail>();
            List<ReminderData> obj = new List<ReminderData>();
            List<ReminderData> objData = new List<ReminderData>();

            string Today = IndianDatetime().ToString("yyyy-MM-dd");
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime dateVal = DateTime.ParseExact(Today, "yyyy-MM-dd", culture);
            var connStr = CR.GetCustomerConnString(groupid);

            int GroupidInt = Convert.ToInt32(groupid);
            int EventIdInt = Convert.ToInt32(EventId);
            string WATokenid, SecondRemainderscript;
            int ExpDays;
            WATokenid = string.Empty;
            SecondRemainderscript = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objWA = context.CommonWAInstanceMasters.Where(e => e.GroupId == groupid).Select(y => y.TokenId).FirstOrDefault();
                    WATokenid = Convert.ToString(objWA);
                    //WATokenid = "5fc8ed623629423c01ce4221";
                }

                using (var context = new BOTSDBContext(connStr))
                {
                    var obj1 = context.EventDetails.Where(e => e.GroupId == GroupidInt && e.EventId == EventIdInt).FirstOrDefault();
                    SecondRemainderscript = obj1.C2ndReminderScript;
                    string EventStartDate = obj1.EventStartDate.Value.ToString("yyyy-MM-dd");
                    ExpDays = Convert.ToInt32(obj1.PointsExpiryDays);
                    DateTime dateVal1 = DateTime.ParseExact(Today, "yyyy-MM-dd", culture);
                    obj = context.Database.SqlQuery<ReminderData>("select E.Mobileno,E.Name,E.PointsGiven,cast(E.DateOfRegistration as date) as DateOfRegistration,E.EventId from EventMemberDetails E where E.SecondRemDate = @Today and E.EventId = @EvenId and E.SecondRemSentDate is NULL and E.PointsGiven > 0 and E.Mobileno not in (select T.Mobileno from TransactionMaster T where (cast(T.Datetime as Date) between cast(@EventStartDate as date) and cast(@Today as date)) and T.TransType = '2')", new SqlParameter("@Today", dateVal), new SqlParameter("@EventStartDate", dateVal1), new SqlParameter("@EvenId", EventIdInt)).ToList();
                    //obj = context.Database.SqlQuery<ReminderData>("select E.Mobileno,E.Name,E.PointsGiven from EventMemberDetails E where E.SecondRemDate = @Today and E.EventId = @EvenId and E.Mobileno not in (select T.Mobileno from TransactionMaster T where (cast(T.Datetime as Date) between @EventStartDate and @Today) and T.TransType = '2')", new SqlParameter("@Today", dateVal), new SqlParameter("@EventStartDate", dateVal1), new SqlParameter("@EvenId", EventIdInt)).ToList();
                }

                foreach (var item in obj)
                {
                    ReminderData Data = new ReminderData();
                    Data.Mobileno = item.Mobileno;
                    Data.Name = item.Name;
                    Data.PointsGiven = item.PointsGiven;
                    Data.SecondReminderScript = SecondRemainderscript;
                    Data.Tokenid = WATokenid;
                    Data.ExpDate = item.DateOfRegistration.AddDays(ExpDays);

                    objData.Add(Data);
                    using (var context = new BOTSDBContext(connStr))
                    {
                        var Lst = context.EventMemberDetails.Where(e => e.Mobileno == item.Mobileno && e.PointsGiven != 0 && e.EventId == item.EventId).FirstOrDefault();
                        Lst.SecondRemSentDate = dateVal;
                        context.EventMemberDetails.AddOrUpdate(Lst);
                        context.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SecondRemainderData");
            }

            return objData;
        }

        public string GetReportEmail(int groupId)
        {
            string emailId = string.Empty;
            using (var context = new CommonDBContext())
            {
                emailId = context.tblEventReportEmails.Where(x => x.GroupId == groupId).Select(y => y.EmailId).FirstOrDefault();
            }
            return emailId;
        }

        public void SendDetailsToPOS(EventMemberDetail Obj)
        {
            try
            {
                CustomerDetailsPADM ObjPADMData = new CustomerDetailsPADM();

                string result;
                string mobileNo = Obj.Mobileno;
                string JSONMobileno = "{\"mobileNo\":"+ mobileNo + "}";
                string APISearchByMobileNo, AuthKey,XKey, APIAddCustomer,APIUpdateCustomer;
                AuthKey = "jdO4jCFnTnOPA5VnQHOmgLhnTnONZeZcRnO3.jdO4EB9mgsWIg6WqNotcQHOmhMGIg6WqNotcQHOog65bV63pEXN0RXbniCSqhpSaELZnToNeReFxNrcaE4SaELZnToNcRIN6THbnD7AdhrAziKOagLAIg6WqNot6SXbniMOmXBVnTnNnQHOrfCOyWrqzBBAmhpqpNot4kV.ER3aLY96NBxUQe9tpiU8CTQf_dUyPvL1tgB24UT4n65NgyRz1V1QoSTXX1_a03lox5gyK5tIqBxr1peHQcNbNV";
                XKey = "zVhY2RjNGFormBoWI4LWE3YmYtZmY2MzVh";

                APISearchByMobileNo = "https://gds.acmepadm.com:8443/acme-document-web/doc/v1/customer/5";
                APIAddCustomer = "https://gds.acmepadm.com:8443/acme-document-web/doc/v1/customer/1";
                APIUpdateCustomer = "https://gds.acmepadm.com:8443/acme-document-web/doc/v1/customer/2";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(APISearchByMobileNo);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Headers.Add("Authorization", AuthKey);
                httpWebRequest.Headers.Add("X-key", XKey);
                httpWebRequest.Headers.Add("sProgramKey","1");

                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JSONMobileno);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                //string customerCode, city, state, address1, address2, area, emailId, firstName, middleName, surName;
                //customerCode = string.Empty;
                var jsonObject = JObject.Parse(result);         
                var data = (JObject)jsonObject["data"];
                var errorCode = (string)data["errorCode"];              
                IEnumerable<JToken> result1 = jsonObject.SelectToken("data.result");
                

                if (errorCode == "0")
                {
                    if (result1 != null)
                    {
                        foreach (JToken item in result1)
                        {
                            ObjPADMData.customerCode = (string)item["customerCode"];
                            ObjPADMData.city = (string)item["city"];
                            ObjPADMData.state = (string)item["state"];
                            ObjPADMData.mobileNo = (string)item["mobileNo"];
                            ObjPADMData.address1 = (string)item["address1"];
                            ObjPADMData.address2 = (string)item["address2"];
                            ObjPADMData.area = (string)item["area"];
                            ObjPADMData.emailId = (string)item["emailId"];
                            ObjPADMData.firstName = (string)item["firstName"];
                            ObjPADMData.middleName = (string)item["middleName"];
                            ObjPADMData.surName = (string)item["surName"];
                        }
                    }
                    ObjPADMData.name = ObjPADMData.firstName + " " + ObjPADMData.middleName + " " + ObjPADMData.surName;
                    //ObjPADMData.name = Obj.Name;
                    
                    if(!string.IsNullOrEmpty(Obj.DOB.ToString()))
                    {
                       ObjPADMData.birthDate = Obj.DOB.Value.ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrEmpty(Obj.DOA.ToString()))
                    {
                        ObjPADMData.weddingAnniversary = Obj.DOA.Value.ToString("dd/MM/yyyy");
                    }
                    
                    //ObjPADMData.customerCode = customerCode;
                    ObjPADMData.reqFromMobApp = true;

                    string stringjson = JsonConvert.SerializeObject(ObjPADMData);

                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(APIUpdateCustomer);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Headers.Add("Authorization", AuthKey);
                    httpWebRequest1.Headers.Add("X-key", XKey);
                    httpWebRequest1.Headers.Add("sProgramKey", "1");

                    httpWebRequest1.Method = "POST";
                    using (var streamWriter1 = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        streamWriter1.Write(stringjson);
                    }
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader1 = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        result = streamReader1.ReadToEnd();
                    }

                }
                else
                {
                    ObjPADMData.name = Obj.Name;
                    ObjPADMData.city = Obj.City;
                    ObjPADMData.state = Obj.State;
                    ObjPADMData.mobileNo = Obj.Mobileno;
                    ObjPADMData.address1 = Obj.Address;
                    ObjPADMData.address2 = Obj.City;
                    ObjPADMData.area = Obj.Area;
                    ObjPADMData.emailId = Obj.EmailId;
                    if (!string.IsNullOrEmpty(Obj.DOB.ToString()))
                    {
                        ObjPADMData.birthDate = Obj.DOB.Value.ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrEmpty(Obj.DOA.ToString()))
                    {
                        ObjPADMData.weddingAnniversary = Obj.DOA.Value.ToString("dd/MM/yyyy");
                    }
                    ObjPADMData.firstName = Obj.FirstName;
                    ObjPADMData.middleName = Obj.MiddleName;
                    ObjPADMData.surName = Obj.SurName;
                    
                    ObjPADMData.reqFromMobApp = true;

                    string stringjson = JsonConvert.SerializeObject(ObjPADMData);

                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(APIAddCustomer);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Accept = "application/json";
                    httpWebRequest1.Headers.Add("Authorization", AuthKey);
                    httpWebRequest1.Headers.Add("X-key", XKey);
                    httpWebRequest1.Headers.Add("sProgramKey", "1");

                    httpWebRequest1.Method = "POST";
                    using (var streamWriter1 = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        streamWriter1.Write(stringjson);
                    }
                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader1 = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        result = streamReader1.ReadToEnd();
                    }
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "SendDetailsToPOS");
            }
        }

        public DateTime IndianDatetime()
        {
            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);

            return Date;
        }
    }

}
