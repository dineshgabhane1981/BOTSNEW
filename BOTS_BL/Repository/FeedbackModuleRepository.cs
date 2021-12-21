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
    public class FeedbackModuleRepository
    {
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
                        if(item.ValidTo.AddDays(7)>DateTime.Today)
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
                        
                        if ((item.ValidTo.AddDays(7) < DateTime.Today) && item.Status!="Stop")
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
        public bool EnableFeedbackModule(Feedback_FeedbackConfig objFeedback, Feedback_Headings headings, Feedback_Questions questions)
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
                    if(!string.IsNullOrEmpty(headings.GroupId))
                    {
                        context.Feedback_Headings.AddOrUpdate(headings);
                        context.SaveChanges();
                    }

                    if (!string.IsNullOrEmpty(questions.GroupId))
                    {
                        context.Feedback_Questions.AddOrUpdate(questions);
                        context.SaveChanges();
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


    }
}
