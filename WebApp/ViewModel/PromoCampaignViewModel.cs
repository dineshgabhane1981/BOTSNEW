using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class PromoCampaignViewModel
    {
        public List<tblGroupDetail> lstGroup { get; set; }
        public List<SelectListItem> lstGroupList { get; set; }
    }
}