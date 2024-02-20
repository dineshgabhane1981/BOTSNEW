using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.IndividualDBModels;

namespace WebApp.ViewModel
{
    public class GroupConfigDetailsViewModel
    {
        public tblGroupDetail objGroupDetails { get; set; }
        public List<tblBrandMaster> lstBrandDetails { get; set; }
        public GroupConfig objGroupConfig { get; set; }
        public List<tblOutletMaster> lstOutlets { get; set; }
        public List<PointsRulesEarnConfig> objEarnConfig { get; set; }
        public List<PointsRulesBurnConfig> objBurnConfig { get; set; }
        public List<SelectListItem> lstOutletList { get; set; }
        public tblSMSWhatsAppCredential objSMSDetails { get; set; }
        public int SMSDetailsCount { get; set; }
        public SMSConfig objSMSConfig { get; set; }
        public WAConfig objWAConfig { get; set; }
        public List<tblDLCRuleMaster> lstDLCDetails { get; set; }
        
        public List<tblDLCSMSWAScriptMaster> objMWPSourceMaster { get; set; }
        public List<tblUniquePoint> lstUniquePoints { get; set; }
        public List<tblAnniversarySMSWAScript> lstAnniversarySMSWAScript { get; set; }
        public List<tblBirthdaySMSWAScript> lstBirthdaySMSWAScript { get; set; }
        public List<tblInActiveSMSWAScript> lstInActiveSMSWAScript { get; set; }
        public List<tblPointsExpirySMSWAScript> lstPointsExpirySMSWAScript { get; set; }
        public List<tblCelebrationRuleMaster> lstCelebrationRule { get; set; }

    }
}