using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class OtherReportProductwiseViewModel
    {
        public List<SellingProductValue> lstTop5Value { get; set; }
        public List<SellingProductValue> lstBottom5Value { get; set; }
        public List<SellingProductValue> lstTop5Volume { get; set; }
        public List<SellingProductValue> lstBottom5Volume { get; set; }
        public List<ReportForDownload> lstReportDownload { get; set; }
    }
}