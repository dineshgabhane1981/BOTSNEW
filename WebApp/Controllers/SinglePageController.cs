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

                //singlevm.lstCitywiseData = SPR.GetCityWiseData();
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
            List<long> dataList1 = new List<long>();
            List<long> dataList2 = new List<long>();
            List<long> dataList3 = new List<long>();
            var allData = SPR.GetDiscussionData();

            var first = allData.Where(x => x.days >= 7 && x.days <= 21).ToList();
            var firstA = first.Where(x => x.CustomerType == "A").Count();
            var firstB = first.Where(x => x.CustomerType == "B").Count();
            var firstC = first.Where(x => x.CustomerType == "C").Count();
            dataList1.Add(firstA);
            dataList1.Add(firstB);
            dataList1.Add(firstC);

            var second = allData.Where(x => x.days >= 22 && x.days <= 35).ToList();
            var secondA = second.Where(x => x.CustomerType == "A").Count();
            var secondB = second.Where(x => x.CustomerType == "B").Count();
            var secondC = second.Where(x => x.CustomerType == "C").Count();
            dataList2.Add(secondA);
            dataList2.Add(secondB);
            dataList2.Add(secondC);

            var third = allData.Where(x => x.days >= 36).ToList();
            var thirdA = third.Where(x => x.CustomerType == "A").Count();
            var thirdB = third.Where(x => x.CustomerType == "B").Count();
            var thirdC = third.Where(x => x.CustomerType == "C").Count();
            dataList3.Add(thirdA);
            dataList3.Add(thirdB);
            dataList3.Add(thirdC);

            lstData.Add(dataList1);
            lstData.Add(dataList2);
            lstData.Add(dataList3);

            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetCSWiseReport(string type, string CSMember, string CustomerType)
        {
            CSWiseDataViewModel objData = new CSWiseDataViewModel();
            List<DiscussionDataForGraph> typeData = new List<DiscussionDataForGraph>();

            var allData = SPR.GetDiscussionData();

            if (type.Equals("7 to 21 days"))
            {
                typeData = allData.Where(x => x.days >= 7 && x.days <= 21).ToList();
            }
            if (type.Equals("22 to 35 days"))
            {
                typeData = allData.Where(x => x.days >= 22 && x.days <= 35).ToList();
            }
            if (type.Equals("More than 36 days"))
            {
                typeData = allData.Where(x => x.days >= 36).ToList();
            }

            objData.lstData = typeData.Where(x => x.CustomerType == CustomerType).ToList();
            objData.lstUniqueCSNames = objData.lstData.GroupBy(x => x.RMAssignedName).Select(y => y.First()).ToList();

            return PartialView("_CSWiseData", objData);
        }

        public ActionResult GetNoCustomerConnect()
        {
            var objData = SPR.GetNoCustomerConnect();
            return PartialView("_NoCustomerConnect", objData);
        }

    }
}