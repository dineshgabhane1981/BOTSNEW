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

        public string RequestPacket { get; set; }
        public string RequestURL { get; set; }
    }
}