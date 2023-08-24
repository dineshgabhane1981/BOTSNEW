using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class ProgrammeViewModel
    {
        public List<tblGroupDetail> lstActive { get; set; }

        public List<tblGroupDetail> lstNotActive { get; set; }

        public List<SelectListItem> lstGroupDetails { get; set; }
        public List<SelectListItem> lstRMAssigned { get; set; }
        public List<SelectListItem> lstOutletDetails { get; set; }
        public List<tblCampaignMaster> lstCampaigns { get; set; }
        public int campaignId { get; set; }
        public tblGroupDetail tblGroupDetails { get; set; }
        public tblRMAssigned tblRMAssigned { get; set; }
        public tblOutletMaster tblOutletMaster { get; set; }
        public string Message { get; set; }
        public List<SelectListItem> lstOutletMaster { get; set; }

        public SelectListItem[] MessageType()
        {
            return new SelectListItem[8] { new SelectListItem() { Text = "Enrollment", Value = "Enrollment" }, new SelectListItem() { Text = "Earn", Value = "Earn" }, new SelectListItem() { Text = "Burn", Value = "Burn" }, new SelectListItem() { Text = "CancelEarn", Value = "CancelEarn" }, new SelectListItem() { Text = "CancelBurn", Value = "CancelBurn" }, new SelectListItem() { Text = "OTP", Value = "OTP" }, new SelectListItem() { Text = "Balance>0", Value = "Balance>0" }, new SelectListItem() { Text = "Balance<0", Value = "Balance<0" } };
        }
        public tblSMSWhatsAppScriptMaster  objSMSWhatsAppScriptMaster { get; set; }
        public GroupDetails objGroupDetail { get; set; }
        public string Script { get; set; }
        public tblGroupOwnerInfo objtblGroupOwnerInfo { get; set; }
        public DemographicData objDemographicData { get; set; }
        public tblRuleMaster objtblRuleMaster { get; set; }
        public Earndata objEarndata { get; set; }
        public string TxnAmt { get; set; }
        public string RevolvingExpiry { get; set; }
        public string Percentage { get; set; }
        public string PointsInRs { get; set; }
        public bool Revolving { get; set; }
        public BurnData objBurnData { get; set; }
        public OTPData objOTPData { get; set; }
        public List<tblCustDetailsMaster> lstMember { get; set; }
        public List<SelectListItem> lstTierDetails { get; set; }
        public tblCustDetailsMaster tblCustDetailsMaster { get; set; }
        public MemberData objMemberData { get; set; }
    }
}