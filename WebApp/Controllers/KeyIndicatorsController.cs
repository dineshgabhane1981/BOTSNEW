using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using ClosedXML.Excel;

namespace WebApp.Controllers
{
    public class KeyIndicatorsController : Controller
    {
        KeyIndecatorsRepository KR = new KeyIndecatorsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository CR = new CustomerRepository();
        // GET: KeyIndicators
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OnlyOnce()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletList = lstOutlet;
            return View();
        }

        public ActionResult NonTransacting()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletList = lstOutlet;
            return View();
        }

        public ActionResult NonRedeeming()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            NonRedemptionCls objNonRedemptionCls = new NonRedemptionCls();
            objNonRedemptionCls = KR.GetNonRedemptionData(userDetails.GroupId, userDetails.connectionString);
            return View(objNonRedemptionCls);
        }

        public ActionResult MemberWebPage()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            MemberWebPage objMemberWebPage = new MemberWebPage();
            objMemberWebPage = KR.GetMemberWebPageData(userDetails.GroupId, userDetails.connectionString);

            List<long> memberbasedataList = new List<long>();
            memberbasedataList.Add(objMemberWebPage.TotalMember);
            memberbasedataList.Add(objMemberWebPage.ReferringBase);
            var lstMemberBaseData = string.Join(",", memberbasedataList);

            List<long> memberbaseProfileList = new List<long>();
            memberbaseProfileList.Add(objMemberWebPage.TotalMember);
            memberbaseProfileList.Add(objMemberWebPage.NoofProfileUpdate);
            var lstMemberBaseProfileData = string.Join(",", memberbaseProfileList);

            List<long> memberbaseGiftList = new List<long>();
            memberbaseGiftList.Add(objMemberWebPage.TotalMember);
            memberbaseGiftList.Add(objMemberWebPage.GiftPointsCount);
            var lstMemberBaseGiftData = string.Join(",", memberbaseGiftList);

            List<long> memberbaseGiftOptOutList = new List<long>();
            memberbaseGiftOptOutList.Add(objMemberWebPage.ProgramOtpOut);
            memberbaseGiftOptOutList.Add(objMemberWebPage.PromoSMSOtpOut);
            var lstMemberBaseOptOutData = string.Join(",", memberbaseGiftOptOutList);

            List<string> referralnameList = new List<string>();
            referralnameList.Add("'Issued'");
            referralnameList.Add("'Redeemed'");
            referralnameList.Add("'Expired'");
            referralnameList.Add("'Unused'");
            var lstreferralNames = string.Join(",", referralnameList);

            List<long> referraldataList = new List<long>();
            referraldataList.Add(objMemberWebPage.ReferralPointsIssued);
            referraldataList.Add(objMemberWebPage.ReferralPointsRedeem);
            referraldataList.Add(objMemberWebPage.ReferralPointsExpired);
            referraldataList.Add(objMemberWebPage.ReferralPointsUnused);
            var lstreferraldata = string.Join(",", referraldataList);

            ViewBag.ReferralNames = lstreferralNames.Trim();
            ViewBag.ReferralData = lstreferraldata.Trim();
            ViewBag.MemberbaseProfileData = lstMemberBaseProfileData.Trim();
            ViewBag.MemberbaseData = lstMemberBaseData.Trim();
            ViewBag.MemberbaseGiftData = lstMemberBaseGiftData.Trim();
            ViewBag.MemberbaseOptOutData = lstMemberBaseOptOutData.Trim();

            return View(objMemberWebPage);
        }

        public ActionResult MemberPage()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            MemberPage objMemberPage = new MemberPage();
            objMemberPage = KR.GetMemberPageData(userDetails.GroupId, userDetails.connectionString);
            return View(objMemberPage);
        }
        [HttpPost]
        public JsonResult GetReferingBaseData(string type)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            List<MemberPageRefData> objMemberPageRefData = new List<MemberPageRefData>();
            objMemberPageRefData = KR.GetMemberPageRefData(userDetails.GroupId, type, userDetails.connectionString);
            return new JsonResult() { Data = objMemberPageRefData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetOnlyOnceResult(string outletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            OnlyOnce objOnlyOnce = new OnlyOnce();
            objOnlyOnce = KR.GetOnlyOnceData(userDetails.GroupId, outletId, userDetails.connectionString);

            objOnlyOnce.TotalMemberStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.TotalMember));
            objOnlyOnce.OnlyOnceMemberStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.OnlyOnceMember));
            objOnlyOnce.RecentVisitHighStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.RecentVisitHigh));
            objOnlyOnce.RecentVisitLowStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.RecentVisitLow));
            objOnlyOnce.NotSeenHighStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.NotSeenHigh));
            objOnlyOnce.NotSeenLowStr = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(objOnlyOnce.NotSeenLow));

            return new JsonResult() { Data = objOnlyOnce, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetOnlyOnceTxnResult(string outletId, string type)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            List<OnlyOnceTxn> objOnlyOnceTxn = new List<OnlyOnceTxn>();
            objOnlyOnceTxn = KR.GetOnlyOnceTxnData(userDetails.GroupId, outletId, type, userDetails.connectionString);

            var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
            if (!GroupDetails.IsMasked)
            {
                foreach (var item in objOnlyOnceTxn)
                {
                    item.MaskedMobileNo = item.MobileNo;
                }
            }
            return new JsonResult() { Data = objOnlyOnceTxn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportToExcelOnlyOnce(string outletId, string type, string ReportName, string EmailId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<OnlyOnceTxn> objOnlyOnceTxn = new List<OnlyOnceTxn>();
                objOnlyOnceTxn = KR.GetOnlyOnceTxnData(userDetails.GroupId, outletId, type, userDetails.connectionString);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(OnlyOnceTxn));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (OnlyOnceTxn item in objOnlyOnceTxn)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LastTxnDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["LastTxnDate"] = DateTime.ParseExact(Convert.ToString(dr["LastTxnDate"]), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }
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
                //table.Columns.Remove("OutletId");
                //table.Columns.Remove("MobileNo");
                //table.Columns["MaskedMobileNo"].ColumnName = "MobileNo";
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "OnlyOnce";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Filter";
                    string category = "";
                    if (type == "1")
                    {
                        category = "High Spend, Recent Member";
                    }
                    if (type == "2")
                    {
                        category = "Low Spend, Recent Member";
                    }
                    if (type == "3")
                    {
                        category = "High Spend, Long time no see member";
                    }
                    if (type == "4")
                    {
                        category = "Low Spend, Long time no see member";
                    }
                    if (type == "5")
                    {
                        category = "All";
                    }
                    worksheet.Cell(3, 2).Value = category;
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
                return null;
            }


        }
        [HttpPost]
        public JsonResult GetNonTransactingResult(string outletId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            NonTransactingCls objNonTransactingCls = new NonTransactingCls();
            objNonTransactingCls = KR.GetNonTransactingData(userDetails.GroupId, outletId, userDetails.connectionString);
            return new JsonResult() { Data = objNonTransactingCls, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetNonTransactingTxnResult(string outletId, string type)
        {
            string barType = "";
            if (type.Equals("Within 30 days"))
                barType = "1";
            if (type.Equals("31 to 60 days"))
                barType = "2";
            if (type.Equals("61 to 90 days"))
                barType = "3";
            if (type.Equals("91 to 180 days"))
                barType = "4";
            if (type.Equals("181 to 365 days"))
                barType = "5";
            if (type.Equals("More than a year"))
                barType = "6";

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (outletId.Equals("All"))
            {
                outletId = "";
            }
            List<NonTransactingTxn> objNonTransactingTxn = new List<NonTransactingTxn>();
            objNonTransactingTxn = KR.GetNonTransactingTxnData(userDetails.GroupId, outletId, barType, userDetails.connectionString);
            var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
            if (!GroupDetails.IsMasked)
            {
                foreach (var item in objNonTransactingTxn)
                {
                    item.MaskedMobileNo = item.MobileNo;
                }
            }

            return new JsonResult() { Data = objNonTransactingTxn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetNonRedemptionTxnResult(int type, int daysType)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<NonRedemptionTxn> objNonRedemptionTxn = new List<NonRedemptionTxn>();
            objNonRedemptionTxn = KR.GetNonRedemptionTxnData(userDetails.GroupId, type, daysType, userDetails.connectionString);
            var GroupDetails = CR.GetGroupDetails(Convert.ToInt32(userDetails.GroupId));
            if (!GroupDetails.IsMasked)
            {
                foreach (var item in objNonRedemptionTxn)
                {
                    item.MaskedMobileNo = item.MobileNo;
                }
            }

            return new JsonResult() { Data = objNonRedemptionTxn, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetNewRegistrationList(string SourceId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<MemberPageNewRegisterationData> lstnewregdata = new List<MemberPageNewRegisterationData>();
            lstnewregdata = KR.GetNewRegistrationData(userDetails.GroupId, SourceId, userDetails.connectionString);
            return new JsonResult() { Data = lstnewregdata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetMemberMisinformationData()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            MembersInformation objMembersInformation = new MembersInformation();
            objMembersInformation = KR.GetMemberMisinformationData(userDetails.GroupId, userDetails.connectionString);
            return new JsonResult() { Data = objMembersInformation, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportToExcelNonTransacting(string outletId, string type, string ReportName, string EmailId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                string barType = "";
                if (type.Equals("Within 30 days"))
                    barType = "1";
                if (type.Equals("31 to 60 days"))
                    barType = "2";
                if (type.Equals("61 to 90 days"))
                    barType = "3";
                if (type.Equals("91 to 180 days"))
                    barType = "4";
                if (type.Equals("181 to 365 days"))
                    barType = "5";
                if (type.Equals("More than a year"))
                    barType = "6";
                List<NonTransactingTxn> objNonTransactingTxn = new List<NonTransactingTxn>();
                objNonTransactingTxn = KR.GetNonTransactingTxnData(userDetails.GroupId, outletId, barType, userDetails.connectionString);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(NonTransactingTxn));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (NonTransactingTxn item in objNonTransactingTxn)
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
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LastTxnDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["LastTxnDate"] = DateTime.ParseExact(Convert.ToString(dr["LastTxnDate"]), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "NonTransacting";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "NonTransacting from";
                    worksheet.Cell(3, 2).Value = type;
                    worksheet.Cell(6, 1).InsertTable(table);
                    // wb.Worksheets.Add(table);
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
                return null;
            }
        }

        public ActionResult ExportToExcelNonRedemption(int type, int daysType, string ReportName, string EmailId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<NonRedemptionTxn> objNonRedemptionTxn = new List<NonRedemptionTxn>();
                objNonRedemptionTxn = KR.GetNonRedemptionTxnData(userDetails.GroupId, type, daysType, userDetails.connectionString);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(NonRedemptionTxn));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (NonRedemptionTxn item in objNonRedemptionTxn)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                foreach (DataRow dr in table.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LastTxnDate"])))
                    {
                        //dr["TxnDatetime"] = Convert.ToDateTime(dr["TxnDatetime"]).ToString("MM/dd/yyyy");
                        dr["LastTxnDate"] = DateTime.ParseExact(Convert.ToString(dr["LastTxnDate"]), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
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
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "NonRedemption";
                    worksheet.Cell(2, 1).Value = "Date";
                    worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                    worksheet.Cell(3, 1).Value = "Duration";
                    string pointbalance = "";
                    if (type == 1)
                    { pointbalance = "High"; }
                    if (type == 2)
                    { pointbalance = "Medium"; }
                    if (type == 3)
                    { pointbalance = "Low"; }
                    string days = "";
                    if (daysType == 1)
                    {
                        days = "Less than 90 days";
                    }
                    if (daysType == 2)
                    {
                        days = "Between 90 & 180";
                    }
                    if (daysType == 3)
                    {
                        days = "More than 180 days";
                    }
                    worksheet.Cell(3, 2).Value = pointbalance + "-" + days;
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
                return null;
            }
        }

        public ActionResult DLCCreation()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<DLCCreation> lstData = new List<DLCCreation>();
            lstData = KR.GetDLCCreationData(userDetails.GroupId);
            return View(lstData);
        }
    }
}