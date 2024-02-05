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

namespace WebApp.Controllers
{
    public class CouponController : Controller
    {
        Exceptions newexception = new Exceptions();
        CouponRepository CR = new CouponRepository();
        ReportsRepository RR = new ReportsRepository();
        OtherReportsRepository ORR = new OtherReportsRepository();
        // GET: Coupon
        public ActionResult Index()
        {
            UploadCouponViewModel objData = new UploadCouponViewModel();
            CommonFunctions Common = new CommonFunctions();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData.lstCouponUpload = CR.GetAllCouponUpload(userDetails.connectionString);
            objData.lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            objData.lstCategory = ORR.GetCategoryCode(userDetails.GroupId, userDetails.connectionString);
            objData.lstProduct = ORR.GetProductId(userDetails.GroupId, userDetails.connectionString);
            return View(objData);
        }
        public JsonResult UploadCouponBulkData(HttpPostedFileBase file, string ExpiryDate, string Reminder, string CouponValue, string InvoiceValue, string Day, string Category, string Product, string Outlet)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            DataTable dt = new DataTable();
            tblCouponUpload objUpload = new tblCouponUpload();
            try
            {
                objUpload.CouponFileName = file.FileName;
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
                        objData.CouponCode = Common.GenerateCoupon(6);
                        objData.CouponValue = Convert.ToDecimal(CouponValue);
                        objData.ExpiryDate = Convert.ToDateTime(ExpiryDate);
                        objData.CreatedDate = DateTime.Now;
                        objData.CreatedBy = userDetails.LoginId;
                        objData.ReminderDate = Convert.ToDateTime(ExpiryDate).AddDays(-Convert.ToInt32(Reminder));
                        if (!string.IsNullOrEmpty(InvoiceValue))
                            objData.RedeemInvoiceAmount = Convert.ToDecimal(InvoiceValue);
                        if (!string.IsNullOrEmpty(Day))
                            objData.RedeemDay = Day;
                        if (!string.IsNullOrEmpty(Category))
                            objData.RedeemCategory = Category;
                        if (!string.IsNullOrEmpty(Product))
                            objData.RedeemProduct = Product;
                        if (!string.IsNullOrEmpty(Outlet))
                            objData.RedeemOutlet = Outlet;
                                                
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
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnInvoiceAmount"])))
                        objRule.EarnInvoiceAmount = Convert.ToDecimal(item["EarnInvoiceAmount"]);
                    objRule.EarnDay = Convert.ToString(item["EarnDay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnCategory"])))
                        objRule.EarnCategory = Convert.ToInt32(item["EarnCategory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["EarnProduct"])))
                        objRule.EarnCategory = Convert.ToInt32(item["EarnProduct"]);
                    objRule.EarnOutlet = Convert.ToString(item["EarnOutlet"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemInvoiceAmount"])))
                        objRule.RedeemInvoiceAmount = Convert.ToDecimal(item["RedeemInvoiceAmount"]);
                    objRule.RedeemDay = Convert.ToString(item["RedeemDay"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemCategory"])))
                        objRule.RedeemCategory = Convert.ToInt32(item["RedeemCategory"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["RedeemProduct"])))
                        objRule.RedeemProduct = Convert.ToInt32(item["RedeemProduct"]);
                    objRule.RedeemOutlet = Convert.ToString(item["RedeemOutlet"]);

                    objRule.IsActive = true;
                    objRule.AddedDate = DateTime.Now;
                    objRule.AddedBy = userDetails.LoginId;
                    objRule.IsOnlyCoupon = Convert.ToBoolean(item["IsOnlyCoupon"]);
                }
                result = CR.SaveCouponEarnRule(objRule, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}