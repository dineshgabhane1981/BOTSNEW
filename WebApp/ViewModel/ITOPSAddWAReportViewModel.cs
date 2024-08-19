using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModel
{
    public class ITOPSAddWAReportViewModel
    {
        public WAReportDetailsViewModel objWADetails { get; set; }
        public List<ListWAGroupDetailsViewModel> LstWAAPIDetails { get; set; } = new List<ListWAGroupDetailsViewModel>();
    }
    public class WAReportDetailsViewModel
    {
        public string Groupid { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string WAGroupName { get; set; }
        public string Status { get; set; }
    }
    public class ListWAGroupDetailsViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}