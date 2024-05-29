using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;
using ClosedXML.Excel;
using System.Data;
using WebApp.ViewModel;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ComponentModel;
using System.IO;

namespace WebApp.Controllers
{
    public class CouponController : Controller
    {
        Exceptions newexception = new Exceptions();
        CouponRepository CR = new CouponRepository();
        ReportsRepository RR = new ReportsRepository();
        OtherReportsRepository ORR = new OtherReportsRepository();
        // GET: Coupon
        public  ActionResult  Index()
        {            
            CommonFunctions Common = new CommonFunctions();
            string cCode = Common.GenerateCoupon();
            UploadCouponViewModel objData = new UploadCouponViewModel();
            
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData.lstCouponUpload = CR.GetAllCouponUpload(userDetails.connectionString);
            objData.lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            objData.lstCategory = ORR.GetCategoryCode(userDetails.GroupId, userDetails.connectionString);
            objData.lstProduct = ORR.GetProductId(userDetails.GroupId, userDetails.connectionString);
            return View(objData);
        }
        public JsonResult UploadCouponBulkData(HttpPostedFileBase file, string ExpiryDate, string Reminder, string CouponValue, string InvoiceValueFrom, string InvoiceValueTo, string Day, string Category, string Product, string Outlet,string OfferCode, string IsOnlyCoupon)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            DataTable dt = new DataTable();
            tblCouponUpload objUpload = new tblCouponUpload();
            try
            {
                objUpload.CouponFileName = file.FileName + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;
                if (!string.IsNullOrEmpty(Reminder))
                {
                    objUpload.IsReminder = true;
                    objUpload.ReminderDate = Convert.ToDateTime(ExpiryDate).AddDays(-Convert.ToInt32(Reminder));
                }
                objUpload.ExpiryDate = Convert.ToDateTime(ExpiryDate);
                objUpload.UploadedDate = DateTime.Now;
                objUpload.UploadedBy = userDetails.LoginId;
                objUpload.CouponValue = Convert.ToDecimal(CouponValue);

                using (XLWorkbook workBook = new XLWorkbook(file.InputStream))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);
                    dt.Columns.Add("MobileNo", typeof(string));
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        int i = 0;
                        DataRow toInsert = dt.NewRow();
                        foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                        {
                            try
                            {
                                toInsert[i] = cell.Value.ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                            i++;
                        }
                        dt.Rows.Add(toInsert);
                    }
                }
                List<tblCouponMapping> lstData = new List<tblCouponMapping>();

