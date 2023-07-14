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
                    DailyActivityAllGroup obj1 = new DailyActivityAllGroup();
                    tblDatabaseDetail objtblDatabaseDetail = new tblDatabaseDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsActive = false;
                        groupDetail.IsLive = false;
                        
                        context.SaveChanges();
                        status = true;

                        
                        var DatabaseDetail = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        DatabaseDetail.IsActive = false;

                        context.SaveChanges();
                        status = true;

                        var DailyActivityGroup = context.DailyActivityAllGroups.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        DailyActivityGroup.IsActive = false;

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
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsActive = true;
                        groupDetail.IsLive = true;
                        context.SaveChanges();
                        status = true;

                        var DatabaseDetail = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        DatabaseDetail.IsActive = true;

                        context.SaveChanges();
                        status = true;

                        var DailyActivityGroup = context.DailyActivityAllGroups.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        DailyActivityGroup.IsActive = true;

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
        public bool SaveBurnRule(tblRuleMaster ObjRuleMaster, string connectionstring)
        {
            bool status = false;
            try
            {
                tblRuleMaster objRule = new tblRuleMaster();               
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
                //int varid = Convert.ToInt32(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objtblRuleMaster = contextNew.tblRuleMasters.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
                if (objtblRuleMaster != null)
                {
                    obj.EarnMinTxnAmt = objtblRuleMaster.EarnMinTxnAmt;
                    obj.PointsExpiryMonths = objtblRuleMaster.PointsExpiryMonths;
                    obj.PointsAllocation = objtblRuleMaster.PointsAllocation;
                    obj.PointsPercentage = objtblRuleMaster.PointsPercentage;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetEarnRule");
            }
            return obj;

        }
        public bool SaveEarnRule(tblRuleMaster ObjEarn, string connectionstring)
        {
            bool status = false;
            try
            {
                tblRuleMaster objCustomerDetail = new tblRuleMaster();
                //BrandDetail obj = new BrandDetail();
                using (var context = new BOTSDBContext(connectionstring))
                {
                    objCustomerDetail = context.tblRuleMasters.Where(x => x.GroupId == ObjEarn.GroupId).FirstOrDefault();
                    //obj = context.BrandDetails.Where(x => x.GroupId == ObjGroup.GroupId).FirstOrDefault();
                    if (objCustomerDetail != null)
                    {
                        objCustomerDetail.GroupId = ObjEarn.GroupId;
                        objCustomerDetail.EarnMinTxnAmt = ObjEarn.EarnMinTxnAmt;
                        objCustomerDetail.PointsExpiryMonths = ObjEarn.PointsExpiryMonths;
                        objCustomerDetail.PointsPercentage = ObjEarn.PointsPercentage;
                        objCustomerDetail.PointsAllocation = ObjEarn.PointsAllocation;
                    }

                    context.tblRuleMasters.AddOrUpdate(objCustomerDetail);

                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEarnRule");
            }
            return status;
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
        public bool BlockTransaction(string GroupId, string MobileNo, bool DisableSMSWATxn)
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
                newexception.AddException(ex, "BlockTransaction");
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
                    var Outlets = contextNew.tblOutletMasters.Where(x => x.GroupId == GroupId).ToList();

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

            lstOutlets = lstOutlets.OrderBy(x => x.Text).ToList();

            return lstOutlets;
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

        //public List<MemberData> GetTierList(string GroupId)
        //{
        //    List<MemberData> lstTiers = new List<MemberData>();
        //    List<tblCustDetailsMaster> lstTempMemberData = new List<tblCustDetailsMaster>();
        //    try
        //    {
        //        var connStr = CR.GetCustomerConnString((GroupId));
        //        using (var contextNew = new BOTSDBContext(connStr))
        //        {
        //            var MemberList = contextNew.Database.SqlQuery<tblCustDetailsMaster>("select * from tblCustDetailsMaster").ToList();

        //            foreach (var item in MemberList)
        //            {
        //                MemberData Obj = new MemberData();
        //                Obj.MobileNo = item.MobileNo;
        //                Obj.MemberName = item.Name;
        //                Obj.EnrolledOn = item.DOJ.Value.ToString("dd/MM/yyyy");
        //                Obj.Tier = item.Tier;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        newexception.AddException(ex, "GetMemberList");
        //    }

        //    lstTiers = lstTiers.OrderBy(x => x.Tier).ToList();

        //    return lstTiers;
        //}
    }

}
