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

        //public ActionResult UploadData()
        //{
        //    if (Request.Files.Count > 0)
        //    {
        //        try
        //        {
        //            string temp;
        //            temp = string.Empty;
        //            var userDetails = (CustomerLoginDetail)Session["UserSession"];
        //            DataSet ds = new DataSet();
        //            DataTable dt = new DataTable();
        //            HttpPostedFileBase files = Request.Files[0];
        //            //string fileName = "TransactionUpload_" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        //            string fileName = "TransactionUpload_" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        //            var path = ConfigurationManager.AppSettings["FilePath"].ToString();
        //            var fullFilePath = path + "\\" + fileName;
        //            files.SaveAs(path + "\\" + fileName);

        //            using (StreamReader sr = new StreamReader(fullFilePath))
        //            {
        //                string[] headers = sr.ReadLine().Split(',');
        //                foreach (string header in headers)
        //                {
        //                    dt.Columns.Add(header.Trim());
        //                }
        //                while (!sr.EndOfStream)
        //                {
        //                    string[] rows = sr.ReadLine().Split(',');
        //                    DataRow dr = dt.NewRow();
        //                    for (int i = 0; i < headers.Length; i++)
        //                    {
        //                        dr[i] = rows[i].Trim();

        //                        //if(i == 3)
        //                        //{
        //                        //    temp = rows[i].Trim();
        //                        //    dr[i] = temp.Trim();
        //                        //}
        //                    }
        //                    dt.Rows.Add(dr);
        //                }
        //            }
        //            int rowcount = dt.Rows.Count;

        //            var ObjA = RWR.BulkTransaction(dt, userDetails.OutletOrBrandId);

        //            return new JsonResult() { Data = ObjA, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "UploadTransaction");
        //        }

        //    }
        //    return Json("File Not Uploaded Successfully!");
        //}

        public ActionResult UploadData()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    RetailBulkResponse ObjA = new RetailBulkResponse();
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    SPResponse result = new SPResponse();
                    DataSet ds = new DataSet();
                    HttpPostedFileBase file = Request.Files[0];

                    using (XLWorkbook workBook = new XLWorkbook(file.InputStream))
                    {

                        IXLWorksheet workSheet = workBook.Worksheet(1);
                        workSheet.Columns("F:G,I:Y,AA:AE").Delete();
                        //workSheet.Columns(9, 25).Delete();
                        //workSheet.Columns(27, 31).Delete();

                        DataTable dt = new DataTable();
                        dt.Columns.Add("vdate", typeof(string));
                        dt.Columns.Add("ctime", typeof(string));
                        dt.Columns.Add("mode", typeof(string));
                        dt.Columns.Add("billno", typeof(string));
                        dt.Columns.Add("csname", typeof(string));
                        dt.Columns.Add("amt", typeof(string));
                        dt.Columns.Add("mobile", typeof(string));

                        foreach (IXLRow row in workSheet.Rows())
                        {
                            int i = 0;
                            DataRow toInsert = dt.NewRow();
                            foreach (IXLCell cell in row.Cells(1, 7))
                            {
                                try
                                {
                                    toInsert[i] = cell.Value.ToString();
                                }
                                catch (Exception ex)
                                {

                                }
                                i++;
                            }
                            dt.Rows.Add(toInsert);

                        }
 
                        if (dt.Rows.Count > 0)
                        {
                            ObjA = RWR.BulkTransaction(dt, userDetails.OutletOrBrandId);
                        }

                    }



                    //var ObjA = RWR.BulkTransaction(ds.Tables[0]);

                    return new JsonResult() { Data = ObjA, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UploadTransaction");
                }

            }
            return Json("File Not Uploaded Successfully!");
        }
    }
}