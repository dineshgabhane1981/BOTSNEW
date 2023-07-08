using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models.RetailerWeb;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
                    GroupDetail obj1 = new GroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsActive = false;
                        groupDetail.IsLive = false;
                        
                        context.SaveChanges();
                        status = true;

                        
                        var groupdetails = context.GroupDetail.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if(groupdetails.ActiveStatus == "Yes")
                        {
                            groupdetails.ActiveStatus = "No";
                        }
                        else
                        {
                            groupdetails.ActiveStatus = "No";
                        }
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
                    GroupDetail obj1 = new GroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsActive = true;
                        groupDetail.IsLive = true;
                        context.SaveChanges();
                        status = true;

                        var groupdetails = context.GroupDetail.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (groupdetails.ActiveStatus == "No")
                        {
                            groupdetails.ActiveStatus = "Yes";
                        }
                        else
                        {
                            groupdetails.ActiveStatus = "Yes";
                        }
                        context.SaveChanges();
                        status = true;
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
      
        public WhatsAppSMSMaster GetWAScripts(int GroupId, string GroupName, string MessageType)
        {
            WhatsAppSMSMaster obj = new WhatsAppSMSMaster();
            string Id;
            string Script;
            Id = string.Empty;
            Script = string.Empty;
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))
                {
                    //obj = context.WhatsAppSMSMasters.Where(x=> x.MessageId == MessageType).FirstOrDefault();

                    if (MessageType == "Enrollment")
                    {
                        Id = "100";
                    }
                    else if (MessageType == "Earn")
                    {
                        Id = "101";
                    }
                    else if (MessageType == "Burn")
                    {
                        Id = "102";
                    }
                    else if (MessageType == "Cancel")
                    {
                        Id = "103";
                    }
                    obj = context.WhatsAppSMSMasters.Where(x => x.MessageId == Id).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetWAScripts");
            }
            return obj;
           
        }

        public bool SaveScripts (int GroupId, string Script,string MessageType)
        {
            bool result = false;
            string Id;
            Id = string.Empty;
            tblGroupDetail obj = new tblGroupDetail();
            WhatsAppSMSMaster obj1 = new WhatsAppSMSMaster();
            WhatsAppSMSMaster objWhatsAppSMSMaster = new WhatsAppSMSMaster();
            try
            {
                var connStr = CR.GetCustomerConnString(Convert.ToString(GroupId));
                using (var context = new BOTSDBContext(connStr))

                {
                    if (MessageType == "Enrollment")
                    {
                        Id = "100";
                    }
                    else if (MessageType == "Earn")
                    {
                        Id = "101";
                    }
                    else if (MessageType == "Burn")
                    {
                        Id = "102";
                    }
                    else if (MessageType == "Cancel")
                    {
                        Id = "103";
                    }
                    var Script1 = context.WhatsAppSMSMasters.Where(x => x.MessageId == Id).FirstOrDefault();
                    obj1.SMS = Script;
                    obj1.SlNo = Convert.ToInt64(Script1.SlNo);
                    obj1.MessageId = Convert.ToString(Script1.MessageId);
                    obj1.OutletId= Convert.ToString(Script1.OutletId);
                    obj1.TokenId = Convert.ToString(Script1.TokenId);
                    obj1.BrandId = Convert.ToString(Script1.BrandId);
                   
                    context.WhatsAppSMSMasters.AddOrUpdate(obj1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return result;
        }
        public MemberData GetChangeNameByMobileNo(string GroupId, string searchData)
        {
            MemberData objMemberData = new MemberData();
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.MobileNo == searchData && x.Status == "00").FirstOrDefault();
                }
                if (objCustomerDetail != null)
                {
                    objMemberData.MemberName = objCustomerDetail.CustomerName;
                    objMemberData.MobileNo = objCustomerDetail.MobileNo;

                    //using (var contextNew = new BOTSDBContext(connStr))
                    //{
                    //    objMemberData.EnrolledOutletName = contextNew.OutletDetails.Where(x => x.OutletId == objCustomerDetail.EnrollingOutlet).Select(y => y.OutletName).FirstOrDefault();
                    //}
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameByMobileNo");
            }
            return objMemberData;
        }
        public bool DisablePromotionalSMS(string GroupId,string MobileNo)
        {
            bool status = false;
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                GroupDetail obj1 = new GroupDetail();
                string connStr = CR.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objCustomerDetail = contextNew.CustomerDetails.Where(x => x.MobileNo == MobileNo).FirstOrDefault();                    
                    objCustomerDetail.IsSMS = false;
                    
                    contextNew.CustomerDetails.AddOrUpdate(objCustomerDetail);
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
               //if (objCustomerDetail != null)
               //   {
               //         objMemberData.CSName = objCustomerDetail.RMAssigned;
               //   }
                
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
    }
}
