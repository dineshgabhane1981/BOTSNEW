using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models.IndividualDBModels;
using BOTS_BL.Models.RetailerWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net.Mime;
using DocumentFormat.OpenXml.Math;

namespace BOTS_BL.Repository
{
    public class ITCSRepository
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
                        lstData = context.tblGroupDetails.Where(x => x.IsActive == true).OrderBy(x => x.GroupName).ToList();
                    else
                        lstData = context.tblGroupDetails.Where(x => x.IsActive == false).OrderBy(x => x.GroupName).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return lstData;
        }
        public bool DisableProgrammeDetails(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    DailyActivityAllGroup obj1 = new DailyActivityAllGroup();
                    tblDatabaseDetail objtblDatabaseDetail = new tblDatabaseDetail();
                    WAReport objWAReport = new WAReport();
                    GroupDetails objGroupDetail = new GroupDetails();
                    DatabaseDetail objDatabaseDetail = new DatabaseDetail();
                    tblDeactivatedGroupList objdeactivatedGroupList = new tblDeactivatedGroupList();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var WAReport = context.WAReports.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (WAReport != null)
                        {
                            WAReport.SMSStatus = "3";
                        }
                        context.SaveChanges();
                        status = true;

                        var GroupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        if (GroupDetail != null)
                        {
                            GroupDetail.IsActive = false;
                            GroupDetail.IsLive = false;
                        }
                        context.SaveChanges();
                        status = true;

                        var DatabaseDetails = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).ToList();
                        foreach (var Details in DatabaseDetails)
                        {
                            Details.IsActive = false;
                        }
                        context.SaveChanges();
                        status = true;

                        var DailyActivityGroup = context.DailyActivityAllGroups.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (DailyActivityGroup != null)
                        {
                            DailyActivityGroup.IsActive = false;
                        }
                        context.SaveChanges();
                        status = true;

                        var groupdetails = context.GroupDetail.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (groupdetails != null)
                        {
                            groupdetails.ActiveStatus = "No";
                        }
                        context.SaveChanges();
                        status = true;

                        var DatabaseDetailsNew = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (DatabaseDetailsNew != null)
                        {
                            context.DatabaseDetails.Remove(DatabaseDetailsNew);
                        }
                        context.SaveChanges();
                        status = true;

                        var GroupName = context.tblGroupDetails.Where(x => x.GroupId == varid).Select(x => x.GroupName).FirstOrDefault();
                        var DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(x => x.DBName).FirstOrDefault();
                        var deactivatedGroup = new tblDeactivatedGroupList
                        {
                            GroupId = GroupId,
                            GroupName = GroupName,
                            DBName = DBName,
                            DeactivationDate = DateTime.Now
                        };
                        context.tblDeactivatedGroupLists.Add(deactivatedGroup);
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableProgrammeDetails");
            }
            return status;
        }
        public bool EnableProgrammeDetails(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    DailyActivityAllGroup obj1 = new DailyActivityAllGroup();
                    tblDatabaseDetail objtblDatabaseDetail = new tblDatabaseDetail();
                    WAReport objWAReport = new WAReport();
                    GroupDetails objGroupDetail = new GroupDetails();
                    DatabaseDetail objDatabaseDetail = new DatabaseDetail();
                    tblDeactivatedGroupList objdeactivatedGroupList = new tblDeactivatedGroupList();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var WAReport = context.WAReports.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (WAReport != null)
                        {
                            WAReport.SMSStatus = "0";
                        }
                        context.SaveChanges();
                        status = true;

                        var GroupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        if (GroupDetail != null)
                        {
                            GroupDetail.IsActive = true;
                            GroupDetail.IsLive = true;
                        }
                        context.SaveChanges();
                        status = true;

                        var DatabaseDetail = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (DatabaseDetail != null)
                        {
                            DatabaseDetail.IsActive = true;
                        }
                        context.SaveChanges();
                        status = true;

                        var DailyActivityGroup = context.DailyActivityAllGroups.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (DailyActivityGroup != null)
                        {
                            DailyActivityGroup.IsActive = true;
                        }
                        context.SaveChanges();
                        status = true;

                        var groupdetails = context.GroupDetail.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (groupdetails != null)
                        {
                            groupdetails.ActiveStatus = "Live";
                        }
                        context.SaveChanges();
                        status = true;

                        var CounterId = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(x => x.CounterId).FirstOrDefault();
                        var CounterIdNew = CounterId.Substring(0, 5);
                        var DBNameNew = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(x => x.DBName).FirstOrDefault();
                        var existingDatabaseDetails = context.DatabaseDetails.FirstOrDefault(x => x.GroupId == GroupId);
                        if (existingDatabaseDetails == null)
                        {
                            var newDatabaseDetails = new DatabaseDetail
                            {
                                CounterId = CounterIdNew,
                                SecurityKey = "MWP_" + CounterIdNew,
                                IPAddress = "13.233.58.231",
                                DBName = DBNameNew,
                                DBPassword = "F59VM$KDE@KF!AW",
                                DBId = "Renaldo",
                                EncryptionStatus = "0",
                                GroupId = GroupId
                            };
                            context.DatabaseDetails.Add(newDatabaseDetails);
                        }
                        context.SaveChanges();
                        status = true;

                        var deactivatedGroup = context.tblDeactivatedGroupLists.FirstOrDefault(x => x.GroupId == GroupId);
                        if (deactivatedGroup != null)
                        {
                            context.tblDeactivatedGroupLists.Remove(deactivatedGroup);
                            context.SaveChanges();
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableProgrammeDetails");
            }
            return status;
        }
        public List<SelectListItem> GetGroupDetails()
        {
            List<SelectListItem> lstGroupDetails = new List<SelectListItem>();
            List<tblGroupDetail> GroupDetails = new List<tblGroupDetail>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    GroupDetails = context.tblGroupDetails.Where(x => x.IsActive == true).ToList();
                }
                foreach (var item in GroupDetails)
                {
                    lstGroupDetails.Add(new SelectListItem
                    {
                        Text = item.GroupName,
                        Value = Convert.ToString(item.GroupId)
                    });
                }
                lstGroupDetails = lstGroupDetails.OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupDetails");
            }
            return lstGroupDetails;
        }
        public List<SelectListItem> GetScript(string GroupId, string OutletId)
        {
            List<SelectListItem> lstScript = new List<SelectListItem>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    if (OutletId == "1")
                    {
                        var MessageType = contextNew.tblSMSWhatsAppScriptMasters.Take(8).ToList();
                        foreach (var item in MessageType)
                        {
                            lstScript.Add(new SelectListItem
                            {
                                Text = item.MessageType,
                                Value = Convert.ToString(item.Id)
                            });
                        }
                    }
                    else
                        {
                            var MessageType = contextNew.tblSMSWhatsAppScriptMasters.Where(x => x.OutletId == OutletId).ToList();
                            foreach (var item in MessageType)
                            {
                                lstScript.Add(new SelectListItem
                                {
                                    Text = item.MessageType,
                                    Value = Convert.ToString(item.Id)
                                });
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetScript");
            }
            return lstScript;
        }
        public tblSMSWhatsAppScriptMaster GetTransactionalScripts(int GroupId, string OutletId, string MessageType)
        {
            tblSMSWhatsAppScriptMaster objSMSWhatsAppScriptMaster = new tblSMSWhatsAppScriptMaster();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objSMSWhatsAppScriptMaster = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == OutletId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionalScripts");
            }
            return objSMSWhatsAppScriptMaster;
        }
        public bool SaveWATransactionalScripts(int GroupId, string OutletId, string Script, string MessageType, string ScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblSMSWhatsAppScriptMaster objSMSWhatsAppScriptMaster = new tblSMSWhatsAppScriptMaster();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    if (OutletId == "1") 
                    {
                        var scripts = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType).ToList();
                        foreach (var script in scripts)
                        {
                            script.WhatsAppScript = Script;
                            script.WhatsAppScriptType = ScriptType;
                            script.IsActive = true;
                            //script.WhatsAppMessageType = "Text";

                            var existingScript = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == script.OutletId).Select(x => x.SMSScript).FirstOrDefault();
                            if (existingScript != null)
                            {
                                script.SMSWhatsAppSendStatus = "Both";
                            }
                            else
                            {
                                script.SMSWhatsAppSendStatus = "WA";
                            }
                            context.tblSMSWhatsAppScriptMasters.AddOrUpdate(script);
                            context.SaveChanges();
                        }
                    }
                    else
                    { 
                        var Script1 = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == OutletId).FirstOrDefault();
                        Script1.WhatsAppScript = Script;
                        Script1.WhatsAppScriptType = ScriptType;
                        Script1.IsActive = true;
                        //Script1.WhatsAppMessageType = "Text";
                     
                        var existingScript = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == OutletId).Select(x => x.SMSScript).FirstOrDefault();
                        if (existingScript != null)
                        {
                            Script1.SMSWhatsAppSendStatus = "Both";
                        }
                        else
                        {
                            Script1.SMSWhatsAppSendStatus = "WA";
                        }
                        if (Script1.WhatsAppMessageType == null)
                        {
                            Script1.WhatsAppMessageType = "Text";
                        }
                        context.tblSMSWhatsAppScriptMasters.AddOrUpdate(Script1);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWATransactionalScripts");
            }
            return result;
        }
        public bool SaveSMSTransactionalScripts(int GroupId, string OutletId, string Script, string MessageType, string ScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblSMSWhatsAppScriptMaster objSMSWhatsAppScriptMaster = new tblSMSWhatsAppScriptMaster();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    if (OutletId == "1")
                    {
                        var scripts = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType).ToList();
                        foreach (var script in scripts)
                        {
                            script.SMSScript = Script;
                            script.SMSScriptType = ScriptType;
                            script.IsActive = true;
                            //script.WhatsAppMessageType = "Text";

                            var existingScript = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == script.OutletId).Select(x => x.WhatsAppScript).FirstOrDefault();
                            if (existingScript != null)
                            {
                                script.SMSWhatsAppSendStatus = "Both";
                            }
                            else
                            {
                                script.SMSWhatsAppSendStatus = "WA";
                            }

                            context.tblSMSWhatsAppScriptMasters.AddOrUpdate(script);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        var Script1 = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == OutletId).FirstOrDefault();
                        Script1.SMSScript = Script;
                        Script1.SMSScriptType = ScriptType;
                        Script1.IsActive = true;
                        //Script1.WhatsAppMessageType = "Text";
                        //context.tblSMSWhatsAppScriptMasters.AddOrUpdate(Script1);
                       // context.SaveChanges();

                        var existingScript = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == OutletId).Select(x => x.WhatsAppScript).FirstOrDefault();
                        if (existingScript != null)
                        {
                            Script1.SMSWhatsAppSendStatus = "Both";
                        }
                        else
                        {
                            Script1.SMSWhatsAppSendStatus = "SMS";
                        }
                        if (Script1.WhatsAppMessageType == null)
                        {
                            Script1.WhatsAppMessageType = "Text";
                        }
                        context.tblSMSWhatsAppScriptMasters.AddOrUpdate(Script1);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSTransactionalScripts");
            }
            return result;
        }
        public tblSMSWhatsAppScriptMaster GetTransactionalSMSWASendStatus(int GroupId, string OutletId, string MessageType)
        {
            tblSMSWhatsAppScriptMaster objSMSWhatsAppScriptMaster = new tblSMSWhatsAppScriptMaster();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objSMSWhatsAppScriptMaster = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == OutletId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionalSMSWASendStatus");
            }
            return objSMSWhatsAppScriptMaster;
        }
        public bool SaveSMSWASendStatus(int GroupId, string OutletId,string MessageType,string SendStatus)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblSMSWhatsAppScriptMaster objSMSWhatsAppScriptMaster = new tblSMSWhatsAppScriptMaster();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var Script1 = context.tblSMSWhatsAppScriptMasters.Where(x => x.MessageType == MessageType && x.OutletId == OutletId).FirstOrDefault();
                    Script1.SMSWhatsAppSendStatus = SendStatus;
                    context.tblSMSWhatsAppScriptMasters.AddOrUpdate(Script1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSWASendStatus");
            }
            return result;
        }
        public List<SelectListItem> GetBirthdayScript(string GroupId)
        {
            List<SelectListItem> lstBirthday = new List<SelectListItem>();
            List<tblBirthdaySMSWAScript> BirthdayScript = new List<tblBirthdaySMSWAScript>();
            try
            {

                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    BirthdayScript = contextNew.tblBirthdaySMSWAScript.Where(x => x.IsActive == true).ToList();
                }
                foreach (var item in BirthdayScript)
                {
                    lstBirthday.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = Convert.ToString(item.Id)
                    });
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBirthdayScript");
            }
            return lstBirthday;
        }
        public tblBirthdaySMSWAScript GetBirthdaySMSWAScript(int GroupId, string Name)
        {
            tblBirthdaySMSWAScript objBirthdaySMSWAScript = new tblBirthdaySMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objBirthdaySMSWAScript = context.tblBirthdaySMSWAScript.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBirthdaySMSWAScript");
            }
            return objBirthdaySMSWAScript;
        }
        public bool SaveWABirthdayScripts(int GroupId, string Script1, string Name1,string BirthdayWAScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblBirthdaySMSWAScript objBirthdaySMSWAScript = new tblBirthdaySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew = context.tblBirthdaySMSWAScript.Where(x => x.Name == Name1).FirstOrDefault();
                    ScriptNew.WAScript = Script1;
                    ScriptNew.WhatsAppScriptType = BirthdayWAScriptType;
                    context.tblBirthdaySMSWAScript.AddOrUpdate(ScriptNew);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWABirthdayScripts");
            }
            return result;
        }
        public bool SaveSMSBirthdayScripts(int GroupId, string Script1, string Name1,string BirthdaySMSScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblBirthdaySMSWAScript objBirthdaySMSWAScript = new tblBirthdaySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew = context.tblBirthdaySMSWAScript.Where(x => x.Name == Name1).FirstOrDefault();
                    ScriptNew.SMSScript = Script1;
                    ScriptNew.SMSScriptType = BirthdaySMSScriptType;
                    context.tblBirthdaySMSWAScript.AddOrUpdate(ScriptNew);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSBirthdayScripts");
            }
            return result;
        }
        public tblBirthdaySMSWAScript GetBirthdaySMSWASendStatus(int GroupId, string Name)
        {
            tblBirthdaySMSWAScript objBirthdaySMSWAScript = new tblBirthdaySMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objBirthdaySMSWAScript = context.tblBirthdaySMSWAScript.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBirthdaySMSWASendStatus");
            }
            return objBirthdaySMSWAScript;
        }
        public bool SaveBirthdaySMSWASendStatus(int GroupId, string Name1, string SendStatus)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblBirthdaySMSWAScript objBirthdaySMSWAScript = new tblBirthdaySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew = context.tblBirthdaySMSWAScript.Where(x => x.Name == Name1).FirstOrDefault();
                    ScriptNew.SMSWhatsAppSendStatus = SendStatus;
                    context.tblBirthdaySMSWAScript.AddOrUpdate(ScriptNew);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBirthdaySMSWASendStatus");
            }
            return result;
        }
        public List<SelectListItem> GetAnniversaryScript(string GroupId)
        {
            List<SelectListItem> lstAnniversary = new List<SelectListItem>();
            List<tblAnniversarySMSWAScript> AnniversaryScript = new List<tblAnniversarySMSWAScript>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    AnniversaryScript = contextNew.tblAnniversarySMSWAScript.Where(x => x.IsActive == true).ToList();
                }
                foreach (var item in AnniversaryScript)
                {
                    lstAnniversary.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = Convert.ToString(item.Id)
                    });
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAnniversaryScript");
            }
            return lstAnniversary;
        }
        public tblAnniversarySMSWAScript GetAnniversarySMSWAScript(int GroupId, string Name)
        {
            tblAnniversarySMSWAScript objAnniversarySMSWAScript = new tblAnniversarySMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objAnniversarySMSWAScript = context.tblAnniversarySMSWAScript.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAnniversarySMSWAScript");
            }
            return objAnniversarySMSWAScript;
        }
        public bool SaveWAAnniversaryScripts(int GroupId, string Script2, string Name2,string AnniversaryWAScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblAnniversarySMSWAScript objAnniversarySMSWAScript = new tblAnniversarySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew1 = context.tblAnniversarySMSWAScript.Where(x => x.Name == Name2).FirstOrDefault();
                    ScriptNew1.WAScript = Script2;
                    ScriptNew1.WhatsAppScriptType = AnniversaryWAScriptType;
                    context.tblAnniversarySMSWAScript.AddOrUpdate(ScriptNew1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWAAnniversaryScripts");
            }
            return result;
        }
        public bool SaveSMSAnniversaryScripts(int GroupId, string Script2, string Name2,string AnniversarySMSScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblAnniversarySMSWAScript objAnniversarySMSWAScript = new tblAnniversarySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew1 = context.tblAnniversarySMSWAScript.Where(x => x.Name == Name2).FirstOrDefault();
                    ScriptNew1.SMSScript = Script2;
                    ScriptNew1.SMSScriptType = AnniversarySMSScriptType;
                    context.tblAnniversarySMSWAScript.AddOrUpdate(ScriptNew1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSAnniversaryScripts");
            }
            return result;
        }
        public tblAnniversarySMSWAScript GetAnniversarySMSWASendStatus(int GroupId, string Name)
        {
            tblAnniversarySMSWAScript objAnniversarySMSWAScript = new tblAnniversarySMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objAnniversarySMSWAScript = context.tblAnniversarySMSWAScript.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAnniversarySMSWASendStatus");
            }
            return objAnniversarySMSWAScript;
        }
        public bool SaveAnniversarySMSWASendStatus(int GroupId, string Name2, string SendStatus)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblAnniversarySMSWAScript objAnniversarySMSWAScript = new tblAnniversarySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew1 = context.tblAnniversarySMSWAScript.Where(x => x.Name == Name2).FirstOrDefault();
                    ScriptNew1.SMSWhatsAppSendStatus = SendStatus;
                    context.tblAnniversarySMSWAScript.AddOrUpdate(ScriptNew1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveAnniversarySMSWASendStatus");
            }
            return result;
        }
        public List<SelectListItem> GetInactiveScript(string GroupId)
        {
            List<SelectListItem> lstInactive = new List<SelectListItem>();
            List<tblInActiveSMSWAScript> InactiveScript = new List<tblInActiveSMSWAScript>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    InactiveScript = contextNew.tblInActiveSMSWAScripts.Where(x => x.IsActive == true).ToList();
                }
                foreach (var item in InactiveScript)
                {
                    lstInactive.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = Convert.ToString(item.Id)
                    });
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetInactiveScript");
            }
            return lstInactive;
        }
        public tblInActiveSMSWAScript GetInactiveSMSWAScript(int GroupId, string Name)
        {
            tblInActiveSMSWAScript objInActiveSMSWAScript = new tblInActiveSMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objInActiveSMSWAScript = context.tblInActiveSMSWAScripts.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetInactiveSMSWAScript");
            }
            return objInActiveSMSWAScript;

        }
        public bool SaveWAInactiveScripts(int GroupId, string Script3, string Name3,string InactiveWAScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblInActiveSMSWAScript objInActiveSMSWAScript = new tblInActiveSMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew2 = context.tblInActiveSMSWAScripts.Where(x => x.Name == Name3).FirstOrDefault();
                    ScriptNew2.WAScript = Script3;
                    ScriptNew2.WhatsAppScriptType = InactiveWAScriptType;
                    context.tblInActiveSMSWAScripts.AddOrUpdate(ScriptNew2);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWAInactiveScripts");
            }
            return result;
        }
        public bool SaveSMSInactiveScripts(int GroupId, string Script3, string Name3,string InactiveSMSScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblInActiveSMSWAScript objInActiveSMSWAScript = new tblInActiveSMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew2 = context.tblInActiveSMSWAScripts.Where(x => x.Name == Name3).FirstOrDefault();
                    ScriptNew2.SMSScript = Script3;
                    ScriptNew2.SMSScriptType = InactiveSMSScriptType;
                    context.tblInActiveSMSWAScripts.AddOrUpdate(ScriptNew2);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWAInactiveScripts");
            }
            return result;
        }
        public tblInActiveSMSWAScript GetInactiveSMSWASendStatus(int GroupId, string Name)
        {
            tblInActiveSMSWAScript objInActiveSMSWAScript = new tblInActiveSMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objInActiveSMSWAScript = context.tblInActiveSMSWAScripts.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetInactiveSMSWASendStatus");
            }
            return objInActiveSMSWAScript;
        }
        public bool SaveInactiveSMSWASendStatus(int GroupId, string Name3, string SendStatus)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblInActiveSMSWAScript objInActiveSMSWAScript = new tblInActiveSMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew2 = context.tblInActiveSMSWAScripts.Where(x => x.Name == Name3).FirstOrDefault();
                    ScriptNew2.SMSWhatsAppSendStatus = SendStatus;
                    context.tblInActiveSMSWAScripts.AddOrUpdate(ScriptNew2);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveInactiveSMSWASendStatus");
            }
            return result;
        }
        public List<SelectListItem> GetPointsExpiryScript(string GroupId)
        {
            List<SelectListItem> lstPointsExpiry = new List<SelectListItem>();
            List<tblPointsExpirySMSWAScript> PointsExpiry = new List<tblPointsExpirySMSWAScript>();
            try
            {

                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    PointsExpiry = contextNew.tblPointsExpirySMSWAScript.Where(x => x.IsActive == true).ToList();
                }
                foreach (var item in PointsExpiry)
                {
                    lstPointsExpiry.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = Convert.ToString(item.Id)
                    });
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsExpiryScript");
            }
            return lstPointsExpiry;
        }
        public tblPointsExpirySMSWAScript GetPointsExpirySMSWAScripts(int GroupId, string Name)
        {
            tblPointsExpirySMSWAScript objPointsExpirySMSWAScript = new tblPointsExpirySMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objPointsExpirySMSWAScript = context.tblPointsExpirySMSWAScript.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsExpirySMSWAScripts");
            }
            return objPointsExpirySMSWAScript;

        }
        public bool SaveWAPointsExpiryScripts(int GroupId, string Script4, string Name4,string PointsExpiryWAScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblPointsExpirySMSWAScript objPointsExpirySMSWAScript = new tblPointsExpirySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew3 = context.tblPointsExpirySMSWAScript.Where(x => x.Name == Name4).FirstOrDefault();
                    ScriptNew3.WAScript = Script4;
                    ScriptNew3.WhatsAppScriptType = PointsExpiryWAScriptType;
                    context.tblPointsExpirySMSWAScript.AddOrUpdate(ScriptNew3);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWAPointsExpiryScripts");
            }
            return result;
        }
        public bool SaveSMSPointsExpiryScripts(int GroupId, string Script4, string Name4,string PointsExpirySMSScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblPointsExpirySMSWAScript objPointsExpirySMSWAScript = new tblPointsExpirySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew3 = context.tblPointsExpirySMSWAScript.Where(x => x.Name == Name4).FirstOrDefault();
                    ScriptNew3.SMSScript = Script4;
                    ScriptNew3.SMSScriptType = PointsExpirySMSScriptType;
                    context.tblPointsExpirySMSWAScript.AddOrUpdate(ScriptNew3);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSPointsExpiryScripts");
            }
            return result;
        }
        public tblPointsExpirySMSWAScript GetPointsExpirySMSWASendStatus(int GroupId, string Name)
        {
            tblPointsExpirySMSWAScript objPointsExpirySMSWAScript = new tblPointsExpirySMSWAScript();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objPointsExpirySMSWAScript = context.tblPointsExpirySMSWAScript.Where(x => x.Name == Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsExpirySMSWAScripts");
            }
            return objPointsExpirySMSWAScript;
        }
        public bool SavePointsExpirySMSWASendStatus(int GroupId, string Name4, string SendStatus)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblPointsExpirySMSWAScript objPointsExpirySMSWAScript = new tblPointsExpirySMSWAScript();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var ScriptNew3 = context.tblPointsExpirySMSWAScript.Where(x => x.Name == Name4).FirstOrDefault();
                    ScriptNew3.SMSWhatsAppSendStatus = SendStatus;
                    context.tblPointsExpirySMSWAScript.AddOrUpdate(ScriptNew3);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SavePointsExpirySMSWASendStatus");
            }
            return result;
        }
        public List<SelectListItem> GetDLCScript(string GroupId)
        {
            List<SelectListItem> lstDLCScripts = new List<SelectListItem>();
            List<tblDLCSMSWAScriptMaster> DLCScript = new List<tblDLCSMSWAScriptMaster>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    DLCScript = contextNew.tblDLCSMSWAScriptMasters.Where(x => x.IsActive == true).ToList();
                }
                foreach (var item in DLCScript)
                {
                    lstDLCScripts.Add(new SelectListItem
                    {
                        Text = item.DLCMessageType,
                        Value = Convert.ToString(item.DLCMessageId)
                    });
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCScript");
            }
            return lstDLCScripts;
        }
        public tblDLCSMSWAScriptMaster GetDLCSMSWAScripts(int GroupId, string DLCMessageType)
        {
            tblDLCSMSWAScriptMaster objDLCSMSWAScriptMaster = new tblDLCSMSWAScriptMaster();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objDLCSMSWAScriptMaster = context.tblDLCSMSWAScriptMasters.Where(x => x.DLCMessageType == DLCMessageType).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCSMSWAScripts");
            }
            return objDLCSMSWAScriptMaster;
        }
        public bool SaveWADLCScripts(int GroupId, string DLCMessageType, string DLCScript,string DLCWAScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblDLCSMSWAScriptMaster objDLCSMSWAScriptMaster = new tblDLCSMSWAScriptMaster();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var DLCSMSScriptNew = context.tblDLCSMSWAScriptMasters.Where(x => x.DLCMessageType == DLCMessageType).FirstOrDefault();
                    DLCSMSScriptNew.DLCWAScript = DLCScript;
                    DLCSMSScriptNew.WhatsAppScriptType = DLCWAScriptType;
                    context.tblDLCSMSWAScriptMasters.AddOrUpdate(DLCSMSScriptNew);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveWADLCScripts");
            }
            return result;
        }
        public bool SaveSMSDLCScripts(int GroupId, string DLCMessageType, string DLCScript,string DLCSMSScriptType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblDLCSMSWAScriptMaster objDLCSMSWAScriptMaster = new tblDLCSMSWAScriptMaster();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var DLCSMSScriptNew = context.tblDLCSMSWAScriptMasters.Where(x => x.DLCMessageType == DLCMessageType).FirstOrDefault();
                    DLCSMSScriptNew.DLCSMSScript = DLCScript;
                    DLCSMSScriptNew.SMSScriptType = DLCSMSScriptType;
                    context.tblDLCSMSWAScriptMasters.AddOrUpdate(DLCSMSScriptNew);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSDLCScripts");
            }
            return result;
        }
        public tblDLCSMSWAScriptMaster GetDLCSMSWASendStatus(int GroupId, string DLCMessageType)
        {
            tblDLCSMSWAScriptMaster objDLCSMSWAScriptMaster = new tblDLCSMSWAScriptMaster();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    objDLCSMSWAScriptMaster = context.tblDLCSMSWAScriptMasters.Where(x => x.DLCMessageType == DLCMessageType).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCSMSWASendStatus");
            }
            return objDLCSMSWAScriptMaster;
        }
        public bool SaveDLCSMSWASendStatus(int GroupId, string DLCMessageType, string SendStatus)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            tblDLCSMSWAScriptMaster objDLCSMSWAScriptMaster = new tblDLCSMSWAScriptMaster();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var DLCSMSScriptNew = context.tblDLCSMSWAScriptMasters.Where(x => x.DLCMessageType == DLCMessageType).FirstOrDefault();
                    DLCSMSScriptNew.SMSWhatsAppSendStatus = SendStatus;
                    context.tblDLCSMSWAScriptMasters.AddOrUpdate(DLCSMSScriptNew);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCSMSWASendStatus");
            }
            return result;
        }
        public List<SelectListItem> GetOutletDetails(string GroupId)
        {
            List<SelectListItem> lstOutlet = new List<SelectListItem>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var Outlets = contextNew.tblOutletMasters.Where(x => x.GroupId == GroupId).ToList();

                    foreach (var item in Outlets)
                    {
                        lstOutlet.Add(new SelectListItem
                        {
                            Text = item.OutletName,
                            Value = Convert.ToString(item.OutletId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutlet");
            }
            return lstOutlet;
        }
        public SMSCredential GetSMSCredentials(string OutletId,string groupid)
        {
            SMSCredential obj = new SMSCredential();
            try
            {
                var connStr = CR.GetCustomerConnString((groupid));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    tblSMSWhatsAppCredential objSMSCredential = contextNew.tblSMSWhatsAppCredentials.Where(x => x.OutletId == OutletId).FirstOrDefault();

                    if (objSMSCredential != null)
                    {
                        obj.OutletId = objSMSCredential.OutletId;
                        obj.SMSVendor = objSMSCredential.SMSVendor;
                        obj.SMSUrl = objSMSCredential.SMSUrl;
                        obj.SMSLoginId = objSMSCredential.SMSLoginId;
                        obj.SMSPassword = objSMSCredential.SMSPassword;
                        obj.SMSAPIKey = objSMSCredential.SMSAPIKey;
                        obj.SMSSenderId = objSMSCredential.SMSSenderId;
                        obj.IsActiveSMS = Convert.ToBoolean(objSMSCredential.IsActiveSMS);
                    }
                    //else
                    //{
                    //    obj.OutletId = OutletId;
                    //    obj.SMSVendor = "";
                    //    obj.SMSUrl = "";
                    //    obj.SMSLoginId = "";
                    //    obj.SMSPassword = "";
                    //    obj.SMSAPIKey = "";
                    //    obj.SMSSenderId = "";
                    //    obj.IsActiveSMS = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSMSCredentials");
            }
            return obj;
        }
        public bool SaveSMSCredentials(tblSMSWhatsAppCredential ObjCredential,string groupid)
        {
            bool status = false;
            try
            {
                var connStr = CR.GetCustomerConnString((groupid));
                using (var context = new BOTSDBContext(connStr))
                {
                    tblSMSWhatsAppCredential objtblSMSWhatsAppCredential = context.tblSMSWhatsAppCredentials.Where(x => x.OutletId == ObjCredential.OutletId).FirstOrDefault();

                    if (objtblSMSWhatsAppCredential == null)
                    {
                        objtblSMSWhatsAppCredential = new tblSMSWhatsAppCredential();
                        objtblSMSWhatsAppCredential.OutletId = ObjCredential.OutletId;
                    }
                    objtblSMSWhatsAppCredential.SMSVendor = ObjCredential.SMSVendor;
                    objtblSMSWhatsAppCredential.SMSUrl = ObjCredential.SMSUrl;
                    objtblSMSWhatsAppCredential.SMSLoginId = ObjCredential.SMSLoginId;
                    objtblSMSWhatsAppCredential.SMSPassword = ObjCredential.SMSPassword;
                    objtblSMSWhatsAppCredential.SMSAPIKey = ObjCredential.SMSAPIKey;
                    objtblSMSWhatsAppCredential.SMSSenderId = ObjCredential.SMSSenderId;
                    objtblSMSWhatsAppCredential.IsActiveSMS = ObjCredential.IsActiveSMS;

                    context.tblSMSWhatsAppCredentials.AddOrUpdate(objtblSMSWhatsAppCredential);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveSMSCredentials");
            }
            return status;
        }
        public List<SelectListItem> GetOutlet(string GroupId)
        {
            List<SelectListItem> lstOutlets = new List<SelectListItem>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var Outlets = contextNew.tblOutletMasters.Where(x => x.GroupId == GroupId && !x.OutletName.ToLower().Contains("admin")).ToList();

                    foreach (var item in Outlets)
                    {
                        lstOutlets.Add(new SelectListItem
                        {
                            Text = item.OutletName,
                            Value = Convert.ToString(item.OutletId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutlet");
            }
            return lstOutlets;
        }
        public MemberData GetChangeNameByMobileNo(string GroupId, string searchData)
        {
            MemberData objMemberData = new MemberData();
            try
            {
                tblCustDetailsMaster objtblCustDetailsMaster = new tblCustDetailsMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objtblCustDetailsMaster = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == searchData && x.IsActive == true).FirstOrDefault();
                }
                if (objtblCustDetailsMaster != null)
                {
                    objMemberData.MemberName = objtblCustDetailsMaster.Name;
                    objMemberData.MobileNo = objtblCustDetailsMaster.MobileNo;
                    objMemberData.DisableSMSWAPromo = Convert.ToBoolean(objtblCustDetailsMaster.DisableSMSWAPromo);
                    objMemberData.DisableSMSWATxn = Convert.ToBoolean(objtblCustDetailsMaster.DisableSMSWATxn);
                    objMemberData.DisableTxn = Convert.ToBoolean(objtblCustDetailsMaster.DisableTxn);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameByMobileNo");
            }
            return objMemberData;
        }
        public bool DisablePromotionalSMS(string GroupId, string MobileNo, bool DisableSMSWAPromo)
        {
            bool status = false;
            try
            {
                tblCustDetailsMaster objtblCustDetailsMaster = new tblCustDetailsMaster();
                tblGroupMaster obj1 = new tblGroupMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objtblCustDetailsMaster = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    objtblCustDetailsMaster.DisableSMSWAPromo = DisableSMSWAPromo;

                    contextNew.tblCustDetailsMasters.AddOrUpdate(objtblCustDetailsMaster);
                    contextNew.SaveChanges();
                    status = true;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisablePromotionalSMS");
            }
            return status;
        }
        public GroupData GetCSNameByGroupId(string GroupId)
        {
            GroupData objMemberData = new GroupData();
            try
            {
                tblGroupDetail objCustomerDetail = new tblGroupDetail();
                string CSName = string.Empty;
                int varid = Convert.ToInt32(GroupId);
                using (var context = new CommonDBContext())
                {
                    //objCustomerDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                    objMemberData = context.Database.SqlQuery<GroupData>("select TR.RMAssignedName from tblGroupDetails TG inner join tblRMAssigned TR on TG.RMAssigned=TR.RMAssignedId where TG.GroupId = @Groupid", new SqlParameter("@Groupid", varid)).FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCSNameByGroupId");
            }
            return objMemberData;
        }
        public List<SelectListItem> GetRMAssignedList()
        {
            List<SelectListItem> lstRMAssigned = new List<SelectListItem>();
            List<tblRMAssigned> GroupDetails = new List<tblRMAssigned>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    GroupDetails = context.tblRMAssigneds.Where(x => x.IsActive == true).ToList();
                }
                foreach (var item in GroupDetails)
                {
                    lstRMAssigned.Add(new SelectListItem
                    {
                        Text = item.RMAssignedName,
                        Value = Convert.ToString(item.RMAssignedId)
                    });
                }
                lstRMAssigned = lstRMAssigned.OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupDetails");
            }
            return lstRMAssigned;
        }
        public bool SaveCSData(string GroupId, string RMAssignedId)
        {
            bool status = false;

            try
            {
                tblGroupDetail objCustomerDetail = new tblGroupDetail();
                tblRMAssigned obj = new tblRMAssigned();
                int varid = Convert.ToInt32(GroupId);
                int CSId = Convert.ToInt32(RMAssignedId);
                using (var context = new CommonDBContext())
                {
                    objCustomerDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                    objCustomerDetail.RMAssigned = CSId;

                    context.tblGroupDetails.AddOrUpdate(objCustomerDetail);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCSData");
            }
            return status;
        }
        public BurnData GetBurnRule(string GroupId)
        {
            BurnData objMemberData = new BurnData();
            try
            {
                tblRuleMaster objCustomerDetail = new tblRuleMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.tblRuleMasters.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
                if (objCustomerDetail != null)
                {
                    objMemberData.GroupId = objCustomerDetail.GroupId;
                    objMemberData.BurnMinTxnAmt = objCustomerDetail.BurnMinTxnAmt;
                    objMemberData.MinRedemptionPts = objCustomerDetail.MinRedemptionPts;
                    objMemberData.MinRedemptionPtsFirstTime = objCustomerDetail.MinRedemptionPtsFirstTime;
                    objMemberData.BurnInvoiceAmtPercentage = objCustomerDetail.BurnInvoiceAmtPercentage;
                    objMemberData.BurnDBPointsPercentage = objCustomerDetail.BurnDBPointsPercentage;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBurnRule");
            }
            return objMemberData;
        }
        public bool SaveBurnRule(tblRuleMaster ObjRuleMaster, string connectionstring, string FromName)
        {
            string _WAGroupCode = string.Empty;
            string CSName = string.Empty;
            Int32 RMId;
            int? IntGroupId = Convert.ToInt32(ObjRuleMaster.GroupId);
            bool status = false;
            ITCSMessage ObjCSMessage = new ITCSMessage();
            try
            {
                tblRuleMaster objRule = new tblRuleMaster();
                using (var con = new CommonDBContext())
                {

                    if (ObjRuleMaster.GroupId == "1051" || ObjRuleMaster.GroupId == "1002")
                    {
                        // Takes FineFoods Testing
                        _WAGroupCode = con.WAReports.Where(x => x.GroupId == "1051" && x.SMSStatus == "5").Select(y => y.GroupCode).FirstOrDefault();
                        RMId = (int)con.tblGroupDetails.Where(x => x.GroupId == IntGroupId).Select(y => y.RMAssigned).FirstOrDefault();
                        CSName = con.tblRMAssigneds.Where(x => x.RMAssignedId == RMId).Select(y => y.RMAssignedName).FirstOrDefault();
                    }
                    else
                    {
                        _WAGroupCode = con.WAReports.Where(x => x.GroupId == ObjRuleMaster.GroupId && x.SMSStatus == "0").Select(y => y.GroupCode).FirstOrDefault();
                    }

                }
                using (var context = new BOTSDBContext(connectionstring))
                {
                    objRule = context.tblRuleMasters.Where(x => x.GroupId == ObjRuleMaster.GroupId).FirstOrDefault();

                    if (objRule != null)
                    {
                        objRule.BurnMinTxnAmt = ObjRuleMaster.BurnMinTxnAmt;
                        objRule.MinRedemptionPts = ObjRuleMaster.MinRedemptionPts;
                        objRule.MinRedemptionPtsFirstTime = ObjRuleMaster.MinRedemptionPtsFirstTime;
                        objRule.BurnInvoiceAmtPercentage = ObjRuleMaster.BurnInvoiceAmtPercentage;
                        objRule.BurnDBPointsPercentage = ObjRuleMaster.BurnDBPointsPercentage;
                    }

                    context.tblRuleMasters.AddOrUpdate(objRule);
                    context.SaveChanges();
                    status = true;

                    ObjCSMessage.GroupCode = _WAGroupCode;
                    ObjCSMessage.CSName = CSName;
                    ObjCSMessage.BOTokenid = ConfigurationManager.AppSettings["BOTokenid"].ToString();
                    ObjCSMessage.WAAPILink = ConfigurationManager.AppSettings["WAAPILink"].ToString();
                    ObjCSMessage.OldBurnMinTxnAmt = Convert.ToString(ObjRuleMaster.OldBurnMinTxnAmt);
                    ObjCSMessage.OldMinRedemptionPts = Convert.ToString(ObjRuleMaster.OldMinRedemptionPts);
                    ObjCSMessage.OldMinRedemptionPtsFirstTime = Convert.ToString(ObjRuleMaster.OldMinRedemptionPtsFirstTime);
                    ObjCSMessage.OldBurnInvoiceAmtPercentage = Convert.ToString(ObjRuleMaster.OldBurnInvoiceAmtPercentage);
                    ObjCSMessage.OldBurnDBPointsPercentage = Convert.ToString(ObjRuleMaster.OldBurnDBPointsPercentage);
                    ObjCSMessage.BurnMinTxnAmt = Convert.ToString(ObjRuleMaster.BurnMinTxnAmt);
                    ObjCSMessage.MinRedemptionPts = Convert.ToString(ObjRuleMaster.MinRedemptionPts);
                    ObjCSMessage.MinRedemptionPtsFirstTime = Convert.ToString(ObjRuleMaster.MinRedemptionPtsFirstTime);
                    ObjCSMessage.BurnInvoiceAmtPercentage = Convert.ToString(ObjRuleMaster.BurnInvoiceAmtPercentage);
                    ObjCSMessage.BurnDBPointsPercentage = Convert.ToString(ObjRuleMaster.BurnDBPointsPercentage);
                    ObjCSMessage.FromName = FromName;
                    if (_WAGroupCode != null)
                    {
                        Thread _job2 = new Thread(() => SendWAMessageBurnRule(ObjCSMessage));
                        _job2.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBurnRule");

            }
            return status;
        }
        public Earndata GetEarnRule(string GroupId)
        {
            
            Earndata obj = new Earndata();
            try
            {

                tblRuleMaster objtblRuleMaster = new tblRuleMaster();
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));                
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objtblRuleMaster = contextNew.tblRuleMasters.Where(x => x.GroupId == GroupId && x.IsActive==true).FirstOrDefault();
                }
                if (objtblRuleMaster != null)
                {
                    obj.RuleName = objtblRuleMaster.RuleName;
                    //obj.StartDate = objtblRuleMaster.StartDate.Value.ToString("yyyy/MM/dd");
                    obj.StartDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    obj.EndDate = objtblRuleMaster.EndDate.Value.ToString("yyyy/MM/dd");
                    obj.EarnMinTxnAmt = objtblRuleMaster.EarnMinTxnAmt;
                    obj.PointsExpiryMonths = objtblRuleMaster.PointsExpiryMonths;
                    obj.PointsAllocation = objtblRuleMaster.PointsAllocation;
                    obj.PointsPercentage = objtblRuleMaster.PointsPercentage;
                    obj.Revolving = Convert.ToBoolean(objtblRuleMaster.Revolving);
                    //Get burn
                    obj.GroupId = objtblRuleMaster.GroupId;
                    obj.BurnMinTxnAmt = objtblRuleMaster.BurnMinTxnAmt;
                    obj.MinRedemptionPts = objtblRuleMaster.MinRedemptionPts;
                    obj.MinRedemptionPtsFirstTime = objtblRuleMaster.MinRedemptionPtsFirstTime;
                    obj.BurnInvoiceAmtPercentage = objtblRuleMaster.BurnInvoiceAmtPercentage;
                    obj.BurnDBPointsPercentage = objtblRuleMaster.BurnDBPointsPercentage;

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetEarnRule");
            }
            return obj;

        }
        public bool SaveEarnRule(tblRuleMaster ObjEarn, string connectionstring, string FromName)
        {
            string _WAGroupCode = string.Empty;
            string CSName = string.Empty;
            Int32 RMId;
            int? IntGroupId = Convert.ToInt32(ObjEarn.GroupId);
            bool status = false;
            ITCSMessage ObjCSMessage = new ITCSMessage();
            try
            {
                tblRuleMaster objCustomerDetail = new tblRuleMaster();

                using (var con = new CommonDBContext())
                {

                    if (ObjEarn.GroupId == "1051" || ObjEarn.GroupId == "1002")
                    {
                        // Takes FineFoods Testing
                        _WAGroupCode = con.WAReports.Where(x => x.GroupId == "1051" && x.SMSStatus == "5").Select(y => y.GroupCode).FirstOrDefault();
                        RMId = (int)con.tblGroupDetails.Where(x => x.GroupId == IntGroupId).Select(y => y.RMAssigned).FirstOrDefault();
                        CSName = con.tblRMAssigneds.Where(x => x.RMAssignedId == RMId).Select(y => y.RMAssignedName).FirstOrDefault();
                    }
                    else
                    {
                        _WAGroupCode = con.WAReports.Where(x => x.GroupId == ObjEarn.GroupId && x.SMSStatus == "0").Select(y => y.GroupCode).FirstOrDefault();
                    }

                }



                //Insert New Record
                using (var context = new BOTSDBContext(connectionstring))
                {
                    objCustomerDetail = context.tblRuleMasters.Where(x => x.GroupId == ObjEarn.GroupId).FirstOrDefault();
                    if (objCustomerDetail != null)
                    {
                        objCustomerDetail.GroupId = ObjEarn.GroupId;
                        objCustomerDetail.RuleName = ObjEarn.RuleName;
                        objCustomerDetail.StartDate = ObjEarn.StartDate;
                        objCustomerDetail.EndDate = ObjEarn.EndDate;
                        objCustomerDetail.EarnMinTxnAmt = ObjEarn.EarnMinTxnAmt;
                        objCustomerDetail.PointsExpiryMonths = ObjEarn.PointsExpiryMonths;
                        objCustomerDetail.PointsPercentage = ObjEarn.PointsPercentage;
                        objCustomerDetail.PointsAllocation = ObjEarn.PointsAllocation;
                        objCustomerDetail.Revolving = ObjEarn.Revolving;
                        objCustomerDetail.IsActive = true;

                        //BURN
                        //obj.GroupId = objCustomerDetail.GroupId;
                        objCustomerDetail.BurnMinTxnAmt = ObjEarn.BurnMinTxnAmt;
                        objCustomerDetail.MinRedemptionPts = ObjEarn.MinRedemptionPts;
                        objCustomerDetail.MinRedemptionPtsFirstTime = ObjEarn.MinRedemptionPtsFirstTime;
                        objCustomerDetail.BurnInvoiceAmtPercentage = ObjEarn.BurnInvoiceAmtPercentage;
                        objCustomerDetail.BurnDBPointsPercentage = ObjEarn.BurnDBPointsPercentage;
                    }

                    context.tblRuleMasters.Add(objCustomerDetail);
                    context.SaveChanges();
                    status = true;



                    objCustomerDetail = context.tblRuleMasters.Where(x => x.IsActive == true).FirstOrDefault();
                    if (objCustomerDetail != null)
                    {
                        objCustomerDetail.IsActive = false;
                        //objCustomerDetail.EndDate = DateTime.Now.Date;
                        objCustomerDetail.EndDate = DateTime.Now.AddDays(-1).Date;
                        context.tblRuleMasters.AddOrUpdate(objCustomerDetail);
                        context.SaveChanges();
                        status = true;
                    }

                    ObjCSMessage.GroupCode = _WAGroupCode;
                    ObjCSMessage.CSName = CSName;
                    ObjCSMessage.BOTokenid = ConfigurationManager.AppSettings["BOTokenid"].ToString();
                    ObjCSMessage.WAAPILink = ConfigurationManager.AppSettings["WAAPILink"].ToString();
                    ObjCSMessage.OldEarnMinTxnAmt = Convert.ToString(ObjEarn.OldEarnMinTxnAmt);
                    ObjCSMessage.OldPointsAllocation = Convert.ToString(ObjEarn.OldPointsAllocation);
                    ObjCSMessage.OldPointsExpiryMonths = Convert.ToString(ObjEarn.OldPointsExpiryMonths);
                    ObjCSMessage.OldPointsPercentage = Convert.ToString(ObjEarn.OldPointsPercentage);
                    ObjCSMessage.OldRevolvingStatus = Convert.ToString(ObjEarn.OldRevolvingStatus);
                    ObjCSMessage.EarnMinTxnAmt = Convert.ToString(ObjEarn.EarnMinTxnAmt);
                    ObjCSMessage.PointsAllocation = Convert.ToString(ObjEarn.PointsAllocation);
                    ObjCSMessage.PointsExpiryMonths = Convert.ToString(ObjEarn.PointsExpiryMonths);
                    ObjCSMessage.PointsPercentage = Convert.ToString(ObjEarn.PointsPercentage);
                    ObjCSMessage.Revolving = Convert.ToString(ObjEarn.Revolving);
                    ObjCSMessage.OldBurnMinTxnAmt = Convert.ToString(ObjEarn.OldBurnMinTxnAmt);
                    ObjCSMessage.OldMinRedemptionPts = Convert.ToString(ObjEarn.OldMinRedemptionPts);
                    ObjCSMessage.OldMinRedemptionPtsFirstTime = Convert.ToString(ObjEarn.OldMinRedemptionPtsFirstTime);
                    ObjCSMessage.OldBurnInvoiceAmtPercentage = Convert.ToString(ObjEarn.OldBurnInvoiceAmtPercentage);
                    ObjCSMessage.OldBurnDBPointsPercentage = Convert.ToString(ObjEarn.OldBurnDBPointsPercentage);
                    ObjCSMessage.BurnMinTxnAmt = Convert.ToString(ObjEarn.BurnMinTxnAmt);
                    ObjCSMessage.MinRedemptionPts = Convert.ToString(ObjEarn.MinRedemptionPts);
                    ObjCSMessage.MinRedemptionPtsFirstTime = Convert.ToString(ObjEarn.MinRedemptionPtsFirstTime);
                    ObjCSMessage.BurnInvoiceAmtPercentage = Convert.ToString(ObjEarn.BurnInvoiceAmtPercentage);
                    ObjCSMessage.BurnDBPointsPercentage = Convert.ToString(ObjEarn.BurnDBPointsPercentage);
                    ObjCSMessage.Message = ConfigurationManager.AppSettings["ITCSMessage"].ToString();
                    ObjCSMessage.FromName = FromName;

                    if (_WAGroupCode != null)
                    {
                        Thread _job2 = new Thread(() => SendWAMessageEarnRule(ObjCSMessage));
                        _job2.Start();
                    }

                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEarnRule");
            }
            return status;
        }


        public List<LisRules> GetRuleList(string groupId, string connectionString)
        {
            List<LisRules> CM = new List<LisRules>();

            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    using (var newcontext = new CommonDBContext())
                    {
                        var CM2 = context.tblRuleMasters.ToList();

                        foreach (var item in CM2)
                        {
                            LisRules itemData = new LisRules();
                            itemData.RuleName = Convert.ToString(item.RuleName);
                            itemData.MinRedemptionPts = item.MinRedemptionPts;
                            itemData.EarnMinTxnAmt = item.EarnMinTxnAmt;
                            itemData.BurnMinTxnAmt = item.BurnMinTxnAmt;
                            itemData.StartDate = item.StartDate.Value.ToString("yyyy-MM-dd");
                            itemData.MinRedemptionPts = item.MinRedemptionPts;
                            itemData.MinRedemptionPtsFirstTime = item.MinRedemptionPtsFirstTime;
                            itemData.IsActive = Convert.ToBoolean(item.IsActive);
                            itemData.EndDate = item.EndDate.Value.ToString("yyyy-MM-dd");
                            itemData.PointsAllocation = item.PointsAllocation;
                            itemData.PointsPercentage = item.PointsPercentage;
                            itemData.PointsExpiryMonths = item.PointsExpiryMonths;
                            itemData.BurnInvoiceAmtPercentage = item.BurnInvoiceAmtPercentage;
                            itemData.BurnDBPointsPercentage = item.BurnDBPointsPercentage;
                            CM.Add(itemData);
                        }


                    }

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetRuleList");
                }
            }
            return CM;
        }

        public PointExpiryDummyModel GetPointExpiryDetails(string groupid, string mobileNo)
        {
            PointExpiryDummyModel objData = new PointExpiryDummyModel();
            var connStr = CR.GetCustomerConnString(groupid);

            using (var context = new BOTSDBContext(connStr))
            {
                var pointExpiryData = context.tblCustPointsMasters.Where(x => x.MobileNo == mobileNo && x.PointsType == "Base").FirstOrDefault();
                if (pointExpiryData != null)
                {
                    objData.MobileNo = pointExpiryData.MobileNo;
                    objData.Points = pointExpiryData.Points;
                    objData.EndDate = pointExpiryData.EndDate.Value.ToString("MM/dd/yyyy");

                    var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    objData.CustName = custDetails.Name;
                }
            }

            return objData;
        }
        public bool DisableTransactionalSMS(string GroupId, string MobileNo, bool DisableSMSWATxn)
        {
            bool status = false;
            try
            {
                tblCustDetailsMaster objtblCustDetailsMaster = new tblCustDetailsMaster();
                tblGroupMaster obj1 = new tblGroupMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objtblCustDetailsMaster = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    objtblCustDetailsMaster.DisableSMSWATxn = DisableSMSWATxn;

                    contextNew.tblCustDetailsMasters.AddOrUpdate(objtblCustDetailsMaster);
                    contextNew.SaveChanges();
                    status = true;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableTransactionalSMS");
            }
            return status;
        }
        public bool DisableTransactions(string GroupId, string MobileNo, bool DisableTxn)
        {
            bool status = false;
            try
            {
                tblCustDetailsMaster objtblCustDetailsMaster = new tblCustDetailsMaster();
                tblGroupMaster obj1 = new tblGroupMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objtblCustDetailsMaster = contextNew.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    objtblCustDetailsMaster.DisableTxn = DisableTxn;

                    contextNew.tblCustDetailsMasters.AddOrUpdate(objtblCustDetailsMaster);
                    contextNew.SaveChanges();
                    status = true;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableTransactions");
            }
            return status;
        }
        public OTPData GetDefaultOTP(string OutletId, string GroupId)
        {
            OTPData objOTPData = new OTPData();
            try
            {
                tblOutletMaster objOutletMaster = new tblOutletMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objOutletMaster = contextNew.tblOutletMasters.Where(x => x.OutletId == OutletId).FirstOrDefault();
                }
                if (objOutletMaster != null)
                {
                    objOTPData.GroupId = objOutletMaster.GroupId;
                    objOTPData.OutletId = objOutletMaster.OutletId;
                    objOTPData.OTP = objOutletMaster.DefaultOTP;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDefaultOTP");
            }
            return objOTPData;
        }
        public bool SaveDefaultOTP(tblOutletMaster ObjOutletMaster, string connectionstring)
        {
            bool status = false;
            try
            {
                tblOutletMaster objOutlet = new tblOutletMaster();
                using (var context = new BOTSDBContext(connectionstring))
                {
                    objOutlet = context.tblOutletMasters.Where(x => x.OutletId == ObjOutletMaster.OutletId).FirstOrDefault();

                    if (objOutlet != null)
                    {
                        objOutlet.DefaultOTP = ObjOutletMaster.DefaultOTP;
                    }

                    context.tblOutletMasters.AddOrUpdate(objOutlet);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDefaultOTP");

            }
            return status;
        }
        public bool UpdateExpiryPointsDate(string mobileNo, string groupId, string expiryDate)
        {
            bool result = false;
            try
            {
                var connStr = CR.GetCustomerConnString((groupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    var pointExpiryData = context.tblCustPointsMasters.Where(x => x.MobileNo == mobileNo && x.PointsType == "Base").FirstOrDefault();
                    if (pointExpiryData != null)
                    {
                        if (pointExpiryData.EndDate < DateTime.Now.Date)
                        {
                            result = false;
                        }
                        else
                        {
                            pointExpiryData.EndDate = Convert.ToDateTime(expiryDate);
                            context.tblCustPointsMasters.AddOrUpdate(pointExpiryData);
                            context.SaveChanges();
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateExpiryPointsDate");
            }
            return result;
        }
        public List<PointExpiryDummyModel> GetPointExpiryDateRange(string groupid, string fromDate, string toDate)
        {
            List<PointExpiryDummyModel> objData = new List<PointExpiryDummyModel>();
            try
            {
                var connStr = CR.GetCustomerConnString((groupid));
                using (var context = new BOTSDBContext(connStr))
                {

                    objData = (from c in context.tblCustPointsMasters
                               join gd in context.tblCustDetailsMasters on c.MobileNo equals gd.MobileNo
                               select new PointExpiryDummyModel
                               {
                                   MobileNo = c.MobileNo,
                                   CustName = gd.Name,
                                   EDate = c.EndDate,
                                   EndDate = c.EndDate.ToString(),
                                   Points = c.Points
                               }).OrderByDescending(x => x.EndDate).ToList();
                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        var FDate = Convert.ToDateTime(fromDate);
                        var TDate = Convert.ToDateTime(toDate);
                        objData = objData.Where(x => x.EDate >= FDate && x.EDate <= TDate).ToList();
                    }
                    else if (!string.IsNullOrEmpty(fromDate))
                    {
                        var FDate = Convert.ToDateTime(fromDate);
                        objData = objData.Where(x => x.EDate >= FDate).ToList();
                    }
                    else if (!string.IsNullOrEmpty(toDate))
                    {
                        var TDate = Convert.ToDateTime(toDate);
                        objData = objData.Where(x => x.EDate <= TDate).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateExpiryPointsDate");
            }
            return objData;
        }
        public bool UpdateExpiryPointsRangeDate(string groupid, string fromDate, string toDate, string updateDate)
        {
            bool result = false;
            List<PointExpiryDummyModel> objData = new List<PointExpiryDummyModel>();
            try
            {
                var connStr = CR.GetCustomerConnString((groupid));
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = (from c in context.tblCustPointsMasters
                               join gd in context.tblCustDetailsMasters on c.MobileNo equals gd.MobileNo
                               select new PointExpiryDummyModel
                               {
                                   MobileNo = c.MobileNo,
                                   CustName = gd.Name,
                                   EDate = c.EndDate,
                                   EndDate = c.EndDate.ToString(),
                                   Points = c.Points
                               }).OrderByDescending(x => x.EndDate).ToList();
                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        var FDate = Convert.ToDateTime(fromDate);
                        var TDate = Convert.ToDateTime(toDate);
                        objData = objData.Where(x => x.EDate >= FDate && x.EDate <= TDate).ToList();
                    }
                    else if (!string.IsNullOrEmpty(fromDate))
                    {
                        var FDate = Convert.ToDateTime(fromDate);
                        objData = objData.Where(x => x.EDate >= FDate).ToList();
                    }
                    else if (!string.IsNullOrEmpty(toDate))
                    {
                        var TDate = Convert.ToDateTime(toDate);
                        objData = objData.Where(x => x.EDate <= TDate).ToList();
                    }
                    foreach (var item in objData)
                    {
                        var custItem = context.tblCustPointsMasters.Where(x => x.MobileNo == item.MobileNo).FirstOrDefault();
                        custItem.EndDate = Convert.ToDateTime(updateDate);
                        context.tblCustPointsMasters.AddOrUpdate(custItem);
                        context.SaveChanges();
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateExpiryPointsRangeDate");
            }
            return result;
        }
        public List<tblCampaignMaster> GetCampaignList(string groupid)
        {
            List<tblCampaignMaster> lstData = new List<tblCampaignMaster>();
            try
            {
                var connStr = CR.GetCustomerConnString((groupid));
                using (var context = new BOTSDBContext(connStr))
                {
                    lstData = context.tblCampaignMasters.Where(x => x.CampaignStatus != "Completed").ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignList");
            }
            return lstData;
        }
        public PointExpiryCampaignDetails GetCamaignPointExpiryDetails(string groupid, string campaignName)
        {
            PointExpiryCampaignDetails objData = new PointExpiryCampaignDetails();
            try
            {
                var connStr = CR.GetCustomerConnString((groupid));
                using (var context = new BOTSDBContext(connStr))
                {
                    var CampaignData = context.tblCampaignMasters.Where(x => x.CampaignName == campaignName).FirstOrDefault();
                    objData.EndDate = CampaignData.EndDate.Value.ToString("MM/dd/yyyy");
                    objData.CampaignStatus = CampaignData.CampaignStatus;
                    var count = context.tblCustPointsMasters.Where(x => x.PointsDesc == CampaignData.CampaignName).Count();
                    objData.NoOfUsers = count;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCamaignPointExpiryDetails");
            }
            return objData;
        }
        public bool UpdateCammpaignExpiryDate(string groupid, string campaignName, string updatedDate)
        {
            bool result = false;
            List<PointExpiryDummyModel> objData = new List<PointExpiryDummyModel>();
            try
            {
                var connStr = CR.GetCustomerConnString((groupid));
                using (var context = new BOTSDBContext(connStr))
                {
                    var cData = context.tblCampaignMasters.Where(x => x.CampaignName == campaignName).FirstOrDefault();
                    var expiryData = context.tblCustPointsMasters.Where(x => x.PointsDesc == campaignName).ToList();
                    cData.EndDate = Convert.ToDateTime(updatedDate);
                    context.tblCampaignMasters.AddOrUpdate(cData);
                    context.SaveChanges();

                    foreach (var item in expiryData)
                    {
                        item.EndDate = Convert.ToDateTime(updatedDate);
                        context.tblCustPointsMasters.AddOrUpdate(item);
                        context.SaveChanges();
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCamaignPointExpiryDetails");
            }
            return result;
        }
        public bool UpdateCelebrationData(string groupId, string mobileNo, string custName, string DOB, string DOA)
        {
            bool result = false;
            try
            {
                var connStr = CR.GetCustomerConnString(groupId);
                using (var context = new BOTSDBContext(connStr))
                {
                    var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    if (custDetails != null)
                    {
                        if (!string.IsNullOrEmpty(custName))
                            custDetails.Name = custName;
                        if (!string.IsNullOrEmpty(Convert.ToString(DOB)))
                            custDetails.DOB = Convert.ToDateTime(DOB);
                        if (!string.IsNullOrEmpty(Convert.ToString(DOA)))
                            custDetails.AnniversaryDate = Convert.ToDateTime(DOA);

                        context.tblCustDetailsMasters.AddOrUpdate(custDetails);
                        context.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateCelebrationData");
            }

            return result;
        }
        public List<SelectListItem> GetTierList(string GroupId)
        {
            List<SelectListItem> lstTier = new List<SelectListItem>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var MemberList = contextNew.Database.SqlQuery<string>("select distinct Tier from tblCustDetailsMaster").ToList();

                    foreach (var item in MemberList)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            lstTier.Add(new SelectListItem
                            {
                                Text = item,
                                Value = Convert.ToString(item)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTierList");
            }

            lstTier = lstTier.OrderBy(x => x.Text).ToList();

            return lstTier;
        }
        public List<MemberData> GetSlabWiseReport(string GroupId, string Tier)
        {
            List<tblCustDetailsMaster> lstMember = new List<tblCustDetailsMaster>();
            List<MemberData> lstData = new List<MemberData>();
            try
            {
                List<SlabData> lstSlab = new List<SlabData>();
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    lstSlab = contextNew.Database.SqlQuery<SlabData>("select MobileNo,Name,Tier,LastTxnDate,AvlPts from View_CustSlabWiseData", new SqlParameter("@Tier", Tier)).ToList<SlabData>();
                    if (lstSlab != null)
                    {
                        foreach (var Data in lstSlab)
                        {
                            MemberData Obj = new MemberData();
                            Obj.MobileNo = Data.MobileNo;
                            Obj.MemberName = Data.Name;
                            Obj.Tier = Data.Tier;
                            if (Data.LastTxnDate.HasValue)
                            {
                                Obj.LastTxnDate = Data.LastTxnDate.Value.ToString("yyyy/MM/dd");
                            }                           
                            Obj.PointsBalance = Data.AvlPts;

                            lstData.Add(Obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTransactionByMobileNo");
            }
            return lstData;
        }        
        public DemographicData GetDemographicDetails(string GroupId, string OutletId)
        {
            DemographicData objDemoData = new DemographicData();
            try
            {
                tblGroupOwnerInfo objCustomerDetail = new tblGroupOwnerInfo();
                tblOutletMaster objOutletMaster = new tblOutletMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.tblGroupOwnerInfoes.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    objOutletMaster = contextNew.tblOutletMasters.Where(x => x.OutletId == OutletId).FirstOrDefault();
                }
                if (objCustomerDetail != null)
                {
                    objDemoData.GroupId = objCustomerDetail.GroupId;
                    objDemoData.MobileNo = objCustomerDetail.MobileNo;
                    objDemoData.AlternateNo = objCustomerDetail.AlternateNo;
                    objDemoData.Email = objCustomerDetail.Email;
                    objDemoData.Address = objCustomerDetail.Address;
                    if (objCustomerDetail.DOB.HasValue)
                    {
                        objDemoData.DOB = objCustomerDetail.DOB.Value.ToString("yyyy/MM/dd");
                    }
                    if(objCustomerDetail.DOA.HasValue)
                    {
                        objDemoData.DOA = objCustomerDetail.DOA.Value.ToString("yyyy/MM/dd");
                    }
                    objDemoData.Gender = objCustomerDetail.Gender;
                    objDemoData.Name = objCustomerDetail.Name;
                }
                if (objOutletMaster != null && objOutletMaster.StoreAnniversaryDate.HasValue)
                {
                    objDemoData.StoreAnniversary = objOutletMaster.StoreAnniversaryDate.Value.ToString("yyyy/MM/dd");
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDemographicDetails");
            }
            return objDemoData;
        }
        public bool SaveDemographicDetails(tblGroupOwnerInfo ObjGroup, tblOutletMaster objOutletMaster, string connectionstring)
        {
            bool status = false;
            try
            {
                tblGroupOwnerInfo objGroupOwnerInfo = new tblGroupOwnerInfo();
                tblOutletMaster objOutlet = new tblOutletMaster();
                using (var context = new BOTSDBContext(connectionstring))
                {
                    objGroupOwnerInfo = context.tblGroupOwnerInfoes.Where(x => x.GroupId == ObjGroup.GroupId).FirstOrDefault();
                    objOutlet = context.tblOutletMasters.Where(x => x.OutletId == objOutletMaster.OutletId).FirstOrDefault();

                    if (objGroupOwnerInfo != null)
                    {
                        objGroupOwnerInfo.MobileNo = ObjGroup.MobileNo;
                        objGroupOwnerInfo.AlternateNo = ObjGroup.AlternateNo;
                        objGroupOwnerInfo.Email = ObjGroup.Email;
                        objGroupOwnerInfo.Address = ObjGroup.Address;
                        objGroupOwnerInfo.DOB = ObjGroup.DOB;
                        objGroupOwnerInfo.DOA = ObjGroup.DOA;
                        objGroupOwnerInfo.Gender = ObjGroup.Gender;
                        objGroupOwnerInfo.Name = ObjGroup.Name;
                    }
                    if (objOutlet != null)
                    {
                        objOutlet.StoreAnniversaryDate = objOutletMaster.StoreAnniversaryDate;
                    }

                    context.tblGroupOwnerInfoes.AddOrUpdate(objGroupOwnerInfo);
                    context.tblOutletMasters.AddOrUpdate(objOutlet);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDemographicDetails");

            }
            return status;
        }
        public OutletDetail GetOutletDetails(string GroupId, string OutletId)
        {
            OutletDetail objDemoData = new OutletDetail();
            try
            {
                tblGroupOwnerInfo objCustomerDetail = new tblGroupOwnerInfo();
                tblOutletMaster objOutletMaster = new tblOutletMaster();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.tblGroupOwnerInfoes.FirstOrDefault(x => x.GroupId == GroupId);
                    objOutletMaster = contextNew.tblOutletMasters.FirstOrDefault(x => x.OutletId == OutletId);
                    var maxOutletId = contextNew.tblOutletMasters.Where(x => x.GroupId == GroupId).Select(x => x.OutletId).DefaultIfEmpty().Max();
                    if (maxOutletId != null)
                    {
                        maxOutletId = maxOutletId + 1;
                    }
                }
                if (objCustomerDetail != null && objOutletMaster != null)
                {
                    objDemoData.GroupId = objCustomerDetail.GroupId;
                    objDemoData.OutletId = objOutletMaster.OutletId;
                    objDemoData.OutletName = objOutletMaster.OutletName;
                    objDemoData.BrandId = objOutletMaster.BrandId;
                    objDemoData.Address = objOutletMaster.Address;
                    objDemoData.City = objOutletMaster.City;
                    objDemoData.Area = objOutletMaster.Area;
                    objDemoData.Phone = objOutletMaster.Phone;
                    objDemoData.PinCode = objOutletMaster.Pincode;
                    objDemoData.DefaultOTP = objOutletMaster.DefaultOTP;
                    objDemoData.Latitude = objOutletMaster.Latitude;
                    objDemoData.Longitude = objOutletMaster.Longitude;
                    if (objOutletMaster.InvoiceDate.HasValue)
                    {
                        objDemoData.InvoiceDate = objOutletMaster.InvoiceDate.Value.ToString("yyyy/MM/dd");
                    }
                    if (objOutletMaster.LiveDate.HasValue)
                    {
                        objDemoData.LiveDate = objOutletMaster.LiveDate.Value.ToString("yyyy/MM/dd");
                    }
                    if (objOutletMaster.StoreAnniversaryDate.HasValue)
                    {
                        objDemoData.StoreAnniversaryDate = objOutletMaster.StoreAnniversaryDate.Value.ToString("yyyy/MM/dd");
                    }
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDemographicDetails");
            }
            return objDemoData;
        }
        public OutletDetail GetAssignOutletDetails(string GroupId)
        {
            OutletDetail objDemoData = new OutletDetail();
            try
            {
                using (var contextNew = new BOTSDBContext(CR.GetCustomerConnString(GroupId)))
                {
                    var objCustomerDetail = contextNew.tblGroupOwnerInfoes.FirstOrDefault(x => x.GroupId == GroupId);
                    var maxOutletId = contextNew.tblOutletMasters.Where(x => x.GroupId == GroupId).Select(x => x.OutletId).DefaultIfEmpty().Max();
                    if (maxOutletId == null)
                    {
                        maxOutletId = objCustomerDetail.GroupId + "1001";
                    }
                    else
                    {
                        maxOutletId = (Convert.ToInt32(maxOutletId) + 1).ToString();
                    }
                    objDemoData.OutletId = maxOutletId;
                    objDemoData.GroupId = objCustomerDetail.GroupId;
                    objDemoData.BrandId = GroupId + "1";
                    objDemoData.CounterId = objDemoData.OutletId + "01";
                    string securityKey;
                    do
                    {
                        securityKey = GenerateSecurityKey();
                    }
                    while (contextNew.tblStoreMasters.Any(x => x.Securitykey == securityKey));

                    objDemoData.Securitykey = securityKey;
                    objDemoData.CounterType = "Billing";
                    objDemoData.InvoiceDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    objDemoData.LiveDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    objDemoData.StoreAnniversaryDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAssignOutletDetails");
            }
            return objDemoData;
        }
        private string GenerateSecurityKey(int length = 10)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder key = new StringBuilder();
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    key.Append(validChars[(int)(num % (uint)validChars.Length)]);
                }
            }
            return key.ToString();
        }
        public bool SaveOutletDetails(tblOutletMaster objOutletMaster, string connectionString)
        {
            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    tblOutletMaster objOutlet = context.tblOutletMasters.FirstOrDefault(x => x.OutletId == objOutletMaster.OutletId);
                    if (objOutlet != null)
                    {
                        objOutlet.OutletName = objOutletMaster.OutletName;
                        objOutlet.BrandId = objOutletMaster.BrandId;
                        objOutlet.Address = objOutletMaster.Address;
                        objOutlet.GroupId = objOutletMaster.GroupId;
                        objOutlet.IsActive = objOutletMaster.IsActive;
                        objOutlet.City = objOutletMaster.City;
                        objOutlet.Area = objOutletMaster.Area;
                        objOutlet.Phone = objOutletMaster.Phone;
                        objOutlet.Pincode = objOutletMaster.Pincode;
                        objOutlet.DefaultOTP = objOutletMaster.DefaultOTP;
                        objOutlet.Latitude = objOutletMaster.Latitude;
                        objOutlet.Longitude = objOutletMaster.Longitude;
                        objOutlet.InvoiceDate = objOutletMaster.InvoiceDate;
                        objOutlet.LiveDate = objOutletMaster.LiveDate;
                        objOutlet.StoreAnniversaryDate = objOutletMaster.StoreAnniversaryDate;
                    }
                    else
                    {
                        context.tblOutletMasters.Add(objOutletMaster);
                    }
                    context.SaveChanges();
                    status = true;
                    tblSMSWhatsAppScriptMaster objsmsScript = context.tblSMSWhatsAppScriptMasters.FirstOrDefault(x => x.OutletId == objOutletMaster.OutletId);
                    if (objsmsScript == null)
                    {
                        var Data = new List<tblSMSWhatsAppScriptMaster>
                        {
                             new tblSMSWhatsAppScriptMaster { Id = "100", MessageType = "Enrollment", OutletId = objOutletMaster.OutletId },
                             new tblSMSWhatsAppScriptMaster { Id = "101", MessageType = "Earn", OutletId = objOutletMaster.OutletId },
                             new tblSMSWhatsAppScriptMaster { Id = "102", MessageType = "Burn", OutletId = objOutletMaster.OutletId },
                             new tblSMSWhatsAppScriptMaster { Id = "103", MessageType = "Cancel Earn", OutletId = objOutletMaster.OutletId },
                             new tblSMSWhatsAppScriptMaster { Id = "104", MessageType = "Cancel Burn", OutletId = objOutletMaster.OutletId },
                             new tblSMSWhatsAppScriptMaster { Id = "105", MessageType = "OTP", OutletId = objOutletMaster.OutletId },
                             new tblSMSWhatsAppScriptMaster { Id = "106", MessageType = "Balance > 0", OutletId = objOutletMaster.OutletId },
                             new tblSMSWhatsAppScriptMaster { Id = "107", MessageType = "Balance < 0", OutletId = objOutletMaster.OutletId }
                        };
                        context.tblSMSWhatsAppScriptMasters.AddRange(Data);
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveOutletDetails");
            }
            return status;
        }
        public bool SaveOutletCrediantialDetails(tblSMSWhatsAppCredential objCrediantialMaster, string connectionString)
        {
            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    tblSMSWhatsAppCredential objcrediantial = context.tblSMSWhatsAppCredentials.FirstOrDefault(x => x.OutletId == objCrediantialMaster.OutletId);
                    if (objcrediantial != null)
                    {
                        objcrediantial.OutletId = objCrediantialMaster.OutletId;
                        objcrediantial.SMSVendor = objCrediantialMaster.SMSVendor;
                        objcrediantial.SMSUrl = objCrediantialMaster.SMSUrl;
                        objcrediantial.SMSLoginId = objCrediantialMaster.SMSLoginId;
                        objcrediantial.SMSPassword = objCrediantialMaster.SMSPassword;
                        objcrediantial.WhatsAppMessageType = objCrediantialMaster.WhatsAppMessageType;
                        objcrediantial.SMSAPIKey = objCrediantialMaster.SMSAPIKey;
                        objcrediantial.WhatsAppVendor = objCrediantialMaster.WhatsAppVendor;
                        objcrediantial.WhatsAppUrl = objCrediantialMaster.WhatsAppUrl;
                        objcrediantial.WhatsAppTokenId = objCrediantialMaster.WhatsAppTokenId;
                        objcrediantial.VerifiedWhatsAppUrl = objCrediantialMaster.VerifiedWhatsAppUrl;
                        objcrediantial.VerifiedWhatsAppLoginId = objCrediantialMaster.VerifiedWhatsAppLoginId;
                        objcrediantial.VerifiedWhatsAppPassword = objCrediantialMaster.VerifiedWhatsAppPassword;
                        objcrediantial.VerifiedWhatsAppAPIKey = objCrediantialMaster.VerifiedWhatsAppAPIKey;
                        objcrediantial.SMSSenderId = objCrediantialMaster.SMSSenderId;
                        objcrediantial.VerifiedWhatsAppVendor = objCrediantialMaster.VerifiedWhatsAppVendor;
                    }
                    else
                    {
                        context.tblSMSWhatsAppCredentials.Add(objCrediantialMaster);
                    }
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveOutletCrediantialDetails");
            }
            return status;
        }
        public bool SaveOutletStoreDetails(tblStoreMaster objStoreMaster, string connectionString, string GroupId,string LoginCS,string EmailId)
        {
            bool status = false;
            tblOutletMaster objOutlet = new tblOutletMaster();
            tblGroupMaster objGroup = new tblGroupMaster();
            try
            {
              
                using (var context = new BOTSDBContext(connectionString))
                {
                    tblStoreMaster existingStore = context.tblStoreMasters.FirstOrDefault(x => x.OutletId == objStoreMaster.OutletId);
                    objGroup.GroupName = context.tblGroupMasters.Where(x => x.GroupId == GroupId).Select(x => x.GroupName).FirstOrDefault();
                    objOutlet.OutletName = context.tblOutletMasters.Where(x => x.GroupId == GroupId).OrderByDescending(x => x.OutletId).Select(x => x.OutletName).FirstOrDefault();
                    if (existingStore != null)
                    {
                        existingStore.CounterId = objStoreMaster.CounterId;
                        existingStore.CounterType = objStoreMaster.CounterType;
                        existingStore.Securitykey = objStoreMaster.Securitykey;
                        existingStore.OutletId = objStoreMaster.OutletId;
                        existingStore.IsActive = true;
                        existingStore.CreatedDate = DateTime.Today;
                    }
                    else
                    {
                        context.tblStoreMasters.Add(objStoreMaster);
                    }
                    context.SaveChanges();
                    status = true;
                    SendEmailComplete(objGroup.GroupName, objOutlet.OutletName, objStoreMaster.CounterId, objStoreMaster.Securitykey, LoginCS, EmailId);
                }

                using (var context = new CommonDBContext())
                {
                    tblDatabaseDetail objtblDatabaseDetail = new tblDatabaseDetail();
                    var group = context.tblDatabaseDetails.FirstOrDefault(x => x.GroupId == GroupId);
                    var DBNameNew = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(x => x.DBName).FirstOrDefault();
                    tblGroupDetail objOutletcount = new tblGroupDetail();
                    if (objtblDatabaseDetail != null)
                    {
                        objtblDatabaseDetail.CounterId = objStoreMaster.CounterId;
                        objtblDatabaseDetail.SecurityKey = objStoreMaster.Securitykey;
                        objtblDatabaseDetail.IPAddress = "52.66.245.116";
                        objtblDatabaseDetail.DBPassword = "F59VM$KDE@KF!AW";
                        objtblDatabaseDetail.DBId = "Renaldo";
                        objtblDatabaseDetail.GroupId = group.GroupId;
                        objtblDatabaseDetail.IsActive = true;
                        objtblDatabaseDetail.DBName = DBNameNew;

                        context.tblDatabaseDetails.Add(objtblDatabaseDetail);
                    }
                    else
                    {
                        context.tblDatabaseDetails.Add(objtblDatabaseDetail);
                    }
                    context.SaveChanges();
                    status = true;
                    objOutletcount = context.tblGroupDetails.Where(x => x.GroupId.ToString() == GroupId).FirstOrDefault();
                    if (objOutletcount != null)
                    {
                        objOutletcount.OutletCount++;
                        context.tblGroupDetails.AddOrUpdate(objOutletcount);
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveOutletStoreDetails");
            }
            return status;
        }
        public void SendEmailComplete(string DBName,string outletName, string CounterId, string Securitykey,string LoginCS,string EmailId)
        {
            string responseString;
            var from = ConfigurationManager.AppSettings["Email"].ToString();
            var PWD = ConfigurationManager.AppSettings["EmailPassword"].ToString();
            var smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"].ToString();
            //EmailId = "dinesh@blueocktopus.in";
            //var from = EmailId;
            //var PWD = "JedheKiran@123@";
            //  var smtpAddress = "smtp.zoho.com";
            var PortNo = 587;
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.AppendLine("<td>Dear Vikas,");
                    str.AppendLine("</tr>");
                    str.Append("<tr><td>&nbsp;</td></tr>");
                    str.AppendLine("<td><b>Description:"+"&nbsp;"+"</b>New Outlet Added. Please do some testing.</td>");
                    //str.AppendLine("<td><b>Description: </b>New Outlet Added in"+ DBName + "Please do some testing.</td>");
                    str.Append("<tr><td>Customer Name: " + DBName + "</td></tr>");
                    str.Append("<tr><td>Outlet Name: " + outletName + "</td></tr>");
                    str.Append("<tr><td>Counter ID: " + CounterId + "</td></tr>");
                    str.Append("<tr><td>Security Key: " + Securitykey + "</td></tr>");
                    str.AppendLine("</tr>");
                    str.Append("<tr><td>&nbsp;</td></tr>");
                    str.Append("<tr><td>Regards,");
                    str.Append("<tr><td>-" + LoginCS + "</td></tr>");
                    str.Append("</table>");

                    mail.From = new MailAddress(from);
                    mail.To.Add("kiran@blueocktopus.in");
                    mail.To.Add("vikas@blueocktopus.in");
                    mail.CC.Add("jacqueline@blueocktopus.in");
                    mail.CC.Add("dinesh @blueocktopus.in");
                    if (!string.IsNullOrEmpty(EmailId))
                        mail.CC.Add(EmailId);
                    mail.Subject = DBName+":"+"&nbsp;"+ "New Outlet Added";
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, PortNo))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, PWD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }            
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendEmailComplete");
            }
        }
        public void AddCSLog(tblAuditC objData)
        {
            using (var context = new CommonDBContext())
            {
                context.tblAuditCS.Add(objData);
                context.SaveChanges();
            }
        }
        public void SendWAMessageEarnRule(ITCSMessage ObjCSMessage)
        {
            string responseString;

            try
            {
                //objMsg.Message = objMsg.Message.Replace("#01", objMsg.SpokenTo);
                //objMsg.Message = HttpUtility.UrlEncode(objMsg.Message);
                //string type = "TEXT";
                StringBuilder stb = new StringBuilder();
                stb.AppendLine("Dear Customer,");
                stb.AppendLine();
                stb.AppendLine("As discussed the Loyalty Points Rule have been changed :");
                stb.AppendLine();
                stb.AppendLine("*Old Rule*");
                stb.AppendLine("--------------");
                stb.AppendLine("EarnMinTxnAmt : "+ ObjCSMessage.OldEarnMinTxnAmt);
                stb.AppendLine("PointsAllocation : " + ObjCSMessage.OldPointsAllocation);
                stb.AppendLine("PointsExpiryMonths : " + ObjCSMessage.OldPointsExpiryMonths);
                stb.AppendLine("PointsPercentage : " + ObjCSMessage.OldPointsPercentage);
                stb.AppendLine("PointsRevolving : " + ObjCSMessage.OldRevolvingStatus);
                stb.AppendLine("BurnMinTxnAmt : " + ObjCSMessage.OldBurnMinTxnAmt);
                stb.AppendLine("MinRedemptionPts : " + ObjCSMessage.OldMinRedemptionPts);
                stb.AppendLine("MinRedemptionPtsFirstTime : " + ObjCSMessage.OldMinRedemptionPtsFirstTime);
                stb.AppendLine("BurnInvoiceAmtPercentage : " + ObjCSMessage.OldBurnInvoiceAmtPercentage);
                stb.AppendLine("BurnDBPointsPercentage : " + ObjCSMessage.OldBurnDBPointsPercentage);
                stb.AppendLine();
                stb.AppendLine("*New Rule*");
                stb.AppendLine("--------------");
                stb.AppendLine("EarnMinTxnAmt : " + ObjCSMessage.EarnMinTxnAmt);
                stb.AppendLine("PointsAllocation : " + ObjCSMessage.PointsAllocation);
                stb.AppendLine("PointsExpiryMonths : " + ObjCSMessage.PointsExpiryMonths);
                stb.AppendLine("PointsPercentage : " + ObjCSMessage.PointsPercentage);
                stb.AppendLine("PointsRevolving : " + ObjCSMessage.Revolving);
                stb.AppendLine("BurnMinTxnAmt : " + ObjCSMessage.BurnMinTxnAmt);
                stb.AppendLine("MinRedemptionPts : " + ObjCSMessage.MinRedemptionPts);
                stb.AppendLine("MinRedemptionPtsFirstTime : " + ObjCSMessage.MinRedemptionPtsFirstTime);
                stb.AppendLine("BurnInvoiceAmtPercentage : " + ObjCSMessage.BurnInvoiceAmtPercentage);
                stb.AppendLine("BurnDBPointsPercentage : " + ObjCSMessage.BurnDBPointsPercentage);
                stb.AppendLine();
                stb.AppendLine("Regards,");
                stb.AppendLine(" - " + ObjCSMessage.FromName);
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(ObjCSMessage.WAAPILink);
                sbposdata.AppendFormat("token={0}", ObjCSMessage.BOTokenid);
                sbposdata.AppendFormat("&phone={0}", ObjCSMessage.GroupCode);
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
        public void SendWAMessageBurnRule(ITCSMessage ObjCSMessage)
        {
            string responseString;

            try
            {
                //objMsg.Message = objMsg.Message.Replace("#01", objMsg.SpokenTo);
                //objMsg.Message = HttpUtility.UrlEncode(objMsg.Message);
                //string type = "TEXT";
                StringBuilder stb = new StringBuilder();
                stb.AppendLine("Dear Customer,");
                stb.AppendLine();
                stb.AppendLine("As discussed the Loyalty Burn Rule have been changed :");
                stb.AppendLine();
                stb.AppendLine("*Old Rule*");
                stb.AppendLine("--------------");
                stb.AppendLine("BurnMinTxnAmt : " + ObjCSMessage.OldBurnMinTxnAmt);
                stb.AppendLine("MinRedemptionPts : " + ObjCSMessage.OldMinRedemptionPts);
                stb.AppendLine("MinRedemptionPtsFirstTime : " + ObjCSMessage.OldMinRedemptionPtsFirstTime);
                stb.AppendLine("BurnInvoiceAmtPercentage : " + ObjCSMessage.OldBurnInvoiceAmtPercentage);
                stb.AppendLine("BurnDBPointsPercentage : " + ObjCSMessage.OldBurnDBPointsPercentage);
                stb.AppendLine();
                stb.AppendLine("*New Rule*");
                stb.AppendLine("--------------");
                stb.AppendLine("BurnMinTxnAmt : " + ObjCSMessage.BurnMinTxnAmt);
                stb.AppendLine("MinRedemptionPts : " + ObjCSMessage.MinRedemptionPts);
                stb.AppendLine("MinRedemptionPtsFirstTime : " + ObjCSMessage.MinRedemptionPtsFirstTime);
                stb.AppendLine("BurnInvoiceAmtPercentage : " + ObjCSMessage.BurnInvoiceAmtPercentage);
                stb.AppendLine("BurnDBPointsPercentage : " + ObjCSMessage.BurnDBPointsPercentage);
                stb.AppendLine();
                stb.AppendLine("Regards,");
                stb.AppendLine(" - " + ObjCSMessage.FromName);
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(ObjCSMessage.WAAPILink);
                sbposdata.AppendFormat("token={0}", ObjCSMessage.BOTokenid);
                sbposdata.AppendFormat("&phone={0}", ObjCSMessage.GroupCode);
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
        public bool UpdateDisablePromoData(string groupId, string mobileNo, bool IsPromo, bool IsTxn, bool IsStatus)
        {
            bool result = false;
            try
            {
                var connStr = CR.GetCustomerConnString(groupId);
                using (var context = new BOTSDBContext(connStr))
                {
                    var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    if (custDetails != null)
                    {
                        if (IsPromo)
                            custDetails.DisableSMSWAPromo = IsStatus;
                        if (IsTxn)
                            custDetails.DisableSMSWATxn = IsStatus;

                        context.tblCustDetailsMasters.AddOrUpdate(custDetails);
                        context.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateDisablePromoData");
            }

            return result;
        }
        //public bool UpdateDisableTxnData(string groupId, string mobileNo)
        //{
        //    bool result = false;
        //    try
        //    {
        //        var connStr = CR.GetCustomerConnString(groupId);
        //        using (var context = new BOTSDBContext(connStr))
        //        {
        //            var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
        //            custDetails.DisableSMSWATxn = true;
        //            context.tblCustDetailsMasters.AddOrUpdate(custDetails);
        //            context.SaveChanges();
        //            result = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        newexception.AddException(ex, "UpdateDisableTxnData");
        //    }

        //    return result;
        //}
        public bool UpdateDisableLoyaltyData(string groupId, string mobileNo)
        {
            bool result = false;
            try
            {
                var connStr = CR.GetCustomerConnString(groupId);
                using (var context = new BOTSDBContext(connStr))
                {
                    var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    if (custDetails != null)
                    {
                        custDetails.DisableTxn = true;
                        context.tblCustDetailsMasters.AddOrUpdate(custDetails);
                        context.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateDisableLoyaltyData");
            }

            return result;
        }

        public string GetGroupRenewalDate(int GroupId)
        {
            string renewalDate = string.Empty;
            using (var context = new CommonDBContext())
            {
                var RDate = context.tblGroupDetails.Where(x => x.GroupId == GroupId).Select(y => y.RenewalDate).FirstOrDefault();
                if (RDate.HasValue)
                    renewalDate = RDate.Value.ToString("dd-MMM-yyyy");
            }
            return renewalDate;
        }
        public bool UpdateRenewalDate(int GroupId,DateTime RenewalDate)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var GroupDetails = context.tblGroupDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    GroupDetails.RenewalDate = RenewalDate;
                    context.tblGroupDetails.AddOrUpdate(GroupDetails);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "UpdateRenewalDateRepo");
            }
            return status;
        }
    }
}
