using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.OnBoarding;

namespace WebApp.ViewModel
{
    public class OnBoardingCheckerViewModel
    {
        public BOTS_TblGroupMaster bots_TblGroupMaster { get; set; }
        public BOTS_TblRetailMaster bots_TblRetailMaster { get; set; }
        public BOTS_TblDealDetails bots_TblDealDetails { get; set; }
        public BOTS_TblPaymentDetails bots_TblPaymentDetails { get; set; }
        public BOTS_TblInstallmentDetails bots_TblInstallmentDetails { get; set; }
        public BOTS_TblOutletMaster bots_TblOutletMaster { get; set; }
        public List<BOTS_TblRetailMaster> objRetailList { get; set; }
        public List<BOTS_TblInstallmentDetails> objInstallmentList { get; set; }
        public List<BOTS_TblOutletMaster> lstOutlets { get; set; }
        public List<BOTS_TblSMSConfig> lstSMSConfig { get; set; }
        public bool IsBrand { get; set; }

        public List<BOTS_TblCommunicationSet> lstCommunicationSet { get; set; }
        public List<BOTS_TblCommunicationSetAssignment> lstCommunicationSetAssignment { get; set; }
        public BOTS_TblDLCLinkConfig objDLCLinkConfig { get; set; }
    }
}