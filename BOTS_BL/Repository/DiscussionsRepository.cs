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
using System.Web;
using BOTS_BL.Models.CommonDB;
using System.Web.Mvc;
using System.Configuration;
using System.Threading;
using System.Net.Mail;
using System.Net.Mime;

namespace BOTS_BL.Repository
{
    public class DiscussionsRepository
    {
        Exceptions newexception = new Exceptions();

        public List<DiscussionDetails> GetDiscussions(string GroupId, string LoginType)
        {
            List<DiscussionDetails> objData = new List<DiscussionDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //objData = context.BOTS_TblDiscussion.Where(x => x.GroupId == GroupId).ToList();
                    if (LoginType == "9" || LoginType == "10")
                    {
                        objData = (from c in context.BOTS_TblDiscussion
                                   join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                   join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                   where c.GroupId == GroupId && (c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18 || c.CallType == 1)
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
                    else
                    {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objData;
        }
        public List<DiscussionDetails> GetAllDiscussions()
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
                newexception.AddException(ex, "");
            }
            return objData;
        }
        public bool AddDiscussions(BOTS_TblDiscussion objDiscussion, string _File, string FileName)
        {
            bool status = false,Filestatus = false;
            string path = string.Empty;


            BOTS_TblSubDiscussionData objsubdiscussion = new BOTS_TblSubDiscussionData();
            try
            {
                using (var context = new CommonDBContext())
                {
                    // Need to add Department and Member details in DB

                    string _FilePath = ConfigurationManager.AppSettings["DiscussionFileUpload"];
                    string _FileURL = ConfigurationManager.AppSettings["DiscussionDocumentURL"];
                    string FileLocation = _FilePath + "/" + FileName;

                    if (!string.IsNullOrEmpty(_File))
                    {
                        Filestatus = true;
                        byte[] imageBytes = Convert.FromBase64String(_File);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        path = HttpContext.Current.Server.MapPath("~/DiscussionFileUpload/" + FileName);
                        FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(fileNew);
                        fileNew.Close();
                        ms.Close();
                        objDiscussion.AttachedFile = _FileURL + FileName;
                    }

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


                    int _subtyprId = Convert.ToInt32(objDiscussion.SubCallType);

                    var DepartmentHead = context.tblDepartMembers.Where(x => x.Department == objDiscussion.Department && x.Role == "02").FirstOrDefault();
                    var Sendfrom = context.tblDepartMembers.Where(x => x.LoginId == objDiscussion.AddedBy && x.Department == objDiscussion.Department).FirstOrDefault();
                    var SendTo = context.tblDepartMembers.Where(x => x.Members == objDiscussion.AssignedMember).FirstOrDefault();
                    var _SubCallType = context.BOTS_TblCallSubTypes.Where(x => x.Id == _subtyprId).FirstOrDefault();
                    var _CallType = context.BOTS_TblCallTypes.Where(x=> x.Id == objDiscussion.CallType).FirstOrDefault();

                    string _DepartHead = DepartmentHead.EmailId;
                    //string _Sendfrom = Sendfrom.EmailId;
                    string _Addby = Sendfrom.Members;
                    string _SendTo = SendTo.EmailId;
                    string _Priority = objDiscussion.Priority;
                    string _Member = objDiscussion.AssignedMember;
                   string _CallTypetext  = _CallType.CallType;
                    string _subtypetext = _SubCallType.CallSubType;

                    Thread _job1 = new Thread(() => SendEmail(_DepartHead, _SendTo, Filestatus, path, _Addby, _Member, _subtypetext, _CallTypetext));
                    _job1.Start();


                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, objDiscussion.GroupId);
            }

            return status;
        }

        public bool UpdateDiscussions(string id, string Desc, string Status, string LoginId, string FollowupDate, string Reassign, string DoneFileName, string FileDone)
        {
            BOTS_TblDiscussion objDiscussion = new BOTS_TblDiscussion();
            BOTS_TblSubDiscussionData objsubdiscussion = new BOTS_TblSubDiscussionData();
            bool status = false;
            try
            {
                string _FilePath = ConfigurationManager.AppSettings["DiscussionFileUpload"];
                string FileLocation = _FilePath + "/" + DoneFileName;

                if (!string.IsNullOrEmpty(FileDone))
                {
                    byte[] imageBytes = Convert.FromBase64String(FileDone);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    var path = HttpContext.Current.Server.MapPath("~/DiscussionFileUpload/" + DoneFileName);
                    FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                    ms.WriteTo(fileNew);
                    fileNew.Close();
                    ms.Close();
                }

                using (var context = new CommonDBContext())
                {
                    int discussionId = Convert.ToInt32(id);
                    objDiscussion = context.BOTS_TblDiscussion.Where(x => x.Id == discussionId).FirstOrDefault();
                    objDiscussion.UpdatedDate = DateTime.Now;
                    objDiscussion.Status = Status;

                    context.BOTS_TblDiscussion.AddOrUpdate(objDiscussion);
                    context.SaveChanges();
                    if (!string.IsNullOrEmpty(FollowupDate))
                    {
                        objsubdiscussion.FollowupDate = Convert.ToDateTime(FollowupDate);
                    }
                    else
                    {
                        //objsubdiscussion.FollowupDate = objDiscussion.FollowupDate;
                    }
                    objsubdiscussion.DiscussionId = objDiscussion.Id;
                    objsubdiscussion.GroupId = objDiscussion.GroupId;

                    objsubdiscussion.Description = Desc;
                    objsubdiscussion.Status = objDiscussion.Status;
                    objsubdiscussion.UpdatedBy = LoginId;
                    objsubdiscussion.AddedDate = DateTime.Now;
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
                                            join cld in context.CustomerLoginDetails on ct.UpdatedBy equals cld.LoginId
                                            where ct.DiscussionId == Id
                                            select new SubDiscussionData
                                            {
                                                SubDiscussionId = ct.SubDiscussionId,
                                                DiscussionId = ct.DiscussionId,
                                                //GroupId = ct.GroupId,
                                                FollowupDate = ct.FollowupDate.ToString(),
                                                Description = ct.Description,
                                                UpdatedBy = cld.UserName,
                                                Status = ct.Status,
                                                AddedDate = ct.AddedDate

                                            }).OrderByDescending(x => x.FollowupDate).ToList();

                    foreach (var item in lstsubdiscussionlist)
                    {
                        if (item.AddedDate.HasValue)
                            item.UpdatedDate = Convert.ToString(item.AddedDate);
                        else
                            item.UpdatedDate = "--";
                    }
                }


            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNestedDiscussionList");
            }

            return lstsubdiscussionlist;
        }
        public List<SelectListItem> GetCallTypes(string LoginType)
        {
            List<SelectListItem> lstCallTypes = new List<SelectListItem>();
            //SelectListItem item1 = new SelectListItem();
            //item1.Value = "0";
            //item1.Text = "Please Select";
            //lstCallTypes.Add(item1);
            if (LoginType == "9" || LoginType == "10")
            {
                using (var context = new CommonDBContext())
                {
                    var CallTypes = context.BOTS_TblCallTypes.ToList();
                    foreach (var item in CallTypes)
                    {
                        if (Convert.ToString(item.Id) == "12" || Convert.ToString(item.Id) == "9" || Convert.ToString(item.Id) == "10" || Convert.ToString(item.Id) == "18" || Convert.ToString(item.Id) == "1")
                        {
                            lstCallTypes.Add(new SelectListItem
                            {
                                Text = item.CallType,
                                Value = Convert.ToString(item.Id)
                            });
                        }
                    }
                }
            }
            else
            {
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
        public List<SelectListItem> GetGroupDetails()
        {
            List<SelectListItem> lstgroupdetails = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var data = context.tblGroupDetails.Select(x => new { x.GroupId, x.GroupName }).OrderBy(x => x.GroupName).ToList();
                lstgroupdetails.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = "0"
                });
                foreach (var item in data)
                {

                    lstgroupdetails.Add(new SelectListItem
                    {
                        Text = item.GroupName,
                        Value = Convert.ToString(item.GroupId)
                    });

                }
            }
            return lstgroupdetails;
        }
        public SelectListItem[] CommonStatus()
        {
            return new SelectListItem[6] { new SelectListItem() { Text = "--Select--", Value = "0" }, new SelectListItem() { Text = "Completed", Value = "Completed" }, new SelectListItem() { Text = "WIP", Value = "WIP" }, new SelectListItem() { Text = "WIP>3days", Value = "WIP3" }, new SelectListItem() { Text = "WIP>7days", Value = "WIP7" }, new SelectListItem() { Text = "WIP>15days", Value = "WIP15" } };
        }
        public List<SelectListItem> GetRaisedby()
        {
            List<SelectListItem> lstRMAssigned = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {

                //var raised = context.tblRMAssigneds.Where(x => x.IsActive == true).ToList();
                var raised = context.CustomerLoginDetails.Where(x => x.UserStatus == true && (x.LoginType == "6" || x.LoginType == "7" || x.LoginType == "9" || x.LoginType == "10")).ToList();
                lstRMAssigned.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
                foreach (var item in raised)
                {
                    lstRMAssigned.Add(new SelectListItem
                    {
                        Text = item.UserName,
                        Value = Convert.ToString(item.LoginId)
                    });
                }
            }
            return lstRMAssigned;
        }
        public List<DiscussionDetails> GetfilteredDiscussionData(string status, int calltype, string groupnm, string fromDate, string toDate, string raisedby, string LoginType, string LoginId, bool IsFollowUp)
        {
            List<DiscussionDetails> lstdiscuss = new List<DiscussionDetails>();
            List<DiscussionDetails> lstdiscussOnBoarding = new List<DiscussionDetails>();
            List<BOTS_TblDiscussion> lsttbldiscuss = new List<BOTS_TblDiscussion>();
            using (var context = new CommonDBContext())
            {

                var list = context.BOTS_TblDiscussion.ToList();
                if (groupnm != "")
                {
                    list = list.Where(x => x.GroupId == groupnm).ToList();
                }
                if (list.Count != 0)
                {
                    if (status != "")
                    {
                        DateTime FromDate;
                        DateTime ToDate = DateTime.Now.Date.AddDays(1);
                        FromDate = ToDate.Date;
                        switch (status)
                        {
                            case "Completed":
                                list = list.Where(x => x.Status == "Completed").ToList();
                                break;

                            case "WIP":
                                list = list.Where(x => x.Status == "WIP").ToList();
                                break;

                            case "WIP3":
                                FromDate = ToDate.AddDays(-3);
                                list = list.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.Status == "WIP").ToList();
                                break;
                            case "WIP7":
                                ToDate = ToDate.AddDays(-3);
                                FromDate = ToDate.AddDays(-7);
                                list = list.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.Status == "WIP").ToList();
                                break;
                            case "WIP15":
                                FromDate = ToDate.AddDays(-7);
                                ToDate = ToDate.AddDays(-15);
                                list = list.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.Status == "WIP").ToList();
                                break;
                        }

                    }

