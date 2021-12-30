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

namespace WebApp.Controllers
{
    public class FeedbackController : Controller
    {
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
            objFeedbackAuthor.GroupId = userDetails.GroupId;
            objFeedbackAuthor.lstFeedbackData = FMR.GetFeedback_Contents(userDetails.GroupId);
            objFeedbackAuthor.lstOutletDetail = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            foreach(var item in objFeedbackAuthor.lstOutletDetail)
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
                ViewBag.lsthowtoknow = FMR.GetHowToKnowAboutList();
            }
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            FeedbackGetFeedbackViewModel objgetfeedbackviewmodel = new FeedbackGetFeedbackViewModel();
            Feedback_PointsAndMessages PointsAndMessages = new Feedback_PointsAndMessages();
            // List<Feedback_Content> lstfbget = new List<Feedback_Content>();
            objgetfeedbackviewmodel.OutletId = Id;
            objgetfeedbackviewmodel.GroupId = groupid;
            objgetfeedbackviewmodel.lstFeedbackData = FMR.GetFeedback_VisibleContents(groupid);
            objgetfeedbackviewmodel.LogoUrl = FMR.GetLogo(groupid);
            PointsAndMessages = FMR.GetPointsAndMessages(groupid);
            objgetfeedbackviewmodel.PointsAndMessages = PointsAndMessages;

            objgetfeedbackviewmodel.lstKnowAboutUs = FMR.GetHowToKnowAboutList();

            return View(objgetfeedbackviewmodel);
        }

        public ActionResult UpdateFeedbackDetails(string HomeData, string QuestionData,string OtherInfoData,string OtherConfigData,string OutletMobileNos)
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
            return View();

        }
        public ActionResult Report()
        {
            return View();

        }


    }
}