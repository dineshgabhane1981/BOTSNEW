using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.IO;
using BOTS_BL.Models.CommonDB;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class DiscussionsRepository
    {
        Exceptions newexception = new Exceptions();
        public List<DiscussionDetails> GetDiscussions(string GroupId)
        {
            List<DiscussionDetails> objData = new List<DiscussionDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //objData = context.BOTS_TblDiscussion.Where(x => x.GroupId == GroupId).ToList();
                    objData = (from c in context.BOTS_TblDiscussion
                               join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                               join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                               where c.GroupId == GroupId
                               select new DiscussionDetails
                               {
                                   Id = c.Id,
                                   AddedDate = c.AddedDate,
                                   SpokenTo = c.SpokenTo,
                                   ContactNo = c.ContactNo,
                                   CallType = ct.CallType,
                                   CustomerType = c.CustomerType,
                                   FollowupDate = c.FollowupDate,
                                   CallMode = c.CallMode,
                                   Description = c.Description,
                                   ActionItems = c.ActionItems,
                                   AddedBy = cld.UserName,
                                   Status = c.Status,
                                  
                               }).OrderByDescending(x => x.AddedDate).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objData;
        }

        public bool AddDiscussions(BOTS_TblDiscussion objDiscussion)
        {
            bool status = false;
            BOTS_TblSubDiscussionData objsubdiscussion = new BOTS_TblSubDiscussionData();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.BOTS_TblDiscussion.AddOrUpdate(objDiscussion);
                    context.SaveChanges();
                    if (objDiscussion.Status == "WIP")
                    {
                        objsubdiscussion.DiscussionId = objDiscussion.Id;
                        objsubdiscussion.GroupId = objDiscussion.GroupId;
                        objsubdiscussion.FollowupDate = objDiscussion.FollowupDate;
                        objsubdiscussion.Description = objDiscussion.Description;
                        objsubdiscussion.Status = objDiscussion.Status;
                        objsubdiscussion.UpdatedBy = objDiscussion.AddedBy;
                        context.BOTS_TblSubDiscussionData.AddOrUpdate(objsubdiscussion);
                        context.SaveChanges();
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, objDiscussion.GroupId);
            }

            return status;
        }

        public bool UpdateDiscussions(string id,string Desc,string Status,string LoginId)
        {
            BOTS_TblDiscussion objDiscussion = new BOTS_TblDiscussion();
            BOTS_TblSubDiscussionData objsubdiscussion = new BOTS_TblSubDiscussionData();
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    int discussionId = Convert.ToInt32(id);
                    objDiscussion = context.BOTS_TblDiscussion.Where(x => x.Id == discussionId).FirstOrDefault();
                    objDiscussion.Status = Status;
                    //objDiscussion.AddedBy = LoginId;
                    //if (!string.IsNullOrEmpty(Desc))
                    //{
                    //    objDiscussion.Description = objDiscussion.Description + "-----------\n" +  Desc;
                    //}
                    context.BOTS_TblDiscussion.AddOrUpdate(objDiscussion);
                    context.SaveChanges();

                    objsubdiscussion.DiscussionId = objDiscussion.Id;
                    objsubdiscussion.GroupId = objDiscussion.GroupId;
                    objsubdiscussion.FollowupDate = objDiscussion.FollowupDate;
                    objsubdiscussion.Description = objDiscussion.Description;
                    objsubdiscussion.Status = objDiscussion.Status;
                    objsubdiscussion.UpdatedBy = objDiscussion.AddedBy;
                    context.BOTS_TblSubDiscussionData.AddOrUpdate(objsubdiscussion);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, objDiscussion.GroupId);
            }

            return status;
        }

        public List<SubDiscussionData> GetNestedDiscussionList(int Id)
        {

           // List<BOTS_TblSubDiscussionData> lstsubdiscussionLists = new List<BOTS_TblSubDiscussionData>();
            List<SubDiscussionData> lstsubdiscussionlist = new List<SubDiscussionData>();
            try
            {
                using (var context = new CommonDBContext())
                {
                     // lstsubdiscussionLists = context.BOTS_TblSubDiscussionData.Where(x => x.DiscussionId == Id).OrderBy(x=>x.FollowupDate).ToList();


                    lstsubdiscussionlist = (from c in context.BOTS_TblDiscussion
                                            join ct in context.BOTS_TblSubDiscussionData on c.Id equals ct.DiscussionId
                                            join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                            where ct.DiscussionId == Id
                                            select new SubDiscussionData
                                            {
                                                SubDiscussionId = ct.SubDiscussionId,
                                                DiscussionId = ct.DiscussionId,
                                                //GroupId = ct.GroupId,
                                                FollowupDate = ct.FollowupDate.ToString(),
                                                Description = ct.Description,
                                                UpdatedBy = cld.UserName,
                                                Status = ct.Status

                                            }).OrderByDescending(x => x.FollowupDate).ToList();
                }


            }
            catch (Exception ex)
            {
                newexception.AddException(ex,"");
            }

            return lstsubdiscussionlist;
        }
        public List<SelectListItem> GetCallTypes()
        {
            List<SelectListItem> lstCallTypes = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var CallTypes = context.BOTS_TblCallTypes.ToList();
                foreach (var item in CallTypes)
                {
                    lstCallTypes.Add(new SelectListItem
                    {
                        Text = item.CallType,
                        Value = Convert.ToString(item.Id)
                    });
                }
            }
            return lstCallTypes;
        }
        public List<SelectListItem> GetSubCallTypes(int Id)
        {
            List<SelectListItem> lstSubCallTypes = new List<SelectListItem>();
            SelectListItem item1 = new SelectListItem();
            item1.Value = "0";
            item1.Text = "Please Select";
            lstSubCallTypes.Add(item1);
            using (var context = new CommonDBContext())
            {
                var SubCallTypes = context.BOTS_TblCallSubTypes.Where(x => x.CallTypeId == Id).ToList();
                foreach (var item in SubCallTypes)
                {
                    lstSubCallTypes.Add(new SelectListItem
                    {
                        Text = item.CallSubType,
                        Value = Convert.ToString(item.Id)
                    });
                }
            }
            return lstSubCallTypes;
        }
    }
}
