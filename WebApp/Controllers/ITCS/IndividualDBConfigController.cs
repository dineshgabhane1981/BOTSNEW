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

namespace WebApp.Controllers.ITCS
{
    public class IndividualDBConfigController : Controller
    {
        ITCSRepository ITCSR = new ITCSRepository();
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: IndividualDBConfig
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangeWAScript()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);
        }
        public ActionResult GetWAScripts(string OutletId, string MessageType)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objSMSWhatsAppScriptMaster = ITCSR.GetWAScripts(Convert.ToInt32(userDetails.GroupId), OutletId, MessageType);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveScripts(int OutletId, string Script, string MessageType)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];           
            try
            {
                result = ITCSR.SaveScripts(Convert.ToInt32(userDetails.GroupId), OutletId, Script, MessageType);
                tblAuditC objData = new tblAuditC();
                objData.GroupId = Convert.ToString(userDetails.GroupId);
                objData.RequestedFor = "Change WA Script";
                objData.RequestedBy = userDetails.UserName;
                objData.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveScripts");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult DisableSMS()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);

        }
        public ActionResult GetChangeNameData(string MobileNo)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            MemberData objCustomerDetail = new MemberData();            
            try
            {
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = ITCSR.GetChangeNameByMobileNo(userDetails.GroupId, MobileNo);
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameData");
            }
            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DisablePromotionalSMS(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var result = false;
            string GroupId, MobileNo;
            GroupId = string.Empty;
            MobileNo = string.Empty;
            bool DisableSMSWAPromo = default;
            try
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = userDetails.GroupId;
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    DisableSMSWAPromo = Convert.ToBoolean(item["DisableSMSWAPromo"]);

                }

                result = ITCSR.DisablePromotionalSMS(GroupId, MobileNo, DisableSMSWAPromo);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Disable WaPromo -" + MobileNo;
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisablePromotionalSMS");
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DisableTransactions()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);
        }
        public ActionResult BlockTransaction(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var result = false;
            string GroupId, MobileNo;
            GroupId = string.Empty;
            MobileNo = string.Empty;
            bool DisableSMSWATxn = default;
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = userDetails.GroupId;
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    DisableSMSWATxn = Convert.ToBoolean(item["DisableSMSWATxn"]);
                }

                result = ITCSR.BlockTransaction(GroupId, MobileNo, DisableSMSWATxn);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Disable Txn -" + MobileNo;
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BlockTransaction");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeBurnRule()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);
        }
        public ActionResult GetBurnRule()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objBurnData = ITCSR.GetBurnRule(userDetails.GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBurnRule(string jsonData)
        {
            tblRuleMaster objRuleMaster = new tblRuleMaster();

            bool status = false;            
            var userDetails = (CustomerLoginDetail)Session["UserSession"];            
            
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    objRuleMaster.GroupId = userDetails.GroupId;
                    objRuleMaster.BurnMinTxnAmt = Convert.ToDecimal(item["BurnMinTxnAmt"]);
                    objRuleMaster.MinRedemptionPts = Convert.ToDecimal(item["MinRedemptionPts"]);
                    objRuleMaster.MinRedemptionPtsFirstTime = Convert.ToDecimal(item["MinRedemptionPtsFirstTime"]);
                    objRuleMaster.BurnInvoiceAmtPercentage = Convert.ToDecimal(item["BurnInvoiceAmtPercentage"]);
                    objRuleMaster.BurnDBPointsPercentage = Convert.ToDecimal(item["BurnDBPointsPercentage"]);

                }

                var connectionString = CR.GetCustomerConnString(objRuleMaster.GroupId);
                var Response = ITCSR.SaveBurnRule(objRuleMaster, connectionString);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Burn Rule";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBurnRule");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ChangeEarnRule()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();            
            return View(objData);

        }
        public ActionResult GetEarnRule()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objEarndata = ITCSR.GetEarnRule(userDetails.GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveEarnRule(string jsonData)
        {
            tblRuleMaster objtblRuleMaster = new tblRuleMaster();
            bool status = false;          
            var userDetails = (CustomerLoginDetail)Session["UserSession"];                        
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    objtblRuleMaster.GroupId = userDetails.GroupId;
                    objtblRuleMaster.EarnMinTxnAmt = Convert.ToDecimal(item["EarnMinTxnAmt"]);
                    objtblRuleMaster.PointsExpiryMonths = Convert.ToInt32(item["PointsExpiryMonths"]);
                    objtblRuleMaster.PointsPercentage = Convert.ToDecimal(item["PointsPercentage"]);
                    objtblRuleMaster.PointsAllocation = Convert.ToDecimal(item["PointsAllocation"]);
                    objtblRuleMaster.Revolving = Convert.ToBoolean(item["Revolving"]);
                }
                var connectionString = CR.GetCustomerConnString(Convert.ToString(objtblRuleMaster.GroupId));
                var Response = ITCSR.SaveEarnRule(objtblRuleMaster, connectionString);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Change Earn Rule";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEarnRule");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ExtendPointsExpiry()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();                       
            return View(objData);
        }
        public ActionResult GetPointExpiryDetails(string mobileNo)
        {
            PointExpiryDummyModel objPointsExpiry = new PointExpiryDummyModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objPointsExpiry = ITCSR.GetPointExpiryDetails(userDetails.GroupId, mobileNo);
            return new JsonResult() { Data = objPointsExpiry, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ChangeRedeemptionOTP()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);
        }
        public JsonResult GetOutlet()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstOutletDetails = ITCSR.GetOutlet(userDetails.GroupId);
            return new JsonResult() { Data = lstOutletDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetDefaultOTP(string OutletId, string GroupId)
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objOTPData = ITCSR.GetDefaultOTP(OutletId, GroupId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDefaultOTP(string jsonData)
        {
            tblOutletMaster objOutletMaster = new tblOutletMaster();

            bool status = false;
            //var connectionString = CR.GetCustomerConnString(jsonData);
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string GroupId = userDetails.GroupId;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    objOutletMaster.GroupId = Convert.ToString(item["GroupId"]);
                    objOutletMaster.OutletId = Convert.ToString(item["OutletId"]);
                    objOutletMaster.DefaultOTP = Convert.ToString(item["DefaultOTP"]);
                }
                var connectionString = CR.GetCustomerConnString(objOutletMaster.GroupId);
                var Response = ITCSR.SaveDefaultOTP(objOutletMaster, connectionString);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDefaultOTP");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }    
        public ActionResult UpdateExpiryPointsDate(string mobileNo,string expiryDate)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                status = ITCSR.UpdateExpiryPointsDate(mobileNo, userDetails.GroupId, expiryDate);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Update Expiry Points Date -" + mobileNo;
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateExpiryPointsDate");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetExpiryDataByDateRange(string fromDate,string toDate)
        {
            List<PointExpiryDummyModel> lstData = new List<PointExpiryDummyModel>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            lstData = ITCSR.GetPointExpiryDateRange(userDetails.GroupId, fromDate, toDate);
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult UpdateExpiryPointsRangeDate(string fromDate, string toDate, string updateDate)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                status = ITCSR.UpdateExpiryPointsRangeDate(userDetails.GroupId, fromDate, toDate, updateDate);
                tblAuditC obj = new tblAuditC();
                obj.GroupId = Convert.ToString(userDetails.GroupId);
                obj.RequestedFor = "Update Expiry Points Range Date ";
                obj.RequestedBy = userDetails.UserName;
                obj.RequestedDate = DateTime.Now;
                ITCSR.AddCSLog(obj);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateExpiryPointsRangeDate");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetCampaignList()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var lstData = ITCSR.GetCampaignList(userDetails.GroupId);
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetCampaignDetails(string campaignName)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var objDataNew = ITCSR.GetCamaignPointExpiryDetails(userDetails.GroupId, campaignName);
            return new JsonResult() { Data = objDataNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult UpdateCampaignDetails(string campaignName,string updateDate)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            status = ITCSR.UpdateCammpaignExpiryDate(userDetails.GroupId, campaignName, updateDate);
            tblAuditC obj = new tblAuditC();
            obj.GroupId = Convert.ToString(userDetails.GroupId);
            obj.RequestedFor = "Update Campaign Details -" + campaignName;
            obj.RequestedBy = userDetails.UserName;
            obj.RequestedDate = DateTime.Now;
            ITCSR.AddCSLog(obj);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SlabWiseReport()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);
        }
        public JsonResult GetTierList(string GroupId)
        {
            var lstTierDetails = ITCSR.GetTierList(GroupId);
            return new JsonResult() { Data = lstTierDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GetSlabWiseReport(string GroupId, string Tier)
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstMember = ITCSR.GetSlabWiseReport(GroupId, Tier);
            return PartialView("_Slabwise", objData);
        }
        public ActionResult ExportToExcelSlabMemberList(string GroupId,string Tier)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                List<tblCustDetailsMaster> lstMember = new List<tblCustDetailsMaster>();
                lstMember = ITCSR.GetSlabWiseReport(GroupId, Tier);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(tblCustDetailsMaster));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (tblCustDetailsMaster item in lstMember)
                {
                    DataRow row = table.NewRow();                                  
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }

                table.Columns.Remove("Id");
                table.Columns.Remove("DOB");
                table.Columns.Remove("Email");
                table.Columns.Remove("AnniversaryDate");
                table.Columns.Remove("Category");
                table.Columns.Remove("CardNo");
                table.Columns.Remove("Gender");
                table.Columns.Remove("EnrolledBy");
                table.Columns.Remove("CountryCode");
                table.Columns.Remove("CurrentEnrolledOutlet");
                table.Columns.Remove("DisableSMSWATxn");
                table.Columns.Remove("EnrolledOutlet");
                table.Columns.Remove("DOJ");
                table.Columns.Remove("IsActive");
                table.Columns.Remove("DisableTxn");
                table.Columns.Remove("DisableSMSWAPromo");
                string ReportName = "MemberData";
                    string fileName = "BOTS_" + ReportName + ".xlsx";
                    using (XLWorkbook wb = new XLWorkbook())
                    {                       
                        table.TableName = ReportName;

                        IXLWorksheet worksheet = wb.AddWorksheet(sheetName: ReportName);
                        worksheet.Cell(1, 1).Value = "Report Name";
                        worksheet.Cell(1, 2).Value = "Member Data";
                        worksheet.Cell(2, 1).Value = "Date";
                        worksheet.Cell(2, 2).Value = DateTime.Now.ToString();
                        worksheet.Cell(3, 1).Value = "Filter";

                        worksheet.Cell(5, 1).InsertTable(table);                        
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);

                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ExportToExcelSlabMemberList");
                return null;
            }
        }
        public ActionResult ChangeDemographicDetails()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);
        }
        public ActionResult GetDemographicDetails(string GroupId,string OutletId)
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objDemographicData = ITCSR.GetDemographicDetails(GroupId, OutletId);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDemographicDetails(string jsonData)
        {
            tblGroupOwnerInfo objGroupOwnerInfo = new tblGroupOwnerInfo();
            tblOutletMaster objOutletMaster = new tblOutletMaster();
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
                    objGroupOwnerInfo.GroupId = Convert.ToString(item["GroupId"]);                    
                    objGroupOwnerInfo.MobileNo = Convert.ToString(item["MobileNo"]);
                    objGroupOwnerInfo.AlternateNo = Convert.ToString(item["AlternateNo"]);
                    objGroupOwnerInfo.Email = Convert.ToString(item["Email"]);
                    objGroupOwnerInfo.Address = Convert.ToString(item["Address"]);
                    objGroupOwnerInfo.DOB = Convert.ToDateTime(item["DOB"]);
                    objGroupOwnerInfo.DOA = Convert.ToDateTime(item["DOA"]);
                    objGroupOwnerInfo.Gender = Convert.ToString(item["Gender"]);
                    objGroupOwnerInfo.Name = Convert.ToString(item["Name"]);

                    objOutletMaster.OutletId = Convert.ToString(item["OutletId"]);
                    objOutletMaster.StoreAnniversaryDate = Convert.ToDateTime(item["StoreAnniversary"]);
                }                
                var connectionString = CR.GetCustomerConnString(objGroupOwnerInfo.GroupId);
                var Response = ITCSR.SaveDemographicDetails(objGroupOwnerInfo, objOutletMaster, connectionString);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDemographicDetails");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }

}