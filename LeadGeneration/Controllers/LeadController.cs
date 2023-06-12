using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Models.SalesLead;
using BOTS_BL.Repository;
using LeadGeneration.ViewModel;

using System.IO;
using ClosedXML.Excel;

namespace LeadGeneration.Controllers
{
    public class LeadController : Controller
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        SalesLeadRepository SLR = new SalesLeadRepository();
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstsALES_TblLeads = SLR.GetFollowupLeads(userDetails);
            objviewmodel.lstCity = CR.GetCity();
            objviewmodel.lstBillingPartner = CR.GetBillingPartner();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            return View(objviewmodel);

        }
        public ActionResult LeadTransfer()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstsALES_TblLeads = SLR.GetSalesLeads(userDetails);
            objviewmodel.lstCity = CR.GetCity();
            objviewmodel.lstBillingPartner = CR.GetBillingPartner();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            return View(objviewmodel);

        }
        public ActionResult AddLead(string leadId)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            SALES_tblLeads objData = new SALES_tblLeads();

            objviewmodel.lstBillingPartner = CR.GetBillingPartner();
            objviewmodel.lstcategory = CR.GetRetailCategory();           
            objviewmodel.lstCity = CR.GetCity();
            objviewmodel.sALES_TblLeads = objData;

            List<SelectListItem> refferedname = new List<SelectListItem>();            
            objviewmodel.lstLeadSourceNames = refferedname;

            return View(objviewmodel);
        }

        public ActionResult EditLead(string leadId)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            SALES_tblLeads objData = new SALES_tblLeads();
            if (!string.IsNullOrEmpty(leadId))
            {
                objviewmodel.lstBillingPartner = CR.GetBillingPartner();
                objviewmodel.lstcategory = CR.GetRetailCategory();                
                objviewmodel.lstCity = CR.GetCity();
                objData = SLR.GetsalesLeadByLeadId(Convert.ToInt32(leadId));
                if (objData.FollowupDate != null)
                    objData.FollowupDate = objData.FollowupDate.Value.Date;
                objData.Comments = "";
                objviewmodel.sALES_TblLeads = objData;

                List<SelectListItem> refferedname = new List<SelectListItem>();               
                objviewmodel.lstLeadSourceNames = refferedname;
            }

            return View(objviewmodel);
        }
        public ActionResult AddSalesLead(LeadViewModel objData)
        {
            int LeadId = 0;
            try
            {
                objData.lstBillingPartner = CR.GetBillingPartner();
                objData.lstcategory = CR.GetRetailCategory();
                objData.lstStates = CR.GetStates();
                objData.lstCity = CR.GetCity();
                List<SelectListItem> refferedname = new List<SelectListItem>();
                objData.lstLeadSourceNames = refferedname;
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                if (objData.sALES_TblLeads.LeadId == 0)
                {
                    var exist = SLR.isMobileNoExist(objData.sALES_TblLeads.MobileNo);
                    if (exist)
                    {
                        ViewData["Status"] = "exist";
                        return View("AddLead", objData);
                    }
                }
                objData.sALES_TblLeads.AddedDate = DateTime.Now;
                objData.sALES_TblLeads.AddedBy = userDetails.LoginId;
                objData.sALES_TblLeads.AssignedLead = userDetails.LoginId;
                var meetingType = objData.sALES_TblLeads.MeetingType;
                LeadId = SLR.AddSalesLead(objData.sALES_TblLeads);
                if (meetingType == "salesdone")
                {
                    string url = ConfigurationManager.AppSettings["BOTSURL"].ToString();
                    //string url = "https://blueocktopus.in/bots?LoginID=" + userDetails.LoginId + "&LeadId=" + objData.sALES_TblLeads.LeadId + "";
                    url = url + "?LoginID=" + userDetails.LoginId + "";
                    ViewData["LeadId"] = LeadId;
                    ViewData["URL"] = url;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["error"] = "Error Occured";
                // return View("AddLead");
            }
            ViewData["Status"] = LeadId;
            return View("AddLead", objData);

        }

        public ActionResult GetSearchLeads(string searchData)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(searchData);
            foreach (Dictionary<string, object> item in objData)
            {
                string salesManager = string.Empty;
                if (item.ContainsKey("SalesManager"))
                {
                    salesManager = Convert.ToString(item["SalesManager"]);
                }
                else
                {
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    salesManager = userDetails.LoginId;
                }
                objviewmodel.lstsALES_TblLeads = SLR.GetSearchedLeads(Convert.ToString(item["MobileNo"]), Convert.ToString(item["BusinessName"]),
                Convert.ToString(item["DtFrom"]), Convert.ToString(item["DtTo"]), Convert.ToString(item["LeadStatus"]),
                Convert.ToString(item["ContactType"]), Convert.ToString(item["MeetingType"]), Convert.ToString(item["City"]),
                Convert.ToString(item["BillingPartner"]), salesManager, Convert.ToString(item["LeadType"]));
            }

            return PartialView("_SearchLeadListing", objviewmodel);
        }

        public ActionResult GetSearchLeadsforLeadTransfer(string searchData)
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(searchData);
                foreach (Dictionary<string, object> item in objData)
                {
                    objviewmodel.lstsALES_TblLeads = SLR.GetSearchedLeads(Convert.ToString(item["MobileNo"]), Convert.ToString(item["BusinessName"]),
                        Convert.ToString(item["DtFrom"]), Convert.ToString(item["DtTo"]), Convert.ToString(item["LeadStatus"]),
                        Convert.ToString(item["ContactType"]), Convert.ToString(item["MeetingType"]), Convert.ToString(item["City"]),
                        Convert.ToString(item["BillingPartner"]), Convert.ToString(item["SalesManager"]), "");
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Get Lead");
            }

            return PartialView("_LeadTransferList", objviewmodel);
        }

        public JsonResult GetLeadTrackingList(string Id)
        {
            List<LeadTracking> lstLeadTracking = new List<LeadTracking>();
            lstLeadTracking = SLR.GetLeadTracking(Id);

            return new JsonResult() { Data = lstLeadTracking, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult TransferLead(int[] LeadId, string SaleManagerId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            LeadViewModel objviewmodel = new LeadViewModel();
            bool result = SLR.LeadTransfer(LeadId, SaleManagerId, userDetails.LoginId);
            return PartialView("_LeadTransferList", objviewmodel);
        }

        public ActionResult SalesCount()
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            return View(objviewmodel);
        }

        public JsonResult GetSalesCount(DateTime Fromdate, DateTime ToDate, string SalesManager)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (userDetails.LoginType != "1" && userDetails.LoginType != "5")
            {
                SalesManager = userDetails.LoginId;
            }
            List<SalesCount> lstsalescount = SLR.GetSalesCounts(Fromdate, ToDate, SalesManager);
            return new JsonResult() { Data = lstsalescount, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

        [HttpPost]
        public JsonResult GetRefferedName(string SourceType)
        {
            var lstRefferedName = SLR.GetRefferedName(SourceType);
            return new JsonResult() { Data = lstRefferedName, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SalesReport()
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            objviewmodel.lstBillingPartner = CR.GetBillingPartner();
            objviewmodel.lstcategory = CR.GetRetailCategory();
            objviewmodel.lstCity = CR.GetCity();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            SALES_tblLeads objData = new SALES_tblLeads();
            objviewmodel.sALES_TblLeads = objData;
            return View(objviewmodel);
        }

        public ActionResult SalesReportData(string SalesManager,string City,string Category,string BillingPartner,string LeadSource,string LeadStatus)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstData = SLR.GetNewReport(SalesManager, City, Category, BillingPartner, LeadSource, LeadStatus, userDetails.LoginType, userDetails.UserName);
            return PartialView("_Report", lstData);
        }

        public ActionResult SalesReportDataExport(string SalesManager, string City, string Category, string BillingPartner, string LeadSource, string LeadStatus)
        {            
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                var lstData = SLR.GetNewReport(SalesManager, City, Category, BillingPartner, LeadSource, LeadStatus, userDetails.LoginType, userDetails.UserName);
                
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(NewReport));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (NewReport item in lstData)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }               
                table.Columns.Remove("UserName");
                string ReportName = "SalesLeadData";
                string fileName = "BOTS_" + ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {                   
                    //excelSheet.Name
                    table.TableName = ReportName;

                    wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);                        
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "SalesReportDataExport");
                return null;
            }             
        }

        public ActionResult ReferralLead()
        {
            LeadViewModel objviewmodel = new LeadViewModel();
            SALES_tblLeads objData = new SALES_tblLeads();

            objviewmodel.lstBillingPartner = CR.GetBillingPartner();
            objviewmodel.lstcategory = CR.GetRetailCategory();            
            objviewmodel.lstCity = CR.GetCity();
            objviewmodel.lstSalesManager = SLR.GetSalesManager();
            objviewmodel.sALES_TblLeads = objData;

            List<SelectListItem> refferedname = new List<SelectListItem>();
            objviewmodel.lstLeadSourceNames = refferedname;

            return View(objviewmodel);
        }

        public ActionResult SaveReferralLead(string jsonData)
        {
            int status = 0;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                SALES_tblLeads objlead = new SALES_tblLeads();
                objlead.BusinessName= Convert.ToString(item["BusinessName"]);
                objlead.Category = Convert.ToString(item["Category"]);
                objlead.City = Convert.ToString(item["City"]);
                objlead.AssignedLead = Convert.ToString(item["AddedBy"]);
                objlead.SpokeWith = Convert.ToString(item["ContactPerson"]);
                objlead.MobileNo = Convert.ToString(item["ContactNo"]);
                objlead.LeadSource = Convert.ToString(item["LeadSource"]);
                objlead.LeadSourceName = Convert.ToString(item["LeadSourceName"]);
                objlead.AddedBy = userDetails.LoginId;
                objlead.FollowupDate = DateTime.Now.AddDays(1);
                var exist = SLR.isMobileNoExist(objlead.MobileNo);
                if (!exist)
                {
                    status = SLR.AddSalesLead(objlead);
                }
                else
                {
                    status = -1;
                }
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


    }
}