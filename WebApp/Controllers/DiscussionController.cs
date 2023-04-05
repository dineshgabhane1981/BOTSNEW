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
            
            string LoginType = userDetails.LoginType; 
            
            Session["buttons"] = "Discussion";

            DiscussionViewModel objData = new DiscussionViewModel();
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
            objData.lstDiscussions = DR.GetDiscussions(groupId, LoginType);
            objData.lstCallTypes = DR.GetCallTypes(LoginType);
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
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            userDetails.CustomerName = "";
            string LoginType = userDetails.LoginType;
            DiscussionViewModel ObjData = new DiscussionViewModel();
            ViewBag.lstcommonstatus = DR.CommonStatus();
            ViewBag.lstgroupdetails = DR.GetGroupDetails();
            ViewBag.lstCallTypes = DR.GetCallTypes(LoginType);
            ViewBag.lstRMAssigned = DR.GetRaisedby();
            ObjData.lstDiscussions = DR.GetfilteredDiscussionData("", 0, "", "", "","", LoginType, userDetails.LoginId,false);
            ObjData.lstFollowUpsDiscussions = DR.GetfilteredDiscussionData("", 0, "", "", "", "", LoginType, userDetails.LoginId, true);
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
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData.objDiscussion.AddedDate = DateTime.Now;
            objData.objDiscussion.UpdatedDate = DateTime.Now;
            objData.objDiscussion.AddedBy = userDetails.LoginId;

            objData.objDiscussion.Department = objData.dept;
            objData.objDiscussion.Member = objData.Member;
            objData.objDiscussion.Priority = objData.prior;
            string File = objData.File;
            string FileName = objData.FileName;
            status = DR.AddDiscussions(objData.objDiscussion, File, FileName);

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
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string LoginType = userDetails.LoginType;
            var lstDiscussions = DR.GetDiscussions(groupId, LoginType);
            return PartialView("_DiscussionList", lstDiscussions);
        }

        [HttpPost]
        public bool UpdateStatusAndDiscussion(string dId, string Desc, string Status,string FollowupDate,string Reassign,string FileName, string File)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            status = DR.UpdateDiscussions(dId, Desc, Status, userDetails.LoginId, FollowupDate, Reassign, FileName, File);

            return status;
        }

        public ActionResult GetCommonFilteredDiscussion(string jsonData)
        {
            //var lstdashboard ="";
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
                int calltype = Convert.ToInt32(item["selectedcall"]);
                string status = Convert.ToString(item["selectedstatus"]);
                string raisedby = Convert.ToString(item["selectedRaisedBy"]);

                ObjData.lstDiscussions = DR.GetfilteredDiscussionData(status, calltype, groupnm, fromDate, toDate, raisedby, userDetails.LoginType, userDetails.LoginId, false);
                ObjData.lstFollowUpsDiscussions = DR.GetfilteredDiscussionData(status, calltype, groupnm, fromDate, toDate, raisedby, userDetails.LoginType, userDetails.LoginId, true);

                //lstdashboard = DR.GetfilteredDiscussionData(status, calltype, groupnm, fromDate, toDate, raisedby, userDetails.LoginType, userDetails.LoginId,false);
            }
            return PartialView("_CommonDiscussionList", ObjData);
        }
        public ActionResult ExportToExcelCommonFilteredDiscussion(string fromdt, string Todt, string Groupnm, int calltype, string status, string raised,string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string LoginType = userDetails.LoginType;
            try
            {
                List<DiscussionDetails> lstdashboard = new List<DiscussionDetails>();
               // List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
               // lstOutletWiseTransaction = RR.GetOutletWiseTransactionList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, outletId, EnrolmentDataFlag, userDetails.connectionString);
                lstdashboard = DR.GetfilteredDiscussionData(status, calltype, Groupnm, fromdt, Todt, raised, LoginType, userDetails.LoginId,false);
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
                newexception.AddException(ex, userDetails.GroupId);
                return null;
            }


        }
    
        public ActionResult GetDepartmentMember(string jsonData)
        {
            List < tblDepartMember > objDepartMem = new List<tblDepartMember>();
           // DiscussionViewModel ObjData = new DiscussionViewModel();

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string Department = Convert.ToString(item["Department"]);
                objDepartMem = DR.GetMemberdetails(Department);
            }

                return new JsonResult() { Data = objDepartMem, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetReAssignMember(string jsonData)
        {
            List<tblDepartMember> objDepartMem = new List<tblDepartMember>();

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string id = Convert.ToString(item["id"]);
                objDepartMem = DR.GetReAssignMemberdetails(id);
            }

            return new JsonResult() { Data = objDepartMem, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}