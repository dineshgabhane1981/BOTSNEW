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
                                   Id=c.Id,
                                   AddedDate = c.AddedDate,
                                   SpokenTo = c.SpokenTo,
                                   ContactNo = c.ContactNo,
                                   CallType = ct.CallType,
                                   FollowupDate = c.FollowupDate,
                                   CallMode = c.CallMode,
                                   Description = c.Description,
                                   ActionItems = c.ActionItems,
                                   AddedBy = cld.UserName,
                                   Status = c.Status
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
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.BOTS_TblDiscussion.AddOrUpdate(objDiscussion);
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

        public bool UpdateDiscussions(string id,string Desc,string Status,string LoginId)
        {
            BOTS_TblDiscussion objDiscussion = new BOTS_TblDiscussion();
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    int discussionId = Convert.ToInt32(id);
                    objDiscussion = context.BOTS_TblDiscussion.Where(x => x.Id == discussionId).FirstOrDefault();
                    objDiscussion.Status = Status;
                    objDiscussion.AddedBy = LoginId;
                    if (!string.IsNullOrEmpty(Desc))
                    {
                        objDiscussion.Description = objDiscussion.Description + ", " + Desc;
                    }
                    context.BOTS_TblDiscussion.AddOrUpdate(objDiscussion);
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
