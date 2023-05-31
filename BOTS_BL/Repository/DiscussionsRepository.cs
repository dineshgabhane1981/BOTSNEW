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
using System.Globalization;
using System.Xml;

namespace BOTS_BL.Repository
{
    public class DiscussionsRepository
    {
        Exceptions newexception = new Exceptions();

        public List<DiscussionDetails> GetDiscussions(string GroupId, string LoginType, string LoginId, string LoginName)
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
                                       AssignedMember = c.AssignedMember,
                                       Priority = c.Priority


                                   }).OrderBy(y => y.Priority).OrderByDescending(x => x.AddedDate).ToList();
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
                                       AssignedMember = c.AssignedMember,
                                       Priority = c.Priority

                                   }).OrderBy(y => y.Priority).OrderByDescending(x => x.AddedDate).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDiscussions");
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
                                   AssignedMember = c.AssignedMember,


                               }).OrderByDescending(x => x.AddedDate).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllDiscussions");
            }
            return objData;
        }
        public bool AddDiscussions(BOTS_TblDiscussion objDiscussion, string _File, string FileName)
        {
            XmlDocument doc = new XmlDocument();
            var xmlpath = ConfigurationManager.AppSettings["DiscussionScripts"].ToString();
            doc.Load(xmlpath);

            tblDiscussionCustomerData ObjDisCustomerData = new tblDiscussionCustomerData();

            bool status = false;
            string path = string.Empty;
            string Priority = string.Empty;
            int _subtyprId = 0;

            BOTS_TblSubDiscussionData objsubdiscussion = new BOTS_TblSubDiscussionData();
            try
            {
                using (var context = new CommonDBContext())
                {
                    // Need to add Department and Member details in DB

                    string _FilePath = ConfigurationManager.AppSettings["DiscussionFileUpload"];
                    string _FileURL = ConfigurationManager.AppSettings["DiscussionDocumentURL"];
                    string FileLocation = _FilePath + "/" + FileName;

                    int _GroupId = Convert.ToInt32(objDiscussion.GroupId);
                    var _GroupDetails = context.tblGroupDetails.Where(x => x.GroupId == _GroupId).FirstOrDefault();
                    string _GroupName = string.Empty;
                    if (_GroupDetails == null)
                    {
                        var onBoardingGroupDetail = context.BOTS_TblGroupMaster.Where(x => x.GroupId == objDiscussion.GroupId).FirstOrDefault();
                        _GroupName = onBoardingGroupDetail.GroupName;
                    }
                    else
                    {
                        _GroupName = _GroupDetails.RetailName;
                    }
                    string Script = string.Empty;

                    if (!string.IsNullOrEmpty(_File))
                    {
                        if (Directory.Exists(_FilePath))
                        {
                            var GroupFolder = _FilePath + "/" + _GroupName;
                            if (!Directory.Exists(GroupFolder))
                            {
                                Directory.CreateDirectory(GroupFolder);
                            }
                            byte[] imageBytes = Convert.FromBase64String(_File);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            path = HttpContext.Current.Server.MapPath("~/DiscussionFileUpload/" + _GroupName + "/" + FileName);
                            FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(fileNew);
                            fileNew.Close();
                            ms.Close();
                            objDiscussion.AttachedFile = _FileURL + _GroupName + "/" + FileName;
                        }
                        else
                        {
                            Directory.CreateDirectory(_FilePath);
                            byte[] imageBytes = Convert.FromBase64String(_File);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            path = HttpContext.Current.Server.MapPath("~/DiscussionFileUpload/" + _GroupName + "/" + FileName);
                            FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(fileNew);
                            fileNew.Close();
                            ms.Close();
                            objDiscussion.AttachedFile = _FileURL + _GroupName + "/" + FileName;
                        }
                    }

                    if (objDiscussion.SubCallType != "25" && objDiscussion.SubCallType != "26" && objDiscussion.SubCallType != "27")
                    {
                        objDiscussion.DiscussionDoneNotDone = null;
                    }
                    else
                    {
                        if (objDiscussion.DiscussionDoneNotDone == "Done")
                        {
                            objDiscussion.DiscussionDoneNotDone = "1";
                        }
                        else if (objDiscussion.DiscussionDoneNotDone == "Reschedule")
                        {
                            objDiscussion.DiscussionDoneNotDone = "2";
                        }
                        else
                        {
                            objDiscussion.DiscussionDoneNotDone = "0";
                        }
                    }
                    if (objDiscussion.AssignedMember == "Please Select")
                    {
                        objDiscussion.AssignedMember = null;
                    }


                    if (objDiscussion.Priority == "Critical")
                    {
                        Priority = "Critical";
                        objDiscussion.Priority = "1";
                    }
                    else if (objDiscussion.Priority == "High")
                    {
                        Priority = "High";
                        objDiscussion.Priority = "2";
                    }
                    else if (objDiscussion.Priority == "Medium")
                    {
                        Priority = "Medium";
                        objDiscussion.Priority = "3";
                    }
                    else if (objDiscussion.Priority == "Low")
                    {
                        Priority = "Low";
                        objDiscussion.Priority = "4";
                    }
                    else
                    {
                        Priority = "";
                        objDiscussion.Priority = "5";
                    }

                    if (objDiscussion.SubCallType != "Please Select")
                    {
                        _subtyprId = Convert.ToInt32(objDiscussion.SubCallType);
                    }
                    else
                    {
                        objDiscussion.SubCallType = "69";
                    }

                    ObjDisCustomerData.CustomerName = objDiscussion.SpokenTo;
                    ObjDisCustomerData.MobileNo = objDiscussion.ContactNo;
                    ObjDisCustomerData.GroupId = objDiscussion.GroupId;
                    var TMobileno = context.tblDiscussionCustomerDatas.Where(x => x.MobileNo == objDiscussion.ContactNo && x.GroupId == objDiscussion.GroupId).Select(y => y.MobileNo).FirstOrDefault();
                    if (TMobileno == null)
                    {
                        context.tblDiscussionCustomerDatas.AddOrUpdate(ObjDisCustomerData);
                        context.SaveChanges();
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
                        objsubdiscussion.ReassignedMember = objDiscussion.AssignedMember;
                        objsubdiscussion.AttachedFile = _FileURL + _GroupName + "/" + FileName;
                        objsubdiscussion.FileName = FileName;
                        context.BOTS_TblSubDiscussionData.AddOrUpdate(objsubdiscussion);
                        context.SaveChanges();
                    }
                    status = true;

                    var Sendfrom = context.tblDepartMembers.Where(x => x.LoginId == objDiscussion.AddedBy).FirstOrDefault();
                    var DepartmentHead = context.tblDepartMembers.Where(x => x.Department == Sendfrom.Department && x.Role == "02").FirstOrDefault();
                    var SendTo = context.tblDepartMembers.Where(x => x.Members == objDiscussion.AssignedMember).FirstOrDefault();
                    var _SubCallType = context.BOTS_TblCallSubTypes.Where(x => x.Id == _subtyprId).FirstOrDefault();
                    var _CallType = context.BOTS_TblCallTypes.Where(x => x.Id == objDiscussion.CallType).FirstOrDefault();
                    var _WAGroupCode = context.WAReports.Where(x => x.GroupId == objDiscussion.GroupId && x.SMSStatus == "0").FirstOrDefault();

                    if (_GroupId == 1051)
                    {
                        _WAGroupCode = context.WAReports.Where(x => x.GroupId == objDiscussion.GroupId && x.SMSStatus == "5").FirstOrDefault();
                    }

                    EmailDetails ObjEmailData = new EmailDetails();

                    ObjEmailData.DepartHead = DepartmentHead.EmailId;
                    ObjEmailData.DepartHeadName = DepartmentHead.Members;
                    ObjEmailData.Addby = Sendfrom.Members;
                    ObjEmailData.AddbyEmail = Sendfrom.EmailId;

                    if (SendTo != null)
                    {
                        ObjEmailData.SendTo = SendTo.EmailId;
                    }
                    else
                    {
                        ObjEmailData.SendTo = DepartmentHead.EmailId;
                    }

                    ObjEmailData.Priority = Priority;
                    ObjEmailData.Member = objDiscussion.AssignedMember;
                    ObjEmailData.CallTypetext = _CallType.CallType;
                    if (_SubCallType != null)
                    {
                        ObjEmailData.subtypetext = _SubCallType.CallSubType;
                    }

                    ObjEmailData.GroupName = _GroupName;
                    ObjEmailData.id = objDiscussion.Id;
                    ObjEmailData.Description = objDiscussion.Description;
                    ObjEmailData.FilePath = path;
                    ObjEmailData.TeamName = Sendfrom.Department;
                    ObjEmailData.FromName = Sendfrom.Members;


                    if (objDiscussion.SubCallType == "25" || objDiscussion.SubCallType == "26" || objDiscussion.SubCallType == "27")
                    {
                        if (objDiscussion.SubCallType == "27" && objDiscussion.DiscussionDoneNotDone == "1")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DiscussionFirstDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "27" && objDiscussion.DiscussionDoneNotDone == "0")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DiscussionFirstNotDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "26" && objDiscussion.DiscussionDoneNotDone == "1")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/IdeasCampaignDiscussionDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "26" && objDiscussion.DiscussionDoneNotDone == "0")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/IdeasCampaignDiscussionNotDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "25" && objDiscussion.DiscussionDoneNotDone == "1")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DashboardCampaignDiscussionDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "25" && objDiscussion.DiscussionDoneNotDone == "0")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DashboardCampaignDiscussionNotDone");
                            Script = node.InnerText;
                        }
                    }
                    MessageDetails ObjMsgData = new MessageDetails();
                    if (_WAGroupCode != null)
                    {
                        ObjMsgData.Mobileno = _WAGroupCode.GroupCode;
                        ObjMsgData.Description = objDiscussion.Description;
                        ObjMsgData.GroupName = _GroupName;
                        ObjMsgData.BOEmpName = Sendfrom.Members;
                        ObjMsgData.Addby = objDiscussion.AddedBy;
                        ObjMsgData.Message = Script;
                        ObjMsgData.SpokenTo = objDiscussion.SpokenTo;
                    }

                    if (objDiscussion.AssignedMember == null)
                    {
                        if (objDiscussion.DiscussionType != "Query")
                        {
                            Thread _job1 = new Thread(() => SendEmailOnlyHOD(ObjEmailData));
                            _job1.Start();
                        }
                    }
                    else
                    {
                        if ((objDiscussion.SubCallType == "30" || objDiscussion.SubCallType == "31" || objDiscussion.SubCallType == "32" || objDiscussion.SubCallType == "33"))
                        {
                            Thread _job1 = new Thread(() => SendEmailForTeams(ObjEmailData));
                            _job1.Start();
                        }
                        else
                        {
                            if (objDiscussion.DiscussionType != "Query")
                            {
                                Thread _job1 = new Thread(() => SendEmail(ObjEmailData));
                                _job1.Start();
                            }
                        }
                    }

                    if ((objDiscussion.SubCallType == "25" || objDiscussion.SubCallType == "26" || objDiscussion.SubCallType == "27") && objDiscussion.DiscussionDoneNotDone != "2")//calltype: Dashboard/subtype: DB & Campaign Discussion/Idea & Campaign Discussion/1st discussion
                    {
                        if (_WAGroupCode != null)
                        {
                            Thread _job2 = new Thread(() => SendWAMessage(ObjMsgData));
                            _job2.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddDiscussions");
            }

            return status;
        }
        public bool UpdateDiscussions(string id, string Desc, string Status, string LoginId, string FollowupDate, string Reassign, string DoneFileName, string FileDone, string RequestType,string DoneNotDone)
        {
            XmlDocument doc = new XmlDocument();
            var xmlpath = ConfigurationManager.AppSettings["DiscussionScripts"].ToString();
            doc.Load(xmlpath);

            BOTS_TblDiscussion objDiscussion = new BOTS_TblDiscussion();
            BOTS_TblSubDiscussionData objsubdiscussion = new BOTS_TblSubDiscussionData();
            bool status = false;
            string path = string.Empty;
            bool UpdateStatus = false;
            string _SendTo = string.Empty;
            string Script = string.Empty;
            string _GroupName = string.Empty;

            try
            {
                string _FilePath = ConfigurationManager.AppSettings["DiscussionFileUpload"];
                string _FileURL = ConfigurationManager.AppSettings["DiscussionDocumentURL"];
                string FileLocation = _FilePath + "/" + DoneFileName;

                using (var context = new CommonDBContext())
                {
                    int mId = Convert.ToInt32(id);
                    var groupId = context.BOTS_TblDiscussion.Where(x => x.Id == mId).Select(y => y.GroupId).FirstOrDefault();
                    var gId = Convert.ToInt32(groupId);
                    var _GroupDetails = context.tblGroupDetails.Where(x => x.GroupId == gId).FirstOrDefault();

                    if (_GroupDetails == null)
                    {
                        var onBoardingGroupDetail = context.BOTS_TblGroupMaster.Where(x => x.GroupId == groupId).FirstOrDefault();
                        _GroupName = onBoardingGroupDetail.GroupName;
                    }
                    else
                    {
                        _GroupName = _GroupDetails.RetailName;
                    }
                }
                if (FileDone != null)//(!string.IsNullOrEmpty(FileDone))
                {                   
                   
                    var GroupFolder = _FilePath + "/" + _GroupName;
                    if (!Directory.Exists(GroupFolder))
                    {
                        Directory.CreateDirectory(GroupFolder);
                    }

                    byte[] imageBytes = Convert.FromBase64String(FileDone);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    path = HttpContext.Current.Server.MapPath("~/DiscussionFileUpload/" + _GroupName + "/" + DoneFileName);
                    FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                    ms.WriteTo(fileNew);
                    fileNew.Close();
                    ms.Close();

                    objsubdiscussion.AttachedFile = _FileURL + _GroupName + "/" + DoneFileName;
                    //objsubdiscussion.AttachedFile = _FileURL + DoneFileName;
                    objsubdiscussion.FileName = DoneFileName;
                }

                using (var context = new CommonDBContext())
                {
                    int discussionId = Convert.ToInt32(id);
                    objDiscussion = context.BOTS_TblDiscussion.Where(x => x.Id == discussionId).FirstOrDefault();
                    objDiscussion.UpdatedDate = DateTime.Now;
                    objDiscussion.Status = Status;
                    objDiscussion.AssignedMember = Reassign;
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
                    objsubdiscussion.RequestType = RequestType;
                    objsubdiscussion.DoneNotDone = DoneNotDone;

                    if (Reassign != "Please Select")
                    {
                        objsubdiscussion.ReassignedMember = Reassign;
                    }
                    context.BOTS_TblSubDiscussionData.AddOrUpdate(objsubdiscussion);
                    context.SaveChanges();
                    status = true;

                    int _subtyprId = Convert.ToInt32(objDiscussion.SubCallType);
                    int _GroupId = Convert.ToInt32(objDiscussion.GroupId);

                    var Sendfrom = context.tblDepartMembers.Where(x => x.LoginId == LoginId).FirstOrDefault();
                    if (Status == "Completed")
                    {
                        Sendfrom = context.tblDepartMembers.Where(x => x.LoginId == objDiscussion.AddedBy).FirstOrDefault();
                    }
                    var SendfromComplete = context.tblDepartMembers.Where(x => x.LoginId == LoginId).FirstOrDefault();
                    var DepartmentHead = context.tblDepartMembers.Where(x => x.Department == Sendfrom.Department && x.Role == "02").FirstOrDefault();
                    var SendTo = context.tblDepartMembers.Where(x => x.Members == Reassign).FirstOrDefault();
                    var Completedid = context.tblDepartMembers.Where(x => x.LoginId == LoginId).FirstOrDefault();
                    var _SubCallType = context.BOTS_TblCallSubTypes.Where(x => x.Id == _subtyprId).FirstOrDefault();
                    var _CallType = context.BOTS_TblCallTypes.Where(x => x.Id == objDiscussion.CallType).FirstOrDefault();
                    var _GroupDetails = context.tblGroupDetails.Where(x => x.GroupId == _GroupId).FirstOrDefault();
                    var _WAGroupCode = context.WAReports.Where(x => x.GroupId == objDiscussion.GroupId && x.SMSStatus == "0").FirstOrDefault();
                    var _Discussion = context.BOTS_TblDiscussion.Where(x => x.Id == discussionId).FirstOrDefault();
                    var DiscussionAddedBy = context.tblDepartMembers.Where(x => x.LoginId == objDiscussion.AddedBy).FirstOrDefault();

                    if (_GroupId == 1051)
                    {
                        _WAGroupCode = context.WAReports.Where(x => x.GroupId == objDiscussion.GroupId && x.SMSStatus == "5").FirstOrDefault();
                    }

                    EmailDetails objmail = new EmailDetails();

                    if (Status == "Completed")
                    {
                        objmail.SendTo = Sendfrom.EmailId;
                    }
                    else
                    {
                        objmail.SendTo = DepartmentHead.EmailId;
                        if (SendTo != null)
                        {
                            objmail.SendTo = SendTo.EmailId;
                        }
                    }

                    objmail.DepartHead = DepartmentHead.EmailId;
                    objmail.DepartHeadName = DepartmentHead.Members;
                    objmail.Addby = Sendfrom.Members;
                    string _AddbyEmail = Sendfrom.EmailId;
                    objmail.Priority = objDiscussion.Priority;
                    objmail.Member = Reassign;
                    objmail.CallTypetext = _CallType.CallType;
                    objmail.subtypetext = _SubCallType.CallSubType;
                    objmail.MemberCompleted = Completedid.Members;
                    objmail.FilePath = path;
                    objmail.GroupName = _GroupName;
                    objmail.id = objDiscussion.Id;
                    objmail.Description = objsubdiscussion.Description;
                    objmail.TeamName = Sendfrom.Department;
                    objmail.DiscussionAddedBy = DiscussionAddedBy.EmailId;
                    if (Status == "Completed")
                    {
                        objmail.FromName = SendfromComplete.Members;
                    }
                    else
                    {
                        objmail.FromName = Sendfrom.Members;
                    }

                    if (objDiscussion.SubCallType == "25" || objDiscussion.SubCallType == "26" || objDiscussion.SubCallType == "27")
                    {
                        if (objDiscussion.SubCallType == "27" && objsubdiscussion.Status == "Completed" && DoneNotDone=="Done")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DiscussionFirstDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "27" && objsubdiscussion.Status == "Completed" && DoneNotDone == "Not Done")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DiscussionFirstNotDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "26" && objsubdiscussion.Status == "Completed" && DoneNotDone == "Done")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/IdeasCampaignDiscussionDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "26" && objsubdiscussion.Status == "Completed" && DoneNotDone == "Not Done")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/IdeasCampaignDiscussionNotDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "25" && objsubdiscussion.Status == "Completed" && DoneNotDone == "Done")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DashboardCampaignDiscussionDone");
                            Script = node.InnerText;
                        }
                        else if (objDiscussion.SubCallType == "25" && objsubdiscussion.Status == "Completed" && DoneNotDone == "Not Done")
                        {
                            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/DashboardCampaignDiscussionNotDone");
                            Script = node.InnerText;
                        }
                    }
                    MessageDetails ObjMsgData = new MessageDetails();
                    if (_WAGroupCode != null)
                    {                        
                        ObjMsgData.Mobileno = _WAGroupCode.GroupCode;
                        ObjMsgData.Description = objsubdiscussion.Description;
                        ObjMsgData.GroupName = _GroupName;
                        ObjMsgData.TeamName = Sendfrom.Department;
                        ObjMsgData.Addby = objDiscussion.AddedBy;
                        ObjMsgData.Message = Script;
                        ObjMsgData.SpokenTo = _Discussion.SpokenTo;
                        ObjMsgData.BOEmpName = Sendfrom.Members;
                    }

                    if (objsubdiscussion.Status == "Completed")
                    {
                        UpdateStatus = true;
                        if (SendTo == null)
                        {
                            objmail.SendTo = _AddbyEmail;
                        }
                    }
                    else
                    {
                        if (SendTo != null)
                        {
                            objmail.SendTo = SendTo.EmailId;
                        }
                    }

                    if (UpdateStatus)
                    {
                        if (objDiscussion.SubCallType == "25" || objDiscussion.SubCallType == "26" || objDiscussion.SubCallType == "27")
                        {
                            if (objDiscussion.DiscussionType != "Query")
                            {
                                Thread _job1 = new Thread(() => SendEmailCompleteOnlyHOD(objmail));
                                _job1.Start();
                            }
                            if (_WAGroupCode != null)
                            {
                                Thread _job2 = new Thread(() => SendWAMessageCompleteHOD(ObjMsgData));
                                _job2.Start();
                            }
                        }
                        else
                        { 
                            if (objDiscussion.DiscussionType != "Query")
                            {
                                Thread _job1 = new Thread(() => SendEmailComplete(objmail));
                                _job1.Start();
                            }
                        }
                    }
                    else
                    {
                        if (objDiscussion.SubCallType == "25" || objDiscussion.SubCallType == "26" || objDiscussion.SubCallType == "27")
                        {
                            if (objDiscussion.DiscussionType != "Query")
                            {
                                Thread _job1 = new Thread(() => SendEmailUpdateOnlyHOD(objmail));
                                _job1.Start();
                            }
                            if (_WAGroupCode != null)
                            {
                                if (!string.IsNullOrEmpty(ObjMsgData.Message))
                                {
                                    Thread _job2 = new Thread(() => SendWAMessageUpdateHOD(ObjMsgData));
                                    _job2.Start();
                                }
                            }
                        }
                        else
                        {
                            if (Reassign != "Please Select")
                            {
                                if (objDiscussion.DiscussionType != "Query")
                                {
                                    Thread _job1 = new Thread(() => SendEmailUpdate(objmail));
                                    _job1.Start();
                                }
                            }
                        }
                    }
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
                                                AddedDate = ct.AddedDate,
                                                AssignedTo = ct.ReassignedMember,
                                                AttachedFile = ct.AttachedFile,
                                                FileName = ct.FileName

                                            }).OrderByDescending(x => x.FollowupDate).ToList();

                    foreach (var item in lstsubdiscussionlist)
                    {
                        if (item.AddedDate.HasValue)
                            item.UpdatedDate = Convert.ToString(item.AddedDate);
                        //item.UpdatedDate = DateTime.ParseExact(item.AddedDate, "dd/M/yyyy hh:mm:ss",CultureInfo.InvariantCulture);
                        else
                            item.UpdatedDate = "--";
                    }
                }

                lstsubdiscussionlist = lstsubdiscussionlist.OrderByDescending(x => x.SubDiscussionId).ToList();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNestedDiscussionList");
            }

            return lstsubdiscussionlist;
        }
        public List<SelectListItem> GetSubCallTypes(int Id)
        {
            List<SelectListItem> lstSubCallTypes = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var SubCallTypes = context.BOTS_TblCallSubTypes.Where(x => x.CallTypeId == Id).ToList();
                    SubCallTypes = SubCallTypes.Where(x => x.Id != 69).ToList();

                    foreach (var item in SubCallTypes)
                    {
                        lstSubCallTypes.Add(new SelectListItem
                        {
                            Text = item.CallSubType,
                            Value = Convert.ToString(item.Id)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSubCallTypes");
            }
            
            lstSubCallTypes = lstSubCallTypes.OrderBy(x => x.Text).ToList();

            return lstSubCallTypes;
        }
        public List<SelectListItem> GetCallTypes(string LoginType)
        {
            List<SelectListItem> lstCallTypes = new List<SelectListItem>();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCallTypes");
            }
            lstCallTypes = lstCallTypes.OrderBy(x => x.Text).ToList();
            return lstCallTypes;
        }
        public List<SelectListItem> GetGroupDetails()
        {
            List<SelectListItem> lstgroupdetails = new List<SelectListItem>();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupDetails");
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
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRaisedby");
            }
            return lstRMAssigned;
        }

        public List<DiscussionDetails> GetfilteredDiscussionData(string status, int calltype, string groupnm, string fromDate, string toDate, string raisedby, string LoginType, string LoginId, bool IsFollowUp)
        {
            List<DiscussionDetails> lstdiscuss = new List<DiscussionDetails>();
            List<DiscussionDetails> lstdiscussOnBoarding = new List<DiscussionDetails>();
            List<BOTS_TblDiscussion> lsttbldiscuss = new List<BOTS_TblDiscussion>();
            try
            {
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
                                              join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                  AssignedMember = c.AssignedMember,
                                                  SubCallType = sct.CallSubType,

                                              }).OrderByDescending(x => x.AddedDate).ToList();


                                lstdiscussOnBoarding = (from c in list
                                                        join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                        join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                        join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                            AssignedMember = c.AssignedMember,
                                                            SubCallType = sct.CallSubType,

                                                        }).OrderByDescending(x => x.AddedDate).ToList();
                            }
                            else
                            {
                                lstdiscuss = (from c in list
                                              join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                              join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                              join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                  AssignedMember = c.AssignedMember,
                                                  SubCallType = sct.CallSubType,


                                              }).OrderByDescending(x => x.AddedDate).ToList();

                                lstdiscussOnBoarding = (from c in list
                                                        join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                        join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                        join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                            AssignedMember = c.AssignedMember,
                                                            SubCallType = sct.CallSubType,

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
                                                  join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                      AssignedMember = c.AssignedMember,
                                                      SubCallType = sct.CallSubType,

                                                  }).OrderByDescending(x => x.AddedDate).ToList();

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                                AssignedMember = c.AssignedMember,
                                                                SubCallType = sct.CallSubType,

                                                            }).OrderByDescending(x => x.AddedDate).ToList();
                                }
                                else
                                {
                                    lstdiscuss = (from c in list
                                                  join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                  join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                  join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                      AssignedMember = c.AssignedMember,
                                                      SubCallType = sct.CallSubType,

                                                  }).OrderByDescending(x => x.AddedDate).ToList();

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                                AssignedMember = c.AssignedMember,
                                                                SubCallType = sct.CallSubType,

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
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = sct.CallSubType,

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = sct.CallSubType,

                                                                }).OrderByDescending(x => x.AddedDate).ToList();
                                    }
                                    else
                                    {
                                        lstdiscuss = (from c in list
                                                      join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                      join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = sct.CallSubType,

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = sct.CallSubType,

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
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = sct.CallSubType,

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = sct.CallSubType,

                                                                }).OrderByDescending(x => x.AddedDate).ToList();
                                    }
                                    else
                                    {
                                        lstdiscuss = (from c in list
                                                      join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                      join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = sct.CallSubType,

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = sct.CallSubType,

                                                                }).OrderByDescending(x => x.AddedDate).ToList();
                                    }
                                }

                            }
                        }
                    }



                }
                lstdiscuss.AddRange(lstdiscussOnBoarding);
                //using (var context = new CommonDBContext())
                //{
                //    foreach (var item in lstdiscuss)
                //    {
                //        item.SubDiscussionCount = context.BOTS_TblSubDiscussionData.Where(x => x.DiscussionId == item.Id).Count();
                //    }
                //}
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRaisedby");
            }

            return lstdiscuss;
        }
        public List<DiscussionDetails> GetfilteredDiscussionDataAssign(string status, int calltype, int SubCallType, string groupnm, string fromDate, string toDate, string raisedby, string LoginType, string LoginId, bool IsFollowUp, string AssignMember, string DiscussionType)
        {
            List<DiscussionDetails> lstdiscuss = new List<DiscussionDetails>();
            List<DiscussionDetails> lstdiscussOnBoarding = new List<DiscussionDetails>();
            List<BOTS_TblDiscussion> lsttbldiscuss = new List<BOTS_TblDiscussion>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var AssignedName = "";
                    var list = context.BOTS_TblDiscussion.ToList();
                    if (AssignMember != "")
                    {
                        AssignedName = context.tblDepartMembers.Where(x => x.LoginId == AssignMember).Select(y => y.Members).FirstOrDefault();
                        if (LoginType == "9" || LoginType == "10")
                        {
                            list = list.Where(x => x.AssignedMember == AssignedName || x.AddedBy == LoginId || x.CallType == 3 || x.CallType == 9 || x.CallType == 10 || x.CallType == 12 || x.CallType == 18).ToList();//3,9,10,12,18
                        }
                        else if (LoginType == "1" || LoginType == "7")
                            list = list.Where(x => x.AssignedMember == AssignedName).ToList();
                        else
                            list = list.Where(x => x.AssignedMember == AssignedName || x.AddedBy == LoginId).ToList();
                    }


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
                        if (SubCallType != 0)
                        {
                            var subCallTypeStr = Convert.ToString(SubCallType);
                            list = list.Where(x => x.SubCallType == subCallTypeStr).ToList();
                        }
                        if (!string.IsNullOrEmpty(DiscussionType))
                        {
                            list = list.Where(x => x.DiscussionType == DiscussionType).ToList();
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
                                              join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                              into ps
                                              from p in ps.DefaultIfEmpty()
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
                                                  AssignedMember = c.AssignedMember,
                                                  SubCallType = p.CallSubType,
                                                  DiscussionType = c.DiscussionType
                                              }).OrderByDescending(x => x.AddedDate).ToList();


                                lstdiscussOnBoarding = (from c in list
                                                        join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                        join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                        join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                        from p in ps.DefaultIfEmpty()
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
                                                            AssignedMember = c.AssignedMember,
                                                            SubCallType = p.CallSubType,
                                                            DiscussionType = c.DiscussionType
                                                        }).OrderByDescending(x => x.AddedDate).ToList();
                            }
                            else
                            {
                                lstdiscuss = (from c in list
                                              join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                              join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                              join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                              from p in ps.DefaultIfEmpty()
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
                                                  AssignedMember = c.AssignedMember,
                                                  SubCallType = p.CallSubType,
                                                  DiscussionType = c.DiscussionType
                                              }).OrderByDescending(x => x.AddedDate).ToList();

                                lstdiscussOnBoarding = (from c in list
                                                        join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                        join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                        join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                        from p in ps.DefaultIfEmpty()
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
                                                            AssignedMember = c.AssignedMember,
                                                            SubCallType = p.CallSubType,
                                                            DiscussionType = c.DiscussionType
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
                                                  join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                  from p in ps.DefaultIfEmpty()
                                                  join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId //into cldn from cl in cldn.DefaultIfEmpty()
                                                  where c.Status == "WIP"// && (c.CallType == 3 || c.CallType == 9 || c.CallType == 10 || c.CallType == 12 || c.CallType == 18 || c.AssignedMember == AssignedName)

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
                                                      AssignedMember = c.AssignedMember,
                                                      SubCallType = p.CallSubType,
                                                      DiscussionType = c.DiscussionType

                                                  }).OrderByDescending(x => x.AddedDate).ToList();

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                            from p in ps.DefaultIfEmpty()
                                                            join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                            where c.Status == "WIP"// && (c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18 || c.AssignedMember == AssignedName)
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
                                                                AssignedMember = c.AssignedMember,
                                                                SubCallType = p.CallSubType,
                                                                DiscussionType = c.DiscussionType

                                                            }).OrderByDescending(x => x.AddedDate).ToList();
                                }
                                else
                                {
                                    lstdiscuss = (from c in list
                                                  join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                  join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                  join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                  from p in ps.DefaultIfEmpty()
                                                  join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                  //where c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18 || c.AssignedMember == AssignedName

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
                                                      AssignedMember = c.AssignedMember,
                                                      SubCallType = p.CallSubType,
                                                      DiscussionType = c.DiscussionType

                                                  }).OrderByDescending(x => x.AddedDate).ToList();

                                    lstdiscussOnBoarding = (from c in list
                                                            join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                            join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                            join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                            from p in ps.DefaultIfEmpty()
                                                            join cld in context.CustomerLoginDetails on c.AddedBy equals cld.LoginId
                                                            //where c.CallType == 12 || c.CallType == 9 || c.CallType == 10 || c.CallType == 18 || c.AssignedMember == AssignedName

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
                                                                AssignedMember = c.AssignedMember,
                                                                SubCallType = p.CallSubType,
                                                                DiscussionType = c.DiscussionType

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
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                         into ps
                                                      from p in ps.DefaultIfEmpty()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = p.CallSubType,
                                                          DiscussionType = c.DiscussionType

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                         into ps
                                                                from p in ps.DefaultIfEmpty()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = p.CallSubType,
                                                                    DiscussionType = c.DiscussionType

                                                                }).OrderByDescending(x => x.AddedDate).ToList();
                                    }
                                    else
                                    {
                                        lstdiscuss = (from c in list
                                                      join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                      join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                      from p in ps.DefaultIfEmpty()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = p.CallSubType,
                                                          DiscussionType = c.DiscussionType

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                                from p in ps.DefaultIfEmpty()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = p.CallSubType,
                                                                    DiscussionType = c.DiscussionType

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
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                         into ps
                                                      from p in ps.DefaultIfEmpty()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = p.CallSubType,
                                                          DiscussionType = c.DiscussionType

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                                from p in ps.DefaultIfEmpty()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = p.CallSubType,
                                                                    DiscussionType = c.DiscussionType

                                                                }).OrderByDescending(x => x.AddedDate).ToList();
                                    }
                                    else
                                    {
                                        lstdiscuss = (from c in list
                                                      join gd in context.tblGroupDetails on c.GroupId equals gd.GroupId.ToString()
                                                      join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                      join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                      from p in ps.DefaultIfEmpty()
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
                                                          AssignedMember = c.AssignedMember,
                                                          SubCallType = p.CallSubType,
                                                          DiscussionType = c.DiscussionType

                                                      }).OrderByDescending(x => x.AddedDate).ToList();

                                        lstdiscussOnBoarding = (from c in list
                                                                join gd in context.BOTS_TblGroupMaster on c.GroupId equals gd.GroupId.ToString()
                                                                join ct in context.BOTS_TblCallTypes on c.CallType equals ct.Id
                                                                join sct in context.BOTS_TblCallSubTypes on c.SubCallType equals sct.Id.ToString()
                                                        into ps
                                                                from p in ps.DefaultIfEmpty()
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
                                                                    AssignedMember = c.AssignedMember,
                                                                    SubCallType = p.CallSubType,
                                                                    DiscussionType = c.DiscussionType

                                                                }).OrderByDescending(x => x.AddedDate).ToList();
                                    }
                                }

                            }
                        }
                    }
                }
                lstdiscuss.AddRange(lstdiscussOnBoarding);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetfilteredDiscussionDataAssign");
            }
            //using (var context = new CommonDBContext())
            //{
            //    foreach (var item in lstdiscuss)
            //    {
            //        item.SubDiscussionCount = context.BOTS_TblSubDiscussionData.Where(x => x.DiscussionId == item.Id).Count();
            //    }
            //}
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
                    objtbldepart = context.tblDepartMembers.Where(x => x.Department == Department && x.status == "00").ToList();
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberdetails");
            }
            return objtbldepart;
        }
        public List<SelectListItem> GetReAssignMemberdetails(string id)
        {
            List<SelectListItem> objtbldepart = new List<SelectListItem>();
            //List<BOTS_TblDiscussion> objdata = new List<BOTS_TblDiscussion>();
            int varid = Convert.ToInt32(id);
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objdata = context.BOTS_TblDiscussion.Where(x => x.Id == varid).FirstOrDefault();
                    //string AssignedDepartment = objdata.Department;
                    var objtbldepart1 = context.tblDepartMembers.Where(x => x.status == "00").ToList();
                    foreach (var item in objtbldepart1)
                    {
                        objtbldepart.Add(new SelectListItem
                        {
                            Text = item.Members,
                            Value = Convert.ToString(item.Members),
                            Selected = objdata.AssignedMember == item.Members ? true : false

                        });
                    }

                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetReAssignMemberdetails");
            }
            return objtbldepart;
        }
        public List<SelectListItem> GetAssignedMemberList()
        {
            List<SelectListItem> lstMemberAssigned = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {

                    //var raised = context.tblRMAssigneds.Where(x => x.IsActive == true).ToList();
                    var raised = context.tblDepartMembers.Where(x => x.status == "00").ToList();
                    lstMemberAssigned.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
                    foreach (var item in raised)
                    {
                        lstMemberAssigned.Add(new SelectListItem
                        {
                            Text = item.Members,
                            Value = Convert.ToString(item.LoginId)
                        });
                    }
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAssignedMemberList");
            }
            return lstMemberAssigned;
        }
        public List<DiscussionMemberWiseCount> GetTaskCount(string Department)
        {
            List<DiscussionMemberWiseCount> lstCount = new List<DiscussionMemberWiseCount>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //taskCount = context.BOTS_TblDiscussion.Where(x => x.AssignedMember == MemberName && x.Status == "WIP").Count();
                    var lstMembers = context.tblDepartMembers.Where(x => x.Department == Department).ToList();
                    foreach (var item in lstMembers)
                    {
                        DiscussionMemberWiseCount newItem = new DiscussionMemberWiseCount();
                        newItem.Name = item.Members;
                        newItem.TotalCount = context.BOTS_TblDiscussion.Where(x => x.AssignedMember == item.Members && x.Status == "WIP").Count();
                        lstCount.Add(newItem);
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTaskCount");
            }
            return lstCount;
        }

        public BOTS_TblDiscussion GetDiscussionById(int Id)
        {
            BOTS_TblDiscussion objData = new BOTS_TblDiscussion();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblDiscussion.Where(x => x.Id == Id).FirstOrDefault();
            }

            return objData;
        }

        public void SendEmail(EmailDetails Emaildata)
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailAppPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            var PortNo = 587;

            try
            {
                using (MailMessage mail = new MailMessage())//tech@blueocktopus.in operations@blueocktopus.in
                {

                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear " + Emaildata.Member + ",</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    if (Emaildata.Priority == " ")
                    {
                        str.AppendLine("<td> You have a task assigned with <b>" + Emaildata.Priority + "priority</b></td>");
                    }
                    else
                    {
                        str.AppendLine("<td>You have a task assigned</td>");
                    }
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Discription : " + Emaildata.Description + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td> - " + Emaildata.FromName + "</td>");
                    str.Append("</tr>");
                    str.Append("</table>");
                    mail.From = new MailAddress(from);
                    mail.To.Add(Emaildata.SendTo);
                    mail.Subject = "WIP Raised[#" + Emaildata.id + "]: " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    
                    mail.CC.Add(Emaildata.AddbyEmail);
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    if (!string.IsNullOrEmpty(Emaildata.FilePath))
                    {
                        Attachment data = new Attachment(Emaildata.FilePath, MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
        public void SendEmailOnlyHOD(EmailDetails Emaildata)
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailAppPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            var PortNo = 587;
            try
            {
                using (MailMessage mail = new MailMessage())//tech@blueocktopus.in operations@blueocktopus.in
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear " + Emaildata.DepartHeadName + ",</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>I have raised a ticket for - : " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext + "</td>");
                    str.AppendLine("</br>");
                    str.AppendLine("<td><b>Discription : </b>" + Emaildata.Description + "</td>");
                    str.AppendLine("</br>");
                    str.Append("<tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td> - " + Emaildata.FromName + "</td>");
                    str.Append("</tr>");
                    str.Append("</table>");

                    mail.From = new MailAddress(from);
                    mail.To.Add(Emaildata.SendTo);
                    mail.Subject = "WIP Raised[#" + Emaildata.id + "]: " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    if (!string.IsNullOrEmpty(Emaildata.FilePath))
                    {
                        Attachment data = new Attachment(Emaildata.FilePath, MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
        public void SendEmailUpdate(EmailDetails Emaildata)
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailAppPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            var PortNo = 587;

            try
            {
                using (MailMessage mail = new MailMessage())//tech@blueocktopus.in operations@blueocktopus.in
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear " + Emaildata.Member + ",</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>You have a task assigned from - " + Emaildata.Addby + " with <b> priority " + Emaildata.Priority + "</b></td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td><b>Discription : </b>" + Emaildata.Description + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td> - " + Emaildata.FromName + "</td>");
                    str.Append("</tr>");
                    str.Append("</table>");

                    mail.From = new MailAddress(from);
                    mail.To.Add(Emaildata.SendTo);
                    mail.Subject = "WIP Reassigned[#" + Emaildata.id + "]: " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    
                    mail.CC.Add(Emaildata.DiscussionAddedBy);
                    
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    if (!string.IsNullOrEmpty(Emaildata.FilePath))
                    {
                        Attachment data = new Attachment(Emaildata.FilePath, MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
        public void SendEmailComplete(EmailDetails Emaildata)
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailAppPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            var PortNo = 587;
            try
            {
                using (MailMessage mail = new MailMessage())//tech@blueocktopus.in operations@blueocktopus.in
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear " + Emaildata.Addby + ",</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Your assigned task is Completed by " + Emaildata.MemberCompleted + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td><b>Discription : </b>" + Emaildata.Description + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td> - " + Emaildata.FromName + "</td>");
                    str.Append("</tr>");
                    str.Append("</table>");

                    mail.From = new MailAddress(from);
                    mail.To.Add(Emaildata.SendTo);
                    mail.Subject = "Completed [#" + Emaildata.id + "]: " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    //if (Emaildata.DepartHead != null)
                    //{
                    //    mail.CC.Add(Emaildata.DepartHead);
                    //}
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    if (!string.IsNullOrEmpty(Emaildata.FilePath))
                    {
                        Attachment data = new Attachment(Emaildata.FilePath, MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
        public void SendWAMessage(MessageDetails objMsg)
        {
            string responseString;

            try
            {
                objMsg.Message = objMsg.Message.Replace("#01", objMsg.SpokenTo);
                objMsg.Message = HttpUtility.UrlEncode(objMsg.Message);
                //string type = "TEXT";
                StringBuilder stb = new StringBuilder();
                stb.AppendLine(objMsg.Message);
                stb.AppendLine();
                stb.AppendLine("Regards,");
                stb.AppendLine(" - " + objMsg.BOEmpName);
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", "5fc8ed623629423c01ce4221");
                sbposdata.AppendFormat("&phone={0}", objMsg.Mobileno);
                sbposdata.AppendFormat("&message={0}", stb.ToString());

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
        public void SendEmailUpdateOnlyHOD(EmailDetails Emaildata)
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailAppPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            var PortNo = 587;
            try
            {
                using (MailMessage mail = new MailMessage())//tech@blueocktopus.in operations@blueocktopus.in
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear " + Emaildata.DepartHeadName + ",</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>I have rescheduled task of - : " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td><b>Discription : </b><td>" + Emaildata.Description);
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td> - " + Emaildata.FromName + "</td>");
                    str.Append("</tr>");
                    str.Append("</table>");

                    mail.From = new MailAddress(from);
                    mail.To.Add(Emaildata.SendTo);
                    mail.Subject = "WIP Rescheduled[#" + Emaildata.id + "]: " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    //mail.CC.Add(Emaildata.DepartHead);
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    if (!string.IsNullOrEmpty(Emaildata.FilePath))
                    {
                        Attachment data = new Attachment(Emaildata.FilePath, MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
        public void SendWAMessageUpdateHOD(MessageDetails objMsg)
        {
            string responseString;

            try
            {
                objMsg.Message = objMsg.Message.Replace("#01", objMsg.SpokenTo);
                objMsg.Message = HttpUtility.UrlEncode(objMsg.Message);

                StringBuilder stb = new StringBuilder();
                stb.AppendLine(objMsg.Message);
                stb.AppendLine();
                stb.AppendLine("Regards,");
                stb.AppendLine(" - " + objMsg.BOEmpName);
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", "5fc8ed623629423c01ce4221");
                sbposdata.AppendFormat("&phone={0}", objMsg.Mobileno);
                sbposdata.AppendFormat("&message={0}", stb.ToString());

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
        public void SendWAMessageCompleteHOD(MessageDetails objMsg)
        {
            string responseString;

            try
            {
                objMsg.Message = objMsg.Message.Replace("#01", objMsg.SpokenTo);
                objMsg.Message = HttpUtility.UrlEncode(objMsg.Message);

                StringBuilder stb = new StringBuilder();
                stb.AppendLine(objMsg.Message);
                stb.AppendLine();
                stb.AppendLine("Regards,");
                stb.AppendLine(" - " + objMsg.BOEmpName);
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", "5fc8ed623629423c01ce4221");
                sbposdata.AppendFormat("&phone={0}", objMsg.Mobileno);
                sbposdata.AppendFormat("&message={0}", stb.ToString());

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
        public void SendEmailCompleteOnlyHOD(EmailDetails Emaildata)
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailAppPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            var PortNo = 587;
            try
            {
                using (MailMessage mail = new MailMessage())//tech@blueocktopus.in operations@blueocktopus.in
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear " + Emaildata.DepartHeadName + ",</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Completed the task of - : " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td><b>Discription : </b>" + Emaildata.Description + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td> - " + Emaildata.FromName + "</td>");
                    str.Append("</tr>");
                    str.Append("</table>");

                    mail.From = new MailAddress(from);
                    mail.To.Add(Emaildata.SendTo);
                    mail.Subject = "Completed [#" + Emaildata.id + "]: " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    //mail.CC.Add(Emaildata.DepartHead);
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    if (!string.IsNullOrEmpty(Emaildata.FilePath))
                    {
                        Attachment data = new Attachment(Emaildata.FilePath, MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
        public void SendEmailForTeams(EmailDetails Emaildata) // AddbyEmail
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailAppPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            var PortNo = 587;
            var OpsTeam = "operations@blueocktopus.in";

            try
            {
                using (MailMessage mail = new MailMessage())//tech@blueocktopus.in operations@blueocktopus.in
                {

                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Dear " + Emaildata.Member + ",</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    if (Emaildata.Priority == " ")
                    {
                        str.AppendLine("<td> You have a task assigned with <b>" + Emaildata.Priority + "priority</b></td>");
                    }
                    else
                    {
                        str.AppendLine("<td>You have a task assigned</td>");
                    }
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Discription : " + Emaildata.Description + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td> - " + Emaildata.FromName + "</td>");
                    str.Append("</tr>");
                    str.Append("</table>");
                    mail.From = new MailAddress(from);
                    mail.To.Add(Emaildata.SendTo);
                    mail.Subject = "WIP Raised[#" + Emaildata.id + "]: " + Emaildata.GroupName + " : " + Emaildata.CallTypetext + " : " + Emaildata.subtypetext;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    if (Emaildata.DepartHead != null)
                    {
                        mail.CC.Add(Emaildata.DepartHead);
                    }
                    mail.CC.Add(OpsTeam);
                    mail.CC.Add(Emaildata.AddbyEmail);
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    if (!string.IsNullOrEmpty(Emaildata.FilePath))
                    {
                        Attachment data = new Attachment(Emaildata.FilePath, MediaTypeNames.Application.Octet);
                        mail.Attachments.Add(data);
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
        public string GetDiscussionCustMobile(string CustName, string groupId)
        {
            var MobileNo = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    MobileNo = context.tblDiscussionCustomerDatas.Where(x => x.CustomerName == CustName && x.GroupId == groupId).Select(y => y.MobileNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDiscussionCustMobile");
            }
            return MobileNo;
        }
        public List<string> GetAllDiscussionCustNames(string groupId)
        {
            List<string> CustList = new List<string>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    CustList = context.tblDiscussionCustomerDatas.Where(x => x.GroupId == groupId).Select(y => y.CustomerName).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllDiscussionCustNames");
            }
            return CustList;
        }
    }
}
