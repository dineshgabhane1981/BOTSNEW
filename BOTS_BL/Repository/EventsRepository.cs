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

                            tableScript += "CREATE TABLE [dbo].[EventMemberDetails]([SLno][int] IDENTITY(1, 1) NOT NULL,[GroupId] [int] NOT NULL, [EventId] [int] NOT NULL,"+
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

        public bool SaveEventData(EventDetail Obj,string connectionstring)
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

        public List<EventDetail> GetListEvents(string GroupId,string connectionString)
        {
            List<EventDetail> listEvent = new List<EventDetail>();

            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    listEvent = context.EventDetails.Where(x => x.Status == null || x.Status != "Deleted").ToList();
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
            }

            return obj;
        }

        public EventModuleData GetCustomerDetails(string groupId, string Mobileno, string Place,string EventId, string connectionString)
        {
            EventModuleData obj = new EventModuleData();
            int eventId = Convert.ToInt32(EventId);
            try
            { 
                using (var context = new BOTSDBContext(connectionString))
                {
                    var statusavailable = context.EventMemberDetails.Where(e => e.EventId == eventId && e.Mobileno == Mobileno).Select(y => y.Mobileno).FirstOrDefault();
                    var pointsexp = context.EarnRules.Select(e => e.PointsExpiryVariableDate).FirstOrDefault();
                    int PointExp = Convert.ToInt32(pointsexp);
                    obj = context.Database.SqlQuery<EventModuleData>("select C.MobileNo,C.Points,C.CustomerName,C.Gender,C.DOB,C.AnniversaryDate,C.EmailId,min(CC.Address) as Address,min(C.OldMobileno) as AlternateMobileNo, CASE WHEN Max(cast(TM.Datetime as date)) = NULL THEN Max(cast(TM.Datetime as date)) ELSE Min(C.DOJ) END as LastTxnDate,DATEADD(MONTH, @PointExp, Max(cast(TM.Datetime as date))) as PointExp from CustomerDetails C Left join TransactionMaster TM on C.MobileNo = TM.MobileNo left join CustomerChild CC on CC.MobileNo = C.MobileNo and C.Status = '00' group by C.MobileNo, C.Points, C.CustomerName, C.EnrollingOutlet, C.Gender, C.DOB, C.AnniversaryDate, C.EmailId Having C.MobileNo = @Mobileno", new SqlParameter("@Mobileno", Mobileno), new SqlParameter("@PointExp", PointExp)).FirstOrDefault();

                    if(statusavailable != null)
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
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetails");
            }            
                return obj;
        }

        public bool SaveNewMemberData(EventMemberDetail objData, CustomerDetail objCustomerDetail, CustomerChild objCustomerChild, TransactionMaster objTM, string connectionstring)
        {
            bool result = false;
            string Message;
            Message = string.Empty;
            CustomerDetail TM2 = new CustomerDetail();
            CustomerChild objdata1 = new CustomerChild();
            OutletDetail objdata = new OutletDetail();
            TransactionMaster obj = new TransactionMaster();
            PointsExpiry obj1 = new PointsExpiry();
            using (var context = new BOTSDBContext(connectionstring))
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var bonusPoints = context.EventDetails.Where(x => x.EventId == objData.EventId).Select(y => y.BonusPoints).FirstOrDefault();
                        var existingCust = context.CustomerDetails.Where(x => x.MobileNo == objCustomerDetail.MobileNo).FirstOrDefault();

                        var ExpiryDays = context.EventDetails.Where(x => x.EventId == objData.EventId).Select(y => y.PointsExpiryDays).FirstOrDefault();
                        var Status = context.EventMemberDetails.Where(x => x.EventId == objData.EventId && x.Mobileno == objData.Mobileno).Select(y => y.Mobileno).FirstOrDefault();
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
                            objData.PointsGiven = bonusPoints;

                            objData.PointsGiven = 0;

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
                            var CustomerId = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                            var AdminOutletId = context.OutletDetails.Where(x => x.OutletName.Contains("Admin")).Select(y => y.OutletId).FirstOrDefault();
                            TM2.MobileNo = objCustomerDetail.MobileNo;
                            TM2.CustomerName = objCustomerDetail.CustomerName;
                            TM2.CustomerId = Convert.ToString(Convert.ToInt64(CustomerId) + 1);
                            TM2.CardNumber = objCustomerDetail.CardNumber;
                            TM2.EmailId = objCustomerDetail.EmailId;
                            TM2.DOB = objCustomerDetail.DOB;
                            TM2.AnniversaryDate = objCustomerDetail.AnniversaryDate;
                            TM2.Gender = objCustomerDetail.Gender;
                            TM2.DOJ = DateTime.Now;
                            TM2.EnrollingOutlet = AdminOutletId;
                            TM2.MemberGroupId = "1000";
                            TM2.CustomerThrough = "1";
                            TM2.Status = "00";
                            TM2.OldMobileNo = objCustomerDetail.OldMobileNo;
                            TM2.Points = bonusPoints;

                        }
                        else
                        {
                            TM2 = existingCust;
                            TM2.CustomerName = objCustomerDetail.CustomerName;
                            TM2.CardNumber = objCustomerDetail.CardNumber;
                            TM2.EmailId = objCustomerDetail.EmailId;
                            TM2.DOB = objCustomerDetail.DOB;
                            TM2.AnniversaryDate = objCustomerDetail.AnniversaryDate;
                            TM2.Gender = objCustomerDetail.Gender;
                            TM2.OldMobileNo = objCustomerDetail.OldMobileNo;

                            if (Status == null)
                            {
                                TM2.Points = TM2.Points + bonusPoints;
                            }
                            else
                            {
                                TM2.Points = TM2.Points;
                            }
                        }
                        context.CustomerDetails.AddOrUpdate(TM2);
                        context.SaveChanges();
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
                        context.CustomerChilds.AddOrUpdate(objdata1);
                        context.SaveChanges();

                        if (Status == null)
                        {
                            obj.CounterId = (TM2.EnrollingOutlet + "01");
                            obj.MobileNo = TM2.MobileNo;
                            obj.Datetime = DateTime.Now;
                            obj.TransType = "1";
                            obj.TransSource = "1";
                            obj.InvoiceNo = "Bonus";
                            obj.InvoiceAmt = 0;
                            obj.Status = "00";
                            obj.CustomerId = TM2.CustomerId;
                            obj.PointsEarned = bonusPoints;
                            obj.PointsBurned = 0;
                            obj.CampaignPoints = 0;
                            obj.TxnAmt = 0;
                            obj.CustomerPoints = TM2.Points;

                            context.TransactionMasters.AddOrUpdate(obj);
                            context.SaveChanges();

                            obj1.MobileNo = TM2.MobileNo;
                            obj1.CounterId = (TM2.EnrollingOutlet + "01");
                            obj1.EarnDate = DateTime.Now;
                            obj1.ExpiryDate = obj1.EarnDate.Value.AddDays(Convert.ToInt32(ExpiryDays));
                            obj1.Points = TM2.Points;
                            obj1.InvoiceNo = "Bonus";
                            obj1.Status = "00";
                            obj1.Datetime = DateTime.Now;
                            obj1.CustomerId = TM2.CustomerId;

                            context.PointsExpiries.AddOrUpdate(obj1);
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

        public List<EventMemberDetail> GetEventReport(string groupid,string fromDate,string toDate)
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
                    var objbrandDetail = context.BrandDetails.Where(x => x.GroupId == groupId).FirstOrDefault();
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

        public List<EventDetail> EventReportData(string groupid)
        {
            List<EventDetail> lstReportEventDate = new List<EventDetail>();
            var connStr = CR.GetCustomerConnString(groupid);
            //TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
            string Today = DateTime.Now.ToString("yyyy-MM-dd");
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

        public List<EventMemberDetail> EventMemberData(string GroupId, string EventId)
        {
            List<EventMemberDetail> lstReportData = new List<EventMemberDetail>();
            int eventid = Convert.ToInt32(EventId);
            var connStr = CR.GetCustomerConnString(GroupId);
            string Today = DateTime.Now.ToString("yyyy-MM-dd");
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

            string Today = DateTime.Now.ToString("yyyy-MM-dd");
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
                    obj = context.Database.SqlQuery<ReminderData>("select E.Mobileno,E.Name,E.PointsGiven,cast(E.DateOfRegistration as date) as DateOfRegistration,E.EventId from EventMemberDetails E where E.FirstRemDate = @Today and E.EventId = @EvenId and E.PointsGiven > 0 and E.Mobileno not in (select T.Mobileno from TransactionMaster T where (cast(T.Datetime as Date) between cast(@EventStartDate as date) and cast(@Today as Date)) and T.TransType = '2')", new SqlParameter("@Today", dateVal), new SqlParameter("@EventStartDate", dateVal1), new SqlParameter("@EvenId", EventIdInt)).ToList();
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

            string Today = DateTime.Now.ToString("yyyy-MM-dd");
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
                    obj = context.Database.SqlQuery<ReminderData>("select E.Mobileno,E.Name,E.PointsGiven,cast(E.DateOfRegistration as date) as DateOfRegistration,E.EventId from EventMemberDetails E where E.SecondRemDate = @Today and E.EventId = @EvenId and E.PointsGiven > 0 and E.Mobileno not in (select T.Mobileno from TransactionMaster T where (cast(T.Datetime as Date) between cast(@EventStartDate as date) and cast(@Today as date)) and T.TransType = '2')", new SqlParameter("@Today", dateVal), new SqlParameter("@EventStartDate", dateVal1), new SqlParameter("@EvenId", EventIdInt)).ToList();
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
    }
    
}
