using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL;
using BOTS_BL.Repository;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using BOTS_BL.Models;



namespace AudioPalace.Controllers
{
   
    public class LoginController : Controller
    {
        Exceptions newexception = new Exceptions();
        AudioPalaceRepository APR = new AudioPalaceRepository();

        // GET: Login
        public ActionResult Login()
        {
            LoginDetail objData = new LoginDetail();

            return View(objData);
        }

        public ActionResult Authenticate(LoginDetail objData)
        {
            try
            {
                LoginDetail userDetails = new LoginDetail();
                userDetails = APR.AuthenticateUser(objData);

                if (userDetails != null)
                {
                    if (userDetails.LoginId != null)
                    {
                        Session["UserSession"] = userDetails;
                        LoginDetail objLogData = new LoginDetail();
                        objLogData.LoginId = userDetails.LoginId;
                        objLogData.Password = userDetails.Password;
                        //objLogData.LoggedInTime = DateTime.Now;
                        //LR.AddLoginLog(objLogData);

                        if (!string.IsNullOrEmpty(userDetails.LoginId))
                        {
                            return RedirectToAction("TransactionUpload", "Login");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(TempData["InvalidUserMessage"])))
                        {
                            TempData["InvalidUserMessage"] = "User Does not Exist";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(TempData["InvalidUserMessage"])))
                    {
                        TempData["InvalidUserMessage"] = "User Does not Exist";
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["InvalidUserMessage"] = ex.Message;
                return RedirectToAction("Login");
            }
            return View("Login");
        }


        //public ActionResult UploadProduct()
        //{
        //    if (Request.Files.Count > 0)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            HttpPostedFileBase files = Request.Files[0];
        //            string fileName = "ProductUpload_" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        //            var path = ConfigurationManager.AppSettings["ProductFiles"].ToString();
        //            var fullFilePath = path + "\\" + fileName;
        //            files.SaveAs(path + "\\" + fileName);

        //            string conString = string.Empty;

        //            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

        //            using (OleDbConnection connExcel = new OleDbConnection(conString))
        //            {
        //                using (OleDbCommand cmdExcel = new OleDbCommand())
        //                {
        //                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
        //                    {
        //                        cmdExcel.Connection = connExcel;
        //                        //Get the name of First Sheet.
        //                        connExcel.Open();
        //                        DataTable dtExcelSchema;
        //                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //                        connExcel.Close();

        //                        //Read Data from First Sheet.
        //                        connExcel.Open();
        //                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
        //                        odaExcel.SelectCommand = cmdExcel;
        //                        odaExcel.Fill(ds);
        //                        connExcel.Close();

                                
        //                    }
                            
        //                }
        //            }                                
        //            var ObjA = APR.BulkInsert(ds.Tables[0]);

        //            return new JsonResult() { Data = ObjA, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "UploadProduct");
        //        }

        //    }
        //    return Json("File Not Uploaded Successfully!");

        //}

        public ActionResult UploadTransaction()
        {
            //AudioPalace ObjA = new AudioPalace();
            if (Request.Files.Count > 0)
            {
                try
                {
                    DataSet ds = new DataSet();
                    HttpPostedFileBase files = Request.Files[0];
                    string fileName = "TransactionUpload_" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    var path = ConfigurationManager.AppSettings["TransactionFiles"].ToString();
                    var fullFilePath = path + "\\" + fileName;
                    files.SaveAs(path + "\\" + fileName);

                    string conString = string.Empty;

                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(ds);
                                connExcel.Close();

                                //connExcel.Open();
                                //cmdExcel.CommandText = "SELECT count(*) From [" + sheetName + "]";
                                //odaExcel.SelectCommand = cmdExcel;
                                //odaExcel.Fill(ds);
                                //connExcel.Close();
                            }
                        }
                    }


                    var ObjA = APR.BulkTransaction(ds.Tables[0]);

                    return new JsonResult() { Data = ObjA, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UploadTransaction");
                }

            }
            return Json("File Not Uploaded Successfully!");

        }

        

        public ActionResult TransactionUpload()
        {
            return View();
        }

        
    }
}