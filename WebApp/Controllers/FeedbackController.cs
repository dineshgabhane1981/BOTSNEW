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

namespace WebApp.Controllers
{
    public class FeedbackController : Controller
    {
        FeedbackModuleRepository FMR = new FeedbackModuleRepository();
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

                Feedback_FeedbackDetails objFeedback = new Feedback_FeedbackDetails();
                objFeedback.GroupId = Convert.ToInt32(GroupId);
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;
                objFeedback.StartDate = Convert.ToDateTime(StartDate).Date;
                objFeedback.EndDate = Convert.ToDateTime(StartDate).AddDays(364).Date;
                objFeedback.AddedBy = userDetails.LoginId;
                objFeedback.AddedDate = DateTime.Now;
                objFeedback.Status = "Active";
                status = FMR.EnableFeedbackModule(objFeedback);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult StopFeedbackModule(string GroupId, string Reason)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            Feedback_FeedbackDetails objFeedback = new Feedback_FeedbackDetails();
            objFeedback = FMR.GetFeedbackByGroupId(GroupId);
            objFeedback.GroupId = Convert.ToInt32(GroupId);
            objFeedback.StoppedReason = Reason;
            objFeedback.EndDate = DateTime.Today.Date;
            objFeedback.AddedBy = userDetails.LoginId;
            objFeedback.StoppedDate = DateTime.Now;
            objFeedback.Status = "Stop";
            status = FMR.EnableFeedbackModule(objFeedback);


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

                Feedback_FeedbackDetails objFeedback = new Feedback_FeedbackDetails();
                objFeedback = FMR.GetFeedbackByGroupId(GroupId);                
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;
                objFeedback.StartDate = Convert.ToDateTime(StartDate).Date;
                objFeedback.EndDate = Convert.ToDateTime(StartDate).AddDays(364).Date;
                objFeedback.AddedBy = userDetails.LoginId;
                objFeedback.RenewDate = DateTime.Now;
                objFeedback.Status = "Renew";
                status = FMR.EnableFeedbackModule(objFeedback);
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

                Feedback_FeedbackDetails objFeedback = new Feedback_FeedbackDetails();
                objFeedback = FMR.GetFeedbackByGroupId(GroupId);
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;                
                objFeedback.EndDate = objFeedback.EndDate.Value.AddDays(364).Date;                
                objFeedback.RenewDate = DateTime.Now;
                objFeedback.Status = "Renew";
                status = FMR.EnableFeedbackModule(objFeedback);
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
    }
}