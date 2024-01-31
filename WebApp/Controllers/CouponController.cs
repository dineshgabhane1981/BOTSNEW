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

namespace WebApp.Controllers
{
    public class CouponController : Controller
    {
        Exceptions newexception = new Exceptions();
        CouponRepository CR = new CouponRepository();
        // GET: Coupon
        public ActionResult Index()
        {
            CommonFunctions Common = new CommonFunctions();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var data = CR.GetAllCouponUpload(userDetails.connectionString);
            var coupon = Common.GenerateCoupon(6);
            return View(data);
        }
        public JsonResult UploadCouponBulkData(HttpPostedFileBase file, string ExpiryDate, string Reminder, string CouponValue)
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
                        lstData.Add(objData);
                    }
                    index++;
                }
                objUpload.Count = lstData.Count();
                result = CR.UploadCouponData(lstData, objUpload, userDetails.connectionString);
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "UploadCouponBulkData");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Report()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
    }
}