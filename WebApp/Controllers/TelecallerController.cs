﻿using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;
using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using BOTS_BL;

namespace WebApp.Controllers
{
    public class TelecallerController : Controller
    {
        TelecallerRepository TR = new TelecallerRepository();
        ReportsRepository RR = new ReportsRepository();
        Exceptions newexception = new Exceptions();
        // GET: Telecaller
        public ActionResult Index()
        {
            TelecallerCustomerData objteledata = new TelecallerCustomerData();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];            
            var lstGenderList = RR.GetGenderList(userDetails.GroupId, userDetails.connectionString);
            objteledata.lstOutlet = RR.GetOutletList(userDetails.GroupId, userDetails.connectionString);
            ViewBag.lstGenderList = lstGenderList;
            return View(objteledata);
        }
        public ActionResult Telecaller()
        {
           // TelecallerCustomerData objteledata = new TelecallerCustomerData();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            
            return View();
        }
        public JsonResult GetCustomerData(string MobileNo)
        {
            TelecallerCustomerData objteledata = new TelecallerCustomerData();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];            
            objteledata = TR.GetTelecallerCustomer(MobileNo, userDetails.GroupId, userDetails.connectionString);
            return new JsonResult() { Data = objteledata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }       
        public ActionResult SaveTelecallerData(string jsonData)
        {
            bool status = false;
            DateTime? DateofBirth = null;
            DateTime? DateOfAnni = null;
            string PointsValidity = string.Empty;
            string RedeemingOutlet = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string mobileNo = Convert.ToString(item["MobileNo"]);
                string CustomerNm = Convert.ToString(item["CustNm"]);
                string Gender = Convert.ToString(item["Gender"]);
                if (!string.IsNullOrEmpty(Convert.ToString(item["DOB"])))
                {
                    DateofBirth = Convert.ToDateTime(item["DOB"]);
                }                
                if (!string.IsNullOrEmpty(Convert.ToString(item["DOA"])))
                {
                    DateOfAnni = Convert.ToDateTime(item["DOA"]);
                }
                int PointsGiven = Convert.ToInt32(item["Points"]);
                if (userDetails.GroupId == "1263")
                {
                    PointsGiven = Convert.ToInt32(item["Points"]);
                    PointsValidity = Convert.ToString(item["PointsValidity"]);
                    RedeemingOutlet = Convert.ToString(item["RedeemingOutlet"]);

                }
                string OutletId = Convert.ToString(item["outletId"]);
                string Comments = Convert.ToString(item["Comments"]);

                status = TR.SaveTelecallerTracking(userDetails.connectionString, userDetails.LoginId, mobileNo, CustomerNm, Gender, DateofBirth, DateOfAnni, PointsGiven, OutletId, Comments, PointsValidity, RedeemingOutlet, userDetails.GroupId);
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult GetReportData(string searchData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<TelecallerReport> lsttelecaller = new List<TelecallerReport>();
            try
            {
                
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(searchData);
                foreach (Dictionary<string, object> item in objData)
                {
                    DateTime fromdt = Convert.ToDateTime(item["frmDate"]);
                    DateTime todt = Convert.ToDateTime(item["toDate"]);

                    lsttelecaller = TR.GetTelecallerReportData(fromdt, todt, userDetails.connectionString, userDetails.GroupId);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetReportData");
                return null;
            }

            return PartialView("_TelecallerList", lsttelecaller);

        }

        public ActionResult EnrollData(string jsonData)
        {
            List<JsonData> Obj = new List<JsonData>();
            bool status = false;
            DateTime? DateofBirth = null;
            DateTime? DateOfAnni = null;
            string BonusPoints = string.Empty;
            string PointsValidity = string.Empty;
            string RedeemingOutlet = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string mobileNo = Convert.ToString(item["MobileNo"]);
                string CrdNo = Convert.ToString(item["CrdNo"]);
                string CustomerNm = Convert.ToString(item["CustNm"]);
                string Gender = Convert.ToString(item["Gender"]);
                string DOB = Convert.ToString(item["DOB"]);
                string DOA = Convert.ToString(item["DOA"]);
                string Comment = Convert.ToString(item["Comment"]);
                if(userDetails.GroupId == "1263")
                {
                    BonusPoints = Convert.ToString(item["BonusPoints"]);
                    PointsValidity = Convert.ToString(item["PointsValidity"]);
                    RedeemingOutlet = Convert.ToString(item["RedeemingOutlet"]);

                }
                //if (!string.IsNullOrEmpty(Convert.ToString(item["DOB"])))
                //{
                //    DateofBirth = Convert.ToDateTime(item["DOB"]);
                //}
                //if (!string.IsNullOrEmpty(Convert.ToString(item["DOA"])))
                //{
                //    DateOfAnni = Convert.ToDateTime(item["DOA"]);
                //}
                //int PointsGiven = Convert.ToInt32(item["Points"]);
                //string OutletId = Convert.ToString(item["outletId"]);
                //string Comments = Convert.ToString(item["Comments"]);

                status = TR.SaveEnroll(userDetails.connectionString, userDetails.LoginId, userDetails.OutletOrBrandId, mobileNo, CustomerNm, CrdNo, Gender, DOB, DOA, Comment, BonusPoints, PointsValidity, RedeemingOutlet, userDetails.GroupId);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportToExcelTelecallerReport(DateTime fromdt, DateTime Todt,string ReportName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                List<TelecallerReport> lsttelecaller = new List<TelecallerReport>();
              
                lsttelecaller = TR.GetTelecallerReportData(fromdt, Todt, userDetails.connectionString, userDetails.GroupId);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(TelecallerReport));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (TelecallerReport item in lsttelecaller)
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
                    worksheet.Cell(1, 2).Value = "Telecaller Report";
                    worksheet.Cell(2, 1).Value = "Period";
                    worksheet.Cell(2, 2).Value = fromdt.ToString("dd-MMM-yyyy");
                    worksheet.Cell(2, 3).Value = "To";
                    worksheet.Cell(2, 4).Value = Todt.ToString("dd-MMM-yyyy");
                    worksheet.Cell(5, 1).InsertTable(table);
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
                newexception.AddException(ex, "ExportToExcelTelecallerReport");
                return null;
            }


        }
    }
}