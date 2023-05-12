using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Models.EventModule;
using BOTS_BL.Repository;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class EventsController : Controller
    {
        Exceptions newexception = new Exceptions();
        EventsRepository EVR = new EventsRepository();
        CustomerRepository CR = new CustomerRepository();

        // GET: Events List
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            EventViewModel objData = new EventViewModel();

            objData.lstEvent = EVR.GetListEvents(userDetails.GroupId, userDetails.connectionString);
            return View(objData);
        }
        public ActionResult CreateEvent()
        {
            EventViewModel objData = new EventViewModel();
            EventDetail objEvent = new EventDetail();
            objData.objEvent = objEvent;
            return View(objData);
        }
        public ActionResult EventReport()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstData = EVR.GetEventReport(userDetails.GroupId, "", "");
            return View(lstData);
        }
        public ActionResult EventCustomers()
        {
            EventViewModel objData = new EventViewModel();
            objData.lstNeverOptFor = EVR.GetNeverOptForGroups(false);
            objData.lstActive = EVR.GetNeverOptForGroups(true);
            return View(objData);
        }
        public ActionResult EnableEventModule(string jsonData)
        {
            bool status = false;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string GroupId = Convert.ToString(item["GroupId"]);
                    status = EVR.EnableEventModule(GroupId);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableEventModule");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult DisableEventModule(string jsonData)
        {
            bool status = false;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string GroupId = Convert.ToString(item["GroupId"]);
                    status = EVR.DisableEventModule(GroupId);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableEventModule");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult SaveEvent(string jsonData)
        {
            EventDetail ObjEventDetails = new EventDetail();

            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string GroupId = userDetails.GroupId;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);


            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    ObjEventDetails.AddedBy = userDetails.LoginId;
                    ObjEventDetails.GroupId = Convert.ToInt32(GroupId);
                    ObjEventDetails.EventId = Convert.ToInt64(item["EventId"]);
                    ObjEventDetails.EventName = Convert.ToString(item["EventName"]);
                    ObjEventDetails.Addeddate = Convert.ToDateTime(item["EventDate"]);
                    ObjEventDetails.Place = Convert.ToString(item["EventPlace"]);
                    ObjEventDetails.EventType = Convert.ToString(item["EventType"]);
                    ObjEventDetails.EventStartDate = Convert.ToDateTime(item["EventStrDate"]);
                    ObjEventDetails.EventEndDate = Convert.ToDateTime(item["EventEndDate"]);
                    ObjEventDetails.BonusPoints = Convert.ToInt32(item["BonusPoints"]);
                    ObjEventDetails.PointsExpiryDays = Convert.ToInt32(item["PointsExp"]);
                    ObjEventDetails.C1stReminderScript = Convert.ToString(item["FirstRemaindScript"]);
                    ObjEventDetails.C1stRemBefore = Convert.ToInt32(item["FirstRemdDays"]);
                    ObjEventDetails.C2ndReminderScript = Convert.ToString(item["SecondRemaindScript"]);
                    bool flag = string.IsNullOrEmpty(Convert.ToString(item["SecondRemdDays"]));
                    if (flag == false)
                    {
                        ObjEventDetails.C2ndRemBefore = Convert.ToInt32(item["SecondRemdDays"]);
                    }
                    ObjEventDetails.Desciption = Convert.ToString(item["Description"]);
                    ObjEventDetails.BonusMessageScript = Convert.ToString(item["Script"]);
                    ObjEventDetails.Status = "Created";
                    //ObjEventDetails.EventId = Convert.ToInt32("123");

                    var Response = EVR.SaveEventData(ObjEventDetails, userDetails.connectionString);
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CreateEventData");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult EventForm(string data)
        {
            string eventId = string.Empty;
            string groupId = string.Empty;
            string place = string.Empty;
            CommonFunctions common = new CommonFunctions();
            var AData = common.DecryptString(data);
            var lstParameter = AData.Split('&');
            var groupIdStr = lstParameter[0];
            var eventIdStr = lstParameter[1];
            var placeStr = lstParameter[2];

            var groupData = groupIdStr.Split('=');
            groupId = groupData[1];

            var eventData = eventIdStr.Split('=');
            eventId = eventData[1];

            var placeData = placeStr.Split('=');
            place = placeData[1];

            ViewBag.GroupId = groupId;
            ViewBag.EventId = eventId;
            ViewBag.Place = place;



            return View();
        }

        public ActionResult EventEdit(string groupId, string eventid)
        {
            EventViewModel objData = new EventViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var ListEventData = EVR.GetEditEvents(groupId, eventid, userDetails.connectionString);

            objData.objEvent = ListEventData;
            return View("CreateEvent", objData);
        }
        public ActionResult EventDelete()
        {
            var groupId = (string)Session["GroupId"];
            ViewBag.GroupId = groupId;
            return View();
        }
        public ActionResult DeleteEventDetails(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var result = false;
            string EventId, GroupId;
            EventId = string.Empty;
            GroupId = string.Empty;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

                foreach (Dictionary<string, object> item in objData)
                {
                    EventId = Convert.ToString(item["EventId"]);
                    GroupId = Convert.ToString(item["GroupId"]);
                }
                result = EVR.EventDelete(EventId, GroupId, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DeleteEventDetails");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateLink(string jsonData)
        {
            string _BaseUrl = ConfigurationManager.AppSettings["BaseURL"];
            CommonFunctions Common = new CommonFunctions();
            List<ListOfLink> ObjList = new List<ListOfLink>();

            string EventId, GroupId, Place;
            EventId = string.Empty;
            GroupId = string.Empty;
            Place = string.Empty;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    EventId = Convert.ToString(item["EventId"]);
                    GroupId = Convert.ToString(item["GroupId"]);
                    Place = Convert.ToString(item["Place"]);
                }
                string[] ListOfPlace = Place.Split(',');

                foreach (var item in ListOfPlace)
                {
                    string place = item;
                    ListOfLink objLink = new ListOfLink();

                    var Lstr = "groupid=" + GroupId;
                    Lstr += "&eventid=" + EventId;
                    Lstr += "&place=" + item;
                    var url = _BaseUrl + "Events/EventForm?data=" + Common.EncryptString(Lstr);
                    objLink.Place = place;
                    objLink.Url = url;
                    ObjList.Add(objLink);
                }
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                var EventData = EVR.GetEditEvents(GroupId, EventId, userDetails.connectionString);
                EventData.Status = "Started";
                EventData.Addeddate = DateTime.Now;
                var Response = EVR.SaveEventData(EventData, userDetails.connectionString);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CreateLink");
            }

            return new JsonResult() { Data = ObjList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


        [HttpPost]
        public ActionResult GetCustomerdata(string groupId, string Mobileno, string Place,string EventId)
        {
            //EventDetail objData = new EventDetail();
            EventModuleData objData = new EventModuleData();

            try
            {
                var connStr = CR.GetCustomerConnString(groupId);
                //var userDetails = (CustomerLoginDetail)Session["UserSession"];

                objData = EVR.GetCustomerDetails(groupId, Mobileno, Place, EventId, connStr);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerdata");
            }

            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveNewMemberData(string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            EventMemberDetail objEventDetail = new EventMemberDetail();
            CustomerDetail objCustomerDetail = new CustomerDetail();
            CustomerChild objCustomerChild = new CustomerChild();
            TransactionMaster objTM = new TransactionMaster();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);


            foreach (Dictionary<string, object> item in objData)
            {

                objEventDetail.GroupId = Convert.ToInt32(item["GroupId"]);
                objEventDetail.EventId = Convert.ToInt32(item["EventId"]);
                objEventDetail.Place = Convert.ToString(item["Place"]);
                objEventDetail.Mobileno = Convert.ToString(item["Mobileno"]);
                objEventDetail.Name = Convert.ToString(item["Name"]);
                objEventDetail.Gender = Convert.ToString(item["Gender"]);
                objEventDetail.DOB = Convert.ToDateTime(item["DOB"]);
                objEventDetail.DOA = Convert.ToDateTime(item["DOA"]);
                objEventDetail.EmailId = Convert.ToString(item["EmailId"]);
                objEventDetail.Address = Convert.ToString(item["Address"]);
                objEventDetail.AlternateNo = Convert.ToString(item["AlternateNo"]);
                objEventDetail.DateOfRegistration = DateTime.Now;

            }
            foreach (Dictionary<string, object> item in objData)
            {
                objCustomerDetail.MobileNo = Convert.ToString(item["Mobileno"]);
                objCustomerDetail.CustomerName = Convert.ToString(item["Name"]);
                objCustomerDetail.CardNumber = Convert.ToString(item["Mobileno"]);
                objCustomerDetail.EmailId = Convert.ToString(item["EmailId"]);
                objCustomerDetail.DOB = Convert.ToDateTime(item["DOB"]);
                objCustomerDetail.AnniversaryDate = Convert.ToDateTime(item["DOA"]);
                objCustomerDetail.Gender = Convert.ToString(item["Gender"]);
                objCustomerDetail.OldMobileNo = Convert.ToString(item["AlternateNo"]);

            }
            foreach (Dictionary<string, object> item in objData)
            {
                objCustomerChild.Address = Convert.ToString(item["Address"]);
                objCustomerChild.MobileNo = Convert.ToString(item["Mobileno"]);
            }

            var connStr = CR.GetCustomerConnString(Convert.ToString(objEventDetail.GroupId));
            result = EVR.SaveNewMemberData(objEventDetail, objCustomerDetail, objCustomerChild, objTM, connStr);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

        public ActionResult GetFilterReportData(string fromDate, string toDate)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstData = EVR.GetEventReport(userDetails.GroupId, fromDate, toDate);
            return PartialView("_EventReport", lstData);
        }
        public ActionResult ExportToExcelEventReport(string fromDate, string toDate)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var lstData = EVR.GetEventReport(userDetails.GroupId, fromDate, toDate);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(EventMemberDetail));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);


                foreach (EventMemberDetail item in lstData)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                table.Columns.Remove("SLno");
                table.Columns.Remove("GroupId");
                table.Columns.Remove("EventId");
                string fileName = "BOTS_EventsReport.xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = "EventsReport";
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: "EventsReport");
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Events Report";
                    worksheet.Cell(2, 1).Value = "From Date";
                    worksheet.Cell(2, 2).Value = fromDate;
                    worksheet.Cell(3, 1).Value = "To Date";
                    worksheet.Cell(3, 2).Value = toDate;                    
                    
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
                newexception.AddException(ex, "ExportToExcelEventReport");
                return null;
            }
        }

    }
}