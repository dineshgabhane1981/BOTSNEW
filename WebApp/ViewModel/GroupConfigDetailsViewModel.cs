using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class GroupConfigDetailsViewModel
    {
        public tblGroupDetail objGroupDetails { get; set; }
        public List<BrandDetail> lstBrandDetails { get; set; }
        public GroupConfig objGroupConfig { get; set; }
        public List<OutletDetail> lstOutlets { get; set; }
        public List<PointsRulesEarnConfig> objEarnConfig { get; set; }
        public List<PointsRulesBurnConfig> objBurnConfig { get; set; }
        public List<SelectListItem> lstOutletList { get; set; }
        public SMSDetail objSMSDetails { get; set; }
        public int SMSDetailsCount { get; set; }
        public SMSConfig objSMSConfig { get; set; }
        public WAConfig objWAConfig { get; set; }
        public List<MWP_Details> lstMWPDetails { get; set; }
        public List<MWPSourceMaster> objMWPSourceMaster { get; set; }
        public List<tblUniquePoint> lstUniquePoints { get; set; }

    }
}