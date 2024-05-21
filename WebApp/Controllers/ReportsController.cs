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
using System.Web.Script.Serialization;
using BOTS_BL.Models.Reports;
using System.Net.Mail;
using System.Text;
using WebApp.ViewModel;
using System.Reflection;

namespace WebApp.Controllers
{
    public class ReportsController : Controller
    {
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository CR = new CustomerRepository();
        OtherReportsRepository ORR = new OtherReportsRepository();
        ITOPSNEWRepository ITOPSNEW = new ITOPSNEWRepository();
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
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ViewBag.userdetail = userDetails;
            return View();
        }
        public ActionResult Transactionwise(string OutletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.userdetail = userDetails;
            ViewBag.OutletList = lstOutlet;
            ViewBag.OutletId = OutletId;
            return View();
        }
        public ActionResult PointsExpiry()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            PointExpiryTmp objPointExpiry = new PointExpiryTmp();
            objPointExpiry = RR.GetPointExpiryData(userDetails.GroupId, DateTime.Now.Month, DateTime.Now.Year, userDetails.connectionString, userDetails.LoginId);

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
            try
            {
                objCelebrationsMoreDetails = RR.GetCelebrationsTxnData(userDetails.GroupId, month, type, userDetails.connectionString);
                var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));

                long? amt = 0;
                long? apts = 0;
                long? txnCount = 0;
                foreach (var item in objCelebrationsMoreDetails)
                {
                    amt += item.TotalSpend;
                    apts += item.AvlPoints;
                    txnCount += item.TxnCount;
                }
                CelebrationsMoreDetails objTotal = new CelebrationsMoreDetails();
                objTotal.EnrolledOutlet = "<b>Total</b>";
                objTotal.MobileNo = "";
                objTotal.TotalSpend = amt;
                objTotal.AvlPoints = apts;
                objTotal.TxnCount = txnCount;
                objTotal.MemberName = "";
                objCelebrationsMoreDetails.Add(objTotal);

                if (!GroupDetails.IsMasked)
                {
                    foreach (var item in objCelebrationsMoreDetails)
                    {
                        item.MaskedMobileNo = item.MobileNo;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCelebrationsTxnResult");
            }
            return new JsonResult() { Data = objCelebrationsMoreDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult CreateOwnSegment()
        {
            CreateOwnReportViewModel createownviewmodel = new CreateOwnReportViewModel();

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            //var lstEnrolledList = RR.GetEnrolledList(userDetails.GroupId, userDetails.connectionString);
            //ViewBag.lstEnrolledList = lstEnrolledList;
            var lstnontransactedList = RR.GetNonTransactedList(userDetails.GroupId, userDetails.connectionString);
            // ViewBag.lstnontransactedList = lstnontransactedList;
            createownviewmodel.LstnontransactedList = lstnontransactedList;
            var lstSpendList = RR.GetSpendList(userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.LstSpendList = lstSpendList;
            // ViewBag.lstSpendList = lstSpendList;
            //var lstGenderList = RR.GetGenderList(userDetails.GroupId, userDetails.connectionString);
            //ViewBag.lstGenderList = lstGenderList;
            //var lstSourceList = RR.GetSourseList(userDetails.GroupId, userDetails.connectionString);
            //ViewBag.lstSourceList = lstSourceList;
            var lstRedeemedList = RR.GetRedeemedList(userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.LstRedeemedList = lstRedeemedList;
            var lstOutlet = RR.GetOutletListForSliceAndDice(userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.LstOutlet = lstOutlet;
            DateTime startdt = RR.GetStartDtOfProgram(userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.StartDt = startdt;
            var TotalCount = RR.GetTotalMemberCount(userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.TotalCount = TotalCount;
            var lstBrandList = RR.GetBrandList(userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.LstBrandList = lstBrandList;
            List<string> columnname = new List<string>();
            //object[] columnname = new object[15];
            createownviewmodel.lstcolumnlist = columnname;
            return View(createownviewmodel);


        }
        public JsonResult GetFilteredData(string jsonData)
        {

            // var transcount = 0;
            CustomerIdListAndCount objcounts = new CustomerIdListAndCount();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CustomerDetail> listCustD = new List<CustomerDetail>();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    //string gender = Convert.ToString(item["Gender"]);
                    //string Age_min = Convert.ToString(item["Age-min"]);
                    //string Age_max = Convert.ToString(item["Age-max"]);
                    //string source = Convert.ToString(item["Source"]);
                    string Enroll_min = Convert.ToString(item["Enroll-min"]);
                    string Enroll_max = Convert.ToString(item["Enroll-max"]);
                    string Nontransacted_min = Convert.ToString(item["Nontransacted-min"]);
                    //string Nontransacted_max = Convert.ToString(item["Nontransacted-max"]);
                    DateTime fromdtforall = Convert.ToDateTime(item["fromdtall"]);
                    DateTime todtforall = Convert.ToDateTime(item["todtall"]);
                    int Spend_min = Convert.ToInt32(item["Spend-min"]);
                    int Spend_max = Convert.ToInt32(item["Spend-max"]);
                    int txncount_min = Convert.ToInt32(item["txncount-min"]);
                    int txncount_max = Convert.ToInt32(item["txncount-max"]);
                    int pointBaln_min = Convert.ToInt32(item["pointBaln-min"]);
                    int pointBaln_max = Convert.ToInt32(item["pointBaln-max"]);
                    string Redeem = Convert.ToString(item["Redeem"]);
                    //int TicketSize_min = Convert.ToInt32(item["TicketSize-min"]);
                    //int TicketSize_max = Convert.ToInt32(item["TicketSize-max"]);
                    string Brand = Convert.ToString(item["Brand"]);
                    object[] outletId = (object[])item["Outlet"];

                    objcounts = RR.GetSliceAndDiceFilteredData(fromdtforall, todtforall, Enroll_max, Enroll_min, Nontransacted_min, Spend_min, Spend_max, txncount_min, txncount_max, pointBaln_min, pointBaln_max, Redeem, Brand, outletId, userDetails.GroupId, userDetails.connectionString);
                    Session["customerId"] = objcounts.lstcustomerDetails;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }
            return new JsonResult() { Data = objcounts, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GenerateReport(string jsonData)
        {
            // var transcount = 0;
            // List<int> lstcounts = new List<int>();
            CreateOwnReportViewModel createownviewmodel = new CreateOwnReportViewModel();
            List<CustomerDetail> lstcustomerdetails = (List<CustomerDetail>)Session["customerId"];
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CustomerTypeReport> listCustR = new List<CustomerTypeReport>();
            List<TransactionTypeReport> listTxnR = new List<TransactionTypeReport>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string ReportType = Convert.ToString(item["reporttype"]);
                object[] ColumnId = (object[])item["ColumnId"];
                object[] columnname = (object[])item["columnnm"];
                List<string> lstcolumnlist = new List<string>();
                foreach (var itemNew1 in (columnname))
                {
                    string name = Convert.ToString(itemNew1);
                    lstcolumnlist.Add(name);
                }
                List<string> lstcolumnIdlist = new List<string>();
                foreach (var itemNew1 in (columnname))
                {
                    //string name = Convert.ToString(itemNew1);
                    string name = string.Concat(Convert.ToString(itemNew1).Where(c => !char.IsWhiteSpace(c)));
                    lstcolumnIdlist.Add(name);
                }
                createownviewmodel.lstcolumnlist = lstcolumnlist;
                createownviewmodel.lstcolumnIdlist = lstcolumnIdlist;
                Session["ExportColumnList"] = createownviewmodel.lstcolumnlist;
                if (ReportType == "customer")
                {
                    List<object> lstcustlist = new List<object>();
                    //createownviewmodel.listCustR = RR.GenerateCustomerTypeReport(ColumnId, lstcustomerdetails, userDetails.GroupId, userDetails.connectionString);
                    Session["ExportCustomerReport"] = createownviewmodel.listCustR;
                }
                else if (ReportType == "transaction")
                {
                    //createownviewmodel.listTxnR = RR.GenerateTxnTypeReport(ColumnId, lstcustomerdetails, userDetails.GroupId, userDetails.connectionString);
                    Session["ExportTransactionReport"] = createownviewmodel.listTxnR;
                }

            }

            return PartialView("_CreateOwnReportCustomerWise", createownviewmodel);
            //return new JsonResult() { Data = createownviewmodel, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetFilteredCountforDrillDown(string jsonData)
        {

            List<ReportFilterCount> filtercount = new List<ReportFilterCount>();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<CustomerDetail> listCustD = new List<CustomerDetail>();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {

                    int Elementtype = Convert.ToInt32(item["Element_Type"]);
                    int Elementfilter = Convert.ToInt32(item["Element_Filter"]);
                    long Element1 = Convert.ToInt64(item["element1"]);
                    long Element2 = Convert.ToInt64(item["element2"]);
                    string BillSizeFilter = Convert.ToString(item["AvgBillFilter"]);
                    long BillSize_min = Convert.ToInt64(item["AvgBill-min"]);
                    long BillSize_max = Convert.ToInt64(item["AvgBill-max"]);
                    int periodsfilter = Convert.ToInt32(item["period_filter"]);
                    string periodFrm = Convert.ToString(item["period_From"]);
                    string periodTo = Convert.ToString(item["period_To"]);
                    string outletIds = Convert.ToString(item["OutletIds"]);

                    filtercount = RR.GetFilterCountOfDrillDown(Elementtype, Elementfilter, Element1, Element2, BillSizeFilter, BillSize_min, BillSize_max, periodsfilter, periodFrm, periodTo, outletIds, userDetails.GroupId, userDetails.connectionString);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredCountforDrillDown");
            }
            return new JsonResult() { Data = filtercount, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetPointsExpiryDataResult(int month, int year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            PointExpiryTmp objPointExpiry = new PointExpiryTmp();
            objPointExpiry = RR.GetPointExpiryData(userDetails.GroupId, month, year, userDetails.connectionString, userDetails.LoginId);

            return new JsonResult() { Data = objPointExpiry, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetPointsExpiryTxnResult(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<PointExpiryTxn> objPointExpiryTxn = new List<PointExpiryTxn>();
            try
            {
                objPointExpiryTxn = RR.GetPointExpiryTxnData(userDetails.GroupId, Convert.ToInt32(month), Convert.ToInt32(year), userDetails.connectionString, userDetails.LoginId);
                var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
                long amt = 0;
                long epts = 0;
                long apts = 0;
                long txnCount = 0;
                foreach (var item in objPointExpiryTxn)
                {
                    amt += item.TotalSpend;
                    epts += item.PointsExpiry;
                    apts += item.AvlPoints;
                    txnCount += item.TxnCount;
                }

                PointExpiryTxn objTotal = new PointExpiryTxn();
                objTotal.EnrolledOutlet = "<b>Total</>";
                objTotal.MobileNo = "";
                objTotal.MemberName = "";
                objTotal.TxnCount = txnCount;
                objTotal.TotalSpend = amt;
                objTotal.AvlPoints = apts;
                objTotal.PointsExpiry = epts;

                objPointExpiryTxn.Add(objTotal);

                if (!GroupDetails.IsMasked)
                {
                    foreach (var item in objPointExpiryTxn)
                    {
                        item.MaskedMobileNo = item.MobileNo;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsExpiryTxnResult");
            }
            return new JsonResult() { Data = objPointExpiryTxn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetMemberDataResult(string jsonData)
        {
            List<MemberList> lstMember = new List<MemberList>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string fromDate = string.Empty;
                string ToDate = string.Empty;
                string FrmPts = string.Empty;
                string ToPts = string.Empty;
                string FrmSpend = string.Empty;
                string ToSpend = string.Empty;

                string OutletId = Convert.ToString(item["OutletId"]);
                if (OutletId.Equals("All"))
                {
                    OutletId = "";
                }
                
                lstMember = RR.GetMemberList(userDetails.GroupId, OutletId, userDetails.connectionString, userDetails.LoginId, fromDate, ToDate, FrmPts,
                    ToPts, FrmSpend, ToSpend);
            }

            long? apts = 0;
            long btxn = 0;
            long txnCount = 0;
            long bpts = 0;
            long totalSpend = 0;
            foreach (var item in lstMember)
            {
                txnCount += item.TxnCount;
                totalSpend += item.TotalSpend;
                btxn += item.TotalBurnTxn;
                bpts += item.TotalBurnPoints;
                apts += item.AvlBalPoints;
            }
            MemberList objTotal = new MemberList();
            objTotal.EnrooledOutlet = "<b>Total</b>";
            objTotal.MobileNo = "";
            objTotal.MemberName = "";
            objTotal.TxnCount = txnCount;
            objTotal.TotalSpend = totalSpend;
            objTotal.TotalBurnTxn = btxn;
            objTotal.TotalBurnPoints = bpts;
            objTotal.AvlBalPoints = apts;
            objTotal.MobileNo = "";
            objTotal.MaskedMobileNo = "";
            lstMember.Add(objTotal);

            return new JsonResult() { Data = lstMember, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ExportToExcelMemberList(string outletId, string fromDate, string toDate, string frmPts, string toPts, string frmSpends, string toSpends)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                List<MemberList> lstMember = new List<MemberList>();
                lstMember = RR.GetMemberList(userDetails.GroupId, outletId, userDetails.connectionString, userDetails.LoginId, fromDate, toDate, frmPts, toPts, frmSpends, toSpends);


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
                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalSpend"])))
                    {
                        dr["TotalSpendStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalSpend"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["AvlBalPoints"])))
                    {
                        dr["AvlBalPointsStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["AvlBalPoints"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalBurnPoints"])))
                    {
                        dr["TotalBurnPointsStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalBurnPoints"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LastTxnDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["LastTxnDate"] = DateTime.ParseExact(Convert.ToString(dr["LastTxnDate"]), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["EnrolledDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["EnrolledDate"] = DateTime.ParseExact(Convert.ToString(dr["EnrolledDate"]), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }

                table.Columns.Remove("TotalSpend");
                table.Columns.Remove("AvlBalPoints");
                table.Columns.Remove("TotalBurnPoints");
                table.Columns.Remove("txnDate");
                table.Columns["TotalSpendStr"].ColumnName = "TotalSpend";
                table.Columns["AvlBalPointsStr"].ColumnName = "AvlBalPoints";
                table.Columns["TotalBurnPointsStr"].ColumnName = "TotalBurnPoints";

                table.Columns.Remove("MaskedMobileNo");


                string ReportName = "MemberData";
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;

                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Member Data";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Filter";

                    worksheet.Cell(5, 1).InsertTable(table);
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
                newexception.AddException(ex, "ExportToProfitableCustomers");
                return null;
            }


        }
        public ActionResult GetOutletWiseResult(string DateRangeFlag, string fromDate, string toDate)
        {
            string loginId = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            loginId = userDetails.LoginId;
            if (userDetails.LevelIndicator == "03" || userDetails.LevelIndicator == "04")
            {
                loginId = userDetails.OutletOrBrandId;
            }
            List<OutletWise> lstOutlet = new List<OutletWise>();
            lstOutlet = RR.GetOutletWiseList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, userDetails.connectionString, loginId);
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
                if (item.PointsCancelled.HasValue)
                    item.PointsCancelled = Math.Abs(item.PointsCancelled.Value);
                objSum.PointsCancelled = (objSum.PointsCancelled == null ? 0 : objSum.PointsCancelled) + (item.PointsCancelled == null ? 0 : item.PointsCancelled);

                objSum.PointsExpired = (objSum.PointsExpired == null ? 0 : objSum.PointsExpired) + (item.PointsExpired == null ? 0 : item.PointsExpired);

                if (item.TotalMember > 0)
                {
                    item.NonActivePer = (Convert.ToDecimal(item.NonActive) * 100) / Convert.ToDecimal(item.TotalMember);
                    item.OnlyOncePer = (Convert.ToDecimal(item.OnlyOnce) * 100) / Convert.ToDecimal(item.TotalMember);
                }
            }
            //List<OutletWise> lstOutletFinal = new List<OutletWise>();
            //OutletWise objAdmin = new OutletWise();
            //objAdmin = lstOutlet.Where(x => x.OutletName.ToLower().IndexOf("admin") >= 0).FirstOrDefault();
            //if (objAdmin != null)
            //{
            //    foreach (var item in lstOutlet)
            //    {
            //        if (item.OutletId != objAdmin.OutletId)
            //        {
            //            lstOutletFinal.Add(item);
            //        }
            //    }
            //    //lstOutletFinal.Add(objAdmin);
            //}
            //else
            //{
            //    lstOutletFinal = lstOutlet;
            //}

            int totalCount = lstOutlet.Count;
            int nonActiveRed = totalCount * 30 / 100;
            int nonActiveOrange = totalCount * 45 / 100;
            int nonActiveGreen = totalCount - (nonActiveRed + nonActiveOrange);

            var nonActiveRedOutlets = lstOutlet.OrderByDescending(x => x.NonActivePer).Take(nonActiveRed).ToList();
            var nonActiveOrangeOutlets = lstOutlet.OrderByDescending(x => x.NonActivePer).Skip(nonActiveRed).Take(nonActiveOrange).ToList();
            var nonActiveGreenOutlets = lstOutlet.OrderByDescending(x => x.NonActivePer).Skip(nonActiveRed + nonActiveOrange).Take(nonActiveGreen).ToList();

            var onlyOnceRedOutlets = lstOutlet.OrderByDescending(x => x.OnlyOncePer).Take(nonActiveRed).ToList();
            var onlyOnceOrangeOutlets = lstOutlet.OrderByDescending(x => x.OnlyOncePer).Skip(nonActiveRed).Take(nonActiveOrange).ToList();
            var onlyOnceGreenOutlets = lstOutlet.OrderByDescending(x => x.OnlyOncePer).Skip(nonActiveRed + nonActiveOrange).Take(nonActiveGreen).ToList();

            var RedmRedOutlets = lstOutlet.OrderBy(x => x.RedemptionRate).Take(nonActiveRed).ToList();
            var RedmOrangeOutlets = lstOutlet.OrderBy(x => x.RedemptionRate).Skip(nonActiveRed).Take(nonActiveOrange).ToList();
            var RedmGreenOutlets = lstOutlet.OrderBy(x => x.RedemptionRate).Skip(nonActiveRed + nonActiveOrange).Take(nonActiveGreen).ToList();


            foreach (var item in lstOutlet)
            {
                var outletRed = nonActiveRedOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletRed != null)
                {
                    item.NonActiveColor = "Red";
                }
                var outletOrange = nonActiveOrangeOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletOrange != null)
                {
                    item.NonActiveColor = "Orange";
                }
                var outletGreen = nonActiveGreenOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletGreen != null)
                {
                    item.NonActiveColor = "Green";
                }

                var outletOORed = onlyOnceRedOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletOORed != null)
                {
                    item.OnlyOnceColor = "Red";
                }
                var outletOOOrange = onlyOnceOrangeOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletOOOrange != null)
                {
                    item.OnlyOnceColor = "Orange";
                }
                var outletOOGreen = onlyOnceGreenOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletOOGreen != null)
                {
                    item.OnlyOnceColor = "Green";
                }

                var outletRRRed = RedmRedOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletRRRed != null)
                {
                    item.RedemptionRateColor = "Red";
                }
                var outletRROrange = RedmOrangeOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletRROrange != null)
                {
                    item.RedemptionRateColor = "Orange";
                }
                var outletRRGreen = RedmGreenOutlets.Where(x => x.OutletId == item.OutletId).FirstOrDefault();
                if (outletRRGreen != null)
                {
                    item.RedemptionRateColor = "Green";
                }
            }
            objSum.OutletName = "Total";
            lstOutlet.Add(objSum);
            return PartialView("_Outletwise", lstOutlet);
        }
        [HttpPost]
        public JsonResult GetOutletWiseTransactionResult(string DateRangeFlag, string fromDate, string toDate, string outletId, string EnrolmentDataFlag)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
            try
            {
                lstOutletWiseTransaction = RR.GetOutletWiseTransactionList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, outletId, EnrolmentDataFlag, userDetails.connectionString, userDetails.LoginId);
                var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
                //decimal? amt = 0;
                //decimal? epts = 0;
                //decimal? bpts = 0;
                //foreach (var item in lstOutletWiseTransaction)
                //{
                //    amt += item.InvoiceAmt;
                //    epts += item.PointsEarned;
                //    bpts += item.PointsBurned;
                //}
                //OutletwiseTransaction totalObj = new OutletwiseTransaction();
                //totalObj.InvoiceAmt = amt;
                //totalObj.PointsEarned = epts;
                //totalObj.PointsBurned = bpts;
                //totalObj.OutletName = "<b>Total</b>";
                //totalObj.MobileNo = "-";
                //totalObj.MaskedMobileNo = "-";
                //totalObj.MemberName = "-";
                //totalObj.Type = "-";
                //totalObj.InvoiceNo = "-";
                //totalObj.TxnType = "-";
                //totalObj.TxnDatetime = "-";
                //totalObj.TxnUpdateDate = "-";


                //lstOutletWiseTransaction.Add(totalObj);

                if (!GroupDetails.IsMasked)
                {
                    foreach (var item in lstOutletWiseTransaction)
                    {
                        item.MaskedMobileNo = item.MobileNo;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletWiseTransactionResult");
            }
            return new JsonResult() { Data = lstOutletWiseTransaction, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public ActionResult GetMemberSearchResult(string searchData, string GroupId)
        {
            CustomerRepository objCustRepo = new CustomerRepository();
            MemberSearch objMemberSearch = new MemberSearch();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                string loginId = string.Empty;
                if (userDetails.LevelIndicator == "03" || userDetails.LevelIndicator == "04")
                {
                    loginId = userDetails.LoginId;
                }
                if (!string.IsNullOrEmpty(GroupId) && GroupId != "undefined")
                {
                    string connStr = objCustRepo.GetCustomerConnString(GroupId);
                    objMemberSearch = RR.GetMeamberSearchData(GroupId, searchData, connStr, loginId);
                }
                else
                {
                    objMemberSearch = RR.GetMeamberSearchData(userDetails.GroupId, searchData, userDetails.connectionString, loginId);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberSearchResult");
            }
            return PartialView("_MemberSearch", objMemberSearch);
        }
        public ActionResult ExportToExcelTransactionwise(string DateRangeFlag, string fromDate, string toDate, string outletId, string EnrolmentDataFlag, string ReportName, string EmailId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {

                if (outletId.Equals("All"))
                {
                    outletId = "";
                }
                List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
                lstOutletWiseTransaction = RR.GetOutletWiseTransactionList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, outletId, EnrolmentDataFlag, userDetails.connectionString, userDetails.LoginId);

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
                if (userDetails.LoginType == "1" || userDetails.LoginType == "6" || userDetails.LoginType == "7")
                {
                    table.Columns.Remove("MaskedMobileNo");
                }
                else
                {
                    var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
                    if (!GroupDetails.IsMasked)
                    {
                        table.Columns.Remove("MaskedMobileNo");
                    }
                    else
                    {
                        table.Columns.Remove("MobileNo");
                        table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                    }
                }

                decimal? amt = 0;
                decimal? epts = 0;
                decimal? bpts = 0;
                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["InvoiceAmt"])))
                        amt += Convert.ToDecimal(dr["InvoiceAmt"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsEarned"])))
                        epts += Convert.ToDecimal(dr["PointsEarned"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsBurned"])))
                        bpts += Convert.ToDecimal(dr["PointsBurned"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["InvoiceAmt"])))
                    {
                        dr["InvoiceAmtStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["InvoiceAmt"]));

                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsEarned"])))
                    {
                        dr["PointsEarnedStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsEarned"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsBurned"])))
                    {
                        dr["PointsBurnedStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsBurned"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TxnDatetime"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        var subDate = Convert.ToString(dr["TxnDatetime"]).Substring(0, 10);

                        string subTime = "";
                        if (Convert.ToString(dr["TxnDatetime"]).Length > 10)
                            subTime = Convert.ToString(dr["TxnDatetime"]).Substring(11, 8);
                        var convertedDate = DateTime.ParseExact(subDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                        if (subTime == "")
                            dr["TxnDatetime"] = convertedDate;
                        else
                            dr["TxnDatetime"] = convertedDate + " " + subTime;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TxnUpdateDate"])))
                    {
                        //dr["TxnUpdateDate"] = Convert.ToDateTime(dr["TxnUpdateDate"]).ToString("MM/dd/yyyy hh:mm:ss");
                        var subDate = Convert.ToString(dr["TxnUpdateDate"]).Substring(0, 10);
                        //var subTime = Convert.ToString(dr["TxnUpdateDate"]).Substring(11, 8);
                        var convertedDate = DateTime.ParseExact(subDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                        dr["TxnUpdateDate"] = convertedDate;// + " " + subTime;
                    }

                }
                table.Columns.Remove("InvoiceAmt");
                table.Columns.Remove("PointsEarned");
                table.Columns.Remove("PointsBurned");
                table.Columns["InvoiceAmtStr"].ColumnName = "InvoiceAmt";
                table.Columns["PointsEarnedStr"].ColumnName = "PointsEarned";
                table.Columns["PointsBurnedStr"].ColumnName = "PointsBurned";

                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Transactionwise";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Period";
                    worksheet.Cell(3, 2).Value = fromDate + "-" + toDate;
                    worksheet.Cell(5, 1).InsertTable(table);
                    //wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        if (EmailId != "")
                        {
                            RR.email_send(EmailId, ReportName, stream.ToArray(), userDetails.EmailId);

                        }
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
        public ActionResult ExportToExcelOutletwise(string DateRangeFlag, string fromDate, string toDate, string ReportName, string EmailId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                string loginId = string.Empty;
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                if (userDetails.LevelIndicator == "03" || userDetails.LevelIndicator == "04")
                {
                    loginId = userDetails.LoginId;
                }
                List<OutletWise> lstOutlet = new List<OutletWise>();
                lstOutlet = RR.GetOutletWiseList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, userDetails.connectionString, loginId);

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

                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalMember"])))
                    {
                        dr["TotalMemberStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalMember"]));
                    }
                    else
                    {
                        dr["TotalMemberStr"] = 0;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalTxn"])))
                    {
                        dr["TotalTxnStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalTxn"]));
                    }
                    else
                    {
                        dr["TotalTxnStr"] = 0;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalSpend"])))
                    {
                        dr["TotalSpendStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalSpend"]));
                    }
                    else
                    {
                        dr["TotalSpendStr"] = 0;
                    }
                    //dr["TotalSpendStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalSpend"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ATS"])))
                    {
                        dr["ATSStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["ATS"]));
                    }
                    else
                    {
                        dr["ATSStr"] = 0;
                    }
                    //dr["ATSStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["ATS"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsEarned"])))
                    {
                        dr["PointsEarnedStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsEarned"]));
                    }
                    else
                    {
                        dr["PointsEarnedStr"] = 0;
                    }
                    //dr["PointsEarnedStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsEarned"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsBurned"])))
                    {
                        dr["PointsBurnedStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsBurned"]));
                    }
                    else
                    {
                        dr["PointsBurnedStr"] = 0;
                    }
                    //dr["PointsBurnedStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsBurned"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsCancelled"])))
                    {
                        dr["PointsCancelledStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsCancelled"]));
                    }
                    else
                    {
                        dr["PointsCancelledStr"] = 0;
                    }
                    //dr["PointsCancelledStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsCancelled"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsExpired"])))
                    {
                        dr["PointsExpiredStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsExpired"]));
                    }
                    else
                    {
                        dr["PointsExpiredStr"] = 0;
                    }
                    //dr["PointsExpiredStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsExpired"]));

                }
                table.Columns.Remove("TotalMember");
                table.Columns.Remove("TotalTxn");
                table.Columns.Remove("TotalSpend");
                table.Columns.Remove("ATS");
                table.Columns.Remove("PointsEarned");
                table.Columns.Remove("PointsBurned");
                table.Columns.Remove("PointsCancelled");
                table.Columns.Remove("PointsExpired");

                table.Columns["TotalMemberStr"].ColumnName = "TotalMember";
                table.Columns["TotalTxnStr"].ColumnName = "TotalTxn";
                table.Columns["TotalSpendStr"].ColumnName = "TotalSpend";
                table.Columns["ATSStr"].ColumnName = "ATS";
                table.Columns["PointsEarnedStr"].ColumnName = "PointsEarned";
                table.Columns["PointsBurnedStr"].ColumnName = "PointsBurned";
                table.Columns["PointsCancelledStr"].ColumnName = "PointsCancelled";
                table.Columns["PointsExpiredStr"].ColumnName = "PointsExpired";

                table.Columns.Remove("BizShare");
                table.Columns.Remove("NonActiveColor");
                table.Columns.Remove("OnlyOnceColor");
                table.Columns.Remove("RedemptionRateColor");
                table.Columns.Remove("NonActivePer");
                table.Columns.Remove("OnlyOncePer");

                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Outletwise";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    if (DateRangeFlag == "1")
                    {
                        worksheet.Cell(3, 1).Value = "Period";
                        worksheet.Cell(3, 2).Value = fromDate + "-" + toDate;
                    }
                    if (DateRangeFlag == "0")
                    {
                        worksheet.Cell(3, 1).Value = "BTD";

                    }
                    worksheet.Cell(5, 1).InsertTable(table);
                    //wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        if (EmailId != "")
                        {
                            RR.email_send(EmailId, ReportName, stream.ToArray(), userDetails.EmailId);

                        }

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelOutletwise");
                return null;
            }

        }
        public ActionResult ExportToExcelPointExpiry(int month, int year, string ReportName, string EmailId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<PointExpiryTxn> objPointExpiryTxn = new List<PointExpiryTxn>();
                objPointExpiryTxn = RR.GetPointExpiryTxnData(userDetails.GroupId, month, year, userDetails.connectionString, userDetails.LoginId);

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
                if (userDetails.LoginType == "1" || userDetails.LoginType == "6" || userDetails.LoginType == "7")
                {
                    table.Columns.Remove("MaskedMobileNo");
                }
                else
                {
                    var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
                    if (!GroupDetails.IsMasked)
                    {
                        table.Columns.Remove("MaskedMobileNo");
                    }
                    else
                    {
                        table.Columns.Remove("MobileNo");
                        table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                    }
                }

                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalSpend"])))
                    {
                        dr["TotalSpendStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalSpend"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["AvlPoints"])))
                    {
                        dr["AvlPointsStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["AvlPoints"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PointsExpiry"])))
                    {
                        dr["PointsExpiryStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["PointsExpiry"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LastTxnDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["LastTxnDate"] = DateTime.ParseExact(Convert.ToString(dr["LastTxnDate"]), "MM/dd/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ExpiryDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["ExpiryDate"] = DateTime.ParseExact(Convert.ToString(dr["ExpiryDate"]), "MM/dd/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                table.Columns.Remove("TotalSpend");
                table.Columns.Remove("AvlPoints");
                table.Columns.Remove("PointsExpiry");
                table.Columns["TotalSpendStr"].ColumnName = "TotalSpend";
                table.Columns["AvlPointsStr"].ColumnName = "AvlPoints";
                table.Columns["PointsExpiryStr"].ColumnName = "PointsExpiry";

                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Point Expiry";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Period";
                    worksheet.Cell(3, 2).Value = month + "-" + year;
                    worksheet.Cell(5, 1).InsertTable(table);
                    //wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        if (EmailId != "")
                        {
                            RR.email_send(EmailId, ReportName, stream.ToArray(), userDetails.EmailId);

                        }
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelPointExpiry");
                return null;
            }

        }

        public ActionResult ExportToExcelCelebrations(int month, int type, string ReportName, string EmailId)
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

                if (userDetails.LoginType == "1" || userDetails.LoginType == "6" || userDetails.LoginType == "7")
                {
                    table.Columns.Remove("MaskedMobileNo");
                }
                else
                {
                    var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
                    if (!GroupDetails.IsMasked)
                    {
                        table.Columns.Remove("MaskedMobileNo");
                    }
                    else
                    {
                        table.Columns.Remove("MobileNo");
                        table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                    }
                }

                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalSpend"])))
                    {
                        dr["TotalSpendStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalSpend"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["AvlPoints"])))
                    {
                        dr["AvlPointsStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["AvlPoints"]));
                    }
                    //if (!string.IsNullOrEmpty(Convert.ToString(dr["LastTxnDate"])))
                    //{
                    //    //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");

                    //    var subDate = Convert.ToString(dr["LastTxnDate"]).Substring(0, 10);

                    //    dr["LastTxnDate"] = DateTime.ParseExact(subDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                    //    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //}
                    //if (!string.IsNullOrEmpty(Convert.ToString(dr["CelebrationDate"])))
                    //{
                    //    //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                    //    var subDate = Convert.ToString(dr["CelebrationDate"]).Substring(0, 10);

                    //    dr["CelebrationDate"] = DateTime.ParseExact(subDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                    //    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //}
                }

                table.Columns.Remove("TotalSpend");
                table.Columns.Remove("AvlPoints");
                table.Columns.Remove("PointsExpiry");
                table.Columns.Remove("ExpiryDate");

                table.Columns["TotalSpendStr"].ColumnName = "TotalSpend";
                table.Columns["AvlPointsStr"].ColumnName = "AvlPoints";

                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Celebrations";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Celebration";
                    DateTime today = DateTime.Now;
                    DateTime nxtmnt = today.AddMonths(1).AddSeconds(-1);
                    DateTime nxt2mnt = nxtmnt.AddMonths(1).AddSeconds(-1);
                    string currentmonth = today.ToString("MMM", CultureInfo.InvariantCulture);
                    string nextmonth = nxtmnt.ToString("MMM", CultureInfo.InvariantCulture);
                    string next2month = nxt2mnt.ToString("MMM", CultureInfo.InvariantCulture);
                    if (type == 1)
                    { worksheet.Cell(3, 2).Value = "Birthday"; }
                    if (type == 2)
                    {
                        worksheet.Cell(3, 2).Value = "M.Anniversary";
                    }
                    if (type == 3)
                    {
                        worksheet.Cell(3, 2).Value = "Enr Anniversary";
                    }
                    worksheet.Cell(4, 1).Value = "Month";

                    if (month == 1)
                    { worksheet.Cell(4, 2).Value = currentmonth; }
                    if (month == 2)
                    {
                        worksheet.Cell(4, 2).Value = nextmonth;
                    }
                    if (month == 3)
                    {
                        worksheet.Cell(4, 2).Value = next2month;
                    }
                    worksheet.Cell(6, 1).InsertTable(table);
                    //wb.Worksheets.Add(table);

                    using (MemoryStream stream = new MemoryStream())
                    {

                        wb.SaveAs(stream);

                        if (EmailId != "")
                        {

                            RR.email_send(EmailId, ReportName, stream.ToArray(), userDetails.EmailId);

                        }

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelCelebrations");
                return null;
            }


        }
        public ActionResult ProfitableCustomer()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<SelectListItem> ListFilter = new List<SelectListItem>();
            SelectListItem newItem = new SelectListItem();
            newItem.Value = "50";
            newItem.Text = "50";
            ListFilter.Add(newItem);

            SelectListItem newItem1 = new SelectListItem();
            newItem1.Value = "100";
            newItem1.Text = "100";
            ListFilter.Add(newItem1);

            SelectListItem newItem2 = new SelectListItem();
            newItem2.Value = "500";
            newItem2.Text = "500";
            ListFilter.Add(newItem2);

            SelectListItem newItem3 = new SelectListItem();
            newItem3.Value = "1000";
            newItem3.Text = "1000";
            ListFilter.Add(newItem3);

            ViewBag.ListFilter = ListFilter;
            return View();
        }
        [HttpPost]
        public JsonResult GetProfitableCustomersResult(string CountOrBusiness, string Count)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<MemberList> lstMember = new List<MemberList>();
            try
            {
                lstMember = RR.GetMemberList(userDetails.GroupId, "", userDetails.connectionString, userDetails.LoginId, "", "", "", "", "", "");
                int CountNumber = Convert.ToInt32(Count);
                if (CountOrBusiness == "Count")
                {
                    lstMember = lstMember.OrderByDescending(x => x.TxnCount).Take(CountNumber).ToList();
                }
                if (CountOrBusiness == "Business")
                {
                    lstMember = lstMember.OrderByDescending(x => x.TotalSpend).Take(CountNumber).ToList();
                }
                var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
                if (!GroupDetails.IsMasked)
                {
                    foreach (var item in lstMember)
                    {
                        item.MaskedMobileNo = item.MobileNo;
                    }
                }

                long? apts = 0;
                long btxn = 0;
                long txnCount = 0;
                long bpts = 0;
                long totalSpend = 0;
                foreach (var item in lstMember)
                {
                    txnCount += item.TxnCount;
                    totalSpend += item.TotalSpend;
                    btxn += item.TotalBurnTxn;
                    bpts += item.TotalBurnPoints;
                    apts += item.AvlBalPoints;
                }
                MemberList objTotal = new MemberList();
                objTotal.EnrooledOutlet = "<b>Total</b>";
                objTotal.MobileNo = "";
                objTotal.MemberName = "";
                objTotal.TxnCount = txnCount;
                objTotal.TotalSpend = totalSpend;
                objTotal.TotalBurnTxn = btxn;
                objTotal.TotalBurnPoints = bpts;
                objTotal.AvlBalPoints = apts;
                objTotal.MobileNo = "";
                objTotal.MaskedMobileNo = "";
                lstMember.Add(objTotal);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProfitableCustomersResult");
                return null;
            }

            return new JsonResult() { Data = lstMember, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ExportToProfitableCustomers(string CountOrBusiness, string Count, string ReportName, string EmailId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                List<MemberList> lstMember = new List<MemberList>();
                lstMember = RR.GetMemberList(userDetails.GroupId, "", userDetails.connectionString, userDetails.LoginId, "", "", "", "", "", "");
                int CountNumber = Convert.ToInt32(Count);
                if (CountOrBusiness == "Count")
                {
                    lstMember = lstMember.OrderByDescending(x => x.TxnCount).Take(CountNumber).ToList();
                }
                if (CountOrBusiness == "Business")
                {
                    lstMember = lstMember.OrderByDescending(x => x.TotalSpend).Take(CountNumber).ToList();
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
                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalSpend"])))
                    {
                        dr["TotalSpendStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalSpend"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["AvlBalPoints"])))
                    {
                        dr["AvlBalPointsStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["AvlBalPoints"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TotalBurnPoints"])))
                    {
                        dr["TotalBurnPointsStr"] = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(dr["TotalBurnPoints"]));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LastTxnDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["LastTxnDate"] = DateTime.ParseExact(Convert.ToString(dr["LastTxnDate"]), "MM/dd/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["EnrolledDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["EnrolledDate"] = DateTime.ParseExact(Convert.ToString(dr["EnrolledDate"]), "MM/dd/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }

                table.Columns.Remove("TotalSpend");
                table.Columns.Remove("AvlBalPoints");
                table.Columns.Remove("TotalBurnPoints");
                table.Columns.Remove("txnDate");
                table.Columns["TotalSpendStr"].ColumnName = "TotalSpend";
                table.Columns["AvlBalPointsStr"].ColumnName = "AvlBalPoints";
                table.Columns["TotalBurnPointsStr"].ColumnName = "TotalBurnPoints";
                if (userDetails.LoginType == "1" || userDetails.LoginType == "6" || userDetails.LoginType == "7")
                {
                    table.Columns.Remove("MaskedMobileNo");
                }
                else
                {
                    var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
                    if (!GroupDetails.IsMasked)
                    {
                        table.Columns.Remove("MaskedMobileNo");
                    }
                    else
                    {
                        table.Columns.Remove("MobileNo");
                        table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                    }
                }
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;

                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Profitable Customers";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Filter";
                    if (CountOrBusiness == "Count")
                    {
                        worksheet.Cell(3, 2).Value = "Top" + Count + "Members as per Txn Count";
                    }
                    if (CountOrBusiness == "Business")
                    {
                        worksheet.Cell(3, 2).Value = "Top" + Count + "Members as per Business";
                    }
                    worksheet.Cell(5, 1).InsertTable(table);
                    //wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        if (EmailId != "")
                        {
                            RR.email_send(EmailId, ReportName, stream.ToArray(), userDetails.EmailId);

                        }
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);


                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToProfitableCustomers");
                return null;
            }


        }
        public ActionResult ExportToExcelCreateOwnReport(string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();

            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                CreateOwnReportViewModel objmodelview = new CreateOwnReportViewModel();
                objmodelview.lstcolumnlist = (List<string>)Session["ExportColumnList"];

                if (Session["ExportCustomerReport"] != null)
                {

                    List<CustomerTypeReport> lstcustreport = (List<CustomerTypeReport>)Session["ExportCustomerReport"];
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(CustomerTypeReport));
                    foreach (PropertyDescriptor prop in properties)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                    foreach (CustomerTypeReport item in lstcustreport)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                        table.Rows.Add(row);
                    }

                }
                else if (Session["ExportTransactionReport"] != null)
                {
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(TransactionTypeReport));
                    foreach (PropertyDescriptor prop in properties)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                    List<TransactionTypeReport> lsttransactionreport = (List<TransactionTypeReport>)Session["ExportTransactionReport"];
                    foreach (TransactionTypeReport item in lsttransactionreport)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                        table.Rows.Add(row);
                    }
                }

                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = ReportName;
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
                newexception.AddException(ex, "ExportToExcelCreateOwnReport");
                return null;
            }
        }
        public ActionResult MemberSearchExportToExcel(string mobileNo, string groupId)
        {
            CustomerRepository objCustRepo = new CustomerRepository();
            MemberSearch objMemberSearch = new MemberSearch();
            System.Data.DataTable table = new System.Data.DataTable();

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string loginId = string.Empty;
            if (userDetails.LevelIndicator == "03" || userDetails.LevelIndicator == "04")
            {
                loginId = userDetails.LoginId;
            }
            if (!string.IsNullOrEmpty(groupId) && groupId != "undefined")
            {
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                objMemberSearch = RR.GetMeamberSearchData(groupId, mobileNo, connStr, loginId);
            }
            else
            {
                objMemberSearch = RR.GetMeamberSearchData(userDetails.GroupId, mobileNo, userDetails.connectionString, loginId);
            }


            string fileName = "MemberTransactions_" + mobileNo + ".xlsx";
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(MemberSearchTxn));
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (MemberSearchTxn item in objMemberSearch.lstMemberSearchTxn)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            foreach (DataRow item in table.Rows)
            {
                if (string.IsNullOrEmpty(Convert.ToString(item["TxnUpdateDate"])))
                {
                    item["TxnUpdateDate"] = "";
                }
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                table.TableName = fileName;
                IXLWorksheet worksheet = wb.AddWorksheet(sheetName: mobileNo);
                //worksheet.Cell(1, 1).Value = "Report Name";
                //worksheet.Cell(1, 2).Value = mobileNo;
                worksheet.Cell(1, 1).InsertTable(table);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        public ActionResult SmartSlicer()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            CustomizeMetricsViewModel objData = new CustomizeMetricsViewModel();
            objData.GroupId = userDetails.GroupId;
            objData.lstCategory = ORR.GetCategoryCode(userDetails.GroupId, userDetails.connectionString);
            objData.lstSubCategory = ORR.GetSubCategoryCodeALL(userDetails.GroupId, userDetails.connectionString);
            objData.lstProduct = ORR.GetProductId(userDetails.GroupId, userDetails.connectionString);
            objData.lstBrands = RR.GetBrandList(userDetails.GroupId, userDetails.connectionString);
            objData.lstOutlets = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            objData.lstTiers = ORR.GetTierList(userDetails.connectionString);
            objData.lstDLCSource = ORR.GetDLCSourceList(userDetails.connectionString);
            objData.lstCRDataset = RR.GetCRDataset(userDetails.connectionString);
            SelectListItem item = new SelectListItem();
            item.Value = "0";
            item.Text = "All";
            objData.lstOutlets.Insert(0, item);
            return View(objData);
        }

        [HttpPost]
        public JsonResult GetSSFilterCount(string jsonData)
        {
            bool IsCount = true;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            var count = RR.GetSSFilterCount(objData, userDetails.LoginId, IsCount, userDetails.connectionString, userDetails.GroupId);

            return new JsonResult() { Data = count, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult SaveDataset(string jsonData, string DSName)
        {
            string status = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            status = RR.CheckCRDatasetExist(DSName, userDetails.connectionString);
            if (status != "Exist")
                status = RR.SaveDataset(objData, userDetails, DSName, userDetails.connectionString);

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetSlicerReport(string jsonData, string columnslist)
        {            
            CreateOwnReportViewModel createownviewmodel = new CreateOwnReportViewModel();
            List<CustomerDetail> lstcustomerdetails = (List<CustomerDetail>)Session["customerId"];
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
           
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            object[] columnData = (object[])json_serializer.DeserializeObject(columnslist);
            List<string> lstcolumnlist = new List<string>();
            List<string> lstcolumnIdlist = new List<string>();
            string columns = string.Empty;
            foreach (Dictionary<string, object> item in columnData)
            {
                object[] ColumnId = (object[])item["ColumnId"];
                object[] columnname = (object[])item["columnnm"];
                
                foreach (var itemNew1 in (columnname))
                {
                    string name = Convert.ToString(itemNew1);
                    lstcolumnlist.Add(name);                    
                }
                
                foreach (var itemNew1 in (ColumnId))
                {
                    //foreach (Dictionary<string, object> itemNew2 in itemNew1)
                    //{
                        string name = Convert.ToString(itemNew1);//string.Concat(Convert.ToString(itemNew1).Where(c => !char.IsWhiteSpace(c)));
                        lstcolumnIdlist.Add(name);
                        columns += itemNew1 + ",";
                    //}
                }
            }

            columns = columns.Remove(columns.Length - 1);
            createownviewmodel.listCustR = RR.GetSSFilterReport(objData, columns, userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.lstcolumnlist = lstcolumnlist;
            createownviewmodel.lstcolumnIdlist = lstcolumnIdlist;
            Session["ExportColumnList"] = createownviewmodel.lstcolumnlist;

            return PartialView("_CreateOwnReportCustomerWise", createownviewmodel);            
        }
    
        public ActionResult SuperSaverReport()
        {
            SSvsNonSSViewModel objData = new SSvsNonSSViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData.objSSNonSSReport = RR.GetSSNonSSReport(userDetails.connectionString);
            objData.lstCategory = ORR.GetCategoryCode(userDetails.GroupId, userDetails.connectionString);
            objData.lstSubCategory = ORR.GetSubCategoryCodeALL(userDetails.GroupId, userDetails.connectionString);
            return View(objData);
        }

        public ActionResult VelocityReport()
        {
            VelocityReportViewModel objModel = new VelocityReportViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objModel.lstMain = RR.GetVelocityMains(userDetails.connectionString);
            //objModel.lstMonthwise = RR.GetVelocityMonthwise(userDetails.connectionString);
            return View(objModel);
        }

        [HttpPost]
        public ActionResult GetVelocityCustomerData(string count)
        {
             
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var customerData = RR.GetVelocityCustomerData(count, userDetails.connectionString);
            long? Total = 0;
            foreach (var item in customerData)
            {
                Total = Total + item.TotalAmtSpend;
            }
            ViewBag.Total = Total;
            return PartialView("_VelocityCustomerData", customerData);
        }
        public ActionResult VelocityCustomerDataExportToExcel(string frequency)
        {
            List<tblVelocityCustomerData> lstData = new List<tblVelocityCustomerData>();            
            System.Data.DataTable table = new System.Data.DataTable();

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            lstData = RR.GetVelocityCustomerData(frequency, userDetails.connectionString);
            string fileName = "VelocityCustomerData_Frequency_" + frequency + ".xlsx";
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(tblVelocityCustomerData));
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Name != "SlNo")
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (tblVelocityCustomerData item in lstData)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Name != "SlNo")
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;                  
                }
                table.Rows.Add(row);
            }           

            using (XLWorkbook wb = new XLWorkbook())
            {
                table.TableName = fileName;
                IXLWorksheet worksheet = wb.AddWorksheet(sheetName: "CustomerData");                
                worksheet.Cell(1, 1).InsertTable(table);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        [HttpPost]
        public JsonResult GetSubCategory(string CatCode)
        {
            if (CatCode == "")
            {
                CatCode = "0000";
            }
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var SubCatData= ORR.GetSubCategoryCode(userDetails.connectionString, CatCode);

            return new JsonResult() { Data = SubCatData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetCategorySubCategoryReport(string CatCode, string SubCatCode)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var ReportData = RR.GetSSNonSSCatSubCatReport(userDetails.connectionString, CatCode, SubCatCode);

            ReportData.TotalBusinessStr = String.Format(new CultureInfo("en-US", false), "{0:n0}", Convert.ToDouble(ReportData.TotalBusiness));
            ReportData.TotalBusinessSSStr = String.Format(new CultureInfo("en-US", false), "{0:n0}", Convert.ToDouble(ReportData.TotalBusinessSS));
            ReportData.TotalBusinessNonSSStr = String.Format(new CultureInfo("en-US", false), "{0:n0}", Convert.ToDouble(ReportData.TotalBusinessNonSS));

            return new JsonResult() { Data = ReportData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetSlicerReportFromDS(string DSId, string columnslist)
        {
            CreateOwnReportViewModel createownviewmodel = new CreateOwnReportViewModel();
            List<CustomerDetail> lstcustomerdetails = (List<CustomerDetail>)Session["customerId"];
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;            

            object[] columnData = (object[])json_serializer.DeserializeObject(columnslist);
            List<string> lstcolumnlist = new List<string>();
            List<string> lstcolumnIdlist = new List<string>();
            string columns = string.Empty;
            foreach (Dictionary<string, object> item in columnData)
            {
                object[] ColumnId = (object[])item["ColumnId"];
                object[] columnname = (object[])item["columnnm"];

                foreach (var itemNew1 in (columnname))
                {
                    string name = Convert.ToString(itemNew1);
                    lstcolumnlist.Add(name);
                }

                foreach (var itemNew1 in (ColumnId))
                {
                    string name = Convert.ToString(itemNew1);//string.Concat(Convert.ToString(itemNew1).Where(c => !char.IsWhiteSpace(c)));
                    lstcolumnIdlist.Add(name);
                    columns += itemNew1 + ",";
                }
            }

            columns = columns.Remove(columns.Length - 1);
            createownviewmodel.listCustR = RR.GetSSFilterReportFromDS(DSId, columns, userDetails.GroupId, userDetails.connectionString);
            createownviewmodel.lstcolumnlist = lstcolumnlist;
            createownviewmodel.lstcolumnIdlist = lstcolumnIdlist;
            Session["ExportColumnList"] = createownviewmodel.lstcolumnlist;

            return PartialView("_CreateOwnReportCustomerWise", createownviewmodel);
        }

        [HttpPost]
        public JsonResult GetDSQueryCount(string DSId)
        {           
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var count = RR.GetSavedDSCount(DSId, userDetails.connectionString);
            return new JsonResult() { Data = count, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}