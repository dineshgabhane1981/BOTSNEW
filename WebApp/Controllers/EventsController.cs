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
using System.Data.Entity.Core.Objects;
using System.Net;
using System.Net.Mail;
using System.Text;

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
            objEvent.strEventdate = DateTime.Now.ToString("MM/dd/yyyy");
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
                    if (ObjEventDetails.EventId == 0)
                        ObjEventDetails.Status = "Created";
                    else
                    {
                        var EventData = EVR.GetEditEvents(Convert.ToString(ObjEventDetails.GroupId), Convert.ToString(ObjEventDetails.EventId), userDetails.connectionString);
                        ObjEventDetails.Status = EventData.Status;
                    }
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
            EventViewModel objdata = new EventViewModel();           
            
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
            objdata.Logo = EVR.GetLogo(groupId);
            var connStr = CR.GetCustomerConnString(groupId);
            var eventDetails = EVR.GetEditEvents(groupId, eventId, connStr);
            objdata.EventName = eventDetails.EventName;
            if (eventDetails.EventStartDate.Value > DateTime.Now)
            {
                ViewBag.EventStarted = "Not Started";
            }
            else
            {
                ViewBag.EventStarted = "Started";
            }
            if (eventDetails.EventEndDate.Value < DateTime.Now)
            {
                ViewBag.EventEnded = "Ended";
            }
            else
            {
                ViewBag.EventEnded = "Not Ended";
            }

            return View(objdata);
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
            string strDOB, strDOA;
            strDOB = string.Empty;
            strDOA = string.Empty;
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            EventMemberDetail objEventDetail = new EventMemberDetail();
            CustomerDetail objCustomerDetail = new CustomerDetail();
            CustomerChild objCustomerChild = new CustomerChild();
            TransactionMaster objTM = new TransactionMaster();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            try
            {
                foreach (Dictionary<string, object> item in objData)
                {

                    objEventDetail.GroupId = Convert.ToInt32(item["GroupId"]);
                    objEventDetail.EventId = Convert.ToInt32(item["EventId"]);
                    objEventDetail.Place = Convert.ToString(item["Place"]);
                    objEventDetail.Mobileno = Convert.ToString(item["Mobileno"]);
                    if(!string.IsNullOrEmpty(Convert.ToString(item["MiddleName"])) && !string.IsNullOrEmpty(Convert.ToString(item["SurName"])))
                    {
                        objEventDetail.Name = Convert.ToString(item["FirstName"]) + " " + Convert.ToString(item["MiddleName"]) + " " + Convert.ToString(item["SurName"]);
                    }
                    else if (string.IsNullOrEmpty(Convert.ToString(item["MiddleName"])) && string.IsNullOrEmpty(Convert.ToString(item["SurName"])))
                    {
                        objEventDetail.Name = Convert.ToString(item["FirstName"]);
                    }
                    else if(string.IsNullOrEmpty(Convert.ToString(item["MiddleName"])))
                    {
                        objEventDetail.Name = Convert.ToString(item["FirstName"]) + " " + Convert.ToString(item["SurName"]);
                    }
                    else if(string.IsNullOrEmpty(Convert.ToString(item["SurName"])))
                    {
                        objEventDetail.Name = Convert.ToString(item["FirstName"]) + " " + Convert.ToString(item["MiddleName"]);
                    }
                    objEventDetail.Name = Convert.ToString(item["FirstName"]) +" "+ Convert.ToString(item["MiddleName"]) +" "+ Convert.ToString(item["SurName"]);
                    objEventDetail.FirstName = Convert.ToString(item["FirstName"]);
                    objEventDetail.MiddleName = Convert.ToString(item["MiddleName"]);
                    objEventDetail.SurName = Convert.ToString(item["SurName"]);
                    objEventDetail.Gender = Convert.ToString(item["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["DOB"])))
                    {
                        objEventDetail.DOB = Convert.ToDateTime(item["DOB"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["DOA"])))
                    {
                        objEventDetail.DOA = Convert.ToDateTime(item["DOA"]);
                    }
                    
                    objEventDetail.EmailId = Convert.ToString(item["EmailId"]);
                    objEventDetail.Address = Convert.ToString(item["Address"]);
                    objEventDetail.AlternateNo = Convert.ToString(item["AlternateNo"]);
                    objEventDetail.DateOfRegistration = DateTime.Now;
                    objEventDetail.Area = Convert.ToString(item["Area"]);
                    objEventDetail.City = Convert.ToString(item["City"]);
                    objEventDetail.Pincode = Convert.ToString(item["PinCode"]);
                    objEventDetail.State = Convert.ToString(item["State"]);

                }
                foreach (Dictionary<string, object> item in objData)
                {
                    objCustomerDetail.MobileNo = Convert.ToString(item["Mobileno"]);
                    objCustomerDetail.CustomerName = objEventDetail.Name;
                    objCustomerDetail.CardNumber = Convert.ToString(item["Mobileno"]);
                    objCustomerDetail.EmailId = Convert.ToString(item["EmailId"]);
                    strDOB = Convert.ToString(item["DOB"]);
                    if (!string.IsNullOrEmpty(strDOB))
                    {
                        objCustomerDetail.DOB = Convert.ToDateTime(item["DOB"]);
                    }
                    strDOA = Convert.ToString(item["DOA"]);
                    if (!string.IsNullOrEmpty(strDOA))
                    {
                        objCustomerDetail.AnniversaryDate = Convert.ToDateTime(item["DOA"]);
                    }
                    objCustomerDetail.Gender = Convert.ToString(item["Gender"]);
                    objCustomerDetail.OldMobileNo = Convert.ToString(item["AlternateNo"]);

                }
                foreach (Dictionary<string, object> item in objData)
                {
                    objCustomerChild.Address = Convert.ToString(item["Address"]);
                    objCustomerChild.MobileNo = Convert.ToString(item["Mobileno"]);
                }

                var connStr = CR.GetCustomerConnString(Convert.ToString(objEventDetail.GroupId));
                result = EVR.SaveNewMemberData(objEventDetail, objCustomerDetail, objCustomerChild, objTM, connStr, objEventDetail.GroupId);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveNewMemberData");
            }

            
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

        public string GenerateEventReports()
        {
            string status = "Report Sent";
            try
            {
                var AllCustomer = EVR.GetAllEventCustomer();

                foreach (var customer in AllCustomer)
                {
                    GetReportData(Convert.ToString(customer.GroupId));
                }
                foreach (var customer in AllCustomer)
                {
                    GetFirstRemainderData(Convert.ToString(customer.GroupId));
                }
                foreach (var customer in AllCustomer)
                {
                    GetSecondRemainderData(Convert.ToString(customer.GroupId));
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GenerateEventReports");
            }
            return status;
        }

        public void GetReportData(string groupid)
        {
            string DateOnly = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                var listEventData = EVR.EventReportData(groupid);

                foreach (var item in listEventData)
                {
                    SendEventReportMail(Convert.ToString(item.GroupId), Convert.ToString(item.EventId), listEventData);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetReportData");
            }
        }

        public void SendEventReportMail(string GroupId, string EventId, List<EventDetail> listEventData)
        {
            string EventName;
            EventName = string.Empty;
            List<EventMemberDetail> lstReportData = new List<EventMemberDetail>();

            int eventid = Convert.ToInt32(EventId);

            string Today = DateTime.Now.ToString("dd-MM-yyyy");
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime dateVal = DateTime.ParseExact(Today, "dd-MM-yyyy", culture);
            try
            {
                lstReportData = EVR.EventMemberData(GroupId, EventId);
                string GroupName = EVR.GetGroupdetails(GroupId);

                if (lstReportData.Count > 0)
                {
                    foreach (var lst in lstReportData)
                    {
                        if (lst.EventId == Convert.ToInt32(EventId))
                        {
                            EventName = lst.EventName;
                        }
                        break;
                    }
                    string EmailSubject = string.Empty;
                    EmailSubject = "Daily Event Report - " + GroupName;
                    StringBuilder str = new StringBuilder();
                    str.Append("<table>");
                    str.Append("<tr>");

                    str.AppendLine("<td>Dear Customer,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");

                    str.AppendLine("<td>Please find Daily Event Report - " + EventName + "</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<table style='border:2px solid;border-collapse:collapse;'>");
                    str.Append("<tr style='border:2px solid;border-collapse:collapse;'>");
                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;' colspan='9'>");
                    str.Append(Today);
                    str.Append("</th>");
                    str.Append("</tr>");
                    str.Append("<tr style='background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Event Name");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Mobile Number");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Member Name");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Address");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("EmailId");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Place");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Points Given");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Registration Date");
                    str.Append("</th>");

                    str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                    str.Append("Type");
                    str.Append("</th>");

                    str.Append("</tr>");
                    foreach (var item in lstReportData)
                    {
                        str.Append("<tr style='border:2px solid;border-collapse:collapse;'>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.EventName.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.Mobileno.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.Name.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.Address.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.EmailId.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.Place.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.PointsGiven.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.DateOfRegistration.ToString() + "</td>");
                        str.Append("<td style='border:2px solid;border-collapse:collapse;'align='left'>" + item.CustomerType.ToString() + "</td>");
                        str.Append("</tr>");
                    }
                    str.Append("</table>");

                    str.Append("<table>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.AppendLine("<td>Regards,</td>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td>Blue Ocktopus team</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.AppendLine("<td>support@blueocktopus.in</td>");
                    str.AppendLine("</br>");
                    str.AppendLine("</br>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td>&nbsp;</td>");
                    str.Append("</tr>");

                    str.Append("</table>");

                    MailMessage Msg = new MailMessage();                    

                    Msg.From = new MailAddress("report@blueocktopus.in");
                    var emailId = EVR.GetReportEmail(Convert.ToInt32(GroupId));
                    Msg.To.Add(emailId);

                    Msg.Subject = EmailSubject;
                    Msg.Body = str.ToString();

                    Msg.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.zoho.com");
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    smtp.Credentials = new System.Net.NetworkCredential("report@blueocktopus.in", "S#02MC@OW92d8$x");

                    smtp.Send(Msg);
                    Msg.Dispose();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendEventReportMail");
            }
        }

        public void GetFirstRemainderData(string groupid)
        {
            string DateOnly = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                var listEventData = EVR.EventReportData(groupid);

                foreach (var item in listEventData)
                {
                    var listData = EVR.FirstRemainderData(Convert.ToString(item.GroupId), Convert.ToString(item.EventId));

                    SendMessage(listData);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFirstRemainderData");
            }
        }

        public void GetSecondRemainderData(string groupid)
        {
            string DateOnly = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                var listEventData = EVR.EventReportData(groupid);

                foreach (var item in listEventData)
                {
                    var listData = EVR.SecondRemainderData(Convert.ToString(item.GroupId), Convert.ToString(item.EventId));

                    SendSecondMessage(listData);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSecondRemainderData");
            }
        }

        public void SendMessage(List<ReminderData> Data)
        {
            foreach (var item in Data)
            {
                string responseString;
                
                try
                {                    
                    item.FirstReminderScript = item.FirstReminderScript.Replace("#99", "&");
                    item.FirstReminderScript = item.FirstReminderScript.Replace("#01", item.Name);
                    item.FirstReminderScript = item.FirstReminderScript.Replace("#06", Convert.ToString(item.PointsGiven));
                    item.FirstReminderScript = item.FirstReminderScript.Replace("#15", item.ExpDate.ToString("yyyy-MM-dd"));
                    item.FirstReminderScript = HttpUtility.UrlEncode(item.FirstReminderScript);
                    //string type = "TEXT";
                    StringBuilder sbposdata = new StringBuilder();
                    sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                    sbposdata.AppendFormat("token={0}", item.Tokenid);
                    sbposdata.AppendFormat("&phone=91{0}", item.Mobileno);
                    sbposdata.AppendFormat("&message={0}", item.FirstReminderScript);

                    string Url = sbposdata.ToString();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] data = encoding.GetBytes(sbposdata.ToString());
                    httpWReq.Method = "POST";

                    httpWReq.ContentType = "application/x-www-form-urlencoded";
                    httpWReq.ContentLength = data.Length;
                    using (Stream stream = httpWReq.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SendMessage");
                }

            }
        }

        public void SendSecondMessage(List<ReminderData> Data)
        {
            foreach (var item in Data)
            {
                string responseString;
                try
                {

                    item.SecondReminderScript = item.SecondReminderScript.Replace("#99", "&");
                    item.SecondReminderScript = item.SecondReminderScript.Replace("#01", item.Name);
                    item.SecondReminderScript = item.SecondReminderScript.Replace("#06", Convert.ToString(item.PointsGiven));
                    item.SecondReminderScript = item.SecondReminderScript.Replace("#15", item.ExpDate.ToString("yyyy-MM-dd"));
                    item.SecondReminderScript = HttpUtility.UrlEncode(item.SecondReminderScript);
                    //string type = "TEXT";
                    StringBuilder sbposdata = new StringBuilder();
                    sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                    sbposdata.AppendFormat("token={0}", item.Tokenid);
                    sbposdata.AppendFormat("&phone=91{0}", item.Mobileno);
                    sbposdata.AppendFormat("&message={0}", item.SecondReminderScript);

                    string Url = sbposdata.ToString();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] data = encoding.GetBytes(sbposdata.ToString());
                    httpWReq.Method = "POST";

                    httpWReq.ContentType = "application/x-www-form-urlencoded";
                    httpWReq.ContentLength = data.Length;
                    using (Stream stream = httpWReq.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SendSecondMessage");
                }

            }
        }

    }
}