                int index = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (index > 0)
                    {
                        CommonFunctions Common = new CommonFunctions();
                        tblCouponMapping objData = new tblCouponMapping();
                        objData.MobileNo = Convert.ToString(dr["MobileNo"]);
                        objData.CouponCode = Common.GenerateCoupon();
                        objData.CouponValue = Convert.ToDecimal(CouponValue);
                        objData.ExpiryDate = Convert.ToDateTime(ExpiryDate);
                        objData.CreatedDate = DateTime.Now;
                        objData.CreatedBy = userDetails.LoginId;
                        if (!string.IsNullOrEmpty(Reminder))
                            objData.ReminderDate = Convert.ToDateTime(ExpiryDate).AddDays(-Convert.ToInt32(Reminder));
                        if (!string.IsNullOrEmpty(InvoiceValueFrom))
                            objData.RedeemInvoiceAmountFrom = Convert.ToDecimal(InvoiceValueFrom);
                        if (!string.IsNullOrEmpty(InvoiceValueTo))
                            objData.RedeemInvoiceAmountTo = Convert.ToDecimal(InvoiceValueTo);
                        if (!string.IsNullOrEmpty(Day))
                            objData.RedeemDay = Day;
                        if (!string.IsNullOrEmpty(Category))
                            objData.RedeemCategory = Category;
                        if (!string.IsNullOrEmpty(Product))
                            objData.RedeemProduct = Product;
                        if (!string.IsNullOrEmpty(Outlet))
                            objData.RedeemOutlet = Outlet;

                        objData.EarnRuleId = objUpload.CouponFileName;
                        objData.AllowPointAccrual = Convert.ToBoolean(IsOnlyCoupon);
                        objData.OfferCode = OfferCode;                        

                        lstData.Add(objData);
                    }
                    index++;
                }
                objUpload.Count = lstData.Count();
                result = CR.UploadCouponData(lstData, objUpload, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UploadCouponBulkData");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductData(string CatCode)
        {
            if (CatCode == "")
            {
                CatCode = "0000";
            }
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var ProdData = ORR.GetProductCodeByCategory(userDetails.connectionString, CatCode);

            return new JsonResult() { Data = ProdData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult Report()
        {
            return View();
        }
        public ActionResult Configure()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            UploadCouponViewModel objData = new UploadCouponViewModel();
            objData.lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            objData.lstCategory = ORR.GetCategoryCode(userDetails.GroupId, userDetails.connectionString);
            objData.lstProduct = ORR.GetProductId(userDetails.GroupId, userDetails.connectionString);
            objData.lstCouponRule = CR.GetAllCouponRule(userDetails.connectionString);
            objData.lstCouponRule = objData.lstCouponRule.Where(x => x.CouponCode == "").ToList();
            return View(objData);
        }
        public ActionResult GenericCoupon()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            UploadCouponViewModel objData = new UploadCouponViewModel();
            objData.lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            objData.lstCategory = ORR.GetCategoryCode(userDetails.GroupId, userDetails.connectionString);
            objData.lstProduct = ORR.GetProductId(userDetails.GroupId, userDetails.connectionString);
            objData.lstCouponRule = CR.GetAllCouponRule(userDetails.connectionString);
            objData.lstCouponRule = objData.lstCouponRule.Where(x => x.CouponCode != "").ToList();
            return View(objData);
        }

        public JsonResult SaveCouponRule(string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            tblCouponRule objRule = new tblCouponRule();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objRule.EarnRuleName = Convert.ToString(item["EarnRuleName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnInvoiceAmountFrom"])))
                        objRule.EarnInvoiceAmountFrom = Convert.ToDecimal(item["EarnInvoiceAmountFrom"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnInvoiceAmountTo"])))
                        objRule.EarnInvoiceAmountTo = Convert.ToDecimal(item["EarnInvoiceAmountTo"]);
                    objRule.EarnDay = Convert.ToString(item["EarnDay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnCategory"])) && Convert.ToString(item["EarnCategory"]) != "All Category")
                        objRule.EarnCategory = ConvertToCommaSeperated(item["EarnCategory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnProduct"])) && Convert.ToString(item["EarnProduct"]) != "All Product")
                        objRule.EarnProduct = ConvertToCommaSeperated(item["EarnProduct"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnOutlet"])))
                        objRule.EarnOutlet = ConvertToCommaSeperated(item["EarnOutlet"]); 

                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemInvoiceAmountFrom"])))
                        objRule.RedeemInvoiceAmountFrom = Convert.ToDecimal(item["RedeemInvoiceAmountFrom"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemInvoiceAmountTo"])))
                        objRule.RedeemInvoiceAmountTo = Convert.ToDecimal(item["RedeemInvoiceAmountTo"]);
                    objRule.RedeemDay = Convert.ToString(item["RedeemDay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemCategory"])) && Convert.ToString(item["RedeemCategory"]) != "All Category")
                        objRule.RedeemCategory = ConvertToCommaSeperated(item["RedeemCategory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemProduct"])) && Convert.ToString(item["RedeemProduct"]) != "All Product")
                        objRule.RedeemProduct = ConvertToCommaSeperated(item["RedeemProduct"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemOutlet"])))
                        objRule.RedeemOutlet = ConvertToCommaSeperated(item["RedeemOutlet"]);
                    objRule.IsActive = true;
                    objRule.AddedDate = DateTime.Now;
                    objRule.AddedBy = userDetails.LoginId;
                    objRule.IsOnlyCoupon = Convert.ToBoolean(item["IsOnlyCoupon"]);
                    objRule.UsageTime = Convert.ToString(item["MultipleUse"]);

                    objRule.CouponValue = Convert.ToInt32(item["CouponValue"]);
                    objRule.ValueType = Convert.ToString(item["ValueType"]);
                    objRule.ExpiryDays = Convert.ToInt32(item["ExpiryDays"]);
                    objRule.OfferCode = Convert.ToString(item["OfferCode"]);

                    objRule.EarnBrand = Convert.ToString(item["EarnBrand"]);
                    objRule.EarnDept = Convert.ToString(item["EarnDept"]);
                    objRule.BurnBrand = Convert.ToString(item["BurnBrand"]);
                    objRule.BurnDept = Convert.ToString(item["BurnDept"]);
                    objRule.CouponCode = Convert.ToString(item["CouponCode"]);
                }
                result = CR.SaveCouponEarnRule(objRule, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetRedeemReport(string fileName)
        {
            List<tblCouponMapping> objData = new List<tblCouponMapping>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CR.GetRedeemDetailedReport(fileName, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRedeemReport");
            }
            return PartialView("_RedeemCouponReport", objData);
        }

        public ActionResult CouponReport()
        {
            CouponReportViewModel objData = new CouponReportViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletList = lstOutlet;
            return View(objData);
        }
    
        public JsonResult GetCouponReport(string fromDate,string toDate,string earnOutletId,string burnOutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var data = CR.GetCouponReport(fromDate, toDate, earnOutletId, burnOutletId, userDetails.GroupId);
            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportToExcelCouponReport(string fromDate, string toDate, string earnOutletId, string burnOutletId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                List<CouponReportModel> lstData = new List<CouponReportModel>();
                lstData = CR.GetCouponReport(fromDate, toDate, earnOutletId, burnOutletId, userDetails.GroupId);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(CouponReportModel));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (CouponReportModel item in lstData)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                string ReportName = "CouponReport";
                string fileName = "BOTS_CouponReport.xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    //worksheet.Cell(1, 1).Value = "Report Name";
                    //worksheet.Cell(1, 2).Value = "Transactionwise";
                    //worksheet.Cell(2, 1).Value = "Date";
                    //worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    //worksheet.Cell(3, 1).Value = "Period";
                    //worksheet.Cell(3, 2).Value = fromDate + "-" + toDate;
                    worksheet.Cell(1, 1).InsertTable(table);
                    //wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);                        
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelTransactionwise");
                return null;
            }


        }

        public JsonResult EnableDisableCouponRule(string RuleId)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];            
            try
            {
                result = CR.EnableDisableCouponEarnRule(Convert.ToInt32(RuleId), userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public string ConvertToCommaSeperated(object Items)
        {
            string convertedStr = string.Empty;
            var objData = (object[])Items;
            foreach (var item1 in objData)
            {                
                if (Convert.ToString(item1) == "0")
                {
                    convertedStr = "All";
                }
                else
                {
                    convertedStr += Convert.ToString(Convert.ToString(item1)) + ",";
                }
            }
            if (!string.IsNullOrEmpty(convertedStr))
                convertedStr = convertedStr.Remove(convertedStr.Length - 1);
            return convertedStr;
        }
    }
}