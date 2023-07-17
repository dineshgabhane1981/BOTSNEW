using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.Reports;

namespace WebApp.ViewModel
{
    public class OtherReportProductwiseViewModel
    {
        public List<SellingProductValue> lstTop5Value { get; set; }
        public List<SellingProductValue> lstBottom5Value { get; set; }
        public List<SellingProductValue> lstTop5Volume { get; set; }
        public List<SellingProductValue> lstBottom5Volume { get; set; }
        public List<ReportForDownload> lstReportDownload { get; set; }
        public List<ProductWisePerformance> lstProductReport { get; set; }
        public List<ProductWisePerformance> lstProductReportTop15 { get; set; }
        public List<ProductWisePerformance> lstProductReportBtm15 { get; set; }
        public List<SelectListItem> lstOutletdetails { get; set; }
        public int[] SelectedProdIds { get; set; }
        public int[] SelectedOutletIds { get; set; }
        public IEnumerable<SelectListItem> ProductIds { get; set; }
        public List<SelectListItem> lstCategoryCode { get; set; }
        public List<SelectListItem> lstSubCategoryCode { get; set; }
        public List<ProductAnalytics> lstProdAnaltic { get; set; }
    }
}