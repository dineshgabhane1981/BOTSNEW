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
        public int billingPartnerId { get; set; }

        public string RequestPacketEnrollnment { get; set; }        
        public string RequestPacketEnrollnmentWithEarn { get; set; } 
        public string RequestPacketEarn { get; set; }
        public string RequestPacketBurnValidation { get; set; }
        public string RequestPacketBurn { get; set; }
        public string RequestPacketCancel { get; set; }
        public string RequestPacketSendOTP { get; set; }
        public string RequestURL { get; set; }
    }
}