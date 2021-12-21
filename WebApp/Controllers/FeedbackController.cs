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
                Feedback_Headings headings = new Feedback_Headings();               
               
                headings = GetHeadings();
                headings.GroupId = GroupId;
                headings.AddedBy= userDetails.LoginId;
                headings.AddedDate = DateTime.Now;

                Feedback_Questions questions = new Feedback_Questions();
                questions = GetQuestions();
                questions.GroupId = GroupId;
                questions.AddedBy = userDetails.LoginId;
                questions.AddedDate = DateTime.Now;

                status = FMR.EnableFeedbackModule(objFeedback, headings, questions);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult StopFeedbackModule(string GroupId, string Reason)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            Feedback_FeedbackConfig objFeedback = new Feedback_FeedbackConfig();
            Feedback_Headings headings = new Feedback_Headings();
            Feedback_Questions questions = new Feedback_Questions();
            objFeedback = FMR.GetFeedbackByGroupId(GroupId);
            objFeedback.GroupId = Convert.ToInt32(GroupId);
            objFeedback.StoppedReason = Reason;
            objFeedback.EndDate = DateTime.Today.Date;
            objFeedback.AddedBy = userDetails.LoginId;
            objFeedback.StoppedDate = DateTime.Now;
            objFeedback.Status = "Stop";
            status = FMR.EnableFeedbackModule(objFeedback, headings, questions);


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
                Feedback_Headings headings = new Feedback_Headings();
                Feedback_Questions questions = new Feedback_Questions();
                objFeedback = FMR.GetFeedbackByGroupId(GroupId);
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;
                objFeedback.StartDate = Convert.ToDateTime(StartDate).Date;
                objFeedback.EndDate = Convert.ToDateTime(StartDate).AddDays(364).Date;
                objFeedback.AddedBy = userDetails.LoginId;
                objFeedback.RenewDate = DateTime.Now;
                objFeedback.Status = "Renew";
                status = FMR.EnableFeedbackModule(objFeedback, headings, questions);
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
                Feedback_Headings headings = new Feedback_Headings();
                Feedback_Questions questions = new Feedback_Questions();
                objFeedback = FMR.GetFeedbackByGroupId(GroupId);
                objFeedback.Fees = Fees;
                objFeedback.PaymentMode = PaymentMode;
                objFeedback.EndDate = objFeedback.EndDate.Value.AddDays(364).Date;
                objFeedback.RenewDate = DateTime.Now;
                objFeedback.Status = "Renew";
                status = FMR.EnableFeedbackModule(objFeedback, headings, questions);
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

        public Feedback_Headings GetHeadings()
        {
            Feedback_Headings headings = new Feedback_Headings();
            XmlDocument doc = new XmlDocument();
            string xmlPath = System.Configuration.ConfigurationManager.AppSettings["FeedbackMasterDataXML"];
            doc.Load(xmlPath);
            XmlNode node = doc.DocumentElement.SelectSingleNode("/data/HomePage/Heading1");
            headings.HomeHeading1 = node.InnerText;
            XmlNode node1 = doc.DocumentElement.SelectSingleNode("/data/HomePage/Heading2");
            headings.HomeHeading2 = node1.InnerText;
            XmlNode node2 = doc.DocumentElement.SelectSingleNode("/data/HomePage/Heading3");
            headings.HomeHeading3 = node2.InnerText;
            //XmlNode node3 = doc.DocumentElement.SelectSingleNode("/data/HomePage/Representative");
            //headings.HomeRepresentative = node3.InnerText;

            XmlNode node4 = doc.DocumentElement.SelectSingleNode("/data/FeedbackQuestions/Heading1");
            headings.QuestionsHeading1 = node4.InnerText;
            XmlNode node5 = doc.DocumentElement.SelectSingleNode("/data/FeedbackQuestions/Heading2");
            headings.QuestionsHeading2 = node5.InnerText;

            XmlNode node6 = doc.DocumentElement.SelectSingleNode("/data/OtherInformation/Heading1");
            headings.OtherInfoHeading1 = node6.InnerText;
            XmlNode node7 = doc.DocumentElement.SelectSingleNode("/data/OtherInformation/Heading2");
            headings.OtherInfoHeading2 = node7.InnerText;

            return headings;
        }

        public Feedback_Questions GetQuestions()
        {
            Feedback_Questions questions = new Feedback_Questions();
            XmlDocument doc = new XmlDocument();
            string xmlPath = System.Configuration.ConfigurationManager.AppSettings["FeedbackMasterDataXML"];
            doc.Load(xmlPath);
            XmlNode node = doc.DocumentElement.SelectSingleNode("/data/FeedbackQuestions/Question1");
            questions.FeedbackQuestion1 = node.InnerText;
            XmlNode node1 = doc.DocumentElement.SelectSingleNode("/data/FeedbackQuestions/Question2");
            questions.FeedbackQuestion2 = node1.InnerText;
            XmlNode node2 = doc.DocumentElement.SelectSingleNode("/data/FeedbackQuestions/Question3");
            questions.FeedbackQuestion3 = node2.InnerText;
            XmlNode node3 = doc.DocumentElement.SelectSingleNode("/data/FeedbackQuestions/Question4");
            questions.FeedbackQuestion4 = node3.InnerText;

            XmlNode node4 = doc.DocumentElement.SelectSingleNode("/data/OtherInformation/Question1");
            questions.OtherInfoQuestion1 = node4.InnerText;
            XmlNode node5 = doc.DocumentElement.SelectSingleNode("/data/OtherInformation/Question2");
            questions.OtherInfoQuestion2 = node5.InnerText;
            XmlNode node6 = doc.DocumentElement.SelectSingleNode("/data/OtherInformation/Question3");
            questions.OtherInfoQuestion3 = node6.InnerText;
            XmlNode node7 = doc.DocumentElement.SelectSingleNode("/data/OtherInformation/Question4");
            questions.OtherInfoQuestion4 = node7.InnerText;


            return questions;
        }

        public ActionResult CreateFeedback()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            FeedbackAuthorViewModel objFeedbackAuthor = new FeedbackAuthorViewModel();
            Feedback_PointwsAndMessages PointwsAndMessages = new Feedback_PointwsAndMessages();
            objFeedbackAuthor.headings = FMR.GetHeadings(userDetails.GroupId);
            objFeedbackAuthor.questions = FMR.GetQuestions(userDetails.GroupId);
            objFeedbackAuthor.PointwsAndMessages = PointwsAndMessages;
            return View(objFeedbackAuthor);
        }
    }
}