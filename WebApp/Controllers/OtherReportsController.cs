using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using WebApp.ViewModel;
using BOTS_BL;
using System.Web.Script.Serialization;
using BOTS_BL.Models.Reports;
using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace WebApp.Controllers
{
    public class OtherReportsController : Controller
    {
        OtherReportsRepository ORR = new OtherReportsRepository();
        ReportsRepository RR = new ReportsRepository();
        Exceptions newexception = new Exceptions();
        // GET: OtherReports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Productwise()
        {
            OtherReportProductwiseViewModel objData = new OtherReportProductwiseViewModel();
            List<SellingProductValue> lstTop5SessingProductValue = new List<SellingProductValue>();
            List<SelectListItem> Obj = new List<SelectListItem>();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                //objData.lstTop5Value = ORR.GetTop5SellingProductValue(userDetails.GroupId, userDetails.connectionString);
                objData.lstTop5Value = new List<SellingProductValue>();
                //objData.lstBottom5Value = ORR.GetBottom5SellingProductValue(userDetails.GroupId, userDetails.connectionString);
                objData.lstBottom5Value = new List<SellingProductValue>();
                objData.lstOutletdetails = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
                objData.lstProductReport = ORR.GetProductWiseReport(userDetails.GroupId, userDetails.connectionString);
                objData.lstCategoryCode = ORR.GetCategoryCode(userDetails.GroupId, userDetails.connectionString);
                objData.ProductIds = ORR.GetProductId(userDetails.GroupId, userDetails.connectionString);
                objData.lstSubCategoryCode = ORR.GetSubCategoryCodeALL(userDetails.GroupId, userDetails.connectionString);
                //objData.ProductIds = new List<SelectListItem>();
                //objData.lstSubCategoryCode = new List<SelectListItem>(); 
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Productwise");
            }

            return View(objData);
        }
        public ActionResult Manufacturer()
        {
            return View();
        }
        public ActionResult ReportsDownload()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"]; 
            var lstReportDownload = ORR.GetReportDownloadData(userDetails.GroupId);
                    
            return View(lstReportDownload);
        }
        public ActionResult FranchiseeEnquiryReport()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<tblFranchiseeEnquiry> objData = new List<tblFranchiseeEnquiry>();
            try
            {
                objData = ORR.GetFranchiseeEnquiryList(userDetails.GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "FranchiseeEnquiryReport");
            }
            return View(objData);
        }
        public ActionResult ProductAnalytics()
        {
            OtherReportProductwiseViewModel objData = new OtherReportProductwiseViewModel();
            ProductWisePerformance objProdPerform = new ProductWisePerformance();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                objData.lstProductReportTop15 = ORR.GetTop15SellingProduct(userDetails.GroupId, userDetails.connectionString);
                objData.lstProductReportBtm15 = ORR.GetBtm15SellingProduct(userDetails.GroupId, userDetails.connectionString);

            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "ProductAnalytics");
            }
            return View(objData);
        }

        [HttpPost]
        public JsonResult ProductDataWithFilter(string Fromdte, string Todte, string OutletId,string stmtflag)
        {
            OtherReportProductwiseViewModel objData = new OtherReportProductwiseViewModel();
            
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                objData.lstTop5Value = new List<SellingProductValue>();
                objData.lstBottom5Value = new List<SellingProductValue>();

                objData.lstProductReport = ORR.GetProductDetailFilter(userDetails.GroupId, userDetails.connectionString, Fromdte, Todte, OutletId, stmtflag);            
                    
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "ProductDataWithFilter");
            }

            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult ProductAnalyticFilter(string jsonData)
        {
            OtherReportProductwiseViewModel objData1 = new OtherReportProductwiseViewModel();

            //List<ProductAnalytics> objProdAna = new List<ProductAnalytics>();

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    Int16 PurchaseFiter1 = Convert.ToInt16(item["PurchaseFiter1"]);
                    Int16 PurchaseFiter2 = Convert.ToInt16(item["PurchaseFiter2"]);
                    string dtFrom3 = Convert.ToString(item["dtFrom3"]);
                    string Todte3 = Convert.ToString(item["Todte3"]);
                    string CategoryCode1 = Convert.ToString(item["CategoryCode1"]);
                    string SubCategoryCode1 = Convert.ToString(item["SubCategoryCode1"]);
                    string LstProd1 = Convert.ToString(item["LstProd1"]);
                    string CategoryCode2 = Convert.ToString(item["CategoryCode2"]);
                    string SubCategoryCode2 = Convert.ToString(item["SubCategoryCode2"]);
                    string LstProd2 = Convert.ToString(item["LstProd2"]);
                    string NotPurchasedSince = Convert.ToString(item["NotPurchasedSince"]);
                    Int32 AmountSpentFrom = Convert.ToInt32(item["AmountSpentFrom"]);
                    Int32 AmountSpentTo = Convert.ToInt32(item["AmountSpentTo"]);
                    string LstOutlet = Convert.ToString(item["LstOutlet"]);
                    string LstProdCodeCount1 = Convert.ToString(item["LstProdCodeCount1"]);
                    string LstProdCodeCount2 = Convert.ToString(item["LstProdCodeCount2"]);
                    string LstOutletCount = Convert.ToString(item["LstOutletCount"]);

                    objData1.lstProdAnaltic = ORR.GetProductAnalyticFilter(userDetails.GroupId, userDetails.connectionString, PurchaseFiter1, PurchaseFiter2, dtFrom3, Todte3, CategoryCode1, SubCategoryCode1, LstProd1, CategoryCode2, SubCategoryCode2, LstProd2, NotPurchasedSince, AmountSpentFrom, AmountSpentTo, LstOutlet, LstProdCodeCount1, LstProdCodeCount2, LstOutletCount);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ProductAnalyticFilter");
            }


            //return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            return PartialView("_ProdAnalyticFilter", objData1);
        }

        [HttpPost]
        public JsonResult GetSubCategoryCode(string jsonData)
        {
            string CategoryCode;
            CategoryCode = string.Empty; 
            List<SelectListItem>  obj = new List<SelectListItem>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    if(string.IsNullOrEmpty(Convert.ToString(item["Categorycode"])) || Convert.ToString(item["Categorycode"]) == "All")
                    {
                        CategoryCode = "0000";
                    }
                    else
                    {
                        CategoryCode = Convert.ToString(item["Categorycode"]);
                    }
                    
                    obj = ORR.GetSubCategoryCode(userDetails.connectionString, CategoryCode);
                }
                    
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSubCategoryCode");
            }

            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetProductCode(string jsonData)
        {
            string SubCategorycode;
            SubCategorycode = string.Empty;
            List<SelectListItem> obj = new List<SelectListItem>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(item["SubCategorycode"])) || Convert.ToString(item["SubCategorycode"]) == "All")
                    {
                        SubCategorycode = "0000";
                    }
                    else
                    {
                        SubCategorycode = Convert.ToString(item["SubCategorycode"]);
                    }
                    
                    obj = ORR.GetProductCode(userDetails.connectionString, SubCategorycode);
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProductCode");
            }

            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportToExcelProductwise(string Fromdte, string Todte, string OutletId, string stmtflag)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            OtherReportProductwiseViewModel objData = new OtherReportProductwiseViewModel();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                List<tblCustDetailsMaster> lstMember = new List<tblCustDetailsMaster>();
                objData.lstProductReport = ORR.GetProductDetailFilter(userDetails.GroupId, userDetails.connectionString, Fromdte, Todte, OutletId, stmtflag);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ProductWisePerformance));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (ProductWisePerformance item in objData.lstProductReport)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }

                //table.Columns.Remove("Id");
                //table.Columns.Remove("DOB");
                //table.Columns.Remove("Email");
                //table.Columns.Remove("AnniversaryDate");
                //table.Columns.Remove("Category");
                //table.Columns.Remove("CardNo");
                //table.Columns.Remove("Gender");
                //table.Columns.Remove("EnrolledBy");
                //table.Columns.Remove("CountryCode");
                //table.Columns.Remove("CurrentEnrolledOutlet");
                //table.Columns.Remove("DisableSMSWATxn");
                //table.Columns.Remove("EnrolledOutlet");
                //table.Columns.Remove("DOJ");
                //table.Columns.Remove("IsActive");
                //table.Columns.Remove("DisableTxn");
                //table.Columns.Remove("DisableSMSWAPromo");
                string ReportName = "ProductPerformanceData";
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    table.TableName = ReportName;

                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Product Performance Data";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Filter";

                    worksheet.Cell(5, 1).InsertTable(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }



            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "ExportDataExcel");
                return null;

            }

        }

        public ActionResult ExportToExcelProductAnalytics(Int16 PurchaseFiter1, Int16 PurchaseFiter2, string dtFrom3, string Todte3, string CategoryCode1, string SubCategoryCode1, string LstProd1, string CategoryCode2, string SubCategoryCode2, string LstProd2, string NotPurchasedSince, Int32 AmountSpentFrom, Int32 AmountSpentTo, string LstOutlet, string LstProdCodeCount1, string LstProdCodeCount2, string LstOutletCount)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            OtherReportProductwiseViewModel objData = new OtherReportProductwiseViewModel();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                //List<tblCustDetailsMaster> lstMember = new List<tblCustDetailsMaster>();
                objData.lstProdAnaltic = ORR.GetProductAnalyticFilter(userDetails.GroupId, userDetails.connectionString, PurchaseFiter1, PurchaseFiter2, dtFrom3, Todte3, CategoryCode1, SubCategoryCode1, LstProd1, CategoryCode2, SubCategoryCode2, LstProd2, NotPurchasedSince, AmountSpentFrom, AmountSpentTo, LstOutlet, LstProdCodeCount1, LstProdCodeCount2, LstOutletCount);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ProductAnalytics));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (ProductAnalytics item in objData.lstProdAnaltic)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }

                //table.Columns.Remove("Id");
                //table.Columns.Remove("DOB");
                //table.Columns.Remove("Email");
                //table.Columns.Remove("AnniversaryDate");
                //table.Columns.Remove("Category");
                //table.Columns.Remove("CardNo");
                //table.Columns.Remove("Gender");
                //table.Columns.Remove("EnrolledBy");
                //table.Columns.Remove("CountryCode");
                //table.Columns.Remove("CurrentEnrolledOutlet");
                //table.Columns.Remove("DisableSMSWATxn");
                //table.Columns.Remove("EnrolledOutlet");
                //table.Columns.Remove("DOJ");
                //table.Columns.Remove("IsActive");
                //table.Columns.Remove("DisableTxn");
                //table.Columns.Remove("DisableSMSWAPromo");
                string ReportName = "ProductAnalyticsData";
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    table.TableName = ReportName;

                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Product Analytics Data";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Filter";

                    worksheet.Cell(5, 1).InsertTable(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }



            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelProductAnalytics");
                return null;

            }
        }

    }
}