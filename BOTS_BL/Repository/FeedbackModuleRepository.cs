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
            using (var context = new CommonDBContext())
            {
                int gId = Convert.ToInt32(GroupId);
                objData = context.Feedback_FeedbackConfig.Where(x => x.GroupId == gId).FirstOrDefault();
            }
            return objData;

        }

        public List<Feedback_ContentMaster> GetFeedbackMasterData()
        {
            List<Feedback_ContentMaster> lstData = new List<Feedback_ContentMaster>();
            using (var context = new CommonDBContext())
            {
                lstData = context.Feedback_ContentMaster.OrderBy(x=>x.Id).ToList();
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
                newexception.AddException(ex, "GetHeadings");
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
                newexception.AddException(ex, "Getfeedback");
            }
            return lstfbget;
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
                newexception.AddException(ex, "Getfeedback");
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
                newexception.AddException(ex, "Getfeedback");
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
                newexception.AddException(ex, "Getfeedback");
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
                newexception.AddException(ex, "Getfeedback");
            }
            return lstfbget;
        }
        public List<SelectListItem> GetHowToKnowAboutList()
        {
            CustomerDetail objcustdetails = new CustomerDetail();
            // string connStr = objCustRepo.GetCustomerConnString(GroupId);
            List<SelectListItem> lstlocation = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var knowabt = context.Feedback_KnowAboutYou.ToList();

                foreach (var item in knowabt)
                {
                    lstlocation.Add(new SelectListItem
                    {
                        Text = item.KnowAboutYou,
                        Value = Convert.ToString(item.KnowAboutYouId)
                    });
                }
            }
            return lstlocation;
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
                        //objFeedback.IsMandatory = Convert.ToInt32(item["IsMandatory"]);
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
                            objFeedback.IsMandatory = Convert.ToInt32(item["IsMandatory"]);
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
                            objFeedback.IsMandatory = Convert.ToInt32(item["IsMandatory"]);
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
                            objPointsAndMessages.AwardFeedbackPoints = Convert.ToInt32(item["AwardFeedbackPoints"]);
                        else
                            objPointsAndMessages.AwardFeedbackPoints = 0;

                        objPointsAndMessages.MsgToCustomer = Convert.ToString(item["MsgToCustomer"]);
                        objPointsAndMessages.MsgNegativeFeedback = Convert.ToString(item["MsgNegativeFeedback"]);

                        objPointsAndMessages.IsMsgMissedFeedback = Convert.ToBoolean(item["IsMsgMissedFeedback"]);
                        if (objPointsAndMessages.IsMsgMissedFeedback)
                            objPointsAndMessages.MsgMissedFeedback = Convert.ToString(item["MsgMissedFeedback"]);
                        else
                            objPointsAndMessages.MsgMissedFeedback = "";
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
                    if(objExisting !=null)
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

    }
}
