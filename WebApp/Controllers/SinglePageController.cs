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
                singlevm.lstCSMembers = CR.GetRMAssigned();
                singlevm.lstsummarytable = SPR.GetSinglePageSummaryTable();
                singlevm.lstnontransactingGrp = SPR.GetSinglePageNonTransactingGroups();
                singlevm.lstnontransactingOutlet = SPR.GetNonTransactingOutlet("");
                singlevm.lstlowtransactingOutlet = SPR.GetLowTransactingOutlet("");

                singlevm.lstCitywiseData = SPR.GetCityWiseData();
                if (singlevm.lstCitywiseData != null)
                {
                    var categories = singlevm.lstCitywiseData.GroupBy(x => x.CategoryName).Select(y => y.First()).ToList();                    
                    singlevm.lstCategories = categories;

                    var cities = singlevm.lstCitywiseData.GroupBy(x => x.CityName).Select(y => y.First()).ToList();
                    List<CitywiseReport> objData = new List<CitywiseReport>();
                    foreach (var item in cities)
                    {
                        CitywiseReport objItem = new CitywiseReport();
                        long cityCount = singlevm.lstCitywiseData.Where(x => x.CityName == item.CityName).Sum(y => y.MemberBase);
                        objItem.CityName = item.CityName;
                        objItem.CategoryName = item.CategoryName;
                        objItem.MemberBase = cityCount;

                        objData.Add(objItem);
                    }

                    objData = objData.OrderByDescending(x => x.MemberBase).ToList();
                    cities = objData.GroupBy(x => x.CityName).Select(y => y.First()).ToList();

                    List<CitywiseReport> objCategoryData = new List<CitywiseReport>();
                    foreach (var category in categories)
                    {
                        CitywiseReport objItem = new CitywiseReport();
                        long categotyCount = singlevm.lstCitywiseData.Where(x => x.CategoryName == category.CategoryName).Sum(y => y.MemberBase);
                        objItem.CategoryName = category.CategoryName;
                        objItem.MemberBase = categotyCount;

                        objCategoryData.Add(objItem);
                    }

                    singlevm.lstCategoriesTotal = objCategoryData;
                    singlevm.GrandTotal = objCategoryData.Sum(x => x.MemberBase);
                    singlevm.lstCities = cities;
                }

                singlevm.lstCommunication = SPR.GetCommunicationWhatsAppExpiryData();                
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

        public ActionResult GetAllNonTransactingOutletData()
        {
            var lstData = SPR.GetNonTransactingOutlet("All");
            return PartialView("_NonTransactingAll", lstData);
        }
        public ActionResult GetAllLowTransactingOutletData()
        {
            var lstData = SPR.GetLowTransactingOutlet("All");
            return PartialView("_LowTransactingAll", lstData);
        }

        public JsonResult GetDiscussionGraphData(string CsMember)
        {
            List<object> lstData = new List<object>();
            var firstGraph = SPR.GetDiscussionDataForGraph("1", CsMember);
            var secondGraph = SPR.GetDiscussionDataForGraph("2", CsMember);
            var thirdGraph = SPR.GetDiscussionDataForGraph("3", CsMember);
            List<string> nameList = new List<string>();
            List<long> dataList1 = new List<long>();
            List<long> dataList2 = new List<long>();
            List<long> dataList3 = new List<long>();
            foreach (var item in firstGraph)
            {
                if (item.CustomerType == "A")
                    dataList1.Add(item.count);
                if (item.CustomerType == "B")
                    dataList2.Add(item.count);
                if (item.CustomerType == "C")
                    dataList3.Add(item.count);
            }
            foreach (var item in secondGraph)
            {
                if (item.CustomerType == "A")
                    dataList1.Add(item.count);
                if (item.CustomerType == "B")
                    dataList2.Add(item.count);
                if (item.CustomerType == "C")
                    dataList3.Add(item.count);
            }
            foreach (var item in thirdGraph)
            {
                if (item.CustomerType == "A")
                    dataList1.Add(item.count);
                if (item.CustomerType == "B")
                    dataList2.Add(item.count);
                if (item.CustomerType == "C")
                    dataList3.Add(item.count);
            }

            lstData.Add(dataList1);
            lstData.Add(dataList2);
            lstData.Add(dataList3);

            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }



    }
}