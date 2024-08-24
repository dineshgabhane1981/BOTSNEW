using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModel
{
    public class ITOPSAddWAReportViewModel
    {
        public WAReportDetailsViewModel objWADetails { get; set; } = new WAReportDetailsViewModel();
        public List<ListWAGroupDetailsViewModel> LstWAAPIDetails { get; set; } = new List<ListWAGroupDetailsViewModel>();
    }
    public class WAReportDetailsViewModel
    {
        public string Groupid { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public string WAGroupName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    public class ListWAGroupDetailsViewModel
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
    }
}