﻿using System;
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
using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;
using System.Globalization;
using System.IO;

namespace WebApp.Controllers
{
    public class DiscussionController : Controller
    {
        DiscussionsRepository DR = new DiscussionsRepository();
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();

        // GET: Discussion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllDiscussions(string groupId, string isOnboarding)
        {

            CommonFunctions common = new CommonFunctions();
            groupId = common.DecryptString(groupId);

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            userDetails.GroupId = groupId;
            userDetails.CustomerName = CR.GetCustomerName(groupId);
            DiscussionViewModel objData = new DiscussionViewModel();
            try
            {
                string LoginType = userDetails.LoginType;
                string LoginId = userDetails.LoginId;
                string LoginName = userDetails.UserName;

                Session["buttons"] = "Discussion";

                
                BOTS_TblDiscussion objDiscussion = new BOTS_TblDiscussion();
                string GroupName = string.Empty;
                if (string.IsNullOrEmpty(isOnboarding))
                {
                    var objGroup = CR.GetGroupDetails(Convert.ToInt32(groupId));
                    GroupName = objGroup.GroupName;
                    userDetails.connectionString = CR.GetCustomerConnString(groupId);
                }
                else
                {
                    var objGroup = CR.GetOnboardingGroupDetails(groupId);
                    GroupName = objGroup.GroupName;
                }
                Session["UserSession"] = userDetails;
                objDiscussion.GroupId = groupId;
                objDiscussion.GroupName = GroupName;
                objData.objDiscussion = objDiscussion;
                objData.lstDiscussions = DR.GetDiscussions(groupId, LoginType, LoginId, LoginName);
                objData.lstCallTypes = DR.GetCallTypes(LoginType);
                List<SelectListItem> callSubType = new List<SelectListItem>();
                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "Please Select";
                callSubType.Add(item);
                objData.lstCallSubTypes = callSubType;

                var CustNames = DR.GetAllDiscussionCustNames(groupId);
                ViewBag.CustNames = CustNames.ToArray();
                ViewBag.CustNames1 = CustNames;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AllDiscussions");
            }
            return View(objData);
        }

        public ActionResult CommonDiscussion()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            userDetails.CustomerName = "";
            string LoginType = userDetails.LoginType;
            DiscussionViewModel ObjData = new DiscussionViewModel();
            try
            {
                ViewBag.lstcommonstatus = DR.CommonStatus();
                ViewBag.lstgroupdetails = DR.GetGroupDetails();
                ViewBag.lstCallTypes = DR.GetCallTypes(LoginType);
                ViewBag.lstRMAssigned = DR.GetRaisedby();
                ViewBag.lstMemberAssigned = DR.GetAssignedMemberList();
                if (LoginType != "1" && LoginType != "7")
                {
                    ObjData.lstDiscussions = DR.GetfilteredDiscussionDataAssign("", 0, 0, "", "", "", "", LoginType, userDetails.LoginId, false, userDetails.LoginId, "");
                    ObjData.lstFollowUpsDiscussions = DR.GetfilteredDiscussionDataAssign("", 0, 0, "", "", "", "", LoginType, userDetails.LoginId, true, userDetails.LoginId, "");
                }
                else
                {
                    ObjData.lstDiscussions = DR.GetfilteredDiscussionDataAssign("", 0, 0, "", "", "", "", LoginType, userDetails.LoginId, false, "", "");
                    ObjData.lstFollowUpsDiscussions = DR.GetfilteredDiscussionDataAssign("", 0, 0, "", "", "", "", LoginType, userDetails.LoginId, true, "", "");
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CommonDiscussion");
            }
            return View(ObjData);
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
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                objData.objDiscussion.AddedDate = DateTime.Now;
                objData.objDiscussion.UpdatedDate = DateTime.Now;
                objData.objDiscussion.AddedBy = userDetails.LoginId;
                string File = objData.File;
                string FileName = objData.FileName;
                status = DR.AddDiscussions(objData.objDiscussion, File, FileName);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddDiscussion");
            }

            return status;
        }
        public JsonResult GetSubDiscussionList(int Id)
        {
            List<SubDiscussionData> lstsubdiscussionLists = new List<SubDiscussionData>();
            lstsubdiscussionLists = DR.GetNestedDiscussionList(Id);

            return new JsonResult() { Data = lstsubdiscussionLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetDiscussionList(string groupId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string LoginType = userDetails.LoginType;
            string LoginId = userDetails.LoginId;
            string LoginName = userDetails.UserName;

            var lstDiscussions = DR.GetDiscussions(groupId, LoginType, LoginId, LoginName);
            return PartialView("_DiscussionList", lstDiscussions);
        }

        [HttpPost]
        public bool UpdateStatusAndDiscussion(UpdateCompleteThread Obj)//(string dId, string Desc, string Status, string FollowupDate, string Reassign, string FileName, string File,string RequestType)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                string dId = Obj.dId;
                string Desc = Obj.Desc;
                string Status = Obj.Status;
                string FollowupDate = Obj.FollowupDate;
                string Reassign = Obj.Reassign;
                string FileName = Obj.FileName;
                string File = Obj.File;
                string RequestType = Obj.RequestType;
                string DoneNotDone = Obj.DoneNotDone;
                status = DR.UpdateDiscussions(dId, Desc, Status, userDetails.LoginId, FollowupDate, Reassign, FileName, File, RequestType, DoneNotDone);

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateStatusAndDiscussion");
            }
            return status;
        }

