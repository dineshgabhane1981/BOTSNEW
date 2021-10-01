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
using BOTS_BL.Models.CommonDB;

namespace WebApp.Controllers
{
    public class DiscussionController : Controller
    {
        DiscussionsRepository DR = new DiscussionsRepository();
        CustomerRepository CR = new CustomerRepository();
        // GET: Discussion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllDiscussions(string groupId)
        {
            CommonFunctions common = new CommonFunctions();
            groupId = common.DecryptString(groupId);
            DiscussionViewModel objData = new DiscussionViewModel();
            BOTS_TblDiscussion objDiscussion = new BOTS_TblDiscussion();
            var objGroup = CR.GetGroupDetails(Convert.ToInt32(groupId));
            objDiscussion.GroupId = groupId;
            objDiscussion.GroupName = objGroup.GroupName;
            objData.objDiscussion = objDiscussion;
            objData.lstDiscussions = DR.GetDiscussions(groupId);
            objData.lstCallTypes = DR.GetCallTypes();
            List<SelectListItem> callSubType = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Value = "0";
            item.Text = "Please Select";
            callSubType.Add(item);
            objData.lstCallSubTypes = callSubType;

            return View(objData);
        }

        public ActionResult CommonDiscussion()
        {         
            List<DiscussionDetails> lstdashboard = new List<DiscussionDetails>();
            ViewBag.lstcommonstatus = DR.CommonStatus();
            ViewBag.lstgroupdetails = DR.GetGroupDetails();
            ViewBag.lstCallTypes = DR.GetCallTypes();
            lstdashboard = DR.GetfilteredDiscussionData("", 0, "", "", "");
            return View(lstdashboard);
        }

        [HttpPost]
        public JsonResult GetSubCallTypes(int callId)
        {
            var lstSubCallType = DR.GetSubCallTypes(callId);
            return new JsonResult() { Data = lstSubCallType, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public bool AddDiscussion(DiscussionViewModel objData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData.objDiscussion.AddedDate = DateTime.Now;
            objData.objDiscussion.AddedBy = userDetails.LoginId;
            status = DR.AddDiscussions(objData.objDiscussion);

            return status;
        }
        public JsonResult GetSubDiscussionList(int Id)
        {
             List<SubDiscussionData> lstsubdiscussionLists = new List<SubDiscussionData>();
            // List<BOTS_TblSubDiscussionData> lstsubdiscussionlists = new List<BOTS_TblSubDiscussionData>();
            lstsubdiscussionLists = DR.GetNestedDiscussionList(Id);

            return new JsonResult() { Data = lstsubdiscussionLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetDiscussionList(string groupId)
        {
            var lstDiscussions = DR.GetDiscussions(groupId);
            return PartialView("_DiscussionList", lstDiscussions);
        }

        [HttpPost]
        public bool UpdateStatusAndDiscussion(string dId, string Desc, string Status)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            status = DR.UpdateDiscussions(dId, Desc, Status, userDetails.LoginId);

            return status;
        }
       
        public ActionResult GetCommonFilteredDiscussion(string jsonData)
        {
            //var lstdashboard ="";
            List<DiscussionDetails> lstdashboard = new List<DiscussionDetails>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string fromDate = Convert.ToString(item["fromDate"]);
                string toDate = Convert.ToString(item["toDate"]);
                string groupnm = Convert.ToString(item["selectedgrp"]);
                int calltype = Convert.ToInt32(item["selectedcall"]);
                string status = Convert.ToString(item["selectedstatus"]);
               
                lstdashboard = DR.GetfilteredDiscussionData(status, calltype, groupnm, fromDate, toDate);
            }
            return PartialView("_CommonDiscussionList", lstdashboard);
        }
    }
}