﻿using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class BOPromoCampaignController : Controller
    {
        PromoCampaignRepository PCR = new PromoCampaignRepository();
        Exceptions newexception = new Exceptions();
        EventsRepository ER = new EventsRepository();

        // GET: BOPromoCampaign
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            BOPromoRetailCategory Obj = new BOPromoRetailCategory();
            var WABalance = PCR.GetBOWABalance();
            ViewBag.Balance = WABalance.quota;
            var GroupCount = PCR.GetGroupCount();
            Obj.lstRetailCategory = PCR.GetRetailCategory();
            ViewBag.GroupCount = GroupCount;

            return View(Obj);
        }

        public ActionResult GroupList(string groupId)
        {
            PromoCampaignViewModel objData = new PromoCampaignViewModel();
            try
            {
                objData.lstGroupList = PCR.GetGroupDetails();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GroupList");
            }
            return View(objData);
        }

        [HttpPost]
        public JsonResult GetGroupList(string jsonData)
        {
            string Groupcount, RetailCategoryId;
            Groupcount = string.Empty;
            RetailCategoryId = string.Empty;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                   RetailCategoryId = Convert.ToString(item["RetailCategoryId"]);
                }
                Groupcount = PCR.GetGroupList(RetailCategoryId);
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetGroupList");
            }
            return new JsonResult() { Data = Groupcount, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
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

                    //var path = ConfigurationManager.AppSettings["DiscussionFileUpload"].ToString();
                    //string Path3 = ConfigurationManager.AppSettings["DiscussionDocumentURL"].ToString();

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
        public JsonResult SendTestMessage(string TestNumber,string File1,string Text1,string File2,string Text2)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JsonData Response = new JsonData();
            tblAuditBOPromo objaudit = new tblAuditBOPromo();
            try
            {
                objaudit.RequestedBy = userDetails.LoginId;
                objaudit.RequestedFor = "TestNumber : " + TestNumber + "File1 : " + File1 + " | Text1 : " + Text1 + " | File2 : " + File2 + " | Text2 : " + Text2;
                objaudit.RequestedDate = ER.IndianDatetime();
                objaudit.Type = "Test Message";
                Response = PCR.SendTestMessage(TestNumber,File1, Text1, File2, Text2, objaudit);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendTestMessage");
            }
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult SendMessage(string File1, string Text1, string File2, string Text2, string Category)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JsonData Response = new JsonData();
            tblAuditBOPromo objaudit = new tblAuditBOPromo();
            try
            {
                objaudit.RequestedBy = userDetails.LoginId;
                objaudit.RequestedFor = "File1 : " + File1 + " | Text1 : " + Text1 + " | File2 : " + File2 + " | Text2 : " + Text2 + " | Category : " + Category;
                objaudit.RequestedDate = ER.IndianDatetime();
                objaudit.Type = "Cust Message";
                Response = PCR.SendMessage(File1, Text1, File2, Text2, Category, objaudit);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendMessage");
            }
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult SendTestMessageText (string TestNumber, string Text1, string Text2)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            tblAuditBOPromo objaudit = new tblAuditBOPromo();
            JsonData Response = new JsonData();
            try
            {
                objaudit.RequestedBy = userDetails.LoginId;
                objaudit.RequestedFor = "TestNumber : "+ TestNumber  + "Text1 : " + Text1 + " | Text2 : " + Text2;
                objaudit.RequestedDate = ER.IndianDatetime();
                objaudit.Type = "Test Message";
                Response = PCR.SendTestMessage(TestNumber,Text1, Text2, objaudit);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendTestMessageText");
            }
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult SendMessageText(string Text1, string Text2, string Category)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            tblAuditBOPromo objaudit = new tblAuditBOPromo();
            JsonData Response = new JsonData();
            try
            {
                objaudit.RequestedBy = userDetails.LoginId;
                objaudit.RequestedFor = " Text1 : " + Text1 + " | Text2 : " + Text2 + " | Category : " + Category;
                objaudit.RequestedDate = ER.IndianDatetime();
                objaudit.Type = "Cust Message";

                Response = PCR.SendMessage(Text1, Text2, Category, objaudit);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendMessageText");
            }
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}