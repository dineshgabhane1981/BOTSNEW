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
using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;
using System.IO;

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
                GroupWiseDetails objGrpWise = new GroupWiseDetails();
                //var count = SPR.GetAllTransactionData();
                singlevm.lstGroupWiseDetails = SPR.GetGroupWiseData();
                
                foreach (var item in singlevm.lstGroupWiseDetails)
                {
                    objGrpWise.CustCount = objGrpWise.CustCount + item.CustCount;
                    objGrpWise.BulkUploadCount = objGrpWise.BulkUploadCount + item.BulkUploadCount;
                    objGrpWise.Total = objGrpWise.Total + item.Total;
                }
                objGrpWise.CustName = "Total";
                singlevm.lstGroupWiseDetails.Add(objGrpWise);



                singlevm.lstCommunication = SPR.GetCommunicationWhatsAppExpiryData();
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
                        long categoryCount = singlevm.lstCitywiseData.Where(x => x.CategoryName == category.CategoryName).Sum(y => y.MemberBase);
                        objItem.CategoryName = category.CategoryName;
                        objItem.MemberBase = categoryCount;

                        objCategoryData.Add(objItem);
                    }

                    singlevm.lstCategoriesTotal = objCategoryData;
                    singlevm.GrandTotal = objCategoryData.Sum(x => x.MemberBase);
                    singlevm.lstCities = cities;
                }
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
            if(!string.IsNullOrEmpty(CsMember))
            {
                allData = allData.Where(x => x.RMLoginId == CsMember).ToList();
            }
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

            objData.lstData = typeData.Where(x => x.CustomerType == CustomerType).OrderByDescending(y=>y.days).ToList();
            if(!string.IsNullOrEmpty(CSMember))
            {
                objData.lstData = objData.lstData.Where(x => x.RMLoginId == CSMember).ToList();
            }
            objData.lstUniqueCSNames = objData.lstData.GroupBy(x => x.RMAssignedName).Select(y => y.First()).ToList();

            return PartialView("_CSWiseData", objData);
        }

        public ActionResult GetNoCustomerConnect(string CSWise)
        {
            var objData = SPR.GetNoCustomerConnect(CSWise);
            return PartialView("_NoCustomerConnect", objData);
        }
        public ActionResult GetMostConnectedCustomers(string CSWise)
        {
            var objData = SPR.GetMostConnectedCustomers(CSWise);
            return PartialView("_MostConnectedCustomers", objData);
        }
        public ActionResult GetLeastConnectedCustomers(string CSWise)
        {
            var objData = SPR.GetLeastConnectedCustomers(CSWise);
            return PartialView("_LeastConnectedCustomers", objData);
        }

        public ActionResult ExportGroupWise()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                List<GroupWiseDetails> lstGroupWiseDetails = new List<GroupWiseDetails>();
                lstGroupWiseDetails = SPR.GetGroupWiseData();

                GroupWiseDetails objGrpWise = new GroupWiseDetails();
                lstGroupWiseDetails = SPR.GetGroupWiseData();
                foreach (var item in lstGroupWiseDetails)
                {
                    objGrpWise.CustCount = objGrpWise.CustCount + item.CustCount;
                    objGrpWise.BulkUploadCount = objGrpWise.BulkUploadCount + item.BulkUploadCount;
                    objGrpWise.Total = objGrpWise.Total + item.Total;
                }
                objGrpWise.CustName = "Total";
                lstGroupWiseDetails.Add(objGrpWise);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(GroupWiseDetails));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (GroupWiseDetails item in lstGroupWiseDetails)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                var ReportName = "GroupWiseData";
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    table.TableName = ReportName;
                    wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);                        

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                    }
                }
                    
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult NonTransacting()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.CustomerName = CR.GetCustomerName(userDetails.GroupId);
                Session["UserSession"] = userDetails;
                singlevm.lstnontransactingGrp = SPR.GetSinglePageNonTransactingGroups();
                singlevm.lstnontransactingOutlet = SPR.GetNonTransactingOutlet("");
                singlevm.lstlowtransactingOutlet = SPR.GetLowTransactingOutlet("");
                singlevm.lstsummarytable = SPR.GetSinglePageSummaryTable();
                return View(singlevm);
            }

        }
        public ActionResult Communication()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            {
                singlevm.lstCommunication = SPR.GetCommunicationWhatsAppExpiryData();
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.CustomerName = CR.GetCustomerName(userDetails.GroupId);
                Session["UserSession"] = userDetails;

                return View(singlevm);
            }
        }
        public ActionResult KeyIndicator()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.CustomerName = CR.GetCustomerName(userDetails.GroupId);
                Session["UserSession"] = userDetails;
                return View();
            }
        }
        public ActionResult CityWise()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            {
                try
                {
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    userDetails.CustomerName = CR.GetCustomerName(userDetails.GroupId);
                    Session["UserSession"] = userDetails;
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
                            long categoryCount = singlevm.lstCitywiseData.Where(x => x.CategoryName == category.CategoryName).Sum(y => y.MemberBase);
                            objItem.CategoryName = category.CategoryName;
                            objItem.MemberBase = categoryCount;

                            objCategoryData.Add(objItem);
                        }

                        singlevm.lstCategoriesTotal = objCategoryData;
                        singlevm.GrandTotal = objCategoryData.Sum(x => x.MemberBase);
                        singlevm.lstCities = cities;
                    }


                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "Single Page");
                }


                return View(singlevm);
            }
        }

        public ActionResult Discussion()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.CustomerName = CR.GetCustomerName(userDetails.GroupId);
                Session["UserSession"] = userDetails;
                singlevm.lstCSMembers = CR.GetRMAssigned();
                return View(singlevm);
            }
        }
        public ActionResult GroupWise()
        {
            SinglePageViewModel singlevm = new SinglePageViewModel();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.CustomerName = CR.GetCustomerName(userDetails.GroupId);
                Session["UserSession"] = userDetails;
                GroupWiseDetails objGrpWise = new GroupWiseDetails();
                //var count = SPR.GetAllTransactionData();
                singlevm.lstGroupWiseDetails = SPR.GetGroupWiseData();

                foreach (var item in singlevm.lstGroupWiseDetails)
                {
                    objGrpWise.CustCount = objGrpWise.CustCount + item.CustCount;
                    objGrpWise.BulkUploadCount = objGrpWise.BulkUploadCount + item.BulkUploadCount;
                    objGrpWise.Total = objGrpWise.Total + item.Total;
                }
                objGrpWise.CustName = "Total";
                singlevm.lstGroupWiseDetails.Add(objGrpWise);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Single Page");
            }

            return View(singlevm);
        }
    }
}