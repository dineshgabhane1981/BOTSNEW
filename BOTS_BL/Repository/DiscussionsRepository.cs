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
                                      select new DiscussionDetails
                                      {
                                          AddedDate = c.AddedDate,
                                          SpokenTo = c.SpokenTo,
                                          ContactNo = c.ContactNo,
                                          CallType = ct.CallType,
                                          FollowupDate = c.FollowupDate,
                                          CallMode = c.CallMode
                                      }).OrderByDescending(x=>x.AddedDate).ToList();
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
