using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            return View();
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
                    ObjEventDetails.C2ndRemBefore = Convert.ToInt32(item["SecondRemdDays"]);
                    ObjEventDetails.Desciption = Convert.ToString(item["Description"]);
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

        public ActionResult EventForm()
        {
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

            bool status = false;
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

                foreach(var item in ListOfPlace)
                {
                    string place = item;
                    ListOfLink objLink = new ListOfLink();

                    var Lstr = "groupid=" + GroupId;
                    Lstr = "&eventid=" + EventId;
                    Lstr = "&place=" + item;
                    var url = _BaseUrl + "data=" + Common.EncryptString(Lstr);
                    objLink.Place = place;
                    objLink.Url = url;
                    ObjList.Add(objLink);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CreateLink");
            }

            return new JsonResult() { Data = ObjList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}