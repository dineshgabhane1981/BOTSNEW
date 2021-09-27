using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models.CommonDB;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class SinglePageController : Controller
    {
        SinglePageRepository SPR = new SinglePageRepository();
        public ActionResult Index()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            //  dynamic mymodel = new ExpandoObject();
            // Tbl_SinglePageSummaryTable objsinglepagesummarytable = new Tbl_SinglePageSummaryTable();
            singlevm.lstsummarytable = SPR.GetSinglePageSummaryTable();
            singlevm.lstnontransactingGrp = SPR.GetSinglePageNonTransactingGroups();
            singlevm.lstnontransactingOutlet = SPR.GetNonTransactingOutlet();
            singlevm.lstlowtransactingOutlet = SPR.GetLowTransactingOutlet();
            singlevm.lstCommunication = SPR.GetCommunicationWhatsAppExpiryData();
            //singlevm.lstlowermetrics = 

            return View(singlevm);

            
        }
        [HttpPost]
        public JsonResult GetLowerMetricsData(string Id)
        {

            List<SinglepageLowerMetrics> lstsinglepage = new List<SinglepageLowerMetrics>();
            //// SinglePageViewModel singlevm = new SinglePageViewModel();
            lstsinglepage = SPR.GetLowerMetrics(Id);
            //return PartialView("_LowerMetrics", lstsinglepage);
            return new JsonResult() { Data = lstsinglepage, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };


        }


    }
}