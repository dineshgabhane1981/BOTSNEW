﻿using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.ViewModel
{
    public class DLCDashboardFrontData
    {
        public List<tblDLCFrontEndPageData> lstDLCFrontEndPageData { get; set; }
        public tblDLCDashboardConfig_Publish objDashboardConfig { get; set; }
        public string CountryCode { get; set; }
    }
}