        public ActionResult GetCommonFilteredDiscussion(string jsonData)
        {
            //var lstdashboard ="";
            int calltype = 0;
            int SubCallType = 0;
            string DiscussionType;
            DiscussionType = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string LoginType = userDetails.LoginType;
            DiscussionViewModel ObjData = new DiscussionViewModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string fromDate = Convert.ToString(item["fromDate"]);
                string toDate = Convert.ToString(item["toDate"]);
                string groupnm = Convert.ToString(item["selectedgrp"]);
                if (!string.IsNullOrEmpty(Convert.ToString(item["selectedcall"])))
                    calltype = Convert.ToInt32(item["selectedcall"]);
                string status = Convert.ToString(item["selectedstatus"]);
                string raisedby = Convert.ToString(item["selectedRaisedBy"]);
                string AssignMember = Convert.ToString(item["selectedMemberAssign"]);
                if (!string.IsNullOrEmpty(Convert.ToString(item["SubCallType"])))
                    SubCallType = Convert.ToInt32(item["SubCallType"]);
                if(!string.IsNullOrEmpty(Convert.ToString(item["DiscussionType"])))
                    DiscussionType = Convert.ToString(item["DiscussionType"]);

                ObjData.lstDiscussions = DR.GetfilteredDiscussionDataAssign(status, calltype, SubCallType, groupnm, fromDate, toDate, raisedby, userDetails.LoginType, userDetails.LoginId, false, AssignMember, DiscussionType);
                ObjData.lstFollowUpsDiscussions = DR.GetfilteredDiscussionDataAssign(status, calltype, SubCallType, groupnm, fromDate, toDate, raisedby, userDetails.LoginType, userDetails.LoginId, true, AssignMember, DiscussionType);
            }
            return PartialView("_CommonDiscussionList", ObjData);
        }
        public ActionResult ExportToExcelCommonFilteredDiscussion(string fromdt, string Todt, string Groupnm, int calltype, int subcalltype, string status, string raised, string AssignMember, string ReportName,string DiscussionType)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string LoginType = userDetails.LoginType;
            try
            {
                List<DiscussionDetails> lstdashboard = new List<DiscussionDetails>();
                // List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
                // lstOutletWiseTransaction = RR.GetOutletWiseTransactionList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, outletId, EnrolmentDataFlag, userDetails.connectionString);
                lstdashboard = DR.GetfilteredDiscussionDataAssign(status, calltype, subcalltype, Groupnm, fromdt, Todt, raised, LoginType, userDetails.LoginId, false, AssignMember, DiscussionType);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(DiscussionDetails));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (DiscussionDetails item in lstdashboard)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = ReportName;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                    worksheet.Cell(1, 1).Value = "Report Name";
                    worksheet.Cell(1, 2).Value = "Discussion Report";
                    worksheet.Cell(2, 1).Value = "Group Name";
                    worksheet.Cell(2, 2).Value = Groupnm;
                    worksheet.Cell(3, 1).Value = "Period";
                    worksheet.Cell(3, 2).Value = fromdt + "-" + Todt;
                    worksheet.Cell(4, 1).Value = "Call Type";
                    worksheet.Cell(4, 2).Value = calltype;
                    worksheet.Cell(5, 1).Value = "Status";
                    worksheet.Cell(5, 2).Value = status;
                    worksheet.Cell(6, 1).Value = "Raised By";
                    worksheet.Cell(6, 2).Value = raised;
                    worksheet.Cell(9, 1).InsertTable(table);
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
                newexception.AddException(ex, "ExportToExcelCommonFilteredDiscussion");
                return null;
            }


        }

        public ActionResult GetDepartmentMember(string jsonData)
        {
            List<tblDepartMember> objDepartMem = new List<tblDepartMember>();
            // DiscussionViewModel ObjData = new DiscussionViewModel();

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string Department = Convert.ToString(item["Department"]);
                    objDepartMem = DR.GetMemberdetails(Department);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDepartmentMember");                
            }
            return new JsonResult() { Data = objDepartMem, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetReAssignMember(string jsonData)
        {
            List<SelectListItem> objDepartMem = new List<SelectListItem>();

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string id = Convert.ToString(item["id"]);
                    objDepartMem = DR.GetReAssignMemberdetails(id);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetReAssignMember");
            }
            return new JsonResult() { Data = objDepartMem, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetTaskCounts(string Department)
        {
            
                var TaskCount = DR.GetTaskCount(Department);
            return new JsonResult() { Data = TaskCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetDiscussionCustMobileNo(string CustName)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var CustMobileNo = DR.GetDiscussionCustMobile(CustName, userDetails.GroupId);
            return new JsonResult() { Data = CustMobileNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetDiscussionById(string Id)
        {
            BOTS_TblDiscussion objDiscussionData = new BOTS_TblDiscussion();
            objDiscussionData = DR.GetDiscussionById(Convert.ToInt32(Id));

            return new JsonResult() { Data = objDiscussionData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}