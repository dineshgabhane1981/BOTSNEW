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
using BOTS_BL.Models.FeedbackModule;
using System.Data.Entity.Core.Objects;

namespace BOTS_BL.Repository
{
    public class FeedbackModuleRepository
    {

        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();

        public List<tblGroupDetail> GetNeverOptForGroups()
        {
            List<tblGroupDetail> lstData = new List<tblGroupDetail>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.tblGroupDetails.Where(x => x.IsFeedback.Value == false).OrderBy(y => y.GroupName).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return lstData;
        }

        public List<FeedbackActiveGroup> GetActiveGroups()
        {
            List<FeedbackActiveGroup> lstData1 = new List<FeedbackActiveGroup>();
            List<FeedbackActiveGroup> lstData = new List<FeedbackActiveGroup>();
            try
            {
                using (var context = new CommonDBContext())
                {

                    lstData1 = (from c in context.tblGroupDetails
                                join cc in context.Feedback_FeedbackConfig on c.GroupId equals cc.GroupId
                                where cc.Status == "Active" || cc.Status == "Renew"
                                select new FeedbackActiveGroup
                                {
                                    GroupId = c.GroupId,
                                    GroupName = c.GroupName,
                                    Status = cc.Status,
                                    ValidFrom = cc.StartDate.Value,
                                    ValidTo = cc.EndDate.Value,
                                    Amount = cc.Fees
                                }).ToList();

                    foreach (var item in lstData1)
                    {

                        item.ExpiringInDays = (item.ValidTo.Date - DateTime.Today.Date).Days.ToString();
                        if (item.ValidTo.AddDays(7) > DateTime.Today)
                        {
                            lstData.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetActiveGroups");
            }
            return lstData;
        }

        public List<FeedbackDeActivatedGroup> GetDeActivatedGroups()
        {
            List<FeedbackDeActivatedGroup> lstData = new List<FeedbackDeActivatedGroup>();
            List<FeedbackDeActivatedGroup> lstData1 = new List<FeedbackDeActivatedGroup>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData1 = (from c in context.tblGroupDetails
                                join cc in context.Feedback_FeedbackConfig on c.GroupId equals cc.GroupId
                                //where cc.Status == "Stop"
                                select new FeedbackDeActivatedGroup
                                {
                                    GroupId = c.GroupId,
                                    GroupName = c.GroupName,
                                    Status = cc.Status,
                                    Reason = cc.StoppedReason,
                                    StoppedDate = cc.StoppedDate,
                                    ValidTo = cc.EndDate.Value,
                                    Amount = cc.Fees
                                }).ToList();

                    foreach (var item in lstData1)
                    {

                        if ((item.ValidTo.AddDays(7) < DateTime.Today) && item.Status != "Stop")
                        {
                            item.Status = "Expired";
                            lstData.Add(item);
                        }
                        if (item.Status == "Stop")
                        {
                            lstData.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDeActivatedGroups");
            }
            return lstData;
        }

        public Feedback_FeedbackConfig GetFeedbackByGroupId(string GroupId)
        {
            Feedback_FeedbackConfig objData = new Feedback_FeedbackConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    int gId = Convert.ToInt32(GroupId);
                    objData = context.Feedback_FeedbackConfig.Where(x => x.GroupId == gId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFeedbackByGroupId");
            }
            return objData;

        }

        public List<Feedback_ContentMaster> GetFeedbackMasterData()
        {
            List<Feedback_ContentMaster> lstData = new List<Feedback_ContentMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.Feedback_ContentMaster.OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFeedbackMasterData");
            }
            return lstData;
        }
        public bool EnableFeedbackModule(Feedback_FeedbackConfig objFeedback, List<Feedback_Content> contentData)
        {
            bool status = false;
            try
            {

                using (var context = new CommonDBContext())
                {
                    context.Feedback_FeedbackConfig.AddOrUpdate(objFeedback);
                    context.SaveChanges();
                    var groupDetails = context.tblGroupDetails.Where(x => x.GroupId == objFeedback.GroupId).FirstOrDefault();
                    groupDetails.IsFeedback = true;
                    context.SaveChanges();
                    if (contentData != null)
                    {
                        foreach (var item in contentData)
                        {
                            context.Feedback_Content.AddOrUpdate(item);
                            context.SaveChanges();
                        }
                    }
                    status = true;
                }
                if (status)
                {
                    if (contentData != null)
                    {
                        string connStr = CR.GetCustomerConnString(Convert.ToString(objFeedback.GroupId));
                        using (var contextdb = new BOTSDBContext(connStr))
                        {
                            string tableScript = "CREATE TABLE [dbo].[feedback_FeedbackMaster]([FeedbackId][int] IDENTITY(1, 1) NOT NULL,[GroupId] [varchar](4) NULL,[OutletId] [varchar](10) NULL," +
                                                "[MobileNo] [varchar](10) NULL,	[CustomerName] [varchar](100) NULL,	[QuestionId] [varchar](5) NULL,	[QuestionPoints] [int] NULL,	[DOB] [date] NULL," +
                                                "[DOA] [date] NULL,	[HowToKnowAbout] [varchar](10) NULL,[AddedDate] [datetime] NULL,[SalesRepresentative] [varchar](10) NULL, [Comments] [nvarchar](max) NULL, [AudioStream] [nvarchar](max) NULL, [CustomText] [nvarchar](max) NULL," +
                                                "CONSTRAINT[PK_FeedBackModuleMaster] PRIMARY KEY CLUSTERED([FeedbackId] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY]";
                            contextdb.Database.CreateIfNotExists();
                            contextdb.Database.ExecuteSqlCommand(tableScript);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableFeedbackModule");
            }

            return status;
        }

        public bool StopFeedbackModule(Feedback_FeedbackConfig objFeedback)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.Feedback_FeedbackConfig.AddOrUpdate(objFeedback);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "StopFeedbackModule");
            }

            return status;
        }

        public List<Feedback_Content> GetFeedback_Contents(string GroupId)
        {
            List<Feedback_Content> lstData = new List<Feedback_Content>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.Feedback_Content.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFeedback_Contents");
            }
            return lstData;
        }

        public List<Feedback_Content> GetFeedback_VisibleContents(string GroupId)
        {
            List<Feedback_Content> lstData = new List<Feedback_Content>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.Feedback_Content.Where(x => x.GroupId == GroupId && x.IsDisplay == true).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFeedback_VisibleContents");
            }
            return lstData;
        }

        public Feedback_Headings GetHeadings(string GroupId)
        {
            Feedback_Headings headings = new Feedback_Headings();
            try
            {
                using (var context = new CommonDBContext())
                {
                    headings = context.Feedback_Headings.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetHeadings");
            }
            return headings;
        }

        public Feedback_Questions GetQuestions(string GroupId)
        {
            Feedback_Questions questions = new Feedback_Questions();
            try
            {
                using (var context = new CommonDBContext())
                {
                    questions = context.Feedback_Questions.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetQuestions");
            }
            return questions;
        }
        public List<Feedback_Content> GetHomeHeading(string GroupId)
        {
            List<Feedback_Content> lstfbget = new List<Feedback_Content>();
            Feedback_Content objfeedback = new Feedback_Content();
            BrandDetail objbrandDetail = new BrandDetail();

            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    objbrandDetail = contextdb.BrandDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
                string imageurl = objbrandDetail.BrandLogoUrl;
                using (var context = new CommonDBContext())
                {
                    lstfbget = context.Feedback_Content.Where(x => x.GroupId == GroupId && x.IsDisplay == true && x.Section == "Home" && x.Type == "Heading").ToList();

                    lstfbget[0].ImagePath = imageurl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetHomeHeading");
            }
            return lstfbget;
        }
        public string GetLogo(string groupId)
        {
            string logoUrl = string.Empty;
            CustomerDetail objCustomerDetail = new CustomerDetail();
            string connStr = CR.GetCustomerConnString(groupId);
            try
            {
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    var objbrandDetail = contextdb.BrandDetails.Where(x => x.GroupId == groupId).FirstOrDefault();
                    logoUrl = objbrandDetail.BrandLogoUrl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLogo");
            }
            return logoUrl;

        }
        public string GetGroupName(string groupId)
        {
            string GroupNm = string.Empty;
            CustomerDetail objCustomerDetail = new CustomerDetail();
            try
            {
                string connStr = CR.GetCustomerConnString(groupId);
            
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    var objgroupDetail = contextdb.GroupDetails.Where(x => x.GroupId == groupId).FirstOrDefault();
                    GroupNm = objgroupDetail.GroupName;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupName");
            }
            return GroupNm;

        }

        public bool CheckActiveLink(string groupId)
        {
            bool isActive = true;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var grpId = Convert.ToInt32(groupId);
                    var config = context.Feedback_FeedbackConfig.Where(x => x.GroupId == grpId).FirstOrDefault();
                    if ((config.EndDate.Value.AddDays(7) < DateTime.Today))
                    {
                        isActive = false;
                    }
                    if (config.Status == "Stop")
                    {
                        isActive = false;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CheckActiveLink");
            }

            return isActive;
        }
        public List<Feedback_Content> GetFeedbackHeading(string GroupId)
        {
            List<Feedback_Content> lstfbget = new List<Feedback_Content>();
            Feedback_Content objfeedback = new Feedback_Content();
            BrandDetail objbrandDetail = new BrandDetail();

            try
            {
                using (var context = new CommonDBContext())
                {
                    lstfbget = context.Feedback_Content.Where(x => x.GroupId == GroupId && x.IsDisplay == true && x.Section == "FeedbackQuestions" && x.Type == "Heading").ToList();

                    // lstfbget[0].ImagePath = imageurl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFeedbackHeading");
            }
            return lstfbget;
        }
        public List<Feedback_Content> GetFeedbackQuestion(string GroupId)
        {
            List<Feedback_Content> lstfbget = new List<Feedback_Content>();
            Feedback_Content objfeedback = new Feedback_Content();
            BrandDetail objbrandDetail = new BrandDetail();

            try
            {
                using (var context = new CommonDBContext())
                {
                    lstfbget = context.Feedback_Content.Where(x => x.GroupId == GroupId && x.IsDisplay == true && x.Section == "FeedbackQuestions" && x.Type == "Question").ToList();

                    // lstfbget[0].ImagePath = imageurl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFeedbackQuestion");
            }
            return lstfbget;
        }
        public List<Feedback_Content> GetOtherinfoQuestion(string GroupId)
        {
            List<Feedback_Content> lstfbget = new List<Feedback_Content>();
            Feedback_Content objfeedback = new Feedback_Content();
            BrandDetail objbrandDetail = new BrandDetail();

            try
            {
                using (var context = new CommonDBContext())
                {
                    lstfbget = context.Feedback_Content.Where(x => x.GroupId == GroupId && x.IsDisplay == true && x.Section == "OtherInformation" && x.Type == "Question").ToList();

                    // lstfbget[0].ImagePath = imageurl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOtherinfoQuestion");
            }
            return lstfbget;
        }
        public List<Feedback_Content> GetOtherinfoHeading(string GroupId)
        {
            List<Feedback_Content> lstfbget = new List<Feedback_Content>();
            Feedback_Content objfeedback = new Feedback_Content();
            BrandDetail objbrandDetail = new BrandDetail();

            try
            {
                using (var context = new CommonDBContext())
                {
                    lstfbget = context.Feedback_Content.Where(x => x.GroupId == GroupId && x.IsDisplay == true && x.Section == "OtherInformation" && x.Type == "Heading").ToList();

                    // lstfbget[0].ImagePath = imageurl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOtherinfoHeading");
            }
            return lstfbget;
        }

        public bool UpdateFeedbackDetails(object[] HomeData, object[] QuestionData, object[] OtherInfoData, object[] OtherConfigData, object[] OutletMobileNosData, string GroupId, string LoginId)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    foreach (Dictionary<string, object> item in HomeData)
                    {
                        var Id = Convert.ToInt32(item["Id"]);
                        var objFeedback = context.Feedback_Content.Where(x => x.Id == Id && x.GroupId == GroupId).FirstOrDefault();
                        objFeedback.Text = Convert.ToString(item["Text"]);
                        objFeedback.IsDisplay = Convert.ToBoolean(item["IsDisplay"]);

                        objFeedback.UpdatedDate = DateTime.Now;
                        objFeedback.UpdatedBy = LoginId;

                        context.Feedback_Content.AddOrUpdate(objFeedback);
                        context.SaveChanges();
                    }
                    foreach (Dictionary<string, object> item in QuestionData)
                    {
                        var Id = Convert.ToInt32(item["Id"]);
                        var objFeedback = context.Feedback_Content.Where(x => x.Id == Id && x.GroupId == GroupId).FirstOrDefault();
                        objFeedback.Text = Convert.ToString(item["Text"]);
                        objFeedback.IsDisplay = Convert.ToBoolean(item["IsDisplay"]);
                        if (objFeedback.Type == "Question")
                        {
                            objFeedback.IsMandatory = Convert.ToString(item["IsMandatory"]);
                        }
                        if (objFeedback.Type == "Textbox")
                        {
                            objFeedback.IsMandatory = Convert.ToString(item["IsMandatory"]);
                        }
                        objFeedback.UpdatedDate = DateTime.Now;
                        objFeedback.UpdatedBy = LoginId;

                        context.Feedback_Content.AddOrUpdate(objFeedback);
                        context.SaveChanges();
                    }
                    foreach (Dictionary<string, object> item in OtherInfoData)
                    {
                        var Id = Convert.ToInt32(item["Id"]);
                        var objFeedback = context.Feedback_Content.Where(x => x.Id == Id && x.GroupId == GroupId).FirstOrDefault();
                        objFeedback.Text = Convert.ToString(item["Text"]);
                        objFeedback.IsDisplay = Convert.ToBoolean(item["IsDisplay"]);
                        if (objFeedback.Type == "Question")
                        {
                            objFeedback.IsMandatory = Convert.ToString(item["IsMandatory"]);
                        }
                        objFeedback.UpdatedDate = DateTime.Now;
                        objFeedback.UpdatedBy = LoginId;

                        context.Feedback_Content.AddOrUpdate(objFeedback);
                        context.SaveChanges();
                    }

                    Feedback_PointsAndMessages objPointsAndMessages = new Feedback_PointsAndMessages();

                    var objexisting = context.Feedback_PointsAndMessages.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    if (objexisting != null)
                    {
                        objPointsAndMessages = objexisting;
                    }
                    objPointsAndMessages.GroupId = GroupId;
                    foreach (Dictionary<string, object> item in OtherConfigData)
                    {
                        objPointsAndMessages.IsAddRepresentative = Convert.ToBoolean(item["IsRepresentative"]);
                        if (objPointsAndMessages.IsAddRepresentative)
                            objPointsAndMessages.RepresentativesList = Convert.ToString(item["RepresentativeList"]);
                        else
                            objPointsAndMessages.RepresentativesList = "";

                        objPointsAndMessages.IsFeedbackPoints = Convert.ToBoolean(item["IsFeedbackPoints"]);
                        if (objPointsAndMessages.IsFeedbackPoints)
                        {
                            objPointsAndMessages.AwardFeedbackPoints = Convert.ToInt32(item["AwardFeedbackPoints"]);
                            objPointsAndMessages.PointsConfig = Convert.ToString(item["PointsConfig"]);
                        }
                        else
                        {
                            objPointsAndMessages.AwardFeedbackPoints = 0;
                            objPointsAndMessages.PointsConfig = null;
                        }


                        objPointsAndMessages.MsgToCustomer = Convert.ToString(item["MsgToCustomer"]);
                        objPointsAndMessages.MsgNegativeFeedback = Convert.ToString(item["MsgNegativeFeedback"]);

                        objPointsAndMessages.IsMsgMissedFeedback = Convert.ToBoolean(item["IsMsgMissedFeedback"]);
                        if (objPointsAndMessages.IsMsgMissedFeedback)
                            objPointsAndMessages.MsgMissedFeedback = Convert.ToString(item["MsgMissedFeedback"]);
                        else
                            objPointsAndMessages.MsgMissedFeedback = "";

                        objPointsAndMessages.IsOtherInfoShow = Convert.ToBoolean(item["IsOtherInfoShow"]);
                        objPointsAndMessages.IsPositiveMessage = Convert.ToBoolean(item["IsPositiveMessage"]);
                        objPointsAndMessages.IsAudio = Convert.ToBoolean(item["IsAudio"]);
                        objPointsAndMessages.AudioMessageText = Convert.ToString(item["AudioMessageText"]);
                        objPointsAndMessages.IsGoogleReview = Convert.ToBoolean(item["IsGoogleReview"]);
                        objPointsAndMessages.GoogleReviewLink = Convert.ToString(item["GoogleReviewLink"]);
                        
                    }
                    if (objPointsAndMessages.AddedBy == null)
                    {
                        objPointsAndMessages.AddedBy = LoginId;
                        objPointsAndMessages.AddedDate = DateTime.Now;
                    }
                    else
                    {
                        objPointsAndMessages.UpdatedBy = LoginId;
                        objPointsAndMessages.UpdatedDate = DateTime.Now;
                    }
                    context.Feedback_PointsAndMessages.AddOrUpdate(objPointsAndMessages);
                    context.SaveChanges();

                    foreach (Dictionary<string, object> item in OutletMobileNosData)
                    {
                        Feedback_SMSNumbers objSMSNumber = new Feedback_SMSNumbers();
                        var outletId = Convert.ToString(item["OutletId"]);

                        var objExisting = context.Feedback_SMSNumbers.Where(x => x.GroupId == GroupId && x.OutletId == outletId).FirstOrDefault();
                        if (objExisting != null)
                        {
                            objSMSNumber = objExisting;
                            objSMSNumber.UpdatedBy = LoginId;
                            objSMSNumber.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            objSMSNumber.AddedBy = LoginId;
                            objSMSNumber.AddedDate = DateTime.Now;
                        }
                        objSMSNumber.GroupId = GroupId;
                        objSMSNumber.OutletId = outletId;
                        objSMSNumber.MobileNos = Convert.ToString(item["Numbers"]);

                        context.Feedback_SMSNumbers.AddOrUpdate(objSMSNumber);
                        context.SaveChanges();

                    }

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateFeedbackDetails");
            }
            return status;
        }

        public Feedback_PointsAndMessages GetPointsAndMessages(string GroupId)
        {
            Feedback_PointsAndMessages objData = new Feedback_PointsAndMessages();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objExisting = context.Feedback_PointsAndMessages.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    if (objExisting != null)
                    {
                        objData = objExisting;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsAndMessages");
            }

            return objData;
        }

        public string GetOutletMobileNos(string GroupId, string OutletId)
        {
            string MobileNo = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var outletDetails = context.Feedback_SMSNumbers.Where(x => x.GroupId == GroupId && x.OutletId == OutletId).FirstOrDefault();
                    if (outletDetails != null)
                    {
                        MobileNo = outletDetails.MobileNos;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletMobileNos");
            }

            return MobileNo;
        }

        public CustomerDetailwithFeedback GetCustomerInfo(string mobileNo, string GroupId)
        {
            CustomerDetail objcustdetails = new CustomerDetail();
            CustomerDetailwithFeedback obj = new CustomerDetailwithFeedback();
            feedback_FeedbackMaster objfeedback = new feedback_FeedbackMaster();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
            
                using (var context = new BOTSDBContext(connStr))
                {
                    objfeedback = context.feedback_FeedbackMaster.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.FeedbackId).FirstOrDefault();
                    var customer = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();

                    if (objfeedback != null && objfeedback.AddedDate.Value.Date == DateTime.Now.Date)
                    {
                        obj.IsFeedBackGiven = true;
                        if (objfeedback.DOA != null)
                        {
                            obj.IsDOA = true;
                        }
                        if (objfeedback.DOB != null)
                        {
                            obj.IsDateOfBirth = true;
                        }
                        if (objfeedback.HowToKnowAbout != null)
                        {
                            obj.IsHowtoKnow = true;
                        }
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
                    //}
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerInfo");
            }
            return obj;
        }

        public string SubmitRating(string mobileNo, string ranking, string GroupId, string salesid, string Comments, string outletId, string media,string customText)
        {
            string status = "false";
            // string smsresponce = "";
            feedback_FeedbackMaster objfeedback = new feedback_FeedbackMaster();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
                Feedback_PointsAndMessages feedbackpointsmsg = new Feedback_PointsAndMessages();
                Feedback_SMSNumbers objsmsnumber = new Feedback_SMSNumbers();
                using (var context = new CommonDBContext())
                {
                    feedbackpointsmsg = context.Feedback_PointsAndMessages.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    objsmsnumber = context.Feedback_SMSNumbers.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
                using (var context = new BOTSDBContext(connStr))
                {
                    CustomerDetail objcustdetails = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    //var lstOutlet = context.Database.SqlQuery<OutletList>("sp_GetOutletList @pi_GroupId", new SqlParameter("@pi_GroupId", GroupId)).ToList<OutletList>();
                    //var lstOutlet = context.OutletDetails.Where (x=> x.OutletId = outletId)

                    OutletDetail objoutlet = context.OutletDetails.Where(x => x.OutletId == outletId).FirstOrDefault();
                    TransactionMaster objtransactionMaster = new TransactionMaster();
                    PointsExpiry objpointsExpiry = new PointsExpiry();
                    var transaction = context.TransactionMasters.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.Datetime).Take(2).ToList();
                    var pointexpiry = context.PointsExpiries.Where(x => x.MobileNo == mobileNo).OrderByDescending(y => y.Datetime).Take(2).ToList();
                    int Combinedpoint = 0;
                    int point = 0;

                    var unQuotedString = ranking.TrimStart('[').TrimEnd(']');
                    string[] authorsList = unQuotedString.Split(',');
                    foreach (string author in authorsList)
                    {
                        string firstStringPosition = "rbtvariety";
                        string ipaddr = author.Substring(author.IndexOf(firstStringPosition) + firstStringPosition.Length);
                        char last = ipaddr[(ipaddr.Length - 2)];
                        string id = ipaddr.Remove(ipaddr.Length - 2, 2);

                        if (last == '1')
                        {
                            point = 4;
                        }
                        else if (last == '2')
                        {
                            point = 3;
                        }
                        else if (last == '3')
                        {
                            point = 2;
                        }
                        else if (last == '4')
                        {
                            point = 1;
                        }
                        if (objcustdetails != null)
                        {
                            objfeedback.CustomerName = objcustdetails.CustomerName;
                        }
                        else
                        {
                            objfeedback.CustomerName = "Member";
                        }
                        objfeedback.GroupId = GroupId;
                        if (GroupId == "1051")
                        {
                            objfeedback.MobileNo = outletId;
                        }
                        else
                        {
                            objfeedback.MobileNo = mobileNo;
                        }
                        objfeedback.QuestionPoints = point;
                        objfeedback.QuestionId = id;
                        objfeedback.OutletId = outletId;
                        objfeedback.SalesRepresentative = salesid;
                        objfeedback.Comments = Comments;
                        objfeedback.AudioStream = media;
                        objfeedback.CustomText = customText;
                        objfeedback.AddedDate = date;
                        Combinedpoint += point;
                        objfeedback = context.feedback_FeedbackMaster.Add(objfeedback);
                        context.SaveChanges();

                        status = "true";
                        //if (feedbackpointsmsg.IsOtherInfoShow)
                        //{
                        //    status = "true";
                        //}
                        //else
                        //{
                        //    status = "pointsGiven";
                        //}                    
                    }

                    if (GroupId != "1051")
                    {
                        if (Combinedpoint <= 4)
                        {
                            SMSDetail objsmsdetails = new SMSDetail();
                            // FeedBackMobileMaster objmobilemaster = context.FeedBackMobileMasters.Where(x => x.MessageId == "203").FirstOrDefault();
                            //  SMSEmailMaster objsmsemailmaster = context.SMSEmailMasters.Where(x => x.MessageId == "203").FirstOrDefault();


                            string message = feedbackpointsmsg.MsgNegativeFeedback;
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

                            // message = message.Replace("#31", Convert.ToString(ranking[0]));
                            // message = message.Replace("#32", Convert.ToString(ranking[1]));

                            //objsmsdetails = context.SMSDetails.Where(x => x.OutletId == outletId).FirstOrDefault();
                            // SendMessage(objsmsnumber.MobileNos, objsmsdetails.SenderId, message, objsmsdetails.TxnUrl, objsmsdetails.TxnUserName, objsmsdetails.TxnPassword);

                        }
                        if (feedbackpointsmsg.IsFeedbackPoints)
                        {
                            if (feedbackpointsmsg.PointsConfig == "OnFeedback")
                            {
                                if (objcustdetails == null)
                                {
                                    CustomerDetail objnewcust = new CustomerDetail();
                                    var CustomerId = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                                    DateTime datet = new DateTime(1900, 01, 01);
                                    var NewId = Convert.ToInt64(CustomerId) + 1;
                                    objnewcust.CustomerId = Convert.ToString(NewId);
                                    //objnewcust.Points = feedbackpointsmsg.AwardFeedbackPoints;

                                    objnewcust.CustomerName = "Member";

                                    objnewcust.CustomerCategory = null;
                                    objnewcust.CardNumber = "";
                                    objnewcust.CustomerThrough = "2";

                                    objnewcust.MaritalStatus = "";
                                    objnewcust.MemberGroupId = "1000";
                                    objnewcust.MobileNo = mobileNo;
                                    objnewcust.Status = "00";

                                    objnewcust.DOJ = date;
                                    objnewcust.EmailId = "";
                                    objnewcust.EnrollingOutlet = outletId;

                                    objnewcust.IsSMS = null;
                                    objnewcust.BillingCustomerId = null;

                                    context.CustomerDetails.Add(objnewcust);
                                    context.SaveChanges();

                                    objcustdetails = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                                }
                                var custpoint = objcustdetails.Points;
                                if (custpoint == null)
                                    objcustdetails.Points = feedbackpointsmsg.AwardFeedbackPoints;
                                else
                                    objcustdetails.Points = custpoint + feedbackpointsmsg.AwardFeedbackPoints;
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
                                objtransactionMaster.PointsEarned = feedbackpointsmsg.AwardFeedbackPoints;
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
                                objpointsExpiry.Points = feedbackpointsmsg.AwardFeedbackPoints;
                                objpointsExpiry.Status = "00";
                                objpointsExpiry.InvoiceNo = "B_Feedbackpoints";
                                objpointsExpiry.GroupId = GroupId;
                                objpointsExpiry.OriginalInvoiceNo = "";
                                objpointsExpiry.TransRefNo = null;
                                context.PointsExpiries.Add(objpointsExpiry);
                                context.SaveChanges();
                                if (feedbackpointsmsg.IsOtherInfoShow)
                                {
                                    status = "true";
                                }
                                else
                                {
                                    status = "pointsGiven";
                                }

                                if (transaction.Count > 0)
                                {
                                    if (transaction[0].InvoiceNo == "B_Feedbackpoints" && pointexpiry[0].InvoiceNo == "B_Feedbackpoints" && feedbackpointsmsg.PointsConfig == "OnOtherInfo")
                                    {
                                        var custpoint1 = objcustdetails.Points;
                                        objcustdetails.Points = custpoint1 + feedbackpointsmsg.AwardFeedbackPoints;
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
                                        objtransactionMaster.PointsEarned = feedbackpointsmsg.AwardFeedbackPoints;
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
                                        DateTime next1 = date.AddYears(1);
                                        var currentmonth1 = DateTime.DaysInMonth(next1.Year, next1.Month);

                                        if (next1.Day < currentmonth1)
                                        {
                                            var days = (currentmonth1 - next1.Day);
                                            next1 = date.AddDays(days).AddYears(1);
                                        }
                                        objpointsExpiry.ExpiryDate = next1;
                                        objpointsExpiry.Points = feedbackpointsmsg.AwardFeedbackPoints;
                                        objpointsExpiry.Status = "00";
                                        objpointsExpiry.InvoiceNo = "B_Feedbackpoints";
                                        objpointsExpiry.GroupId = GroupId;
                                        objpointsExpiry.OriginalInvoiceNo = "";
                                        objpointsExpiry.TransRefNo = null;
                                        context.PointsExpiries.Add(objpointsExpiry);
                                        context.SaveChanges();
                                        if (feedbackpointsmsg.IsOtherInfoShow)
                                        {
                                            status = "true";
                                        }
                                        else
                                        {
                                            status = "pointsGiven";
                                        }

                                    }
                                }
                            }
                        }
                        if (!feedbackpointsmsg.IsOtherInfoShow)
                        {
                            if (string.IsNullOrEmpty(feedbackpointsmsg.PointsConfig))
                            {
                                Feedback_SMSNumbers objsmsnumber1 = new Feedback_SMSNumbers();
                                string mobileNos = "";
                                string WATokenId = "";
                                using (var contextNew = new CommonDBContext())
                                {
                                    objsmsnumber1 = contextNew.Feedback_SMSNumbers.Where(x => x.GroupId == GroupId && x.OutletId == outletId).FirstOrDefault();
                                    mobileNos = objsmsnumber1.MobileNos;
                                }
                                string WAMsg = feedbackpointsmsg.MsgToCustomer;
                                var objsmsdetails = context.SMSDetails.Where(x => x.OutletId == outletId).FirstOrDefault();
                                if (objsmsdetails != null)
                                {
                                    WATokenId = objsmsdetails.WhatsAppTokenId;
                                }
                                if (GroupId == "1225")
                                {
                                    var SMSDetailsMapping = context.SMSOutletMappings.Where(x => x.OutletId == outletId).FirstOrDefault();
                                    if (SMSDetailsMapping != null)
                                    {
                                        WATokenId = SMSDetailsMapping.WATokenId;
                                    }
                                }
                                if (objcustdetails != null)
                                {
                                    WAMsg = WAMsg.Replace("#01", objcustdetails.CustomerName);
                                }
                                else
                                {
                                    WAMsg = WAMsg.Replace("#01", "Member");
                                }
                                WAMsg = WAMsg.Replace("#02", Convert.ToString(feedbackpointsmsg.AwardFeedbackPoints));

                                var bitly = context.MWP_Details.Where(x => x.MWP_Id == "08").Select(y => y.MWP_Name).FirstOrDefault();
                                WAMsg = WAMsg.Replace("#03", bitly);
                                var customerName = context.GroupDetails.Where(x => x.GroupId == GroupId).Select(y => y.GroupName).FirstOrDefault();
                                WAMsg = WAMsg.Replace("#04", customerName);

                                SendMessage(mobileNo, WAMsg, WATokenId);
                                if (feedbackpointsmsg.IsPositiveMessage)
                                {
                                    var SuccessMSG = "Successful Feedback registered by : #01";
                                    SuccessMSG = SuccessMSG.Replace("#01", mobileNo);
                                    SendMessage(mobileNos, SuccessMSG, WATokenId);
                                }

                                if (!string.IsNullOrEmpty(feedbackpointsmsg.MsgNegativeFeedback))
                                {
                                    if (GroupId == "1225")
                                    {
                                        if (Combinedpoint <= 9)
                                        {
                                            var negativeMSG = "Negative Feedback by : #01 | On date time #08 | Negative Score: #31 | Outlet Name : #09";
                                            negativeMSG = negativeMSG.Replace("#01", mobileNo);
                                            negativeMSG = negativeMSG.Replace("#08", Convert.ToString(date));
                                            negativeMSG = negativeMSG.Replace("#31", Convert.ToString(Combinedpoint));
                                            negativeMSG = negativeMSG.Replace("#09", objoutlet.OutletName);
                                            //message = message.Replace("#09", objoutlet.OutletName);

                                            SendMessage(feedbackpointsmsg.MsgNegativeFeedback, negativeMSG, WATokenId);
                                        }
                                    }
                                    else
                                    {
                                        if (Combinedpoint <= 6)
                                        {
                                            var negativeMSG = "Negative Feedback by : #01 | On date time #08 | Negative Score: #31 | Outlet Name : #09";
                                            negativeMSG = negativeMSG.Replace("#01", mobileNo);
                                            negativeMSG = negativeMSG.Replace("#08", Convert.ToString(date));
                                            negativeMSG = negativeMSG.Replace("#31", Convert.ToString(Combinedpoint));
                                            negativeMSG = negativeMSG.Replace("#09", objoutlet.OutletName);
                                            //message = message.Replace("#09", objoutlet.OutletName);

                                            SendMessage(feedbackpointsmsg.MsgNegativeFeedback, negativeMSG, WATokenId);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SubmitRatings");
            }
            return status;
        }

        public bool Submitotherinfo(string MemberName, string Gender, string BirthDt, string mobileNo, string AnniversaryDt, string Knowabt, string GroupId, string outletid)
        {
            bool status = false;
            // string smsresponce="";
            List<feedback_FeedbackMaster> lstfeedback = new List<feedback_FeedbackMaster>();
            TransactionMaster objtransactionMaster = new TransactionMaster();
            PointsExpiry objpointsExpiry = new PointsExpiry();
            CustomerDetail objnewcust = new CustomerDetail();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
                Feedback_PointsAndMessages feedbackpointsmsg = new Feedback_PointsAndMessages();
                Feedback_SMSNumbers objsmsnumber = new Feedback_SMSNumbers();
                string mobileNos = "";
                using (var context = new CommonDBContext())
                {
                    feedbackpointsmsg = context.Feedback_PointsAndMessages.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    objsmsnumber = context.Feedback_SMSNumbers.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    var objsmsnumber1 = context.Feedback_SMSNumbers.Where(x => x.GroupId == GroupId && x.OutletId == outletid).FirstOrDefault();
                    mobileNos = objsmsnumber1.MobileNos;
                }
                using (var context = new BOTSDBContext(connStr))
                {
                    int negativePoints = 0;
                    CustomerDetail objcustdetails = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    OutletDetail objoutlet = context.OutletDetails.Where(x => x.OutletId == outletid).FirstOrDefault();
                    FeedBackMaster objfeedback = new FeedBackMaster();
                    // lstfeedback = context.feedback_FeedbackMaster.Where(x => x.MobileNo == mobileNo && x.AddedDate == DateTime.Now.Date).ToList();
                    lstfeedback = context.feedback_FeedbackMaster.Where(x => x.MobileNo == mobileNo && x.AddedDate.Value > DateTime.Today).ToList();
                    if (feedbackpointsmsg.PointsConfig == "OnOtherInfo")
                    {
                        if (objcustdetails != null)
                        {
                            if (AnniversaryDt != null)
                            {
                                objcustdetails.AnniversaryDate = Convert.ToDateTime(AnniversaryDt);
                            }
                            var point = objcustdetails.Points;
                            objcustdetails.Points = point + feedbackpointsmsg.AwardFeedbackPoints;
                            context.CustomerDetails.AddOrUpdate(objcustdetails);
                            context.SaveChanges();
                        }
                        if (objcustdetails == null)
                        {
                            var CustomerId = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                            DateTime datet = new DateTime(1900, 01, 01);
                            var NewId = Convert.ToInt64(CustomerId) + 1;
                            objnewcust.CustomerId = Convert.ToString(NewId);
                            objnewcust.Points = feedbackpointsmsg.AwardFeedbackPoints;
                            if (string.IsNullOrEmpty(MemberName))
                                objnewcust.CustomerName = "Member";
                            else
                                objnewcust.CustomerName = MemberName;
                            objnewcust.CustomerCategory = null;
                            objnewcust.CardNumber = "";
                            objnewcust.CustomerThrough = "2";
                            if (!string.IsNullOrEmpty(BirthDt))
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
                        objtransactionMaster.InvoiceNo = "B_Feedbackpoints";
                        objtransactionMaster.InvoiceAmt = 0;
                        objtransactionMaster.Status = "06";
                        objtransactionMaster.PointsEarned = feedbackpointsmsg.AwardFeedbackPoints;
                        objtransactionMaster.PointsBurned = 0;
                        objtransactionMaster.CampaignPoints = 0;
                        objtransactionMaster.TxnAmt = 0;
                        objtransactionMaster.Synchronization = "";
                        objtransactionMaster.SyncDatetime = null;
                        context.TransactionMasters.Add(objtransactionMaster);
                        context.SaveChanges();
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
                        objpointsExpiry.Points = feedbackpointsmsg.AwardFeedbackPoints;
                        objpointsExpiry.Status = "00";
                        objpointsExpiry.InvoiceNo = "B_Feedbackpoints";

                        objpointsExpiry.GroupId = GroupId;
                        objpointsExpiry.OriginalInvoiceNo = "";
                        objpointsExpiry.TransRefNo = null;
                        context.PointsExpiries.Add(objpointsExpiry);
                        context.SaveChanges();

                    }
                    var isNegative = false;
                    foreach (var feedback in lstfeedback)
                    {
                        if (GroupId == "1181")
                        {
                            if (feedback.QuestionPoints <= 2)
                            {
                                isNegative = true;
                            }
                        }

                        negativePoints = negativePoints + feedback.QuestionPoints;

                        // feedback.Location = LiveIn;
                        feedback.HowToKnowAbout = Knowabt;
                        if (!string.IsNullOrEmpty(BirthDt))
                            feedback.DOB = Convert.ToDateTime(BirthDt);
                        feedback.MobileNo = mobileNo;
                        feedback.CustomerName = MemberName;
                        feedback.OutletId = outletid;
                        if (AnniversaryDt != null)
                        {
                            feedback.DOA = Convert.ToDateTime(AnniversaryDt);
                        }

                        context.feedback_FeedbackMaster.AddOrUpdate(feedback);
                        context.SaveChanges();
                        status = true;
                    }

                    string WAMsg = feedbackpointsmsg.MsgToCustomer;
                    var objsmsdetails = context.SMSDetails.Where(x => x.OutletId == outletid).FirstOrDefault();
                    if (objcustdetails != null)
                    {
                        WAMsg = WAMsg.Replace("#01", objcustdetails.CustomerName);
                    }
                    else
                    {
                        WAMsg = WAMsg.Replace("#01", "Member");
                    }
                    WAMsg = WAMsg.Replace("#02", Convert.ToString(feedbackpointsmsg.AwardFeedbackPoints));

                    var bitly = context.MWP_Details.Where(x => x.MWP_Id == "08").Select(y => y.MWP_Name).FirstOrDefault();
                    WAMsg = WAMsg.Replace("#03", bitly);
                    var customerName = context.GroupDetails.Where(x => x.GroupId == GroupId).Select(y => y.GroupName).FirstOrDefault();
                    WAMsg = WAMsg.Replace("#04", customerName);

                    SendMessage(mobileNo, WAMsg, objsmsdetails.WhatsAppTokenId);
                    if (feedbackpointsmsg.IsPositiveMessage)
                    {
                        var SuccessMSG = "Successful Feedback registered by : #01";
                        SuccessMSG = SuccessMSG.Replace("#01", mobileNo);
                        SendMessage(mobileNos, SuccessMSG, objsmsdetails.WhatsAppTokenId);
                    }
                    if (GroupId == "1181")
                    {
                        if (isNegative)
                        {
                            var negativeMSG = "Negative Feedback by : #01 | On date time #08| Negative Score: #31";
                            negativeMSG = negativeMSG.Replace("#01", mobileNo);
                            negativeMSG = negativeMSG.Replace("#08", Convert.ToString(date));
                            negativeMSG = negativeMSG.Replace("#31", Convert.ToString(negativePoints));

                            SendMessage(feedbackpointsmsg.MsgNegativeFeedback, negativeMSG, objsmsdetails.WhatsAppTokenId);
                        }
                    }
                    else if (negativePoints <= 6)
                    {
                        var negativeMSG = "Negative Feedback by : #01 | On date time #08| Negative Score: #31";
                        negativeMSG = negativeMSG.Replace("#01", mobileNo);
                        negativeMSG = negativeMSG.Replace("#08", Convert.ToString(date));
                        negativeMSG = negativeMSG.Replace("#31", Convert.ToString(negativePoints));

                        SendMessage(feedbackpointsmsg.MsgNegativeFeedback, negativeMSG, objsmsdetails.WhatsAppTokenId);
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Submitotherinfo");

            }
            return status;
        }
        public void SendBulkSMSMessageTxn(string MobileNo, string Sender, string MobileMessage)
        {
            string responseString;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("https://203.212.70.210/smpp/sendsms?username=bluhtployalty&password=blue8621&to=" + MobileNo + "&from=" + Sender + "&text=" + MobileMessage + "&category=bulk");
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
        public void SendMessage(string MobileNo, string MobileMessage, string _TokenNo)
        {
            try
            {
                MobileMessage = MobileMessage.Replace("#99", "&");
                MobileMessage = HttpUtility.UrlEncode(MobileMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", _TokenNo);
                sbposdata.AppendFormat("&phone=91{0}", MobileNo);
                sbposdata.AppendFormat("&message={0}", MobileMessage);

                //sbposdata.AppendFormat("&text={0}", _MobileMessage);
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
                var responseString = reader.ReadToEnd();
                //var J = JObject.Parse(responseString);
                //string J1 = J["status"].ToString();
                //if (J1 == "error")
                //{
                //    Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url, _CounterId));
                //    _job.Start();
                //}
                reader.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendMessage");

            }

        }
        public List<SelectListItem> GetSalesRepresentiveList(string GroupId)
        {
            CustomerDetail objcustdetails = new CustomerDetail();
            // string connStr = objCustRepo.GetCustomerConnString(GroupId);
            List<SelectListItem> lstsales = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var representative = context.Feedback_PointsAndMessages.Where(x => x.GroupId == GroupId).Select(x => x.RepresentativesList).FirstOrDefault();
                    string[] salesList = new string[0];
                    if (!string.IsNullOrEmpty(representative))
                        salesList = representative.Split(',');
                    int id = 1;
                    lstsales.Add(new SelectListItem
                    {
                        Text = "All",
                        Value = "0"
                    });

                    foreach (var item in salesList)
                    {

                        lstsales.Add(new SelectListItem
                        {
                            Text = item,
                            Value = id.ToString()
                        });
                        id++;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSalesRepresentiveList");

            }
            return lstsales;
        }
        public List<SelectListItem> GetHowToKnowAboutList()
        {
            CustomerDetail objcustdetails = new CustomerDetail();
            // string connStr = objCustRepo.GetCustomerConnString(GroupId);
            List<SelectListItem> lstlocation = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var knowabt = context.Feedback_KnowAboutYou.ToList();
                    lstlocation.Add(new SelectListItem
                    {
                        Text = "Please Select",
                        Value = "0"
                    });
                    foreach (var item in knowabt)
                    {

                        lstlocation.Add(new SelectListItem
                        {
                            Text = item.KnowAboutYou,
                            Value = Convert.ToString(item.KnowAboutYouId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetHowToKnowAboutList");

            }
            return lstlocation;
        }

        public List<int> GetDashboardNewExistingData(string GroupId, string OutletId, string FromDt, string ToDT, string neworexisting)
        {
            List<DashboardNewAndExisting> lstData = new List<DashboardNewAndExisting>();
            List<DashboardNewAndExisting> lstReturnData = new List<DashboardNewAndExisting>();
            List<int> lstCount = new List<int>();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    DateTime Frmdt = DateTime.Now.AddDays(-1).Date;
                    DateTime Tdt = DateTime.Now.Date;
                    if (!string.IsNullOrEmpty(FromDt))
                    {
                        Frmdt = Convert.ToDateTime(FromDt);
                    }
                    if (!string.IsNullOrEmpty(ToDT))
                    {
                        Tdt = Convert.ToDateTime(ToDT).AddDays(1);
                    }
                    lstData = contextdb.Database.SqlQuery<DashboardNewAndExisting>("select T.MobileNo,T.QuestionPoints,T.OutletId,T.AddedDate,(case when C.DOJ = cast(T.AddedDate as date) Then 'New' Else 'Existing' End) as MemberType from feedback_FeedbackMaster T left join CustomerDetails C on T.MobileNo = C.MobileNo").ToList();
                    if (neworexisting == "New")
                    {
                        lstData = lstData.Where(x => x.MemberType == "New").Select(y => y).ToList();
                    }
                    if (neworexisting == "Existing")
                    {
                        lstData = lstData.Where(x => x.MemberType == "Existing").Select(y => y).ToList();
                    }
                    if (!string.IsNullOrEmpty(OutletId))
                    {
                        lstData = lstData.Where(x => x.OutletId == OutletId).Select(y => y).ToList();
                    }
                    if (!string.IsNullOrEmpty(FromDt) && !string.IsNullOrEmpty(ToDT))
                    {
                        lstData = lstData.Where(x => x.AddedDate >= Frmdt).Select(y => y).ToList();
                        lstData = lstData.Where(x => x.AddedDate < Tdt).Select(y => y).ToList();
                    }

                    var uniqueMobileNo = lstData.GroupBy(x => x.MobileNo).Select(y => y.First()).ToList();

                    foreach (var item in uniqueMobileNo)
                    {
                        decimal sum = 0;
                        DashboardNewAndExisting objItem = new DashboardNewAndExisting();
                        var oneuserData = lstData.Where(x => x.MobileNo == item.MobileNo).ToList();
                        foreach (var points in oneuserData)
                        {
                            sum = sum + Convert.ToDecimal(points.QuestionPoints);
                        }
                        objItem.MobileNo = item.MobileNo;
                        objItem.AvgPoints = sum / oneuserData.Count();
                        lstReturnData.Add(objItem);
                    }

                    var lessthanone = lstReturnData.Where(x => x.AvgPoints < 1).Count();
                    var onetotwo = lstReturnData.Where(x => x.AvgPoints >= 1 && x.AvgPoints <= 2).Count();
                    var twotothree = lstReturnData.Where(x => x.AvgPoints > 2 && x.AvgPoints <= 3).Count();
                    var morethanthree = lstReturnData.Where(x => x.AvgPoints > 3).Count();

                    lstCount.Add(lessthanone);
                    lstCount.Add(onetotwo);
                    lstCount.Add(twotothree);
                    lstCount.Add(morethanthree);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardNewExistingData");

            }
            return lstCount;
        }
        public List<int> GetTimeWiseData(string GroupId, string OutletId, string FromDt, string ToDT, string timeIndicator)
        {
            List<int> lstCount = new List<int>();
            List<DashboardTimeWise> lstData = new List<DashboardTimeWise>();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    DateTime Frmdt = DateTime.Now.AddDays(-1).Date;
                    DateTime Tdt = DateTime.Now.Date;
                    if (!string.IsNullOrEmpty(FromDt))
                    {
                        Frmdt = Convert.ToDateTime(FromDt);
                    }
                    if (!string.IsNullOrEmpty(ToDT))
                    {
                        Tdt = Convert.ToDateTime(ToDT).AddDays(1);
                    }
                    var data = contextdb.feedback_FeedbackMaster.Where(x => x.GroupId == GroupId && x.AddedDate >= Frmdt && x.AddedDate < Tdt).ToList();
                    var uniqueMobileNoData = data.GroupBy(x => x.MobileNo).Select(y => y.First()).ToList();

                    foreach (var item in uniqueMobileNoData)
                    {
                        DashboardTimeWise objItem = new DashboardTimeWise();
                        decimal sum = 0;
                        var oneuserData = data.Where(x => x.MobileNo == item.MobileNo).ToList();
                        int rowCount = 0;
                        foreach (var points in oneuserData)
                        {
                            var timeslot = points.AddedDate.Value.TimeOfDay.Hours;
                            if (timeIndicator == "1")
                            {
                                if (timeslot <= 12)
                                {
                                    sum = sum + Convert.ToDecimal(points.QuestionPoints);
                                    rowCount++;
                                }
                            }
                            if (timeIndicator == "2")
                            {
                                if (timeslot > 12 && timeslot <= 15)
                                {
                                    sum = sum + Convert.ToDecimal(points.QuestionPoints);
                                    rowCount++;
                                }
                            }
                            if (timeIndicator == "3")
                            {
                                if (timeslot > 15 && timeslot <= 18)
                                {
                                    sum = sum + Convert.ToDecimal(points.QuestionPoints);
                                    rowCount++;
                                }
                            }
                            if (timeIndicator == "4")
                            {
                                if (timeslot > 18)
                                {
                                    sum = sum + Convert.ToDecimal(points.QuestionPoints);
                                    rowCount++;
                                }
                            }
                        }
                        if (sum == 0)
                            objItem.AvgPoints = 0;
                        else
                            objItem.AvgPoints = sum / rowCount;

                        lstData.Add(objItem);
                    }
                    var lessthanone = lstData.Where(x => x.AvgPoints < 1).Count();
                    var onetotwo = lstData.Where(x => x.AvgPoints >= 1 && x.AvgPoints <= 2).Count();
                    var twotothree = lstData.Where(x => x.AvgPoints > 2 && x.AvgPoints <= 3).Count();
                    var morethanthree = lstData.Where(x => x.AvgPoints > 3).Count();

                    lstCount.Add(lessthanone);
                    lstCount.Add(onetotwo);
                    lstCount.Add(twotothree);
                    lstCount.Add(morethanthree);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTimeWiseData");

            }
            return lstCount;
        }


        public List<DashboardOutletWise> GetOutletWiseData(string GroupId, string OutletId, string FromDt, string ToDT)
        {
            List<DashboardOutletWise> lstData = new List<DashboardOutletWise>();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    DateTime Frmdt = DateTime.Now.AddDays(-1).Date;
                    DateTime Tdt = DateTime.Now.Date;
                    if (!string.IsNullOrEmpty(FromDt))
                    {
                        Frmdt = Convert.ToDateTime(FromDt);
                    }
                    if (!string.IsNullOrEmpty(ToDT))
                    {
                        Tdt = Convert.ToDateTime(ToDT).AddDays(1);
                    }
                    var data = contextdb.feedback_FeedbackMaster.Where(x => x.GroupId == GroupId && x.AddedDate >= Frmdt && x.AddedDate < Tdt).ToList();
                    if (!string.IsNullOrEmpty(OutletId))
                    {
                        data = data.Where(x => x.OutletId == OutletId).ToList();
                    }
                    var uniqueOutlet = data.GroupBy(x => x.OutletId).Select(y => y.First()).ToList();
                    foreach (var item in uniqueOutlet)
                    {
                        var outlet = contextdb.OutletDetails.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                        var outletAvgPoints = contextdb.feedback_FeedbackMaster.Where(x => x.OutletId == item.OutletId && x.AddedDate >= Frmdt && x.AddedDate < Tdt).Average(y => y.QuestionPoints);

                        outletAvgPoints = Math.Round(outletAvgPoints, 2);
                        DashboardOutletWise objItem = new DashboardOutletWise();
                        objItem.AvgPoints = outletAvgPoints;
                        objItem.OutletName = outlet.OutletName;

                        lstData.Add(objItem);
                    }
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletWiseData");

            }
            return lstData;
        }

        public List<DashboardSourceWise> GetSourceWiseData(string GroupId, string OutletId, string FromDt, string ToDT)
        {
            List<DashboardSourceWise> lstData = new List<DashboardSourceWise>();
            List<DashboardSourceWise> lstData1 = new List<DashboardSourceWise>();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    DateTime Frmdt = DateTime.Now.AddDays(-1).Date;
                    DateTime Tdt = DateTime.Now.Date;
                    if (!string.IsNullOrEmpty(FromDt))
                    {
                        Frmdt = Convert.ToDateTime(FromDt);
                    }
                    if (!string.IsNullOrEmpty(ToDT))
                    {
                        Tdt = Convert.ToDateTime(ToDT).AddDays(1);
                    }
                    var data = contextdb.feedback_FeedbackMaster.Where(x => x.GroupId == GroupId && x.AddedDate >= Frmdt && x.AddedDate < Tdt).ToList();
                    if (!string.IsNullOrEmpty(OutletId))
                    {
                        data = data.Where(x => x.OutletId == OutletId).ToList();
                    }
                    var uniqueSource = data.GroupBy(x => x.HowToKnowAbout).Select(y => y.First()).ToList();
                    int total = 0;
                    foreach (var item in uniqueSource)
                    {
                        var numberOfCustomer = contextdb.feedback_FeedbackMaster.Where(x => x.HowToKnowAbout == item.HowToKnowAbout && x.AddedDate >= Frmdt && x.AddedDate < Tdt).Count();

                        total = total + numberOfCustomer;
                        DashboardSourceWise objItem = new DashboardSourceWise();
                        objItem.AvgPoints = numberOfCustomer;
                        if (item.HowToKnowAbout == null)
                        {
                            objItem.SourceName = "Other";
                        }
                        else
                        {
                            objItem.SourceName = item.HowToKnowAbout;
                        }

                        lstData.Add(objItem);
                    }
                    foreach (var item in lstData)
                    {
                        item.AvgPoints = Math.Round((item.AvgPoints * 100 / total), 2);
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSourceWiseData");

            }
            return lstData;
        }

        public List<DashboardSRWise> GetSRWiseData(string GroupId, string OutletId, string FromDt, string ToDT)
        {
            List<DashboardSRWise> lstData = new List<DashboardSRWise>();
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextdb = new BOTSDBContext(connStr))
                {
                    DateTime Frmdt = DateTime.Now.AddDays(-1).Date;
                    DateTime Tdt = DateTime.Now.Date;
                    if (!string.IsNullOrEmpty(FromDt))
                    {
                        Frmdt = Convert.ToDateTime(FromDt);
                    }
                    if (!string.IsNullOrEmpty(ToDT))
                    {
                        Tdt = Convert.ToDateTime(ToDT).AddDays(1);
                    }
                    var data = contextdb.feedback_FeedbackMaster.Where(x => x.GroupId == GroupId && x.AddedDate >= Frmdt && x.AddedDate < Tdt).ToList();
                    if (!string.IsNullOrEmpty(OutletId))
                    {
                        data = data.Where(x => x.OutletId == OutletId).ToList();
                    }
                    var uniqueSR = data.GroupBy(x => x.SalesRepresentative).Select(y => y.First()).ToList();
                    foreach (var item in uniqueSR)
                    {
                        var outletAvgPoints = contextdb.feedback_FeedbackMaster.Where(x => x.SalesRepresentative == item.SalesRepresentative && x.AddedDate >= Frmdt && x.AddedDate < Tdt).Average(y => y.QuestionPoints);

                        outletAvgPoints = Math.Round(outletAvgPoints, 2);
                        DashboardSRWise objItem = new DashboardSRWise();
                        objItem.AvgPoints = outletAvgPoints;
                        objItem.SRName = item.SalesRepresentative;

                        lstData.Add(objItem);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSRWiseData");

            }
            return lstData;
        }

        public List<Feedback_Report> GetReportData(string groupId, DateTime fromdt, DateTime todt, string salesr, string outletid)
        {

            List<Feedback_Report> lstobjreport = new List<Feedback_Report>();
            List<feedback_FeedbackMaster> objfeedbackmaster = new List<feedback_FeedbackMaster>();
            List<int> queid = new List<int>();
            string connStr = CR.GetCustomerConnString(groupId);
            DateTime today = DateTime.Today.Date;
            DateTime fmdt = new DateTime();
            fmdt = fmdt.Date;
            fromdt = fromdt.Date;
            todt = todt.Date;
            try
            {

                using (var context = new CommonDBContext())
                {
                    if (groupId != "1013")
                        queid = context.Feedback_Content.Where(x => x.GroupId == groupId && x.Section == "FeedbackQuestions" && x.Type == "Question" && x.IsDisplay == true).Select(x => x.Id).ToList();
                    else
                    {
                        queid.Add(1);
                        queid.Add(2);
                    }
                }
                using (var context = new BOTSDBContext(connStr))
                {
                    List<Feedback_MobileNo> objmobile = new List<Feedback_MobileNo>();
                    if (groupId != "1013")
                    {
                        objmobile = (from f in context.feedback_FeedbackMaster
                                         // where f.SalesRepresentative == salesr && f.OutletId == outletid && f.AddedDate >= fromdt && f.AddedDate <= todt
                                     select new Feedback_MobileNo
                                     {
                                         MobileNo = f.MobileNo,
                                         Datetime = f.AddedDate,
                                         OutletName = f.OutletId,
                                         SalesRName = f.SalesRepresentative
                                     }).Distinct().ToList();
                    }
                    else
                    {
                        objmobile = (from f in context.FeedBackMasters
                                     select new Feedback_MobileNo
                                     {
                                         MobileNo = f.MobileNo,
                                         Datetime = f.DOJ,
                                         OutletName = f.OutletId,
                                         SalesRName = ""
                                     }).Distinct().ToList();
                    }

                    if (salesr != "")
                    {
                        objmobile = (from f in objmobile
                                     where f.SalesRName == salesr //&& f.OutletId == outletid && f.AddedDate >= fromdt && f.AddedDate <= todt
                                     select new Feedback_MobileNo
                                     {
                                         MobileNo = f.MobileNo,
                                         Datetime = f.Datetime,
                                         OutletName = f.OutletName,
                                         SalesRName = f.SalesRName
                                     }).Distinct().ToList();
                    }
                    if (outletid != "")
                    {
                        objmobile = (from f in objmobile
                                     where f.OutletName == outletid
                                     select new Feedback_MobileNo
                                     {
                                         MobileNo = f.MobileNo,
                                         Datetime = f.Datetime,
                                         OutletName = f.OutletName,
                                         SalesRName = f.SalesRName

                                     }).Distinct().ToList();

                    }
                    if (fromdt != fmdt && todt != fmdt)
                    {

                        objmobile = (from f in objmobile
                                     where f.Datetime >= fromdt && f.Datetime <= todt
                                     select new Feedback_MobileNo
                                     {
                                         MobileNo = f.MobileNo,
                                         Datetime = f.Datetime,
                                         OutletName = f.OutletName,
                                         SalesRName = f.SalesRName
                                     }).Distinct().ToList();
                    }

                    // }
                    foreach (var item in objmobile)
                    {
                        Feedback_Report objreport = new Feedback_Report();

                        string a = "";
                        string b = "";
                        string c = "";
                        string d = "";
                        int i = 0;
                        if (groupId != "1013")
                        {
                            if (Convert.ToString(queid[i]) != null)
                            {
                                a = queid[i].ToString();
                                i++;
                            }
                            else
                            {
                                a = "0";
                            }
                            if (queid.Count > i)
                            {
                                if (Convert.ToString(queid[i]) != null)
                                {
                                    b = queid[i].ToString();
                                    i++;
                                }
                                else
                                {
                                    b = "0";
                                }
                            }
                            if (queid.Count > i)
                            {
                                if (Convert.ToString(queid[i]) != null)
                                {
                                    c = queid[i].ToString();
                                    i++;
                                }
                                else
                                {
                                    c = "0";
                                }
                            }
                            if (queid.Count > i)
                            {
                                if (Convert.ToString(queid[i]) != null)
                                {
                                    d = queid[i].ToString();
                                    i++;
                                }
                                else
                                {
                                    d = "0";
                                }
                            }
                        }
                        else
                        {
                            a = "1";
                            b = "2";
                        }
                        FeedbackData feedback = new FeedbackData();

                        if (groupId != "1013")
                        {
                            feedback = (from f in context.feedback_FeedbackMaster
                                        where f.MobileNo == item.MobileNo && DbFunctions.TruncateTime(f.AddedDate) == DbFunctions.TruncateTime(item.Datetime)
                                        group f by f.MobileNo into result
                                        select new FeedbackData
                                        {
                                            Mobilenumber = result.Key,
                                            q1 = result.Where(x => x.QuestionId == a).Select(x => x.QuestionPoints).FirstOrDefault(),
                                            q2 = result.Where(x => x.QuestionId == b).Select(x => x.QuestionPoints).FirstOrDefault(),
                                            q3 = result.Where(x => x.QuestionId == c).Select(x => x.QuestionPoints).FirstOrDefault(),
                                            q4 = result.Where(x => x.QuestionId == d).Select(x => x.QuestionPoints).FirstOrDefault(),
                                            salesR = result.Select(x => x.SalesRepresentative).FirstOrDefault(),
                                            outletid = result.Select(x => x.OutletId).FirstOrDefault(),
                                            howtoknow = result.Select(x => x.HowToKnowAbout).FirstOrDefault(),
                                            datetime = result.Select(x => x.AddedDate).FirstOrDefault(),
                                            comments = result.Select(x => x.Comments).FirstOrDefault(),
                                            AudioStream = result.Select(x => x.AudioStream).FirstOrDefault(),
                                        }).FirstOrDefault();
                        }
                        else
                        {
                            feedback = (from f in context.FeedBackMasters
                                        where f.MobileNo == item.MobileNo && DbFunctions.TruncateTime(f.DOJ) == DbFunctions.TruncateTime(item.Datetime)
                                        group f by f.MobileNo into result
                                        select new FeedbackData
                                        {
                                            Mobilenumber = result.Key,
                                            qq1 = result.Where(x => x.QuestionId == "1").Select(x => x.QuestionPoints).FirstOrDefault(),
                                            qq2 = result.Where(x => x.QuestionId == b).Select(x => x.QuestionPoints).FirstOrDefault(),
                                            qq3 = result.Where(x => x.QuestionId == c).Select(x => x.QuestionPoints).FirstOrDefault(),
                                            qq4 = result.Where(x => x.QuestionId == d).Select(x => x.QuestionPoints).FirstOrDefault(),
                                            salesR = "",
                                            outletid = result.Select(x => x.OutletId).FirstOrDefault(),
                                            howtoknow = result.Select(x => x.HowToKonwAbout).FirstOrDefault(),
                                            datetime = result.Select(x => x.DOJ).FirstOrDefault(),
                                            comments = "",
                                        }).FirstOrDefault();
                        }

                        if (feedback != null)
                        {
                            var invoiceamt = context.TransactionMasters.Where(x => x.MobileNo == feedback.Mobilenumber && x.Datetime == today).GroupBy(x => x.MobileNo)
                                .Select(g => new
                                {
                                    g.Key,
                                    totalamt = g.Sum(s => s.InvoiceAmt > 0 ? s.InvoiceAmt : 0),
                                    txnstatus = g.Sum(s => s.InvoiceAmt).ToString()
                                }).ToList();
                            var custinfo = context.CustomerDetails.Where(x => x.MobileNo == feedback.Mobilenumber).Select(g => new
                            {
                                cust = g.DOJ == today ? "1st time" : "Existing",
                                custname = g.CustomerName
                            }).ToList();

                            var outlet = context.OutletDetails.Where(x => x.OutletId == feedback.outletid).FirstOrDefault();
                            objreport.GroupId = groupId;
                            objreport.MemberName = custinfo.Select(x => x.custname).FirstOrDefault();
                            objreport.MobileNo = feedback.Mobilenumber;
                            if (groupId != "1013")
                            {
                                objreport.Q1 = feedback.q1;
                                objreport.Q2 = feedback.q2;
                            }
                            else
                            {
                                objreport.Q1 = Convert.ToInt32(feedback.qq1);
                                objreport.Q2 = Convert.ToInt32(feedback.qq2);
                            }
                            objreport.Q3 = feedback.q3;
                            objreport.Q4 = feedback.q4;
                            objreport.Source = feedback.howtoknow;
                            objreport.Datetime = feedback.datetime.ToString();
                            objreport.OutletName = outlet.OutletName;
                            objreport.SalesRName = feedback.salesR;
                            objreport.Comments = feedback.comments;
                            objreport.AudioStream = feedback.AudioStream;
                            objreport.Type = custinfo.Select(x => x.cust).FirstOrDefault();
                            objreport.TxnAmount = invoiceamt.Select(x => x.totalamt).FirstOrDefault();
                            objreport.Txn = invoiceamt.Select(x => x.txnstatus).FirstOrDefault();
                            lstobjreport.Add(objreport);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetReportData");
            }
            return lstobjreport;
        }

        public int GetFeedbackCountByGroupId(string GroupId)
        {
            var count = 0;
            try
            {
                string connStr = CR.GetCustomerConnString(GroupId);

                using (var contextdb = new BOTSDBContext(connStr))
                {
                    count = contextdb.feedback_FeedbackMaster.GroupBy(c => new
                    {
                        c.AddedDate,
                        c.MobileNo,
                    }).Count();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFeedbackCountByGroupId");
            }
            return count;
        }
    }
}
