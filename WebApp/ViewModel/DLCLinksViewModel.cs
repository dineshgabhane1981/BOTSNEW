using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class DLCLinksViewModel
    {
        public List<tblDLCCampaignMaster> lstLinks { get; set; }
        public List<SelectListItem> lstBrands { get; set; }
        public string dummyValue { get; set; }
    }
}