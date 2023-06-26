using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.ViewModel
{
    public class DashboardViewModel
    {
        public tblDLCDashboardConfig_Publish objDashboardConfig { get; set; }
        public DLCDashboardContent dLCDashboardContent { get; set; }
    }
}