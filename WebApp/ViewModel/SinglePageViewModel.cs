using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;

namespace WebApp.ViewModel
{
    public class SinglePageViewModel
    {
        public List<SinglepageLowerMetrics> lstlowermetrics { get; set; }
        public Tbl_SinglePageSummaryTable lstsummarytable { get; set; }
        public List<Tbl_SinglePageNonTransactingGroup> lstnontransactingGrp { get; set; }
        public List<Tbl_SinglePageNonTransactingOutlet> lstnontransactingOutlet { get; set; }
        public List<Tbl_SinglePageLowTransactingOutlet> lstlowtransactingOutlet { get; set; }
        public List<CitywiseReport> lstCitywiseData { get; set; }
        public List<CitywiseReport> lstCities { get; set; }
        public List<CitywiseReport> lstCategories { get; set; }
        public List<CitywiseReport> lstCategoriesTotal { get; set; }
        public CommunicationsinglePageData lstCommunication { get; set; }
        public List<SelectListItem> lstCSMembers { get; set; }
        public long GrandTotal { get; set; }
        public string CSMember { get; set; }
        public List<GroupWiseDetails> lstGroupWiseDetails { get; set; }


    }
}