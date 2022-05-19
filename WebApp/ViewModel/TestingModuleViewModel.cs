using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.OnBoarding;

namespace WebApp.ViewModel
{
    public class TestingModuleViewModel
    {
        public BOTS_TblGroupMaster bots_TblGroupMaster { get; set; }
        public BOTS_TblDealDetails bots_TblDealDetails { get; set; }
        public BOTS_TblPaymentDetails bots_TblPaymentDetails { get; set; }
        public List<BOTS_TblRetailMaster> objRetailList { get; set; }
        public List<BOTS_TblInstallmentDetails> objInstallmentList { get; set; }
        public OnBoardingCustomerDetails onBoardingCustomerDetails { get; set; }
        public BOTS_TblCommunicationSet bOTS_TblCommunicationSet { get; set; }
        public BOTS_TblDLCLinkConfig objDLCLinkConfig { get; set; }
        public BOTS_TblCampaignOtherConfig bOTS_TblCampaignOtherConfig { get; set; }
        
        public BOTS_TblSMSConfig bOTS_TblSMSConfig { get; set; }
        public BOTS_TblCampaignInactive bOTS_TblCampaignInactive { get; set; }

    }
}