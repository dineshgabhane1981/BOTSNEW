using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models.CommonDB;
using WebApp.ViewModel;
using BOTS_BL.Models;
using BOTS_BL;

namespace WebApp.Controllers
{
    public class SinglePageController : Controller
    {
        SinglePageRepository SPR = new SinglePageRepository();
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public ActionResult Index()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.CustomerName = CR.GetCustomerName(userDetails.GroupId);
                Session["UserSession"] = userDetails;
                
                // Tbl_SinglePageSummaryTable objsinglepagesummarytable = new Tbl_SinglePageSummaryTable();
                singlevm.lstsummarytable = SPR.GetSinglePageSummaryTable();
                singlevm.lstnontransactingGrp = SPR.GetSinglePageNonTransactingGroups();
                singlevm.lstnontransactingOutlet = SPR.GetNonTransactingOutlet();
                singlevm.lstlowtransactingOutlet = SPR.GetLowTransactingOutlet();

                singlevm.lstCitywiseData = SPR.GetCityWiseData();
                var cities = singlevm.lstCitywiseData.GroupBy(x => x.CityName).Select(y => y.First()).ToList();
                singlevm.lstCities = cities;
                var categories = singlevm.lstCitywiseData.GroupBy(x => x.CategoryName).Select(y => y.First()).ToList();
                singlevm.lstCategories = categories;

                //singlevm.lstCommunication = SPR.GetCommunicationWhatsAppExpiryData();
                //singlevm.lstlowermetrics = 
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Single Page");
            }
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