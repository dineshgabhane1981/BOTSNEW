using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
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

            return View(objCampaignTiles);
        }

        [HttpPost]
        public JsonResult GetCampaignCelebrationsData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrations> objCampaignCelebrations = new List<CampaignCelebrations>();
            objCampaignCelebrations = CMPR.GetCampaignCelebrationsData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignCelebrations, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignCelebrationsSecondData(string month, string year, string type)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            objCampaignCelebrationsData = CMPR.GetCampaignCelebrationsSecondData(userDetails.GroupId, userDetails.connectionString, month, year, type);
            return new JsonResult() { Data = objCampaignCelebrationsData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignPointsExpiryData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignPointsExpiry> objCampaignPointsExpiry = new List<CampaignPointsExpiry>();
            objCampaignPointsExpiry = CMPR.GetCampaignPointsExpiryData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignPointsExpiry, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignPointsExpirySecondData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            objCampaignCelebrationsData = CMPR.GetCampaignPointsExpirySecondData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignCelebrationsData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignInactiveData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignInactive> objCampaignInactive = new List<CampaignInactive>();
            objCampaignInactive = CMPR.GetCampaignInactiveData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignInactive, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignInactiveSecondData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignInactiveData> objCampaignInactiveData = new List<CampaignInactiveData>();
            objCampaignInactiveData = CMPR.GetCampaignInactiveSecondData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignInactiveData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignFirstData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<Campaign> objCampaignData = new List<Campaign>();
            objCampaignData = CMPR.GetCampaignFirstData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignSecondData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            objCampaignData = CMPR.GetCampaignSecondData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignThirdData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            objCampaignData = CMPR.GetCampaignThirdData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignSMSBlastFirstData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignSMSBlastFirst> objCampaignSMSBlastFirstData = new List<CampaignSMSBlastFirst>();
            objCampaignSMSBlastFirstData = CMPR.GetCampaignSMSBlastFirstData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignSMSBlastFirstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetSMSBlastsSecondData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            objCampaignData = CMPR.GetSMSBlastsSecondData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetSMSBlastsThirdData(string month, string year, string CampaignId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            objCampaignData = CMPR.GetSMSBlastsThirdData(userDetails.GroupId, userDetails.connectionString, month, year, CampaignId);
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CreateCampaign()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.OutletData = lstOutlet;

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
            foreach (Dictionary<string, object> item in objData)
            {               
                string BaseType = Convert.ToString(item["BaseType"]);
                string PointsBase = Convert.ToString(item["PointsBase"]);
                string Points = Convert.ToString(item["Points"]);
                string OutletId = Convert.ToString(item["OutletId"]);

                objcustAll = CR.GetFiltData(BaseType, PointsBase, Points, OutletId, userDetails.GroupId, userDetails.connectionString);

               // Session["customerId"] = objcounts.lstcustomerDetails;
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
                //string ScriptType = Convert.ToString(item["ScriptType"]);
                //string Scheduledatetime = Convert.ToString(item["Scheduledatetime"]);

                SaveData = CR.SaveCampaignData(BaseType, Equality, Points, OutletId,Srcipt, StartDate, EndDate, CampaignName, SMSType, userDetails.GroupId, userDetails.connectionString);
                //Session["CampaignId"] = SaveData.;
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
                newexception.AddException(ex, userDetails.GroupId);
                return null;
            }
        }

        public ActionResult CampaignManagement()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var ListCampDetails = CMPR.GetCampList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.ListCampDetails = ListCampDetails;

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
            foreach (Dictionary<string, object> item in objData)
            {
                string CampaignId = Convert.ToString(item["CampaignId"]);

               var response = CR.SendDLTData(CampaignId, userDetails.GroupId, userDetails.connectionString);
                //Session["CampaignId"] = SaveData.;
                Status = response;
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
            ViewBag.TempleteType = objData.TempleteType();

            return View("CampaignDLTList", lstDLTDetails);
        }

        public ActionResult UpdateCampDLCLinkDLTStatus(string Campid, string status, string reason)
        {
            //bool result = false;
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var Response = CR.UpdateCampDLCLinkDLTStatus(Campid, status, reason, userDetails.GroupId, userDetails.connectionString);
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult UpdateCampDLTRejectStatus(string Campid, string status, string reason)
        {
            //bool result = false;
            CampaignRepository CR = new CampaignRepository();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var Response = CR.UpdateCampDLTRejectStat(Campid, status, reason, userDetails.GroupId, userDetails.connectionString);
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
                        //objDLCLinkConfig.UpdatedBy = userDetails.LoginId;
                        //objDLCLinkConfig.UpdatedDate = DateTime.Now;
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
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string CampaignId = Convert.ToString(item["CampaignId"]);

                responsedata = CR.SendTestSMSData(CampaignId, userDetails.GroupId, userDetails.connectionString);
                //Session["CampaignId"] = SaveData.;
                //responsedata = response;
            }

            return new JsonResult() { Data = responsedata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}