using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DLC.ViewModel
{
    public class SessionVariables
    {
        public string CountryCode { get; set; }
        public string MobileNo { get; set; }        
        public string Source { get; set; }        
        public string GroupId { get; set; }
        public string BrandId { get; set; }
        public string Flag { get; set; }
        public string LoginURL { get; set; }
        public tblDLCDashboardConfig_Publish objDashboardConfig { get; set; }
    }
}