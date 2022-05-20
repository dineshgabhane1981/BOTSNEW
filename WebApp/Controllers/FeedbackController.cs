using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;
using BOTS_BL;
using WebApp.App_Start;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Xml;
using BOTS_BL.Models.FeedbackModule;
using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace WebApp.Controllers
{
    public class FeedbackController : Controller
    {
        Exceptions newexception = new Exceptions();
        FeedbackModuleRepository FMR = new FeedbackModuleRepository();
        ReportsRepository RR = new ReportsRepository();
        // GET: Feedback
        public ActionResult Index()
        {
            FeedbackConfigViewModel objAllData = new FeedbackConfigViewModel();
            objAllData.lstNeverOptFor = FMR.GetNeverOptForGroups();
            objAllData.lstActiveGroup = FMR.GetActiveGroups();
            objAllData.lstDeActivatedGroup = FMR.GetDeActivatedGroups();
            return View(objAllData);
        }
        public ActionResult EnableFeedbackModule(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string GroupId = Convert.ToString(item["GroupId"]);
                string Fees = Convert.ToString(item["Fees"]);
                string StartDate = Convert.ToString(item["StartDate"]);
                string PaymentMode = Convert.ToString(item["PaymentMode"]);

                Feedback_FeedbackConfig objFeedback = new Feedback_FeedbackConfig();
                objFeedback.GroupId = Convert.ToInt32(GroupId);
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;
                objFeedback.StartDate = Convert.ToDateTime(StartDate).Date;
                objFeedback.EndDate = Convert.ToDateTime(StartDate).AddDays(364).Date;
                objFeedback.AddedBy = userDetails.LoginId;
                objFeedback.AddedDate = DateTime.Now;
                objFeedback.Status = "Active";
                //Get XML File and send all content with this call to insert
                var masterData = FMR.GetFeedbackMasterData();
                List<Feedback_Content> lstData = new List<Feedback_Content>();
                foreach (var newItem in masterData)
                {
                    Feedback_Content objNew = new Feedback_Content();
                    objNew.GroupId = GroupId;
                    objNew.Section = newItem.Section;
                    objNew.Type = newItem.Type;
                    objNew.TypeId = newItem.TypeId;
                    objNew.Text = newItem.Text;
                    objNew.IsDisplay = newItem.IsDisplay;
                    objNew.IsMandatory = Convert.ToString(newItem.IsMandatory);
                    objNew.AddedDate = DateTime.Now;
                    objNew.AddedBy = userDetails.LoginId;
                    lstData.Add(objNew);
                }

                status = FMR.EnableFeedbackModule(objFeedback, lstData);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult StopFeedbackModule(string GroupId, string Reason)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            Feedback_FeedbackConfig objFeedback = new Feedback_FeedbackConfig();

            objFeedback = FMR.GetFeedbackByGroupId(GroupId);
            objFeedback.GroupId = Convert.ToInt32(GroupId);
            objFeedback.StoppedReason = Reason;
            objFeedback.EndDate = DateTime.Today.Date;
            objFeedback.AddedBy = userDetails.LoginId;
            objFeedback.StoppedDate = DateTime.Now;
            objFeedback.Status = "Stop";
            status = FMR.EnableFeedbackModule(objFeedback, null);


            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult RenewFeedbackModule(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string GroupId = Convert.ToString(item["GroupId"]);
                string Fees = Convert.ToString(item["Fees"]);
                string StartDate = Convert.ToString(item["StartDate"]);
                string PaymentMode = Convert.ToString(item["PaymentMode"]);

                Feedback_FeedbackConfig objFeedback = new Feedback_FeedbackConfig();

                objFeedback = FMR.GetFeedbackByGroupId(GroupId);
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;
                objFeedback.StartDate = Convert.ToDateTime(StartDate).Date;
                objFeedback.EndDate = Convert.ToDateTime(StartDate).AddDays(364).Date;
                objFeedback.AddedBy = userDetails.LoginId;
                objFeedback.RenewDate = DateTime.Now;
                objFeedback.Status = "Renew";
                status = FMR.EnableFeedbackModule(objFeedback, null);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult RenewActiveFeedbackModule(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string GroupId = Convert.ToString(item["GroupId"]);
                string Fees = Convert.ToString(item["Fees"]);
                //string StartDate = Convert.ToString(item["StartDate"]);
                string PaymentMode = Convert.ToString(item["PaymentMode"]);

                Feedback_FeedbackConfig objFeedback = new Feedback_FeedbackConfig();

                objFeedback = FMR.GetFeedbackByGroupId(GroupId);
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;
                objFeedback.EndDate = objFeedback.EndDate.Value.AddDays(364).Date;
                objFeedback.RenewDate = DateTime.Now;
                objFeedback.Status = "Renew";
                status = FMR.EnableFeedbackModule(objFeedback, null);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult BindNeverOpt()
        {
            var lstNeverOptFor = FMR.GetNeverOptForGroups();
            return PartialView("_NeverOptedFor", lstNeverOptFor);
        }
        public ActionResult BindActive()
        {
            var lstActiveGroup = FMR.GetActiveGroups();
            return PartialView("_ActiveGroup", lstActiveGroup);
        }
        public ActionResult BindDeActivated()
        {
            var lstDeActivatedGroup = FMR.GetDeActivatedGroups();
            return PartialView("_DeActiveGroup", lstDeActivatedGroup);
        }

        public ActionResult CreateFeedback()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            FeedbackAuthorViewModel objFeedbackAuthor = new FeedbackAuthorViewModel();
            Feedback_PointsAndMessages PointsAndMessages = new Feedback_PointsAndMessages();
            List<OutletDetailsViewModel> objOutletData = new List<OutletDetailsViewModel>();
            try
            {
                objFeedbackAuthor.GroupId = userDetails.GroupId;
                objFeedbackAuthor.lstFeedbackData = FMR.GetFeedback_Contents(userDetails.GroupId);
                objFeedbackAuthor.lstOutletDetail = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
                foreach (var item in objFeedbackAuthor.lstOutletDetail)
                {
                    OutletDetailsViewModel objOT = new OutletDetailsViewModel();
                    objOT.mobileNos = FMR.GetOutletMobileNos(userDetails.GroupId, item.Value);
                    objOT.outletId = item.Value;
                    objOT.outletName = item.Text;

                    objOutletData.Add(objOT);
                }
                objFeedbackAuthor.lstOutletData = objOutletData;
                PointsAndMessages = FMR.GetPointsAndMessages(userDetails.GroupId);
                objFeedbackAuthor.PointsAndMessages = PointsAndMessages;
                objFeedbackAuthor.lstKnowAboutUs = FMR.GetHowToKnowAboutList();
                objFeedbackAuthor.LogoUrl = FMR.GetLogo(userDetails.GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CreateFeedback");
            }
            return View(objFeedbackAuthor);
        }

        public ActionResult GetFeedBack(string Id)
        {
            string groupid = string.Empty;
            if (!string.IsNullOrEmpty(Id))
            {
                groupid = Id.Substring(0, 4);
                ViewBag.GroupId = groupid;
                ViewBag.OutletId = Id;
                // ViewBag.lsthowtoknow = FMR.GetHowToKnowAboutList();
            }
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            FeedbackGetFeedbackViewModel objgetfeedbackviewmodel = new FeedbackGetFeedbackViewModel();
            Feedback_PointsAndMessages PointsAndMessages = new Feedback_PointsAndMessages();
            
            // List<Feedback_Content> lstfbget = new List<Feedback_Content>();
            objgetfeedbackviewmodel.OutletId = Id;
            objgetfeedbackviewmodel.GroupId = groupid;
            objgetfeedbackviewmodel.IsExpiredOrStopped = FMR.CheckActiveLink(groupid);
            objgetfeedbackviewmodel.GroupName = FMR.GetGroupName(groupid);
            objgetfeedbackviewmodel.lstFeedbackData = FMR.GetFeedback_VisibleContents(groupid);
            objgetfeedbackviewmodel.LogoUrl = FMR.GetLogo(groupid);
            objgetfeedbackviewmodel.lstKnowAboutUs = FMR.GetHowToKnowAboutList();
            objgetfeedbackviewmodel.lstsalesRepresentive = FMR.GetSalesRepresentiveList(groupid);
            PointsAndMessages = FMR.GetPointsAndMessages(groupid);
            objgetfeedbackviewmodel.PointsAndMessages = PointsAndMessages;

            objgetfeedbackviewmodel.lstKnowAboutUs = FMR.GetHowToKnowAboutList();

            return View(objgetfeedbackviewmodel);
        }

        public ActionResult UpdateFeedbackDetails(string HomeData, string QuestionData, string OtherInfoData, string OtherConfigData, string OutletMobileNos)
        {
            bool status = false;

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objHomeData = (object[])json_serializer.DeserializeObject(HomeData);
            object[] objQuestionData = (object[])json_serializer.DeserializeObject(QuestionData);
            object[] objOtherInfoData = (object[])json_serializer.DeserializeObject(OtherInfoData);
            object[] objOtherConfigData = (object[])json_serializer.DeserializeObject(OtherConfigData);
            object[] objOutletMobileNos = (object[])json_serializer.DeserializeObject(OutletMobileNos);

            status = FMR.UpdateFeedbackDetails(objHomeData, objQuestionData, objOtherInfoData, objOtherConfigData, objOutletMobileNos, userDetails.GroupId, userDetails.LoginId);


            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult Dashboard()
        {
            FeedbackDashboardViewModel objData = new FeedbackDashboardViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData.feedbackConfig = FMR.GetPointsAndMessages(userDetails.GroupId);
            if (objData.feedbackConfig != null)
            {
                if (objData.feedbackConfig.IsAddRepresentative)
                    objData.SRChart = true;
                else
                    objData.SRChart = false;

            }
            else
                objData.SRChart = false;
            objData.lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            objData.NoOfFeedback = FMR.GetFeedbackCountByGroupId(userDetails.GroupId);
            objData.objConfig= FMR.GetFeedbackByGroupId(userDetails.GroupId);
            return View(objData);
        }

        public JsonResult DashboardNewData(string OutletId, string FromDt, string ToDT)
        {
            List<int> lstData = new List<int>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                lstData = FMR.GetDashboardNewExistingData(userDetails.GroupId, OutletId, FromDt, ToDT, "New");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult DashboardExistingData(string OutletId, string FromDt, string ToDT)
        {
            List<int> lstData = new List<int>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                lstData = FMR.GetDashboardNewExistingData(userDetails.GroupId, OutletId, FromDt, ToDT, "Existing");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult DashboardOutletWiseData(string OutletId, string FromDt, string ToDT)
        {
            List<object> lstData = new List<object>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                var lstOutletWise = FMR.GetOutletWiseData(userDetails.GroupId, OutletId, FromDt, ToDT);
                List<string> nameList = new List<string>();
                List<double> dataList = new List<double>();

                foreach (var item in lstOutletWise)
                {
                    nameList.Add(item.OutletName);
                    dataList.Add(item.AvgPoints);
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }

            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult DashboardSRWiseData(string OutletId, string FromDt, string ToDT)
        {
            List<object> lstData = new List<object>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                var lstSRWise = FMR.GetSRWiseData(userDetails.GroupId, OutletId, FromDt, ToDT);
                List<string> nameList = new List<string>();
                List<double> dataList = new List<double>();

                foreach (var item in lstSRWise)
                {
                    if (!string.IsNullOrEmpty(item.SRName))
                    {
                        nameList.Add(item.SRName);
                    }
                    else
                    {
                        nameList.Add("Other");
                    }

                    dataList.Add(item.AvgPoints);
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }

            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult DashboardLessThank12Data(string OutletId, string FromDt, string ToDT)
        {
            List<int> lstData = new List<int>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                lstData = FMR.GetTimeWiseData(userDetails.GroupId, OutletId, FromDt, ToDT, "1");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult Dashboard12To3Data(string OutletId, string FromDt, string ToDT)
        {
            List<int> lstData = new List<int>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                lstData = FMR.GetTimeWiseData(userDetails.GroupId, OutletId, FromDt, ToDT, "2");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult Dashboard3To6Data(string OutletId, string FromDt, string ToDT)
        {
            List<int> lstData = new List<int>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                lstData = FMR.GetTimeWiseData(userDetails.GroupId, OutletId, FromDt, ToDT, "3");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult DashboardMoreThan6Data(string OutletId, string FromDt, string ToDT)
        {
            List<int> lstData = new List<int>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                lstData = FMR.GetTimeWiseData(userDetails.GroupId, OutletId, FromDt, ToDT, "4");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult DashboardSourceWiseData(string OutletId, string FromDt, string ToDT)
        {
            List<object> lstData = new List<object>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                var lstOutletWise = FMR.GetSourceWiseData(userDetails.GroupId, OutletId, FromDt, ToDT);
                List<string> nameList = new List<string>();
                List<double> dataList = new List<double>();

                foreach (var item in lstOutletWise)
                {
                    nameList.Add(item.SourceName);
                    dataList.Add(item.AvgPoints);
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }

            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult Report()
        {
            FeedbackGetFeedbackViewModel objviewmodel = new FeedbackGetFeedbackViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objviewmodel.GroupId = userDetails.GroupId;
            objviewmodel.PointsAndMessages = FMR.GetPointsAndMessages(objviewmodel.GroupId);
            objviewmodel.lstsalesRepresentive = FMR.GetSalesRepresentiveList(objviewmodel.GroupId);
            objviewmodel.lstoutletlist = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            return View(objviewmodel);

        }
        public ActionResult GetFilteredReport(string jsonData)
        {
            FeedbackGetFeedbackViewModel objviewmodel = new FeedbackGetFeedbackViewModel();
            List<Feedback_Report> lstreport = new List<Feedback_Report>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                DateTime fromDate = Convert.ToDateTime(item["fromDate"]);
                DateTime toDate = Convert.ToDateTime(item["toDate"]);
                string Groupid = Convert.ToString(item["groupId"]);
                string salesR = Convert.ToString(item["selectedsalesR"]);
                string outletId = Convert.ToString(item["selectedoutlet"]);

                lstreport = FMR.GetReportData(Groupid, fromDate, toDate, salesR, outletId);
            }

            return PartialView("_ReportListing", lstreport);
        }
        public ActionResult GetIsCustomerExist(string mobileNo, string GroupId)
        {
            CustomerDetailwithFeedback obj = new CustomerDetailwithFeedback();
            try
            {
                obj = FMR.GetCustomerInfo(mobileNo, GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Get feedbackcust");
            }
            return new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult SubmitPoints(string mobileNo, string ranking, string GroupId, string SalesRepresentative, string Comments, string outletId)
        {
            string status = "false";
            CustomerDetail objcustomerdetails = new CustomerDetail();
            try
            {
                status = FMR.SubmitRating(mobileNo, ranking, GroupId, SalesRepresentative, Comments, outletId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "submit rating");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult SubmitotherinfowithPoints(string MemberName, string Gender, string BirthDt, string mobileNo, string AnniversaryDt, string Knowabt, string GroupId, string OutletId)
        {
            bool status = false;
            CustomerDetail objcustomerdetails = new CustomerDetail();
            try
            {
                status = FMR.Submitotherinfo(MemberName, Gender, BirthDt, mobileNo, AnniversaryDt, Knowabt, GroupId, OutletId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "submit points");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult ExportToExcelFeedbackReport(string fromdt, string Todt, string GroupId, string salesR, string Outlet, string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                DateTime fromdate = new DateTime();
                DateTime ToDate = new DateTime();
                if (fromdt != "" && Todt !="")
                {
                    fromdate= Convert.ToDateTime(fromdt);
                    ToDate = Convert.ToDateTime(Todt);
                }               
                List<Feedback_Report> lstreport = new List<Feedback_Report>();
                lstreport = lstreport = FMR.GetReportData(GroupId, fromdate, ToDate, salesR, Outlet);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Feedback_Report));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (Feedback_Report item in lstreport)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {                   
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "FeedBack Report";
                    worksheet.Cell(2, 1).Value = "Period";
                    worksheet.Cell(2, 2).Value = fromdt + "-" + Todt;
                    worksheet.Cell(3, 1).Value = "Sales Reprentative";
                    worksheet.Cell(3, 2).Value = salesR;                    
                    worksheet.Cell(6, 1).InsertTable(table);
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
                newexception.AddException(ex, userDetails.GroupId);
                return null;
            }


        }
    }
}