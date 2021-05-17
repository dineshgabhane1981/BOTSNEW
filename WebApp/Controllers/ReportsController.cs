using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.ComponentModel;
using BOTS_BL;
using System.Globalization;

namespace WebApp.Controllers
{
    public class ReportsController : Controller
    {
        ReportsRepository RR = new ReportsRepository();
        Exceptions newexception = new Exceptions();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MemberSearch()
        {
            return View();
        }

        public ActionResult MemberList()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletList = lstOutlet;
            //ViewBag.OutletId = OutletId;
            return View();
        }

        public ActionResult Outletwise()
        {
            return View();
        }

        public ActionResult Transactionwise(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletList = lstOutlet;
            ViewBag.OutletId = OutletId;
            return View();
        }

        public ActionResult PointsExpiry()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            PointExpiryTmp objPointExpiry = new PointExpiryTmp();
            objPointExpiry = RR.GetPointExpiryData(userDetails.GroupId, DateTime.Now.Month, DateTime.Now.Year, userDetails.connectionString);

            List<SelectListItem> MonthList = new List<SelectListItem>();
            int month = DateTime.Now.Month;

            int count = 1;
            for (int i = 1; i <= 12; i++)
            {
                MonthList.Add(new SelectListItem
                {
                    Text = Convert.ToString(DateTime.Now.AddMonths(i).ToString("MMM")),
                    Value = Convert.ToString(DateTime.Now.AddMonths(i).Month)
                });
                count++;
            }
            List<SelectListItem> YearList = new List<SelectListItem>();
            int year = DateTime.Now.Year;
            for (int i = 0; i <= 9; i++)
            {
                YearList.Add(new SelectListItem
                {
                    Text = Convert.ToString(DateTime.Now.AddYears(i).Year.ToString()),
                    Value = Convert.ToString(year + i)
                });
            }

