using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class CSWiseDataViewModel
    {
        public List<DiscussionDataForGraph> lstData { get; set; }
        public List<DiscussionDataForGraph> lstUniqueCSNames { get; set; }
    }
}