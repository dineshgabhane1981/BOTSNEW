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

        public tblSMSWhatsAppScriptMaster GetWAScripts(int GroupId, string OutletId, string MessageType)
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
                    else if (MessageType == "CancelEarn")
                    {
                        Id = "103";
                    }
                    else if (MessageType == "CancelBurn")
                    {
                        Id = "104";
                    }
                    else if (MessageType == "OTP")
                    {
                        Id = "105";
                    }
                    else if (MessageType == "Balance>0")
                    {
                        Id = "106";
                    }
                    else if (MessageType == "Balance<0")
                    {
                        Id = "107";
                    }
                    objSMSWhatsAppScriptMaster = context.tblSMSWhatsAppScriptMasters.Where(x => x.Id == Id).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetWAScripts");
            }
            return objSMSWhatsAppScriptMaster;

        }

        public bool SaveScripts(int GroupId, int OutletId, string Script, string MessageType)
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
                    else if (MessageType == "CancelEarn")
                    {
                        Id = "103";
                    }
                    else if (MessageType == "CancelBurn")
                    {
                        Id = "104";
                    }
                    else if (MessageType == "OTP")
                    {
                        Id = "105";
                    }
                    else if (MessageType == "Balance>0")
                    {
                        Id = "106";
                    }
                    else if (MessageType == "Balance<0")
                    {
                        Id = "107";
                    }
                    var Script1 = context.tblSMSWhatsAppScriptMasters.Where(x => x.Id == Id).FirstOrDefault();
                    Script1.WhatsAppScript = Script;
                    context.tblSMSWhatsAppScriptMasters.AddOrUpdate(Script1);
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
                    objtblRuleMaster = contextNew.tblRuleMasters.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
                if (objtblRuleMaster != null)
                {
                    obj.EarnMinTxnAmt = objtblRuleMaster.EarnMinTxnAmt;
                    obj.PointsExpiryMonths = objtblRuleMaster.PointsExpiryMonths;
                    obj.PointsAllocation = objtblRuleMaster.PointsAllocation;
                    obj.PointsPercentage = objtblRuleMaster.PointsPercentage;
                    obj.Revolving = Convert.ToBoolean(objtblRuleMaster.Revolving);

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
                    
                    if (ObjEarn.GroupId == "1051" || ObjEarn.GroupId == "1002" )
                    {
                        // Takes FineFoods Testing
                       _WAGroupCode = con.WAReports.Where(x => x.GroupId == "1051" && x.SMSStatus == "5").Select(y=> y.GroupCode).FirstOrDefault();
                        RMId = (int)con.tblGroupDetails.Where(x => x.GroupId == IntGroupId).Select(y => y.RMAssigned).FirstOrDefault();
                        CSName = con.tblRMAssigneds.Where(x=>x.RMAssignedId == RMId).Select(y=>y.RMAssignedName).FirstOrDefault();
                    }
                    else
                    {
                        _WAGroupCode = con.WAReports.Where(x => x.GroupId == ObjEarn.GroupId && x.SMSStatus == "0").Select(y => y.GroupCode).FirstOrDefault();
                    }
                    
                }
                using (var context = new BOTSDBContext(connectionstring))
                {
                    objCustomerDetail = context.tblRuleMasters.Where(x => x.GroupId == ObjEarn.GroupId).FirstOrDefault();
                    if (objCustomerDetail != null)
                    {
                        objCustomerDetail.GroupId = ObjEarn.GroupId;
                        objCustomerDetail.EarnMinTxnAmt = ObjEarn.EarnMinTxnAmt;
                        objCustomerDetail.PointsExpiryMonths = ObjEarn.PointsExpiryMonths;
                        objCustomerDetail.PointsPercentage = ObjEarn.PointsPercentage;
                        objCustomerDetail.PointsAllocation = ObjEarn.PointsAllocation;
                        objCustomerDetail.Revolving = ObjEarn.Revolving;
                    }

                    context.tblRuleMasters.AddOrUpdate(objCustomerDetail);
                    context.SaveChanges();
                    status = true;

                    
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
                    //ObjCSMessage.Message = ConfigurationManager.AppSettings["ITCSMessage"].ToString();


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
                        pointExpiryData.EndDate = Convert.ToDateTime(expiryDate);
                        context.tblCustPointsMasters.AddOrUpdate(pointExpiryData);
                        context.SaveChanges();
                        result = true;
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
        public List<tblCustDetailsMaster> GetSlabWiseReport(string GroupId, string Tier)
        {
            List<tblCustDetailsMaster> lstMember = new List<tblCustDetailsMaster>();
            try
            {
                var connStr = CR.GetCustomerConnString((GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    lstMember = contextNew.tblCustDetailsMasters.Where(x => x.Tier == Tier).ToList();

                    foreach (var item in lstMember)
                    {
                        MemberData Obj = new MemberData();
                        Obj.MobileNo = item.MobileNo;
                        Obj.MemberName = item.Name;
                        Obj.Tier = item.Tier;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTierList");
            }

            return lstMember;
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
                    objDemoData.DOB = Convert.ToString(objCustomerDetail.DOB);
                    objDemoData.DOA = Convert.ToString(objCustomerDetail.DOA);
                    objDemoData.Gender = objCustomerDetail.Gender;
                    objDemoData.Name = objCustomerDetail.Name;
                }
                if (objOutletMaster != null)
                {
                    objDemoData.StoreAnniversary = Convert.ToString(objOutletMaster.StoreAnniversaryDate);
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
                stb.AppendLine("As discussed the Loyalty Earn Rule have been changed :");
                stb.AppendLine();
                stb.AppendLine("*Old Rule*");
                stb.AppendLine("--------------");
                stb.AppendLine("EarnMinTxnAmt : "+ ObjCSMessage.OldEarnMinTxnAmt);
                stb.AppendLine("PointsAllocation : " + ObjCSMessage.OldPointsAllocation);
                stb.AppendLine("PointsExpiryMonths : " + ObjCSMessage.OldPointsExpiryMonths);
                stb.AppendLine("PointsPercentage : " + ObjCSMessage.OldPointsExpiryMonths);
                stb.AppendLine("PointsRevolving : " + ObjCSMessage.OldRevolvingStatus);
                stb.AppendLine();
                stb.AppendLine("*New Rule*");
                stb.AppendLine("--------------");
                stb.AppendLine("EarnMinTxnAmt : " + ObjCSMessage.EarnMinTxnAmt);
                stb.AppendLine("PointsAllocation : " + ObjCSMessage.PointsAllocation);
                stb.AppendLine("PointsExpiryMonths : " + ObjCSMessage.PointsExpiryMonths);
                stb.AppendLine("PointsPercentage : " + ObjCSMessage.PointsPercentage);
                stb.AppendLine("PointsRevolving : " + ObjCSMessage.Revolving);
                stb.AppendLine();
                stb.AppendLine("Regards,");
                stb.AppendLine(" - " + ObjCSMessage.CSName);
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
                stb.AppendLine(" - " + ObjCSMessage.CSName);
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
    }
}
