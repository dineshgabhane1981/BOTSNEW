﻿using System;
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
        public String billingPartnerId { get; set; }
        public String outletId { get; set; }

        public string RequestPacketEnrollnment { get; set; }        
        public string RequestPacketEnrollnmentWithEarn { get; set; } 
        public string RequestPacketEarn { get; set; }
        public string RequestPacketBurnValidation { get; set; }
        public string RequestPacketBurn { get; set; }
        public string RequestPacketCancel { get; set; }
        public string RequestPacketSendOTP { get; set; }
        public string RequestURL { get; set; }

        public BOTS_TblEarnRuleConfig objEarnRuleConfig { get; set; }
        public BOTS_TblBurnRuleConfig objBurnRuleConfig { get; set; }
        public List<BOTS_TblSlabConfig> lstSlabConfig { get; set; }
        public List<BOTS_TblProductUpload> lstProductUpload { get; set; }

        public List<SelectListItem> billingpartners { get; set; }
        public List<SelectListItem> lstOutlets { get; set; }
        public List<SelectListItem> BPProduct { get; set; }
        
    }
}