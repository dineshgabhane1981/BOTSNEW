using BOTS_BL;
using BOTS_BL.Models;
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
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class CampaignsController : Controller
        
    {
        CampaignRepository CMPR = new CampaignRepository();
        ReportsRepository RR = new ReportsRepository();
        Exceptions newexception = new Exceptions();
        // GET: Campaigns
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Campaign()
        {
            CampaignTiles objCampaignTiles = new CampaignTiles();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {                
                objCampaignTiles = CMPR.GetCampaignTilesData(userDetails.GroupId, userDetails.connectionString);
                List<SelectListItem> MonthList = new List<SelectListItem>();

                for (int i = 0; i < 12; i++)
                {
                    MonthList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddMonths(i).ToString("MMM")),
                        Value = Convert.ToString(DateTime.Now.AddMonths(i).Month)
                    });
                }
                List<SelectListItem> YearList = new List<SelectListItem>();
                int year = DateTime.Now.Year;
                objCampaignTiles.year = DateTime.Now.Year;
                objCampaignTiles.month = DateTime.Now.Month;
                for (int i = -5; i <= 9; i++)
                {
                    YearList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddYears(i).Year.ToString()),
                        Value = Convert.ToString(year + i)
                    });
                }


                objCampaignTiles.lstMonth = MonthList;
                objCampaignTiles.lstYear = YearList;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Campaign");
            }

            return View(objCampaignTiles);
        }

        [HttpPost]
        public JsonResult GetCampaignCelebrationsData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrations> objCampaignCelebrations = new List<CampaignCelebrations>();
            try
            {                               
                objCampaignCelebrations = CMPR.GetCampaignCelebrationsData(userDetails.GroupId, userDetails.connectionString, month, year);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignCelebrationsData");
            }
            return new JsonResult() { Data = objCampaignCelebrations, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignCelebrationsSecondData(string month, string year, string type)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            try
            {
                objCampaignCelebrationsData = CMPR.GetCampaignCelebrationsSecondData(userDetails.GroupId, userDetails.connectionString, month, year, type);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignCelebrationsSecondData");
            }
            return new JsonResult() { Data = objCampaignCelebrationsData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignPointsExpiryData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignPointsExpiry> objCampaignPointsExpiry = new List<CampaignPointsExpiry>();
            try
            {
                objCampaignPointsExpiry = CMPR.GetCampaignPointsExpiryData(userDetails.GroupId, userDetails.connectionString, month, year);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignPointsExpiryData");
            }
            return new JsonResult() { Data = objCampaignPointsExpiry, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignPointsExpirySecondData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            try
            {
                objCampaignCelebrationsData = CMPR.GetCampaignPointsExpirySecondData(userDetails.GroupId, userDetails.connectionString, month, year);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignPointsExpirySecondData");
            }
            return new JsonResult() { Data = objCampaignCelebrationsData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignInactiveData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignInactive> objCampaignInactive = new List<CampaignInactive>();
            try
            {
                objCampaignInactive = CMPR.GetCampaignInactiveData(userDetails.GroupId, userDetails.connectionString, month, year);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignInactiveData");
            }
            return new JsonResult() { Data = objCampaignInactive, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignInactiveSecondData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignInactiveData> objCampaignInactiveData = new List<CampaignInactiveData>();
            try
            {
                objCampaignInactiveData = CMPR.GetCampaignInactiveSecondData(userDetails.GroupId, userDetails.connectionString, month, year);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignInactiveSecondData");
            }
            return new JsonResult() { Data = objCampaignInactiveData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignFirstData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<Campaign> objCampaignData = new List<Campaign>();
            try
            {
                objCampaignData = CMPR.GetCampaignFirstData(userDetails.GroupId, userDetails.connectionString, month, year);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignFirstData");
            }
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignSecondData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            try
            {
                objCampaignData = CMPR.GetCampaignSecondData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignSecondData");
            }
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignThirdData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            try
            {
                objCampaignData = CMPR.GetCampaignThirdData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignThirdData");
            }
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignSMSBlastFirstData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignSMSBlastFirst> objCampaignSMSBlastFirstData = new List<CampaignSMSBlastFirst>();
            try
            {
                objCampaignSMSBlastFirstData = CMPR.GetCampaignSMSBlastFirstData(userDetails.GroupId, userDetails.connectionString, month, year);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignSMSBlastFirstData");
            }
            return new JsonResult() { Data = objCampaignSMSBlastFirstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetSMSBlastsSecondData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            try
            {
                objCampaignData = CMPR.GetSMSBlastsSecondData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSMSBlastsSecondData");
            }
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetSMSBlastsThirdData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            try
            {
                objCampaignData = CMPR.GetSMSBlastsThirdData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSMSBlastsThirdData");
            }
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CreateCampaign()
        {
            SMSDetailsTemp SDT = new SMSDetailsTemp();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            var SMSGatewayDetails = CMPR.GatewayDetails(userDetails.GroupId, userDetails.connectionString);
            //var DataTemp = CMPR.GetWAInsData(userDetails.GroupId, userDetails.connectionString);
            try
            {
                if (SMSGatewayDetails.Count() > 0)
                {
                    for (int i = 0; i <= SMSGatewayDetails.Count(); i++)
                    {
                        if (i == 0)
                        {
                            SDT.BOCode = SMSGatewayDetails[i].BOCode;
                        }
                        if (i == 1)
                        {
                            SDT.SMSVendor = SMSGatewayDetails[i].smsBalance;
                        }
                    }

                    ViewBag.SMSVendor = SDT.BOCode;
                    ViewBag.SMSBalance = SDT.SMSVendor;
                    ViewBag.OutletData = lstOutlet;
                }
                else
                {
                    ViewBag.SMSVendor = "NA";
                    ViewBag.SMSBalance = "NA";
                    ViewBag.OutletData = lstOutlet;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CreateCampaign");
            }

            return View("CreateCampaign");
        }

        [HttpPost]
        public JsonResult GetFilteredData(string jsonData)
        {
            CustomerIdListAndCount objcounts = new CustomerIdListAndCount();
            CustCount objcustAll = new CustCount();
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CustomerDetail> listCustD = new List<CustomerDetail>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string BaseType = Convert.ToString(item["BaseType"]);
                    string PointsBase = Convert.ToString(item["PointsBase"]);
                    string Points = Convert.ToString(item["Points"]);
                    string PointsRange1 = Convert.ToString(item["PointsRange1"]);
                    string OutletId = Convert.ToString(item["OutletId"]);

                    objcustAll = CR.GetFiltData(BaseType, PointsBase, Points, PointsRange1, OutletId, userDetails.GroupId, userDetails.connectionString);

                    // Session["customerId"] = objcounts.lstcustomerDetails;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFilteredData");
            }

            return new JsonResult() { Data = objcustAll, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult SaveData(string jsonData)
        {
            List<CampaignSaveDetails> SaveData = new List<CampaignSaveDetails>();
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string BaseType = Convert.ToString(item["BaseType"]);
                    string Equality = Convert.ToString(item["Equality"]);
                    string Points = Convert.ToString(item["Points"]);
                    string OutletId = Convert.ToString(item["OutletId"]);
                    string Srcipt = Convert.ToString(item["Srcipt"]);
                    string StartDate = Convert.ToString(item["StartDate"]);
                    string EndDate = Convert.ToString(item["EndDate"]);
                    string CampaignName = Convert.ToString(item["CampaignName"]);
                    string SMSType = Convert.ToString(item["SMSType"]);
                    string ScriptType = Convert.ToString(item["ScriptType"]);
                    string Scheduledatetime = Convert.ToString(item["Scheduledatetime"]);
                    string TempId = Convert.ToString(item["TempId"]); //PointsRange1
                    string PointsRange1 = Convert.ToString(item["PointsRange1"]);

                    SaveData = CR.SaveCampaignData(BaseType, Equality, Points, OutletId, Srcipt, StartDate, EndDate, CampaignName, SMSType, ScriptType, Scheduledatetime, TempId, PointsRange1, userDetails.GroupId, userDetails.connectionString);
                    //Session["CampaignId"] = SaveData.;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveData");
            }

            return new JsonResult() { Data = SaveData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportToExcelCampaignData(string CampaignId)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string LoginType = userDetails.LoginType;


            try
            {

                // List<OutletwiseTransaction> lstOutletWiseTransaction = new List<OutletwiseTransaction>();
                // lstOutletWiseTransaction = RR.GetOutletWiseTransactionList(userDetails.GroupId, DateRangeFlag, fromDate, toDate, outletId, EnrolmentDataFlag, userDetails.connectionString);
                var CD = CMPR.CampDataDownload(CampaignId, userDetails.GroupId, userDetails.connectionString);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(CampaignMemberDetail));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (CampaignMemberDetail item in CD)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)

                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    // if(row[prop.Name] == "Mobileno")
                    //{
                    table.Rows.Add(row);
                    // }


                }
                table.Columns.Remove("SlNo");
                table.Columns.Remove("CampaignId");
                table.Columns.Remove("CustomerBaseType");
                table.Columns.Remove("MemberQualifiedStatus");

                string fileName = "BOTS_" + CampaignId + ".xlsx";

                using (XLWorkbook wb = new XLWorkbook())
                {

                    //excelSheet.Name
                    table.TableName = CampaignId;
                    IXLWorksheet worksheet = wb.AddWorksheet(sheetName: CampaignId);

                    worksheet.Cell(1, 1).InsertTable(table);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
                // return null;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelCampaignData");
                return null;
            }
        }

        public ActionResult CampaignManagement()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var ListCampDetails = CMPR.GetCampList(userDetails.GroupId, userDetails.connectionString);
            try
            {
                ViewBag.ListCampDetails = ListCampDetails;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CampaignManagement");               
            }

            return View("CampaignManagement");
        }

        [HttpPost]
        public JsonResult SendToDLT(string jsonData)
        {
            //SPResponse response = new SPResponse();
            bool Status;
            Status = false;
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string CampaignId = Convert.ToString(item["CampaignId"]);

                    var response = CR.SendDLTData(CampaignId, userDetails.GroupId, userDetails.connectionString);
                    //Session["CampaignId"] = SaveData.;
                    Status = response;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendToDLT");
            }
            return new JsonResult() { Data = Status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CampDLTList()
        {
            CampaignRepository CR = new CampaignRepository();
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstDLTDetails = CR.CampDLTDetailsLst(userDetails.GroupId, userDetails.connectionString);
            //  ViewBag.DLTDetails = lstDLTDetails;
            try
            {
                ViewBag.TempleteType = objData.TempleteType();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CampDLTList");
            }
            return View("CampaignDLTList", lstDLTDetails);
        }

        public ActionResult UpdateCampDLCLinkDLTStatus(string Campid, string status, string reason)
        {
            //bool result = false;
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                var Response = CR.UpdateCampDLCLinkDLTStatus(Campid, status, reason, userDetails.GroupId, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateCampDLCLinkDLTStatus");
            }
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult UpdateCampDLTRejectStatus(string Campid, string status, string reason)
        {
            //bool result = false;
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                var Response = CR.UpdateCampDLTRejectStat(Campid, status, reason, userDetails.GroupId, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateCampDLTRejectStatus");
            }
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveCampDLCDetails(string Campid, string status, string jsonData)
        {
            bool result = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<DLTDetailsLst> CampDetails = new List<DLTDetailsLst>();
                CampaignRepository CR = new CampaignRepository();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    DLTDetailsLst CP = new DLTDetailsLst();
                    if (CampDetails != null)
                    {

                        CP.Script = Convert.ToString(item["Script"]);
                        CP.DLTScript = Convert.ToString(item["ScriptDLT"]);
                        CP.TemplateID = Convert.ToString(item["TemplateId"]);
                        CP.TemplateName = Convert.ToString(item["TemplateName"]);
                        CP.TemplateType = Convert.ToString(item["TemplateType"]);
                        if (Convert.ToString(status) == "Approved")
                        {
                            CP.DLTStatus = "Approved";
                        }
                        CampDetails.Add(CP);
                    }
                }
                result = CR.SaveDLCCampaignDetails(Campid, CampDetails, userDetails.GroupId, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCampDLCDetails");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult SendTestSMS(string jsonData)
        {
            //SPResponse response = new SPResponse();
            //bool Status;
            //Status = false;
            CampaignRepository CR = new CampaignRepository();
            List<CampaignSaveDetails> responsedata = new List<CampaignSaveDetails>();
            DataSet response = new DataSet();

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string CampaignId = Convert.ToString(item["CampaignId"]);
                    string TestNumber = Convert.ToString(item["TestNumber"]);
                    response = CR.SendTestSMSData(CampaignId, TestNumber, userDetails.GroupId, userDetails.connectionString);
                    //Session["CampaignId"] = SaveData.;
                    //responsedata = response;
                    DataTable DT = response.Tables["Table"];

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        CampaignSaveDetails SaveData = new CampaignSaveDetails();
                        SaveData.ResponseCode = DT.Rows[i]["ResponseCode"].ToString();
                        SaveData.ResponseMessage = DT.Rows[i]["ResponseMessage"].ToString();
                        SaveData.CampaignId = DT.Rows[i]["CampaignId"].ToString();
                        responsedata.Add(SaveData);
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendTestSMS");
            }

            return new JsonResult() { Data = responsedata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult SendSMS(string jsonData)
        {
            //SPResponse response = new SPResponse();
            //bool Status;
            //Status = false;
            CampaignRepository CR = new CampaignRepository();
            List<CampaignSaveDetails> responsedata = new List<CampaignSaveDetails>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string CampaignId = Convert.ToString(item["CampaignId"]);

                    responsedata = CR.SendSMSData(CampaignId, userDetails.GroupId, userDetails.connectionString);
                    //Session["CampaignId"] = SaveData.;
                    //responsedata = response;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendSMS");
            }

            return new JsonResult() { Data = responsedata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult UploadData()
        {
            List<JsonData> JSData = new List<JsonData>();
            if (Request.Files.Count > 0)
            {
                try
                {
                    DataSet ds = new DataSet();

                    HttpPostedFileBase files = Request.Files[0];
                    string fileName = Request.Files[0].FileName;
                    var path = ConfigurationManager.AppSettings["Path"].ToString();
                    string Path3 = ConfigurationManager.AppSettings["Path3"].ToString();
                    var fullFilePath = path + fileName;

                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    if (!file.Exists)
                    {
                        files.SaveAs(path + fileName);
                        string _Url = Path3 + fileName;
                        //var jsonData1 = "{\"ResposeCode\":\"00\","+"\"Message\":\"File Uploaded Successfully!\","+"\"Url\":\""+ fullFilePath + "\"}";

                        //return Json(jsonData1);
                        for (int i = 1; i <= 1; i++)
                        {
                            JsonData Temp = new JsonData();
                            Temp.ResponseCode = "00";
                            Temp.ResponseMessage = "Success";
                            Temp.Url = _Url;

                            JSData.Add(Temp);
                        }
                        return Json(JSData);
                    }
                    else
                    {
                        //ViewBag.Message = "File Already Exists";
                        //var jsonData2 = @"{'ResposeCode':'01','Message':'File Already Exists'}";
                        //return Json(jsonData2);
                        for (int i = 1; i <= 1; i++)
                        {
                            JsonData Temp = new JsonData();
                            Temp.ResponseCode = "01";
                            Temp.ResponseMessage = "File Already Exists";

                            JSData.Add(Temp);
                        }
                        return Json(JSData);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UploadData");
                }
            }
            //var jsonData = @"{'ResponseCode':'01','Message':'File Not Uploaded Successfully!'}";
            //return Json(jsonData);
            for (int i = 1; i <= 1; i++)
            {
                JsonData Temp = new JsonData();
                Temp.ResponseCode = "01";
                Temp.ResponseMessage = "File Not Uploaded Successfully!";

                JSData.Add(Temp);
            }
            return Json(JSData);
        }

        [HttpPost]
        public JsonResult SaveDataWA(string jsonData)
        {
            List<CampaignSaveDetails> SaveData = new List<CampaignSaveDetails>();
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string BaseType = Convert.ToString(item["BaseType"]);
                    string Equality = Convert.ToString(item["Equality"]);
                    string Points = Convert.ToString(item["Points"]);
                    string OutletId = Convert.ToString(item["OutletId"]);
                    string Srcipt = Convert.ToString(item["Srcipt"]);
                    string StartDate = Convert.ToString(item["StartDate"]);
                    string EndDate = Convert.ToString(item["EndDate"]);
                    string CampaignName = Convert.ToString(item["CampaignName"]);
                    string SMSType = Convert.ToString(item["SMSType"]);
                    string MessageType = Convert.ToString(item["MessageType"]);
                    string Scheduledatetime = Convert.ToString(item["Scheduledatetime"]);
                    //string TempId = Convert.ToString(item["TempId"]); //PointsRange1
                    string PointsRange1 = Convert.ToString(item["PointsRange1"]);
                    string FileUrlLink = Convert.ToString(item["FileUrlink"]);

                    SaveData = CR.SaveCampaignDataWA(BaseType, Equality, Points, OutletId, Srcipt, StartDate, EndDate, CampaignName, SMSType, MessageType, Scheduledatetime, PointsRange1, FileUrlLink, userDetails.GroupId, userDetails.connectionString);
                    //Session["CampaignId"] = SaveData.;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDataWA");
            }

            return new JsonResult() { Data = SaveData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult WASendTestSMS(string jsonData)
        {
            //SPResponse response = new SPResponse();
            //bool Status;
            //Status = false;
            CampaignRepository CR = new CampaignRepository();
            List<CampaignSaveDetails> responsedata = new List<CampaignSaveDetails>();
            DataSet response = new DataSet();

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string CampaignId = Convert.ToString(item["WACampId"]);
                    string TestNumber = Convert.ToString(item["WATestNumbers"]);
                    response = CR.WASendTestMsgData(CampaignId, TestNumber, userDetails.GroupId, userDetails.connectionString);
                    //Session["CampaignId"] = SaveData.;
                    //responsedata = response;
                    DataTable DT = response.Tables["Table"];

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        CampaignSaveDetails SaveData = new CampaignSaveDetails();
                        SaveData.ResponseCode = DT.Rows[i]["ResponseCode"].ToString();
                        SaveData.ResponseMessage = DT.Rows[i]["ResponseMessage"].ToString();
                        SaveData.CampaignId = DT.Rows[i]["CampaignId"].ToString();
                        responsedata.Add(SaveData);
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "WASendTestSMS");
            }

            return new JsonResult() { Data = responsedata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult WAInstanceData(string jsonData)
        {
            List<WAInsData> responsedata = new List<WAInsData>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                responsedata = CMPR.GetWAInsData(userDetails.GroupId, userDetails.connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "WAInstanceData");
            }
            //ViewBag.ListCampDetails = WAInsDetails;

            return new JsonResult() { Data = responsedata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CampaignReport()
        {
            return View();
        }
        public ActionResult GetCampaignCelebrations(string flag, string stryear, string strmonth)
        {
            List<CelebrationSummary> objData = new List<CelebrationSummary>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CMPR.GetCampaignCelebrationSummery(userDetails.GroupId, flag, stryear, strmonth);
                List<SelectListItem> MonthList = new List<SelectListItem>();
                int month = DateTime.Now.Month;

                int count = 1;
                for (int i = 0; i <= 11; i++)
                {
                    MonthList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i]),
                        Value = Convert.ToString(i)
                    });
                    count++;
                }
                List<SelectListItem> YearList = new List<SelectListItem>();
                int year = DateTime.Now.Year;
                for (int i = 0; i <= 9; i++)
                {
                    YearList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddYears(-i).Year.ToString()),
                        Value = Convert.ToString(year - i)
                    });
                }

                ViewBag.MonthList = MonthList;
                ViewBag.YearList = YearList;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignCelebrations");
            }
            return PartialView("_CelebrationSummary", objData);
        }
        public ActionResult GetCampaignCelebrationDetails(string flag,string type, string year, string month)
        {
            List<CelebrationDetail> objData = new List<CelebrationDetail>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CMPR.GetCampaignCelebrationDetail(userDetails.GroupId, flag, type, year, month);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignCelebrationDetails");
            }
            return PartialView("_CelebrationDetail", objData);
        }

        public ActionResult GetCampaignPointExpirySummary(string flag, string stryear, string strmonth)
        {
            List<PointExpirySummary> objData = new List<PointExpirySummary>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData = CMPR.GetCampaignPointExpirySummary(userDetails.GroupId, flag, stryear, strmonth);
            List<SelectListItem> MonthList = new List<SelectListItem>();
            int month = DateTime.Now.Month;

            int count = 1;
            try
            {
                for (int i = 0; i <= 11; i++)
                {
                    MonthList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i]),
                        Value = Convert.ToString(i)
                    });
                    count++;
                }
                List<SelectListItem> YearList = new List<SelectListItem>();
                int year = DateTime.Now.Year;
                for (int i = 0; i <= 9; i++)
                {
                    YearList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddYears(-i).Year.ToString()),
                        Value = Convert.ToString(year - i)
                    });
                }

                ViewBag.MonthList = MonthList;
                ViewBag.YearList = YearList;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignPointExpirySummary");
            }
            return PartialView("_PointsExpirySummary", objData);
        }

        public ActionResult GetCampaignPointExpiryDetails(string flag, string year, string month)
        {
            List<PointExpiryDetailed> objData = new List<PointExpiryDetailed>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CMPR.GetCampaignPointExpiryDetailed(userDetails.GroupId, flag, year, month);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignPointExpiryDetails");
            }
            return PartialView("_PointsExpiryDetail", objData);
        }

        public ActionResult GetCampaignInactiveSummary(string flag, string stryear, string strmonth)
        {
            List<InactiveSummary> objData = new List<InactiveSummary>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objData = CMPR.GetCampaignInactiveSummary(userDetails.GroupId, flag, stryear, strmonth);
            List<SelectListItem> MonthList = new List<SelectListItem>();
            int month = DateTime.Now.Month;

            int count = 1;
            try
            {
                for (int i = 0; i <= 11; i++)
                {
                    MonthList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i]),
                        Value = Convert.ToString(i)
                    });
                    count++;
                }
                List<SelectListItem> YearList = new List<SelectListItem>();
                int year = DateTime.Now.Year;
                for (int i = 0; i <= 9; i++)
                {
                    YearList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddYears(-i).Year.ToString()),
                        Value = Convert.ToString(year - i)
                    });
                }

                ViewBag.MonthList = MonthList;
                ViewBag.YearList = YearList;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignInactiveSummary");
            }
            return PartialView("_InactiveSummary", objData);
        }

        public ActionResult GetCampaignInactiveDetails(string flag, string year, string month)
        {
            List<InactiveDetailed> objData = new List<InactiveDetailed>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CMPR.GetCampaignInactiveDetailed(userDetails.GroupId, flag, year, month);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignInactiveDetails");
            }
            return PartialView("_InactiveDetail", objData);
        }

        public ActionResult GetCampaignSummary(string flag, string stryear, string strmonth)
        {
            List<CampaignSummary> objData = new List<CampaignSummary>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CMPR.GetCampaignSummary(userDetails.GroupId, flag, stryear, strmonth);
                List<SelectListItem> MonthList = new List<SelectListItem>();
                int month = DateTime.Now.Month;

                int count = 1;
                for (int i = 0; i <= 11; i++)
                {
                    MonthList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i]),
                        Value = Convert.ToString(i)
                    });
                    count++;
                }
                List<SelectListItem> YearList = new List<SelectListItem>();
                int year = DateTime.Now.Year;
                for (int i = 0; i <= 9; i++)
                {
                    YearList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddYears(-i).Year.ToString()),
                        Value = Convert.ToString(year - i)
                    });
                }

                ViewBag.MonthList = MonthList;
                ViewBag.YearList = YearList;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignSummary");
            }
            return PartialView("_CampaignSummary", objData);
        }

        public ActionResult GetCampaignDetails(string CampaignId)
        {
            List<CampaignDetailed> objData = new List<CampaignDetailed>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CMPR.GetCampaignDetailed(userDetails.GroupId, CampaignId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignDetails");
            }
            return PartialView("_CampaignDetail", objData);
        }

        public ActionResult GetCampaignPromoBlastSummary(string flag, string stryear, string strmonth)
        {
            List<PromoBlastSummary> objData = new List<PromoBlastSummary>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                objData = CMPR.GetCampaignPromoBlastSummary(userDetails.GroupId, flag, stryear, strmonth);
                List<SelectListItem> MonthList = new List<SelectListItem>();
                int month = DateTime.Now.Month;

                int count = 1;
                for (int i = 0; i <= 11; i++)
                {
                    MonthList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i]),
                        Value = Convert.ToString(i)
                    });
                    count++;
                }
                List<SelectListItem> YearList = new List<SelectListItem>();
                int year = DateTime.Now.Year;
                for (int i = 0; i <= 9; i++)
                {
                    YearList.Add(new SelectListItem
                    {
                        Text = Convert.ToString(DateTime.Now.AddYears(-i).Year.ToString()),
                        Value = Convert.ToString(year - i)
                    });
                }

                ViewBag.MonthList = MonthList;
                ViewBag.YearList = YearList;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignPromoBlastSummary");
            }
            return PartialView("_CampaignPromoBlastSummary", objData);
        }

        public ActionResult GetPromoBlastDetails(string CampaignId)
        {
            List<PromoBlastDetails> objData = new List<PromoBlastDetails>();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                objData = CMPR.GetCampaignPromoBlastDetailed(userDetails.GroupId, CampaignId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPromoBlastDetails");
            }
            return PartialView("_CampaignPromoBlastDetails", objData);
        }

    }
}