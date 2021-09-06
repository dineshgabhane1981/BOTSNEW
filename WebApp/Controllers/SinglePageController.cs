using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models.CommonDB;

namespace WebApp.Controllers
{
    public class SinglePageController : Controller
    {
        SinglePageRepository SPR = new SinglePageRepository();
        public ActionResult Index()
        {
            //  dynamic mymodel = new ExpandoObject();
            Tbl_SinglePageSummaryTable objsinglepagesummarytable = new Tbl_SinglePageSummaryTable();
            objsinglepagesummarytable = SPR.GetSinglePageSummaryTable();
            ViewBag.NonTransactingGroup = SPR.GetSinglePageNonTransactingGroups();
            ViewBag.NonTransactingoutlet = SPR.GetNonTransactingOutlet();
            ViewBag.lowTransactingoutlet = SPR.GetLowTransactingOutlet();
            ViewBag.communicationData = SPR.GetCommunicationWhatsAppExpiryData();
            return View(objsinglepagesummarytable);

            
        }
    }
}