            ViewBag.MonthList = MonthList;
            ViewBag.YearList = YearList;
            return View(objPointExpiry);
        }

        public ActionResult Celebrations()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var CelebrationsData = RR.GetCelebrationsData(userDetails.GroupId, userDetails.connectionString);
            return View(CelebrationsData);
        }

        [HttpPost]
        public JsonResult GetCelebrationsTxnResult(int month, int type)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CelebrationsMoreDetails> objCelebrationsMoreDetails = new List<CelebrationsMoreDetails>();
            objCelebrationsMoreDetails = RR.GetCelebrationsTxnData(userDetails.GroupId, month, type, userDetails.connectionString);
            return new JsonResult() { Data = objCelebrationsMoreDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CreateOwnSegment()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPointsExpiryDataResult(int month, int year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            PointExpiryTmp objPointExpiry = new PointExpiryTmp();
            objPointExpiry = RR.GetPointExpiryData(userDetails.GroupId, month, year, userDetails.connectionString);
            return new JsonResult() { Data = objPointExpiry, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetPointsExpiryTxnResult(int month, int year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<PointExpiryTxn> objPointExpiryTxn = new List<PointExpiryTxn>();
            objPointExpiryTxn = RR.GetPointExpiryTxnData(userDetails.GroupId, month, year, userDetails.connectionString);
            return new JsonResult() { Data = objPointExpiryTxn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetMemberDataResult(string SearchText)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (SearchText.Equals("All"))
            {
                SearchText = "";
            }
            List<MemberList> lstMember = new List<MemberList>();
            lstMember = RR.GetMemberList(userDetails.GroupId, SearchText, userDetails.connectionString);
            return new JsonResult() { Data = lstMember, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetOutletWiseResult(string DateRangeFlag, string fromDate, string toDate)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<OutletWise> lstOutlet = new List<OutletWise>();
            lstOutlet = RR.GetOutletWiseList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, userDetails.connectionString);
            OutletWise objSum = new OutletWise();
            foreach (var item in lstOutlet)
            {

                objSum.TotalMember = (objSum.TotalMember == null ? 0 : objSum.TotalMember) + (item.TotalMember == null ? 0 : item.TotalMember);
                objSum.TotalTxn = (objSum.TotalTxn == null ? 0 : objSum.TotalTxn) + (item.TotalTxn == null ? 0 : item.TotalTxn);
                objSum.TotalSpend = (objSum.TotalSpend == null ? 0 : objSum.TotalSpend) + (item.TotalSpend == null ? 0 : item.TotalSpend);
                objSum.ATS = (objSum.ATS == null ? 0 : objSum.ATS) + (item.ATS == null ? 0 : item.ATS);
                objSum.NonActive = (objSum.NonActive == null ? 0 : objSum.NonActive) + (item.NonActive == null ? 0 : item.NonActive);
                objSum.OnlyOnce = (objSum.OnlyOnce == null ? 0 : objSum.OnlyOnce) + (item.OnlyOnce == null ? 0 : item.OnlyOnce);
                objSum.PointsEarned = (objSum.PointsEarned == null ? 0 : objSum.PointsEarned) + (item.PointsEarned == null ? 0 : item.PointsEarned);
                objSum.PointsBurned = (objSum.PointsBurned == null ? 0 : objSum.PointsBurned) + (item.PointsBurned == null ? 0 : item.PointsBurned);
                objSum.PointsCancelled = (objSum.PointsCancelled == null ? 0 : objSum.PointsCancelled) + (item.PointsCancelled == null ? 0 : item.PointsCancelled);
                objSum.PointsExpired = (objSum.PointsExpired == null ? 0 : objSum.PointsExpired) + (item.PointsExpired == null ? 0 : item.PointsExpired);
            }
            objSum.OutletName = "Sum";
            lstOutlet.Add(objSum);

            return PartialView("_Outletwise", lstOutlet);
        }

        [HttpPost]
        public JsonResult GetOutletWiseTransactionResult(string DateRangeFlag, string fromDate, string toDate, string outletId, bool EnrolmentDataFlag)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
            lstOutletWiseTransaction = RR.GetOutletWiseTransactionList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, outletId, EnrolmentDataFlag, userDetails.connectionString);
            return new JsonResult() { Data = lstOutletWiseTransaction, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult GetMemberSearchResult(string searchData, string GroupId)
        {
            CustomerRepository objCustRepo = new CustomerRepository();
            MemberSearch objMemberSearch = new MemberSearch();
            try
            {
                if (!string.IsNullOrEmpty(GroupId) && GroupId != "undefined")
                {
                    string connStr = objCustRepo.GetCustomerConnString(GroupId);
                    objMemberSearch = RR.GetMeamberSearchData(GroupId, searchData, connStr);
                }
                else
                {
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    objMemberSearch = RR.GetMeamberSearchData(userDetails.GroupId, searchData, userDetails.connectionString);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return PartialView("_MemberSearch", objMemberSearch);
        }

        public ActionResult ExportToExcelTransactionwise(string DateRangeFlag, string fromDate, string toDate, string outletId, bool EnrolmentDataFlag, string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                if (outletId.Equals("All"))
                {
                    outletId = "";
                }
                List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
                lstOutletWiseTransaction = RR.GetOutletWiseTransactionList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, outletId, EnrolmentDataFlag, userDetails.connectionString);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(OutletwiseTransaction));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (OutletwiseTransaction item in lstOutletWiseTransaction)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                table.Columns.Remove("MobileNo");
                table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                //table.Columns.Remove("MaskedMobileNo");                
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
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

        public ActionResult ExportToExcelOutletwise(string DateRangeFlag, string fromDate, string toDate, string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<OutletWise> lstOutlet = new List<OutletWise>();
                lstOutlet = RR.GetOutletWiseList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, userDetails.connectionString);

                OutletWise objSum = new OutletWise();
                foreach (var item in lstOutlet)
                {
                    objSum.TotalMember = (objSum.TotalMember == null ? 0 : objSum.TotalMember) + (item.TotalMember == null ? 0 : item.TotalMember);
                    objSum.TotalTxn = (objSum.TotalTxn == null ? 0 : objSum.TotalTxn) + (item.TotalTxn == null ? 0 : item.TotalTxn);
                    objSum.TotalSpend = (objSum.TotalSpend == null ? 0 : objSum.TotalSpend) + (item.TotalSpend == null ? 0 : item.TotalSpend);
                    objSum.ATS = (objSum.ATS == null ? 0 : objSum.ATS) + (item.ATS == null ? 0 : item.ATS);
                    objSum.NonActive = (objSum.NonActive == null ? 0 : objSum.NonActive) + (item.NonActive == null ? 0 : item.NonActive);
                    objSum.OnlyOnce = (objSum.OnlyOnce == null ? 0 : objSum.OnlyOnce) + (item.OnlyOnce == null ? 0 : item.OnlyOnce);
                    objSum.PointsEarned = (objSum.PointsEarned == null ? 0 : objSum.PointsEarned) + (item.PointsEarned == null ? 0 : item.PointsEarned);
                    objSum.PointsBurned = (objSum.PointsBurned == null ? 0 : objSum.PointsBurned) + (item.PointsBurned == null ? 0 : item.PointsBurned);
                    objSum.PointsCancelled = (objSum.PointsCancelled == null ? 0 : objSum.PointsCancelled) + (item.PointsCancelled == null ? 0 : item.PointsCancelled);
                    objSum.PointsExpired = (objSum.PointsExpired == null ? 0 : objSum.PointsExpired) + (item.PointsExpired == null ? 0 : item.PointsExpired);
                }
                objSum.OutletName = "Sum";
                lstOutlet.Add(objSum);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(OutletWise));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (OutletWise item in lstOutlet)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                table.Columns.Remove("OutletId");

                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
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

        public ActionResult ExportToExcelPointExpiry(int month, int year, string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<PointExpiryTxn> objPointExpiryTxn = new List<PointExpiryTxn>();
                objPointExpiryTxn = RR.GetPointExpiryTxnData(userDetails.GroupId, month, year, userDetails.connectionString);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(PointExpiryTxn));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (PointExpiryTxn item in objPointExpiryTxn)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                table.Columns.Remove("MobileNo");
                table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
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

        public ActionResult ExportToExcelCelebrations(int month, int type, string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<CelebrationsMoreDetails> objCelebrationsMoreDetails = new List<CelebrationsMoreDetails>();
                objCelebrationsMoreDetails = RR.GetCelebrationsTxnData(userDetails.GroupId, month, type, userDetails.connectionString);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(CelebrationsMoreDetails));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (CelebrationsMoreDetails item in objCelebrationsMoreDetails)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                table.Columns.Remove("MaskedMobileNo");
                //table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";

                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
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

        public ActionResult ProfitableCustomer()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletList = lstOutlet;
            return View();
        }

        [HttpPost]
        public JsonResult GetProfitableCustomersResult(string DateRangeFlag, string fromDate, string toDate, string outletId, string ReportBasis)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            List<MemberList> lstMember = new List<MemberList>();
            lstMember = RR.GetMemberList(userDetails.GroupId, outletId, userDetails.connectionString);

            if (DateRangeFlag == "2")
            {
                foreach(var item in lstMember)
                {
                    if (!string.IsNullOrEmpty(item.LastTxnDate))
                    {                        
                        var datenew = DateTime.ParseExact(item.LastTxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        item.txnDate = Convert.ToDateTime(datenew);
                    }
                    else
                    {
                        item.txnDate = null;
                    }
                }
                DateTime fDate = Convert.ToDateTime(fromDate);
                DateTime tDate = Convert.ToDateTime(toDate);
                lstMember = lstMember.Where(x => x.txnDate >= fDate && x.txnDate <= tDate).ToList();
            }
            if (ReportBasis != "")
            {
                if(ReportBasis=="1")
                {
                    var count = lstMember.Count;
                    if (count > 10)
                    {
                        count = count / 10;
                    }
                    lstMember = lstMember.OrderByDescending(x => x.TotalSpend).Take(count).ToList();
                }
                if (ReportBasis == "2")
                {
                    var count = lstMember.Count;
                    if (count > 10)
                    {
                        count = count / 10;
                    }
                    lstMember = lstMember.OrderByDescending(x => x.TxnCount).Take(count).ToList();
                }
            }

            return new JsonResult() { Data = lstMember, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportToProfitableCustomers(string DateRangeFlag, string fromDate, string toDate, string outletId, string ReportBasis, string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                if (outletId.Equals("All"))
                {
                    outletId = "";
                }
                List<MemberList> lstMember = new List<MemberList>();
                lstMember = RR.GetMemberList(userDetails.GroupId, outletId, userDetails.connectionString);

                if (DateRangeFlag == "2")
                {
                    foreach (var item in lstMember)
                    {
                        if (!string.IsNullOrEmpty(item.LastTxnDate))
                        {
                            var datenew = DateTime.ParseExact(item.LastTxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            item.txnDate = Convert.ToDateTime(datenew);
                        }
                        else
                        {
                            item.txnDate = null;
                        }
                    }
                    DateTime fDate = Convert.ToDateTime(fromDate);
                    DateTime tDate = Convert.ToDateTime(toDate);
                    lstMember = lstMember.Where(x => x.txnDate >= fDate && x.txnDate <= tDate).ToList();
                }
                if (ReportBasis != "")
                {
                    if (ReportBasis == "1")
                    {
                        var count = lstMember.Count;
                        if (count > 10)
                        {
                            count = count / 10;
                        }
                        lstMember = lstMember.OrderByDescending(x => x.TotalSpend).Take(count).ToList();
                    }
                    if (ReportBasis == "2")
                    {
                        var count = lstMember.Count;
                        if (count > 10)
                        {
                            count = count / 10;
                        }
                        lstMember = lstMember.OrderByDescending(x => x.TxnCount).Take(count).ToList();
                    }
                }
                
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(MemberList));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (MemberList item in lstMember)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                table.Columns.Remove("MobileNo");
                table.Columns.Remove("txnDate");                
                table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                //table.Columns.Remove("MaskedMobileNo");                
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
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



    }
}