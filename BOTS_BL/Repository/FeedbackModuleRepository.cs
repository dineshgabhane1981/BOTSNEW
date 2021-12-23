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
                lstData = context.Feedback_ContentMaster.ToList();
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
                        foreach(var item in contentData)
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

        public List<Feedback_Content> GetFeedback(string GroupId)
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
                    //lstfbget = (from fh in context.Feedback_Content
                    //               where fh.GroupId == GroupId && fh.IsDisplay == true
                    //               select new Feedback_Content
                    //               {
                    //                   Id = fh.Id,
                    //                   Type =fh.Type,
                    //                   TypeId =fh.TypeId,
                    //                   GroupId = fh.GroupId,
                    //                   ImagePath = "",
                    //                   Text = fh.Text,
                    //                   IsMandatory = fh.IsMandatory,
                    //                   Section = fh.Section,
                    //                   IsDisplay=fh.IsDisplay,
                    //                   AddedDate = fh.AddedDate,
                    //                   AddedBy = fh.AddedBy,
                    //                   UpdatedDate = fh.UpdatedDate,
                    //                   UpdatedBy =fh.UpdatedBy
                    //               }).ToList();                   
                    lstfbget = context.Feedback_Content.Where(x => x.GroupId == GroupId && x.IsDisplay == true).ToList();

                    lstfbget[0].ImagePath = imageurl;
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
    }
}
