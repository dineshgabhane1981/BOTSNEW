using BOTS_BL;
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
        
        // GET: BOPromoCampaign
        public ActionResult Index()
        {
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
        public JsonResult SendTestMessage(string File1,string Text1,string File2,string Text2)
        {
            JsonData Response = new JsonData();
            try
            {
                Response = PCR.SendTestMessage(File1, Text1, File2, Text2);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendTestMessage");
            }
            return new JsonResult() { Data = Response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}