                    if (calltype != 0)
                    {
                        list = list.Where(x => x.CallType == calltype).ToList();
                    }

                    if (fromDate != "" && toDate != "")
                    {
                        DateTime fmdt = Convert.ToDateTime(fromDate);
                        DateTime todt = Convert.ToDateTime(toDate);
                        todt = todt.AddDays(1);
                        list = list.Where(x => x.AddedDate > fmdt && x.AddedDate < todt).ToList();

                    }
                    if (raisedby != "")
                    {
                        var sublist = context.BOTS_TblSubDiscussionData.Where(x => x.UpdatedBy == raisedby).Select(y => y.DiscussionId).ToList();
                        var newlist = list.Where(x => x.AddedBy != raisedby && sublist.Contains(x.Id)).ToList();
                        
                        list = list.Where(x => x.AddedBy == raisedby).ToList();                        
                        list.AddRange(newlist);

                    }
                    if (LoginType == "1")
                    {
                        if (IsFollowUp)
                        {
                            lstdiscuss = (from c in list
                                          join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                          join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                          join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                          where c.Status == "WIP"

                                          select new DiscussionDetails
                                          {
                                              GroupName = gd.GroupName,
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


                            lstdiscussOnBoarding = (from c in list
                                                    join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                    join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                    join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                    where c.Status == "WIP"

                                                    select new DiscussionDetails
                                                    {
                                                        GroupName = gd.GroupName,
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
                        else
                        {
                            lstdiscuss = (from c in list
                                          join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                          join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                          join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId

                                          select new DiscussionDetails
                                          {
                                              GroupName = gd.GroupName,
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

                            lstdiscussOnBoarding = (from c in list
                                                    join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                    join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                    join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId

                                                    select new DiscussionDetails
                                                    {
                                                        GroupName = gd.GroupName,
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
                    else
                    {
                        if (LoginType == "9" || LoginType == "10")
                        {
                            if (IsFollowUp)
                            {
                                lstdiscuss = (from c in list
                                              join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                              join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                              join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                              where c.Status == "WIP" && (c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18)

                                              select new DiscussionDetails
                                              {
                                                  GroupName = gd.GroupName,
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

                                lstdiscussOnBoarding = (from c in list
                                                        join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                        join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                        join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                        where c.Status == "WIP" && (c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18)

                                                        select new DiscussionDetails
                                                        {
                                                            GroupName = gd.GroupName,
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
                            else
                            {
                                lstdiscuss = (from c in list
                                              join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                              join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                              join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                              where c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18

                                              select new DiscussionDetails
                                              {
                                                  GroupName = gd.GroupName,
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

                                lstdiscussOnBoarding = (from c in list
                                                        join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                        join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                        join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                        where c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18

                                                        select new DiscussionDetails
                                                        {
                                                            GroupName = gd.GroupName,
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
                        else
                        {
                            if (LoginType == "7")
                            {
                                if (IsFollowUp)
                                {
                                    lstdiscuss = (from c in list
                                                  join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                  join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                  join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                  where c.AddedBy == LoginId && c.Status == "WIP"

                                                  select new DiscussionDetails
                                                  {
                                                      GroupName = gd.GroupName,
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

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                            where c.AddedBy == LoginId && c.Status == "WIP"

                                                            select new DiscussionDetails
                                                            {
                                                                GroupName = gd.GroupName,
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
                                else
                                {
                                    lstdiscuss = (from c in list
                                                  join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                  join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                  join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                  where c.AddedBy == LoginId

                                                  select new DiscussionDetails
                                                  {
                                                      GroupName = gd.GroupName,
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

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                            where c.AddedBy == LoginId

                                                            select new DiscussionDetails
                                                            {
                                                                GroupName = gd.GroupName,
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
                            else
                            {
                                if (IsFollowUp)
                                {
                                    lstdiscuss = (from c in list
                                                  join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                  join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                  join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                  where c.Status == "WIP"
                                                  select new DiscussionDetails
                                                  {
                                                      GroupName = gd.GroupName,
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

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                            where c.Status == "WIP"
                                                            select new DiscussionDetails
                                                            {
                                                                GroupName = gd.GroupName,
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
                                else
                                {
                                    lstdiscuss = (from c in list
                                                  join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                  join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                  join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId

                                                  select new DiscussionDetails
                                                  {
                                                      GroupName = gd.GroupName,
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

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId

                                                            select new DiscussionDetails
                                                            {
                                                                GroupName = gd.GroupName,
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

                        }
                    }
                }
            }
            lstdiscuss.AddRange(lstdiscussOnBoarding);
            return lstdiscuss;
        }
        public List<DiscussionCount> GetDiscussionCountReport()
        {
            List<DiscussionCount> lstData = new List<DiscussionCount>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var raised = context.CustomerLoginDetails.Where(x => x.UserStatus == true && (x.LoginType == "6" || x.LoginType == "7" || x.LoginType == "9" || x.LoginType == "10")).ToList();
                    foreach (var item in raised)
                    {
                        DiscussionCount objItem = new DiscussionCount();
                        objItem.Name = item.UserName;

                        var FirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        objItem.TotalCount = context.BOTS_TblDiscussion.Where(x => x.AddedBy == item.LoginId && x.AddedDate >= FirstDay).Count();
                        var SubDiscussionCount = context.BOTS_TblSubDiscussionData.Where(x => x.UpdatedBy == item.LoginId && x.AddedDate >= FirstDay).GroupBy(y => y.DiscussionId).Count();
                        objItem.TotalCount = objItem.TotalCount + SubDiscussionCount;

                        var Yeasterday = DateTime.Today.AddDays(-2);
                        objItem.TotalCountYesterday = context.BOTS_TblDiscussion.Where(x => x.AddedBy == item.LoginId && x.AddedDate > Yeasterday && x.AddedDate < DateTime.Today).Count();
                        var SubDiscussionYesterdayCount = context.BOTS_TblSubDiscussionData.Where(x => x.UpdatedBy == item.LoginId && x.AddedDate > Yeasterday && x.AddedDate < DateTime.Today).GroupBy(y => y.DiscussionId).Count();
                        objItem.TotalCountYesterday = objItem.TotalCountYesterday + SubDiscussionYesterdayCount;

                        objItem.TotalWIPCount = context.BOTS_TblDiscussion.Where(x => x.AddedBy == item.LoginId && x.Status == "WIP").Count();
                        //var TotalSubCount = context.BOTS_TblSubDiscussionData.Where(x => x.UpdatedBy == item.LoginId && x.Status == "WIP").GroupBy(y => y.DiscussionId).Count();
                        //objItem.TotalWIPCount = objItem.TotalWIPCount + TotalSubCount;

                        var NewDate = DateTime.Today.AddDays(-3);
                        objItem.TotalWIPLast3Days = context.BOTS_TblDiscussion.Where(x => x.AddedBy == item.LoginId && x.Status == "WIP" && x.UpdatedDate >= NewDate).Count();


                        objItem.TotalWIPBefore3Days = context.BOTS_TblDiscussion.Where(x => x.AddedBy == item.LoginId && x.Status == "WIP" && x.UpdatedDate <= NewDate).Count();

                        lstData.Add(objItem);
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDiscussionCountReport");
            }

            return lstData;
        }
        public List<tblDepartMember> GetMemberdetails(string Department)
        {
            List<tblDepartMember> objtbldepart = new List<tblDepartMember>();

            try
            {
                using (var context = new CommonDBContext())
                {
                    objtbldepart = context.tblDepartMembers.Where(x => x.Department == Department).ToList();
                }

            }
            catch(Exception ex)
            {

            }
                return objtbldepart;
        }
        public List<tblDepartMember> GetReAssignMemberdetails(string id)
        {
            List<tblDepartMember> objtbldepart = new List<tblDepartMember>();
            int varid = Convert.ToInt32(id);
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objdata = context.BOTS_TblDiscussion.Where(x => x.Id == varid).ToList();

                    //objdata.
                    //objtbldepart = = context.tblDepartMembers.Where(x => x.Department == Department).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return objtbldepart;
        }

        public int GetTaskCount(string MemberName)
        {
            int taskCount = 0;
            try
            {
                using (var context = new CommonDBContext())
                {
                    taskCount = context.BOTS_TblDiscussion.Where(x => x.AssignedMember == MemberName && x.Status == "WIP").Count();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTaskCount");
            }
            return taskCount;
        }



        public void SendEmail(string _DepartHead, string _SendTo, bool _Filestatus, string _path,string _Addby, string _Member,string _SubCallType, string _CallType)
        {
            var from = ConfigurationManager.AppSettings["DiscussionEmail"].ToString();
            var PWD = ConfigurationManager.AppSettings["DiscussionEmailPwd"].ToString();

            using (MailMessage mail = new MailMessage(from, _SendTo))//tech@blueocktopus.in operations@blueocktopus.in
            {
                StringBuilder str = new StringBuilder();
                str.AppendLine("Dear "+ _Member + ",");
                str.AppendLine();
                str.AppendLine("You have assigned a task with sub call type - " + _SubCallType + "from "+ _Addby);
                str.AppendLine();
                str.AppendLine("Regards,");
                str.AppendLine(" - BlueOcktopus Team");

                mail.Subject = "Call type "+ _CallType + " SubCallType " + _SubCallType;
                mail.SubjectEncoding = System.Text.Encoding.Default;
                mail.CC.Add(_DepartHead);
                mail.Body = str.ToString();
                mail.IsBodyHtml = false;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                if(!string.IsNullOrEmpty(_path))
                {
                    Attachment data = new Attachment(_path, MediaTypeNames.Application.Octet);
                    mail.Attachments.Add(data);
                }
                
                
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.zoho.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from, PWD);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
                //TempData["Status"] = "File Uploaded successfully";
                //status = true;
            }
            
        }

    }
}
