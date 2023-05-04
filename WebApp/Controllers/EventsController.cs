using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
            return View();
        }
        public ActionResult CreateEvent()
        {
            EventViewModel objData = new EventViewModel();
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
        public ActionResult EnableEventModule(string GroupId, List<tblGroupDetail> contentData)
        {
            bool status = false;
            try
            {
                status = EVR.EnableEventModule(GroupId, contentData);
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
                    string EventName = Convert.ToString(item["EventName"]);
                    string EventDate = Convert.ToString(item["EventDate"]);
                    string EventPlace = Convert.ToString(item["EventPlace"]);
                    string EventType = Convert.ToString(item["EventType"]);
                    string EventStrDate = Convert.ToString(item["EventStrDate"]);
                    string EventEndDate = Convert.ToString(item["EEventEndDate"]);
                    string BonusPoints = Convert.ToString(item["BonusPoints"]);
                    string PointsExp = Convert.ToString(item["PointsExp"]);
                    string FirstRemaindScript = Convert.ToString(item["FirstRemaindScript"]);
                    string FirstRemaindDate = Convert.ToString(item["FirstRemaindDate"]);
                    string SecondRemaindScript = Convert.ToString(item["SecondRemaindScript"]);
                    string SecondRemdDate = Convert.ToString(item["SecondRemdDate"]);
                    string Description = Convert.ToString(item["Description"]);

                    var Response = EVR.SaveEventData(GroupId, EventName, EventDate, EventPlace, EventType, EventStrDate, EventEndDate, BonusPoints, PointsExp, FirstRemaindScript, FirstRemaindDate, SecondRemaindScript, SecondRemdDate, Description);
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CreateEventData");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}