using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL;
using BOTS_BL.Repository;
using System.Data;
using ClosedXML.Excel;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
//using System.Data.OleDb;

namespace RetailerApp.Controllers
{
    public class DataUploadController : Controller
    {
        Exceptions newexception = new Exceptions();
        RetailerWebRepository RWR = new RetailerWebRepository();

        // GET: DataUpload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadData()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string temp;
                    temp = string.Empty;
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    HttpPostedFileBase files = Request.Files[0];
                    //string fileName = "TransactionUpload_" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    string fileName = "TransactionUpload_" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    var path = ConfigurationManager.AppSettings["FilePath"].ToString();
                    var fullFilePath = path + "\\" + fileName;
                    files.SaveAs(path + "\\" + fileName);

                    using (StreamReader sr = new StreamReader(fullFilePath))
                    {
                        string[] headers = sr.ReadLine().Split(',');
                        foreach (string header in headers)
                        {
                            dt.Columns.Add(header.Trim());
                        }
                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(',');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i].Trim();

                                //if(i == 3)
                                //{
                                //    temp = rows[i].Trim();
                                //    dr[i] = temp.Trim();
                                //}
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    int rowcount = dt.Rows.Count;

                    var ObjA = RWR.BulkTransaction(dt, userDetails.OutletOrBrandId);

                    return new JsonResult() { Data = ObjA, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UploadTransaction");
                }

            }
            return Json("File Not Uploaded Successfully!");
        }

        //public ActionResult UploadData()
        //{
        //    if (Request.Files.Count > 0)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            HttpPostedFileBase file = Request.Files[0];

        //            using (XLWorkbook workBook = new XLWorkbook(file.InputStream))
        //            {

        //                IXLWorksheet workSheet = workBook.Worksheet(1);
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("CustomerName", typeof(string));
        //                dt.Columns.Add("MobileNo", typeof(string));

        //                foreach (IXLRow row in workSheet.Rows())
        //                {
        //                    int i = 0;
        //                    DataRow toInsert = dt.NewRow();
        //                    foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
        //                    {
        //                        try
        //                        {
        //                            toInsert[i] = cell.Value.ToString();
        //                        }
        //                        catch (Exception ex)
        //                        {

        //                        }
        //                        i++;
        //                    }
        //                    dt.Rows.Add(toInsert);

        //                }
        //                if (dt.Rows.Count > 0)
        //                {
        //                    int TotalRows = 0;
        //                    int index = 0;
        //                    int invalid = 0;

        //                    //foreach (DataRow dr in dt.Rows)
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(Convert.ToString(dr["MobileNo"])))
        //                    //    {
        //                    //        Regex regex = new Regex(@"[0-9]{10}");
        //                    //        Match match = regex.Match(Convert.ToString(dr["MobileNo"]));
        //                    //        if (match.Success)
        //                    //        {
        //                    //            objtblBulkCust.MobileNo = Convert.ToString(dr["MobileNo"]);
        //                    //            objtblBulkCust.CustName = Convert.ToString(dr["CustomerName"]);
        //                    //            objtblBulkCust.EnrolledOutlet = OutletId;
        //                    //            objtblBulkCust.EnrolledDate = DateTime.Now;
        //                    //            objtblBulkCust.BonusPoints = 0;
        //                    //            objtblBulkCust.IsActive = true;
        //                    //            objtblBulkCust.ConvertedStatus = false;

        //                    //            result = NewITOPS.AddBulkCustomerData(groupId, objtblBulkCust);
        //                    //            if (result.ResponseCode == "00")
        //                    //            {
        //                    //                TotalRows++;
        //                    //            }

        //                    //            if (result.ResponseCode == "01")
        //                    //            {
        //                    //                index++;

        //                    //            }
        //                    //        }
        //                    //        else
        //                    //        {
        //                    //            invalid++;

        //                    //        }
        //                    //    }
        //                    //}
        //                    //result.ResponseSucessCount = TotalRows.ToString();
        //                    //result.ResponseFailCount = index.ToString();
        //                    ////result.ResponseInValidFormatCount = invalid.ToString();
        //                }

        //            }



        //            var ObjA = RWR.BulkTransaction(ds.Tables[0]);

        //            return new JsonResult() { Data = ObjA, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "UploadTransaction");
        //        }

        //    }
        //    return Json("File Not Uploaded Successfully!");
        //}
    }
}